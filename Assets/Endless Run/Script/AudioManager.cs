using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip button;
    public AudioClip coin;
    public AudioClip getitem;
    public AudioClip hit;
    public AudioClip slide;

    public static AudioManager instance;
    AudioSource bgm;

    private void PlayAudio(AudioClip clip)
    {
        //if (GameAttribute.instance.soundOn)
            AudioSource.PlayClipAtPoint(clip, PlayController.instance.transform.position);
    }

    //public void SwichSound()
    //{
    //    GameAttribute.instance.soundOn = !GameAttribute.instance.soundOn;
    //    soundImage.sprite = GameAttribute.instance.soundOn ? soundOnSprite : soundOffSprite;
    //    if (GameAttribute.instance.soundOn)
    //        bgm.Play();
    //}

    // 播放按钮音效
    public void PlayButtonAudio()
    {
        PlayAudio(button);
    }
    // 播放硬币音效
    public void PlayCoinAudio()
    {
        PlayAudio(coin);
    }
    // 播放获取物体音效
    public void PlayGetItemAudio()
    {
        PlayAudio(getitem);
    }
    // 播放撞击音效
    public void PlayHitAudio()
    {
        PlayAudio(hit);
    }
    // 播放滑动音效
    public void PlaySlideAudio()
    {
        PlayAudio(slide);
    }

    // Use this for initialization
    void Start () {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
