﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    struct InputData
    {
        public float x, z;
    }
    InputData inputData;

    Rigidbody myRb;

    [SerializeField] GameObject cameraMaster;

    [Header("PlayerのParamater")]
    [SerializeField,Tooltip("最大体力")] int maxHp;
    [SerializeField,Tooltip("歩き速度")] float runSpeed;
    [SerializeField,Tooltip("走り速度")] float walkSpeed;

    [Header("Game設計データ")]
    [SerializeField, Tooltip("デッドゾーン")] float deadZone; 

    [SerializeField] bool isController;

    //Playerの内部データ
    int myHp;
    Vector3 dir;
    
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isController)   //Pad
        {
            inputData.x = Input.GetAxis("Horizontal");
            inputData.z = Input.GetAxis("Vertical");
        }
        else                //キーボード
        {
            inputData.x = 0;
            inputData.z = 0;
            if (Input.GetKey(KeyCode.W))
            {
                inputData.z += 1;
            }
            if(Input.GetKey(KeyCode.S))
            {
                inputData.z -= 1;
            }
            if(Input.GetKey(KeyCode.A))
            {
                inputData.x -= 1;
            }
            if(Input.GetKey(KeyCode.D))
            {
                inputData.x += 1;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 axisDirV = Vector3.Scale(cameraMaster.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 axisDirH = Vector3.Scale(cameraMaster.transform.right, new Vector3(1, 0, 1)).normalized;

        dir = axisDirV * inputData.z + axisDirH * inputData.x;

        dir = dir.normalized;

        if (Mathf.Abs(inputData.x) > deadZone || Mathf.Abs(inputData.z) > deadZone)
        {
            transform.rotation = Quaternion.LookRotation(dir);

            if(Input.GetKey(KeyCode.LeftShift))// || Input.GetButton("BtnA")
            {
                dir *= runSpeed;
            }
            else
            {
                dir *= walkSpeed;
            }
        }

        dir.y = myRb.velocity.y;
        myRb.velocity = dir;
    }
}