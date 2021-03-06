using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PatternSystemEditor : EditorWindow
{

    [MenuItem("Window/AddPatternToSystem")]
    static void AddPatternToSystem()
    {
        var gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            var patternManager = gameManager.GetComponent<PatternManager>();

            if (Selection.gameObjects.Length == 1)//选中road1
            {
                var item = Selection.gameObjects[0].transform.Find("Item");
                if (item != null)//不为空则组合为子物体
                {
                    Pattern pattern = new Pattern();
                    foreach (var child in item)
                    {
                        Transform childTransform = child as Transform;
                        if (childTransform != null)
                        {
                            //找到场景中对应的预设
                            var prefeb = UnityEditor.PrefabUtility.GetPrefabParent(childTransform.gameObject);
                            if (prefeb != null)
                            {
                                PatternItem patternItem = new PatternItem
                                {
                                    gameobject = prefeb as GameObject,
                                    position = childTransform.transform.localPosition
                                };
                                pattern.PatternItems.Add(patternItem);
                            }
                        }
                    }
                    patternManager.Patterns.Add(pattern);
                }
            }

        }
    }
}
