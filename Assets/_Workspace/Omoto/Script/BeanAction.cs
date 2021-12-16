using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*フィールドに生成されるアイテム　拾われたとき*/

public class BeanAction : MonoBehaviour
{ 
    GameObject Gun;

    public float DestoryCnt=30.0f;

    // Start is called before the first frame update
    void Start()
    {
        Gun = GameObject.FindGameObjectWithTag("Gun");

        // 30秒経過で削除　(外してもいいかなぁ)
        Destroy(gameObject, DestoryCnt);
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーと触れたとき　(ゲットされたら)
        if (other.gameObject.tag == "Player")
        {
            // 銃の弾の数を増やす
            Gun.SendMessage("Bullet_Add",SendMessageOptions.DontRequireReceiver);

            // 自分は消える
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
