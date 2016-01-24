using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using ZelderFramework.Helpers;

public class MapController : MonoBehaviour
{
    public Canvas MainCanvas;
    public Camera MainCamera;

    public Transform BigBack;
    public Color BigBackJopedColor = Color.white;

    public Transform PanelForWorld;
    public PanelActionLogic PanelAction;
    public PanelCityActionLogic PanelCityAction;
    public PanelWorkLogic PanelWork;
    public PanelCitySmallLogic PanelCitySmall;
    public PanelOptionLogic PanelOption;
    public PanelHelpLogic PanelHelp;
    //public PanelShipLogic PanelShip;
    public GameObject BackPanel;

    public WorldMap WorldMap;
    //public LogScript Log;


    public Text DayNumTxt;


    //private List<CityListItem> _cities;
    private City _currenctSelectCity = null;
    private CityMapItem _currenctCityMap = null;

    
    private MapLife _mapLife;
    private ShipLife _shipLife;

    private GameObject _shipPrefab;
    private GameObject _parentCityMap;
    //private GameObject _gexPrefab;
    private GameObject _gexActivePrefab;
    private GameObject _parentGexMap;
    private GameObject _cometPrefab;

    private Boolean _mapInTouchControl = true;
    public Boolean IsMapInTouch { get { return _mapInTouchControl; }}


    public Material GexNormalMat;
    public Material GexActiveMat;
    public Material GexCurrentMat;
    public Material GexBadMat;

    void Awake()
    {
        FarLife.Init();
        FarLife.ScreenInit(BackPressed);
    }

    // Use this for initialization
    private void Start()
    {
        // загрузка данных игрока
        _mapLife = FarLife.MapLife;
        _shipLife = FarLife.ShipLife;

        //+ сохраняем
        FarLife.SaveGame();


        //
        _parentGexMap = GameObject.Find("CityMapGexParent") as GameObject;
        //_gexPrefab = Resources.Load("Prefabs/Map/GexPref", typeof(GameObject)) as GameObject;
        //x _gexActivePrefab = Resources.Load("Prefabs/Map/GexActivePref", typeof(GameObject)) as GameObject;
        _gexActivePrefab = Resources.Load("Prefabs/Map/GexAcPref", typeof(GameObject)) as GameObject;
        _cometPrefab = Resources.Load("Prefabs/Map/CometPref", typeof(GameObject)) as GameObject;
        _parentCityMap = GameObject.Find("CityMapParent") as GameObject;


        InitGexItems();
        InitSelectableItems();


        PanelAction.Init();
        HideActionPanel();

        PanelWork.Init();
        PanelCityAction.Init();

        PanelOption.Init();

        PanelHelp.Init();

        _currenctSelectCity = _mapLife.CurrenctCity;
        InitShip();

        
        //+ с учетом, прилетели ли с уровня
        if (_mapLife.LevelStatus == FarStatusLevel.Init)
        {
            UpdateDayPanels();
        }
        else
        {
            var levelIsWin = _mapLife.LevelStatus == FarStatusLevel.LevelWin;
            LevelEnd(levelIsWin);
            SelectCity(_currenctSelectCity, _currenctSelectCity.CityMap);
        }
        //- собираем ресурс
        _mapLife.CurrentResourceCollect();
        //- подсветка города, в котором мы
        ShowCurrentGex();
        //- центрируем карту
        CenterMapOnCity(_currenctSelectCity);

        //
        UpdateDayNum();


        //+ панель приветствия
        if (FarLife.GlobalData.IsFirstRunMap)
        {
            FarLife.GlobalData.IsFirstRunMap = false;
            ShowHelpPanel(1);
            FarLife.SaveGlobalOnly();
        }


        GestHelpers.ClearMove();
        //- сообщаем движку что готовы к сцене
        FarLife.OnScreenLoaded();
    }
    /// <summary>
    /// Suspending.
    /// </summary>
    /// <param name="pausing"></param>
    public void OnApplicationPause(bool pausing)
    {
        FarLife.GameLife.OnApplicationPause(pausing);

    }


    public MapLife GetMapLife()
    {
        return _mapLife;
    }
    public ShipLife GetShipLife()
    {
        return _shipLife;
    }


    
    #region gex


