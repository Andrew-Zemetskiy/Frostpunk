using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResidentManagementUI : MonoBehaviour
{
    [SerializeField] private List<TMP_Dropdown> _targetObjectList;

    public void GetSelectedTargetObjects(TMP_Dropdown dropdown)
    {
        int dropdownIndex = _targetObjectList.IndexOf(dropdown);
        if (dropdownIndex != -1)
        {
            Debug.Log($"element: {dropdownIndex + 1}; with value: {dropdown.options[dropdown.value].text}");
            BaseControlSystem.Instance.SentResidentToObject(dropdownIndex, dropdown.value - 1);
        }
    }
}