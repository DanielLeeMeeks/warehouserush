using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player : MonoBehaviour {

    gameManager gm;
    public GameObject holding;
    public TextMeshProUGUI boxInfo;
    public bool playable = true;
    public AudioClip walkAudio, pickUpAudio, putDownAudio, errorAudio;

	// Use this for initialization
	void Start () {
       // boxInfo = GameObject.Find("boxInfo").GetComponent<TextMeshProUGUI>();
        gm = GameObject.Find("Main Camera").GetComponent<gameManager>() ;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (playable)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { movePlayer(270); }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) { movePlayer(180); }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { movePlayer(90); }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) { movePlayer(0); }
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X)) { pickupBox(); }
        }

    }

    void pickupBox() {

        if (holding == null) {//Not holding box
            GameObject tempHold = gm.pickUpBox(nextPost());
            if (tempHold != null) {
                holding = tempHold;
                holdingBox();
                gm.PlaySoundPitchy(pickUpAudio);
            }
        }else{//Put down box
            if (gm.canYouPlaceBox(nextPost()))
            {
                holding.transform.position = new Vector3(nextPost().x, nextPost().y, 0);
                holding = null;
                gm.PlaySoundPitchy(putDownAudio);
            }
            else {
                gm.PlaySoundPitchy(errorAudio);
            }
        }
    }

    Vector2 nextPost()
    {
        if (this.transform.eulerAngles.z == 0)
        {
            return new Vector2(this.transform.position.x - 1, this.transform.position.y);
        }
        else if (this.transform.eulerAngles.z == 90)
        {
            return new Vector2(this.transform.position.x, this.transform.position.y - 1);
        }
        else if (this.transform.eulerAngles.z == 180)
        {
            return new Vector2(this.transform.position.x + 1, this.transform.position.y);
        }
        else if (this.transform.eulerAngles.z == 270)
        {
            return new Vector2(this.transform.position.x, this.transform.position.y + 1);
        }
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public GameObject GetHolding() { return holding; }

    void movePlayer(int rotation) {
        if (rotation == this.transform.eulerAngles.z)
        {
            if (this.transform.eulerAngles.z == 0)
            {
                bool move = gm.CanIMoveHere(new Vector2(this.transform.position.x - 1, this.transform.position.y));
                if (this.transform.position.x == 0) { move = false; }

                if (move) {
                    this.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, 0);
                    gm.PlaySoundPitchy(walkAudio);
                } else { Debug.LogWarning("You can't move here"); gm.PlaySoundPitchy(errorAudio);
                }

            }
            else if (this.transform.eulerAngles.z == 90)
            {
                bool move = gm.CanIMoveHere(new Vector2(this.transform.position.x, this.transform.position.y - 1));
                if (this.transform.position.y == 5) { move = false; }

                if (move) {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y-1, 0);
                    gm.PlaySoundPitchy(walkAudio);
                } else { Debug.LogWarning("You can't move here"); gm.PlaySoundPitchy(errorAudio);
                }

            }
            else if (this.transform.eulerAngles.z == 180)
            {
                bool move = gm.CanIMoveHere(new Vector2(this.transform.position.x + 1, this.transform.position.y));
                if (this.transform.position.x == 11) { move = false; }

                if (move) {
                    this.transform.position = new Vector3(this.transform.position.x + 1, this.transform.position.y, 0);
                    gm.PlaySoundPitchy(walkAudio);
                    if (holding != null) { holding.transform.position = new Vector3(this.transform.position.x + 0.6f, this.transform.position.y, 0); }
                } else { Debug.LogWarning("You can't move here"); gm.PlaySoundPitchy(errorAudio);
                }

            }
            else if (this.transform.eulerAngles.z == 270)
            {
                bool move = gm.CanIMoveHere(new Vector2(this.transform.position.x, this.transform.position.y+1));
                if (this.transform.position.y == 0) { move = false; }

                if (move) {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+1, 0);
                    gm.PlaySoundPitchy(walkAudio);
                    if (holding != null) { holding.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+0.6f, 0); }
                } else { Debug.LogWarning("You can't move here"); gm.PlaySoundPitchy(errorAudio);
                }

            }
            else {
                Debug.LogWarning("Rotation error. Could not move."); this.transform.eulerAngles = new Vector3(0, 0, 0); gm.PlaySoundPitchy(errorAudio);
            }

        } else {
            this.transform.eulerAngles = new Vector3(0,0,rotation);
        }

        holdingBox();

    }

    

    void holdingBox() {
        if (holding != null)
        {
            if (this.transform.eulerAngles.z == 0)
            {
                holding.transform.position = new Vector3(this.transform.position.x - 0.6f, this.transform.position.y, 0);
            }
            else if (this.transform.eulerAngles.z == 90)
            {
                holding.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.6f, 0);
            }
            else if (this.transform.eulerAngles.z == 180)
            {
                holding.transform.position = new Vector3(this.transform.position.x + 0.6f, this.transform.position.y, 0);
            }
            else if (this.transform.eulerAngles.z == 270)
            {
                holding.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.6f, 0);
            }
            boxInfo.text = "<b>Box Info</b> | Tracking ID: " + holding.GetComponent<box>().ToString() + " | Import time: " + holding.GetComponent<box>().GetImportTime() + " | Export time: " + holding.GetComponent<box>().GetExportTime() ;
        }
        else {
            boxInfo.text = "Not holding box.";
        }
    }

    
}
