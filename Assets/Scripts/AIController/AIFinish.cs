using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class AIFinish : MonoBehaviour
{
    public int aiRc = 0;
    public int round = 0;
    public AICarBehaviour acb;
    public AICarControl acc;
    public AIController ac;

    void Awake()
    {
        round = PlayerPrefs.GetInt("Round");
        acb = GameObject.FindGameObjectWithTag ("AI").GetComponent<AICarBehaviour>();
        acc = GameObject.FindGameObjectWithTag ("AI").GetComponent<AICarControl>();
        ac = GameObject.FindGameObjectWithTag ("AI").GetComponent<AIController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AICar1")
        {
            aiRc++;
        }
        if(aiRc == round)
        {
            acb.enabled = false;
            acc.enabled = false;
            ac.hasFinished = true;
        }
    }
}
