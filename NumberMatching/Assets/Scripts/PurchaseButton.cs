using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseButton : MonoBehaviour{

    public enum PurchaseType { swap_30, swap_75, swap_200 };
    public PurchaseType purchaseType;
    [SerializeField] TextMeshProUGUI priceText=default;
    [SerializeField] TextMeshProUGUI ammountText = default;
    [SerializeField] Image dc = default;
    [SerializeField] Color activeColor = default;
    [SerializeField] Color deactiveColor = default;

    [SerializeField] Button mainButton = default;
    [SerializeField] Image mainButtonImage = default;
    [SerializeField] GameObject mainText = default;

    private void Awake() {
        UpdateSwitchAmmountDisplay();
        //StartCoroutine(LoadPrice());
    }

    private void UpdateSwitchAmmountDisplay() {
        switch (purchaseType) {
            case PurchaseType.swap_30:
                ammountText.text = "30";
                break;
            case PurchaseType.swap_75:
                ammountText.text = "75";
                break;
            case PurchaseType.swap_200:
                ammountText.text = "200";
                break;
        }
    }

    private IEnumerator LoadPrice() {
        while (!IAPManager.instance.IsInitialized()) {
            yield return null;
        }
        string loadedPrice = "";

        switch (purchaseType) {
            case PurchaseType.swap_30:
                loadedPrice = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.swap_30);
                break;
            case PurchaseType.swap_75:
                loadedPrice = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.swap_75);
                break;
            case PurchaseType.swap_200:
                loadedPrice = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.swap_200);
                break;
        }
        priceText.text = loadedPrice;
    }

    public void ButtonActiveMode() {
        dc.gameObject.SetActive(false);
        mainButton.interactable = true;
        mainButtonImage.color = activeColor;
        mainText.SetActive(true);
        StartCoroutine(LoadPrice());
    }

    public void ButtonDeactiveMode() {
        dc.gameObject.SetActive(true);
        mainButton.interactable = false;
        mainButtonImage.color = deactiveColor;
        mainText.SetActive(false);
    }

    public void PurchaseButtonOnClick() {
        switch (purchaseType) {
            case PurchaseType.swap_30:
                IAPManager.instance.BuySwitches30();
                break;
            case PurchaseType.swap_75:
                IAPManager.instance.BuySwitches75();
                break;
            case PurchaseType.swap_200:
                IAPManager.instance.BuySwitches120();
                break;
        }
    }

}
