using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    CameraAction act_Camera;
    M_StateAction act_MState;
    Play_UI_Managet mng_PlayUI;
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

    [Header("Game設計データ")]
    [SerializeField, Tooltip("デッドゾーン"),Range(0,1)] float deadZone;
    [SerializeField, Tooltip("恵方巻の最大所持数")] int maxEhomaki;
    [SerializeField] bool isController;
    [SerializeField, Tooltip("スタミナバー")] Image img_Stamina;

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

    bool isFire;            //true: 発砲中
    bool isDash;
    bool test;


    //Component
    Rigidbody myRb;
    Animator myAnim;
    SkinnedMeshRenderer[] mySM_Renderer;
    Material myMaterial;
    
    public bool IsFire
    {
        get { return isFire; }
    }

    public int GetEhomaki
    {
        get { return cntEhomaki; }
    }

    public int GetBulletCnt
    {
        get { return cntBullet; }
    }

    // Start is called before the first frame update
    void Start()
    {
        myStamina = 1;

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

    private void Ready()
    {
        myHp = maxHp;
        myStamina = 1;
        cntBullet = 0;
        animSpeed = 1.0f;
        isDamage = false;
        isFire = false;
        elapsed.run = 0.0f;
        elapsed.invincivle = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !isDamage) //&& other.gameObject.GetComponent<EnemyAction>().IsAttack
        {
            myHp--;
            isDamage = true;
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
                    }
                    break;
            }
        }
    }

    public void SetMoveFlg(int value)
    {
        isFire = value == 0 ? false : true;
    }

    void FlashTest()
    {
        test = !test;
        elapsed.test += Time.deltaTime;
        if(elapsed.test >= 1.0f)
        {
            for(int i = 0;i < mySM_Renderer.Length;i++)
            {
                mySM_Renderer[i].enabled = test;
            }
            elapsed.test = 0.0f;
        }
    }



    // Update is called once per frame
    void Update()
    {
        inputData.x = 0;
        inputData.z = 0;

        if (!isFire)
        {
            if (isController)   //Pad
            {
                inputData.x = Input.GetAxis("Horizontal");
                inputData.z = Input.GetAxis("Vertical");
            }
            else                //キーボード
            {
                if (Input.GetKey(KeyCode.W))
                {
                    inputData.z += 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    inputData.z -= 1;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    inputData.x -= 1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    inputData.x += 1;
                }

                if(Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Debug.Log(myStamina);
                    if ((myStamina * 100) >= subtractionPersent)
                    {
                        myStamina -= subtractionPersent / 100.0f;
                        //elapsed.run += Time.deltaTime / toRunSecond;
                        isDash = true;
                        Debug.Log(myStamina);
                    }
                }
                else if(Input.GetKeyUp(KeyCode.LeftShift))
                {
                    isDash = false;
                }
            }
        }
        else
        {
            Vector3 lookPos = transform.position - cameraMaster.transform.position;
            transform.LookAt(lookPos);

            gameObject.transform.localEulerAngles = new Vector3(0, act_Camera.GetRotY, 0);
        }

        //if(!isFire)
        //{
        if (Input.GetMouseButton(0))    //発射
        {
            //発射の関数呼び出しはAnimaitonのEventでやってる
            isFire = true;
            myAnim.SetBool("Fire", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //isFire = false;
            myAnim.SetBool("Fire", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            fireMode = fireMode == FireMode.single ? FireMode.diffusion : FireMode.single;
        }
    }

    private void FixedUpdate()
    {
        if(isDamage)
        {
            elapsed.invincivle += Time.deltaTime;
            if(elapsed.invincivle >= invincivleTime)
            {
                isDamage = false;
                elapsed.invincivle = 0.0f;
            }
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


            if (isFire)
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


        img_Stamina.fillAmount = myStamina;

        dir.y = myRb.velocity.y;
        myRb.velocity = dir;
        
    }
}
