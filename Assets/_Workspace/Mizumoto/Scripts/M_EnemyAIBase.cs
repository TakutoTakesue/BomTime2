using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(M_StateAction))]
[RequireComponent(typeof(Rigidbody))]
public class M_EnemyAIBase : MonoBehaviour, StateCaller , SensingRangeCaller
{

    [SerializeField, Header("巡回する場所")]
    GameObject[] patrolPos = new GameObject[0];
    [SerializeField, Header("Rayに当たるレイヤーの選択")]
    LayerMask[] layerMask;
    [SerializeField, Header("通常状態のスピード")]
    float normalSpeed = 1.5f;
    [SerializeField, Header("ダッシュスピード（敵のことを見つけている状態のスピード）")]
    float dashSpeed = 2.0f;
    [SerializeField, Header("何メートル近づいたら目的地についたと判定するか")]
    float patrolDistance = 1;
    [SerializeField, Header("仰角制限")]
    float maxPitch = 0;
    [SerializeField, Header("俯角制限")]
    float minPitch = 0;
    [SerializeField, Header("見渡す時間")]
    float vigilantTime = 1;
    [SerializeField, Header("回転速度")]
    float rotateSpeed = 1;
    int mask = 0;   // 衝突するマスク

    // 自身の状態
    public enum State
    {
        normal, // プレイヤーを見つけていない
        discover, // 敵を見つけている状態
        vigilant, // プレイヤーを見失ったがそこまで時間が経過していない場合
        stop,   // 停止状態
    };

    Transform targetPos;    // 目標地点
    NavMeshAgent myNavi; // 自身のナビメッシュ
    GameObject player; // player
    Rigidbody myRB; // 自身のリジッドボディ
    M_StateAction enemyState;   // 自身の体力などのステータス
    State state;    // 自身の状態
    int patrolNo = 0;   // 巡回する順番
    
    // Start is called before the first frame update

    private void Awake()
    {
        targetPos = null;
        myNavi = GetComponent<NavMeshAgent>();
        enemyState = GetComponent<M_StateAction>();
        myRB = GetComponent<Rigidbody>();
        myRB.useGravity = false;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        foreach (var i in layerMask)
        {
            mask = mask | 1 << i.value;
        }
        myNavi.speed = normalSpeed;
    }

    // 目的地に到着した場合の処理
    void PatrolArrival()
    {
        targetPos = null;
        ++patrolNo;
        patrolNo %= patrolPos.Length;   // 巡回場所の配列の数よりも大きくはなってはいけないため超えるときは0に戻る
    }

    // 死亡処理
    public void CallDead()
    {
        Destroy(gameObject);
    }

    public void CallDiscover() {
        myNavi.enabled = true;
        myNavi.speed = dashSpeed;
        state = State.discover; // 敵を発見
    }

    // 見渡す
    IEnumerator Overlooking() {
        myNavi.enabled = false;
        Vector3 rot = Vector3.zero;
        float speed = rotateSpeed;
        var rightForward = transform.localEulerAngles;
        for (float totalTime = 0; vigilantTime > totalTime; totalTime += 0.01f)
        {
            rot.y += speed;
            if (rot.y > maxPitch && speed > 0)
            {
                speed = -speed;
            }
            else if (rot.y < minPitch && speed < 0)
            {
                speed = -speed;
            }

            transform.localEulerAngles = rot + rightForward;
           yield return new WaitForSeconds(0.01f);
        }
        myNavi.enabled = true;
        yield break;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (myNavi.enabled)
        {
            switch (state)
            {
                case State.normal:
                    // 巡回するポジションを入れ忘れていないなら
                    if (patrolPos.Length > 0)
                    {
                        // 今の目的地がないなら目的地を作る                
                        targetPos = patrolPos[patrolNo].transform;
                        // 巡回場所までの距離が一定以下になった場合目的地を次に進める
                        if (Vector3.Distance(patrolPos[patrolNo].transform.position, transform.position) <= patrolDistance)
                        {
                            var overlookingAction = patrolPos[patrolNo].GetComponent<M_PatrolPosAction>();
                            if (overlookingAction) {
                                if (overlookingAction.OverlookingFlg)
                                {
                                    StartCoroutine(Overlooking());
                                }
                            }
                            PatrolArrival();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("patrolPosのLengthが0です代入してください");
                    }
                    break;
                case State.discover:
                    targetPos = player.transform; //ナビメッシュが有効なら目的地へ

                    Ray targetRay = new Ray(transform.position + Vector3.up, player.transform.position - transform.position);
                    RaycastHit hit;
                    if (Physics.Raycast(targetRay, out hit, mask))
                    {

                        if (!hit.collider.CompareTag("Player"))
                        {
                            Debug.Log(hit.collider.name);
                        }
                    }
                    Debug.DrawRay(targetRay.origin, targetRay.direction * 10, Color.red, 1, true);
                    break;
                case State.vigilant:
                    break;
                case State.stop:
                    // 何もしない
                    break;
            }

            if (targetPos != null)
            {
                // 今の目的地がないなら目的地を作る                
                myNavi.SetDestination(targetPos.transform.position);
            }
            

        }
    }
}
