using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class LevelMarketBtnLogic : MonoBehaviour {

    public Text CostTxt;
    public Button Btn;

    public Int32 Cost { get; private set; }

    private CityResourceFrom _res;

	// Use this for initialization
	void Start () {
	
	}


    public void Init(Int32 cost)
    {
        Cost = cost;
        CostTxt.text = Cost.ToString();

        _res = new CityResourceFrom();
        _res.Type = CityRecources.Material;
        _res.MustBeForProduct = cost;
    }

    public bool IsEnough()
    {
        return FarLife.MapLife.IsResourceEnough(_res);
    }

    public void UpdateView()
    {
        if (IsEnough())
        {
            Btn.interactable = true;
        }
        else
        {
            Btn.interactable = false;
        }
    }

    	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
