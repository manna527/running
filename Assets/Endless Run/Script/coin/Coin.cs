using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            
            base.HitItem();
            
            GameAttribute.instance.AddCoin(1);
        }
    }
    // 播放金币音效
    public override void PlayHitAudio()
    {
        AudioManager.instance.PlayCoinAudio();
    }
}

