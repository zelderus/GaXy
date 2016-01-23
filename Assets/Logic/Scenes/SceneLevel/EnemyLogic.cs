using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using ZelderFramework.Helpers;

public class EnemyLogic : MonoBehaviour {


    public LevelController Controller { get; private set; }
    /// <summary>
    /// Уничтожен.
    /// </summary>
    public Boolean IsDied { get; private set; }
    /// <summary>
    /// Тип маршрута.
    /// <remarks>
    ///     <para>0 - простой, для улетающих с карты после маршрута</para>
    ///     <para>1 - аля вертолет, зацикленный на маршруте, всегда смотрящий вниз</para>
    ///     <para>2 - аля вертолет, зацикленный на маршруте, всегда смотрящий вниз имеющий точки останова</para>
    /// </remarks>
    /// </summary>
    public Int32 BossRouteNum = 0;
    /// <summary>
    /// Количество циклов повторения прохождения маршрута.
    /// </summary>
    public Int32 RouteCicles = 1;
    public Int32 GunIndex = 0;

    public float MaxHealth = 10.0f;
    public float Speed = 2.0f;
    public float BodyDamage = 0.1f;
    public float ResistantAir = 1.0f;       // 1.0 - 0.001
    public float ResistantRocket = 1.0f;    // 1.0 - 0.001
    public Int32 MiddleMaterials = 10;
    public float SimpleFireRate = 5.0f;
    
    public float Health { get; private set; }


    public Transform Explosion;
    public Transform ShipModel;


    
    private WaypointModel _currentWaypoint;
    private Int32 _routeIndex = 0;
    private Int32 _currentWayIndex = 0;
    private bool _routeInversed = false;
    private float _inWaypointTime = 0.0f;

    private float _cityFactor = 1.0f;
    private float _fireRate = 0.0f;

    private readonly List<WeightObject> _materialWeight = new List<WeightObject>();

    private bool _disabled = false;
    private bool _isLastWaypoint = false;

	// Use this for initialization
	void Start ()
    {
	
	}

