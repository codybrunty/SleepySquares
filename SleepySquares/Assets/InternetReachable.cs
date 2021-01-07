using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetReachable : MonoBehaviour
{

    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] List<GameObject> adbuttons = new List<GameObject>();
    [SerializeField] GameObject errorMessage = default;

    private void OnEnable() {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            //no internet
            DisableAllButtons();
        }
        else {
            //yes internet
            EnableAllButtons();
        }
    }

    private void EnableAllButtons() {
        foreach (GameObject button in buttons) {
            button.GetComponent<PurchaseButton>().ButtonActiveMode();
        }
        foreach (GameObject ab in adbuttons) {
            ab.GetComponent<AdManager>().AdButtonActive();
        }
        errorMessage.SetActive(false);
    }

    private void DisableAllButtons() {
        foreach (GameObject button in buttons) {
            button.GetComponent<PurchaseButton>().ButtonDeactiveMode();
        }
        foreach (GameObject ab in adbuttons) {
            ab.GetComponent<AdManager>().AdButtonDeactive();
        }
        errorMessage.SetActive(true);
    }

}
