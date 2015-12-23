using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelInfoSmallMissionPanel : MonoBehaviour
{
    public Image ResFrom1MatImg;
    public Image ResFrom1Res1Img;
    public Image ResFrom1Res2Img;
    public Image ResFrom1Res3Img;
    public Image ResFrom1Res4Img;

    public Image ResFrom2Res1Img;
    public Image ResFrom2Res2Img;
    public Image ResFrom2Res3Img;
    public Image ResFrom2Res4Img;

    public Image ResToRes1Img;
    public Image ResToRes2Img;
    public Image ResToRes3Img;
    public Image ResToRes4Img;
    //public Image ResToResBlackImg;


    public Text Res1Txt;
    public Text Res2Txt;
    //public Text ProTxt;
    public Text DaysLeftTxt;
    public Text ProductCountTxt;
    //public Text Res1MustTxt;
    //public Text Res2MustTxt;

    public Color ResSuccessColor;
    public Color ResNotEnoughColor;


    public Image Proc1WorkEmptyImg;
    public Image Proc2WorkEmptyImg;
    public Image Proc3WorkEmptyImg;
    public Image Proc4WorkEmptyImg;
    public Image Proc5WorkEmptyImg;
    public Image Proc1WorkImg;
    public Image Proc2WorkImg;
    public Image Proc3WorkImg;
    public Image Proc4WorkImg;
    public Image Proc5WorkImg;
    public Color ProcHaveColor;
    public Color ProcNotColor;

    private City _city = null;


	// Use this for initialization
	void Start () {
	
	}


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(City city)
    {
        if (city == null) return;
        _city = city;
        ShowContentCity(city);
        this.gameObject.SetActive(true);
    }


    private void ShowContentCity(City city)
    {
        var model = city.Model;

        //+ resources from
        ResFrom1MatImg.gameObject.SetActive(false);
        ResFrom1Res1Img.gameObject.SetActive(false);
        ResFrom1Res2Img.gameObject.SetActive(false);
        ResFrom1Res3Img.gameObject.SetActive(false);
        ResFrom1Res4Img.gameObject.SetActive(false);

        ResFrom2Res1Img.gameObject.SetActive(false);
        ResFrom2Res2Img.gameObject.SetActive(false);
        ResFrom2Res3Img.gameObject.SetActive(false);
        ResFrom2Res4Img.gameObject.SetActive(false);

        ResToRes1Img.gameObject.SetActive(false);
        ResToRes2Img.gameObject.SetActive(false);
        ResToRes3Img.gameObject.SetActive(false);
        ResToRes4Img.gameObject.SetActive(false);
        //ResToResBlackImg.gameObject.SetActive(false);

        //+ resources from
        var indx = 0;
        foreach (var resFrom in model.ResourceProduct.Resources)
        {
            indx++;
            if (indx == 1)
            {
                //- icon
                switch (resFrom.Type)
                {
                    case CityRecources.Material: ResFrom1MatImg.gameObject.SetActive(true); break;
                    case CityRecources.Res1: ResFrom1Res1Img.gameObject.SetActive(true); break;
                    case CityRecources.Res2: ResFrom1Res2Img.gameObject.SetActive(true); break;
                    case CityRecources.Res3: ResFrom1Res3Img.gameObject.SetActive(true); break;
                    case CityRecources.Res4: ResFrom1Res4Img.gameObject.SetActive(true); break;
                }
            }
            else if (indx == 2)
            {
                //- icon
                switch (resFrom.Type)
                {
                    case CityRecources.Res1: ResFrom2Res1Img.gameObject.SetActive(true); break;
                    case CityRecources.Res2: ResFrom2Res2Img.gameObject.SetActive(true); break;
                    case CityRecources.Res3: ResFrom2Res3Img.gameObject.SetActive(true); break;
                    case CityRecources.Res4: ResFrom2Res4Img.gameObject.SetActive(true); break;
                }
            }
        }

        //+ вид
        UpdateView();


        //- icon
        switch (model.ResourceProduct.Type)
        {
            case CityRecources.Res1: ResToRes1Img.gameObject.SetActive(true); break;
            case CityRecources.Res2: ResToRes2Img.gameObject.SetActive(true); break;
            case CityRecources.Res3: ResToRes3Img.gameObject.SetActive(true); break;
            case CityRecources.Res4: ResToRes4Img.gameObject.SetActive(true); break;
            //case CityRecources.Black: ResToResBlackImg.gameObject.SetActive(true); break;
        }
    }

    public void UpdateView()
    {
        if (_city == null) return;
        var model = _city.Model;

        //+ days
        if (_city.Model.HasProcess())
        {
            DaysLeftTxt.gameObject.SetActive(true);
            DaysLeftTxt.text = _city.Model.DaysToCollect.ToString();

            //ProductCountTxt.gameObject.SetActive(true);
            //ProductCountTxt.text = _city.Model.ResourceProduct.ProductionCount.ToString("+0");
        }
        else
        {
            //DaysLeftTxt.gameObject.SetActive(false);
            DaysLeftTxt.text = _city.Model.ResourceProduct.ProductDays.ToString();
            //ProductCountTxt.gameObject.SetActive(false);
        }

        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();

        Res2Txt.gameObject.SetActive(false);
        //Res1MustTxt.gameObject.SetActive(false);
        //Res2MustTxt.gameObject.SetActive(false);

        //+ resources from
        var indx = 0;
        foreach (var resFrom in model.ResourceProduct.Resources)
        {
            indx++;
            if (indx == 1)
            {
                Res1Txt.text = resFrom.MustBeForProduct.ToString();
                Res1Txt.color = ResSuccessColor;
                Res1Txt.gameObject.SetActive(true);
                //- must
                if (!mapLife.IsResourceEnough(resFrom))
                {
                    Res1Txt.color = ResNotEnoughColor;
                }
            }
            else if (indx == 2)
            {
                Res2Txt.text = resFrom.MustBeForProduct.ToString();
                Res2Txt.color = ResSuccessColor;
                Res2Txt.gameObject.SetActive(true);
                //- must
                if (!mapLife.IsResourceEnough(resFrom))
                {
                    Res2Txt.color = ResNotEnoughColor;
                }
            }
        }
        //+ resources to
        //ProTxt.text = model.ResourceProduct.CurrentCount.ToString();
        ProductCountTxt.text = _city.Model.ResourceProduct.ProductionCount.ToString("+0");


        //+ proc
        var maxProc = model.ResourceProduct.MaxProcesses;
        Proc1WorkEmptyImg.color = maxProc >= 1 ? ProcHaveColor : ProcNotColor;
        Proc2WorkEmptyImg.color = maxProc >= 2 ? ProcHaveColor : ProcNotColor;
        Proc3WorkEmptyImg.color = maxProc >= 3 ? ProcHaveColor : ProcNotColor;
        Proc4WorkEmptyImg.color = maxProc >= 4 ? ProcHaveColor : ProcNotColor;
        Proc5WorkEmptyImg.color = maxProc >= 5 ? ProcHaveColor : ProcNotColor;

        Proc1WorkImg.gameObject.SetActive(false);
        Proc2WorkImg.gameObject.SetActive(false);
        Proc3WorkImg.gameObject.SetActive(false);
        Proc4WorkImg.gameObject.SetActive(false);
        Proc5WorkImg.gameObject.SetActive(false);

        if (_city.Model.ResourceProduct.ProcessStarted >= 1) Proc1WorkImg.gameObject.SetActive(true);
        if (_city.Model.ResourceProduct.ProcessStarted >= 2) Proc2WorkImg.gameObject.SetActive(true);
        if (_city.Model.ResourceProduct.ProcessStarted >= 3) Proc3WorkImg.gameObject.SetActive(true);
        if (_city.Model.ResourceProduct.ProcessStarted >= 4) Proc4WorkImg.gameObject.SetActive(true);
        if (_city.Model.ResourceProduct.ProcessStarted >= 5) Proc5WorkImg.gameObject.SetActive(true);
   
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
