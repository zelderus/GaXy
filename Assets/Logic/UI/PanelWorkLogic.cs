using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Общая панель корабля.
/// </summary>
public class PanelWorkLogic : MonoBehaviour
{

    public PanelWorkResourceBlockLogic MatBlock;
    public PanelWorkResourceBlockLogic Res1Block;
    public PanelWorkResourceBlockLogic Res2Block;
    public PanelWorkResourceBlockLogic Res3Block;
    public PanelWorkResourceBlockLogic Res4Block;


	// Use this for initialization
	void Start () {
	
	}





    public void Init()
    {
        SetMat(0);
        SetRes1(0);
        SetRes2(0);
        SetRes3(0);
        SetRes4(0);

    }


    public void SetMat(Int32 count)
    {
        MatBlock.SetCount(count);
    }
    public void SetRes1(Int32 count)
    {
        Res1Block.SetCount(count);
    }
    public void SetRes2(Int32 count)
    {
        Res2Block.SetCount(count);
    }
    public void SetRes3(Int32 count)
    {
        Res3Block.SetCount(count);
    }
    public void SetRes4(Int32 count)
    {
        Res4Block.SetCount(count);
    }
    public void SetRes(CityResourceShip res)
    {
        switch (res.Type)
        {
            case CityRecources.Material: SetMat(res.CurrentCount); break;
            case CityRecources.Res1: SetRes1(res.CurrentCount); break;
            case CityRecources.Res2: SetRes2(res.CurrentCount); break;
            case CityRecources.Res3: SetRes3(res.CurrentCount); break;
            case CityRecources.Res4: SetRes4(res.CurrentCount); break;
        }
    }


    // Update is called once per frame
	void Update () {
	
	}
}




