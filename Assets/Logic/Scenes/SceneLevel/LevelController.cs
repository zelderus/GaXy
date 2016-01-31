using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework.Helpers;
using ZelderFramework.Maths;

public class LevelController : MonoBehaviour
{

    public LevelParalaxLogic Paralax1;
    public LevelParalaxLogic Paralax2;
    public LevelPanelOptionLogic PanelOption;
    public LevelPanelWorkLogic PanelWork;
    public LevelPanelShipLogic PanelShip;
    public LevelPanelMarketLogic PanelMarket;
    public ShipFlyLogic ShipLogic;

    public Transform MaterialParent;

    public Transform BigBack;
    public Color BigBackJopedColor;

    public Transform HitBlock;
    
    public Image BoomImg;
    public Color BoomColor;


    public LogScript Log;

    public LevelEndTextLogic WinText;
    public LevelEndTextLogic LoseText;


    private MapLife _mapLife;
    private ShipLife _ship;
    private City _city;
    private LevelManager _manager;
    public LevelManager Manager { get { return _manager; } }

    private Boolean _inTouchControl = true;
    public Boolean IsInTouch { get { return _inTouchControl; } }

    private Boolean _inited = false;
    private FarStatusLevel _statusLevel = FarStatusLevel.LevelLose;


    private GameObject _parentMaterialMap;
    private GameObject _matPrefab;
    private GameObject _parentPanelForCounts;
    private GameObject _matCountPrefab;

    private GameObject _parentEenemyPrefab;
    private GameObject _enemy1Prefab;
    private GameObject _enemy2Prefab;
    private GameObject _enemy3Prefab;
    private GameObject _enemy4Prefab;
    private GameObject _enemy5Prefab;
    private GameObject _enemy6Prefab;
    private GameObject _enemyPen1Prefab;
    private GameObject _enemyPen2Prefab;
    private GameObject _enemyPen3Prefab;
    private GameObject _enemyPen4Prefab;
    private GameObject _enemyPen5Prefab;
    private GameObject _enemyPen6Prefab;
    private GameObject _enemyBoss1Prefab;
    private GameObject _enemyBoss2Prefab;
    private GameObject _enemyBoss3Prefab;

    private GameObject _bomm1Prefab;

    private GameObject _parentBulletShipPrefab;
    private GameObject _bombPrefab;
    private GameObject _bulletShip1Prefab;
    private GameObject _bulletShip2Prefab;
    private GameObject _bulletShip3Prefab;
    private GameObject _bulletEnemy1Prefab;
    private GameObject _bulletEnemy2Prefab;
    private GameObject _bulletEnemy3Prefab;
    private GameObject _bulletEnemy10Prefab;
    private GameObject _bulletEnemy11Prefab;

    private GameObject _trashPrefab;

    private Transform _mainWaypoints;
    private Dictionary<Int32, ParentWaypointModel> _waypoints;
    private Dictionary<Int32, ParentWaypointModel> _waypointsBoss;
    private Dictionary<Int32, ParentWaypointModel> _waypointsBossTime;
    //private Int32 _currentWayIndex = -1;
    //private Transform _currentWaypoint = null;
    private AudioSource _audio;


    void Awake()
    {
        FarLife.Init();
        FarLife.ScreenInit(BackPressed);
    }
	// Use this for initialization
	void Start ()
	{
        _audio = this.GetComponent<AudioSource>();

        _statusLevel = FarStatusLevel.LevelLose;
        _mapLife = FarLife.MapLife;
	    _ship = FarLife.ShipLife;
        _ship.PrepareToFly();



        FarLife.FarStat.OnLevelRun(); // stat

        //+ сохраняем
        FarLife.SaveGame();

        InitPrefabs();

        _city = _mapLife.NextCity;
        InitDebugCity();            //?++ !!!!!! для отладки сцены !!!!!

        _manager = new LevelManager(this, _ship, _city);
        ShipLogic.Init(_ship, this);
	    ShipLogic.SetPosition(0.0f, 0.0f);

        // panels
        PanelOption.Init();
        PanelWork.Init(_ship);
        PanelShip.Init(_ship);
        PanelMarket.Init(_ship, this);
        //
        WinText.Init(FarLife.GetText(FarText.Level_Win));
        LoseText.Init(FarLife.GetText(FarText.Level_Lose));

	    InitLevel();
        if (FarLife.IsJopCompleted) BigBackSetAsJoped();

        GestHelpers.ClearMove();
        //- сообщаем движку что готовы к сцене
        FarLife.OnScreenLoaded();
        _inited = true;
        SetPause(true);


        ShowMarketPanel();
	}

