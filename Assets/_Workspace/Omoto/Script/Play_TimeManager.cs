using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //シーンのロードに必要

public class Play_TimeManager : MonoBehaviour
{
    // 経過時間.
    public float Elapsed = 0.0f;

    public static float Score;
 
    // ランキング用
    float[] Rank = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GameEnd()
    {
        Score = Elapsed;
        
        Invoke("NextScene",2.0f);

    }

    void NextScene()
    {
        SceneManager.LoadScene("Rank");
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;
        
    }
}
