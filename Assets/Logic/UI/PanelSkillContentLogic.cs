using UnityEngine;
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


    private Int32 _skillNum = 0;

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

    public void SetView(Int32 num)
    {
        _skillNum = num;
        UpdateView();
    }
    /// <summary>
    /// Обновление данных.
    /// </summary>
    /// <param name="num"></param>
    public void UpdateView()
    {
        // TODO: _skillNum

        //+ text
        var txt = "Описание скилла.. олрвылоарыоарыло лывораловыраловы лоывраолывралоырвлаоы рлыоварлыоврал ырлаыоварлыовралоыва рлоыварлывоарл ывлоарывлрло алвыоралоывр ывлоралвыор арлыовралыв авылоар";
        TitleText.text = txt;

        UpdateBuyBtn();
    }

    private void UpdateBuyBtn()
    {
        // TODO: _skillNum

        //+ btn
        var isMax = false;
        if (isMax)
        {
            BuyBtn.gameObject.SetActive(false);
        }
        else
        {
            BuyBtn.gameObject.SetActive(true);
            //- has enought
            var hasEnought = true;
            BuyBtn.interactable = hasEnought;
        }
    }


    /// <summary>
    /// Покупка текущего скилла.
    /// </summary>
    public void BuyClickDo()
    {
        // TODO: _skillNum


        UpdateBuyBtn();
        MapController.UpdateMathPanels();
    }


    // Update is called once per frame
    void Update ()
    {
	
	}
}
