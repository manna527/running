using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	public float rotateSpeed = 1;//金币旋转的速度
	public GameObject hitEffect;
	void Update()
	{
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);//金币转动的方向
	}
	//
	public virtual void HitItem()
	{
        // 播放获取物体音效
        PlayHitAudio();
        //粒子特效在人物背后出现
        GameObject effect = Instantiate(hitEffect);
		effect.transform.parent = PlayController.instance.gameObject.transform;
		effect.transform.localPosition = new Vector3(0.082f, 1.165f, -0.861f);
		Destroy(gameObject);//金币消失
	}
    public virtual void PlayHitAudio()
    {
        AudioManager.instance.PlayGetItemAudio();
    }
	//金币与人物碰撞时
	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")//其他碰撞器为人物时
		{
            
			HitItem();
		}
    }
}

