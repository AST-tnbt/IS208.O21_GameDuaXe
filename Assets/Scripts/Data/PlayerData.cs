using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coin = 0;

    public int lastChoose = 0;
    public int[] OwnedCar = new int[4];

    public PlayerData()
    {
        coin = 5000;
        lastChoose = 0;
        OwnedCar[0] = 1;
        for (int i = 1; i < 4; i++)
        {
            OwnedCar[i] = 0;
        }
    }
}
