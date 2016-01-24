using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PanelHelpLogic : MonoBehaviour {

    public GameObject PanelWelcome;
    public GameObject PanelHelper;

    public Text Welcome_TitleText;
    public Text Welcome_DescText;

    public Text Help_TitleText;
    public Text Help_Desc1Text;
    public Text Help_Desc2Text;
    public Text Help_Desc3Text;
    public Text Help_Desc4Text;

    public bool IsShowed { get; private set; }

	// Use this for initialization
	void Start () {
	
	}


    public void Init()
    {
        //- welcome
        Welcome_TitleText.text = FarLife.GetText(FarText.Map_Help_WelcomeTitle);
        Welcome_DescText.text = FarLife.GetText(FarText.Map_Help_WelcomeDesc);
        //- help
        Help_TitleText.text = FarLife.GetText(FarText.Map_Help_HelpTitle);
        Help_Desc1Text.text = FarLife.GetText(FarText.Map_Help_HelpDesc1);
        Help_Desc2Text.text = FarLife.GetText(FarText.Map_Help_HelpDesc2);
        Help_Desc3Text.text = FarLife.GetText(FarText.Map_Help_HelpDesc3);
        Help_Desc4Text.text = FarLife.GetText(FarText.Map_Help_HelpDesc4);
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
        IsShowed = false;
    }

    public void Show(Int32 tag)
    {
        if (tag == 1)
        {
            PanelWelcome.SetActive(true);
            PanelHelper.SetActive(false);
        }
        else
        {
            PanelWelcome.SetActive(false);
            PanelHelper.SetActive(true);
        }

        this.gameObject.SetActive(true);
        IsShowed = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