    public void Init(LevelController controller, Int32 routeIndex, bool routeInversed, Int32 cityIndex)
    {
        Controller = controller;
        _routeIndex = routeIndex;
        _routeInversed = routeInversed;
        var ship = controller.ShipLogic.ShipLife;

        //+ by ship, cityIndex: 0-16
        _cityFactor = GetCityFactor(cityIndex);

        Health = MaxHealth * _cityFactor;
        BodyDamage = BodyDamage * _cityFactor;
        ResistantAir = ResistantAir * (1 / _cityFactor);
        ResistantRocket = ResistantRocket * (1 / _cityFactor);

        //- materials
        if (BossRouteNum <= 1) _materialWeight.Add(new WeightObject(0, 8.0f));  //! вероятность НЕ выпадания
        _materialWeight.Add(new WeightObject(MiddleMaterials, BossRouteNum == 0 ? 3.0f : 1.0f * ship.LuckyForMaterials));

        

        // инициализация маршрута и времени удаления
        GetCurrentWaypoint();
        //+ look at next
        //- pos
        transform.position = _currentWaypoint.Position(routeInversed);//.Waypoint.position;
        //- rot
        var nextWaypoint = Controller.GetCurrentWaypoint(_routeIndex, 1, BossRouteNum);
        if (!RotateByWaypoint(nextWaypoint, false))
        {
            //- look just to down
            var newRotation = Quaternion.LookRotation(transform.position - (transform.position + new Vector3(0, -1, 0)), Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            transform.rotation = newRotation;
        }

        //+ simple fire
        SetFireRateTime();
    }


    private const float MaxFactor = 5.5f;
    private float GetCityFactor(Int32 cityIndex)
    {
        var p = MathHelpers.Percent(0, 16, cityIndex);
        var f = MathHelpers.ByPercent(MaxFactor-1.0f, p);
        f = f + 1.0f;   // сдвигаем от 0
        return f;
    }


    
    private void AnimToLose()
    {
        if (Explosion != null)
        {
            Explosion.gameObject.SetActive(true);
        }
        if (ShipModel != null)
        {
            ShipModel.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        // rot
        var rot = 50.0f * Time.deltaTime;
        this.transform.Rotate(0, 0, rot);

        var def = (Time.deltaTime * 2.0f);
        var nextZ = this.transform.position.z + def;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, nextZ);

        //var def = (Time.deltaTime * Speed*0.8f);
        //var nextY = this.transform.position.y - def;
        //this.transform.position = new Vector3(this.transform.position.x, nextY, this.transform.position.z);
    }


    #region Routes
    /// <summary>
    /// Следующая точка маршрута для нас.
    /// </summary>
    /// <returns></returns>
    private WaypointModel GetCurrentWaypoint()
    {
        if (_isLastWaypoint)
        {
            _currentWaypoint = null;
            return null;
        }
        _currentWaypoint = Controller.GetCurrentWaypoint(_routeIndex, _currentWayIndex, BossRouteNum);
        _currentWayIndex++;
        _inWaypointTime = 0.0f;
        return _currentWaypoint;
    }
    private WaypointModel GetHomeWaypoint()
    {
        _currentWaypoint = Controller.GetCurrentWaypoint(_routeIndex, 0, BossRouteNum);
        _currentWayIndex = 0;
        _inWaypointTime = 0.0f;
        return _currentWaypoint;
    }

    private void SetStartIndex()
    {
        _currentWayIndex = Controller.GetRouteStartIndex(_routeIndex, BossRouteNum);
    }


    /// <summary>
    /// Вращаем в сторону точки.
    /// </summary>
    /// <returns></returns>
    private bool RotateByWaypoint(WaypointModel waypoint, bool withSlerp = true)
    {
        return RotateByWaypoint(waypoint, waypoint.Position(_routeInversed), withSlerp);
    }
    private bool RotateByWaypoint(WaypointModel waypoint, Vector3 waypointPos, bool withSlerp = true)
    {
        if (!waypoint.WithoutRotate)
        {
            //var newRotation = Quaternion.LookRotation(transform.position - waypointPos, Vector3.forward);
            var lr = transform.position - waypointPos;
            if (lr != Vector3.zero)
            {
                var newRotation = Quaternion.LookRotation(lr, Vector3.forward);
                newRotation.x = 0.0f;
                newRotation.y = 0.0f;
                //x transform.rotation = newRotation;
                transform.rotation = withSlerp ? Quaternion.Slerp(transform.rotation, newRotation, RotationSpeed*Time.deltaTime) : newRotation;
            }
            return true;
        }
        return false;
    }

    private const float RotationSpeed = 5.0f;
    private const float StayTimer = 0.5f;
    private float _stayTimerDo = 99.0f;
    private Vector3 _stayPosOffset = Vector3.zero;
    private bool _inStay = false;
    private bool _megaFireInStay = false;

    private void AnimToRoute()
    {
        if (_disabled) return;
        const float minDist = 0.3f;

        if (_currentWaypoint != null)
        {
            var currentWaypointPos = _currentWaypoint.Position(_routeInversed);
            var distanceToCurrentWaypoint = Vector3.Distance(currentWaypointPos, transform.position);
            if (!_inStay && distanceToCurrentWaypoint > minDist)
            {
                _megaFireInStay = false;
                // pos
                transform.position = Vector3.MoveTowards(transform.position, currentWaypointPos, Speed * Time.deltaTime);
                // rot
                RotateByWaypoint(_currentWaypoint, currentWaypointPos, true);
            }
            else if (_inStay || distanceToCurrentWaypoint <= minDist)
            {
                //_inStay = true;
                if (_currentWaypoint.HasTime)   // задержка в позиции
                {
                    _inStay = true;
                    // pos
                    _stayTimerDo += Time.deltaTime;
                    if (_stayTimerDo >= StayTimer)
                    {
                        _stayTimerDo = 0.0f;
                        var omf = 0.3f;
                        _stayPosOffset = new Vector3(UnityEngine.Random.Range(-omf, omf), UnityEngine.Random.Range(-omf, omf), 0);
                    }
                    var osp = 0.2f;
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypointPos + _stayPosOffset, osp * Time.deltaTime);

                    // wait
                    _inWaypointTime += Time.deltaTime;
                    if (_inWaypointTime >= _currentWaypoint.TimeToWait)
                    {
                        GetCurrentWaypoint();
                        _inStay = false;
                    }
                    // mega fire
                    MegaFireInStay();
                }
                else
                {
                    GetCurrentWaypoint();
                    _inStay = false;
                }
            }
        }
        else
        {
            if (BossRouteNum > 0) // если не самый простой тип врага (улетающий)
            {
                if (RouteCicles > 0)
                {
                    RouteCicles--;
                    SetStartIndex();
                    GetCurrentWaypoint();
                }
                else if (RouteCicles == 0)
                {
                    RouteCicles--;
                    GetHomeWaypoint();
                    _isLastWaypoint = true;
                }
                else if (RouteCicles < 0)
                {
                    if (!IsDied)
                    {
                        _disabled = true;
                        Controller.EnemyFloated();
                        Destroy(this.gameObject, 2.0f);
                    }
                }
            }
            else
            {
                if (!IsDied)
                {
                    _disabled = true;
                    Controller.EnemyFloated();
                    Destroy(this.gameObject, 2.0f);
                }
            }
        }

    }
    #endregion
    
