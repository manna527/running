using UnityEngine;
using System.Collections;

/// <summary>
/// UI控制器
/// </summary>
public class UIController : MonoBehaviour {

    /// <summary>
    /// 播放菜单
    /// </summary>
    public GameObject PlayUI;

    /// <summary>
    /// 恢复菜单
    /// </summary>
    public GameObject ResumeUI;

    /// <summary>
    /// 重启菜单
    /// </summary>
    public GameObject RestartUI;

    /// <summary>
    /// 暂停菜单
    /// </summary>
    public GameObject PauseUI;

    /// <summary>
    /// 全局实例
    /// </summary>
    public static UIController instance;

    /// <summary>
    /// canvas
    /// </summary>
    public Canvas canvas;

    /// <summary>
    /// 隐藏播放菜单
    /// </summary>
    public void HidePlayUI()
    {
        iTween.MoveTo(PlayUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, 0, 0), 1.0f);
    }

    /// <summary>
    /// 显示暂停菜单
    /// </summary>
    public void ShowPauseUI()
    {
        iTween.MoveTo(PauseUI, canvas.transform.position + new Vector3(-Screen.width / 2, -Screen.height / 2, 0), 1.0f);
    }

    /// <summary>
    /// 显示恢复菜单
    /// </summary>
    public void ShowResumeUI()
    {
        iTween.MoveTo(ResumeUI, canvas.transform.position + Vector3.zero, 1.0f);
    }

    /// <summary>
    /// 隐藏暂停菜单
    /// </summary>
    public void HidePauseUI()
    {
        iTween.MoveTo(PauseUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, -Screen.height / 2, 0), 1.0f);
    }

    /// <summary>
    /// 隐藏恢复菜单
    /// </summary>
    public void HideResumeUI()
    {
        iTween.MoveTo(ResumeUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, 0, 0), 1.0f);
    }

    /// <summary>
    /// 点击Play处理程序
    /// </summary>
    public void PlayHandler()
    {
        //隐藏播放菜单
        HidePlayUI();

        //显示暂停菜单
        ShowPauseUI();

        //播放菜单音效
        //   AudioManager.instance.PlayButtonAudio();
        AudioManager.instance.PlayButtonAudio();
        //开始游戏
        GameController.instance.Play();
    }

    /// <summary>
    /// 点击暂停处理程序
    /// </summary>
    public void PauseHandler()
    {
        //显示恢复菜单
        ShowResumeUI();

        //隐藏暂停菜单
        HidePauseUI();

        //播放菜单音效
        AudioManager.instance.PlayButtonAudio();

        //暂停
        GameController.instance.Pause();
    }

    /// <summary>
    /// 显示重启菜单
    /// </summary>
    public void ShowRestartUI()
    {
        iTween.MoveTo(RestartUI, canvas.transform.position + Vector3.zero, 1.0f);
    }

    /// <summary>
    /// 隐藏重启菜单
    /// </summary>
    public void HideRestartUI()
    {
        iTween.MoveTo(RestartUI, canvas.transform.position + new Vector3(-Screen.width / 2 - 500, 0, 0), 1.0f);
    }

    /// <summary>
    /// 点击恢复处理程序
    /// </summary>
    public void ResumeHandler()
    {
        //隐藏恢复菜单
        HideResumeUI();

        //显示暂停菜单
        ShowPauseUI();

        //播放菜单音效
        AudioManager.instance.PlayButtonAudio();

        //恢复
        GameController.instance.Resume();
    }

    /// <summary>
    /// 点击重启处理程序
    /// </summary>
    public void RestartHandler()
    {
        //隐藏重启菜单
        HideRestartUI();

        //显示暂停菜单
        ShowPauseUI();

        //播放菜单音效
        AudioManager.instance.PlayButtonAudio();

        //重启
        GameController.instance.Restart();
    }

    //点击退出处理程序
    public void ExitHandler()
    {
        //播放菜单音效
        AudioManager.instance.PlayButtonAudio();

        //退出
        GameController.instance.Exit();
    }

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}