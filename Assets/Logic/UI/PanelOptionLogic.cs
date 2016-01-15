using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelOptionLogic : MonoBehaviour {



    public Boolean IsShowed { get; private set; }

    public PanelSkillLogic SkillPanel;
    public Text TitleSkillTxt;


	// Use this for initialization
	void Start ()
    {
        TitleSkillTxt.text = FarLife.GetText(FarText.Map_SkillTitle);

	}


    public void Init()
    {
        SkillPanel.Init();
    }



    public void Hide()
    {
        IsShowed = false;
        this.gameObject.SetActive(false);
        SkillPanel.SetShowed(false);
    }

    public void Show()
    {
        IsShowed = true;
        this.gameObject.SetActive(true);
        SkillPanel.SetShowed(true);
    }



	
	// Update is called once per frame
	void Update () {
	
	}
}