    private void InitPrefabs()
    {
        _parentMaterialMap = GameObject.Find("MaterialParent") as GameObject;
        _matPrefab = Resources.Load("Prefabs/Level/MaterialPref", typeof(GameObject)) as GameObject;
        _parentPanelForCounts = GameObject.Find("PanelForCounts") as GameObject;
        _matCountPrefab = Resources.Load("Prefabs/Level/MaterialCountPref", typeof(GameObject)) as GameObject;

        _parentEenemyPrefab = GameObject.Find("EnemyParent") as GameObject;
        _enemy1Prefab = Resources.Load("Prefabs/Level/Enemies/Enemy1Pref", typeof(GameObject)) as GameObject;
        _enemy2Prefab = Resources.Load("Prefabs/Level/Enemies/Enemy2Pref", typeof(GameObject)) as GameObject;
        _enemy3Prefab = Resources.Load("Prefabs/Level/Enemies/Enemy3Pref", typeof(GameObject)) as GameObject;
        _enemy4Prefab = Resources.Load("Prefabs/Level/Enemies/Enemy4Pref", typeof(GameObject)) as GameObject;
        _enemy5Prefab = Resources.Load("Prefabs/Level/Enemies/Enemy5Pref", typeof(GameObject)) as GameObject;
        _enemy6Prefab = Resources.Load("Prefabs/Level/Enemies/Enemy6Pref", typeof(GameObject)) as GameObject;
        _enemyPen1Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyPen1Pref", typeof(GameObject)) as GameObject;
        _enemyPen2Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyPen2Pref", typeof(GameObject)) as GameObject;
        _enemyPen3Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyPen3Pref", typeof(GameObject)) as GameObject;
        _enemyPen4Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyPen4Pref", typeof(GameObject)) as GameObject;
        _enemyPen5Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyPen5Pref", typeof(GameObject)) as GameObject;
        _enemyPen6Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyPen6Pref", typeof(GameObject)) as GameObject;
        _enemyBoss1Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyBoss1Pref", typeof(GameObject)) as GameObject;
        _enemyBoss2Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyBoss2Pref", typeof(GameObject)) as GameObject;
        _enemyBoss3Prefab = Resources.Load("Prefabs/Level/Enemies/EnemyBoss3Pref", typeof(GameObject)) as GameObject;

        _parentBulletShipPrefab = GameObject.Find("BulletShipParent") as GameObject;
        _bombPrefab = Resources.Load("Prefabs/Level/BulletShip/BombPref", typeof(GameObject)) as GameObject;
        _bulletShip1Prefab = Resources.Load("Prefabs/Level/BulletShip/BulletShip1Pref", typeof(GameObject)) as GameObject;
        _bulletShip2Prefab = Resources.Load("Prefabs/Level/BulletShip/BulletShip2Pref", typeof(GameObject)) as GameObject;
        _bulletShip3Prefab = Resources.Load("Prefabs/Level/BulletShip/BulletShip3Pref", typeof(GameObject)) as GameObject;

        //- взрывы врагов
        _bomm1Prefab = Resources.Load("Prefabs/Level/Boom1Pref", typeof(GameObject)) as GameObject;
        //- снаярды врагов
        _bulletEnemy1Prefab = Resources.Load("Prefabs/Level/BulletEnemy/BulletEnemy1Pref", typeof(GameObject)) as GameObject;
        _bulletEnemy2Prefab = Resources.Load("Prefabs/Level/BulletEnemy/BulletEnemy2Pref", typeof(GameObject)) as GameObject;
        _bulletEnemy3Prefab = Resources.Load("Prefabs/Level/BulletEnemy/BulletEnemy3Pref", typeof(GameObject)) as GameObject;
        _bulletEnemy10Prefab = Resources.Load("Prefabs/Level/BulletEnemy/BulletEnemy10Pref", typeof(GameObject)) as GameObject;
        _bulletEnemy11Prefab = Resources.Load("Prefabs/Level/BulletEnemy/BulletEnemy11Pref", typeof(GameObject)) as GameObject;

        _trashPrefab = Resources.Load("Prefabs/Level/TrashPref", typeof(GameObject)) as GameObject;
        //+ ways
        InitRoutes();
    }

