using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework.Helpers;

public class LevelPanelHealthLogic : MonoBehaviour
{


    public Image HpImg;
    //public Material Mat;


    private RectTransform _rectTrans;
    private ShipLife _ship;

    private float _maxWidth = 196.0f;
    private float _hpMaxHealth = 100.0f;

	// Use this for initialization
	void Start () {
	
	}


    public void Init(ShipLife ship)
    {
        _ship = ship;
        _rectTrans = HpImg.GetComponent<RectTransform>();

        _hpMaxHealth = ship.MaxHealth;
        UpdateHealth();
    }


    /// <summary>
    /// Обновление шкалы жизней.
    /// </summary>
    public void UpdateHealth()
    {
        var shipPerc = MathHelpers.Percent(_ship.MaxHealth, _ship.Health);
        var barPerc = MathHelpers.ByPercent(_maxWidth, shipPerc);
        _rectTrans.sizeDelta = new Vector2(barPerc, _rectTrans.sizeDelta.y);

        //var matPerc = MathHelpers.ByPercent(1.0f, shipPerc);
        //Mat.mainTextureOffset = new Vector2(matPerc, Mat.mainTextureOffset.y);
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
