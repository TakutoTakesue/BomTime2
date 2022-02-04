using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    public GameObject Bean;    // 装弾数追加用の豆
    public GameObject HealItem;// 回復アイテム

    // 生成をストップするか
    public bool MakeStop = false;

    public GameObject floor;
    MeshCollider floorCollider;

    // 回復アイテムの確率
    public float Healitems_probability = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        floorCollider = floor.GetComponent<MeshCollider>();

        //生成処理を起動
        StartCoroutine("BeanMake");

    }

    //コイン生成処理
    IEnumerator BeanMake()
    {
        float Mapmin_x = floorCollider.bounds.min.x;
        float Mapmax_x = floorCollider.bounds.max.x;
        float Mapmin_z = floorCollider.bounds.min.z;
        float Mapmax_z = floorCollider.bounds.max.z;

        while (true)
        {
            if (!MakeStop)
            {
                //ランダム時間を待機
                yield return new WaitForSeconds(Random.Range(1.0f,1.5f));

                // マップ 0～指定した広さまでの範囲にランダムに生成 高さyは固定
                float RanX = Random.Range(Mapmin_x, Mapmax_x);
                float RanZ = Random.Range(Mapmin_z, Mapmax_z);

                //生成位置決定
                Vector3 pos = new Vector3(RanX, 0.5f, RanZ);

                //確率で回復アイテム位置
                if (Random.value < Healitems_probability)
                {
                    Instantiate(HealItem, pos, Quaternion.identity);
                }
                else
                {
                    Instantiate(Bean, pos, Quaternion.identity);
                }
             
            }
        }
    }

    // ゲーム終了時などの全撤去
void ClearBeans()
    {
        // フィールド上に存在する豆を探して削除
        GameObject[]　Beans = GameObject.FindGameObjectsWithTag("Bean");
        foreach (GameObject Stored in Beans)
        {
            Destroy(Stored);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