    #region Init routes
    private void InitRoutes()
    {
        //! с интерполяцией без остановок
        var indx = 0;
        _waypoints = new Dictionary<int, ParentWaypointModel>();
        _mainWaypoints = GameObject.Find("EnemyRoutes").transform;
        foreach (Transform child in _mainWaypoints)
        {
            //x var ways = new List<WaypointModel>();
            var w = new ParentWaypointModel();
            _waypoints.Add(indx, w);

            var ws = new List<Vector3>();
            //+ перебираем с редактора метки
            foreach (Transform way in child)
            {
                ws.Add(way.position);
            }
            //+ интерполируем (становится гораздо больше точек)
            var nws = Intepolator.NewCatmullRom(ws.ToArray(), ws.Count(), false);
            //+ суем в модель
            foreach (var way in nws)
            {
                const float t = 0.0f;
                w.Waypoints.Add(new WaypointModel(way, t));
            }
            //- next
            indx++;
        }
        Destroy(_mainWaypoints.gameObject);

        ////! без интерполяции
        //indx = 0;
        //_waypointsBoss = new Dictionary<int, ParentWaypointModel>();
        //var mainBoosWaypoints = GameObject.Find("EnemyBossRoutes").transform;
        //foreach (Transform child in mainBoosWaypoints)
        //{
        //    var w = new ParentWaypointModel();
        //    _waypointsBoss.Add(indx, w);

        //    //- parent data
        //    var mainWl = child.GetComponent<WaypointLogic>();
        //    if (mainWl != null)
        //    {
        //        w.LoopStartIndex = mainWl.LoopStartIndexWay;
        //    }

        //    foreach (Transform way in child)
        //    {
        //        //var t = 0.0f;
        //        var wl = way.GetComponent<WaypointLogic>();
        //        //if (wl != null) t = wl.Timer;
        //        //ways.Add(new WaypointModel(way, t, wl.WithoutRotate));
        //        w.Waypoints.Add(new WaypointModel(wl));
        //    }
        //    //- next
        //    indx++;
        //}
        //! с интерполяцией без остановок
        indx = 0;
        _waypointsBoss = new Dictionary<int, ParentWaypointModel>();
        var mainBoosWaypoints = GameObject.Find("EnemyBossRoutes").transform;
        foreach (Transform child in mainBoosWaypoints)
        {
            //x var ways = new List<WaypointModel>();
            var w = new ParentWaypointModel();
            _waypointsBoss.Add(indx, w);

            //- parent data
            var mainWl = child.GetComponent<WaypointLogic>();
            if (mainWl != null)
            {
                w.LoopStartIndex = mainWl.LoopStartIndexWay;
            }

            var ws = new List<Vector3>();
            //+ перебираем с редактора метки
            foreach (Transform way in child)
            {
                ws.Add(way.position);
            }
            //+ интерполируем (становится гораздо больше точек)
            var nws = Intepolator.NewCatmullRom(ws.ToArray(), ws.Count(), false);
            //+ суем в модель
            Int32 iid = 0;
            foreach (var way in nws)
            {
                const float t = 0.0f;
                w.Waypoints.Add(new WaypointModel(way, t));

                ////?++ DEBUG
                //if (mainWl.WithDebug)
                //{
                //    GameObject pref = Instantiate(_trashPrefab, mainBoosWaypoints.gameObject.transform.position, Quaternion.identity) as GameObject;
                //    pref.transform.SetParent(mainBoosWaypoints.gameObject.transform);
                //    pref.transform.localPosition = new Vector3(way.x, way.y, way.z);
                //    var trl = pref.GetComponent<TrashObjLogic>();
                //    trl.Index = iid++;
                //}
            }
            //- next
            indx++;
        }
        Destroy(mainBoosWaypoints.gameObject);

        //! без интерполяции с остановками
        indx = 0;
        _waypointsBossTime = new Dictionary<int, ParentWaypointModel>();
        var mainBoosTimesWaypoints = GameObject.Find("EnemyBossTimeRoutes").transform;
        foreach (Transform child in mainBoosTimesWaypoints)
        {
            var w = new ParentWaypointModel();
            _waypointsBossTime.Add(indx, w);

            //- parent data
            var mainWl = child.GetComponent<WaypointLogic>();
            if (mainWl != null)
            {
                w.LoopStartIndex = mainWl.LoopStartIndexWay;
            }

            foreach (Transform way in child)
            {
                var wl = way.GetComponent<WaypointLogic>();
                w.Waypoints.Add(new WaypointModel(wl));
            }
            
            //- next
            indx++;
        }
        Destroy(mainBoosTimesWaypoints.gameObject);
    }
    #endregion

    private void InitDebugCity()
    {
        if (_city == null)
        {
            _city = _mapLife.Cities.FirstOrDefault();
            _mapLife.CurrenctCity = _city;
            _mapLife.NextCity = _city;
        }
    }

    /// <summary>
    /// Инициализация уровня.
    /// </summary>
    private void InitLevel()
    {
        Paralax1.Init(_ship);
        Paralax2.Init(_ship);
        //_city.Title

        //! Игнор столкновений
        /* 
         *  8   - ResMaterialLay
         *  9   - Ship
         *  10  - Enemy
         *  11  - BulletsShip
         *  12  - BulletsEnemy
         *  13  - Bomb
        */
        //- ship
        Physics.IgnoreLayerCollision(9, 11, true);
        Physics.IgnoreLayerCollision(9, 13, true);
        //- enemy
        Physics.IgnoreLayerCollision(10, 12, true);
        Physics.IgnoreLayerCollision(10, 8, true);
        //- bullets
        Physics.IgnoreLayerCollision(11, 8, true);
        Physics.IgnoreLayerCollision(12, 8, true);
        Physics.IgnoreLayerCollision(13, 8, true);

    
    }

    /// <summary>
    /// Suspending.
    /// </summary>
    /// <param name="pausing"></param>
    public void OnApplicationPause(bool pausing)
    {
        FarLife.GameLife.OnApplicationPause(pausing);
        if (!_inited) return;

        if (!PanelMarket.IsShowed)
            ShowOptionPanel();
    }


    private void BigBackSetAsJoped()
    {
        BigBack.GetComponent<Renderer>().material.color = BigBackJopedColor;
    }



