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

        // ランキング処理
        int newRank = 0; 
        for (int idx = 5; idx > 0; idx--)
        { //逆順 5...1
            if (Rank[idx] > Elapsed)
            {
                newRank = idx; 
            }
        }
        if (newRank != 0)
        {
            for (int idx = 5; idx > newRank; idx--)
            {
                Rank[idx] = Rank[idx - 1];
            }
            Rank[newRank] = Elapsed;
            for (int idx = 1; idx <= 5; idx++)
            {
                PlayerPrefs.SetFloat("R" + idx, Rank[idx]);
            }
        }

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
