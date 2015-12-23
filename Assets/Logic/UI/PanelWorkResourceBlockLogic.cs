using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;



/// <summary>
/// Блок Ресурса на панели.
/// </summary>
public class PanelWorkResourceBlockLogic : MonoBehaviour
{
    public Image Back;
    public Image ResImg;
    public Text ResTxt;

    public Color AlphaFull = new Color(1, 1, 1, 1);
    public Color AlphaNone = new Color(1, 1, 1, 0.3f);


    // Use this for initialization
    void Start()
    {

    }



    /// <summary>
    /// Количество.
    /// </summary>
    /// <param name="count"></param>
    public void SetCount(Int32 count)
    {
        ResTxt.text = count.ToString();
        SetHave(count > 0);
    }

    private void SetHave(Boolean isHave)
    {
        Back.color = isHave ? AlphaFull : AlphaNone;
        ResImg.color = isHave ? AlphaFull : AlphaNone;
        ResTxt.color = isHave ? AlphaFull : AlphaNone;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
