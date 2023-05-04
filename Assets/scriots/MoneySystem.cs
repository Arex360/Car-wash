using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    public static MoneySystem instance;
    public TMPro.TextMeshProUGUI moneyText;
    [SerializeField] int _amount;
    public int ammount { get { return _amount; } set { PlayerPrefs.SetInt("cash",value); _amount = value; moneyText.text = _amount.ToString()+"$"; } }
    void Start()
    {
        instance = this;
        ammount = PlayerPrefs.GetInt("cash", 2000);
    }
    public void Deduct(int ammount)
    {
        this.ammount -= ammount;
    }
    public void Increament(int amount)
    {
        this.ammount += ammount;
    }
}
