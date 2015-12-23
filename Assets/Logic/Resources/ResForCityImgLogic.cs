using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResForCityImgLogic : MonoBehaviour
{


    public Transform ObjectToFollow = null;
    public RectTransform RectTrans;

    public Image BackRedImg;
    public Image BackGreenImg;

    public Image Res1Img;
    public Image Res2Img;
    public Image Res3Img;
    public Image Res4Img;

    public Transform IcomePanel;
    public Image CountImg;
    public Text CountTxt;

    public Image CanIncomeImg;

    private City _cityModel;
    private MapLife _mapLife;

	// Use this for initialization
	void Start () {
	
	}


    public void SetCity(Transform objectToFollow, City cityModel, MapLife mapLife)
    {
        _mapLife = mapLife;
        ObjectToFollow = objectToFollow;
        SetCityModel(cityModel);
        UpdatePosition();
    }


    /// <summary>
    /// Установка типа города.
    /// </summary>
    /// <param name="cityModel"></param>
    public void SetCityModel(City cityModel)
    {
        _cityModel = cityModel;
        SetImageByCity(cityModel);

        // не выводим у чужих
        if (cityModel.Model.CityType != CityType.Friend)
        {
            IcomePanel.gameObject.SetActive(false);
            CanIncomeImg.gameObject.SetActive(false);
        }
    }

    private void SetImageByCity(City cityModel)
    {
        //Res1Img.gameObject.SetActive(false);
        //Res2Img.gameObject.SetActive(false);
        //Res3Img.gameObject.SetActive(false);
        //Res4Img.gameObject.SetActive(false);

        UpdateIcon();
        if (cityModel.Model.CityType != CityType.Friend) return; // не свои, не отображаем

        //switch (cityModel.Model.ResourceProduct.Type)
        //{
        //    case CityRecources.Res1: Res1Img.gameObject.SetActive(true); break;
        //    case CityRecources.Res2: Res2Img.gameObject.SetActive(true); break;
        //    case CityRecources.Res3: Res3Img.gameObject.SetActive(true); break;
        //    case CityRecources.Res4: Res4Img.gameObject.SetActive(true); break;
        //}
    }

    /// <summary>
    /// Обновление иконок у города.
    /// </summary>
    public void UpdateIcon()
    {
        IcomePanel.gameObject.SetActive(false);
        //BackRedImg.gameObject.SetActive(false);
        BackGreenImg.gameObject.SetActive(false);
        CanIncomeImg.gameObject.SetActive(false);
        if (_cityModel.Model.CityType != CityType.Friend) 
        {
            BackRedImg.gameObject.SetActive(false);
            return;    // не свои, не отображаем
        }
        BackRedImg.gameObject.SetActive(true);

        //- count
        if (_cityModel.Model.ResourceProduct.CurrentCount > 0)
        {
            IcomePanel.gameObject.SetActive(true);
            CountTxt.text = _cityModel.Model.ResourceProduct.CurrentCount.ToString();
        }

        
        //- icon
        //- фон
        if (_cityModel.Model.HasProcess())
        {
            BackGreenImg.gameObject.SetActive(true);
        }
        else
        {
            //BackRedImg.gameObject.SetActive(true);

            //- can income (если производство стоит)
            if (_mapLife.ShipHaveEnoughForCity(_cityModel))
            {
                CanIncomeImg.gameObject.SetActive(true);
            }
        }
    }


    public void UpdatePosition()
    {
        if (ObjectToFollow == null) return;

        Vector2 sp = RectTransformUtility.WorldToScreenPoint(Camera.main, ObjectToFollow.position);
        RectTrans.position = sp;
    }


	
	// Update is called once per frame
	void Update ()
	{
	    UpdatePosition();
	}
}