    //private GameObject[,] _gexAll;  // все поле гексов
    private void InitGexItems()
    {
        //_gexAll = new GameObject[_mapLife.GexMaxX, _mapLife.GexMaxY];
        ////?+ все поле
        //for (Int32 y = 0; y < _mapLife.GexMaxY; y++)
        //{
        //    for (Int32 x = 0; x < _mapLife.GexMaxX; x++)
        //    {
        //        var gex = _mapLife.Gex[x, y];
        //        //
        //        var gexObject = PlaceGex(_gexPrefab, gex.Position, 1.0f);
        //        _gexAll[x, y] = gexObject;
        //    }
        //}
        
    }

    private GameObject PlaceGex(GameObject gexPref, Vector2 pos, float z = 0.0f)
    {
        GameObject pref = Instantiate(gexPref, _parentGexMap.transform.position, Quaternion.identity) as GameObject;
        pref.transform.SetParent(_parentGexMap.transform);
        pref.transform.localPosition = new Vector3(pos.x, pos.y, z);

        return pref;
    }

    /// <summary>
    /// Маршрут между городами.
    /// </summary>
    /// <param name="cityFrom"></param>
    /// <param name="cityTo"></param>
    private void ShowGexRoute(City cityFrom, City cityTo)
    {
        HideGexRoute();

        var xS = cityFrom.MapGex.GexX;
        var yS = cityFrom.MapGex.GexY;
        var xF = cityTo.MapGex.GexX;
        var yF = cityTo.MapGex.GexY;

        // поиск пути
        var routes = _mapLife.SearchRouteWorkGex(xS, yS, xF, yF);
        Int32 step = 0;
        foreach (var route in routes)
        {
            step++;
            var gex = _mapLife.Gex[route.X, route.Y];
            var gexObject = PlaceGex(_gexActivePrefab, gex.Position, 0.92f);
            //var gexObject = _gexAll[route.X, route.Y];
            if (step <= _shipLife.MaxSteps) gexObject.GetComponent<Renderer>().sharedMaterial = GexActiveMat;
            else gexObject.GetComponent<Renderer>().sharedMaterial = GexBadMat;

            _gexRoutes.Add(gexObject);
        }

        // хватает ходов
        var canFly = step <= _shipLife.MaxSteps;

        // говорим городу
        cityTo.CanFly = canFly;

        // подсветка последнего гекса (выбранный город)
        if (canFly && _gexRoutes.Any())
        {
            var lst = _gexRoutes.LastOrDefault();
            lst.GetComponent<Renderer>().sharedMaterial = GexCurrentMat;
        }

    }
    private readonly List<GameObject> _gexRoutes = new List<GameObject>();
    private void HideGexRoute()
    {
        foreach (var gexObject in _gexRoutes)
        {
            Destroy(gexObject);
            //gexObject.GetComponent<Renderer>().sharedMaterial = GexNormalMat;
        }
        _gexRoutes.Clear();
    }


    private void ShowCurrentGex()
    {
        if (_mapLife.CurrenctCity == null) return;

        var gexObject = PlaceGex(_gexActivePrefab, _mapLife.CurrenctCity.Position, 0.92f);
        gexObject.GetComponent<Renderer>().sharedMaterial = GexCurrentMat;
    }

    #endregion

    #region cities

    private void InitSelectableItems()
    {
        //_cities = new List<CityListItem>();
        //x _cities.AddRange(FindObjectsOfType<SelectItem>());

        //- list
        //var parent = GameObject.Find("ContentCityListUI") as GameObject;
        //var parentTrans = parent.GetComponent<RectTransform>();

        //- city res img
        var cityResImgPrefab = Resources.Load("Prefabs/Map/ResForCityPref", typeof(GameObject)) as GameObject;

        //- city list prefab
        //var prefab = Resources.Load("Prefabs/CityListItemPref", typeof (GameObject)) as GameObject;
        //- city model prefab
        //_parentCityMap = GameObject.Find("CityMapParent") as GameObject;
        var cityPref = Resources.Load("Prefabs/Map/CityPref", typeof(GameObject)) as GameObject;
        var cityJopPref = Resources.Load("Prefabs/Map/CityJopPref", typeof(GameObject)) as GameObject;

        //! cities
        foreach (var cityModel in _mapLife.Cities)
        {
            if (cityModel.Model.IsJopAndCompleted())
            {
                BigBackSetAsJoped();
                continue;
            }
            AddCityToMap(_parentCityMap, cityModel.Model.IsJop ? cityJopPref : cityPref, cityResImgPrefab, cityModel);
        }

    }

    


