using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainSceneManager : MonoBehaviour
{
    [Header("Camera")]
    public float lerpTime;
    public GameObject CameraObject;
    public GameObject finalCameraPosition , startCameraPosition;
    
    [Header("Default Canvas")]
    public GameObject DefaultCanvas;
    public TextMeshProUGUI DefaultCanvasCurrency;
    public GameObject startGameButton;

    [Header("Car Select Canvas")]
    public GameObject SelectCarCanvas;
    public GameObject buyButton;
    public GameObject lockButton;
    public Image lockImage;
    public Image unlockImage;
    public GameObject rightButton;
    public GameObject leftButton;
    public ListCar listOfCars;
    public TextMeshProUGUI carSelectCurrency;
    public TextMeshProUGUI carInfo;
    public GameObject rotateTurnTable;

    [Header("Settings Canvas")]
    public GameObject SettingsCanvas;


    private bool finalToStart,startToFinal;
    [HideInInspector] public float rotateSpeed = 10f;
    [HideInInspector] public SaveLoadManager saveLoadManager;

    //PlayerPref
    [HideInInspector] public int carPointer;
    [HideInInspector] public int choosePointer;
    [HideInInspector] public int coin;

    void Awake()
    {
        PlayerPrefs.DeleteAll();

        SelectCarCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        DefaultCanvas.SetActive(true);

        //load data
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager == null)
        {
            Debug.LogError("Không tìm thấy đối tượng SaveLoadManager trong Scene!");
        }

        //saveLoadManager.LoadData();

        coin = saveLoadManager.playerData.coin;
        choosePointer = saveLoadManager.playerData.lastChoose;
        carPointer = choosePointer;
        InitOwnedCarData();

        //Save Data vào PlayerPref
        PlayerPrefs.SetInt("currency", coin);
        PlayerPrefs.SetInt("cPointer", choosePointer);
        PlayerPrefs.SetInt("carPointer", carPointer);

        //hiển thị giao diện
        DefaultCanvasCurrency.text = PlayerPrefs.GetInt("currency").ToString();
        GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
        childObject.transform.parent = rotateTurnTable.transform;

    }
    public void InitOwnedCarData()
    {
        for (int i = 0; i < saveLoadManager.playerData.OwnedCar.Length; i++)
        {
            PlayerPrefs.SetInt("Car_" + i, saveLoadManager.playerData.OwnedCar[i]);
        }
    }

    public void SaveOwnedCarData()
    {
        for (int i = 0; i < saveLoadManager.playerData.OwnedCar.Length; i++)
        {
            saveLoadManager.playerData.OwnedCar[i] = PlayerPrefs.GetInt("Car_" + i);
        }
    }

    private void FixedUpdate() 
    {
        rotateTurnTable.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        CameraTranzition();
    }

    public void CameraTranzition()
    {
        if(startToFinal)
        {
            CameraObject.transform.position = Vector3.Lerp(CameraObject.transform.position,finalCameraPosition.transform.position,lerpTime * Time.deltaTime); 
        }
        if(finalToStart)
        {
            CameraObject.transform.position = Vector3.Lerp(CameraObject.transform.position,startCameraPosition.transform.position,lerpTime * Time.deltaTime); 
        }
    }

    public void RightButton()
    {
        if(carPointer < listOfCars.Cars.Length - 1)
        {
            leftButton.SetActive(true);
            if(carPointer < listOfCars.Cars.Length - 2)
            {
                rightButton.SetActive(true);
            }
            else
            {
                rightButton.SetActive(false);
            }
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            carPointer++;
            PlayerPrefs.SetInt("carPointer",carPointer);
            GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
            childObject.transform.parent = rotateTurnTable.transform;
            GetCarInfo();
        }
    }

    public void LeftButton()
    {
        if(carPointer > 0)
        {
            rightButton.SetActive(true);
            if(carPointer > 1)
            {
                leftButton.SetActive(true);
            }
            else 
            {
                leftButton.SetActive(false);
            }
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            carPointer--;
            PlayerPrefs.SetInt("carPointer",carPointer);
            GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
            childObject.transform.parent = rotateTurnTable.transform;
            GetCarInfo();
        }
    }
    public void BuyButton()
    {
        int carIndex = PlayerPrefs.GetInt("carPointer");
        CarController CC = listOfCars.Cars[carIndex].GetComponent<CarController>();
        int currentCoin = PlayerPrefs.GetInt("currency");

        if(currentCoin >= CC.carPrice)
        {
            currentCoin -= CC.carPrice;

            PlayerPrefs.SetInt("currency", currentCoin);

            PlayerPrefs.SetInt("Car_" + carIndex, 1);

            GetCarInfo();
        }
    }

    public void GetCarInfo()
    {
        int carIndex = PlayerPrefs.GetInt("carPointer");
        CarController CC = listOfCars.Cars[carIndex].GetComponent<CarController>();

        carSelectCurrency.text = PlayerPrefs.GetInt("currency").ToString();

        if (PlayerPrefs.GetInt("Car_" + carIndex) == 1)
        {
            carInfo.text = "Owned";
            buyButton.SetActive(false);

            int chooseIndex = PlayerPrefs.GetInt("cPointer");
            lockButton.SetActive(true);
            if(chooseIndex == carIndex)
            {
                lockImage.gameObject.SetActive(true);
                unlockImage.gameObject.SetActive(false);
            }
            else 
            {
                lockImage.gameObject.SetActive(false);
                unlockImage.gameObject.SetActive(true);
            }
            
        }
        else
        {
            carInfo.text = CC.carName + " " + CC.carPrice;
            buyButton.SetActive(true);
            lockButton.SetActive(false);
        }
    }

    public void DefaultCanvasShopButton()
    {
        DefaultCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        SelectCarCanvas.SetActive(true);
        startToFinal = true;
        finalToStart = false;
        
        carSelectCurrency.text = saveLoadManager.playerData.coin.ToString();

        InitOwnedCarData();

        if(carPointer == 0)
        {
            leftButton.SetActive(false);
        }
        if(carPointer == listOfCars.Cars.Length - 1)
        {
            rightButton.SetActive(false);
        }

        GetCarInfo();
    }

    public void BackToDefaultCanvasButton()
    {
        DefaultCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
        SelectCarCanvas.SetActive(false);
        finalToStart = true;
        startToFinal = false;

        int chooseIndex = PlayerPrefs.GetInt("cPointer");

        if(carPointer != chooseIndex)
        {
            carPointer = chooseIndex;
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            PlayerPrefs.SetInt("carPointer",carPointer);
            GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
            childObject.transform.parent = rotateTurnTable.transform;
        }

        saveLoadManager.playerData.coin = PlayerPrefs.GetInt("currency");
        saveLoadManager.playerData.lastChoose = PlayerPrefs.GetInt("cPointer");
        SaveOwnedCarData();

        DefaultCanvasCurrency.text = saveLoadManager.playerData.coin.ToString();
        saveLoadManager.SaveData();
    }

    public void LockButton()
    {
        bool isUnlockActive = unlockImage.gameObject.activeSelf;
        if(isUnlockActive)
        {
            unlockImage.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(true);
            PlayerPrefs.SetInt("cPointer" , carPointer);
        }
    }
    
    public void StartGameButton()
    {
        saveLoadManager.playerData.coin = PlayerPrefs.GetInt("currency");
        saveLoadManager.playerData.lastChoose = PlayerPrefs.GetInt("cPointer");
        SaveOwnedCarData();
        SceneManager.LoadScene("MapSelectScene");
    }

    public void QuitButton()
    {
        SettingsCanvas.SetActive(false);
    }
    public void SettingsButton()
    {
        SettingsCanvas.SetActive(true);
    }
    public void LogoutButton()
    {
        SceneManager.LoadScene("AwakeScene");
    }
}
