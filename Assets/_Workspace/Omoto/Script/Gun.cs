using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float Vel = 45.0f; //弾の初速度
    public float interval = 0.5f; //弾の射出間隔
    public Text txtBullet_cnt;

    int Bullet_cnt; // 弾の残り数

    public GameObject Bean_Bullet; //発射用弾
    GameObject B;   // 発射用を生成
   
    bool isTrigger = false; //　銃が発射されているか,打てるのか

    // Start is called before the first frame update
    void Start()
    {
        // 銃の発射待機
        StartCoroutine("Shot");

        Bullet_cnt = 5;
    }

    IEnumerator Shot()
    {
        // 打たれる待機
        while (true)
        {

            Vector3 Bullet_pos;
            Bullet_pos.y = transform.position.y + 0.5f;
            Bullet_pos.x = transform.position.x;
            Bullet_pos.z = transform.position.z + 1;

            if (isTrigger)
            { 
                B = Instantiate(Bean_Bullet,Bullet_pos , Quaternion.identity);
                B.GetComponent<Rigidbody>().velocity = transform.position * Vel;

                Bullet_cnt--;
            }
            yield return new WaitForSeconds(interval); //射出間隔だけ待つ
        }
    }

    // 装弾数の加算
    void Bullet_Add()
    {
        Bullet_cnt++;
    }

    // Update is called once per frame
    void Update()
    {
        // Bullet_cntが0より大きければトリガーを引ける.
        if (Bullet_cnt > 0)
        {
            isTrigger = Input.GetKey(KeyCode.Space);
        }
        else
        {
            isTrigger = false;
        }

        txtBullet_cnt.text = "Bullet:" + Bullet_cnt.ToString().PadLeft(3, '0'); 

    }
}
