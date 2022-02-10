using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_EnemyManager : MonoBehaviour
{
    int enemyCount = 0;
    int EnemyCount => enemyCount;
    Play_TimeManager timeManager;
    // Start is called before the first frame update
    void Start()
    {
        var enemys = GameObject.FindGameObjectsWithTag("Enemy");
        var obj = GameObject.FindGameObjectWithTag("PlayManager");
        if (obj != null)
        {
            timeManager = obj.GetComponent<Play_TimeManager>();
        }
        else {
            Debug.LogWarning("PlayManagerが存在しません");
        }
        foreach (var enemy in enemys){
            ++enemyCount;
        }
        if (enemyCount == 0) {
            GameEnd();
        }
    }

    public void EnemyDead() {
        --enemyCount;
        if (enemyCount <= 0) {
            GameEnd();
        }
    }

    // ゲームマネージャーにゲームを終了する処理を送る
    void GameEnd() {
        if (timeManager)
        {
            AudioManager.Instance.StartSe("End_SE");
            timeManager.GameEnd();
            Debug.Log("ゲーム終了");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
