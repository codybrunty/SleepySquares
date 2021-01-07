using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener {
    public static IAPManager instance { set; get; }

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Products
    public string swap_30 = "swap_30";
    public string swap_75 = "swap_75";
    public string swap_200 = "swap_200";

    private void Awake() {
        instance = this;
    }

    void Start() {
        if (m_StoreController == null) {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing() {
        if (IsInitialized()) {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(swap_30, ProductType.Consumable);
        builder.AddProduct(swap_75, ProductType.Consumable);
        builder.AddProduct(swap_200, ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public bool IsInitialized() {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    
    public void BuySwitches30() {
        BuyProductID(swap_30);
    }

    public void BuySwitches75() {
        BuyProductID(swap_75);
    }

    public void BuySwitches120() {
        BuyProductID(swap_200);
    }

    public string GetProductPriceFromStore(string id) {

        if (m_StoreController != null && m_StoreController.products != null) {
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;
        }
        else {
            return "";
        }
    }

    void BuyProductID(string productId) {
        if (IsInitialized()) {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error) {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
        if (String.Equals(args.purchasedProduct.definition.id, swap_30, StringComparison.Ordinal)) {
            Debug.Log("purchase 30 switches");
            FindObjectOfType<SettingsMenu>().ExitSettings();
            FindObjectOfType<SwitchButton>().AddSwitches(30);
            SoundManager.SM.PlayOneShotSound("yahoo");

            //for playfab tracking
            int counter = PlayerPrefs.GetInt("Purchase_30", 0);
            counter++;
            PlayerPrefs.SetInt("Purchase_30", counter);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, swap_75, StringComparison.Ordinal)) {
            Debug.Log("purchase 75 switches");
            FindObjectOfType<SettingsMenu>().ExitSettings();
            FindObjectOfType<SwitchButton>().AddSwitches(75);
            SoundManager.SM.PlayOneShotSound("yahoo");

            //for playfab tracking
            int counter = PlayerPrefs.GetInt("Purchase_75", 0);
            counter++;
            PlayerPrefs.SetInt("Purchase_75", counter);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, swap_200, StringComparison.Ordinal)) {
            Debug.Log("purchase 200 switches");
            FindObjectOfType<SettingsMenu>().ExitSettings();
            FindObjectOfType<SwitchButton>().AddSwitches(200);
            SoundManager.SM.PlayOneShotSound("yahoo");

            //for playfab tracking
            int counter = PlayerPrefs.GetInt("Purchase_200", 0);
            counter++;
            PlayerPrefs.SetInt("Purchase_200", counter);
        }
        else {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}