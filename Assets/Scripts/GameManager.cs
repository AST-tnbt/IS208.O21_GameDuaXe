using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Callbacks;
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

    [Header ("Racers List")]
    public GameObject uiList;
    public GameObject uiListFolder;
    public GameObject backImage;
    private List<Car> presentCars;
    private List<GameObject> temporaryList;
    private GameObject[] temporaryArray;

    void Awake()
    {
        //Tạo một đối tượng mới trong trò chơi từ danh sách vehicles trong biến list. PlayerPrefs.GetInt("pointer")
        Instantiate (listCar.Cars[PlayerPrefs.GetInt ("pointer")], startPosition.transform.position, startPosition.transform.rotation);

        //Tìm đối tượng có tag là "Player" trong cảnh và gán vào biến CC. Sau đó, từ đối tượng này, thành phần controller được trích xuất và gán vào biến CC.
        CC = GameObject.FindGameObjectWithTag ("Player").GetComponent<CarController>();

        //Tìm tất cả các đối tượng có tag là "AI" trong cảnh và gán chúng vào mảng presentGameObjectCars.
        //presentGameObjectCars = GameObject.FindGameObjectsWithTag("AI"); 

        //presentCars = new List<Car> ();

        //foreach (GameObject R in presentGameObjectCars)
        //    presentCars.Add (new Car (R.GetComponent<AIDriver>().currentNode, R.GetComponent<AIDriver>().carName, R.GetComponent<AIDriver>().hasFinished));

        //presentCars.Add (new Car (CC.gameObject.GetComponent<AIDriver>().currentNode, CC.carName , CC.hasFinished));

        // temporaryArray = new GameObject[presentCars.Count];

        //temporaryList = new List<GameObject> ();
        //foreach (GameObject R in presentGameObjectCars)
        //    temporaryList.Add (R);
        //temporaryList.Add (CC.gameObject);
        //fullArray = temporaryList.ToArray ();
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
