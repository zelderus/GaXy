using System;
using UnityEngine;
using System.Collections;
using ZelderFramework.Animations;
using UnityEngine.UI;

public class LevelPanelMarketLogic : MonoBehaviour {


    public Boolean IsShowed { get; private set; }

    public Image BackImg;
    public RectTransform PanelBody;
    public LevelPanelShipLogic ShipPanel;
    public LevelPanelWorkLogic WorkPanel;
    //public Text TotalMat;

    public Text HpCount;
    public Text ShieldCount;
    public Text TreeCount;

    public LevelMarketBtnLogic HpBtn;
    public LevelMarketBtnLogic ShieldBtn;
    public LevelMarketBtnLogic TreeBtn;

    //private RectTransform _body;
    private EaseAnimations _animHide;
    private ShipLife _ship;

	// Use this for initialization
	void Start () 
    {

        //_body = this.gameObject.GetComponent<RectTransform>();

        
        _animHide = new EaseAnimations(EaseAnimationTypes.EaseNo, PanelBody.position.x, -PanelBody.rect.width, 0.15f);
        _animHide.SetOnEndFunction(OnHide);
	}


    public void Init(ShipLife ship)
    {
        _ship = ship;

        HpBtn.Init(_ship.ShipBonusHealthCost);
        ShieldBtn.Init(_ship.ShipBonusShieldCost);
        TreeBtn.Init(_ship.ShipBonusTreeCost);

        UpdateView();
    }



    public void OnBuyHp()
    {
        if (!HpBtn.IsEnough()) return;

        _ship.AddBonusHealth();
        FarLife.MapLife.AddMaterial(-HpBtn.Cost);

        UpdateView();
        ShipPanel.UpdateCounters();
    }
    public void OnBuyShield()
    {
        if (!ShieldBtn.IsEnough()) return;

        _ship.AddBonusShield();
        FarLife.MapLife.AddMaterial(-ShieldBtn.Cost);

        UpdateView();
        ShipPanel.UpdateCounters();
    }
    public void OnBuyTree()
    {
        if (!TreeBtn.IsEnough()) return;

        _ship.AddBonusTree();
        FarLife.MapLife.AddMaterial(-TreeBtn.Cost);

        UpdateView();
        ShipPanel.UpdateCounters();
    }
    


    public void Hide()
    {
        //this.gameObject.SetActive(false);
        BackImg.gameObject.SetActive(false);
        IsShowed = false;
        _animHide.Start();
    }
    private void OnHide()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
        IsShowed = true;
        BackImg.gameObject.SetActive(true);
    }



    private void UpdateView()
    {
        //TotalMat.text = FarLife.MapLife.GetSelfResource(CityRecources.Material).CurrentCount.ToString();
        WorkPanel.UpdateMaterial();

        HpCount.text = _ship.ShipBonusHealthCount.ToString();
        ShieldCount.text = _ship.ShipBonusShieldCount.ToString();
        TreeCount.text = _ship.ShipBonusTreeCount.ToString();

        HpBtn.UpdateView();
        ShieldBtn.UpdateView();
        TreeBtn.UpdateView();
    }



	
	// Update is called once per frame
	void Update () 
    {
        _animHide.Update(Time.deltaTime);
        if (_animHide.IsStarted())
        {
            PanelBody.position = new Vector3(_animHide.Value, PanelBody.position.y, PanelBody.position.z);
        }
	}
}
