using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class exportTruck : MonoBehaviour {

    public SpriteRenderer truckOverlay;
    gameManager gm;
    public TextMeshProUGUI text;
    AudioSource _as;

    public Sprite noTruck, Truck;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("Main Camera").GetComponent<gameManager>();
        _as = this.GetComponent<AudioSource>();
	}

    public void removeTruck() {
        truckOverlay.enabled = false;
        this.GetComponent<SpriteRenderer>().sprite = noTruck;
        text.text = "";
        _as.volume = 0;
    }

    public void setTruck(Color c, string id) {
        truckOverlay.enabled = true;
        truckOverlay.color = c;
        this.GetComponent<SpriteRenderer>().sprite = Truck;
        text.text = id;
        _as.volume = 0.25f;
    }
}
