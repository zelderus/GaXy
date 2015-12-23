using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Панель кнопок активная.
/// </summary>
public class PanelCityActionLogic : MonoBehaviour
{

    public Transform InfoBtn;
    public Transform FlyBtn;
    public Transform StartProductBtn;
    private Button _startButton;

    private RectTransform _rect;
    private City _city;
    private Boolean _isCurrent;

	// Use this for initialization
	void Start () {
        
	}


    public void Init()
    {
        _rect = this.GetComponent<RectTransform>();
        _startButton = StartProductBtn.GetComponent<Button>();

        //- 
        //var t = transform.Find("Title");
        //_titleTxt = t.gameObject.GetComponent<Text>();
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(City city, bool isCurrent)
    {
        if (city == null) return;
        _city = city;
        _isCurrent = isCurrent;
        SetCityAsCurrent(isCurrent);
        //_titleTxt.text = city.Title;

        //this.gameObject.transform.position = city.transform.position + new Vector3(_rect.rect.width, 0, 0);
        //this.gameObject.transform.Translate(_rect.rect.width, 0, 0);
        UpdateView();
       
        this.gameObject.SetActive(true);
        
    }

    /// <summary>
    /// Установка выбранного города как текущего. Позиция коробля.
    /// </summary>
    /// <param name="isCurrent"></param>
    public void SetCityAsCurrent(bool isCurrent)
    {
        _isCurrent = isCurrent;
        UpdateFlyBtn();
    }

    /// <summary>
    /// Кнопка полета.
    /// </summary>
    public void UpdateFlyBtn()
    {
        if (!_isCurrent && _city.CanFly)
        {
            FlyBtn.gameObject.SetActive(true);
        }
        else
        {
            FlyBtn.gameObject.SetActive(false);
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
        _city.CityMap.MainLogicObject.UpdateMathPanels();
        //mapLife.UpdateViewDay();
        //_city.CityMap.MainLogicObject.WorkPanelUpdate();
        ////x _actionPanel.UpdateView();
        //_city.CityMap.MainLogicObject.PanelCitySmall.UpdateView();
        //UpdateView();

    }

    private void UpdateStatStartProductBtn()
    {
        if (_city == null) return;
        var mapLife = _city.CityMap.MainLogicObject.GetMapLife();

        // кнопка полета (достаточно ли ходов)
        //if (_city.ca)

        //
        if (_isCurrent)
        {
            StartProductBtn.gameObject.SetActive(true);

            var canProduct = mapLife.ShipCanStartProductInCity(_city.Model);
            //var btn = StartProductBtn.GetComponent<Button>();
            if (canProduct)
            {
                _startButton.interactable = true;
            }
            else
            {
                _startButton.interactable = false;
            }
        }
        else
        {
            StartProductBtn.gameObject.SetActive(false);
        }
    }



    public void UpdateView()
    {
        UpdateStatStartProductBtn();
        UpdateFlyBtn();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
