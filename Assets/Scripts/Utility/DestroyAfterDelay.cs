using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField]
    private float destroyAfterSeconds;

    private float startTime;
    private float destroyAtTime;

    bool qwe = false;

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
            if (qwe)
            {
                Destroy(gameObject);
            }
            else
            {
                qwe = true;
                destroyAtTime += destroyAfterSeconds;
                foreach (Transform t in transform.GetComponentsInChildren<Transform>())
                {
                    if (t.TryGetComponent<MeshCollider>(out MeshCollider mc))
                    {
                        mc.enabled = false;
                    }
                }
            }
            
        }

        if (qwe)
        {
            Vector3 pos = transform.position;
            float speed = 0.005f; //* Time.deltaTime;
            pos.y -= speed;
            Debug.Log(speed);
            transform.position = pos;
        }
        
    }
}
