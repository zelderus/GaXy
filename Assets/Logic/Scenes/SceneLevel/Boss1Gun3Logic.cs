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


    /// <summary>
    /// Включение оружия.
    /// </summary>
    public void Show()
    {
        if (ColliderObj == null) return;
        //this.gameObject.transform.position 
        ColliderObj.enabled = true;
        _isEnabled = true;

        var zo = -0.5f;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z + zo);
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
        _boomAnimation = true;
        Destroy(this.gameObject, 3.0f);
    }

    private void BoomAnimation()
    {
        if (!_boomAnimation) return;
        // TODO: float
        var rot = 50.0f * Time.deltaTime;
        this.transform.Rotate(0, 0, rot);
    }


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
        BoomAnimation();


    }


}
