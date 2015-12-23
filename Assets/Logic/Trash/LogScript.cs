using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogScript : MonoBehaviour {


    public Text LogText;

	// Use this for initialization
	void Start () {
        LogText = this.GetComponent<Text>();
	}



    public void SetText(string msg)
    {
        LogText.text = msg;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
