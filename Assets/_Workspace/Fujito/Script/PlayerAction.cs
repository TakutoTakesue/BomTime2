using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    CameraAction act_Camera;
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


    [Header("各オブジェクト")]
    [SerializeField, Tooltip("弾のPrefab")] GameObject obj_Bullet;
    [SerializeField, Tooltip("弾の発射位置")] Transform tForm_Shoot;
    [SerializeField, Tooltip("カメラの親Empty")] GameObject cameraMaster;

    [Header("PlayerのParamater")]
    [SerializeField, Tooltip("最大体力")] int maxHp;
    [SerializeField, Tooltip("歩き速度")] float runSpeed;
    [SerializeField, Tooltip("走り速度")] float walkSpeed;
    [SerializeField, Tooltip("歩き→走り速度になるまでの時間(S)")] float toRunSecond;
    //[SerializeField, Tooltip("銃発射間隔(F)")] int interval;
    [SerializeField, Tooltip("被弾時の無敵時間(S)")] float invincivleTime;

    [Header("Game設計データ")]
    [SerializeField, Tooltip("デッドゾーン")] float deadZone; 

    [SerializeField] bool isController;

    //Playerの内部データ
    float animSpeed;        //走るときのAnimationスピード
    int myHp;               //現在のHP
    bool isDamage;          //被弾中か

    Vector3 dir;            //進む方向ベクトル
    GameObject obj_Copy;    //Materialをコピーする用のインスタンスオブジェクト


    bool isFire;            //true: 発砲中
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

    // Start is called before the first frame update
    void Start()
    {
        obj_Copy = Instantiate(gameObject);
        obj_Copy.SetActive(false);      //コピーを作って隠す
        animSpeed = 0.0f;

        act_Camera = cameraMaster.GetComponent<CameraAction>();
        myRb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        mySM_Renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        myMaterial = obj_Copy.GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    private void Ready()
    {
        myHp = maxHp;
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
        Instantiate(obj_Bullet, tForm_Shoot.position, gameObject.transform.rotation);
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
            }
        }
        else
        {
            Vector3 lookPos = transform.position - cameraMaster.transform.position;
            transform.LookAt(lookPos);

            gameObject.transform.localEulerAngles = new Vector3(0, act_Camera.GetRotY, 0);
        }

        if (Input.GetMouseButtonDown(0))    //発射
        {
            //発射の関数呼び出しはAnimaitonのEventでやってる
            isFire = true;
            myAnim.SetBool("Fire", true);
        }
        if(Input.GetMouseButtonUp(0))
        {
            isFire = false;
            myAnim.SetBool("Fire", false);
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
            if(Input.GetKey(KeyCode.LeftShift))// || Input.GetButton("BtnA")
            {
                elapsed.run += Time.deltaTime / toRunSecond;
            }
            else
            {
                elapsed.run -= Time.deltaTime / toRunSecond;
            }

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
            elapsed.run = 0.0f;
            animSpeed = 1.0f;
            myAnim.SetFloat("Speed", 0);
        }

        dir.y = myRb.velocity.y;
        myRb.velocity = dir;
    }
}
