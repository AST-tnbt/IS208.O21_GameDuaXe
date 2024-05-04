using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public ListCar listCar;
    public CarController CC;
    public GameObject needle;
    public GameObject startPosition;
    public TextMeshProUGUI Kph;
    public Slider nitrusSlider;
    private float startPos = 32f, endPos = -211f;
    private float desiredPosition;
    private GameObject[] presentGameObjectCars , fullArray;

    void Awake()
    {
        //Tạo một đối tượng mới trong trò chơi từ danh sách vehicles trong biến list. PlayerPrefs.GetInt("pointer")
        Instantiate (listCar.Cars[PlayerPrefs.GetInt("cPointer")], startPosition.transform.position, startPosition.transform.rotation);

        //Tìm đối tượng có tag là "Player" trong cảnh và gán vào biến CC. Sau đó, từ đối tượng này, thành phần controller được trích xuất và gán vào biến CC.
        CC = GameObject.FindGameObjectWithTag ("Player").GetComponent<CarController>();
        CC.useSounds = true;
    }

    private void FixedUpdate () 
    {
        if(CC.carSpeed >= 0)
        {
            Kph.text = CC.carSpeed.ToString ("0");
        }
        else
        {
            Kph.text = (-CC.carSpeed).ToString("0");
        }
        updateNeedle ();
        nitrusUI ();
    }

    public void updateNeedle () 
    {
        desiredPosition = startPos- endPos;
        float temp = CC.RPM / 1500;
        needle.transform.eulerAngles = new Vector3 (0, 0, (startPos - temp * desiredPosition));
    }

    public void nitrusUI () 
    {
        nitrusSlider.value = CC.nitrusValue/10;
    }
    public void loadAwakeScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
