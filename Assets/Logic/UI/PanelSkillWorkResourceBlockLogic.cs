using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// Блок ресурса на панели скиллов покупки.
/// </summary>
public class PanelSkillWorkResourceBlockLogic : MonoBehaviour {

    public Image Back;
    public Image ResImg;
    public Text ResTxt;

    public Color NormalColor = new Color(1, 1, 1, 1);
    public Color NotEnoughColor = new Color(1, 0, 0, 1);



    // Use this for initialization
    void Start()
    {

    }



    /// <summary>
    /// Количество.
    /// </summary>
    public void SetCount(Int32 count, bool isEnough)
    {
        ResTxt.text = count.ToString();
        ResTxt.color = isEnough ? NormalColor : NotEnoughColor;
        SetHave(count > 0);
    }

    private void SetHave(Boolean isHave)
    {
        //Back.color = isHave ? AlphaFull : AlphaNone;
        //ResImg.color = isHave ? AlphaFull : AlphaNone;
        //ResTxt.color = isHave ? AlphaFull : AlphaNone;
        this.gameObject.SetActive(isHave);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