    #region Sound
    public AudioClip AudioClick;
    public AudioClip AudioBow;
    public AudioClip AudioUp;
    public AudioClip AudioFig;
    public AudioClip AudioStart;
    public AudioClip AudioCoins;
    public AudioClip AudioGameOver;
    public AudioClip AudioGameWin;
    public AudioClip AudioBomb;
    public AudioClip AudioTechStop;
    public AudioClip AudioTick;
    public AudioClip AudioTick2;

    private void SoundClick()
    {
        _audio.PlayOneShot(AudioClick);
    }
    public void SoundBow()
    {
        _audio.PlayOneShot(AudioBow);
    }
    public void SoundUp()
    {
        _audio.PlayOneShot(AudioUp);
    }
    public void SoundFig()
    {
        _audio.PlayOneShot(AudioFig);
    }
    public void SoundStart()
    {
        _audio.PlayOneShot(AudioStart);
    }
    public void SoundCoins()
    {
        _audio.PlayOneShot(AudioCoins);
    }
    public void SoundGameOver()
    {
        _audio.PlayOneShot(AudioGameOver);
    }
    public void SoundGameWin()
    {
        _audio.PlayOneShot(AudioGameWin);
    }
    public void SoundBomb()
    {
        _audio.PlayOneShot(AudioBomb);
    }
    public void SoundTechStop()
    {
        _audio.PlayOneShot(AudioTechStop);
    }
    public void SoundTick()
    {
        _audio.PlayOneShot(AudioTick);
    }
    public void SoundEnemyTick()
    {
        _audio.PlayOneShot(AudioTick2);
    }
    #endregion




