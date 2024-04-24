using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowCamera : MonoBehaviour 
{

    private GameObject Player , CameraLookAt, CameraFollow;
    private CarController CC;
    
    private CinemachineVirtualCamera CVC; 
    private void Start () 
    {
        Player = GameObject.FindGameObjectWithTag ("Player");
        CVC = GetComponent<CinemachineVirtualCamera>();
        
        CameraLookAt = Player.transform.Find ("Camera lookat").gameObject;
        CameraFollow = Player.transform.Find ("Camera constraint").gameObject;

        CVC.LookAt = CameraLookAt.transform;
        CVC.Follow = CameraFollow.transform;
    }
}