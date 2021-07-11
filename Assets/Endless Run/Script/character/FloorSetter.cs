using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSetter : MonoBehaviour {

    /// <summary>
    /// 当前主角正在上面奔跑的道路
    /// </summary>
    public GameObject floorOnRunning;

    /// <summary>
    /// 主角奔跑前面的道路
    /// </summary>
    public GameObject floorForward;

    /// <summary>
    /// 全局实例
    /// </summary>
    public static FloorSetter instance;
    // Use this for initialization
    void Start () {

        instance = this;



    }

    //场景切换时清除道路的物体摆放
    void RemoveItem(GameObject floor)
    {
        var item = floor.transform.Find("Item");
        if (item != null)
        {
            foreach (var child in item)
            {
                Transform childTranform = child as Transform;
                if (childTranform != null)
                {
                    Destroy(childTranform.gameObject);
                }
            }
        }
    }

    //场景切换时添加新的道路物体摆放
    void AddItem(GameObject floor)
    {
		Debug.Log ("!!!!!!!");
        var item = floor.transform.Find("Item");
        if (item != null)
        {
            var patternManager = PatternManager.instance;
            if (patternManager != null && patternManager.Patterns != null && patternManager.Patterns.Count > 0)
            {
                 var pattern = patternManager.Patterns[Random.Range(0, patternManager.Patterns.Count)];
                

                if (pattern != null && pattern.PatternItems != null && pattern.PatternItems.Count > 0)
                {
                    foreach (var patternItem in pattern.PatternItems)
                    {
                        var newObj = Instantiate(patternItem.gameobject);
                        newObj.transform.parent = item;
                        newObj.transform.localPosition = patternItem.position;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (transform.position.z > floorOnRunning.transform.position.z + 96)    //主角已经跑到下一个floor上
        {
            //场景切换时清除道路的物体摆放
            RemoveItem(floorOnRunning);
            //场景切换时添加新的道路物体摆放
            AddItem(floorOnRunning);

            //将当前正在奔跑的道路往后移动96
            floorOnRunning.transform.position = new Vector3(floorForward.transform.position.x, floorForward.transform.position.y, floorForward.transform.position.z + 96);

            //以下交换正在奔跑的道路与将要奔跑的道路
            GameObject temp = floorOnRunning;
            floorOnRunning = floorForward;
            floorForward = temp;
        }
    }
}
