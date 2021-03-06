﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// Панель контента скилла - описание.
/// </summary>
public class PanelSkillContentLogic : MonoBehaviour {


    public Text TitleText;
    public Button BuyBtn;
    public MapController MapController;
    public PanelSkillWorkLogic ResourceLogic;

    private Int32 _skillNum = 0;
    private MapSkillBtnLogic _skillBtn;

    // Use this for initialization
    void Start ()
    {
	
	}

    public void Init()
    {
        
    }



    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SetView(MapSkillBtnLogic skillBtn)
    {
        _skillBtn = skillBtn;
        _skillNum = skillBtn.SkillNum;
        UpdateView();
    }
    /// <summary>
    /// Обновление данных.
    /// </summary>
    /// <param name="num"></param>
    public void UpdateView()
    {
        var skill = FarLife.ShipLife.GetSkill(_skillNum);
        if (skill == null) return;

        //+ text
        var txt = skill.GetText();
        TitleText.text = txt;

        //+ res
        ResourceLogic.UpdateBySkill(skill);

        UpdateBuyBtn();
    }

    private void UpdateBuyBtn()
    {
        //+ btn
        var hasAccessBuy = !_skillBtn.IsActivated && _skillBtn.IsAccessible;// false;
        if (!hasAccessBuy)
        {
            BuyBtn.gameObject.SetActive(false);
            //x BuyBtn.interactable = false;
        }
        else
        {
            BuyBtn.interactable = SkillHasEnough();
            BuyBtn.gameObject.SetActive(true);
        }

        //+ res
        if (_skillBtn.IsActivated) ResourceLogic.Hide();
        else ResourceLogic.Show();
    }


    /// <summary>
    /// Достаточно средств для текущего скилла.
    /// </summary>
    /// <returns></returns>
    private bool SkillHasEnough()
    {
        var skill = FarLife.ShipLife.GetSkill(_skillNum);
        if (skill == null) return false;

        var mapLife = MapController.GetMapLife();

        if (mapLife.IsResourceEnough(skill.Materials)
            && mapLife.IsResourceEnough(skill.Res1)
            && mapLife.IsResourceEnough(skill.Res2)
            && mapLife.IsResourceEnough(skill.Res3)
            && mapLife.IsResourceEnough(skill.Res4)
            ) return true;

        return false;
    }
    /// <summary>
    /// Приобритение скилла. Математика.
    /// </summary>
    private void SkillBuyDo()
    {
        var skill = FarLife.ShipLife.GetSkill(_skillNum);
        if (skill == null) return;

        var mapLife = MapController.GetMapLife();
        //- снимаем ресурсы
        mapLife.ResourceTrash(skill.Materials);
        mapLife.ResourceTrash(skill.Res1);
        mapLife.ResourceTrash(skill.Res2);
        mapLife.ResourceTrash(skill.Res3);
        mapLife.ResourceTrash(skill.Res4);
        //- запоминаем у скилла
        FarLife.ShipLife.SkillBuy(_skillNum);
    }


    /// <summary>
    /// Покупка текущего скилла.
    /// </summary>
    public void BuyClickDo()
    {
        if (!SkillHasEnough()) return;
        SkillBuyDo();

        _skillBtn.Activation();
        UpdateBuyBtn();

        MapController.SoundFig();
        MapController.UpdateMathPanels();
    }


    // Update is called once per frame
    void Update ()
    {
	
	}
}
