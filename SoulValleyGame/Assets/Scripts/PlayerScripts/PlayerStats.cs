using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public static int playerSoulCoin = 500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SpendCoin(int spendAmount)
    {
        playerSoulCoin -= spendAmount;
    }

    public static void GainCoin(int gainAmount)
    {
        playerSoulCoin += gainAmount;
    }
}
