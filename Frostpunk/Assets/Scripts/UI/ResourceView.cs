using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : UIHandlerBase, IInit
{
    [Header("Sprites")]
    [SerializeField] private Sprite _coalIcon;
    [SerializeField] private Sprite _woodIcon;
    [SerializeField] private Sprite _steelIcon;
    
    [Header("Images")]
    [SerializeField] private Image _coalImage;
    [SerializeField] private Image _woodImage;
    [SerializeField] private Image _steelImage;

    [Header("Amount")]
    [SerializeField] private TextMeshProUGUI _coalAmountText;
    [SerializeField] private TextMeshProUGUI _woodAmountText;
    [SerializeField] private TextMeshProUGUI _steelAmountText;
    
    public override void Init()
    {
        ResourceHandler.Instance.OnResourceChanged += UpdateData;
        SetData();
    }

    private void SetData()
    {
        _coalImage.sprite = _coalIcon;
        _woodImage.sprite = _woodIcon;
        _steelImage.sprite = _steelIcon;
        UpdateData();
    }

    private void UpdateData()
    {
        _coalAmountText.text = Mathf.Floor(ResourceHandler.Instance.CoalAmount).ToString();
        _woodAmountText.text = Mathf.Floor(ResourceHandler.Instance.WoodAmount).ToString();
        _steelAmountText.text = Mathf.Floor(ResourceHandler.Instance.SteelAmount).ToString();
    }
}
