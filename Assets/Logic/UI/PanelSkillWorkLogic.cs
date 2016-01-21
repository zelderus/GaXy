using UnityEngine;
using System.Collections;
using System;

public class PanelSkillWorkLogic : MonoBehaviour {

    public PanelSkillWorkResourceBlockLogic MatBlock;
    public PanelSkillWorkResourceBlockLogic Res1Block;
    public PanelSkillWorkResourceBlockLogic Res2Block;
    public PanelSkillWorkResourceBlockLogic Res3Block;
    public PanelSkillWorkResourceBlockLogic Res4Block;


    // Use this for initialization
    void Start()
    {

    }





    public void Init()
    {
        SetMat(0, false);
        SetRes1(0, false);
        SetRes2(0, false);
        SetRes3(0, false);
        SetRes4(0, false);

    }



    public void UpdateBySkill(FarSkill skill)
    {
        SetMat(skill.Materials.MustBeForProduct, FarLife.MapLife.IsResourceEnough(skill.Materials));
        SetRes1(skill.Res1.MustBeForProduct, FarLife.MapLife.IsResourceEnough(skill.Res1));
        SetRes2(skill.Res2.MustBeForProduct, FarLife.MapLife.IsResourceEnough(skill.Res2));
        SetRes3(skill.Res3.MustBeForProduct, FarLife.MapLife.IsResourceEnough(skill.Res3));
        SetRes4(skill.Res4.MustBeForProduct, FarLife.MapLife.IsResourceEnough(skill.Res4));
    }


    public void SetMat(Int32 count, bool isEnough)
    {
        MatBlock.SetCount(count, isEnough);
    }
    public void SetRes1(Int32 count, bool isEnough)
    {
        Res1Block.SetCount(count, isEnough);
    }
    public void SetRes2(Int32 count, bool isEnough)
    {
        Res2Block.SetCount(count, isEnough);
    }
    public void SetRes3(Int32 count, bool isEnough)
    {
        Res3Block.SetCount(count, isEnough);
    }
    public void SetRes4(Int32 count, bool isEnough)
    {
        Res4Block.SetCount(count, isEnough);
    }



    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
