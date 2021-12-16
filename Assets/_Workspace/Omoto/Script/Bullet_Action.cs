using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Action : MonoBehaviour
{
    public GameObject HitEffect; //衝突時のエフェクト

   
    // 消えるまでの時間
    public float DestroyCnt = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 発射されてから自身を削除
        Destroy(gameObject, DestroyCnt);
    }
    void OnCollisionEnter(Collision other)
    {
        //何かに当たったらエフェクトを表示して消える.
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
