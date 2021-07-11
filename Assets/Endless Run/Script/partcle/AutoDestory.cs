using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour {

    public float destoryTime = 0.7f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, destoryTime);//特效销毁
    }

    // Update is called once per frame
    void Update()
    {

    }
}
