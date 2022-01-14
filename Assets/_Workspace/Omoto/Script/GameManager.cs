using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Play_UI_Managet P_UI_MGR;

    float Elapsed = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        P_UI_MGR = GetComponent<Play_UI_Managet>();
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed -= Time.deltaTime;

        P_UI_MGR.Time_UI(Elapsed);
    }
}
