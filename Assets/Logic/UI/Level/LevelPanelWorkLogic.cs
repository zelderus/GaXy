using UnityEngine;
using System.Collections;

public class LevelPanelWorkLogic : MonoBehaviour
{


    public LevelPanelHealthLogic HealthPanel;


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


    // Update is called once per frame
	void Update () {
	
	}
}
