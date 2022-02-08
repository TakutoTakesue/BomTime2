using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  全体のシーン遷移管理 
///  全シーン通して必要なもの
/// </summary>

public class GameManager : MonoBehaviour
{


    //  シーン遷移.
    enum Scene
    {
        Title,
        Select,
        Play,
        Ranking
    }

    // ステージ数.
    public enum StageNum
    {
        stage1
    }

    Scene scene;
    StageNum stage;

    // Start is called before the first frame update
    void Start()
    {
        scene = Scene.Title;
        stage = StageNum.stage1;
    }

    // プレイスタート処理.
    void PlayStater()
    {
    }

    // ゲーム
    void GameEnd()
    {

    }
    // Update is called once per frame
    void Update()
    {
       
        switch (scene)
        {
            case Scene.Title:

                if(Input.GetButtonDown("BtnA"))
                {
                    SceneManager.LoadScene("Select");
                    scene = Scene.Select;
                }

                break;

                // ステージ選択.
            case Scene.Select:
                
                
                
                break;

            case Scene.Play:

               

                break;

            case Scene.Ranking:
                if (Input.GetButtonDown("BtnA"))
                {
                    SceneManager.LoadScene("Title");
                    scene = Scene.Title;
                }
                break;

            default:
                break;
        }

    }
}
