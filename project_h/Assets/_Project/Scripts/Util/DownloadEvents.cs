using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DownloadProgressStatus
{
    public long DownloadedBytes;// 다운로드된 바이트 사이즈 
    public long TotalBytes;     // 다운로드 받을 전체 사이즈 
    public long RemainingBytes;  // 남은 바이트 사이즈 
    public float TotalProgress; // 전체 진행률 0 ~ 1 

    public DownloadProgressStatus(long downloadedBytes, long totalBytes, long remainedBytes, float totalProgress)
    {
        DownloadedBytes = downloadedBytes;
        TotalBytes = totalBytes;
        RemainingBytes = remainedBytes;
        TotalProgress = totalProgress;
    }
}

public class DownloadEvents
{
    public event Action OnInitialized;
    public event Action OnCatalogUpdated;
    public event Action<long> OnSizeDownloaded;
    public event Action<DownloadProgressStatus> OnProgressUpdated;
    public event Action<bool> OnFinished;

    public void NotifyInitialized() => OnInitialized?.Invoke();
    public void NotifyCatalogUpdated() => OnCatalogUpdated?.Invoke();
    public void NotifySizeDownloaded(long size) => OnSizeDownloaded?.Invoke(size);
    public void NotifyDownloadProgress(DownloadProgressStatus status) => OnProgressUpdated?.Invoke(status);
    public void NotifyDownloadFinished(bool isSuccess) => OnFinished?.Invoke(isSuccess);
}
