using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    CameraAction act_Camera;
    struct InputData
    {
        public float x, z;
    }
    InputData inputData;

    struct Elapsed
    {
        public float fire;
    }
    Elapsed elapsed;


    [Header("各オブジェクト")]
    [SerializeField] GameObject obj_Bullet;
    [SerializeField] Transform tForm_Shoot;
    [SerializeField] GameObject cameraMaster;

    [Header("PlayerのParamater")]
    [SerializeField,Tooltip("最大体力")] int maxHp;
    [SerializeField,Tooltip("歩き速度")] float runSpeed;
    [SerializeField,Tooltip("走り速度")] float walkSpeed;
    [SerializeField, Tooltip("銃発射間隔(Frame)")] int interval;

    [Header("Game設計データ")]
    [SerializeField, Tooltip("デッドゾーン")] float deadZone; 

    [SerializeField] bool isController;

    //Playerの内部データ
    int myHp;
    Vector3 dir;

    bool isFire;

    public bool IsFire
    {
        get { return isFire; }
    }

    //Component
    Rigidbody myRb;
    Animator myAnim;


    // Start is called before the first frame update
    void Start()
    {
        act_Camera = cameraMaster.GetComponent<CameraAction>();
        myRb = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }

    void Fire()
    {
        Instantiate(obj_Bullet, tForm_Shoot.position, gameObject.transform.rotation);
        elapsed.fire = 0;
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

        if (Input.GetMouseButton(0))
        {

            isFire = true;
            if (elapsed.fire >= interval)
            {
                Fire();
            }
            myAnim.SetBool("Fire", true);
        }
        else
        {
            isFire = false;
            elapsed.fire = interval;
            myAnim.SetBool("Fire", false);
        }
    }

    private void FixedUpdate()
    {
        elapsed.fire++;

        Vector3 axisDirV = Vector3.Scale(cameraMaster.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 axisDirH = Vector3.Scale(cameraMaster.transform.right, new Vector3(1, 0, 1)).normalized;

        dir = axisDirV * inputData.z + axisDirH * inputData.x;

        dir = dir.normalized;

        if (Mathf.Abs(inputData.x) > deadZone || Mathf.Abs(inputData.z) > deadZone)
        {
            transform.rotation = Quaternion.LookRotation(dir);
            myAnim.SetFloat("Speed", 1);
            if(Input.GetKey(KeyCode.LeftShift))// || Input.GetButton("BtnA")
            {
                dir *= runSpeed;
            }
            else
            {
                dir *= walkSpeed;
            }
        }
        else
        {
            myAnim.SetFloat("Speed", 0);
        }

        dir.y = myRb.velocity.y;
        myRb.velocity = dir;
    }
}
