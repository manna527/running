using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]


public class PlayController : MonoBehaviour {
	//suxiaona 
	/// <summary>
	/// 暴走协程
	/// </summary>
	IEnumerator quickMoveCor;
	/// <summary>
	/// 暴走剩余时间
	/// </summary>
	public float quickMoveTimeLeft;
	/// <summary>
	/// 暴走持续时间
	/// </summary>
	float quickMoveDuration = 3;
	/// <summary>
	/// 是否在暴走
	/// </summary>
	bool isQuickMoving = false;
	/// <summary>
	/// 保存暴走前的速度
	/// </summary>
	float saveSpeed;


	//suxiaona2

	/// <summary>
	/// 磁铁道具状态
	/// </summary>
	public Text Text_Magnet;
	/// <summary>
	/// 暴走道具状态
	/// </summary>
	public Text Text_Star;

	/// <summary>
	/// 积分加成道具状态
	/// </summary>
	public Text Text_Multiply;

    /// <summary>
    /// 重启后重新开始场景
    /// </summary>
    public GameObject Road;
    public GameObject Road1;
    public GameObject StartRoad;

    private Animator anim;                      // Animatorへの参照
    private AnimatorStateInfo currentState;     // 現在のステート状態を保存する参照
    private AnimatorStateInfo previousState;    // ひとつ前のステート状態を保存する参照

    /// <summary>
    /// x方向移动
    /// </summary>
    Vector3 xDirection;

    /// <summary>
    /// y和z移动
    /// </summary>
    Vector3 moveDirection;

    /// <summary>
    /// 速度
    /// </summary>
    public float speed = 5;
    public float init_speed = 5;//初始速度
    public float maxSpeed = 20;//最大速度

    /// <summary>
    /// 每过多远距离速度递增
    /// </summary>
    private float speedAddDistance = 100;

    /// <summary>
    /// 速度每次递增多少
    /// </summary>
    private float speedAddRate = 1f;

    /// <summary>
    /// 距离计数
    /// </summary>
    private float speedAddCount = 0;

    /// <summary>
    /// 滑动方向
    /// </summary>
    InputDirection inputDirection;

    /// <summary>
    /// 输入位置
    /// </summary>
    Vector3 mousePos;

    /// <summary>
    /// 是否已经输入
    /// </summary>
    bool activeInput;

    /// <summary>
    /// 站立位置
    /// </summary>
    Position standPosition;

    /// <summary>
    /// 从什么位置跳过来
    /// </summary>
    Position fromPosition;

    /// <summary>
    /// 全局实例
    /// </summary>
    public static PlayController instance;
    /// <summary>
    /// 任务控制器
    /// </summary>
    CharacterController characterController;

    /// <summary>
    /// 是否在roll
    /// </summary>
    public bool isRoll = false;

    /// <summary>
    /// 向上跳动距离
    /// </summary>
     float jumpValue = 7;

    /// <summary>
    /// 重力
    /// </summary>
      float gravity = 10;

    //道具磁铁
    float magnetDuration = 15;//磁铁持续的时间
    public float magnetTimeLeft;//磁铁剩余的时间
    IEnumerator magnetCor;//磁铁的协成
    public GameObject MagnetCollider;//适用于磁铁的外部碰撞器

    //双倍积分道具
    float multiplyDuration = 10;
    public float multiplyTimeLeft;
    IEnumerator multiplyCor;

    private Animator animator;

    // Use this for initialization
    void Start () {

        //设置全局实例
        instance = this;
        //获取人物控制器
        characterController = GetComponent<CharacterController>();

        speed = init_speed;

        animator = GetComponent<Animator>();

        //默认站在中间
        standPosition = Position.Middle;

        //开始游戏协程
        StartCoroutine(UpdateAction());

        // 各参照の初期化
        anim = GetComponent<Animator>();
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;

    }

    /// <summary>
    /// 输入方向
    /// </summary>
    public enum InputDirection
    {
        /// <summary>
        /// 空
        /// </summary>
        NULL,

