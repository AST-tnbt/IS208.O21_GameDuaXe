using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriver : MonoBehaviour 
{

    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;
    private TrackWaypoint wayPoints;
    private Transform currentWayPoint;
    private List<Transform> nodes = new List<Transform>();
    private int distanceOffset = 5;
    private float steerForce = 1;

    [Header("AI ACCELERATION VALUE")]
    [Range(0, 1)] public float acceleration = 0.5f;
    public int currentNode;
    public int carPrice ;
    public string carName;
    public bool hasFinished;

    private void Start() 
    {
        wayPoints = GameObject.FindGameObjectWithTag("path").GetComponent<TrackWaypoint>();
        currentWayPoint = gameObject.transform;
        nodes = wayPoints.nodes;
    }

    private void FixedUpdate() 
    {
        AIDrive();
    }

    private void AIDrive() 
    {
        calculateDistanceOfWaypoints();
        AISteer();
        vertical = acceleration;
    }

    private void calculateDistanceOfWaypoints() 
    {
        Vector3 position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++) 
        {
            Vector3 difference = nodes[i].transform.position - position;
            float currentDistance = difference.magnitude;
            if (currentDistance < distance) 
            {
                if ((i + distanceOffset) >= nodes.Count) 
                {
                    currentWayPoint = nodes[1];
                    distance = currentDistance;
                } 
                else 
                {
                    currentWayPoint = nodes[i + distanceOffset];
                    distance = currentDistance;
                }
                currentNode = i;
            }
        }
    }

    private void AISteer() 
    {
        Vector3 relative = transform.InverseTransformPoint(currentWayPoint.transform.position);
        
        relative /= relative.magnitude;

        horizontal = (relative.x / relative.magnitude) * steerForce;
    }
}
