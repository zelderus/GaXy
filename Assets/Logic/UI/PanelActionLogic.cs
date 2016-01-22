using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework.Animations;

/// <summary>
/// Большое окно города.
/// </summary>
public class PanelActionLogic : MonoBehaviour
{
    public Transform FlyBtn;
    public PanelInfoMissionPanel PanelMission;
    public PanelInfoNeutralPanel PanelNeutral;

    public Boolean IsShowed { get; private set; }

    private Transform _closeBtn = null;
    //private Text _titleTxt = null;

    private City _city;
    private Boolean _isCurrent;


    private float _endRightPosX = 768.0f;
    private RectTransform _body;
    private EaseAnimations _animShow;

	// Use this for initialization
	void Start ()
	{
	    IsShowed = false;
	}

    public void Init()
    {
        _closeBtn = transform.Find("CloseBtn");

        //var t = transform.Find("Title");
        //_titleTxt = t.gameObject.GetComponent<Text>();

        PanelMission.Init();
        PanelNeutral.Init();

        _body = this.gameObject.GetComponent<RectTransform>();
        _endRightPosX = Screen.width;// 768.0f;
        _animShow = new EaseAnimations(EaseAnimationTypes.EaseOut, _endRightPosX, 0.0f, 0.3f);
    }


    public void Hide()
    {
        IsShowed = false;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Открытие панели с городом.
    /// </summary>
    /// <param name="city"></param>
    /// <param name="isCurrent"></param>
    public void Show(City city, bool isCurrent)
    {
        if (city == null) return;
        IsShowed = true;
        _city = city;
        SetCityAsCurrent(isCurrent);

        //_titleTxt.text = city.Title;

        UpdateView();


        this.gameObject.SetActive(true);

        // anim
        _body.position = new Vector3(_endRightPosX, _body.position.y, _body.position.z);
        _animShow.Start();
    }


    /// <summary>
    /// Установка выбранного города как текущего. Позиция корабля.
    /// </summary>
    /// <param name="isCurrent"></param>
    private void SetCityAsCurrent(bool isCurrent)
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



    public void UpdateView()
    {
        if (_city == null) return;
        var city = _city;
        var isCurrent = _isCurrent;


        PanelMission.Hide();
        PanelNeutral.Hide();

        if (city.Model.CityType == CityType.Friend)
        {
            PanelMission.Show(this, city, isCurrent);
        }
        else if (city.Model.CityType == CityType.Neutral)
        {
            PanelNeutral.Show(this, city, isCurrent);
        }
    }


    private void UpdateEaseAnimations()
    {
        _animShow.Update(Time.deltaTime);
        if (_animShow.IsStarted())
        {
            var x = _animShow.Value;
            _body.position = new Vector3(x, _body.position.y, _body.position.z);
        }
    }
    

    


	// Update is called once per frame
	void Update () 
    {
        UpdateEaseAnimations();


	}
}
