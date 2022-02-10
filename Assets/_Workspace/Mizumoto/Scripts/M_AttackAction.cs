using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AttackAction : MonoBehaviour
{
    [SerializeField, Header("敵の本体")]
    M_EnemyAIBase enemy;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        // プレイヤーが居たら
        if (other.gameObject.tag == "Player")
        {
            // 攻撃
            var state = other.GetComponent<M_StateAction>();
            // 敵にステータスがあってプレイヤーにもステータスがある場合
            if (state && enemy.EnemyState)
            {
                var playerScript= other.GetComponent<PlayerAction>();

                if (playerScript)
                {
                    if (!playerScript.IsDamage) {
                        // 敵の攻撃力分ダメージを与える
                        state.OnDamage(enemy.EnemyState.Attack);
                        // 攻撃判定を消す
                        gameObject.SetActive(false); 
                    }
                }
                else {
                    Debug.LogWarning("プレイヤーにplayerScriptがついていません");
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
