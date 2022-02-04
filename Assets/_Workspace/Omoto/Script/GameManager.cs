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
        Elapsed = 0.0f;
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
                }

                break;

                // ステージ選択.
            case Scene.Select:
                Elapsed = 3.0f;

                
                
                break;

            case Scene.Play:
                Elapsed += Time.deltaTime;



                break;

            case Scene.Ranking:
                if (Input.GetButtonDown("BtnA"))
                {
                    SceneManager.LoadScene("Title");
                }
                break;
        }

    }
}
