using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
public class UIMenuManager : MonoBehaviour
{
    [Header("Camera")]
    public GameObject cameraObject;
    public float lerpTime;
    public GameObject finalCameraPosition , startCameraPosition;

    [Header("MENUS")]
    [Tooltip("The Menu for when the MAIN menu buttons")]
    public GameObject mainCanvas;

    [Tooltip("The Canvas for when the Shop button is clicked")]
    public GameObject shopCanvas;

    [Tooltip("Setting window")]
    public GameObject settingsCanvas;

    [Tooltip("The list of buttons")]
    public GameObject mainMenu;

    [Tooltip("The Menu for when the PLAY button is clicked")]
    public GameObject playMenu;

    [Tooltip("The Menu for when the EXIT button is clicked")]
    public GameObject exitMenu;

    [Header("PANELS")]
    [Tooltip("The UI Panel that holds the CONTROLS window tab")]
    public GameObject PanelControls;

    [Tooltip("The UI Panel that holds the GAME window tab")]
    public GameObject PanelGame;
    
    [Header("SETTINGS SCREEN")]
    [Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
    public GameObject lineGame;

    [Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
    public GameObject lineControls;

    public GameObject lineNormal;
    public GameObject lineHardcore;

	[Header("SFX")]

    [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
    public AudioSource hoverSound;
    [Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
    public AudioSource sliderSound;
    [Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
    public AudioSource swooshSound;

    [Header("OTHERS")]

    public GameObject buyButton;
    public GameObject lockButton;
    public GameObject leftButton;
    public GameObject rightButton;
    public TextMeshProUGUI mainCurrency;
    public TextMeshProUGUI shopCurrency;
    public TextMeshProUGUI carInfo;
    public ListCar listOfCars;
    public GameObject rotateTurnTable;
    private bool finalToStart,startToFinal;
    public Image lockImage, unlockImage;
    [HideInInspector] public float rotateSpeed = 10f;
    [HideInInspector] public SaveLoadManager saveLoadManager;

    //PlayerPref
    [HideInInspector] public int carPointer;
    [HideInInspector] public int choosePointer;
    [HideInInspector] public int coin;

    void Awake()
    {
        //Set active 
        mainCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
		exitMenu.SetActive(false);


        //load data
        PlayerPrefs.DeleteAll();
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        if (saveLoadManager == null)
        {
            Debug.LogError("Không tìm thấy đối tượng SaveLoadManager trong Scene!");
        }

        saveLoadManager.LoadData();

        coin = saveLoadManager.playerData.coin;
        choosePointer = saveLoadManager.playerData.lastChoose;
        carPointer = choosePointer;
        InitOwnedCarData();

        //Save Data vào PlayerPref
        PlayerPrefs.SetInt("currency", coin);
        PlayerPrefs.SetInt("cPointer", choosePointer);
        PlayerPrefs.SetInt("carPointer", carPointer);

        //hiển thị giao diện
        mainCurrency.text = PlayerPrefs.GetInt("currency").ToString();
        GameObject childObject = Instantiate(listOfCars.Cars[carPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
        childObject.transform.parent = rotateTurnTable.transform;

        childObject.GetComponent<CarController>().enabled = false;
        if(childObject.GetComponent<CarController>().carEngineSound != null)
        {
            childObject.GetComponent<CarController>().carEngineSound.Stop();
        }
        if(childObject.GetComponent<CarController>().tireScreechSound != null)
        {
            childObject.GetComponent<CarController>().tireScreechSound.Stop();
        }
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
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position,finalCameraPosition.transform.position,lerpTime * Time.deltaTime); 
        }
        if(finalToStart)
        {
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position,startCameraPosition.transform.position,lerpTime * Time.deltaTime); 
        }
    }

    //Setting button

    //main canvas

    public void  DisableMain()
    {
        mainMenu.SetActive(false);
		playMenu.SetActive(false);
        exitMenu.SetActive(false);
	}
    public void PlayButtonClicked()
    {
        exitMenu.SetActive(false);
        if(!playMenu.activeSelf)
		{
            playMenu.SetActive(true);
        }
        else 
        {
            playMenu.SetActive(false);
        }
	}

    public void ShopButtonClicked()
    {
        mainCanvas.SetActive(false);
        playMenu.SetActive(false);
        exitMenu.SetActive(false);
        shopCanvas.SetActive(true);
        startToFinal = true;
        finalToStart = false;
        
        shopCurrency.text = saveLoadManager.playerData.coin.ToString();

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
    public void SettingButtonClicked()
    {
        mainCanvas.SetActive(false);
        playMenu.SetActive(false);
        exitMenu.SetActive(false);
        settingsCanvas.SetActive(true);
        PanelGame.SetActive(true);
        lineGame.SetActive(true);
        PanelControls.SetActive(false);
        lineControls.SetActive(false);
        //test
        lineNormal.SetActive(true);
        lineHardcore.SetActive(false);
    }

    public void ExitButtonClicked()
    {
        playMenu.SetActive(false);
        exitMenu.SetActive(true);
    }   

    public void ReturnMenu()
    {
        mainCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        PanelGame.SetActive(false);
        lineGame.SetActive(false);
        PanelControls.SetActive(false);
        lineControls.SetActive(false);
		playMenu.SetActive(false);
		exitMenu.SetActive(false);
		mainMenu.SetActive(true);
	}
    
    //Play canvas
    public void NewGameButtonClicked()
    {
        saveLoadManager.playerData.coin = PlayerPrefs.GetInt("currency");
        saveLoadManager.playerData.lastChoose = PlayerPrefs.GetInt("cPointer");
        SaveOwnedCarData();
        SceneManager.LoadScene("MapSelectScene");
    }

    //Shop canvas
    public void RightButtonClicked()
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
            childObject.GetComponent<CarController>().enabled = false;
            if(childObject.GetComponent<CarController>().carEngineSound != null)
            {
                childObject.GetComponent<CarController>().carEngineSound.Stop();
            }
            if(childObject.GetComponent<CarController>().tireScreechSound != null)
            {
                childObject.GetComponent<CarController>().tireScreechSound.Stop();
            }
            GetCarInfo();
        }
    }

    public void LeftButtonClicked()
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
            childObject.GetComponent<CarController>().enabled = false;
            if(childObject.GetComponent<CarController>().carEngineSound != null)
            {
                childObject.GetComponent<CarController>().carEngineSound.Stop();
            }
            if(childObject.GetComponent<CarController>().tireScreechSound != null)
            {
                childObject.GetComponent<CarController>().tireScreechSound.Stop();
            }
            GetCarInfo();
        }
    }
    public void BuyButtonClicked()
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

        shopCurrency.text = PlayerPrefs.GetInt("currency").ToString();

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

    public void LockButtonClicked()
    {
        bool isUnlockActive = unlockImage.gameObject.activeSelf;
        if(isUnlockActive)
        {
            unlockImage.gameObject.SetActive(false);
            lockImage.gameObject.SetActive(true);
            PlayerPrefs.SetInt("cPointer" , carPointer);
        }
    }

    public void ShopReturnButtonClicked()
    {
        shopCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        mainMenu.SetActive(true);

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

        mainCurrency.text = saveLoadManager.playerData.coin.ToString();
        saveLoadManager.SaveData();
    }
    //Setting canvas
	void DisablePanels()
    {
		settingsCanvas.SetActive(false);
	}

	public void GameButtonClicked()
    {
		PanelControls.SetActive(false);
        lineControls.SetActive(false);
		PanelGame.SetActive(true);
		lineGame.SetActive(true);
	}

    public void NormalButtonClicked()
    {
        lineHardcore.SetActive(false);
        lineNormal.SetActive(true);
    }

    public void HardcoreButtonClicked()
    {

        lineNormal.SetActive(false);
        lineHardcore.SetActive(true);
    }

	public void ControlButtonClicked()
    {
		PanelGame.SetActive(false);
		lineGame.SetActive(false);
		PanelControls.SetActive(true);
		lineControls.SetActive(true);
	}
    
    //Exit canvas
    public void QuitGame()
    {
        saveLoadManager.playerData.coin = PlayerPrefs.GetInt("currency");
        saveLoadManager.playerData.lastChoose = PlayerPrefs.GetInt("cPointer");
        SaveOwnedCarData();
        saveLoadManager.SaveData();
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

    public void PlayHover()
    {
		hoverSound.Play();
	}

	public void PlaySFXHover()
    {
		sliderSound.Play();
	}

	public void PlaySwoosh()
    {
		swooshSound.Play();
	}

    // IEnumerator LoadAsynchronously(string sceneName)
    // { // scene name is just the name of the current scene being loaded
	// 	AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
	// 	operation.allowSceneActivation = false;
	// 	mainCanvas.SetActive(false);
	// 	loadingMenu.SetActive(true);

	// 	while (!operation.isDone)
    //     {
	// 		float progress = Mathf.Clamp01(operation.progress / .95f);
	// 		loadingBar.value = progress;

	// 		if (operation.progress >= 0.9f && waitForInput)
    //         {
	// 			loadPromptText.text = "Press " + userPromptKey.ToString().ToUpper() + " to continue";
	// 			loadingBar.value = 1;

	// 			if (Input.GetKeyDown(userPromptKey))
    //             {
	// 				operation.allowSceneActivation = true;
	// 			}
    //         }
    //         else if(operation.progress >= 0.9f && !waitForInput)
    //         {
	// 			operation.allowSceneActivation = true;
	// 		}

	// 		yield return null;
	// 	}
	// }
}