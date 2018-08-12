using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class importsDoor : MonoBehaviour {

    gameManager gm;
    bool on;
    public Sprite lightOn, lightOff;
    public SpriteRenderer lightS;
    AudioSource _as;
    public AudioClip alarm;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("Main Camera").GetComponent<gameManager>();
        InvokeRepeating("blink", 5f, 0.5f);
        _as = this.GetComponent<AudioSource>();
	}

    void blink() {
        if (on)
        {
            lightS.sprite = lightOff;
            on = false;
        }
        else {
            if (gm.boxesWaiting())
            {
                lightS.sprite = lightOn;
                on = true;
                _as.PlayOneShot(alarm);
            }
        }
    }
}
