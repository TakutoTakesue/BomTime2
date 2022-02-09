using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_TimeManager : MonoBehaviour
{

    // 経過時間.
    public float Elapsed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        
    }
}
