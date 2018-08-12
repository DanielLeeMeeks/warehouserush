using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class box : MonoBehaviour {

    public bool unMovable;
    public TextMeshProUGUI lable;
    public Color[] boxColors;
    string id = "";
    int exportTime;
    int importTime;
    gameManager gm;
    exportTruck et;

    // Use this for initialization
    void Start() {

        gm = GameObject.Find("Main Camera").GetComponent<gameManager>();

        if (unMovable) { this.GetComponent<SpriteRenderer>().color = new Color(0.48f, 0.27f, 0f); lable.text = "Do not move!"; }
        else {
            this.GetComponent<SpriteRenderer>().color = boxColors[Random.Range(0, boxColors.Length)];
            exportTime = gm.GetTimeUnformatted() + Random.Range(10, 45);
            Debug.Log(exportTime);
            createID();
        }

        importTime = gm.GetTimeUnformatted();

        et = GameObject.Find("exports_Truck(Clone)").GetComponent<exportTruck>();
    }

    void createID() {
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] c = letters.ToCharArray();
        int i = 5;
        while (i > 0) {
            id = id + c[Random.Range(0, c.Length)];
            i--;
        }
        lable.text = id;
    }

    public int GetTimeUnformatted() { return exportTime; }
    public string GetImportTime() { return FormatTime(importTime); }
    public string GetExportTime() { return FormatTime(exportTime); }

    public string FormatTime(int clock)
    {
        int hour = 0;
        int minute = clock;
        while (minute > 60)
        {
            minute -= 60;
            hour += 1;
        }
        return hour + ":" + minute;
    }

    public IEnumerator exportBox() {
        yield return new WaitForSecondsRealtime(0.25f);
        Destroy(this.gameObject);
        //et.removeTruck();
    }

    override public string ToString() { return id; }

}
