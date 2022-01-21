using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(M_StateAction))]
[RequireComponent(typeof(Rigidbody))]

public class M_EnemyAIBase : MonoBehaviour, StateCaller, SensingRangeCaller, AttackCaller
{

    [SerializeField, Header("通常状態のスピード")]
    float normalSpeed = 1.5f;
    [SerializeField, Header("ダッシュスピード（敵のことを見つけている状態のスピード）")]
    float dashSpeed = 2.0f;
    [SerializeField, Header("巡回する場所")]
    Transform[] patrolPos = new Transform[0];
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
    [SerializeField, Header("敵を何秒見つけられなかったら敵を見失うか")]
    float lostTagertTime = 5;
    [SerializeField, Header("敵を攻撃したときのインターバル")]
    float attackInterval = 1;
    [SerializeField, Header("死亡時何秒で消えるか")]
    float deathInterval = 1.5f;
    [SerializeField, Header("Rayに当たるレイヤーの選択")]
    LayerMask[] layerMask;
    // 経過時間をまとめたもの
    struct Elapsed
    {
        public float lostTimeElapsed;
        public float attackTimeElapsed;
        public void ElapsedReset()
        {
            lostTimeElapsed = 0;
            attackTimeElapsed = 0;
        }

    }
    Elapsed elapsed;
    Animator myAnim;   // 自身のアニメーション
    IEnumerator overlooking;    // 見渡すためのコール―チン
    int mask = 0;   // 衝突するマスク
    bool dashFlg = false;   // 敵がダッシュしているかどうか
    bool overlookingFlg = false;   // 敵がダッシュしているかどうか
    bool deathFlg = false;  // 死亡しているか


    // 自身の状態
    public enum State
    {
        normal, // プレイヤーを見つけていない
        discover, // 敵を見つけている状態
        vigilant, // プレイヤーを見失ったがそこまで時間が経過していない場合
        stop,   // 停止状態
    };

    Vector3 targetPos;    // 目標地点
    NavMeshAgent myNavi; // 自身のナビメッシュ
    GameObject player; // player
    Rigidbody myRB; // 自身のリジッドボディ
    M_StateAction enemyState;   // 自身の体力などのステータス
    State state;    // 自身の状態
    int patrolNo = 0;   // 巡回する順番

    // Start is called before the first frame update

    private void Awake()
    {
        targetPos = Vector3.zero;
        myNavi = GetComponent<NavMeshAgent>();
        enemyState = GetComponent<M_StateAction>();
        myRB = GetComponent<Rigidbody>();
        myRB.useGravity = false;
        elapsed.ElapsedReset();
    }

    void Start()
    {
        GameObject demonGirlMesh = transform.Find("DemonGirlMesh").gameObject;
        myAnim = demonGirlMesh.GetComponent<Animator>();
        if (!myAnim) {
            Debug.LogWarning("myAnimの取得に失敗しました");
        }
        player = GameObject.FindWithTag("Player");
        foreach (var i in layerMask)
        {
            mask += i.value;
        }
        myNavi.speed = normalSpeed;
    }

    // 目的地に到着した場合の処理
    void PatrolArrival()
    {
        targetPos = Vector3.zero;
        ++patrolNo;
        patrolNo %= patrolPos.Length;   // 巡回場所の配列の数よりも大きくはなってはいけないため超えるときは0に戻る
    }

    // 死亡処理
    public void CallDead()
    {
        if (deathFlg)
        {
            return;
        }
        myAnim.SetTrigger("Die");
        deathFlg = true;
        StopOverlooking();
        var enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
        Destroy(gameObject, deathInterval);
    }

    // ダメージを受けた時の処理
    public void CallDamage()
    {
        CallDiscover();
    }

    public void CallAttack()
    {
        if (deathFlg) {
            return;
        }
        if (elapsed.attackTimeElapsed <= 0 && state == State.discover)
        {
            myNavi.enabled = false;
            myAnim.SetTrigger("Attack");
            elapsed.attackTimeElapsed = attackInterval;
        }
    }

