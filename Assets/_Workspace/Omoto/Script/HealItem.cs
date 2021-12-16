using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    GameObject Player;

    // HP回復量
    public float recovery_amount = 50.0f;

    public float DestoryCnt = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        transform.rotation= Quaternion.AngleAxis(-90.0f, new Vector3(1, 0, 0));

        // 60秒経過で削除　(外してもいいかなぁ)
        Destroy(gameObject, DestoryCnt);
    }
    void OnTriggerEnter(Collider other)
    {
        // プレイヤーと触れたとき　(ゲットされたら)
        if (other.gameObject.tag == "Player")
        {
            // プレイヤーのHPを回復
            /* Player.SendMessage("HP_Heal",recovery_amount,SendMessageOptions.DontRequireReceiver);
             *
             * もしくは
             * 
             * Player.HP+=recovery_amount 
             * 
             * など未定
             */


            // 自分は消える
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // 回転
        transform.Rotate(new Vector3(0.0f, 0.0f, 0.3f));
        // 上下
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * 0.45f, transform.position.z);
    }
}
