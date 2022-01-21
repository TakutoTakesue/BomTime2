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

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Time_UI(0.0f);

        HealImage.SetActive(false);

        //Sliderを満タンにする。
        slider.value = 1;
    }

    // 初期化
    public void Init()
    {
        txtBullet_cnt.text = "";
        txtTime.text = "Time:"+"88.888";
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
    public void PlayerHP_Bar(int PlayerHP,int MaxHP)
    {
        slider.value = (float)PlayerHP / (float)MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