        /// <summary>
        /// 向左
        /// </summary>
        Left,

        /// <summary>
        /// 向右
        /// </summary>
        Right,

        /// <summary>
        /// 向上
        /// </summary>
        Up,

        /// <summary>
        /// 向下
        /// </summary>
        Down
    }

    /// <summary>
    /// 位置
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// 左侧
        /// </summary>
        Left,

        /// <summary>
        /// 中间
        /// </summary>
        Middle,

        /// <summary>
        /// 右侧
        /// </summary>
        Right
    }

    public void Play()//确认游戏重新开始时人物的动作
    {
        GameController.instance.isPause = false;
        GameController.instance.isPlay = true;
        StartCoroutine(UpdateAction());
    }

    /// <summary>
    /// 设置主角速度
    /// </summary>
    /// <param name="newSpeed"></param>
    private void SetSpeed(float newSpeed)
    {
        //速度不能大于最大速度
        if (newSpeed <= maxSpeed)
            speed = newSpeed;
        else
            speed = maxSpeed;
    }


    IEnumerator UpdateAction()
    {
        while (GameAttribute.instance.life > 0)     //生命值大于0
        {
            if (GameController.instance.isPlay && !GameController.instance.isPause)
            {
                GetInputDirection();    //获取输入方向
                // PlayAnimation();//播放动画
                MoveLeftRight();    //左右移动
                MoveForward();//上下移动
                UpdateSpeed();//递增速度
            }
            else
            {
                //animator.enabled = false;
                //animator.Stop();
            }

            yield return 0;
        }
        speed = 0;  // 速度变回0
        GameController.instance.isPlay = false;//人物死后倒计时结束
        xDirection = Vector3.zero;
        AnimationController.instance.animationHandler = AnimationController.instance.PlayDead; // 执行死亡动画

        iTween.MoveTo(gameObject, gameObject.transform.position - new Vector3(0, 0, 2), 2.0f);//使人物碰撞死亡后，碰撞体可后退

        yield return new WaitForSeconds(3);  // 等待三秒之后重启
        Debug.Log("restart");


		UIController.instance.ShowRestartUI();  //显示重启菜单
		UIController.instance.HidePauseUI();    //隐藏暂停菜单

    }

    /// <summary>
    /// 获取输入方向
    /// </summary>
    void GetInputDirection()
    {
        inputDirection = InputDirection.NULL;
        if (Input.GetMouseButtonDown(0)) //鼠标按下
        {
            activeInput = true; //开始输入
            mousePos = Input.mousePosition;     //记录按下的位置
        }
        if (Input.GetMouseButton(0) && activeInput)  //鼠标没有松开，并且曾经按下过
        {
            Vector3 vec = Input.mousePosition - mousePos;   //滑动方向
            if (vec.magnitude > 20)     //距离大于20才处理
            {
                //滑动方向与+y轴夹角
                var angleY = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.up)) * Mathf.Rad2Deg;

                //滑动方向与+x轴夹角
                var anglex = Mathf.Acos(Vector3.Dot(vec.normalized, Vector2.right)) * Mathf.Rad2Deg;

                if (angleY <= 45)   //y夹角小于45度，为向上滑动
                {
                    inputDirection = InputDirection.Up;
                    AudioManager.instance.PlaySlideAudio(); //播放滑动音效
                }
                else if (angleY >= 135)   //y夹角大于135度，为向下滑动
                {
                    inputDirection = InputDirection.Down;
                    AudioManager.instance.PlaySlideAudio(); //播放滑动音效
                }
                else if (anglex <= 45)  //x夹角小于45度，为向右移动
                {
                    inputDirection = InputDirection.Right;
                    AudioManager.instance.PlaySlideAudio(); //播放滑动音效
                }
                else if (anglex >= 135)    //x夹角大于135度，为向左移动
                {
                    inputDirection = InputDirection.Left;
                    AudioManager.instance.PlaySlideAudio(); //播放滑动音效
                }

                //输入结束
                activeInput = false;
            }
        }
    }

            /// <summary>
            /// 播放动画
            /// </summary>
            void PlayAnimation()
    {
        if (inputDirection == InputDirection.Left)  //向左滑动
        {
            AnimationController .instance.animationHandler = AnimationController.instance.PlayTurnLeft;
        }
        else if (inputDirection == InputDirection.Right)    //向右滑动
        {

            AnimationController.instance.animationHandler = AnimationController.instance.PlayTurnRight;
        }
        else if (inputDirection == InputDirection.Up)   //向上滑动
        {
            AnimationController.instance.animationHandler = AnimationController.instance.PlayJumpUp;
        }
        else if (inputDirection == InputDirection.Down) //向下滑动
        {
            AnimationController.instance.animationHandler = AnimationController.instance.PlayJumpDown;
        }
    }


    /// <summary>
    /// 更新用户速度
    /// </summary>
    private void UpdateSpeed()
    {
        //累加距离
        speedAddCount += speed * Time.deltaTime;

        //距离达到了需要更新的值
        if (speedAddCount >= speedAddDistance)
        {
            //设置新的速度
            SetSpeed(speed + speedAddRate);

            //重置距离计数器
            speedAddCount = 0;
        }
    }

    /// <summary>
    /// 往前以及上下移动
    /// </summary>
    void MoveForward()
    {
        if (inputDirection == InputDirection.Down)   //向下滑动
        {
            //播放down动画
            AnimationController.instance.animationHandler = AnimationController.instance.PlayJumpDown;
        }
        if (characterController.isGrounded)  //主角在地面上
        {
            moveDirection = Vector3.zero;   //重置x方向输入

            if (inputDirection == InputDirection.Up) //向上滑动
            {
                JumpUp();   //跳跃
               // if (canDoubleJump)      //能双连跳
                //    doubleJump = true;  //进入双连跳状态
                //播放JumpUp动画
               // AnimationController.instance.animationHandler = AnimationController.instance.PlayJumpUp;
            }
            //如果当前不在播放Roll、TurnLeft、TurnRight动画
            if (AnimationController.instance.animationHandler != AnimationController.instance.PlayJumpDown &&
                 AnimationController.instance.animationHandler != AnimationController.instance.PlayTurnLeft &&
                AnimationController.instance.animationHandler != AnimationController.instance.PlayTurnRight &&
                AnimationController.instance.animationHandler != AnimationController.instance.PlayJumpUp)
            {
                //那么就要播放run动画
                AnimationController.instance.animationHandler = AnimationController.instance.PlayRun;
            }
            moveDirection.z = speed;
            moveDirection.y -= gravity * Time.deltaTime;
            characterController .Move ((xDirection+moveDirection)*Time.deltaTime);

        }
        //z方向为当前速度
        moveDirection.z = speed;

        //   //y方向需要计算重力
           moveDirection.y -= gravity * Time.deltaTime;

        //移动主角
        characterController.Move((xDirection*5 + moveDirection) * Time.deltaTime);
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    void JumpUp()
    {
        //播放JumpUp动画
        AnimationController.instance.animationHandler = AnimationController.instance.PlayJumpUp;

        //y值增加
          moveDirection.y += jumpValue;
    }

    /// <summary>
    /// 往左移动
    /// </summary>
    void MoveLeft()
    {
        if (standPosition != Position.Left) //当前位置不在最左侧
        {
           // GetComponent<Animation>().Stop();   //停止动画
            AnimationController.instance.animationHandler = AnimationController.instance.PlayTurnLeft;    //播放TurnLeft动画

            xDirection = Vector3.left;  //设置x方向移动为左侧

            if (standPosition == Position.Middle)   //从中间移动过来
            {
                standPosition = Position.Left;  //新的站立位置为最左侧
                fromPosition = Position.Middle; //从中间移动过来
            }
            else if (standPosition == Position.Right)    //从右侧移动过来
            {
                standPosition = Position.Middle;    //新的站立位置为中间
                fromPosition = Position.Right;  //从右侧移动过来
            }
        }
    }
    /// <summary>
    /// 往右移动
    /// </summary>
    void MoveRight()
    {
        if (standPosition != Position.Right)    //当前位置不在最右侧
        {
           // GetComponent<Animation>().Stop();   //停止动画
            AnimationController.instance.animationHandler = AnimationController.instance.PlayTurnRight;   //播放TurnRight动画

            xDirection = Vector3.right; //设置x方向移动为右侧

            if (standPosition == Position.Middle)   //从中间移动过来
            {
                standPosition = Position.Right;     //新的站立位置为左右侧
                fromPosition = Position.Middle;     //从中间移动过来
            }
            else if (standPosition == Position.Left)    //从左侧移动过来
            {
                standPosition = Position.Middle;    //新的站立位置为中间
                fromPosition = Position.Left;       //从左侧移动过来
            }
        }
    }

    /// <summary>
    /// 左右移动
    /// </summary>
    void MoveLeftRight()
    {
        if (inputDirection == InputDirection.Left)  //左侧移动
        {
            MoveLeft();
        }
        else if (inputDirection == InputDirection.Right) //右侧移动
        {
            MoveRight();
        }

        if (standPosition == Position.Left) //移到左侧
        {
            if (transform.position.x <= -5f)  //主角已经到达最左边的位置
            {
                xDirection = Vector3.zero;  //不能再往左移动了
                transform.position = new Vector3(-5f, transform.position.y, transform.position.z);    //更新主角位置
            }
        }
        if (standPosition == Position.Middle)    //主角站在中间
        {
            if (fromPosition == Position.Left)  //从左侧移动过来
            {
                if (transform.position.x >= -2f)   //主角的x值已经开始大于-2
                {
                    xDirection = Vector3.zero;      //不能再往右移动了
                    transform.position = new Vector3(-2f, transform.position.y, transform.position.z);    //更新主角位置
                }
            }
            else if (fromPosition == Position.Right) //从右侧移动过来
            {
                if (transform.position.x <=-2f)   //主角的x值已经开始小于-2了
                {
                    xDirection = Vector3.zero;      //不能再往左移动了
                    transform.position = new Vector3(-2f, transform.position.y, transform.position.z);    //更新主角位置
                }
            }
        }

        if (standPosition == Position.Right)     //移到右侧
        {
            if (transform.position.x >= 1f)   //主角已经到达最右边的位置
            {
                xDirection = Vector3.zero;      //不能再往右移动了
                transform.position = new Vector3(1f, transform.position.y, transform.position.z);     //更新主角位置
            }
        }
    }
    // Update is called once per frame
    void Update () {
       // moveDirection.z = speed;
       // moveDirection.y -= gravity * Time.deltaTime;
       // characterController .Move ((xDirection+moveDirection)*Time.deltaTime);

        //statusText.text = GetTime(multiplyTimeLeft);
		UpdateItemTime();
    }
    /// <summary>
    /// 重启设置
    /// </summary>
    public void Reset()
    {
        speed = init_speed;
        inputDirection = InputDirection.NULL;
        activeInput = false;
        standPosition = Position.Middle;
        xDirection = Vector3.zero;
        moveDirection = Vector3.zero;
        isRoll = false;
        isQuickMoving = false;
        quickMoveTimeLeft = 0;
        magnetTimeLeft = 0;
        multiplyTimeLeft = 0;

        gameObject.transform.position = new Vector3(-2, 0, -189);//人物位置
        Camera.main.transform.position = new Vector3(-2, 5, -100);//主相机位置

        AnimationController.instance.animationHandler = AnimationController.instance.PlayRun;

        var newRoad=Respawn("Road",Road,new Vector3(-3.5f,1.5367f,-90f));
        var newRoad1=Respawn("Road1", Road1, new Vector3(-3.5f, 1.5367f, 6f));
        Respawn("StartRoad", StartRoad, new Vector3(-3.5f, 1.5367f, -186f));

        FloorSetter.instance.floorOnRunning = newRoad;
        FloorSetter.instance.floorForward = newRoad1;
    }
    /// <summary>
    /// 重启后的路
    /// </summary>
    private GameObject Respawn(string name, GameObject prefab, Vector3 location)
    {
        var old = GameObject.Find(name);
        if (old != null)
        {
            Destroy(old);
            var newObj = Instantiate(prefab);
            newObj.name = name;
            newObj.transform.localPosition = location;
            return newObj;
        }
        return null;
    }

    //suxiaona

    /// <summary>
    /// 暴走
    /// </summary>
    public void QuickMove()
	{
		//如果协程已经创建，则停止
		if (quickMoveCor != null)
			StopCoroutine(quickMoveCor);

		//初始化协程
		quickMoveCor = QuickMoveCoroutine();

		//启动协程
		StartCoroutine(quickMoveCor);
	}

    private bool CanPlay()
    {
        //游戏是否在正常运行
        return !GameController.instance.isPause && GameController.instance.isPlay;
    }

    public void UseMagnet()//磁铁道具
    {
        if (magnetCor != null)
            StopCoroutine(magnetCor);
        magnetCor = MagnetCoroutine();
        StartCoroutine(magnetCor);
    }
    IEnumerator MagnetCoroutine()
    {
        magnetTimeLeft = magnetDuration;
        MagnetCollider.SetActive(true);
        while (magnetTimeLeft >= 0)
        {
            if (CanPlay())
            magnetTimeLeft -= Time.deltaTime;
            yield return null;
        }
        MagnetCollider.SetActive(false);
    }
    public void Multiply()//双倍积分道具
    {
        if (multiplyCor != null)
            StopCoroutine(multiplyCor);
        multiplyCor = MultiplyCoroutine();
        StartCoroutine(multiplyCor);
    }
    IEnumerator MultiplyCoroutine()
    {
        multiplyTimeLeft = multiplyDuration;
        GameAttribute.instance.multiply = 2;
        while (multiplyTimeLeft >= 0)
        {
            if (CanPlay())
            multiplyTimeLeft -= Time.deltaTime;
            yield return null;
        }
        GameAttribute.instance.multiply = 1;
    }

    /// <summary>
    /// 暴走协程
    /// </summary>
    /// <returns></returns>
    IEnumerator QuickMoveCoroutine()
	{
		//初始剩余时间
		quickMoveTimeLeft = quickMoveDuration;

		//如果不在暴走
		if(!isQuickMoving)
			saveSpeed = speed;  //保存暴走之前的速度
		speed = maxSpeed;     //设置暴走速度
		isQuickMoving = true;   //正在暴走
		while (quickMoveTimeLeft>=0)    //倒计时
		{
			if (CanPlay())
			quickMoveTimeLeft -= Time.deltaTime;    //时间递减
			yield return null;
		}
		speed = saveSpeed;  //恢复暴走前的速度
		isQuickMoving = false;  //不在暴走
	}
	/// <summary>
	/// 是否可以更新道具状态
	/// </summary>
	/// <returns></returns>
	/*private bool CanPlay()
	{
		return !GameController.instance.isPause &&  //没有暂停
			GameController.instance.isPlay;     //在游戏中
	}*/

	/// <summary>
	/// 更新技能剩余时间
	/// </summary>
	private void UpdateItemTime()
	{
		//以下设置各个技能的剩余时间
		Text_Magnet.text = GetTime(magnetTimeLeft);
		Text_Multiply.text = GetTime(multiplyTimeLeft);
		Text_Star.text = GetTime(quickMoveTimeLeft);
	}
	private string GetTime(float time)
	{
		//小于等于0，返回空
		if (time <= 0)
			return "";

		//获取文本显示
		return ((int)time + 1).ToString()+"s";
	}

}
