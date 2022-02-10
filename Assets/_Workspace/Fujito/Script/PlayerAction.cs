using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class PlayerAction : MonoBehaviour,StateCaller
{
    CameraAction act_Camera;
    M_StateAction act_MState;
    Play_UI_Manager mng_PlayUI;
    struct InputData
    {
        public float x, z;  //移動入力の値
    }
    InputData inputData;

    struct Elapsed  //それぞれの経過時間変数
    {
        //public float fire;  //発射の間隔用
        public float run;   //歩き→走りを滑らかにする用 (0~1)
        public float invincivle;    //被弾時の点滅用
        public float test;
    }
    Elapsed elapsed;

    enum FireMode
    {
        single,
        diffusion,
    }
    [Header("Diffusion: 拡散撃ち")]
    [Header("Single: 単発撃ち")]
    [Header("発射のMode")]
    [SerializeField] FireMode fireMode;

    [Header("各オブジェクト")]
    [SerializeField, Tooltip("弾のPrefab")] GameObject obj_Bullet;
    [SerializeField, Tooltip("弾が向かうTransform")] Transform[] tForm_ToShoot;
    [SerializeField, Tooltip("カメラの親Empty")] GameObject cameraMaster;
    [SerializeField, Tooltip("弾の発射Position")] GameObject shootPos;

    [Header("PlayerのParamater")]
    [SerializeField, Tooltip("最大体力")] int maxHp;
    [SerializeField, Tooltip("歩き速度")] float runSpeed;
    [SerializeField, Tooltip("走り速度")] float walkSpeed;
    [SerializeField, Tooltip("歩き→走り速度になるまでの時間(S)")] float toRunSecond;
    [SerializeField, Tooltip("走り→歩き速度になるまでの時間(S)")] float toWalkSecond;
    [SerializeField, Tooltip("スタミナが空になるまでの時間(S)")] float subtractionStamina;
    [SerializeField, Tooltip("スタミナが満タンになる時間(S)")] float addStamina;
    [SerializeField, Tooltip("ダッシュ時にスタミナの減る割合(%)")] float subtractionPersent;
    [SerializeField, Tooltip("被弾時の無敵時間(S)")] float invincivleTime;
    [SerializeField, Tooltip("弾の初期装弾数")] int startBulletCnt;

    [Header("Game設計データ")]
    [SerializeField, Tooltip("デッドゾーン"), Range(0, 1)] float deadZone;
    [SerializeField, Tooltip("恵方巻の最大所持数")] int maxEhomaki;
    [SerializeField, Tooltip("スタミナバー")] Image img_Stamina;
    [SerializeField, Tooltip("豆を拾った時に増える弾数")] int plusBulletNum;

    //Playerの内部データ
    float animSpeed;        //走るときのAnimationスピード
    int myHp;               //現在のHP
    bool isDamage;          //被弾中か
    int cntBullet;          //弾の所持数
    int cntEhomaki;         //恵方巻の所持数
    Vector3 fireOffset = new Vector3(0.227f, 1.541f, 0);
    float myStamina;

    Vector3 dir;            //進む方向ベクトル
    GameObject obj_Copy;    //Materialをコピーする用のインスタンスオブジェクト

    bool canMove;
    bool isFire;            //true: 発砲中
    bool isDead;
    bool isDash;
    bool isInvinsivle;
    bool padIsFire;
    bool padIsDash;


    //Component
    Rigidbody myRb;
    Animator myAnim;
    SkinnedMeshRenderer[] mySM_Renderer;
    Material myMaterial;


    public bool IsFire
    {
        get { return isFire; }
    }

    public bool IsDamage
    {
        get { return isDamage; }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    public int GetEhomaki
    {
        get { return cntEhomaki; }
    }

    //GetComponent<PlayerAction>().GetBulletCnt これで弾の数を取得できます
    public int GetBulletCnt
    {
        get { return cntBullet; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Ready();
        obj_Copy = Instantiate(gameObject);
        obj_Copy.SetActive(false);      //コピーを作って隠す
        animSpeed = 0.0f;

        act_Camera = cameraMaster.GetComponent<CameraAction>();
        act_MState = GetComponent<M_StateAction>();

        myRb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        mySM_Renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        myMaterial = obj_Copy.GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    public void CallDamage()
    {
        if(!isDamage)
        {
            StartCoroutine("Flash");
            myAnim.SetTrigger("Damage");
            isDamage = true;
            canMove = false;
        }
    }

    public void CallDead()
    {
        if(!isDamage && !IsDead)
        {
            AudioManager.Instance.StartSe("PlayerDown_SE");
            myAnim.SetTrigger("Dead");
            isDead = true;
        }
    }

    private void Ready()
    {
        myHp = maxHp;
        myStamina = 1;
        cntBullet = startBulletCnt;
        animSpeed = 1.0f;
        canMove = true;
        isDamage = false;
        isFire = false;
        isDead = false;
        padIsFire = false;
        padIsDash = false;
        elapsed.run = 0.0f;
        elapsed.invincivle = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bean")
        {
            cntBullet += plusBulletNum;
            AudioManager.Instance.StartSe("GetBullet_SE");
        }
        if(other.gameObject.tag == "Recoveryitem")
        {
            AudioManager.Instance.StartSe("Kaihuku_SE");
            act_MState.Recovery(1);
        }
    }

    IEnumerator Flash()
    {
        isInvinsivle = true;
        for (int i = 0; i < mySM_Renderer.Length; i++)
        {
            mySM_Renderer[i].enabled = false;
        }

        while (true)
        {
            elapsed.invincivle += Time.deltaTime;
            if (elapsed.invincivle % 0.2f >= 0.1f && isInvinsivle)
            {
                for (int i = 0; i < mySM_Renderer.Length; i++)
                {
                    mySM_Renderer[i].enabled = isInvinsivle;
                }
                isInvinsivle = false;
            }
            else if(elapsed.invincivle % 0.2f < 0.1f && !isInvinsivle)
            {
                for (int i = 0; i < mySM_Renderer.Length; i++)
                {
                    mySM_Renderer[i].enabled = isInvinsivle;
                }
                isInvinsivle = true;
            }

            if (elapsed.invincivle >= invincivleTime)
            {
                for (int i = 0; i < mySM_Renderer.Length; i++)
                {
                    mySM_Renderer[i].enabled = true;
                }
                isDamage = false;
                elapsed.invincivle = 0.0f;
                break;
            }

            yield return null;
        }
    }

    public void Fire()
    {
        if (cntBullet > 0)
        {
            switch (fireMode)
            {
                case FireMode.single:
                    GameObject bullet_0 = Instantiate(obj_Bullet, shootPos.transform.position, gameObject.transform.rotation);
                    bullet_0.transform.LookAt(tForm_ToShoot[0].position);
                    cntBullet--;
                    AudioManager.Instance.StartSe("Shot_SE");
                    break;
                case FireMode.diffusion:
                    if (cntBullet >= tForm_ToShoot.Length)
                    {
                        for (int i = 0; i < tForm_ToShoot.Length; i++)
                        {
                            GameObject bullet_1 = Instantiate(obj_Bullet, shootPos.transform.position, gameObject.transform.rotation);
                            bullet_1.transform.LookAt(tForm_ToShoot[i].position);
                            cntBullet--;
                        }
                        AudioManager.Instance.StartSe("Shot_SE");
                    }
                    else
                    {
                        AudioManager.Instance.StartSe("NoBullet_SE");
                    }
                    break;
            }
        }
        AudioManager.Instance.StartSe("NoBullet_SE");
    }
    public void SetCanMoveFlgFalse()
    {
        canMove = false;
    }

    public void SetCanMoveFlgTrue()
    {
        canMove = true;
    }

    public void SetMoveFlg(int value)
    {
        isFire = value == 0 ? false : true;
    }

    //void Flash()
    //{
    //    isInvinsivle = !isInvinsivle;
    //    elapsed.test += Time.deltaTime;
    //    if (elapsed.test >= 1.0f)
    //    {
    //        for (int i = 0; i < mySM_Renderer.Length; i++)
    //        {
    //            mySM_Renderer[i].enabled = isInvinsivle;
    //        }
    //        elapsed.test = 0.0f;
    //    }
    //}

    void OnDamage()
    {
        myAnim.SetTrigger("Damage");
        canMove = false;
        StartCoroutine("Flash");
    }


    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            dir.x = 0.0f;
            dir.z = 0.0f;
            return;
        }

        inputData.x = 0;
        inputData.z = 0;
        
        if (canMove)
        {
            inputData.x = Input.GetAxis("Horizontal");
            inputData.z = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if ((myStamina * 100) >= subtractionPersent)
                {
                    myStamina -= subtractionPersent / 100.0f;
                    isDash = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isDash = false;
            }

            if (Input.GetAxis("Trigger_L") > deadZone && !isDash)
            {
                if ((myStamina * 100) >= subtractionPersent)
                {
                    myStamina -= subtractionPersent / 100.0f;
                    isDash = true;
                    padIsDash = true;
                }
            }

            if (Input.GetAxis("Trigger_L") < deadZone && isDash && padIsDash)
            {
                isDash = false;
            }
        }
        else
        {
            isDash = false;
            padIsDash = false;
            Vector3 lookPos = transform.position - cameraMaster.transform.position;
            transform.LookAt(lookPos);

            gameObject.transform.localEulerAngles = new Vector3(0, act_Camera.GetRotY, 0);
        }

        if (Input.GetMouseButton(0) || Input.GetAxis("Trigger_R") > 0.6f)    //発射
        {
            //発射の関数呼び出しはAnimaitonのEventでやってる
            canMove = false;
            isFire = true;
            isDash = false;
            myAnim.SetBool("Fire", true);
            if(Input.GetAxis("Trigger_R") > 0.6f)
            {
                padIsFire = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            myAnim.SetBool("Fire", false);
        }

        if(padIsFire)
        {
            if(Input.GetAxis("Trigger_R") < deadZone)
            {
                myAnim.SetBool("Fire", false);
            }
        }

        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("BtnY"))
        {
            fireMode = fireMode == FireMode.single ? FireMode.diffusion : FireMode.single;
            AudioManager.Instance.StartSe("NoBullet_SE");  
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CallDamage();
        }
    }

    private void FixedUpdate()
    {
        if(isDead)
        {
            return;
        }

        
        //FlashTest();

        Vector3 axisDirV = Vector3.Scale(cameraMaster.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 axisDirH = Vector3.Scale(cameraMaster.transform.right, new Vector3(1, 0, 1)).normalized;

        dir = axisDirV * inputData.z + axisDirH * inputData.x;

        dir = dir.normalized;

        if (Mathf.Abs(inputData.x) > deadZone || Mathf.Abs(inputData.z) > deadZone)
        {
            transform.rotation = Quaternion.LookRotation(dir);

            if(isDash)
            {
                if (myStamina > 0.0f)
                {
                    elapsed.run += Time.deltaTime / toRunSecond;
                    myStamina -= Time.deltaTime / subtractionStamina;
                }
            }
            else
            {
                myStamina += Time.deltaTime / addStamina;
                elapsed.run -= Time.deltaTime / toWalkSecond;
            }

            if(myStamina <= 0.0f)
            {
                isDash = false;
            }

            myStamina = Mathf.Clamp01(myStamina);
            elapsed.run = Mathf.Clamp01(elapsed.run);
            dir *= Mathf.Lerp(walkSpeed, runSpeed, elapsed.run);

            animSpeed = Mathf.Lerp(1.0f, runSpeed / walkSpeed, elapsed.run);
            animSpeed = Mathf.Clamp(animSpeed, 1.0f, runSpeed / walkSpeed);


            if (!canMove)
            {
                animSpeed = 1.0f;
            }

            myAnim.SetFloat("Speed", animSpeed);
        }
        else
        {
            if (!isDash)
            {
                myStamina += Time.deltaTime / addStamina;
            }

            elapsed.run = 0.0f;
            animSpeed = 1.0f;
            myAnim.SetFloat("Speed", 0);
        }


        if(img_Stamina)
        {
            img_Stamina.fillAmount = myStamina;
        }

        if (!canMove)
        {
            dir = new Vector3(0.0f, 0.0f, 0.0f);
        }

        dir.y = myRb.velocity.y;
        myRb.velocity = dir;
        
    }
}
