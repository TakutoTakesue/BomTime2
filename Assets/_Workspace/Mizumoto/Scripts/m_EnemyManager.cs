using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_EnemyManager : MonoBehaviour
{
    int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        var enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemys){
            ++enemyCount;
        }
        if (enemyCount == 0) {
            // ゲームマネージャーにゲームを終了する処理を送る
        }
    }

    public void EnemyDead() {
        --enemyCount;
        if (enemyCount <= 0) {
            // ゲームマネージャーにゲームを終了する処理を送る
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
