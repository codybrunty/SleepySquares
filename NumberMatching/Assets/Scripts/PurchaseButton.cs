using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PurchaseButton : MonoBehaviour{

    public enum PurchaseType { switches30, switches75, switches120};
    public PurchaseType purchaseType;
    [SerializeField] TextMeshProUGUI priceText=default;
    [SerializeField] TextMeshProUGUI ammountText=default;

    private void Start() {
        UpdateSwitchAmmountDisplay();
        StartCoroutine(LoadPrice());
    }

    private void UpdateSwitchAmmountDisplay() {
        switch (purchaseType) {
            case PurchaseType.switches30:
                ammountText.text = "30";
                break;
            case PurchaseType.switches75:
                ammountText.text = "75";
                break;
            case PurchaseType.switches120:
                ammountText.text = "120";
                break;
        }
    }

    private IEnumerator LoadPrice() {
        while (!IAPManager.instance.IsInitialized()) {
            yield return null;
        }
        string loadedPrice = "";

        switch (purchaseType) {
            case PurchaseType.switches30:
                loadedPrice = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.switches30);
                break;
            case PurchaseType.switches75:
                loadedPrice = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.switches75);
                break;
            case PurchaseType.switches120:
                loadedPrice = IAPManager.instance.GetProductPriceFromStore(IAPManager.instance.switches120);
                break;
        }
        priceText.text = loadedPrice;
    }

    public void PurchaseButtonOnClick() {
        switch (purchaseType) {
            case PurchaseType.switches30:
                IAPManager.instance.BuySwitches30();
                break;
            case PurchaseType.switches75:
                IAPManager.instance.BuySwitches75();
                break;
            case PurchaseType.switches120:
                IAPManager.instance.BuySwitches120();
                break;
        }
    }

}
