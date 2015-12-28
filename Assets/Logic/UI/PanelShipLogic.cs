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


    private bool _hasChange = false;


    // Use this for initialization
    void Start()
    {
        TitleTxt.text = FarLife.GetText(FarText.Map_SkillTitle);
    }



    private void OnHide()
    {
        if (!_hasChange) return;
        FarLife.SaveGame();
    }


    public void Hide()
    {
        IsShowed = false;
        OnHide();

        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        IsShowed = true;
        _hasChange = false;
        this.gameObject.SetActive(true);
    }




    // Update is called once per frame
    void Update()
    {

    }
}
