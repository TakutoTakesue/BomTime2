using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //シーンのロードに必要

public class Play_TimeManager : MonoBehaviour
{
    // 経過時間.
    public float Elapsed = 0.0f;

    public static float Score;

    public bool GameOverFlg = false;
    public bool GameClearFlg = false;

    //プレイヤーのオブジェクト
    [SerializeField] GameObject player;
    M_StateAction act_MState;
    PlayerAction act_Player;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.StartBgm("Play_BGM", 2.0f);

        act_MState = player.GetComponent<M_StateAction>();
        act_Player = player.GetComponent<PlayerAction>();

        Score = 0.0f;
        GameOverFlg = false;
        GameClearFlg = false;
    }

    public void GameEnd()
    {
        if(!GameOverFlg)
        {
            GameClearFlg = true;
        }
        AudioManager.Instance.StopBgm(2.0f);
        Invoke("NextScene",2.0f);

    }

    void NextScene()
    {

        if(GameOverFlg)
        {
            SceneManager.LoadScene("Select");
        }
        else 
        {
            Score = Elapsed;
            SceneManager.LoadScene("Rank");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;

        if(act_MState.HP<=0)
        {
            GameOverFlg = true;
            GameEnd();
        }
        
    }
}
