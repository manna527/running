using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public bool isPause;//是否已经暂停
    public bool isPlay;//是否在游戏
    public static GameController instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
        isPause = true;
        isPlay = true;//必须用户手动开启游戏

    }

    public void Play()
    {
        isPause = false;
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Resume()
    {
        isPause = false;
    }

    public void Restart()
    {
        GameAttribute.instance.Reset();
        PlayController.instance.Reset();
        PlayController.instance.Play(); 
    }

    public void Exit()
    {
        //在编辑器中和发布的游戏中
#if UNITY_EDITOR     
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif 
    }


    // Update is called once per frame
    void Update()
    {

    }
}
