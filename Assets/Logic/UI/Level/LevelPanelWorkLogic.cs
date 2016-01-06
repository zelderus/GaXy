using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelPanelWorkLogic : MonoBehaviour
{


    public LevelPanelHealthLogic HealthPanel;
    public PanelWorkResourceBlockLogic MatBlock;
    //public Text TotalMat;

	// Use this for initialization
	void Start () {
	
	}



    public void Init(ShipLife ship)
    {
        HealthPanel.Init(ship);
    }


    /// <summary>
    /// Обновление шкалы жизней.
    /// </summary>
    public void UpdateHealth()
    {
        HealthPanel.UpdateHealth();
    }

    /// <summary>
    /// Обновление количества материала.
    /// </summary>
    public void UpdateMaterial()
    {
        //TotalMat.text = FarLife.MapLife.GetSelfResource(CityRecources.Material).CurrentCount.ToString();
        MatBlock.SetCount(FarLife.MapLife.GetSelfResource(CityRecources.Material).CurrentCount);
    }


    // Update is called once per frame
	void Update () {
	
	}
}