    #region materials
    /// <summary>
    /// Помещение материала.
    /// </summary>
    public void PlaceMaterial(Vector2 pos, Int32 count, float speed = 1.0f)
    {
        if (count <= 0) return;

        GameObject pref = Instantiate(_matPrefab, _parentMaterialMap.transform.position, Quaternion.identity) as GameObject;
        pref.transform.SetParent(_parentMaterialMap.transform);
        pref.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);
        pref.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //- logic
        MaterialLogic mat = pref.GetComponent<MaterialLogic>();
        mat.Init(this);
        mat.Count = count;
        mat.Speed = speed;
        mat.SetPosition(pos);
        //- add
        //_materials.Add(mat);
    }
    public void DeleteMaterial(GameObject mat)
    {
        Destroy(mat);
    }
    /// <summary>
    /// Помещение количества собранного материала.
    /// </summary>
    public void PlaceMaterialFloat(Vector3 pos, Int32 count, float speed)
    {
        GameObject resImgPref = Instantiate(_matCountPrefab, _parentPanelForCounts.transform.position, Quaternion.identity) as GameObject;
        resImgPref.transform.SetParent(_parentPanelForCounts.transform);
        //resImgPref.transform.localPosition = Camera.main.WorldToScreenPoint(pos);//new Vector3(pos.x, pos.y, 0.0f);

        Vector2 sp = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        resImgPref.GetComponent<RectTransform>().position = sp;
        resImgPref.transform.localScale = Vector3.one;

        var matCountLogic = resImgPref.GetComponent<MaterialCountLogic>();
        matCountLogic.Init(this, count, speed);

        //Log.SetText(sp.ToString());

        Destroy(resImgPref, 3.0f);
    }
    #endregion
    #region enemies

    public void PlaceEnemy(EnemyIndexes enemyIndex, Int32 routeIndex, bool routeInversed)
    {
        //var route = 0;
        GameObject gob = null;
        //var isBoss = false;
        float offsetZ = 0.0f;
        var scale = new Vector3(0.4f, 0.4f, 0.4f);

        switch (enemyIndex) //! типы врагов в Enemy.cs
        {
            case EnemyIndexes.Loh1: gob = _enemy1Prefab; offsetZ = 0.0f; break;
            case EnemyIndexes.Loh2: gob = _enemy2Prefab; offsetZ = 0.0f; break;
            case EnemyIndexes.Loh3: gob = _enemy3Prefab; offsetZ = 0.0f; break;
            case EnemyIndexes.Loh4: gob = _enemy4Prefab; offsetZ = 0.0f; break;
            case EnemyIndexes.Loh5: gob = _enemy5Prefab; offsetZ = 0.0f; break;
            case EnemyIndexes.Loh6: gob = _enemy6Prefab; offsetZ = 0.0f; break;

            case EnemyIndexes.Pen1: gob = _enemyPen1Prefab; scale = new Vector3(0.4f, 0.4f, 0.4f); offsetZ = 0.0f; break;
            case EnemyIndexes.Pen2: gob = _enemyPen2Prefab; scale = new Vector3(0.7f, 0.7f, 0.7f); offsetZ = 0.0f; break;
            case EnemyIndexes.Pen3: gob = _enemyPen3Prefab; scale = new Vector3(0.4f, 0.4f, 0.4f); offsetZ = 0.0f; break;
            case EnemyIndexes.Pen4: gob = _enemyPen4Prefab; scale = new Vector3(0.7f, 0.7f, 0.7f); offsetZ = 0.0f; break;
            case EnemyIndexes.Pen5: gob = _enemyPen5Prefab; scale = new Vector3(0.4f, 0.4f, 0.4f); offsetZ = 0.0f; break;
            case EnemyIndexes.Pen6: gob = _enemyPen6Prefab; scale = new Vector3(0.7f, 0.7f, 0.7f); offsetZ = 0.0f; break;

            case EnemyIndexes.Boss1: gob = _enemyBoss1Prefab; offsetZ = 1.0f; break;
            case EnemyIndexes.Boss2: gob = _enemyBoss2Prefab; offsetZ = 1.0f; break;
            case EnemyIndexes.Boss3: gob = _enemyBoss3Prefab; offsetZ = 1.0f; break;
        }

        //var firstWay = GetCurrentWaypoint(route, 0, isBoss);
        //Vector2 pos = firstWay.Waypoint.position;
        Vector2 pos = new Vector2(-100, -10);// firstWay.Position(routeInversed);

        GameObject pref = Instantiate(gob, _parentEenemyPrefab.transform.position, Quaternion.identity) as GameObject;
        pref.transform.SetParent(_parentEenemyPrefab.transform);
        pref.transform.localPosition = new Vector3(pos.x, pos.y, offsetZ);
        pref.transform.localScale = scale;// new Vector3(0.4f, 0.4f, 0.4f);

        //- logic
        if (enemyIndex == EnemyIndexes.Boss1)
        {
            var bl = pref.GetComponent<Boss1Logic>();
            bl.Init(this, routeIndex, routeInversed, _manager.GetCityIndex());
        }
        else if (enemyIndex == EnemyIndexes.Boss2)
        {
            var bl = pref.GetComponent<Boss1Logic>();
            bl.Init(this, routeIndex, routeInversed, _manager.GetCityIndex());
        }
        else if (enemyIndex == EnemyIndexes.Boss3)
        {
            var bl = pref.GetComponent<Boss1Logic>();
            bl.Init(this, routeIndex, routeInversed, _manager.GetCityIndex());
        }
        else
        {
            var enemy = pref.GetComponent<EnemyLogic>();
            enemy.Init(this, routeIndex, routeInversed, _manager.GetCityIndex());
        }
        
    }
    /// <summary>
    /// Враг уничтожен.
    /// </summary>
    public void EnemyDied()
    {
        ShipLogic.ShipLife.EnemyDestoy();
        FarLife.FarStat.OnEnemyDie();
        _manager.EnemyRemoved();
    }
    /// <summary>
    /// Уничтожен босс.
    /// </summary>
    public void EnemyDiedIsBoss()
    {
        FarLife.FarStat.OnBossDie();
    }
    /// <summary>
    /// Враг улетел.
    /// </summary>
    public void EnemyFloated()
    {
        _manager.EnemyRemoved();
    }
    #endregion
    #region bullet ship

    public void PlaceBulletShip(Int32 gunIndex)
    {
        if (gunIndex == 1)
        {
            var pref = Instantiate(_bulletShip1Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = ShipLogic.transform.position;
            pref.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            var gunOffsetPos = new Vector3(0.0f, 0.3f, 0.0f);
            //- logic
            var bullet = pref.GetComponent<BulletShipLogic>();
            bullet.Init(this, _ship, ShipLogic.transform.position + gunOffsetPos, new Vector3(0, 0.1f, 0), 0);
        }
        if (gunIndex == 2)
        {
            //+ left
            var pref = Instantiate(_bulletShip2Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = ShipLogic.transform.position;
            pref.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            var gunOffsetPos = new Vector3(-0.31f, -0.0f, 0.2f);
            //- logic
            var bullet = pref.GetComponent<BulletShipLogic>();
            bullet.Init(this, _ship, ShipLogic.transform.position + gunOffsetPos, new Vector3(0, 0.1f, 0), 1);

            //+ right
            pref = Instantiate(_bulletShip2Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = ShipLogic.transform.position;
            pref.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            gunOffsetPos = new Vector3(0.31f, -0.0f, 0.2f);
            //- logic
            var bullet2 = pref.GetComponent<BulletShipLogic>();
            bullet2.Init(this, _ship, ShipLogic.transform.position + gunOffsetPos, new Vector3(0, 0.1f, 0), -1);
        }
        if (gunIndex == 3)
        {
            //+ left
            var pref = Instantiate(_bulletShip3Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = ShipLogic.transform.position;
            pref.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            var gunOffsetPos = new Vector3(-0.31f, -0.0f, 0.24f);
            //- logic
            var bullet = pref.GetComponent<BulletShipLogic>();
            bullet.Init(this, _ship, ShipLogic.transform.position + gunOffsetPos, new Vector3(-0.05f, 0.1f, 0), 1);

            //+ right
            pref = Instantiate(_bulletShip3Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = ShipLogic.transform.position;
            pref.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            gunOffsetPos = new Vector3(0.31f, -0.0f, 0.24f);
            //- logic
            var bullet2 = pref.GetComponent<BulletShipLogic>();
            bullet2.Init(this, _ship, ShipLogic.transform.position + gunOffsetPos, new Vector3(0.05f, 0.1f, 0), -1);
        }
    }

    private Int32 _bombId = 0;
    public void PlaceBomb()
    {
        var pref = Instantiate(_bombPrefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
        pref.transform.SetParent(_parentBulletShipPrefab.transform);
        pref.transform.localPosition = ShipLogic.transform.position;
        pref.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //- logic
        var bullet = pref.GetComponent<BombLogic>();
        bullet.Init(this, _ship, ShipLogic.transform.position, _bombId++);
    }

    #endregion
    #region bullet enemy

    public void PlaceBulletEnemy(Int32 gunIndex, Vector3 pos, float damageFactor, float speedFactor)
    {
        damageFactor = damageFactor < 1.0f ? 1.0f : damageFactor;
        speedFactor = speedFactor < 1.0f ? 1.0f : speedFactor;

        GameObject pref = null;
        Vector3 gunOffsetPos = Vector3.zero;
        // boss 1
        if (gunIndex == 0)
        {
            pref = Instantiate(_bulletEnemy1Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = pos;
            pref.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            gunOffsetPos = new Vector3(0, 0, -1);
        }
        if (gunIndex == 1)
        {
            pref = Instantiate(_bulletEnemy2Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = pos;
            pref.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            gunOffsetPos = new Vector3(0, 0, -1);
        }
        if (gunIndex == 2)
        {
            pref = Instantiate(_bulletEnemy3Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = pos;
            pref.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            gunOffsetPos = new Vector3(0, 0, -1);
        }



        // pens
        if (gunIndex == 10)
        {
            pref = Instantiate(_bulletEnemy10Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = pos;
            pref.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        if (gunIndex == 11)
        {
            pref = Instantiate(_bulletEnemy11Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = pos;
            pref.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        //- logic
        var bullet = pref.GetComponent<BulletEnemyLogic>();
        bullet.Init(this, pos + gunOffsetPos, damageFactor, speedFactor);
    }

    #endregion
    #region booms

    public void PlaceBoomEnemy(Int32 boomIndex, Vector3 pos)
    {
        GameObject pref = null;
        if (boomIndex == 0)
        {
            pref = Instantiate(_bomm1Prefab, _parentBulletShipPrefab.transform.position, Quaternion.identity) as GameObject;
            pref.transform.SetParent(_parentBulletShipPrefab.transform);
            pref.transform.localPosition = pos;
            pref.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        // TODO: boomIndex

        Destroy(pref, 1.0f);
    }

    #endregion
    #region ways
    //public Transform GetCurrentRandomWaypoint()
    //{
    //    int i = UnityEngine.Random.Range(0, _waypoints.Count);
    //    // Получаем случайный вейпоинт по индексу (i)
    //    Transform current = _waypoints[i];
    //    // Возвращаем полученный текущий вейпоинт
    //    return current;
    //}
    //public Transform GetCurrentWaypoint()
    //{
    //    _currentWayIndex++;
    //    if (_currentWayIndex >= _waypoints.Count()) return null;
    //    return _waypoints[_currentWayIndex];
    //}

    private Dictionary<Int32, ParentWaypointModel> GetParentWaypoint(Int32 bossRouteNum = 0)
    {
        var ways = _waypoints;
        switch (bossRouteNum)
        {
            case 0: ways = _waypoints; break;
            case 1: ways = _waypointsBoss; break;
            case 2: ways = _waypointsBossTime; break;
        }
        return ways;
    }

    public WaypointModel GetCurrentWaypoint(Int32 routeIndex, Int32 wayIndex, Int32 bossRouteNum)
    {
        var ways = GetParentWaypoint(bossRouteNum);
        if (!ways.ContainsKey(routeIndex)) return null;
        var route = ways[routeIndex];
        if (wayIndex >= route.Waypoints.Count()) return null;
        return route.Waypoints[wayIndex];
    }
    /// <summary>
    /// Начальный индекс маршрута.
    /// <remarks>Зацикленные не всегда возвращаются в точку старта.</remarks>
    /// </summary>
    /// <returns></returns>
    public Int32 GetRouteStartIndex(Int32 routeIndex, Int32 bossRouteNum)
    {
        var ways = GetParentWaypoint(bossRouteNum);
        if (!ways.ContainsKey(routeIndex)) return 0;
        var route = ways[routeIndex];
        return route.LoopStartIndex;
    }

    #endregion


    #region Logic
    /// <summary>
    /// Получение материала.
    /// </summary>
    /// <param name="count"></param>
    public void AddMaterial(Int32 count)
    {
        _mapLife.AddMaterial(count);
        //+ panel update
        PanelWork.UpdateMaterial();
    }
    /// <summary>
    /// Нанесение урона.
    /// </summary>
    /// <param name="damage"></param>
    public void AddDamage(float damage)
    {
        if (_ship.IsDied) return;

        var dif = _ship.AddHealth(-damage);
        ShipLogic.Damage(dif);

        if (dif > 0.0f)
        {
            BoomAnimStart();
            SoundBow();
        }

        // panel health
        PanelWork.UpdateHealth();
        PanelShip.UpdateHealth();

        if (_ship.IsDied)
        {
            OnStartLevelLose();
        }
    }

    private float _boomTime = 1.0f;
    private bool _bommInAnim = false;
    //private float _boomAlpha
    private void BoomAnimStart()
    {
        _bommInAnim = true;
        //BoomImg.color = BoomColor*new Color(1, 1, 1);
        _boomTime = 1.0f;

        BoomImg.color = BoomColor;// * new Color(1, 1, 1, 1);
        BoomImg.gameObject.SetActive(true);
    }

    private void BommAnimStop()
    {
        _bommInAnim = false;
        BoomImg.gameObject.SetActive(false);
    }
    private void BoomAnimDo()
    {
        if (!_bommInAnim) return;
        _boomTime -= Time.deltaTime;
        if (_boomTime <= 0.0f)
        {
            BommAnimStop();
        }
        BoomImg.color = BoomColor * new Color(1, 1, 1, Mathf.Lerp(0.0f, 1.0f, _boomTime));
    }


    /// <summary>
    /// Получение здоровья.
    /// </summary>
    /// <param name="hp"></param>
    public void AddHealth(float hp)
    {
        if (_ship.IsDied) return;

        var dif = _ship.AddHealth(hp);
        ShipLogic.AddHealth(dif);

        // panel health
        PanelWork.UpdateHealth();
        PanelShip.UpdateHealth();
    }

    /// <summary>
    /// Нажали добавить HP.
    /// </summary>
    public void ShipPanelAddHealthBtnClick()
    {
        if (PanelShip.CanAddHealth())
        {
            _ship.UseBonusHealth();
            SoundUp();
            PanelShip.HealthUse();
            AddHealth(_ship.ShipBonusHealth);
        }
    }
    /// <summary>
    /// Включение щита.
    /// </summary>
    public void ShipPanelAddShieldBtnClick()
    {
        if (PanelShip.CanAddShield())
        {
            ShipLogic.EnableShield(_ship.ShipBonusShield);
            _ship.UseBonusShield();
            SoundUp();
            //
            PanelShip.ShieldUse();
            PanelShip.UpdateShieldBtn();
        }
    }
    /// <summary>
    /// Третий бонус.
    /// </summary>
    public void ShipPanelAddTreeBtnClick()
    {
        if (PanelShip.CanAddTree())
        {
            PlaceBomb();
            SoundBomb();
            _ship.UseBonusTree();
            //
            PanelShip.TreeUse();
            PanelShip.UpdateTreeBtn();
        }
    }
    #endregion

    

    #region Fly pos
    private readonly Vector3 _fingerOffset = new Vector3(0, 1.3f, 0);
    private void FlyToRay(Vector3 ray)
    {
        if (_ship.IsDied) return;
        //- конечная позиция (со смещением над пальцем)
        ray += _fingerOffset;
        //- смещаем корабль
        var slide = ShipLogic.Fly(ray);
        //- смещаем паралаксы
        var paralOffset = -((slide > 0.0f ? 1 : slide < 0.0f ? -1 : 0) * 4.0f);// -(slide * 60.0f);
        Paralax1.RotY = paralOffset;
        Paralax2.RotY = paralOffset;
    }


    #endregion


    #region panels

    public void SetPause(bool isPause)
    {
        _manager.Pause(isPause);
        ShipLogic.SetPause(isPause);
    }

    public void ShowMarketPanel()
    {
        _inTouchControl = false;
        SetPause(true);
        PanelMarket.Show();
    }
    public void HideMarketPanel()
    {
        _inTouchControl = true;
        PanelMarket.Hide();
        SoundStart();
        SetPause(false);
    }


    public void ShowOptionPanelFromGame()
    {
        ShowOptionPanel();
        SoundClick();
    }
    public void ShowOptionPanel()
    {
        _inTouchControl = false;
        SetPause(true);
        PanelOption.Show();
    }
    public void HideOptionPanel()
    {
        _inTouchControl = true;
        PanelOption.Hide();
        SoundClick();
        SetPause(false);
    }

    private void ShowEndMessage(Boolean isWin)
    {
        if (isWin) WinText.Show();
        else LoseText.Show();
    }
    #endregion
    #region ship touch panel

    private void RestoreTimeScale()
    {
        Time.timeScale = 1.0F;
    }

    private bool _isWeaponPanelShowed = false;
    private bool _inShipPanel = false;
    private bool _inShipPanelClick = false;
    private void ShowWeaponPanel()
    {
        if (_isWeaponPanelShowed || _goingToEnd) return;
        _isWeaponPanelShowed = true;
        // slow down
        Time.timeScale = 0.2F;
        // panel
        PanelShip.Show();
    }
    private void HideWeaponPanel()
    {
        if (!_isWeaponPanelShowed) return;
        _isWeaponPanelShowed = false;
        // normal speed
        RestoreTimeScale();
        // panel
        PanelShip.Hide();
    }
    //public void UpdateShipPanelCounters()
    //{
    //    PanelShip.UpdateCounters();
    //}

    public void WeaponPanelTouched()
    {
        //Debug.Log("touch");
        _inShipPanel = true;
        _shipPanelClicked = false;
    }

    //private bool _shipPanelUped = false;
    public void WeaponPanelUp()
    {
        //Debug.Log("up");
        //_shipPanelUped = true;

        _inShipPanel = false;
        //_shipPanelClicked = false;
    }
    private bool _shipPanelClicked = false;
    public void WeaponPanelClicked()
    {
        //Debug.Log("click");
        _inShipPanel = false;
        _shipPanelClicked = true;
    }
    
    

    #endregion


    #region level end

    private Boolean _goingToEnd = false;
    private float _timeToWndAnim = 3.0f;
    private void OnStartLevelWin()
    {
        _statusLevel = FarStatusLevel.LevelWin;
        _goingToEnd = true;
        FarLife.FarStat.OnLevelWin(); // stat
        HideWeaponPanel();
        ShipLogic.AnimToWin();
        ShowEndMessage(true);
        SoundGameWin();
    }
    private void OnStartLevelLose()
    {
        _statusLevel = FarStatusLevel.LevelLose;
        _goingToEnd = true;
        HideWeaponPanel();
        ShipLogic.AnimToLose();
        ShowEndMessage(false);
        SoundGameOver();

        //+ счетчик текущей попытки полета
        if (FarLife.GlobalData.LastRunCity == FarLife.MapLife.NextCity.Model.Id)
        {
            FarLife.GlobalData.LastCityCountOfRuns++;
        }
    }

    public void LevelLose()
    {
        _statusLevel = FarStatusLevel.LevelLose;
        LevelEnd();
    }

    /// <summary>
    /// Уровень закончен. Либо выигран, либо проигран.
    /// </summary>
    public void LevelEnd()
    {
        FarLife.FarStat.OnLevelEndTime(_manager.TotalTimeGo(), _statusLevel == FarStatusLevel.LevelWin);    // stat

        _mapLife.SetLevelStatus(_statusLevel);
        //FarLife.SaveGame(); // сохраняем по завершении миссии
        GoToMap();
    }
    #endregion

    #region Button press
    /// <summary>
    /// Нажали Back.
    /// </summary>
    private void BackPressed()
    {
        // открыть опции
        if (!PanelOption.IsShowed)
        {
            ShowOptionPanel();
            return;
        }
        else
        {
            HideOptionPanel();
            return;
        }
        // выйти
        //LevelEnd();// никогда не выходим, только по кнопке в меню
    }

    public void GoToMap()
    {
        RestoreTimeScale();
        FarLife.GoToMap();
    }
    private bool _isTouchMoving = false;

    private Vector3 _touchPos = Vector3.zero;
    private float _touchMaxY = 80.0f;
    #endregion
    

    private void FixedUpdate()
    {
        var delta = Time.deltaTime;
        // полет (скролим, появляем и прочее)
        if (!_manager.IsPaused && !_ship.IsDied && !_manager.LevelIsEnded)
        {
            _manager.Update(delta);
            if (_manager.LevelIsEnded)
            {
                OnStartLevelWin();
            }
            // панель бонусов
            PanelShip.UpdateTimers();
        }
    }



	void Update ()
    {
        FarLife.Update();
        var delta = Time.deltaTime;


        DisplayHelper.UpdateScreenSacle();

        
        #region touches

        // обработка только если карта
        if (_inTouchControl && !_manager.LevelIsEnded)
        {
            // touch
            if (!_shipPanelClicked && GestHelpers.GetClap(ref _touchPos))
            {
                if (!_inShipPanel)
                {
                    // time speed
                    HideWeaponPanel();
                    // go
                    var sp = DisplayHelper.UnityToScreenCoord(_touchPos);
                    if (sp.y >= _touchMaxY) // попадаем в рамку
                    {
                        var ray = Camera.main.ScreenToWorldPoint(new Vector3(_touchPos.x, _touchPos.y, 10));
                        FlyToRay(ray);
                    }
                }
                //_inShipPanelClick = false;
            }
            else
            {
                // slow down
                ShowWeaponPanel();
            }
            _shipPanelClicked = false;
        }
        #endregion

        BoomAnimDo();

        //// полет (скролим)
        if (!_manager.IsPaused && !_ship.IsDied && !_manager.LevelIsEnded)
        {
            Paralax1.Rotate(_ship.FlySpeed);
            Paralax2.Rotate(_ship.FlySpeed);
        }

        // концовка
        if (_goingToEnd)
        {
            _timeToWndAnim -= delta;
            if (_timeToWndAnim <= 0.0f)
            {
                LevelEnd();
                return;
            }
            Paralax1.Rotate(_ship.FlySpeed);
            Paralax2.Rotate(_ship.FlySpeed);
        }



        //Log.SetText(_manager._shipPosY.ToString()); //?++ !!!
	}


    


    void OnGUI()
    {
        //- рисует движок
        FarLife.OnGUI();
    }


}