    private float _lastPosY = 0.0f;

    
    private void AddCityToMap(GameObject parentCityMap, GameObject cityPref, GameObject cityResPref, City cityModel)
    {
        //GameObject pref = Instantiate(cityPref, new Vector3(cityModel.Position.x, cityModel.Position.y, 0.0f), Quaternion.identity) as GameObject;
        GameObject pref = Instantiate(cityPref, parentCityMap.transform.position, new Quaternion(UnityEngine.Random.Range(0, 45), 0, 0, 0)) as GameObject;
        pref.transform.SetParent(parentCityMap.transform);
        pref.transform.localPosition = new Vector3(cityModel.Position.x, cityModel.Position.y, 0.0f);
        pref.transform.localScale = Vector3.one * cityModel.Model.SizeScale;
        pref.transform.localRotation = cityModel.Model.IsJop ? Quaternion.identity : Quaternion.AngleAxis(UnityEngine.Random.Range(0, 255), Vector3.right);

        //- img
        //GameObject resImgPref = Instantiate(cityResPref, MainCanvas.transform.position, Quaternion.identity) as GameObject;
        GameObject resImgPref = Instantiate(cityResPref, PanelForWorld.transform.position, Quaternion.identity) as GameObject;
        resImgPref.transform.SetParent(PanelForWorld.transform);
        resImgPref.transform.localScale = Vector3.one;
        ResForCityImgLogic cityImgLogic = resImgPref.GetComponent<ResForCityImgLogic>();
        cityImgLogic.SetCity(pref.transform, cityModel, _mapLife);
        cityModel.IconLogic = cityImgLogic;
        
        //- logic
        CityMapItem city = pref.GetComponent<CityMapItem>();
        city.Init(this, cityModel);
        //x cityList.SetCityMap(city);
    }

    public void DeselectCities()
    {
        //if (_currenctCity != null) _currenctCity.Deselect();
        _currenctSelectCity = null;

        if (_currenctCityMap != null) _currenctCityMap.Deselect();
        _currenctCityMap = null;

        HideGexRoute();
        PanelCitySmall.Hide();
        //foreach(var city in _cities)
        //{
        //    city.Deselect(); 
        //}
        ViewCityInfo(null);
    }

    private void SelectCity(City city, CityMapItem cityMap)
    {
        DeselectCities();
        _currenctSelectCity = city;

        _currenctCityMap = cityMap;
        if (_currenctCityMap != null)
        {
            ShowGexRoute(_mapLife.CurrenctCity, _currenctCityMap.CityModel);    // вывод маршрута
            _currenctCityMap.Select();
        }
        
        PanelCitySmall.Show(_currenctSelectCity);
        

       // _currenctCity.Select();
        ViewCityInfo(_currenctSelectCity);
    }
    /// <summary>
    /// Выбран город.
    /// </summary>
    public void OnSelectCity(City city, CityMapItem cityMap)
    {
        SelectCity(city, cityMap);
    }

    private void CenterMapOnCity(City city)
    {
        WorldMap.SetPosition(-city.Position);
    }

    /// <summary>
    /// Город является текущим. Корабль в нем.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    public Boolean IsCityCurrent(City city)
    {
        return _mapLife.CurrenctCity == city;
    }

    /// <summary>
    /// Вывод инфо города.
    /// </summary>
    /// <param name="city"></param>
    private void ViewCityInfo(City city)
    {
        if (city == null)
        {
            HideCityPanel();
            HideActionPanel();
            //HideCityInfoPanel();
        } //- 
        else
        {
            ShowActionPanel();
            //ShowCityPanel();
            //ShowCityInfoPanel();
        }
    }

