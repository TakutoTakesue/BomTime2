using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 経過時間.
    public float Elapsed = 0.0f;

    //  シーン遷移.
    enum Scene
    {
        Title,
        Select,
        Play,
        Ranking
    }

    // ステージ数.
    enum StageNum
    {
        stage1=0
    }

    Scene scene;
    StageNum stage;

    // Start is called before the first frame update
    void Start()
    {
        scene = Scene.Title;
        stage = StageNum.stage1;
    }

    // Update is called once per frame
    void Update()
    {
 
        switch(scene)
        {
            case Scene.Title:

                //if()
                //{
                //    Elapsed = 0.0f;
                //}

                break;

            case Scene.Play:
                Elapsed += Time.deltaTime;
                break;
        }

    }
}
