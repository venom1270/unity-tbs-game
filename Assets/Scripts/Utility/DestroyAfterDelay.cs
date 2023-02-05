using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField]
    private float destroyAfterSeconds;

    private float startTime;
    private float destroyAtTime;

    void Start()
    {
        startTime = Time.time;
        destroyAtTime = startTime + destroyAfterSeconds;
        Debug.Log(startTime);
        Debug.Log(destroyAtTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyAtTime < Time.time) 
        {
            Destroy(gameObject);
        }
    }
}