    /// <summary>
    /// Закончены задачи JOP и объект улетел. Отцепляем от карты и прочее.
    /// </summary>
    public void JopCompletedOn(CityMapItem city)
    {
        BigBackSetAsJoped();
        FarLife.JopIsComplete();
        DeselectCities();
        _mapLife.FreeMapGex(city.CityModel.MapGex);
    }

    private void BigBackSetAsJoped()
    {
        BigBack.GetComponent<Renderer>().material.color = BigBackJopedColor;
    }
    #endregion
    #region ship
    private void InitShip()
    {
        //- ships
        var shipPrefab = Resources.Load("Prefabs/Map/ShipPref", typeof(GameObject)) as GameObject;

        //! ships
        AddShipToMap(shipPrefab, _mapLife.CurrenctCity);
        //AddShipToMap(parentCityMap, shipPrefab, new Vector2(6, 9));
    }

    private void AddShipToMap(GameObject shipPref, City city)
    {
        AddShipToMap(shipPref, city.Position);
    }

    private void AddShipToMap(GameObject shipPref, Vector2 pos)
    {
        _shipPrefab = Instantiate(shipPref);//, parentCityMap.transform.position, Quaternion.identity) as GameObject;
        _shipPrefab.transform.SetParent(_parentCityMap.transform);
        ShipSetPosition(pos);
    }

    private void ShipSetPosition(Vector2 pos)
    {
        _shipPrefab.transform.localPosition = new Vector3(pos.x, pos.y, -2.0f);
    }

    #endregion
    #region comets

    private float _timeToComet = 4.0f;
    private float _lastTimeComet = 0.0f;
    private void CommetAddLogic()
    {
        _lastTimeComet += Time.deltaTime;

        if (_lastTimeComet >= _timeToComet)
        {
            _lastTimeComet = 0.0f;
            AddComet();
            DeleteComets();
        }
    }

    private void DeleteComets()
    {
        foreach (var comet in _comets)
        {
            if (comet == null) continue;
            if (comet.Died)
            {
                Destroy(comet.gameObject);
            }
        }
    }
    private List<CometLogic> _comets = new List<CometLogic>();
    private void AddComet()
    {
        var d = CometGetPos();
        var pos = d.Position;
        var dir = d.Direction;
        
        var scale = 0.08f;
        var speed = UnityEngine.Random.Range(14.0f, 18.0f);
        var z = 6.0f;
        
        GameObject pref = Instantiate(_cometPrefab, _parentCityMap.transform.position, Quaternion.identity) as GameObject;
        pref.transform.SetParent(_parentCityMap.transform);
        pref.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);
        pref.transform.localScale = Vector3.one*scale;
        
        //- logic
        CometLogic comet = pref.GetComponent<CometLogic>();
        comet.Speed = speed;
        comet.Dir = dir;
        comet.PosZ = z;
        comet.Go();

