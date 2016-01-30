using System;
using UnityEngine;
using System.Collections;
using ZelderFramework.Helpers;

public class ShipFlyLogic : MonoBehaviour
{



    public ShipLife ShipLife;
    public LevelController Controller { get; private set; }
    public Transform DamageObj;
    public Transform ShieldObj;
    public Transform ShipObj;


    public ParticleSystem PartLeft;
    public ParticleSystem PartRight;
    public ParticleSystem LoseBoom;

    public Transform Gun1Left;
    public Transform Gun1Right;
    public Transform Gun3Left;
    public Transform Gun3Right;
    //public Transform ShipObj;
    //public Transform ColliderObj;

    #region damage
    private float _timeInDamage = 2.0f;
    private bool _inDamage = false;
    private float _timeInDamageDo = 0.0f;
    #endregion


    private float _modelRotX = 0.0f;//270.0f;  //?++ начальное вращение модели
    private float _modelRotY = 0.0f;    //?++ начальное вращение модели
    private float _modelRotZ = 0.0f;

    // Use this for initialization
	void Start ()
	{

        //transform.rotation = Quaternion.FromToRotation(new Vector3(-1, 0, 0), new Vector3(0, 1, 0));

	    //_modelRotX = ShipObj.transform.rotation.x;
	    //_modelRotY = ShipObj.transform.rotation.y;
        //ParticlePause();
	}

