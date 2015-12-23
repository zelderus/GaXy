using UnityEngine;
using System.Collections;



/// <summary>
/// Маленькая панель над городом на карте.
/// </summary>
public class PanelCitySmallLogic : MonoBehaviour
{
    public Transform ObjectToFollow = null;
    public RectTransform RectTrans;
    public PanelInfoSmallMissionPanel PanelMission;
    public PanelInfoSmallNeutralPanel PanelNeutral;

    public Animation Anim;

    private City _city;

	// Use this for initialization
	void Start () {
        
	}



    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(City city)
    {
        if (city == null) return;
        _city = city;
        //ShowContentCity(city);
        ObjectToFollow = _city.CityMap.transform;


        //+ panels
        PanelMission.Hide();
        PanelNeutral.Hide();

        if (_city.Model.CityType == CityType.Friend)
        {
            PanelMission.Show(city);
        }
        else if (_city.Model.CityType == CityType.Neutral)
        {
            PanelNeutral.Show(city);
        }


        UpdatePosition();
        this.gameObject.SetActive(true);

        Anim.Play();
    }



    /// <summary>
    /// Обновление вида.
    /// </summary>
    public void UpdateView()
    {
        PanelMission.UpdateView();
        PanelNeutral.UpdateView();
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
