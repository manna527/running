using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 游戏配置
/// </summary>
public class GameAttribute : MonoBehaviour {

	/// <summary>
	/// 金币数
	/// </summary>
	public int coin;

    /// <summary>
    /// 积分增加基数
    /// </summary>
    public int multiply = 1;

	/// <summary>
	/// 全局实例
	/// </summary>
	public static GameAttribute instance;

	/// <summary>
	/// 生命值
	/// </summary>
	public int life = 1;

	/// <summary>
	/// 初始生命值
	/// </summary>
	public int init_life = 1;

	/// <summary>
	/// 金币Text
	/// </summary>
	public Text Text_Coin;

	/// <summary>
	/// 音效是否打开
	/// </summary>
	public bool soundOn = true;

	// Use this for initialization
	void Start () {
		//金币重置
		coin = 0;

		//设置全局实例
		instance = this;
	}

	/// <summary>
	/// 重置
	/// </summary>
	public void Reset()
	{
		life = init_life;
		coin = 0;
		multiply = 1;
	}

	// Update is called once per frame
	void Update () {

		//显示金币数目
		Text_Coin.text = coin.ToString();
	}

	/// <summary>
	/// 增加金币
	/// </summary>
	/// <param name="coinNumber">金币数目</param>
	public void AddCoin(int coinNumber)
	{
		//金币增加需要乘以基数
		coin += multiply * coinNumber;
        Debug.Log("this is a test");

    }
}