        _comets.Add(comet);  //
    }
    private readonly BehemotPos[] _cometPoses = new BehemotPos[]
    {
        new BehemotPos(-6, -6,  1, 1),
        new BehemotPos(-6, 30,  1, -0.4f),
        new BehemotPos(-6, 15,  1, 0.0001f),
        new BehemotPos(30, -6,  -1, 1),
        new BehemotPos(30, 30,  -1, -0.4f),
        new BehemotPos(30, 15,  -1, -0.0001f),
        new BehemotPos(12, -6,  0.00001f, 1),
        new BehemotPos(12, 30,  -0.00001f, -1),
    };
    private BehemotPos CometGetPos()
    {
        var data = _cometPoses[UnityEngine.Random.Range(0, 7)];
        return data;
    }
    #endregion

    #region panel action
    private void ShowBackPanel()
    {
        BackPanel.SetActive(true);
    }
    private void HideBackPanel()
    {
        BackPanel.SetActive(false);
    }


    public void ShowOptionPanel()
    {
        if (!_mapInTouchControl) return;
        DeselectCities();

        _mapInTouchControl = false;
        _mapDoInTouch = false;
                
        PanelOption.Show();
        ShowBackPanel();
    }
    public void HideOptionPanel()
    {
        _mapDoInTouch = true;
        PanelOption.Hide();
        HideBackPanel();
    }

    //public void ShowShipPanel()
    //{
    //    _mapInTouchControl = false;
    //    PanelShip.Show();
    //    ShowBackPanel();
    //}
    //public void HideShipPanel()
    //{
    //    _mapDoInTouch = true;
    //    PanelShip.Hide();
    //    HideBackPanel();
    //}

    /// <summary>
    /// Большая панель города.
    /// </summary>
    public void HideCityPanel()
    {
        PanelAction.Hide(); //- 
        _mapDoInTouch = true;
        HideBackPanel();
    }
    public void ShowCityPanel()
    {
        if (!_mapInTouchControl) return;

        _mapInTouchControl = false;
        _mapDoInTouch = false;

        PanelAction.Show(_currenctSelectCity, IsCityCurrent(_currenctSelectCity));
        ShowBackPanel();
    }


    /// <summary>
    /// Панель помощи.
    /// </summary>
    public void HideHelpPanel()
    {
        PanelHelp.Hide(); //- 
        _mapDoInTouch = true;
        HideBackPanel();
    }
    public void ShowHelpPanel(Int32 tag)
    {
        if (!_mapInTouchControl) return;

        _mapInTouchControl = false;
        _mapDoInTouch = false;

        PanelHelp.Show(tag);
        ShowBackPanel();
    }


    /// <summary>
    /// Быстрая панель управления внизу.
    /// </summary>
    private void HideActionPanel()
    {
        PanelCityAction.Hide();
    }
    private void ShowActionPanel()
    {
       //- PanelAction.Show(_currenctCity);
        PanelCityAction.Show(_currenctSelectCity, IsCityCurrent(_currenctSelectCity));
        //ShowBackPanel();
    }

    private bool _mapDoInTouch = false;
    private void MapInTouchInEndUpdate()
    {
        _mapInTouchControl = true;
        _mapDoInTouch = false;
        GestHelpers.ClearMove();
    }
    #endregion
    #region update working panels

    private void UpdateDayNum()
    {
        DayNumTxt.text = _mapLife.Day.ToString();
    }

    /// <summary>
    /// Обновление данных общей панели ресурсов.
    /// </summary>
    public void WorkPanelUpdate()
    {
        foreach (var res in _mapLife.Resources)
        {
            PanelWork.SetRes(res);
        }
    }
    
    /// <summary>
    /// Обновление панелей после дня.
    /// </summary>
    private void UpdateDayPanels()
    {
        WorkPanelUpdate();
        PanelCitySmall.UpdateView();
        PanelCityAction.SetCityAsCurrent(IsCityCurrent(_currenctSelectCity));
        PanelCityAction.UpdateView();
    }

    /// <summary>
    /// Обновление всех зависящих панелей от вычислений.
    /// </summary>
    public void UpdateMathPanels()
    {
        _mapLife.UpdateViewDay();
        WorkPanelUpdate();
        PanelAction.UpdateView();
        PanelCitySmall.UpdateView();
        PanelCityAction.UpdateView();
    }

    #endregion


    #region play level
    /// <summary>
    /// Запуск текущего уровня.
    /// </summary>
    public void PlayLevel()
    {
        if (_currenctSelectCity == null) return;
        //FarLife.SaveGame();

        FlyToCity();
    }
    public void PlayLevelFromMap()
    {
        if (!IsMapInTouch) return;
        PlayLevel();
    }

    private void FlyToCity()
    {
        if (_currenctSelectCity == null)
        {
            //Log.SetText("Не выбран город для перелета");
            return;
        }
        _mapLife.SetNextCity(_currenctSelectCity);
        HideCityPanel();

        // запуск уровня
        StartLevel();
    }

    private void StartLevel()
    {
        //+ счетчик текущей попытки полета
        if (FarLife.GlobalData.LastRunCity == FarLife.MapLife.NextCity.Model.Id)
        {
            FarLife.GlobalData.LastCityCountOfRuns++;
        }
        else
        {
            FarLife.GlobalData.LastRunCity = FarLife.MapLife.NextCity.Model.Id;
            FarLife.GlobalData.LastCityCountOfRuns = 1;
        }

        //debugStartLevel(); // TODO: debug
        FarLife.GoToLevel();
    }
    private void debugStartLevel()
    {
        _mapLife.SetLevelStatus(FarStatusLevel.LevelWin);
        _mapLife.AddMaterial(120);
        //FarLife.SaveGame(); // сохраняем по завершении миссии
        FarLife.GoToMap();
    }

    private void LevelEnd(Boolean isWin)
    {
        // сбор ресурсов и прочее (это должно выполниться по окончании уровня)
        _mapLife.LevelEnd(isWin);

        _currenctSelectCity = _mapLife.CurrenctCity;
        // позиция корабля
        ShipSetPosition(_mapLife.CurrenctCity.Position);

        //- обновление панелей после сбора и добычи ресурсов
        UpdateDayPanels();
    }

    #endregion


    #region Button press
    /// <summary>
    /// Нажали Back.
    /// </summary>
    private void BackPressed()
    {
        // закрываем панель города
        if (PanelAction.IsShowed)
        {
            DeselectCities();
            return;
        }
        // закрываем панель помощи
        if (PanelHelp.IsShowed)
        {
            HideHelpPanel();
            return;
        }

        //// закрываем панель корабля
        //if (PanelShip.IsShowed)
        //{
        //    HideShipPanel();
        //    return;
        //}
        // открываем панель опций
        if (!PanelOption.IsShowed)
        {
            ShowOptionPanel();
            //x PanelOption.Show();
            return;
        }
        else
        {
            HideOptionPanel();
            //x PanelOption.Hide();
            return;
        }
        // выйти
        //GoToMenu();   // никогда не выходим, только по кнопке в меню
    }

    public void GoToMenu()
    {
        //FarLife.SaveGame();
        FarLife.GoToMenu();
    }
    private bool _isTouchMoving = false;
    #endregion

    private float _diffTimeTap = 0.0f;
    private bool _difTimeGo = false;
    private float _diffTimeTapMax = 0.7f;
    private Int32 _difCurCityId = -1;

    // Update is called once per frame
    private void Update()
    {
        FarLife.Update();
        var delta = Time.deltaTime;
        //_mapDoInTouch = false;

        //+ comets
        CommetAddLogic();


        DisplayHelper.UpdateScreenSacle();

        #region touches

        // обработка только если карта
        if (_mapInTouchControl)
        {
            var moveAreaY = 60;
            var notTapAreaY = 1150;

            // TAP
            var tap = GestHelpers.GetTap(false); //- 
            if (tap != null)
            {
                if (!_isTouchMoving) //- работаем только после завершения перемещения
                {
                    Ray ray = Camera.main.ScreenPointToRay(tap.Position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        var o = (GameObject)hit.transform.gameObject;
                        var cmap = o.GetComponent<CityMapItem>();
                        if (cmap != null)
                        {
                            cmap.OnPointerClick();
                            //+ pseudo double tap
                            if (_difCurCityId == cmap.CityModel.Model.Id && _difTimeGo)
                            {
                                ShowCityPanel();
                            }
                            _difCurCityId = cmap.CityModel.Model.Id;
                            _difTimeGo = true;
                        }
                    }
                    else
                    {
                        var ts = DisplayHelper.UnityToScreenCoord(tap.Position);
                        if (ts.y <= notTapAreaY) //- область кнопок не учитываем
                            DeselectCities();
                    }
                }
                _isTouchMoving = false;
            }

            // MOVE map
            var move = GestHelpers.GetMove(true);
            // TODO: при маленьком смещении не считать как движение?
            if (move != null && move.Position.y >= moveAreaY)
            {
                _isTouchMoving = true;
                //Log.SetText(move.Position.ToString());
                // move map
                WorldMap.Move(delta, move.DeltaPosition);
            }
        }
        #endregion

        //+ double tap timer
        if (_difTimeGo)
        {
            _diffTimeTap += Time.deltaTime;
            if (_diffTimeTap >= _diffTimeTapMax)
            {
                _diffTimeTap = 0.0f;
                _difCurCityId = -1;
                _difTimeGo = false;
            }
        }


        if (_mapDoInTouch) MapInTouchInEndUpdate();
    }


    void OnGUI()
    {
        
        //- рисует движок
        FarLife.OnGUI();
    }

}
