using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private List<UIHandlerBase> UIHandlers;

    private void Start()
    {
        foreach (var uiHandler in UIHandlers)
        {
            uiHandler.Init();
        }
    }
}
