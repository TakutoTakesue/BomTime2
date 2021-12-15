using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    GameObject Player;

    // HP回復量
    public float recovery_amount = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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
        // 回転 わかりやすように？
        //transform.Rotate(Vector3.up * Time.deltaTime * 90.0f);
    }
}
