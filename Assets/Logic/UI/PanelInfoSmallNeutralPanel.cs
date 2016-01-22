using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelInfoSmallNeutralPanel : MonoBehaviour
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

    private City _city;

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
        
        //+ вид
        UpdateView();

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


    }


    private void QuestView(CityResourceFrom questRes, MapLife mapLife, Image img, Text txt)
    {
        switch (questRes.Type)
        {
            case CityRecources.Res1:
                img = ResFrom1ResImg;
                txt = Res1Txt;
                break;
            case CityRecources.Res2:
                img = ResFrom2ResImg;
                txt = Res2Txt;
                break;
            case CityRecources.Res3:
                img = ResFrom3ResImg;
                txt = Res3Txt;
                break;
            case CityRecources.Res4:
                img = ResFrom4ResImg;
                txt = Res4Txt;
                break;
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
