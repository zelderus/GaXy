using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelPanelOptionLogic : MonoBehaviour {

    public Boolean IsShowed { get; private set; }

    public Text PauseText;

    // Use this for initialization
    void Start()
    {

    }

    public void Init()
    {
        PauseText.text = FarLife.GetText(FarText.Level_Pause);
    }


    public void Hide()
    {
        IsShowed = false;
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        IsShowed = true;
        this.gameObject.SetActive(true);
    }




    // Update is called once per frame
    void Update()
    {

    }
}
