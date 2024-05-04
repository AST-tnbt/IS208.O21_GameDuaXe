using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;

public class MapSelectManager : MonoBehaviour
{

    [Header("Car Select Canvas")]
    public GameObject selectMapCanvas;
    public GameObject betMenu;
    public GameObject errorCanvas;
    public TextMeshProUGUI errorText;
    public GameObject rightButton;
    public GameObject leftButton;
    public ListMap listOfMap;
    public TextMeshProUGUI nameOfMap;
    public GameObject rotateTurnTable;
    public TextMeshProUGUI currency;
    public TMP_InputField inputField;

    [Header("SFX")]
    [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
    public AudioSource hoverSound;

    [HideInInspector] public float rotateSpeed = 10f;
    [HideInInspector] public int mapPointer = 0;

    void Awake()
    {
        betMenu.SetActive(false);
        errorCanvas.SetActive(false);
        selectMapCanvas.SetActive(true);
        currency.text = PlayerPrefs.GetInt("currency").ToString();
        mapPointer = PlayerPrefs.GetInt("mp");
        GameObject childObject = Instantiate(listOfMap.Maps[mapPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
        childObject.transform.parent = rotateTurnTable.transform;
        if(mapPointer == 0)
        {
            leftButton.SetActive(false);
        }
        if(mapPointer == listOfMap.Maps.Length - 1)
        {
            rightButton.SetActive(false);
        }
        GetMapInfo();
    }

    private void FixedUpdate() 
    {
        rotateTurnTable.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void RightButtonClicked()
    {
        betMenu.SetActive(false);
        if(mapPointer < listOfMap.Maps.Length - 1)
        {
            leftButton.SetActive(true);
            if(mapPointer < listOfMap.Maps.Length - 2)
            {
                rightButton.SetActive(true);
            }
            else
            {
                rightButton.SetActive(false);
            }
            Destroy(GameObject.FindGameObjectWithTag("Map"));
            mapPointer++;
            PlayerPrefs.SetInt("mp",mapPointer);
            GameObject childObject = Instantiate(listOfMap.Maps[mapPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
            childObject.transform.parent = rotateTurnTable.transform;
            GetMapInfo();
        }
    }

    public void LeftButtonClicked()
    {
        betMenu.SetActive(false);
        if(mapPointer > 0)
        {
            rightButton.SetActive(true);
            if(mapPointer > 1)
            {
                leftButton.SetActive(true);
            }
            else 
            {
                leftButton.SetActive(false);
            }
            Destroy(GameObject.FindGameObjectWithTag("Map"));
            mapPointer--;
            PlayerPrefs.SetInt("mp",mapPointer);
            GameObject childObject = Instantiate(listOfMap.Maps[mapPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
            childObject.transform.parent = rotateTurnTable.transform;
            GetMapInfo();
        }
    }
    public void GetMapInfo()
    {
        int mapIndex = PlayerPrefs.GetInt("mp");
        MapModifier MM = listOfMap.Maps[mapIndex].GetComponent<MapModifier>();

        nameOfMap.text = MM.mapName;
    }

    public void StartGameButtonClicked()
    {
        betMenu.SetActive(true);
    }
    public void BackButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void BetPlayButtonClicked()
    {
        int coin;
        try
        {
            string inputValue = inputField.text;
            coin = int.Parse(inputValue);
        }
        catch (FormatException)
        {
            ShowErrorMessage("Invalid input value");
            Invoke("CloseErrorMessage",1.5f);
            return;
        }
        int currentCoin = PlayerPrefs.GetInt("currency");
        if(coin > currentCoin)
        {
            ShowErrorMessage("Not enough currency");
            Invoke("CloseErrorMessage",1.5f);
        }
        else
        {
            PlayerPrefs.SetInt("betcoin", coin);
            int mapIndex = PlayerPrefs.GetInt("mp");
            MapModifier MM = listOfMap.Maps[mapIndex].GetComponent<MapModifier>();
            SceneManager.LoadScene(MM.mapName);
        }
    }

    public void BetBackButtonClicked()
    {
        betMenu.SetActive(false);
    }
    public void PlayHover()
    {
		hoverSound.Play();
	}

    private void ShowErrorMessage(string message)
    {
        errorText.text = "" + message;
        errorCanvas.SetActive(true);
    }
    
    public void CloseErrorMessage()
    {
        errorText.text = "";
        errorCanvas.SetActive(false);
    }
}
