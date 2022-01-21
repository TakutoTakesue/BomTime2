using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    float Elapsed = 100.0f;

    GameObject UI_MGR;
    Play_UI_Managet P_UI_MGR;

    // Start is called before the first frame update
    void Start()
    {
        UI_MGR = GameObject.FindGameObjectWithTag("GameController");
        P_UI_MGR = UI_MGR.GetComponent<Play_UI_Managet>();

    }

    // Update is called once per frame
    void Update()
    {
        Elapsed -= Time.deltaTime;

        P_UI_MGR.Time_UI(Elapsed);

        /*UIテスト用*/
        P_UI_MGR.HealItem_UI(1);
        P_UI_MGR.PlayerHP_Bar(90, 100);
    }
}
