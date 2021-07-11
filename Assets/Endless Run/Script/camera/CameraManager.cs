using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    /// <summary>
    /// 跟随目标
    /// </summary>
    public GameObject target;

    /// <summary>
    /// 摄像机与主角的高度差
    /// </summary>
    public float height;

    /// <summary>
    /// 摄像机与主角的距离
    /// </summary>
    public float distance;

    /// <summary>
    /// 摄像机位置临时变量
    /// </summary>
    Vector3 pos;

    /// <summary>
    /// 全局实例
    /// </summary>
    public static CameraManager instance;

    bool isShaking = false;

    // Use this for initialization
    void Start () {
        //设置全局实例
        instance = this;

        //设置pos初始值
        pos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CameraShake()
    {
        if (!isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    //相机抖动
    IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        float time = 0.5f;
        while (time > 0)
        {
            transform.position = new Vector3(
                target.transform.position.x + Random.Range(-0.3f, 0.3f),
                target.transform.position.y + height,
                target.transform.position.z - distance);
            time -= Time.deltaTime;
            yield return null;
        }
        isShaking = false;
    }

    void LateUpdate()
    {
        if (GameController.instance.isPlay && !GameController.instance.isPause)
        {
            pos.x = target.transform.position.x;    //x与主角的x相同

            //y=主角的y+height，使用Lerp实现缓冲效果
            pos.y = Mathf.Lerp(pos.y, target.transform.position.y + height, Time.deltaTime * 5);

            //z=主角的z-distance
            pos.z = target.transform.position.z - distance;

            transform.position = pos;   //设置摄像机位置，实现跟随效果

        }

    }
}
