using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveTitleScene()
    {
        SceneManager.LoadScene("1_Title");
    }
    public void MoveBossScene()
    {
        SceneManager.LoadScene("3_Boss");
    }
}
