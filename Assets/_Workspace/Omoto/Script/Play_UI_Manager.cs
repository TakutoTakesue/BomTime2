using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * UI表示関連 プレイヤーとゲームマネージャーの取得が必要です.
 */
public class Play_UI_Manager : MonoBehaviour
{
    [SerializeField] Text txtBullet_cnt;
    [SerializeField] Text txtTime;

    [SerializeField] GameObject HealImage;
    
    //プレイヤーのオブジェクト
    [SerializeField] GameObject player;

    [SerializeField] Slider slider;

    //　ゲームマネージャーの取得
    [SerializeField] private GameManager ManaerScript;
 

    M_StateAction act_MState;
    PlayerAction act_Player;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Time_UI(0.0f);
        HealImage.SetActive(false);
        //Sliderを満タンにする。
        slider.value = 1;

        GameObject gamemanager = GameObject.FindWithTag("GameController");
        ManaerScript = gamemanager.GetComponent<GameManager>();

        act_MState = player.GetComponent<M_StateAction>();
        act_Player = player.GetComponent<PlayerAction>();
    }

    // 初期化
    public void Init()
    {

    }

    // 弾の残り弾数表示
    public void Bullet_UI(int Bullet_cnt)
    {
        txtBullet_cnt.text = ":"+ Bullet_cnt.ToString().PadLeft(3, '0');
    }

    // 残り時間表示
    public void Time_UI(float TIME)
    {
        txtTime.text = "TIME:" + TIME.ToString("f0").PadLeft(3, '0');
    }

    //恵方巻の表示非表示 所持数0以下なら非表示
    public void HealItem_UI(int ItemCnt)
    {
        if (ItemCnt <= 0) 
        {
            HealImage.SetActive(false);
        }
        else
        {
            HealImage.SetActive(true);
        }
    }

    //プレイヤーのHPバーの増減
    public void PlayerHP_Bar(int PlayerHP, int MaxHP)
    {
        slider.value = (float)PlayerHP / (float)MaxHP;
    }

   
    // Update is called once per frame
    void Update()
    {

        PlayerHP_Bar(act_MState.HP, act_MState.HPMax);
        Time_UI(ManaerScript.Elapsed);

        Bullet_UI(act_Player.GetBulletCnt);
        HealItem_UI(act_Player.GetEhomaki);
    }
}
