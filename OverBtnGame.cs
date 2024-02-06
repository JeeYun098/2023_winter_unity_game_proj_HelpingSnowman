using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverBtnGame : MonoBehaviour
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
    public void MoveGameScene()
    {
        SceneManager.LoadScene("2_GameStage");
    }
}
