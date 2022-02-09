using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class M_AttackSensingAction : MonoBehaviour
{
    [SerializeField, Header("誰に対して攻撃と通達するか")]
    M_EnemyAIBase enemy;
  
    // Start is called before the first frame update
    void Start()
    {
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // 敵の攻撃が成功していたら実行する
            enemy.Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
