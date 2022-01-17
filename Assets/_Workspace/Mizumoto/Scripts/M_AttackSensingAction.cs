using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface AttackCaller : IEventSystemHandler
{
    // イベントを呼び出すメソッド
    // 死亡通知用関数
    void CallAttack();
}


public class M_AttackSensingAction : MonoBehaviour
{
    [SerializeField, Header("誰に対して攻撃と通達するか")]
    GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Attack()
    {
        ExecuteEvents.Execute<AttackCaller>(
                        target: enemy,
                        eventData: null,
                        functor: CallMyAttack
                        );
    }

    void CallMyAttack(AttackCaller inf, BaseEventData eventData)
    {
        // 自分に対して死亡を通達
        inf.CallAttack();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
