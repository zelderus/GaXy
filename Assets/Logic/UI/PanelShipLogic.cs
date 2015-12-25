using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Панель коробля. Навыков.
/// </summary>
public class PanelShipLogic : MonoBehaviour {

    public Boolean IsShowed { get; private set; }


    public Text TitleTxt;



    // Use this for initialization
    void Start()
    {
        TitleTxt.text = FarLife.GetText(FarText.Map_SkillTitle);
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
