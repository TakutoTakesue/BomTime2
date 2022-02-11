using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //uGUIを扱うのに必要
using UnityEngine.SceneManagement; //シーンのロードに必要

public class RankAction : MonoBehaviour
{
    float Elapsed = 0.0f;

    public Text txtNavi;
    public Text txtScore;

    float Score = Play_TimeManager.Score;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;

        Elapsed %= 1.0f;

        txtNavi.text = (Elapsed < 0.8f) ? "Press A or Click Title" : "";

        txtScore.text = "TIME:" + Score.ToString("f1") + "s";

        if (Input.GetButtonDown("BtnA") || Input.GetMouseButtonDown(0))
        {
                SceneManager.LoadScene("Title");
            
        }
    }
}
