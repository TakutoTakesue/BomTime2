using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AnimScript : MonoBehaviour
{
    [SerializeField, Header("攻撃処理を持ったオブジェクト")]
    GameObject attackSensing;
    // Start is called before the first frame update
    void Start()
    {
        attackSensing.SetActive(false);
    }

    // アニメーションからくる攻撃命令
    void Attack() {
        attackSensing.SetActive(true);
    }

    // 攻撃の終わり
    void AttackFinish() {
        attackSensing.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
