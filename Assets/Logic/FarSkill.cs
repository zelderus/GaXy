using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Скилл.
/// </summary>
public class FarSkill
{
    public Int32 Num { get; set; }
    public Boolean IsActivated { get; set; }

    public CityResourceFrom Materials { get; private set; }
    public CityResourceFrom Res1 { get; private set; }
    public CityResourceFrom Res2 { get; private set; }
    public CityResourceFrom Res3 { get; private set; }
    public CityResourceFrom Res4 { get; private set; }


    public FarSkill(Int32 num)
    {
        Num = num;
        Materials = new CityResourceFrom() { Type = CityRecources.Material, MustBeForProduct = 10 };
        Res1 = new CityResourceFrom() { Type = CityRecources.Res1, MustBeForProduct = 0 };
        Res2 = new CityResourceFrom() { Type = CityRecources.Res2, MustBeForProduct = 0 };
        Res3 = new CityResourceFrom() { Type = CityRecources.Res3, MustBeForProduct = 0 };
        Res4 = new CityResourceFrom() { Type = CityRecources.Res4, MustBeForProduct = 0 };
    }
    public FarSkill(Int32 num, Boolean isActivated) : this(num)
    {
        IsActivated = isActivated;
    }
    public FarSkill(Int32 num, Int32 mat, Int32 res1, Int32 res2, Int32 res3, Int32 res4) : this(num, false)
    {
        Materials.MustBeForProduct = mat;
        Res1.MustBeForProduct = res1;
        Res2.MustBeForProduct = res2;
        Res3.MustBeForProduct = res3;
        Res4.MustBeForProduct = res4;
    }

    /// <summary>
    /// Описание.
    /// </summary>
    /// <returns></returns>
    public String GetText()
    {
        return FarLife.GetSkillText(Num); //.GetText((FarText)(6000 + Num));
    }


    //public void LoadData(Boolean isActivated, Int32 mat, Int32 res1, Int32 res2, Int32 res3, Int32 res4)
    //{
    //    IsActivated = isActivated;
    //    Materials.MustBeForProduct = mat;
    //    Res1.MustBeForProduct = res1;
    //    Res2.MustBeForProduct = res2;
    //    Res3.MustBeForProduct = res3;
    //    Res4.MustBeForProduct = res4;
    //}
    public void LoadData(Boolean isActivated)
    {
        IsActivated = isActivated;
    }
}
