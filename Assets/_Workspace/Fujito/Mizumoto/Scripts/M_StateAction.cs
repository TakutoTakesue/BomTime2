using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface StateCaller : IEventSystemHandler
{
    // イベントを呼び出すメソッド
    // 死亡通知用関数
    void CallDead();
}


public class M_StateAction : MonoBehaviour
{
    [SerializeField, Header("HPの最大値")]
    int hpmax;
    public int HPMax => hpmax;
    [SerializeField, Header("攻撃力")]
    int attack;
    public int Attack => attack;
    int hp; // 自身の体力
    public int HP => hp;

   
    // Start is called before the first frame update
    void Start()
    {
        hp = hpmax;
    }

    public void OnDamage(int damage)
    {
        // 死んでる場合はリターン
        if (hp <= 0) {
            return;
        }
        hp -= Mathf.Clamp(damage, 0, hp);
        if (hp <= 0)
        {
            // 死んだ場合は死亡処理
            Dead();
        }
    }
    // 回復する処理
    public void Recovery(int recovery)
    {
        hp += Mathf.Clamp(recovery, 0, hpmax);
    }

    // 死んだ場合は自身に通達して後は自分に処理させる
    void Dead()
    {
        ExecuteEvents.Execute<StateCaller>(
                        target: gameObject,
                        eventData: null,
                        functor: CallMyDead
                        );
    }

    void CallMyDead(StateCaller inf, BaseEventData eventData)
    {
        // 自分に対して死亡を通達
        inf.CallDead();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
