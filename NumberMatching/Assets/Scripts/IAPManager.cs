using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener {
    public static IAPManager instance { set; get; }

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Products
    public string switches30 = "switches30";
    public string switches75 = "switches75";
    public string switches120 = "switches120";

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
        builder.AddProduct(switches30, ProductType.Consumable);
        builder.AddProduct(switches75, ProductType.Consumable);
        builder.AddProduct(switches120, ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public bool IsInitialized() {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    
    public void BuySwitches30() {
        BuyProductID(switches30);
    }

    public void BuySwitches75() {
        BuyProductID(switches75);
    }

    public void BuySwitches120() {
        BuyProductID(switches120);
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
        if (String.Equals(args.purchasedProduct.definition.id, switches30, StringComparison.Ordinal)) {
            Debug.Log("purchase 30 switches");
            FindObjectOfType<ExitPanels>().ExitOnClick();
            FindObjectOfType<SwitchButton>().AddSwitches(30);
            FindObjectOfType<SoundManager>().PlayOneShotSound("yahoo");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, switches75, StringComparison.Ordinal)) {
            Debug.Log("purchase 75 switches");
            FindObjectOfType<ExitPanels>().ExitOnClick();
            FindObjectOfType<SwitchButton>().AddSwitches(75);
            FindObjectOfType<SoundManager>().PlayOneShotSound("yahoo");
        }
        else if (String.Equals(args.purchasedProduct.definition.id, switches120, StringComparison.Ordinal)) {
            Debug.Log("purchase 120 switches");
            FindObjectOfType<ExitPanels>().ExitOnClick();
            FindObjectOfType<SwitchButton>().AddSwitches(120);
            FindObjectOfType<SoundManager>().PlayOneShotSound("yahoo");
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