    public void Init(ShipLife shipLife, LevelController controller)
    {
        ShipLife = shipLife;
        Controller = controller;

        // gun2
        var gun2 = ShipLife.Bullets.Find(f => f.GunIndex == 2);
        if (gun2 != null && gun2.ShipHave)
        {
            Gun1Left.gameObject.SetActive(true);
            Gun1Right.gameObject.SetActive(true);
        }
        else
        {
            Gun1Left.gameObject.SetActive(false);
            Gun1Right.gameObject.SetActive(false);
        }
        // gun3
        var gun3 = ShipLife.Bullets.Find(f => f.GunIndex == 3);
        if (gun3 != null && gun3.ShipHave)
        {
            Gun3Left.gameObject.SetActive(true);
            Gun3Right.gameObject.SetActive(true);
        }
        else
        {
            Gun3Left.gameObject.SetActive(false);
            Gun3Right.gameObject.SetActive(false);
        }
    }

    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            ParticlePause();
        }
        else
        {
            ParticlePlay();
        }
    }


    #region Fly
    public void SetPosition(float posX, float posY)
    {
        this.transform.position = new Vector3(posX, posY, this.transform.position.z);
    }
    public void SetPosition(Vector2 pos)
    {
        SetPosition(pos.x, pos.y);
    }
    private const float MinX = -3.5f;
    private const float MaxX = 3.5f;
    private const float MinY = -4.0f;//-3.0f;
    private const float MaxY = 6.0f;
    private Vector2 WrapPos(Vector2 pos)
    {
        // есть ли возможность туда
        pos.x = pos.x < MinX ? MinX : pos.x > MaxX ? MaxX : pos.x;
        //pos.y = pos.y < MinY ? MinY : pos.y > MaxY ? MaxY : pos.y;

        return pos;
    }
    private bool _isFlying = false;
    public float Fly(Vector3 pos)
    {
        var oldPos = new Vector2(transform.position.x, transform.position.y); // new Vector2(PositionX, PositionY);
        //- новая возможная позиция
        var goPos = new Vector2(pos.x, pos.y);
        goPos = WrapPos(goPos);
        //- крен
        Cren(goPos.x - oldPos.x);
        //- смещение
        float step = ShipLife.MoveSpeed * Time.deltaTime;
        //- смещаем от старой к новой на дистанцию
        var toPos = Vector2.MoveTowards(oldPos, goPos, step);
        SetPosition(toPos.x, toPos.y);
        _isFlying = true;
        //- отдаем реальную разницу в сдвиге
        return toPos.x - oldPos.x;
    }

    private float _crenX = 0.0f;
    private void Cren(float offsetX)
    {
        _crenX = offsetX;
    }

    private float _oldCrenY = 0.0f;
    private void UpdateCren()
    {
        if (!_isFlying) return;

        const float maxCren = 60.0f;// 40.0f;
        const float minCren = 0.01f;

        var rot = _crenX > minCren ? maxCren : _crenX < -minCren ? -maxCren : 0.0f;
        rot = Mathf.Lerp(_oldCrenY, rot, Time.deltaTime * 8.0f);
        
        _oldCrenY = rot;
        //ShipObj.transform.rotation = Quaternion.Euler(_modelRotX, _modelRotY, -rot);
        //ColliderObj.transform.rotation = Quaternion.Euler(_modelRotX, _modelRotY, -rot);
        this.transform.rotation = Quaternion.Euler(_modelRotX, -rot, _modelRotZ);
    }
    #endregion

    #region HP/Damage
    private void OnDamageByBullet(BulletEnemyLogic bullet)
    {
        if (!bullet.Work) return;
        if (_inDamage || _shieldEnabled) return;
        bullet.Work = false;
        // ship defense
        var damage = bullet.Damage;
        if (bullet.TypeIsAir) damage *= ShipLife.ResistantAir;
        if (bullet.TypeIsRocket) damage *= ShipLife.ResistantRocket;
        Controller.AddDamage(damage);

        Destroy(bullet.gameObject);
    }
    private void OnDamageByBody(float damage)
    {
        if (_inDamage || _shieldEnabled) return;
        damage *= ShipLife.ResistantBodyDamage;
        Controller.AddDamage(damage);
    }
    /// <summary>
    /// Вызывается из контроллера.
    /// </summary>
    /// <param name="differenceHealth"></param>
    public void Damage(float differenceHealth)
    {
        if (_inDamage) return;
        _inDamage = true;
        EnableDamageObj();
        AnimHealthAdd(differenceHealth);
        _timeInDamageDo = _timeInDamage;
    }
    /// <summary>
    /// Вызывается из контроллера.
    /// </summary>
    /// <param name="differenceHealth"></param>
    public void AddHealth(float differenceHealth)
    {
        AnimHealthAdd(differenceHealth);
    }



    private bool _dmgActive = false;
    private void DisableDamageObj()
    {
        DamageObj.gameObject.SetActive(false);
        _dmgActive = false;
    }
    private void EnableDamageObj()
    {
        DamageObj.gameObject.SetActive(true);
        _dmgActive = true;
        _dmgTimerDo = 0.0f;
    }
    #endregion
    #region Shield

    private bool _shieldEnabled = false;
    private float _shieldTimer = 0.0f;
    /// <summary>
    /// Включение щита.
    /// </summary>
    /// <param name="time"></param>
    public void EnableShield(float time)
    {
        _shieldTimer += time;
        EnableShieldObj();
    }
    private void DisableShieldObj()
    {
        ShieldObj.gameObject.SetActive(false);
        _shieldEnabled = false;
        _shieldTimer = 0.0f;
    }
    private void EnableShieldObj()
    {
        ShieldObj.gameObject.SetActive(true);
        _shieldEnabled = true;
    }
    #endregion


    #region anims
    private void AnimHealthAdd(float differenceHealth)
    {
        // TODO: анимация прихода/убыли энергии

    }

    private float _winPosX = 0.0f;
    private float _winPosY = 20.0f;
    private bool _winAnimGo = false;

    public void AnimToWin()
    {
        DisableDamageObj();
        // полет к концу
        _winAnimGo = true;
    }
    private void AnimWinDo()
    {
        if (!_winAnimGo) return;
        Fly(new Vector3(_winPosX, _winPosY, 0));
    }

    public void AnimToLose()
    {
        DisableDamageObj();
        ParticleStop();
        // анимация взрыва
        ShipObj.gameObject.SetActive(false);
        LoseBoom.gameObject.SetActive(true);

        Gun1Left.gameObject.SetActive(false);
        Gun1Right.gameObject.SetActive(false);
        Gun3Left.gameObject.SetActive(false);
        Gun3Right.gameObject.SetActive(false);
    }

    private float _dmgTimer = 0.5f;
    private float _dmgTimerDo = 0.0f;
    private void AnimInDamage()
    {
        //_timeInDamageDo

        //+ моргаем
        //_dmgTimerDo += Time.deltaTime;
        //if (_dmgTimerDo >= _dmgTimer)
        //{
        //    _dmgTimerDo = 0.0f;
        //    if (_dmgActive) DisableDamageObj();
        //    else EnableDamageObj();
        //}

    }
    #endregion
    #region particles

    private void ParticlePlay()
    {
        PartLeft.Play();
        PartRight.Play();
    }
    private void ParticlePause()
    {
        PartLeft.Pause();
        PartRight.Pause();
    }
    private void ParticleStop()
    {
        PartLeft.Stop();
        PartRight.Stop();
    }
    #endregion

    #region Collider

    private void OnForeverCollide(Collider other)
    {
        //! враги
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<EnemyLogic>();
            if (enemy == null || enemy.IsDied) return;
            var damage = enemy.BodyDamage;
            OnDamageByBody(damage);
            return;
        }
        //! снаряды
        if (other.gameObject.tag == "BulletEnemy")
        {
            var bullet = other.gameObject.GetComponent<BulletEnemyLogic>();
            if (bullet == null) return;
            if (!bullet.Work) return;
            OnDamageByBullet(bullet);
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //! ресурсы
        if (other.gameObject.tag == "ResMaterial")
        {
            var mat = other.gameObject.GetComponent<MaterialLogic>();

            if (!mat.IsFloated)
            {
                this.Controller.AddMaterial(mat.Count);
                mat.IsFloated = true;
                this.Controller.PlaceMaterialFloat(mat.transform.position, mat.Count, mat.Speed);

                Destroy(mat.gameObject);
            }
            return;
        }
        //! снаряды и враги
        OnForeverCollide(other);
    }

    void OnTriggerStay(Collider other)
    {
        //! снаряды и враги
        OnForeverCollide(other);
    }

    void OnTriggerExit(Collider other)
    {

    }
    #endregion

    #region Bullets

    private void BulletsLaunchUpdate(float delta)
    {

        foreach (var bullet in ShipLife.Bullets)
        {
            if (!bullet.ShipHave) continue;
            bullet.TimeToGoWorking += delta;
            if (bullet.TimeToGoWorking >= bullet.TimeToGo)
            {
                bullet.TimeToGoWorking = 0.0f;
                Controller.PlaceBulletShip(bullet.GunIndex);
            }
        }

    }
    #endregion


    // Update is called once per frame
	void Update ()
    {
        if (Controller.Manager.IsPaused) return;
        if (ShipLife.IsDied) return;

	    UpdateCren();
	    if (_inDamage)
	    {
	        AnimInDamage();
	        _timeInDamageDo -= Time.deltaTime;
	        if (_timeInDamageDo <= 0.0f)
	        {
	            _inDamage = false;
	            DisableDamageObj();
	        }
	    }
        //- запуск своих снарядов
	    BulletsLaunchUpdate(Time.deltaTime);

        //- защита
	    if (_shieldEnabled)
	    {
	        _shieldTimer -= Time.deltaTime;
	        if (_shieldTimer <= 0.0f)
	        {
	            DisableShieldObj();
	        }
	    }
        _isFlying = false;

        // anim
        AnimWinDo();
    }


}
