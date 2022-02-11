using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectAction : MonoBehaviour
{
    public Button stage1;
    public Button stage2;
    public Button stage3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Stage1_onclick()
    {
        //AudioManager.Instance.StopBgm(2.0f);
        SceneManager.LoadScene("Stage1");
    }

    void Stage2_onclick()
    {
        //AudioManager.Instance.StopBgm(2.0f);
        SceneManager.LoadScene("Stage2");
    }

    void Stage3_onclick()
    {
        //AudioManager.Instance.StopBgm(2.0f);
        SceneManager.LoadScene("Stage3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
