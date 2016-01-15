using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// Кнопка скилла.
/// </summary>
public class MapSkillBtnLogic : MonoBehaviour {


    public Int32 SkillNum = 0;
    public PanelOptionLogic PanelOption;


    public Color NormalColor = Color.white;
    public Color CurrentColor = Color.green;

	// Use this for initialization
	void Start ()
    {
	
	}
	
    /// <summary>
    /// Нажали.
    /// </summary>
    public void ClickDo()
    {
        PanelOption.ShowSkillContent(this);
    }


    public void ViewAsCurrent()
    {
        // TODO:
        this.gameObject.GetComponent<Image>().color = CurrentColor;
    }
    public void ViewAsNotCurrent()
    {
        // TODO:
        this.gameObject.GetComponent<Image>().color = NormalColor;
    }


	// Update is called once per frame
	void Update ()
    {
	
	}
}
