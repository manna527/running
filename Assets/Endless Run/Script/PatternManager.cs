using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Pattern//物体项
{
    public List<PatternItem> PatternItems = new List<PatternItem>();
}

[System.Serializable]
public class PatternItem //场景中物体的类型和位置
{
    public GameObject gameobject;
    public Vector3 position;
}
public class PatternManager : MonoBehaviour
{

    public static PatternManager instance;

    public List<Pattern> Patterns = new List<Pattern>();

    // Use this for initialization
    void Start()
    {

        instance = this;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
