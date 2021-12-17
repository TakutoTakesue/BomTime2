using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Play_UI_Managet P_UI_MGR;

    float Elapsed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        P_UI_MGR.Time_UI(Elapsed);
    }
}
