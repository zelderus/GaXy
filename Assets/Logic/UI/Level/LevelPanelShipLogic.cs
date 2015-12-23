using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework.Helpers;

public class LevelPanelShipLogic : MonoBehaviour {


    public Transform ObjectToFollow;
    public RectTransform RectTrans;

    public Text HealthTxt;
    public Button HealthBtn;
    public Image HealthBar;
    public Button ShieldBtn;
    public Image ShieldBar;
    public Button TreeBtn;
    public Image TreeBar;

    public Color BarLineColor;
    public Color BarLineFullColor;

    public Image HpCount1;
    public Image HpCount2;
    public Image HpCount3;
    public Image HpCount4;

    public Image ShieldCount1;
    public Image ShieldCount2;
    public Image ShieldCount3;
    public Image ShieldCount4;

    public Image TreeCount1;
    public Image TreeCount2;
    public Image TreeCount3;
    public Image TreeCount4;

    private bool _isShowed = false;

    private float _hpTime = 0.0f;
    private bool _hpRespaned = true;
    private bool _hpCanNext = true;
    private float _shieldTime = 0.0f;
    private bool _shieldRespaned = true;
    private bool _shieldCanNext = true;
    private float _treeTime = 0.0f;
    private bool _treeRespaned = true;
    private bool _treeCanNext = true;

    private Vector2 _realSize = new Vector2(0, 100);
    
    private ShipLife _ship;
    private readonly Vector3 _shipOffset = new Vector3(0, 2.0f, 0); // сдвиг над моделью корабля

	// Use this for initialization
	void Start ()
	{
        _realSize = new Vector2(RectTrans.sizeDelta.x * DisplayHelper.ScreenScale, RectTrans.sizeDelta.y * DisplayHelper.ScreenScale);
	}


    public void Init(ShipLife ship)
    {
        _ship = ship;

        UpdateHealth();

        UpdateHealthBtn();
        UpdateShieldBtn();
        UpdateTreeBtn();

        UpdateHealthCounter();
        UpdateShieldCounter();
        UpdateTreeCounter();


        _hpRespaned = true;
        _hpTime = _ship.ShipBonusHealthTime;
        UpdateHpBar();

        _shieldRespaned = true;
        _shieldTime = _ship.ShipBonusShieldTime;
        UpdateShieldBar();

        _treeRespaned = true;
        _treeTime = _ship.ShipBonusTreeTime;
        UpdateTreeBar();
    }
    public void Hide()
    {
        _isShowed = false;
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        _isShowed = true;
        UpdatePosition();
        UpdateHpBar();
        UpdateShieldBar();
        UpdateTreeBar();
        this.gameObject.SetActive(true);
    }


    #region hp
    /// <summary>
    /// Процент здоровья.
    /// </summary>
    /// <returns></returns>
    private float GetShipHealthPerc()
    {
        return MathHelpers.Percent(_ship.MaxHealth, _ship.Health);
    }

    public void UpdateHealth()
    {
        var shipPerc = GetShipHealthPerc();
        var barPerc = MathHelpers.ByPercent(100.0f, shipPerc);
        HealthTxt.text = String.Format("{0}%", barPerc.ToString("F0"));

        UpdateHealthBtn();
    }

    public bool CanAddHealth()
    {
        return _hpRespaned && _ship.ShipBonusHealthCount > 0 && GetShipHealthPerc() < 100.0f;
    }
    public void UpdateHealthBtn()
    {
        if (CanAddHealth())
        {
            HealthBtn.interactable = true;
        }
        else
        {
            HealthBtn.interactable = false;
        }
    }

    public void HealthUse()
    {
        _hpRespaned = false;
        UpdateHealthCounter();
        UpdateHpBar();
    }

    private void UpdateHealthCounter()
    {
        HpCount1.gameObject.SetActive(_ship.ShipBonusHealthCount >= 1);
        HpCount2.gameObject.SetActive(_ship.ShipBonusHealthCount >= 2);
        HpCount3.gameObject.SetActive(_ship.ShipBonusHealthCount >= 3);
        HpCount4.gameObject.SetActive(_ship.ShipBonusHealthCount >= 4);
    }

    private void UpdateHpBar()
    {
        if (!_isShowed) return;
        if (_ship.ShipBonusHealthCount <= 0)
        {
            HealthBar.gameObject.SetActive(false);
            //HealthBtn.gameObject.SetActive(false);
            return;
        }

        var s = _hpRespaned ? 100.0f : MathHelpers.Percent(_ship.ShipBonusHealthTime, _ship.ShipBonusHealthTime - _hpTime);
        var barPerc = MathHelpers.ByPercent(1.0f, s);
        HealthBar.fillAmount = barPerc;
        if (s >= 100.0f)
        {
            HealthBar.color = BarLineFullColor;
            UpdateHealthBtn();    //- включаем кнопку
        }
        else
        {
            HealthBar.color = BarLineColor;
        }
    }
    #endregion
    #region shield
    public bool CanAddShield()
    {
        return _shieldRespaned && _ship.ShipBonusShieldCount > 0;
    }
    public void UpdateShieldBtn()
    {
        if (CanAddShield())
        {
            ShieldBtn.interactable = true;
        }
        else
        {
            ShieldBtn.interactable = false;
        }
    }
    public void ShieldUse()
    {
        _shieldRespaned = false;
        UpdateShieldCounter();
        UpdateShieldBar();
    }
    private void UpdateShieldCounter()
    {
        ShieldCount1.gameObject.SetActive(_ship.ShipBonusShieldCount >= 1);
        ShieldCount2.gameObject.SetActive(_ship.ShipBonusShieldCount >= 2);
        ShieldCount3.gameObject.SetActive(_ship.ShipBonusShieldCount >= 3);
        ShieldCount4.gameObject.SetActive(_ship.ShipBonusShieldCount >= 4);
    }

