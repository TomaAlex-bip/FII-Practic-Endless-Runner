using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraMainMenuAnimation : MonoBehaviour
{
    [SerializeField] private float maxPosition = 30f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float minSpeed = 5f;

    private Vector3 initialPos;
    
    private bool moving;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        if (!moving)
        {
            print("am pornit");
            StartCoroutine(MoveCamera());
        }
    }

    private IEnumerator MoveCamera()
    {
        moving = true;
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);
        var randomPos = initialPos + new Vector3(x, y) * maxPosition;
        var randomSpeed = Random.Range(minSpeed, maxSpeed);
        
        print(randomPos);
        
        while(Mathf.Abs(Vector3.Distance(transform.position, randomPos)) >= 0.1f)
        {
            // print(Mathf.Abs(Vector3.Distance(transform.position, randomPos)));
            
            transform.position = Vector3.MoveTowards(
                transform.position,
                randomPos,
                 randomSpeed * Time.deltaTime);

            // print("merge");
            yield return null;
        }

        moving = false;
    }
}
