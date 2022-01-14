using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play_UI_Managet : MonoBehaviour
{
    public Text txtBullet_cnt;
    public Text txtTime;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Time_UI(0.0f);
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
        txtBullet_cnt.text = "豆:" + Bullet_cnt.ToString().PadLeft(3, '0');
    }

    // 残り時間表示
    public void Time_UI(float TIME)
    {
        txtTime.text = "Time:" + TIME.ToString("f3")+"s";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
