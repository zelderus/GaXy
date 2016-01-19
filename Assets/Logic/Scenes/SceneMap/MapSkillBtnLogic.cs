using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Кнопка скилла.
/// </summary>
public class MapSkillBtnLogic : MonoBehaviour {


    public Int32 SkillNum = 0;
    public Color ActivatedColor = Color.yellow;
    public Color NormalColor = Color.white;
    public Color CurrentColor = Color.green;
    public Color NotAccessColor = new Color(Color.white.r, Color.white.g, Color.white.b, 0.4f);
    public List<MapSkillBtnLogic> NearButtons;

    public bool IsActivated { get; private set; }
    public bool IsAccessible { get; private set; }


    private PanelOptionLogic _panelOption;
    private Image _backImage;

    // Use this for initialization
    void Start ()
    {
	
	}



    public void Init(PanelOptionLogic panel)
    {
        _panelOption = panel;
        _backImage = this.gameObject.GetComponent<Image>();
    }

    public void InitActivation()
    {
        //+ установка из загрузки
        if (FarLife.ShipLife.SkillIsActivated(SkillNum)) Activation();
        ViewAsNotCurrent();
    }



    /// <summary>
    /// Нажали.
    /// </summary>
    public void ClickDo()
    {
        if (SkillNum <= 0) return;
        _panelOption.ShowSkillContent(this);
    }



    /// <summary>
    /// Активация кнопки. Доступ к соседним.
    /// </summary>
    public void Activation()
    {
        if (IsActivated) return;    //+ если уже были активированы, больше нет надобности
        IsActivated = true;
        foreach (var nearBtn in NearButtons)
        {
            nearBtn.Accessable(true, true);
        }
    }

    /// <summary>
    /// Установка доступа и вида.
    /// </summary>
    private void Accessable(bool isAccess, bool withUpdateView = true)
    {
        IsAccessible = isAccess;
        if (withUpdateView) ViewAsAccessible();
    }




    public void ViewAsAccessible()
    {
        if (IsActivated)
        {
            _backImage.color = ActivatedColor;
        }
        else
        {
            //+ доступен
            if (IsAccessible) _backImage.color = NormalColor;
            else _backImage.color = NotAccessColor;
        }
    }
    public void ViewAsCurrent()
    {
        //+ выбран
        _backImage.color = CurrentColor;
    }
    public void ViewAsNotCurrent()
    {
        //+ не выбран
        ViewAsAccessible();
    }







	// Update is called once per frame
	void Update ()
    {
	
	}
}
