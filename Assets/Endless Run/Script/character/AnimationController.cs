using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    /// <summary>
    /// 动画播放委托
    /// </summary>
    public delegate void AnimationHandler();

    /// <summary>
    /// 当前挂载的动画组件
    /// </summary>

    private Animator anim;                      // Animatorへの参照
    private AnimatorStateInfo currentState;     // 現在のステート状態を保存する参照
    private AnimatorStateInfo previousState;    // ひとつ前のステート状態を保存する参照

    /// <summary>
    /// 全局实例
    /// </summary>
    public static AnimationController instance;
    /// <summary>
    /// 动画播放委托实例
    /// </summary>
    public AnimationHandler animationHandler;

    // Use this for initialization
    void Start () {
        //设置全局实例
        instance = this;

        //默认播放Run动画
        animationHandler = PlayRun;

        //获取挂载的动画组件
        anim = GetComponent<Animator>();
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;
    }
    /// <summary>
    /// 播放Dead动画
    /// </summary>
    public void PlayDead()
    {
        anim.Play("DAMAGED01");
       // anim.SetBool("DAMAGED01", true);
        anim.SetBool("UMATOBI00", false);
        anim.SetBool("SLIDE00", false);
        anim.SetBool("RUN00_L", false);
        anim.SetBool("RUN00_L", false);
        anim.SetBool("RUN00_R", false);
       // anim.enabled = false;
        currentState = anim.GetCurrentAnimatorStateInfo(0);
       // if (currentState.normalizedTime >= 0.8f && currentState.IsName("DAMAGED01"))
       // {
           // anim.enabled = false;
            //anim.SetBool("SLIDE00", false);
           // animationHandler = PlayRun;
           // PlayController.instance.isRoll = false;
       // }

    }

    /// <summary>
    /// 播放JumpDown动画
    /// </summary>
    public void PlayJumpDown()
    {
        anim.Play("SLIDE00");
        //anim.SetBool("SLIDE00", true);
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.8f && currentState.IsName("SLIDE00"))
        {
            anim.SetBool("SLIDE00", false);
            animationHandler = PlayRun;
            PlayController.instance.isRoll = false;
        }
        else
        {
            PlayController.instance.isRoll = true;
        }

    }

 

    /// <summary>
    /// 播放JumpUp动画
    /// </summary>
    public void PlayJumpUp()
    {
		anim.Play ("UMATOBI00");
       // anim.SetBool("UMATOBI00", true);
        // animationHandler = PlayRun;
        transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);    //更新主角位置
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (currentState.normalizedTime >= 0.6f && currentState.IsName("UMATOBI00"))
        { 
            anim.SetBool("UMATOBI00", false);
            animationHandler = PlayRun;
        }
    }

   


    /// <summary>
    /// 播放Run动画
    /// </summary>
    public void PlayRun()
    {
        //anim.enabled = true;
        anim.Play("RUN00_F");
        //anim.SetBool("RUN00_F", true);
       
    }

    /// <summary>
    /// 播放TurnLeft动画
    /// </summary>
    public void PlayTurnLeft()
    {
        anim.Play("RUN00_L");
        //anim.SetBool("RUN00_L", true);
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        // animationHandler = PlayRun;
        if (currentState.normalizedTime >= 0.1f && currentState.IsName("RUN00_L"))
        {
            anim.SetBool("RUN00_L", false);
            animationHandler = PlayRun;
        }
    }

    /// <summary>
    /// 播放TurnRight动画
    /// </summary>
    public void PlayTurnRight()
    {
        anim.Play("RUN00_R");
        //anim.SetBool("RUN00_R", true);
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        //如果动画播放到尾声，则自动切换到Run动画
        //  animationHandler = PlayRun;
        if (currentState.normalizedTime >= 0.1f && currentState.IsName("RUN00_R"))
        {
            anim.SetBool("RUN00_R", false);
            animationHandler = PlayRun;
        }
    }

    // Update is called once per frame
    void Update () {
       
        //每一帧里面调用动画播放委托
        if (animationHandler != null)
        {
            animationHandler();
        }
    }
}