    /// <summary>
    /// Выстрел при остановке на маршруте.
    /// </summary>
    private void MegaFireInStay()
    {
        if (!_inStay) return;
        if (_megaFireInStay) return;
        if (_inWaypointTime >= _currentWaypoint.TimeToWait/2)
        {
            _megaFireInStay = true;
            // mega f
            Controller.PlaceBulletEnemy(GunIndex, this.transform.position, _cityFactor, _cityFactor/2);
        }
    }
    /// <summary>
    /// Элементарный выстрел.
    /// </summary>
    private void SimpleFire()
    {
        Controller.PlaceBulletEnemy(GunIndex, this.transform.position, _cityFactor, _cityFactor / 2);
    }

    private void SetFireRateTime()
    {
        _fireRate = UnityEngine.Random.Range(SimpleFireRate - (SimpleFireRate / 3.0f), SimpleFireRate + (SimpleFireRate / 3.0f));
        _fireRate = _fireRate < 1.0f ? 1.0f : _fireRate;
    }

    private void OnDamage(BulletShipLogic bullet)
    {
        bullet.Work = false;
        //+ resistant defense
        var damage = bullet.Damage;
        if (bullet.TypeIsAir) damage *= ResistantAir;
        if (bullet.TypeIsRocket) damage *= ResistantRocket;

        AddDamage(damage);

        Destroy(bullet.gameObject);
    }

    private Int32 _bombBitted = -1;
    private void OnBomb(BombLogic bomb)
    {
        if (!bomb.Work) return;
        if (bomb.BombId <= _bombBitted) return;
        _bombBitted = bomb.BombId;

        //+ resistant defense
        var damage = bomb.Damage;

        AddDamage(damage);
    }

    /// <summary>
    /// Нанесение урона.
    /// </summary>
    /// <param name="count"></param>
    private float AddDamage(float count)
    {
        if (IsDied) return 0.0f;

        var oldHealth = Health;
        Health -= count;
        Health = Health < 0 ? 0 : Health > MaxHealth ? MaxHealth : Health;

        // boom anim
        Controller.PlaceBoomEnemy(0, this.transform.position);

        if (Health <= 0)
        {
            IsDied = true;

            if (!_disabled)
            {
                AnimToLose();
                //Controller.PlaceMaterial(this.transform.position, UnityEngine.Random.Range(MinMaterials, MaxMaterials+1));
                var matCount = (Int32) MathHelpers.GetByWeight(_materialWeight);
                Controller.PlaceMaterial(this.transform.position, matCount);

                Controller.EnemyDied();
                Destroy(this.gameObject, 1.9f); // взрыв должен уложиться в 2 сек
            }
        }
        
        return oldHealth - Health; // разница
    }


    #region Collider
    void OnTriggerEnter(Collider other)
    {
        if (IsDied) return;

        //Physics.Linecast(previousPoint, currentPoint)
        if (this.transform.position.x <= -4.0f || this.transform.position.x >= 4.0f) return;    //+ слишком за пределами экрана, не реагируем на попадания

        //! снаряды
        if (other.gameObject.tag == "BulletShip")
        {
            //Debug.Log("Enemy boom");
            var bullet = other.gameObject.GetComponent<BulletShipLogic>();
            if (!bullet.Work) return;
            OnDamage(bullet);
            return;
        }
        //! бомба
        if (other.gameObject.tag == "Bomb")
        {
            var bomb = other.gameObject.GetComponent<BombLogic>();
            if (!bomb.Work) return;
            OnBomb(bomb);
            return;
        }
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {

    }
    #endregion


	// Update is called once per frame
	void Update ()
    {
        if (Controller.Manager.IsPaused) return;

	    if (IsDied)
	    {
            AnimToRoute();
	        AnimToLose();
	    }
	    else
	    {
	        AnimToRoute();
            // только Pen так делает, у Босса свое оружие и логика
	        if (BossRouteNum == 1)
	        {
	            if (!_inStay) _fireRate -= Time.deltaTime;
	            if (_fireRate <= 0.0f)
	            {
	                SetFireRateTime();
	                SimpleFire();
	            }
	        }
	    }
	}


}
