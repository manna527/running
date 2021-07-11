using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //碰撞函数
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")//碰到的是金币
        {
            Debug.Log(other.name);
            GameAttribute.instance.AddCoin(1);
            StartCoroutine(HitCoin(other.gameObject));
        }
    }
    IEnumerator HitCoin(GameObject coin)
    {
        bool isLoop = true;
        while (isLoop)
        {
            if (coin == null)
            {
                isLoop = false;
                continue;
            }
            coin.transform.position = Vector3.Lerp(coin.transform.position, PlayController.instance.gameObject.transform.position, Time.deltaTime * 20);
            if (Vector3.Distance(coin.transform.position, PlayController.instance.gameObject.transform.position) < 0.5f)
            {
                coin.GetComponent<Coin>().HitItem();
                isLoop = false;
            }
            yield return null;
        }
    }
}
