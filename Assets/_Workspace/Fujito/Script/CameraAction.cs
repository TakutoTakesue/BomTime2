using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    PlayerAction act_Player;

    struct PAD
    {
        public float horizontal;
        public float vertical;
    }
    PAD padData;

    struct MOUSE
    {
        public float horizontal;
        public float vertical;
    }
    MOUSE mouseData;

    [SerializeField] GameObject obj_Target; //カメラの見つめる先
    [SerializeField] GameObject myCamera;   //カメラ

    [Header("カメラ詳細情報")]
    [SerializeField, Tooltip("仰角制限")] float maxPitch;   //仰角制限
    [SerializeField, Tooltip("俯角制限")] float minPitch;   //俯角制限
    [SerializeField, Tooltip("感度")] Vector2 bias;         //カメラの感度
    [SerializeField, Tooltip("Controllerでのスピード調整"), Range(0.0f, 1.0f)] float padBias;
    [SerializeField] Vector3 offset;                        //rayの発射位置のオフセット

    float h;
    float v;
    Vector2 rot = Vector2.zero;
    Vector3 camStartPos;
    Vector3 dir;
    RaycastHit hitInfo;

    public float GetRotY
    {
        get { return rot.y; }
    }
    // Start is called before the first frame update
    void Start()
    {
        act_Player = obj_Target.GetComponent<PlayerAction>();
        camStartPos = myCamera.transform.localPosition;
    }

    private void CheckWall()
    {
        Vector3 myPos = gameObject.transform.position;
        Vector3 cameraPos = myCamera.transform.position;
        Vector3 cameraDirecation = cameraPos - myPos;

        Debug.DrawRay(myPos, cameraDirecation * 50);
        if (Physics.Raycast(myPos, cameraDirecation, out hitInfo, 100.0f))
        {
            if (hitInfo.collider.gameObject.tag != "MainCamera")
            {
                myCamera.transform.position = hitInfo.point;
            }
            else
            {

            }
        }
    }

    void CheckBarrier()
    {
        //Larpの速度
        var rate = 3 * Time.deltaTime;
        //壁判定用
        RaycastHit hit;
        //元の位置と今の距離がどのぐらい離れているか。
        if ((myCamera.transform.localPosition - camStartPos).sqrMagnitude > 0.001f)
        {
            //元の位置に戻るLarp
            myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, camStartPos, rate);
        }
        else if (myCamera.transform.localPosition != camStartPos)//近づいたら元の位置に。
        {
            myCamera.transform.localPosition = camStartPos;
        }
        //壁判定
        if (Physics.Linecast(gameObject.transform.position + offset, myCamera.transform.position, out hit))
        {
            //カメラを当たった場所に。
            myCamera.transform.localPosition = gameObject.transform.InverseTransformPoint(hit.point);
        }
    }


    // Update is called once per frame
    void Update()
    {
        mouseData.horizontal = Input.GetAxis("Mouse X");
        mouseData.vertical = -Input.GetAxis("Mouse Y");

        padData.horizontal = Input.GetAxis("Horizontal_R") * padBias;
        padData.vertical = Input.GetAxis("Vertical_R") * padBias;

        h = (Mathf.Abs(padData.horizontal) < Mathf.Abs(mouseData.horizontal)) ? mouseData.horizontal : padData.horizontal;
        v = (Mathf.Abs(padData.vertical) < Mathf.Abs(mouseData.vertical)) ? mouseData.vertical : padData.vertical;

        h *= bias.x;
        v *= bias.y;

        rot += new Vector2(v, h);

        rot.x = Mathf.Clamp(rot.x, minPitch, maxPitch);

        if (rot.y > 180.0f)
        {
            rot.y = -180.0f;
        }
        else if (rot.y < -180.0f)
        {
            rot.y = 180.0f;
        }

        transform.localEulerAngles = rot;
        transform.position = obj_Target.gameObject.transform.position;

        CheckBarrier();
    }
}