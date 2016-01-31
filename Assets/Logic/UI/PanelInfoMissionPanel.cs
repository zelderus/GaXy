using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Процесс в большом окне города.
/// </summary>
public class PanelInfoMissionPanel : MonoBehaviour
{
    public Text DayTitle;
    public Text RatingTitle;
    public Text MustResourceTitle;
    public Text ProductTitle;

    public Image ResFrom1Back;
    public Image ResFrom1MatImg;
    public Image ResFrom1Res1Img;
    public Image ResFrom1Res2Img;
    public Image ResFrom1Res3Img;
    public Image ResFrom1Res4Img;

    public Image ResFrom2Back;
    public Image ResFrom2Res1Img;
    public Image ResFrom2Res2Img;
    public Image ResFrom2Res3Img;
    public Image ResFrom2Res4Img;

    public Image ResToRes1Img;
    public Image ResToRes2Img;
    public Image ResToRes3Img;
    public Image ResToRes4Img;


    public Text Res1Txt;
    public Text Res2Txt;
    public Text DaysLeftTxt;
    public Text RatingTxt;
    public Text ProductCountTxt;


    public Color ResSuccessColor = new Color(100, 255, 100);
    public Color ResNotEnoughColor = new Color(255, 100, 100);


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
    public Transform StartProductBtn;
    public Button StartButton;

    private PanelActionLogic _actionPanel;
    private City _city = null;
    private Boolean _isCityCurrent = false;

	// Use this for initialization
    void Awake()
    {
        //StartButton = StartProductBtn.GetComponent<Button>();
    }
	void Start () 
    {

	}


    public void Init()
    {
        DayTitle.text = FarLife.GetText(FarText.Map_InfoWnd_DayTitle);
        RatingTitle.text = FarLife.GetText(FarText.Map_InfoWnd_RatingTitle);
        MustResourceTitle.text = FarLife.GetText(FarText.Map_InfoWnd_ResTitle);
        ProductTitle.text = FarLife.GetText(FarText.Map_InfoWnd_ProdTitle);
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(PanelActionLogic actionPanel, City city, bool isCurrent)
    {
        if (city == null) return;
        _actionPanel = actionPanel;
        _city = city;
        _isCityCurrent = isCurrent;

        ShowContentCity(city);
        ShowActionPanel(isCurrent);
        //+ вид
        UpdateView();

        this.gameObject.SetActive(true);
    }

    
    private void ShowContentCity(City city)
    {
        var model = city.Model;

        //+ resources from
        ResFrom1Back.gameObject.SetActive(false);
        ResFrom1MatImg.gameObject.SetActive(false);
        ResFrom1Res1Img.gameObject.SetActive(false);
        ResFrom1Res2Img.gameObject.SetActive(false);
        ResFrom1Res3Img.gameObject.SetActive(false);
        ResFrom1Res4Img.gameObject.SetActive(false);

        ResFrom2Back.gameObject.SetActive(false);
        ResFrom2Res1Img.gameObject.SetActive(false);
        ResFrom2Res2Img.gameObject.SetActive(false);
        ResFrom2Res3Img.gameObject.SetActive(false);
        ResFrom2Res4Img.gameObject.SetActive(false);

        ResToRes1Img.gameObject.SetActive(false);
        ResToRes2Img.gameObject.SetActive(false);
        ResToRes3Img.gameObject.SetActive(false);
        ResToRes4Img.gameObject.SetActive(false);

        //+ resources from
        var indx = 0;
        foreach (var resFrom in model.ResourceProduct.Resources)
        {
            indx++;
            if (indx == 1)
            {
                ResFrom1Back.gameObject.SetActive(true);
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
                ResFrom2Back.gameObject.SetActive(true);
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
        //UpdateView();


        //- icon
        switch (model.ResourceProduct.Type)
        {
            case CityRecources.Res1: ResToRes1Img.gameObject.SetActive(true); break;
            case CityRecources.Res2: ResToRes2Img.gameObject.SetActive(true); break;
            case CityRecources.Res3: ResToRes3Img.gameObject.SetActive(true); break;
            case CityRecources.Res4: ResToRes4Img.gameObject.SetActive(true); break;
        }
    }

    /// <summary>
    /// Панель управления. Находимся в этом городе.
    /// </summary>
    private void ShowActionPanel(Boolean isCurrent)
    {
        if (_city == null) return;
        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();

        if (isCurrent)
        {
            StartProductBtn.gameObject.SetActive(true);
        }
        else
        {
            StartProductBtn.gameObject.SetActive(false);
        }

    }




    /// <summary>
    /// Запуск производства.
    /// </summary>
    public void StartProduct()
    {
        if (_city == null) return;
        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();
        //- производим
        mapLife.CityStart(_city.Model);
        //- обновляем все зависящее внешне
        //mapLife.UpdateViewDay();
        //_city.CityMap.MainLogicObject.WorkPanelUpdate();
        //_actionPanel.UpdateView();
        //_city.CityMap.MainLogicObject.PanelCitySmall.UpdateView();
        _city.CityMap.MainLogicObject.UpdateMathPanels();
        _city.CityMap.MainLogicObject.SoundUp();
    }

    private void UpdateStatStartProductBtn()
    {
        if (_city == null) return;
        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();

        var canProduct = mapLife.ShipCanStartProductInCity(_city.Model);
        //var btn = StartProductBtn.GetComponent<Button>();
        if (canProduct)
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }

    }



    public void UpdateView()
    {
        if (_city == null) return;
        var model = _city.Model;

        //+ days
        if (_city.Model.HasProcess())
        {
            //DaysLeftTxt.gameObject.SetActive(true);
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

        RatingTxt.text = _city.Model.Rating.ToString();


        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();

        Res2Txt.gameObject.SetActive(false);

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
        ProductCountTxt.text = _city.Model.ResourceProduct.ProductionCount.ToString("+0");


        UpdateStatStartProductBtn();
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
	void Update () 
    {
	
	}
}
