using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaceFinish : MonoBehaviour
{
    public int roundCurrent = 0;
    public int round = 0;
    public CarController cc;
    public GameObject notifCanvas;
    public TextMeshProUGUI notifText;

    void Awake()
    {
        round = PlayerPrefs.GetInt("Round");
        cc = GameObject.FindGameObjectWithTag ("Player").GetComponent<CarController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collider")
        {
            roundCurrent++;
            string text = "You finished: " + roundCurrent.ToString() + "/" + round.ToString();
            ShowNotifMessage(text);
            Invoke("CloseNotifMessage",2f);
        }
        if(roundCurrent == round)
        {
            cc.hasFinished = true;
            cc.enabled = false;
            if(cc.carEngineSound != null)
            {
                cc.carEngineSound.Stop();
            }
            if(cc.tireScreechSound != null)
            {
                cc.tireScreechSound.Stop();
            }
        }
    }
    private void ShowNotifMessage(string message)
    {
        notifText.text = "" + message;
        notifCanvas.SetActive(true);
    }
    
    public void CloseNotifMessage()
    {
        notifText.text = "";
        notifCanvas.SetActive(false);
    }
}
