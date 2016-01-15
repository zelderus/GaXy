using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



/// <summary>
/// Большая панель опций со скилами.
/// </summary>
public class PanelOptionLogic : MonoBehaviour {



    public Boolean IsShowed { get; private set; }

    public PanelSkillContentLogic ContentPanel;
    public PanelSkillLogic SkillPanel;
    public Text TitleSkillTxt;


    private MapSkillBtnLogic _currentSkillBtn = null;

    // Use this for initialization
    void Start ()
    {
        TitleSkillTxt.text = FarLife.GetText(FarText.Map_SkillTitle);

	}


    public void Init()
    {
        SkillPanel.Init();
        ContentPanel.Init();
        ContentPanel.Hide();
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


    public void HideSkillContent()
    {
        ContentPanel.Hide();
    }
    /// <summary>
    /// Отображение контента скилла.
    /// </summary>
    /// <param name="skillBtn"></param>
    public void ShowSkillContent(MapSkillBtnLogic skillBtn)
    {
        if (_currentSkillBtn != null) _currentSkillBtn.ViewAsNotCurrent();
        _currentSkillBtn = skillBtn;
        skillBtn.ViewAsCurrent();

        ContentPanel.SetView(skillBtn.SkillNum);
        ContentPanel.Show();
    }




	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