    private void UpdateShieldBar()
    {
        if (!_isShowed) return;
        if (_ship.ShipBonusShieldCount <= 0)
        {
            ShieldBar.gameObject.SetActive(false);
            //ShieldBtn.gameObject.SetActive(false);
            return;
        }

        var s = _shieldRespaned ? 100.0f : MathHelpers.Percent(_ship.ShipBonusShieldTime, _ship.ShipBonusShieldTime-_shieldTime);
        var barPerc = MathHelpers.ByPercent(1.0f, s);
        ShieldBar.fillAmount = barPerc;
        if (s >= 100.0f)
        {
            ShieldBar.color = BarLineFullColor;
            UpdateShieldBtn();    //- включаем кнопку
        }
        else
        {
            ShieldBar.color = BarLineColor;
        }
    }
    #endregion
    #region treeweapon
    public bool CanAddTree()
    {
        return _treeRespaned && _ship.ShipBonusTreeCount > 0;
    }
    public void UpdateTreeBtn()
    {
        if (CanAddTree())
        {
            TreeBtn.interactable = true;
        }
        else
        {
            TreeBtn.interactable = false;
        }
    }

    public void TreeUse()
    {
        _treeRespaned = false;
        UpdateTreeCounter();
        UpdateTreeBar();
    }
    private void UpdateTreeCounter()
    {
        TreeCount1.gameObject.SetActive(_ship.ShipBonusTreeCount >= 1);
        TreeCount2.gameObject.SetActive(_ship.ShipBonusTreeCount >= 2);
        TreeCount3.gameObject.SetActive(_ship.ShipBonusTreeCount >= 3);
        TreeCount4.gameObject.SetActive(_ship.ShipBonusTreeCount >= 4);
    }
    private void UpdateTreeBar()
    {
        if (!_isShowed) return;
        if (_ship.ShipBonusTreeCount <= 0)
        {
            TreeBar.gameObject.SetActive(false);
            //TreeBtn.gameObject.SetActive(false);
            return;
        }

        var s = _treeRespaned ? 100.0f : MathHelpers.Percent(_ship.ShipBonusTreeTime, _ship.ShipBonusTreeTime - _treeTime);
        var barPerc = MathHelpers.ByPercent(1.0f, s);
        TreeBar.fillAmount = barPerc;
        if (s >= 100.0f)
        {
            TreeBar.color = BarLineFullColor;
            UpdateTreeBtn();    //- включаем кнопку
        }
        else
        {
            TreeBar.color = BarLineColor;
        }
    }
    #endregion




    public void UpdatePosition()
    {
        if (ObjectToFollow == null) return;

        //Vector2 sp = RectTransformUtility.WorldToScreenPoint(Camera.main, ObjectToFollow.position + _shipOffset);
        Vector2 sp = Camera.main.WorldToScreenPoint(ObjectToFollow.position + _shipOffset);

        if (sp.x - (_realSize.x / 2) < 0) sp.x = (_realSize.x / 2);
        if (sp.x + (_realSize.x / 2) > Screen.width) sp.x = Screen.width-(_realSize.x / 2);

        RectTrans.position = sp;
    }


    /// <summary>
    /// Отдельное обновление таймеров.
    /// <remarks>Гарантируем, если даже выключена панель и не работает Update.</remarks>
    /// </summary>
    public void UpdateTimers()
    {
        //
        if (!_hpRespaned)
        {
            _hpTime -= Time.deltaTime;
            if (_hpTime <= 0.0f)
            {
                _hpTime = _ship.ShipBonusHealthTime;
                _hpRespaned = true;
            }
            UpdateHpBar();
        }
        //
        if (!_shieldRespaned)
        {
            _shieldTime -= Time.deltaTime;
            if (_shieldTime <= 0.0f)
            {
                _shieldTime = _ship.ShipBonusShieldTime;
                _shieldRespaned = true;
            }
            UpdateShieldBar();
        }
        //
        if (!_treeRespaned)
        {
            _treeTime -= Time.deltaTime;
            if (_treeTime <= 0.0f)
            {
                _treeTime = _ship.ShipBonusTreeTime;
                _treeRespaned = true;
            }
            UpdateTreeBar();
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
	    UpdatePosition();

	}


}
