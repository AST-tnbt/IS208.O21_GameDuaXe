using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapSelectManager : MonoBehaviour
{

    [Header("Car Select Canvas")]
    public GameObject SelectMapCanvas;
    public GameObject rightButton;
    public GameObject leftButton;
    public GameObject startButton;
    public GameObject backButton;
    public ListMap listOfMap;
    public TextMeshProUGUI nameOfMap;
    public GameObject rotateTurnTable;

    [HideInInspector] public float rotateSpeed = 10f;
    [HideInInspector] public int mapPointer = 0;

    void Awake()
    {
        SelectMapCanvas.SetActive(true);
        mapPointer = PlayerPrefs.GetInt("mp");
        GameObject childObject = Instantiate(listOfMap.Maps[mapPointer],Vector3.zero,rotateTurnTable.transform.rotation) as GameObject;
        childObject.transform.parent = rotateTurnTable.transform;
        GetMapInfo();
    }

    private void FixedUpdate() 
    {
        rotateTurnTable.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void RightButton()
    {
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

    public void LeftButton()
    {
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

    public void StartGameButton()
    {
        int mapIndex = PlayerPrefs.GetInt("mp");
        MapModifier MM = listOfMap.Maps[mapIndex].GetComponent<MapModifier>();

        SceneManager.LoadScene(MM.mapName);
    }
    public void BackButton()
    {
        SceneManager.LoadScene("MainScene");
    }

}
