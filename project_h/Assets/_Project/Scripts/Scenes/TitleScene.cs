using System;
using System.Collections;
using UnityEngine;
using static Util;
using Object = UnityEngine.Object;

public enum ELoginSceneState
{
    None = 0,
    CalculatingSize,
    NothingToDownload,
    AskingDownload,
    Downloading,
    DownloadFinished,
    ResourceLoadFinished,
}

public class TitleScene : BaseScene
{
    public delegate void DownloadStateDelegate(string downloadInfoText);
    public DownloadStateDelegate OnDownloadStateStateChanged;
    public Action OnDownloadEnd;

    Downloader _downloader;
    DownloadProgressStatus _prevProgress;

    private ELoginSceneState _loginSceneState = ELoginSceneState.None;
    public ELoginSceneState LoginSceneState
    {
        get => _loginSceneState;
        set
        {
            if (_loginSceneState != value)
            {
                _loginSceneState = value;
                if (_loginSceneState == ELoginSceneState.ResourceLoadFinished)
                    OnDownloadEnd?.Invoke();
            }
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
    
        SceneType = EScene.TitleScene;
        Managers.Scene.SetCurrentScene(this);

        _downloader = gameObject.GetOrAddComponent<Downloader>();
        _downloader.DownloadLabel = "PreGame";

        Managers.Resource.LoadAllAsync<Object>("PreTitle", (key, current, total) =>
        {
            if (current == total)
            {
                Managers.UI.ShowSceneUI<UI_TitleScene>();
                StartCoroutine(StartDownload());
            }
        });

        return true;
    }

    private IEnumerator StartDownload()
    {
        yield return _downloader.StartDownload((events) =>
        {
            events.OnInitialized += OnInitialized;
            events.OnCatalogUpdated += OnCatalogUpdated;
            events.OnSizeDownloaded += OnSizeDownloaded;
            events.OnProgressUpdated += OnProgress;
            events.OnFinished += OnFinished;
        });

    }
    void OnInitialized()
    {
        _downloader.GoNext();
    }

    void OnCatalogUpdated()
    {
        _downloader.GoNext();
    }

    private void OnSizeDownloaded(long size)
    {
        Debug.Log($"다운로드 완료 ! : {Util.GetConvertedByteString(size, ESizeUnits.KB)} ({size}바이트)");

        OnDownloadStateStateChanged?.Invoke($"다운로드 완료 ! : {Util.GetConvertedByteString(size, ESizeUnits.KB)} ({size}바이트)");

        if (size == 0)
        {
            LoginSceneState = ELoginSceneState.DownloadFinished;
            // Load 시작
            Managers.Resource.LoadAllAsync<Object>(_downloader.DownloadLabel, (key, count, totalCount) =>
            {
                string text;
                if (count != totalCount)
                {
                    text = $"Loading... {key} {count}/{totalCount}";
                }
                else
                {
                    text = "Loading Completed";
                    LoginSceneState = ELoginSceneState.ResourceLoadFinished;
                }

                OnDownloadStateStateChanged?.Invoke(text);
            });
        }
        else
        {
            // 용량 큰 컨텐츠라면
            var sizeUnit = Util.GetProperByteUnit(size);
            var totalSizeUnit = Util.ConvertByteByUnit(size, sizeUnit);

            // 다운로드할지 물어봄
            LoginSceneState = ELoginSceneState.AskingDownload;
            Managers.UI.ShowPopupUI<UI_MessagePopup>().SetInfo($"Wifi 환경이 아니라면 데이터가 많이 소모될 수 있습니다. 다운로드 하시겠습니까? <color=green>({$"{totalSizeUnit}{sizeUnit})</color>"}", showConfirmButton: true, callback: (result) =>
            {
                if (result)
                {
                    // Confirm
                    LoginSceneState = ELoginSceneState.Downloading;
                    _downloader.GoNext();
                }


            });

        }
    }

    private void OnProgress(DownloadProgressStatus progress)
    {
        if (_prevProgress.DownloadedBytes == progress.DownloadedBytes)
            return;

        _prevProgress = progress;

        string text = $"다운로드 중... {(progress.TotalProgress * 100).ToString("0.00")}% 완료";
    }

    private void OnFinished(bool isSuccess)
    {
        LoginSceneState = ELoginSceneState.DownloadFinished;
        _downloader.GoNext();
    }

    public override void Clear()
    {
        
    }
}