using System;
using UnityEngine;
using System.Collections;


/// <summary>
/// 3D объект города на карте.
/// </summary>
public class CityMapItem : MonoBehaviour//, IPointerClickHandler
{

    public MapController MainLogicObject;
    public City CityModel;
    public Transform PlanetModel;
    public ParticleSystem Particles;

    public Material Planet1Mat;
    public Material Planet2Mat;
    public Material Planet3Mat;
    public Material Planet4Mat;
    public Material PlanetBlackMat;


    //private Transform _selectObj;
    

    //public Transform City1Model;
    //public Transform City2Model;
    //public Transform City3Model;
    //public Transform City4Model;

    private float _speedRot = 3.0f;
    private Int32 _rotDir = 1;
    private float _sizeScale = 1.0f;

    void Awake()
    {
        //_selectObj = this.transform.Find("SelectObj");
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
        if (res == CityRecources.Black) return;

        Material mat = Planet1Mat;
        switch (res)
        {
            case CityRecources.Res1: mat = Planet1Mat; break;
            case CityRecources.Res2: mat = Planet2Mat; break;
            case CityRecources.Res3: mat = Planet3Mat; break;
            case CityRecources.Res4: mat = Planet4Mat; break;
            case CityRecources.Black: mat = PlanetBlackMat; break;
        }
        PlanetModel.GetComponent<Renderer>().material = mat;
    }


    #region JOP
    private bool _jopFloatOn = false;
    /// <summary>
    /// Выполнены все задачи JOP.
    /// </summary>
    public void JopFullCompleted()
    {
        if (!CityModel.Model.IsJopAndCompleted()) return;
        _jopFloatOn = true;

        Particles.gameObject.SetActive(true);
        //this.GetComponent<BoxCollider>().gameObject.SetActive(false);
        //+ fly anim
        PlanetModel.GetComponent<Animator>().enabled = true;
        //PlanetModel.GetComponent<Animator>().applyRootMotion = true;
        PlanetModel.GetComponent<Animator>().applyRootMotion = false;
        PlanetModel.GetComponent<Animator>().Play(0);

        Destroy(this.gameObject, 8.0f);
    }

    //private void JopUpdate()
    //{
    //    if (!_jopFloatOn) return;
    //    // улетаем как JOP

    //}
    #endregion



    // Update is called once per frame
	void Update () 
    {
        if (!CityModel.Model.IsJop) this.transform.Rotate(0, Time.deltaTime * _speedRot * _rotDir, 0);
	    //JopUpdate();
	}

    public void OnPointerClick()//PointerEventData eventData)
    {
        if (MainLogicObject != null)
            MainLogicObject.OnSelectCity(CityModel, this);
    }
}
