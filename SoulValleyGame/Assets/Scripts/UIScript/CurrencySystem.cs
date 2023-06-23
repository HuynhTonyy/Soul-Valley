using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{

    public int soulCoin;

    public TextMeshProUGUI souldCoinValues;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        soulCoin = PlayerStats.playerCurrency;
        souldCoinValues.SetText("Currency: " + soulCoin);
    }
}
