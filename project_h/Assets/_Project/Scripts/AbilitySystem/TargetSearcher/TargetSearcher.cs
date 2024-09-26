using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class TargetSearcher
{
    public delegate void SelectionCompletedHandler(TargetSearcher targetSearcher, TargetSelectionResult result);

    // Target 검색 기준점을 찾는 Module
    [Header("Select Action")]
    [SerializeReference]
    private TargetSelectionAction selectionAction;

}