using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleAction : MonoBehaviour
{
    public Text txtNavi;
    float Elapsed;

    float[] Rank = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        txtNavi.text = "Press A Start";

        if (PlayerPrefs.HasKey("R1"))
        {
            Debug.Log("データ領域を読み込みました。");
            for (int idx = 1; idx <= 5; idx++)
            {
                Rank[idx] = PlayerPrefs.GetFloat("R" + idx); // データ領域読み込み
            }
        }
        else
        {
            Debug.Log("データ領域を初期化");
            for (int idx = 1; idx <= 5; idx++)
            {
                Rank[idx] = float.MaxValue;
                PlayerPrefs.SetFloat("R" + idx, float.MaxValue); // 最大値を格納する
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Elapsed += Time.deltaTime;

        Elapsed %= 1.0f;
        txtNavi.text = (Elapsed < 0.8f) ? "Press A Start" : "";

        if (Input.GetButtonDown("BtnA"))
        {
            SceneManager.LoadScene("Select");
        }

        //データ領域の初期化
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("データ領域を削除しました。");
        }
    }
}
