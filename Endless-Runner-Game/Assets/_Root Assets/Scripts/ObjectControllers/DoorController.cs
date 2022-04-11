using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float doorSpeed = 3f;
    [SerializeField] private float openPositionX = 1.98f;
    
    private Vector3 desiredPositionL;
    private Vector3 desiredPositionR;

    private Transform leftDoor;
    private Transform rightDoor;

    private void Awake()
    {
        leftDoor = transform.Find("left_door");
        rightDoor = transform.Find("right_door");
        
        desiredPositionL = new Vector3(openPositionX, 0f, 0f);
        desiredPositionR = new Vector3(-openPositionX, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // print("open door");
        StartCoroutine(OpenLeftDoorCoroutine());
        StartCoroutine(OpenRightDoorCoroutine());
        
    }

    private IEnumerator OpenLeftDoorCoroutine()
    {
        while (Vector3.Distance(leftDoor.localPosition, desiredPositionL) > -0.0001f)
        {
            leftDoor.localPosition = Vector3.Lerp(
                leftDoor.localPosition, 
                desiredPositionL, 
                doorSpeed * Time.deltaTime);
            
            yield return null;
        }
        
    }
    
    private IEnumerator OpenRightDoorCoroutine()
    {
        while (Vector3.Distance(rightDoor.localPosition, desiredPositionR) > -0.0001f)
        {
            rightDoor.localPosition = Vector3.Lerp(
                rightDoor.localPosition, 
                desiredPositionR, 
                doorSpeed * Time.deltaTime);
            
            yield return null;
        }
        
    }
}
