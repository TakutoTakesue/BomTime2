﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k_TestAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.GetComponent<MeshFilter>().mesh);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
