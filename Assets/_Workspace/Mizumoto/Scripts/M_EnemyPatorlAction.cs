using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_EnemyPatorlAction : MonoBehaviour
{
    [SerializeField, Header("敵の徘徊ルートの分別")]
    int patrolNo = 0;
    // 受け取り用
    public int PatrolNo { get {return patrolNo;}}
    [SerializeField, Header("敵の徘徊ルートの順番")]
    int patrolOrder = 0; // 0番目 1番目 2番目の順番で動いたりして次の番号がなくなった場合0に戻る
    public int PatrolOrder { get { return patrolOrder; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
