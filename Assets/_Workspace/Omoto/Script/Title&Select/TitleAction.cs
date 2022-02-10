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
    }
}
