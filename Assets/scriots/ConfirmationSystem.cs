using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationSystem : MonoBehaviour
{
    public static ConfirmationSystem instance;
    public GameObject body;
    public TMPro.TextMeshProUGUI title;
    public TMPro.TextMeshProUGUI priceText;
    public UnityEngine.UI.Button acceptButton;
    public int currentOffer;
    void Start()
    {
        instance = this;
        Hide();
    }

    public void Hide() => body.SetActive(false);
    public void ShowConfimation(string title,int price,System.Action onAccept)
    {
        currentOffer = price;
        this.title.text = title;
        priceText.text = price.ToString();
        if (MoneySystem.instance.ammount < price) acceptButton.interactable = false;
        body.SetActive(true);
        acceptButton.onClick.AddListener(onAccept.Invoke);
    }
    public void OnAccept()
    {
        MoneySystem.instance.Deduct(currentOffer);
        Hide();
    }
}
