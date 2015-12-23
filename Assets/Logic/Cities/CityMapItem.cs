using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;



/// <summary>
/// 3D объект города на карте.
/// </summary>
public class CityMapItem : MonoBehaviour//, IPointerClickHandler
{

    public MapController MainLogicObject;
    public City CityModel;
    public Transform PlanetModel;


    public Material Planet1Mat;
    public Material Planet2Mat;
    public Material Planet3Mat;
    public Material Planet4Mat;


    private Transform _selectObj;
    

    //public Transform City1Model;
    //public Transform City2Model;
    //public Transform City3Model;
    //public Transform City4Model;

    private float _speedRot = 3.0f;
    private Int32 _rotDir = 1;
    private float _sizeScale = 1.0f;

    void Awake()
    {
        _selectObj = this.transform.Find("SelectObj");
        //_planetModel = this.transform.Find("cityProtModel");
        
    }
	// Use this for initialization
	void Start ()
	{
        //_selectObj = this.transform.Find("SelectObj");
	}


    public void Init(MapController mainLogicObject, City cityModel)
    {
        MainLogicObject = mainLogicObject;
        CityModel = cityModel;
        CityModel.SetCityMap(this);
        //- 

        _speedRot = cityModel.Model.SpeedRot;
        _rotDir = cityModel.Model.RotDir;
        _sizeScale = cityModel.Model.SizeScale;
    }


    public void Select()
    {
        //x _img.color = new Color(255.0f, 0.0f, 0.0f, 255.0f);
        //_selectObj.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        //x _img.color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        //_selectObj.gameObject.SetActive(false);
    }


    /// <summary>
    /// Вид города.
    /// </summary>
    public void SetCityView(CityRecources res, Int32 rating)
    {
        //City1Model.gameObject.SetActive(false);
        //City2Model.gameObject.SetActive(false);
        //City3Model.gameObject.SetActive(false);
        //City4Model.gameObject.SetActive(false);

        //switch (res)
        //{
        //    case CityRecources.Res1: City1Model.gameObject.SetActive(true); break;
        //    case CityRecources.Res2: City2Model.gameObject.SetActive(true); break;
        //    case CityRecources.Res3: City3Model.gameObject.SetActive(true); break;
        //    case CityRecources.Res4: City4Model.gameObject.SetActive(true); break;
        //}

        Material mat = Planet1Mat;
        switch (res)
        {
            case CityRecources.Res1: mat = Planet1Mat; break;
            case CityRecources.Res2: mat = Planet2Mat; break;
            case CityRecources.Res3: mat = Planet3Mat; break;
            case CityRecources.Res4: mat = Planet4Mat; break;
        }
        PlanetModel.GetComponent<Renderer>().material = mat;

    }
    

	// Update is called once per frame
	void Update () {

        this.transform.Rotate(0, Time.deltaTime * _speedRot * _rotDir, 0);
	}

    public void OnPointerClick()//PointerEventData eventData)
    {
        if (MainLogicObject != null)
            MainLogicObject.OnSelectCity(CityModel, this);
    }
}
