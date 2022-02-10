using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AnimScript : MonoBehaviour
{
    [SerializeField, Header("攻撃処理を持ったオブジェクト")]
    GameObject attackObj;
    [SerializeField, Header("何らかの理由で攻撃範囲が消えなかった場合何秒後に消すか")]
    float attackInterval = 0.5f;
    float elapsed = 0;  // 経過時間
    bool attackFlg = false; // 攻撃しているかどうか
    // Start is called before the first frame update
    void Start()
    {
        attackObj.SetActive(false);
    }

    // 攻撃開始
    public void BginAttackAnim()
    {
        attackFlg = true;
        AudioManager.Instance.StartSe("EnemyAttack_SE");
        // 攻撃はじめ
        attackObj.SetActive(true);
    }

    // 攻撃終了
    public void EndAttackAnim()
    {
        attackFlg = false;
        // 攻撃終わり
        attackObj.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        // 攻撃処理中なら
        if (attackFlg) {
            // 経過時間を0かelapsed - Time.deltaTimeの値にする
            elapsed = Mathf.Min(elapsed - Time.deltaTime, 0);
            // もし0になったら自身を描画しなくする
            if (elapsed == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
