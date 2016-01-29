using System;
using UnityEngine;
using System.Collections;
using ZelderFramework.Helpers;

public class Boss1Gun3Logic : MonoBehaviour
{

    public Boss1Logic Boss;
    public bool IsDied = false;

    public Int32 GunIndex = 0;
    public float MaxHealth = 10.0f;
    public float ResistantAir = 1.0f;       // 1.0 - 0.001
    public float ResistantRocket = 1.0f;    // 1.0 - 0.001

    public float SimpleFireRate = 5.0f;
    public Int32 SimpleFireCount = 3;
    public float SimpleFireStep = 1.0f;



    public float Health { get; private set; }

    public ParticleSystem GunBoomParts;
    public BoxCollider ColliderObj;

    private LevelController _controller;
    private float _cityFactor;

    private bool _isEnabled = false;
    private float _fireRate = 0.0f;
    private Int32 _currFireNum = 0;
    private float _fireNumRate = 0.0f;

    private bool _inFire = false;

    // Use this for initialization
    void Start()
    {
        Health = MaxHealth;
    }

    public void Init(LevelController controller, float cityFactor)
    {
        _controller = controller;
        _cityFactor = cityFactor;


        if (cityFactor >= 2.0f) SimpleFireCount++;
        if (cityFactor >= 3.0f) SimpleFireCount++;
        if (cityFactor >= 4.0f) SimpleFireCount++;
        if (cityFactor >= 5.0f) SimpleFireCount++;


        SetFireRateTime();
    }


    private bool _showing = false;
    private float _showingCurrAngle = 0.0f;
    private float _showingEndAngle = 180.0f;
    private float _showingSpeed = 60.0f;
    private void Showing()
    {
        if (!_showing) return;

        var angle = Time.deltaTime * _showingSpeed;
        _showingCurrAngle += angle;
        if (_showingCurrAngle >= _showingEndAngle)
        {
            _showing = false;
            _isEnabled = true;
            ColliderObj.enabled = true;

            angle = _showingCurrAngle - _showingEndAngle;   //- выравниваем градус
        }
        this.gameObject.transform.Rotate(new Vector3(0, 1, 0), angle);
    }
    /// <summary>
    /// Включение оружия.
    /// </summary>
    public void Show()
    {
        //this.gameObject.transform.position 
        //ColliderObj.gameObject.collider.gameObject.SetActive(); = true;//.SetActive(true);
        if (ColliderObj == null) return;

        _showing = true;
    }




    private void SetFireRateTime()
    {
        _fireRate = SimpleFireRate;//UnityEngine.Random.Range(SimpleFireRate - (SimpleFireRate / 3.0f), SimpleFireRate + (SimpleFireRate / 3.0f));
        _fireRate = _fireRate < 1.0f ? 1.0f : _fireRate;
    }
    private void Firing()
    {
        if (_currFireNum >= SimpleFireCount)
        {
            _inFire = false;
            _currFireNum = 0;
            SetFireRateTime();
            return;
        }

        _fireNumRate += Time.deltaTime;
        if (_fireNumRate >= SimpleFireStep)
        {
            _currFireNum++;
            _fireNumRate = 0.0f;
            _controller.PlaceBulletEnemy(GunIndex, this.transform.position, _cityFactor, _cityFactor / 2);
        }

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
    private void OnBomb(BombLogic bomb)
    {
        if (!bomb.Work || !_isEnabled) return;
        if (!Boss.IsOnBomb(bomb)) return;

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
        if (IsDied || !_isEnabled) return 0.0f;

        var oldHealth = Health;
        Health -= count;
        Health = Health < 0 ? 0 : Health > MaxHealth ? MaxHealth : Health;

        // boom anim
        _controller.PlaceBoomEnemy(0, this.transform.position);

        if (Health <= 0)
        {
            IsDied = true;
            Boss.Gun3Die();
            BoomDestroy();
        }

        return oldHealth - Health; // разница
    }

    private bool _boomAnimation = false;
    private void BoomDestroy()
    {
        // anim boom
        if (GunBoomParts != null)
        {
            GunBoomParts.gameObject.SetActive(true);
            Destroy(GunBoomParts.gameObject, 2.0f);
        }
        Destroy(this.gameObject);
    }

    //private void BoomAnimation()
    //{
    //    if (!_boomAnimation) return;
    //    var rot = 50.0f * Time.deltaTime;
    //    this.transform.Rotate(0, 0, rot);
    //}


    #region Collider
    void OnTriggerEnter(Collider other)
    {
        if (IsDied || !_isEnabled) return;

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
    void Update()
    {
        if (!IsDied)
        {
            Showing();
        }

        if (_isEnabled && !IsDied)
        {
            _fireRate -= Time.deltaTime;
            if (_fireRate <= 0.0f)
            {
                _inFire = true;
                //SetFireRateTime();
                //SimpleFire();
            }
            if (_inFire)
            {
                Firing();
            }
        }
        //BoomAnimation();


    }


}
