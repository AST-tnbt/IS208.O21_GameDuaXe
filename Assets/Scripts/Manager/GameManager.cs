using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class GameManager : MonoBehaviour
{

    public ListCar listCar;
    public CarController cc;
    public AICarBehaviour acb;
    public AICarControl acc;
    public AIController ac;
    public GameObject needle;
    public GameObject startPosition;
    public TextMeshProUGUI kph;
    public Slider nitrusSlider;
    public GameObject finishSprite;
    public GameObject cDPanel;
    public TextMeshProUGUI cooldownText;
    public GameObject notifCanvas;
    public GameObject pauseMenuUI;
    public TextMeshProUGUI timeLapText;
    public GameObject finishCanvas;
    public TextMeshProUGUI finishTime;
    public TextMeshProUGUI finishPlace;
    public int slotOrder = 0;
    private bool isFinish = false;
    private bool isPaused = false;
    private bool isStart = false;
    private float timeLap = 0f;
    private float startPos = 32f, endPos = -211f;
    private float desiredPosition;
    private int round = 0;
    private RaceFinish raceFinish;
    private float cooldownTime = 3f;

    private int isHard = 0;

    [Header("SFX")]
    [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
    public AudioSource hoverSound;

    void Awake()
    {   
        isFinish = false;
        cDPanel.SetActive(true);
        finishCanvas.SetActive(false);
        notifCanvas.SetActive(false);
        //Tạo một đối tượng mới trong trò chơi từ danh sách vehicles trong biến list. PlayerPrefs.GetInt("pointer")
        Instantiate (listCar.Cars[PlayerPrefs.GetInt("cPointer")], startPosition.transform.position, startPosition.transform.rotation);

        //Tìm đối tượng có tag là "Player" trong cảnh và gán vào biến CC. Sau đó, từ đối tượng này, thành phần controller được trích xuất và gán vào biến CC.
        cc = GameObject.FindGameObjectWithTag ("Player").GetComponent<CarController>();
        cc.useSounds = true;
        cc.hasFinished = false;

        acb = GameObject.FindGameObjectWithTag ("AI").GetComponent<AICarBehaviour>();
        acc = GameObject.FindGameObjectWithTag ("AI").GetComponent<AICarControl>();
        ac = GameObject.FindGameObjectWithTag ("AI").GetComponent<AIController>();
        ac.hasFinished = false;

        raceFinish = GameObject.FindGameObjectWithTag ("Finish").GetComponent<RaceFinish>();
        round = PlayerPrefs.GetInt("Round", 0);
        isHard = PlayerPrefs.GetInt("IsHard", 0);
        if(isHard == 0)
        {
            ac.m_FullTorqueOverAllWheels = 250;
            ac.m_Topspeed = 50;
        }
        else if(isHard == 1)
        {
            ac.m_FullTorqueOverAllWheels = 2250;
            ac.m_Topspeed = 200;
        }
        finishSprite.SetActive(false);
    }

    private void Update () 
    {
        UpdateTimeLap();
        UpdateFinishSprite();
        RaceOrder();
        if(cc.carSpeed >= 0)
        {
            kph.text = cc.carSpeed.ToString ("0");
        }
        else
        {
            kph.text = (-cc.carSpeed).ToString("0");
        }
        UpdateNeedle();
        NitrusUI();
        if(cooldownTime > 0)
        {
            Cooldown();
            cc.enabled = false;
            acb.enabled = false;
            acc.enabled = false;
        }
        else
        {
            if(!isStart)
            {
                cooldownText.text = "Start";
                Invoke("StartGame",0.5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        if(raceFinish.roundCurrent == round && !isFinish)
        {
            isFinish = true;
            finishCanvas.SetActive(true);
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeLap);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            finishTime.text = formattedTime;
            if(slotOrder == 1)
            {
                finishPlace.text = "1st";
            }
            else if(slotOrder == 2)
            {
                finishPlace.text = "2nd";
            }
            PlayerPrefs.SetInt("SlotOrder", slotOrder);
            Debug.Log(PlayerPrefs.GetInt("SlotOrder"));
            Debug.Log(PlayerPrefs.GetInt("Betcoin"));
            Invoke("ReturnMainScene",5f);
        }
    }

    private void UpdateTimeLap()
    {
        if(isStart)
        {
            if(!isFinish)
            {
                timeLap += Time.deltaTime;
                TimeSpan timeSpan = TimeSpan.FromSeconds(timeLap);
                string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}.{3}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
                timeLapText.text = formattedTime;
            }
        }
        
    }
    public void RaceOrder()
    {
        if(cc.hasFinished == true && ac.hasFinished == false)
        {
            slotOrder = 1;
        }
        else if(cc.hasFinished == false && ac.hasFinished == true)
        {
            slotOrder = 2;
        }
    }
    public void StartGame () 
    {
        cc.enabled = true;
        acb.enabled = true;
        acc.enabled = true;
        cDPanel.SetActive(false);
        isStart = true;
    }
    public void UpdateNeedle () 
    {
        desiredPosition = startPos - endPos;
        float temp = cc.carSpeed / 200;
        float Pos = Mathf.Abs(temp * desiredPosition);
    
        needle.transform.eulerAngles = new Vector3 (0, 0, (startPos - Pos));
    }
    public void UpdateFinishSprite()
    {
        if(round == 1)
        {
            finishSprite.SetActive(true);
        }
        else
        {
            if(raceFinish.roundCurrent == round - 1)
            {
                finishSprite.SetActive(true);
            }
        }
    }
    public void NitrusUI () 
    {
        nitrusSlider.value = cc.nitrusValue/10;
    }

    public void Cooldown()
    {
        int cd = (int)cooldownTime;
        cooldownText.text = cd.ToString();
        cooldownTime -= Time.deltaTime;
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;
        isStart = false;
        if(cc.carEngineSound != null)
        {
            cc.carEngineSound.Stop();
        }
        if(cc.tireScreechSound != null)
        {
            cc.tireScreechSound.Stop();
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        isStart = true;
        if(cc.carEngineSound != null)
        {
            cc.carEngineSound.Play();
        }
        if(cc.tireScreechSound != null)
        {
            cc.tireScreechSound.Play();
        }
    }
    public void ReturnBut()
    {
        slotOrder = 2;
        PlayerPrefs.SetInt("SlotOrder", slotOrder);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
    public void ReturnMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void PlayHover()
    {
		hoverSound.Play();
	}
}