    // 敵を発見したとき
    public void CallDiscover()
    {
        myNavi.enabled = true;
        myNavi.speed = dashSpeed;
        dashFlg = true;
        targetPos = player.transform.position;
        StopOverlooking();
        state = State.discover; // 敵を発見
    }

    // Overlookingを一時停止するときに呼び出す
    void StopOverlooking()
    {
        if (overlooking != null)
        {
            StopCoroutine(overlooking);
            myNavi.enabled = true;
            overlookingFlg = false;
        }
    }

    // 見渡す
    IEnumerator Overlooking()
    {
        overlookingFlg = true;
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
        dashFlg = false;
        myNavi.enabled = true;
        myNavi.speed = normalSpeed;
        overlookingFlg = false;
        yield break;
    }

    protected void OnTriggerEnter(Collider other) {
        if (other.tag == "Bullet") {
            Destroy(other.gameObject);

            var bulletScript = other.GetComponent<BulletAction>();
            if (bulletScript)
            {
                Debug.Log("ダメージを受けた");
                enemyState.OnDamage(bulletScript.GetPower);
            }
            else {
                Debug.LogWarning("BulletがScriptを持っていません");
            }

        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CallDead();
        }
        if (deathFlg) {
            return;
        }
        if (elapsed.attackTimeElapsed > 0)
        {
            elapsed.attackTimeElapsed -= Time.deltaTime;
            myNavi.enabled = true;
            return;
        }
        if (myNavi.enabled)
        {
            switch (state)
            {
                case State.normal:
                    // 巡回するポジションを入れ忘れていないなら
                    if (patrolPos.Length > 0)
                    {
                        // 今の目的地がないなら目的地を作る                
                        targetPos = patrolPos[patrolNo].transform.position;
                        // 巡回場所までの距離が一定以下になった場合目的地を次に進める
                        if (Vector3.Distance(patrolPos[patrolNo].transform.position, transform.position) <= patrolDistance)
                        {
                            var overlookingAction = patrolPos[patrolNo].GetComponent<M_PatrolAction>();
                            if (overlookingAction)
                            {
                                if (overlookingAction.OverlookingFlg)
                                {
                                    overlooking = Overlooking();
                                    StartCoroutine(overlooking);
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
                    Ray targetRay = new Ray(transform.position + Vector3.up / 2, player.transform.position - transform.position);
                    //myNavi.SetDestination(player.transform.position); //ナビメッシュが有効なら目的地へ
                    RaycastHit hit;
                    if (Physics.Raycast(targetRay, out hit, 10, mask))
                    {
                        // playerにRayが当たっていたらプレイヤーを見つける
                        targetPos = player.transform.position;
                        elapsed.lostTimeElapsed = 0;
                    }
                    else
                    {
                        // プレイヤーを見失った
                        elapsed.lostTimeElapsed += Time.deltaTime;
                        // 見失った時間が基底時間以上なら見失う
                        if (elapsed.lostTimeElapsed >= lostTagertTime)
                        {
                            elapsed.lostTimeElapsed = 0;
                            state = State.normal;
                            overlooking = Overlooking();
                            StartCoroutine(overlooking);
                            targetPos = Vector3.zero;
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

            if (targetPos != Vector3.zero)
            {
                Debug.Log(targetPos + "targetpos");
                // 今の目的地がないなら目的地を作る                
                myNavi.SetDestination(targetPos);
            }

            // アニメーション
            myAnim.SetFloat("Speed", myNavi.speed);
            myAnim.SetBool("Dash", dashFlg);

        }
        else {
            // アニメーション
            myAnim.SetFloat("Speed", 0);
            myAnim.SetBool("Dash", false);
        }
        myAnim.SetBool("Overlooking", overlookingFlg);
    }
}
