using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyME : MonoBehaviour
{
    // The object on which this code is located will destroy itself after a while

    public int lifeTime;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
