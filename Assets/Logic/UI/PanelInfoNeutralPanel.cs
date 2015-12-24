using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PanelInfoNeutralPanel : MonoBehaviour
{

    public Text Res1Txt;
    public Image ResFrom1ResImg;
    public Text Res2Txt;
    public Image ResFrom2ResImg;
    public Text Res3Txt;
    public Image ResFrom3ResImg;
    public Text Res4Txt;
    public Image ResFrom4ResImg;
    public Color ResSuccessColor;
    public Color ResNotEnoughColor;

    public Transform Res1Btn;
    //public Transform Res2Btn;
    //public Transform Res3Btn;
    //public Transform Res4Btn;

    private PanelActionLogic _actionPanel;
    private City _city = null;
    private Boolean _isCurrent;

	// Use this for initialization
	void Start () {
	
	}


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Отображение панели.
    /// </summary>
    public void Show(PanelActionLogic actionPanel, City city, bool isCurrent)
    {
        if (city == null) return;
        _isCurrent = isCurrent;
        _actionPanel = actionPanel;
        _city = city;

        UpdateView();

        this.gameObject.SetActive(true);
    }


    /// <summary>
    /// Построение своего города с типом.
    /// </summary>
    public void BuildCityByType()
    {
        if (_city == null) return;
        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();

        var cityResource = _city.Model.ResourceProduct.Type;


        if (!_city.Model.IsJop) //! NORMAL
        {
            //+ тратим
            if (!mapLife.ShipHaveEnoughForQuestCity(_city)) return;
            mapLife.QuestCityStart(_city.Model);
            //+ строим новый тип
            _city.SetFriendCityType(cityResource);
        }
        else    //! JOP
        {
            //+ тратим
            if (!mapLife.ShipHaveEnoughForQuestCity(_city)) return;
            mapLife.QuestJopStart(_city.Model);
            //+ следующий
            _city.SetJopQuestCompleted();
            //+ если выполнено все
            if (_city.Model.IsJopCompleted)
            {
                _city.CityMap.MainLogicObject.GetShipLife().SetJopCompleted();
                _city.CityMap.JopFullCompleted();
                _city.CityMap.MainLogicObject.JopCompletedOn(_city.CityMap);
            }
        }


        //- обновляем все зависящее внешне
        _city.CityMap.MainLogicObject.UpdateMathPanels();
    }


    public void UpdateView()
    {
        if (_city == null) return;
        var model = _city.Model;
        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();

        //+ resources from
        Image img = ResFrom1ResImg;
        Text txt = Res1Txt;
        if (!model.IsJop)   //! NORMAL
        {
            foreach (var questRes in model.QuestResources)
            {
                QuestView(questRes, mapLife, img, txt);
            }
        }
        else                //! JOP
        {
            var jopQuest = model.GetCurrentJopQuestList();
            foreach (var questRes in jopQuest)
            {
                QuestView(questRes, mapLife, img, txt);
            }
        }



        //+ btns
        if (_isCurrent)
        {
            Res1Btn.gameObject.SetActive(true);

            
            if (mapLife.ShipHaveEnoughForQuestCity(_city))
            {
                Res1Btn.GetComponent<Button>().interactable = true;
            }
            else
            {
                Res1Btn.GetComponent<Button>().interactable = false;
            }
            
            //! JOP (если выполнены все задачи)
            if (_city.Model.IsJop && _city.Model.IsJopCompleted)
            {
                Res1Btn.gameObject.SetActive(false);
            }

            

        }
        else
        {
            Res1Btn.gameObject.SetActive(false);
        }

    }



    private void QuestView(CityResourceFrom questRes, MapLife mapLife, Image img, Text txt)
    {
        switch (questRes.Type)
        {
            case CityRecources.Res1: img = ResFrom1ResImg; txt = Res1Txt; break;
            case CityRecources.Res2: img = ResFrom2ResImg; txt = Res2Txt; break;
            case CityRecources.Res3: img = ResFrom3ResImg; txt = Res3Txt; break;
            case CityRecources.Res4: img = ResFrom4ResImg; txt = Res4Txt; break;
        }
        if (questRes.MustBeForProduct > 0)
        {
            img.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);
            txt.text = questRes.MustBeForProduct.ToString();
            txt.color = ResSuccessColor;
            if (!mapLife.IsResourceEnough(questRes))
            {
                txt.color = ResNotEnoughColor;
            }
        }
        else
        {
            img.gameObject.SetActive(false);
            txt.gameObject.SetActive(false);
        }
    }



	// Update is called once per frame
	void Update () {
	
	}
}
