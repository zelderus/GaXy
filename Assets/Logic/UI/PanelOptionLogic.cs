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
        // автоматически привязываем (для избежания вручную в редакторе)
        var content = transform.Find("SkillPanel/Wrapper/ContentPanel").transform;
        foreach (Transform child in content)
        {
            var skillItem = child.GetComponent<MapSkillBtnLogic>();
            skillItem.Init(this);
        }
        foreach (Transform child in content)
        {
            var skillItem = child.GetComponent<MapSkillBtnLogic>();
            skillItem.InitActivation();
        }
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

        if (_currentSkillBtn != null) ContentPanel.UpdateView();

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

        ContentPanel.SetView(skillBtn);
        ContentPanel.Show();
    }




	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
