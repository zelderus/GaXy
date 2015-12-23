using System;
using UnityEngine;
using System.Collections;

public class LevelPanelOptionLogic : MonoBehaviour {

    public Boolean IsShowed { get; private set; }


    // Use this for initialization
    void Start()
    {

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
