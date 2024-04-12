using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarModifier : MonoBehaviour
{
    //public CarController carControllerRef;

    private void Start()
    {

        //updateValues();
        PlayerPrefs.SetInt("currency", 99999);
        //carControllerRef = GetComponent<CarController>();
        /*
        PlayerPrefs.SetInt((controllerRef.carName + "engineUpgrade").ToString(), 0);
        PlayerPrefs.SetInt((controllerRef.carName + "colorUpgrade").ToString(), 0);
        PlayerPrefs.SetInt((controllerRef.carName + "handlingUpgrade").ToString(), 0);
        PlayerPrefs.SetInt((controllerRef.carName + "nitrusUpgrade").ToString(), 0);
        PlayerPrefs.SetInt((controllerRef.carName + "wheelUpgrade").ToString(), 0);
        PlayerPrefs.SetInt((controllerRef.carName + "spoilerUpgrade").ToString(), 0);
        */
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene") return;
        //updateValues();
    }
}
