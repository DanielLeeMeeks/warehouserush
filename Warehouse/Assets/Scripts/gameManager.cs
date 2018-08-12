using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {

    public GameObject floor, floor_noBoxes, floor_imports, floor_exports, permanentBox, box, exportDoorPrefab, importDoorPrefab;

    GameObject[,] grid = new GameObject [12,6];
    public string [] gridInput1 = new string [6];
    public string[] gridInput2 = new string[6];
    public string[] gridInput3 = new string[6];
    public string[] gridInput4 = new string[6];
    string levelID;

    public int clock = 0;
    public bool randomSpawn;
    public int boxRate=25;
    public GameObject currentExport;

    public player p;

    GameObject import, export, exportDoor, importDoor;
    exportTruck et;
    List<GameObject> boxes = new List<GameObject>();
    bool impotFull = false;
    int impotBacklog = 0;

    public TextMeshProUGUI display, goText;
    public Image goPanel;
    public GameObject resetButton, menuButton;

    int boxesExported;

    public Slider importSlider, exportSlider;

    AudioSource _as;

	// Use this for initialization
	void Start () {

        _as = this.GetComponent<AudioSource>();

        gridInput1[0] = "IXXXXXXXXXXE";
        gridInput1[1] = "XXOOOXXBOBBB";
        gridInput1[2] = "OOOOOXXBOOOO";
        gridInput1[3] = "OOOOOXXBOBBB";
        gridInput1[4] = "OOOXXXXXOOOO";
        gridInput1[5] = "BBBOOOOOOBBB";

        gridInput2[0] = "OIOOOOOOOOEO";
        gridInput2[1] = "OOOOOOOOOOOO";
        gridInput2[2] = "OOOOOOOOOOOO";
        gridInput2[3] = "OOOOOOOOOOOO";
        gridInput2[4] = "OOOOOOOOOOOO";
        gridInput2[5] = "OOOOOOOOOOOO";

        gridInput3[0] = "OIOOBBBBBOEO";
        gridInput3[1] = "OOOOOOOOOOOO";
        gridInput3[2] = "BBBBBOBBBBBB";
        gridInput3[3] = "OOOOOOOOOOOO";
        gridInput3[4] = "BBBBBBOBBBBB";
        gridInput3[5] = "OOOOOOOOOOOO";

        gridInput4[0] = "XIXXXXXXXXEX";
        gridInput4[1] = "XXXXOOOOXXXX";
        gridInput4[2] = "BBBXOOOOXBBB";
        gridInput4[3] = "BBBXOOOOXBBB";
        gridInput4[4] = "BBBXOOOOXBBB";
        gridInput4[5] = "BBBBBBBBBBBB";

        if (PlayerPrefs.GetString("level") == "1") { GridSetup(gridInput1);
        } else if (PlayerPrefs.GetString("level") == "blank") { GridSetup(gridInput2);
        }
        else if (PlayerPrefs.GetString("level") == "boxes")
        { GridSetup(gridInput3);
        } else if (PlayerPrefs.GetString("level") == "small") { GridSetup(gridInput4); } else { GridSetup(gridInput2); }



        //et = GameObject.Find("exports_Truck").GetComponent<exportTruck>();

        p.transform.position = export.transform.position;

        InvokeRepeating("tick", 5f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySoundPitchy(AudioClip a) {
        _as.pitch = (float)Random.Range(80, 120)/100f;
        _as.PlayOneShot(a);
    }

    public void AddNewBox(GameObject box) {
        boxes.Add(box);
    }

    public GameObject pickUpBox(Vector2 post) {
        post = new Vector2(Mathf.Abs(post.x), Mathf.Abs(post.y));
        if (grid[Mathf.RoundToInt(post.x), Mathf.RoundToInt(post.y)] != null) {
            if (grid[Mathf.RoundToInt(post.x), Mathf.RoundToInt(post.y)].tag == "box") {
                if (!grid[Mathf.RoundToInt(post.x), Mathf.RoundToInt(post.y)].GetComponent<box>().unMovable) { 
                GameObject r = grid[Mathf.RoundToInt(post.x), Mathf.RoundToInt(post.y)];
                grid[Mathf.RoundToInt(post.x), Mathf.RoundToInt(post.y)] = null;//Debug.Log (r);
                return r;
                }
            }
        }
        return null;
    }

    public bool canYouPlaceBox(Vector2 post) {

        if (post.x == import.transform.position.x && post.y == import.transform.position.y) { return false; }
        if (post.x == export.transform.position.x && post.y == export.transform.position.y) {
            if (currentExport == p.GetHolding()) {
                boxes.Remove(currentExport);
                et.removeTruck();
                exportSlider.value = exportSlider.value-20;
                currentExport.GetComponent<box>().StartCoroutine(currentExport.GetComponent<box>().exportBox());
                currentExport = null;
                boxesExported++;
                return true;
            } else { return false; }
        }

        if (post.x < 0 || post.x > 11) { return false; }
        if (post.y > 0 || post.y < -5) { return false; }

        post = new Vector2(Mathf.Abs(post.x), Mathf.Abs(post.y));

        if (grid[Mathf.RoundToInt(post.x), Mathf.RoundToInt(post.y)] == null) {
            grid[Mathf.RoundToInt(post.x), Mathf.RoundToInt(post.y)] = p.GetHolding();
            return true;
        }
        return false;
    }

    void tick() {
        clock++;

        if (impotBacklog < 3)
        {
            if (randomSpawn)
            {
                if (Random.Range(0, 100) < boxRate)
                {
                    impotBacklog++;
                }
                if (boxes.Count + impotBacklog < 3)
                {
                    impotBacklog++;
                    Debug.Log("Not enough boxes. " + (boxes.Count + impotBacklog));
                }
            }
        }

            if (impotBacklog > 0) {
                if (grid[Mathf.RoundToInt(import.transform.position.x), Mathf.RoundToInt(import.transform.position.y)] == null) {

                    grid[Mathf.RoundToInt(import.transform.position.x), Mathf.RoundToInt(import.transform.position.y)] = Instantiate (box, new Vector3(import.transform.position.x, import.transform.position.y, 0), this.transform.rotation);
                    AddNewBox(grid[Mathf.RoundToInt(import.transform.position.x), Mathf.RoundToInt(import.transform.position.y)]);
                    impotBacklog--;
                }
            }

        

        if (currentExport == null) { FindNextBox(); }

        if (currentExport != null)
        {
            display.text = "Time: " + this.GetTime() + " - Box Wanted in Export: " + currentExport.GetComponent<box>().ToString() + " - Boxes waiting in Import: " + impotBacklog + " - Exported " + boxesExported + " boxes.";
            exportSlider.value = exportSlider.value + 1;
            if (exportSlider.value >= exportSlider.maxValue) { lose("You did not get exports out fast enough."); }
        }
        else {
            display.text = "Time: " + this.GetTime() + " - Box Wanted in Export: None - Boxes waiting in Import: " + impotBacklog + " - Exported " + boxesExported + " boxes.";
            exportSlider.value = exportSlider.value - 2;
        }

        if (impotBacklog > 0)
        {
            importSlider.value = importSlider.value + impotBacklog;
            if (importSlider.value >= importSlider.maxValue) { lose("To many boxes pilled up in imports."); }
        }
        else {
            importSlider.value = importSlider.value - 1;
        }
    }

    public void lose(string message) {
        Debug.LogWarning("YOU LOSE");
        p.playable = false;
        goPanel.enabled = true;
        goText.text = "Game Over!\n\n"+message+"\n\nBoxes exported: "+boxesExported;
        menuButton.SetActive(true);
        resetButton.SetActive(true);
        CancelInvoke();
    }

    public void FindNextBox()
    {
        box next = null;
        Debug.Log(boxes.Count);

        if (boxes.Count > 0)
        {
            foreach (GameObject g in boxes)
            {
                if ((next == null || g.GetComponent<box>().GetTimeUnformatted() < next.GetTimeUnformatted()) && g.GetComponent<box>().GetTimeUnformatted() != 0)
                {
                    next = g.GetComponent<box>();
                }
            }

            if (next != null) { 
                Debug.Log(next.GetTimeUnformatted() + " < " + this.GetTimeUnformatted());
                if (next.GetTimeUnformatted() < this.GetTimeUnformatted())
                {
                    currentExport = next.gameObject;
                    et.setTruck(next.GetComponent<SpriteRenderer>().color, currentExport.GetComponent<box>().ToString());
                }
            }
        }

    }

    public int GetTimeUnformatted() { return clock; }

    public string GetTime() {
        int hour = 0;
        int minute = clock;
        while (minute > 60) {
            minute -= 60;
            hour += 1;
        }
        return hour+":"+minute;
    }

    /// <summary>
    /// Returns if you can move to a position in the world.
    /// </summary>
    /// <param name="v">World position</param>
    /// <returns>If you can move to this position.</returns>
    public bool CanIMoveHere(Vector2 v) {
        v = new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));

        if (v.x > 11) { return false; }
        if (v.y > 5) { return false; }

        if (grid[Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y)] == null) { return true; }
        if (grid[Mathf.RoundToInt(v.x),Mathf.RoundToInt(v.y)].tag == "box") {return false;}
        return true;
        
    }

    public bool boxesWaiting() {
        if (impotBacklog > 0) {
            return true;
        }else { return false; }
    }

    void GridSetup(string [] gridInput) {
        int y = 0;
        while (y < 6) { 
            int x = 0;
            foreach (char s in gridInput[y]) {
                if (s == 'X') { grid[x, y] = Instantiate(floor_noBoxes, new Vector3(x, y * -1, 0), this.transform.rotation); }
                else if (s == 'O') { Instantiate(floor, new Vector3(x, y * -1, 0), this.transform.rotation); }
                else if (s == 'B')
                {
                    Instantiate(floor, new Vector3(x, y * -1, 0), this.transform.rotation);
                    grid[x, y] = Instantiate(permanentBox, new Vector3(x, y * -1, 0), this.transform.rotation);
                }
                else if (s == 'I')
                {
                    import = Instantiate(floor_imports, new Vector3(x, y * -1, 0), this.transform.rotation);   
                }
                else if (s == 'E') {
                    export = Instantiate(floor_exports, new Vector3(x, y * -1, 0), this.transform.rotation);
                    
                }
                else { Debug.LogError("Grid setup error."); }
                x++;
            }
            y++;
        }

        et = Instantiate(exportDoorPrefab, new Vector3(export.transform.position.x, 1, 0), this.transform.rotation).GetComponent<exportTruck>();
        Instantiate(importDoorPrefab, new Vector3(import.transform.position.x, 1, 0), this.transform.rotation);
    }

    public void ResetLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void GoToMenu() { Application.LoadLevel(0); }

}
