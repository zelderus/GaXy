using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using ZelderFramework.Helpers;

public class Boss1Logic : MonoBehaviour {

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
    public Int32 GunIndex = 0;

    public float MaxHealth = 10.0f;
    public float Speed = 2.0f;
    public float BodyDamage = 0.1f;
    public float ResistantAir = 1.0f;       // 1.0 - 0.001
    public float ResistantRocket = 1.0f;    // 1.0 - 0.001
    public Int32 MiddleMaterials = 10;
    public float SimpleFireRate = 5.0f;

    public float Health { get; private set; }

    private WaypointModel _currentWaypoint;
    private Int32 _routeIndex = 0;
    private Int32 _currentWayIndex = 0;
    private bool _routeInversed = false;
    private float _inWaypointTime = 0.0f;

    private float _cityFactor = 1.0f;
    private float _fireRate = 0.0f;

    private readonly List<WeightObject> _materialWeight = new List<WeightObject>();
    private bool _disabled = false;

    public Boss1Gun1Logic Gun1;
    public Boss1Gun2Logic Gun2;
    public Boss1Gun3Logic Gun3;

    private bool _isGun1Died = false;
    private bool _isGun2Died = false;
    private bool _isGun3Died = false;



    // Use this for initialization
    void Start()
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


        //+ guns
        Gun1.Init(controller, _cityFactor);
        Gun2.Init(controller, _cityFactor);
        Gun3.Init(controller, _cityFactor);
    }


    private const float MaxFactor = 5.5f;
    private float GetCityFactor(Int32 cityIndex)
    {
        var p = MathHelpers.Percent(0, 16, cityIndex);
        var f = MathHelpers.ByPercent(MaxFactor - 1.0f, p);
        f = f + 1.0f;   // сдвигаем от 0
        return f;
    }



    private void AnimToLose()
    {
        var def = (Time.deltaTime * 2.0f);
        this.transform.position = new Vector3(this.transform.position.x + def, this.transform.position.y, this.transform.position.z);
    }


    #region Routes
    /// <summary>
    /// Следующая точка маршрута для нас.
    /// </summary>
    /// <returns></returns>
    private WaypointModel GetCurrentWaypoint()
    {
        _currentWaypoint = Controller.GetCurrentWaypoint(_routeIndex, _currentWayIndex, BossRouteNum);
        _currentWayIndex++;
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
            var newRotation = Quaternion.LookRotation(transform.position - waypointPos, Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            //x transform.rotation = newRotation;
            transform.rotation = withSlerp ? Quaternion.Slerp(transform.rotation, newRotation, RotationSpeed * Time.deltaTime) : newRotation;
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
            // босс всегда зациклен
            SetStartIndex();
            GetCurrentWaypoint();
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
        if (_inWaypointTime >= _currentWaypoint.TimeToWait / 2)
        {
            _megaFireInStay = true;
            // mega f
            //Controller.PlaceBulletEnemy(GunIndex, this.transform.position, _cityFactor, _cityFactor / 2);
            Gun1.Fire();
        }
    }
    ///// <summary>
    ///// Элементарный выстрел.
    ///// </summary>
    //private void SimpleFire()
    //{
    //    Controller.PlaceBulletEnemy(GunIndex, this.transform.position, _cityFactor, _cityFactor / 2);
    //}

    private void SetFireRateTime()
    {
        _fireRate = UnityEngine.Random.Range(SimpleFireRate - (SimpleFireRate / 3.0f), SimpleFireRate + (SimpleFireRate / 3.0f));
        _fireRate = _fireRate < 1.0f ? 1.0f : _fireRate;
    }

    //private void OnDamage(BulletShipLogic bullet)
    //{
    //    bullet.Work = false;
    //    //+ resistant defense
    //    var damage = bullet.Damage;
    //    if (bullet.TypeIsAir) damage *= ResistantAir;
    //    if (bullet.TypeIsRocket) damage *= ResistantRocket;

    //    AddDamage(damage);

    //    Destroy(bullet.gameObject);
    //}


    // босс доступен для поражения бомбой
    private Int32 _bombBitted = -1;
    public bool IsOnBomb(BombLogic bomb)
    {
        if (!bomb.Work) return false;
        if (bomb.BombId <= _bombBitted) return false;
        _bombBitted = bomb.BombId;
        return true;
    }


    private void OnGunDestoy()
    {
        if (IsDied) return;
        if (_isGun1Died && _isGun2Died && _isGun3Died)
        {
            OnDied();
        }
    }


    public void Gun1Die()
    {
        _isGun1Died = true;
        if (!_isGun2Died) Gun2.Show();
        OnGunDestoy();
    }
    public void Gun2Die()
    {
        _isGun2Died = true;
        if (!_isGun3Died) Gun3.Show();
        OnGunDestoy();
    }
    public void Gun3Die()
    {
        _isGun3Died = true;
        OnGunDestoy();
    }


    private void OnDied()
    {
        if (IsDied) return;
        IsDied = true;

        var matCount = (Int32)MathHelpers.GetByWeight(_materialWeight);
        Controller.PlaceMaterial(this.transform.position, matCount);

        Controller.EnemyDied();

        Destroy(this.gameObject, 5.0f);
    }


    


    ///// <summary>
    ///// Нанесение урона.
    ///// </summary>
    ///// <param name="count"></param>
    //private float AddDamage(float count)
    //{
    //    if (IsDied) return 0.0f;

    //    var oldHealth = Health;
    //    Health -= count;
    //    Health = Health < 0 ? 0 : Health > MaxHealth ? MaxHealth : Health;

    //    if (Health <= 0)
    //    {
    //        IsDied = true;
    //        AnimToLose();
    //        //Controller.PlaceMaterial(this.transform.position, UnityEngine.Random.Range(MinMaterials, MaxMaterials+1));
    //        var matCount = (Int32)MathHelpers.GetByWeight(_materialWeight);
    //        Controller.PlaceMaterial(this.transform.position, matCount);

    //        Controller.EnemyDied();
    //        Destroy(this.gameObject, 3.0f);
    //    }

    //    return oldHealth - Health; // разница
    //}





    // Update is called once per frame
    void Update()
    {
        if (Controller.Manager.IsPaused) return;

        if (IsDied)
        {
            AnimToLose();
        }
        else
        {
            AnimToRoute();
        }
    }
}
