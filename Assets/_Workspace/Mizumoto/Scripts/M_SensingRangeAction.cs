using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface SensingRangeCaller : IEventSystemHandler
{
    // 敵を発見
    void CallDiscover();
}



public class M_SensingRangeAction : MonoBehaviour
{
    [SerializeField, Header("誰に対してプレイヤーを見つけたと通達するか")]
    GameObject enemy;
    //[SerializeField, Header("目の場所")]
    //Transform eyepos;
    [SerializeField, Header("Rayに当たるレイヤーの選択")]
    LayerMask[] layerMask;
    int mask = 0;   // 衝突するマスク
    // Start is called before the first frame update
    void Start()
    {
        foreach (var i in layerMask)
        {
            mask += i.value;
        }
    }

   
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("プレイヤー侵入");
            Ray ray = new Ray(enemy.transform.position + Vector3.up / 2, other.transform.position - enemy.transform.position);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1, true);
            if (Physics.Raycast(ray, out hit, 15, mask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Discover();    // 敵の発見を通達
                }
            }
        }
    }

    void Discover()
    {
        ExecuteEvents.Execute<SensingRangeCaller>(
                        target: enemy,
                        eventData: null,
                        functor: CallMyDiscover
                        );
    }
    void CallMyDiscover(SensingRangeCaller inf, BaseEventData eventData)
    {
        // 敵の発見
        inf.CallDiscover();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
