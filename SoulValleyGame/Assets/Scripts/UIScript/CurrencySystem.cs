using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{

    public int soulCoin;

    public TextMeshProUGUI souldCoinValues;

    void Update()
    {
        soulCoin = PlayerStats.gold;
        souldCoinValues.SetText("Currency: " + soulCoin);
    }
}
