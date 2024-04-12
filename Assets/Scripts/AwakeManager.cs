using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class AwakeManager : MonoBehaviour
{
    [Header("Camera")]
    public float lerpTime;
    public GameObject CameraObject;
    public GameObject finalCameraPosition , startCameraPosition;
    
    [Header("Default Canvas")]
    public GameObject DefaultCanvas;
    public TextMeshProUGUI DefaultCanvasCurrency;

    [Header("Car Select Canvas")]
    public GameObject SelectCarCanvas;
    public GameObject buyButton;
    public GameObject startButton;
    public ListCar listOfCars;
    public TextMeshProUGUI currency;
    public TextMeshProUGUI carInfo;
    public GameObject rotateTurnTable;

    [Header("Map Select Canvas")]
    public GameObject SelectMapCanvas;
    [HideInInspector]public float rotateSpeed = 10f;
    [HideInInspector]public int carPointer = 0;
    private bool finalToStart,startToFinal;

    void Awake()
    {
        SelectMapCanvas.SetActive(false);
        SelectCarCanvas.SetActive(false);
        DefaultCanvas.SetActive(true);
        
        carPointer = PlayerPrefs.GetInt("pointer");
        GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
        childObject.transform.parent = rotateTurnTable.transform;
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
        if(carPointer < listOfCars.Cars.Length -1 )
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            carPointer++;
            PlayerPrefs.SetInt("pointer",carPointer);
            GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
            childObject.transform.parent = rotateTurnTable.transform;
            GetCarInfo();
        }
    }

    public void LeftButton()
    {
        if(carPointer > 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            carPointer--;
            PlayerPrefs.SetInt("pointer",carPointer);
            GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
            childObject.transform.parent = rotateTurnTable.transform;
            GetCarInfo();
        }
    }

    public void StartGameButton()
    {
        DefaultCanvas.SetActive(false);
        SelectCarCanvas.SetActive(false);
        SelectMapCanvas.SetActive(true);
    }

    public void BuyButton()
    {
        if(PlayerPrefs.GetInt("currency") >= listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carPrice)
        {
            PlayerPrefs.SetInt("currency", PlayerPrefs.GetInt("currency") - listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carPrice);

            PlayerPrefs.SetString(listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carName.ToString(),listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carName.ToString());

            GetCarInfo();
        }
    }

    public void GetCarInfo()
    {
        if(listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carName.ToString() == PlayerPrefs.GetString(listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carName.ToString()) )
        {
                carInfo.text = "Owned";

                buyButton.SetActive(false);
                currency.text = PlayerPrefs.GetInt("currency").ToString("");
                return;
        }
        currency.text = PlayerPrefs.GetInt("currency").ToString("");

        carInfo.text = listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carName.ToString() + listOfCars.Cars[PlayerPrefs.GetInt("pointer")].GetComponent<CarController>().carPrice.ToString();
            
        buyButton.SetActive(buyButton);
    }

    public void DefaultCanvasShopButton()
    {
        SelectMapCanvas.SetActive(false);
        DefaultCanvas.SetActive(false);
        SelectCarCanvas.SetActive(true);
        startToFinal = true;
        finalToStart = false;
    }

    public void BackToDefaultCanvasButton()
    {
        SelectMapCanvas.SetActive(false);
        DefaultCanvas.SetActive(true);
        SelectCarCanvas.SetActive(false);
        finalToStart = true;
        startToFinal = false;
    }

    public void loadMap1()
    {
        SceneManager.LoadScene("Map1");
    }
}
