using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class PriceIAP : MonoBehaviour
{
    [Header("Element")]
    public CodelessIAPButton iapButton;
    public TMP_Text priceText;

    private void OnEnable()
    {
        UpdatePriceText();
    }

    public void UpdatePriceText()
    {
        if (priceText != null && iapButton != null)
        {
            Product product = CodelessIAPStoreListener.Instance.GetProduct(iapButton.productId);
            if (product != null)
            {
                priceText.SetText(product.metadata.localizedPriceString);
            }
        }
    }
}
