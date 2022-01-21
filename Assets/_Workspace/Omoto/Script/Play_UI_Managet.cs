using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * UI表示関連
 * アイテムのカウントを保持している辺巣を入れてもらえれば表示されます。
 * publicになってるので引っ張ってUpdateにおいといてもらえたら表示されます
 * 
 * Timeはゲームマネージャーに
 * 他のアイテムカウントはプレイヤーに
 */
public class Play_UI_Managet : MonoBehaviour
{
    [SerializeField] Text txtBullet_cnt;
    [SerializeField] Text txtTime;

    [SerializeField] GameObject HealImage;

    [SerializeField] Slider slider;

    [SerializeField] private PlayerAction PlayerScript;
    [SerializeField] private GameManager ManaerScript;
    [SerializeField] private Gun GunScript;

    int Bullet_cnt;
    float TIME;
    int ItemCnt;
    int PlayerHP;
    int MaxHP;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Time_UI();
        HealImage.SetActive(false);
        //Sliderを満タンにする。
        slider.value = 1;

        GameObject player = GameObject.FindWithTag("Player");
        PlayerScript = player.GetComponent<PlayerAction>();

        GameObject gamemanager = GameObject.FindWithTag("GameController");
        ManaerScript = gamemanager.GetComponent<GameManager>();

        GameObject gun = GameObject.FindWithTag("Gun");
        GunScript = gun.GetComponent<Gun>();

    }

    // 初期化
    public void Init()
    {

    }

    // 弾の残り弾数表示
    public void Bullet_UI()
    {
        txtBullet_cnt.text = ":"+ Bullet_cnt.ToString().PadLeft(3, '0');
    }

    // 残り時間表示
    public void Time_UI()
    {
        txtTime.text = "TIME:" + TIME.ToString("f0").PadLeft(3, '0');
    }

    //恵方巻の表示非表示 所持数0以下なら非表示
    public void HealItem_UI()
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
    public void PlayerHP_Bar()
    {
        slider.value = (float)PlayerHP / (float)MaxHP;
    }

   
    // Update is called once per frame
    void Update()
    {
        TIME = ManaerScript.Elapsed;



        Time_UI();
    }
}
