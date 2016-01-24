using System;
using System.Linq;
using UnityEngine;
using System.Collections;


/// <summary>
/// Снаряд корабля.
/// </summary>
public class BulletShipLogic : MonoBehaviour
{
    public Int32 GunIndex = 1;
    public Boolean Work;
    public float Damage = 1.0f;
    public float Speed = 4.0f;
    public Boolean TypeIsAir = true;
    public Boolean TypeIsRocket = false;

    public Transform ModelTransform;
    public ParticleSystem Particles;

    //public Bullet Bullet { get; private set; }

    private Action _updateDoFn;

    private LevelController _controller;
    private bool _inFloated = false;

    private Vector3 _direction = new Vector3(0, 0.1f, 0);
    private Int32 _tagNum = 0;

    //private Vector3 _lastPos = null;

	// Use this for initialization
	void Start ()
    {
	
	}


    public void Init(LevelController controller, ShipLife ship, Vector3 pos, Vector3 dir, Int32 tagNum)
    {
        _controller = controller;
        _direction = dir;
        _tagNum = tagNum;
        Work = true;
        //Bullet = new Bullet(ship, Damage, Speed, TypeIsAir, TypeIsRocket);
        //+ на основе ship
        var gun = ship.Bullets.FirstOrDefault(f => f.GunIndex == GunIndex);
        if (gun != null)
        {
            Damage *= gun.DamageDif;
            Speed *= gun.SpeedDif;
        }

        //+ у типа снаряда своя логика полета
        _updateDoFn = UpdateFly1Do;
        if (GunIndex == 2)
        {
            _updateDoFn = UpdateFly2Do;
        }
        else if (GunIndex == 3)
        {
            _updateDoFn = UpdateFly3Do;
            //- поворачиваем по направлению движения
            this.transform.rotation = Quaternion.FromToRotation(new Vector3(0, 0.1f, 0), _direction);
        }

        this.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }


    /// <summary>
    /// Снаряд вышел за пределы.
    /// </summary>
    /// <returns></returns>
    public bool IsFloated(Vector3 pos)
    {
        if (_inFloated) return true;
        if (pos.x <= -6.0f) _inFloated = true;
        if (pos.x >= 6.0f) _inFloated = true;
        if (pos.y <= -5.0f) _inFloated = true;
        if (pos.y >= 9.0f) _inFloated = true;
        return _inFloated;
    }
    private bool UpdateFlyMainLogic()
    {
        if (!Work) return false;
        if (_controller.Manager.IsPaused) return false;
        if (IsFloated(this.transform.position))
        {
            Destroy(this.gameObject, 1.0f);
            //x return;
        }
        return true;
    }



    public void OnDestroyByEnemy()
    {
        if (GunIndex < 3)
        {
            Destroy(this.gameObject);
            return;
        }

        // ждем когда кончатся частицы
        if (GunIndex == 3)
        {
            ModelTransform.gameObject.SetActive(false); //- прячем модельку

            //- останавливаем каждую частичку и затухаем 
            ParticleSystem.Particle[] p = new ParticleSystem.Particle[Particles.particleCount + 1];
            int l = Particles.GetParticles(p);
            int i = 0;
            while (i < l)
            {
                p[i].velocity = p[i].velocity / 4;// new Vector3(0, 0, 0);
                i++;
            }
            Particles.SetParticles(p, l);    


            Particles.Stop();
            Destroy(this.gameObject, 2.0f);
        }
    }



    private void UpdateFly1Do()
    {
        if (!UpdateFlyMainLogic()) return;
        // <--- общее для всех снарядов

        // просто летим по прямой со своей скоростью
        var def = (Time.deltaTime * Speed);
        Vector3 n = new Vector3(_direction.x * def, _direction.y * def, 0);
        var nextPos = this.transform.position + n;
        //
        this.transform.position = new Vector3(nextPos.x, nextPos.y, 0);
        // вращаем модельку
        ModelTransform.transform.Rotate(0, 290.0f * Time.deltaTime, 0);
    }


    private void UpdateFly2Do()
    {
        if (!UpdateFlyMainLogic()) return;
        // <--- общее для всех снарядов

        // просто летим по прямой со своей скоростью
        var def = (Time.deltaTime * Speed);
        Vector3 n = new Vector3(_direction.x * def, _direction.y * def, 0);
        var nextPos = this.transform.position + n;
        //
        this.transform.position = new Vector3(nextPos.x, nextPos.y, 0);
        // вращаем модельку
        ModelTransform.transform.Rotate(0, 0, 500.0f * _tagNum * Time.deltaTime);
    }

    private void UpdateFly3Do()
    {
        if (!UpdateFlyMainLogic()) return;
        // <--- общее для всех снарядов

        // просто летим по прямой со своей скоростью
        var def = (Time.deltaTime * Speed);
        Vector3 n = new Vector3(_direction.x * def, _direction.y * def, 0);
        var nextPos = this.transform.position + n;
        //
        //this.transform.LookAt(new Vector3(this.transform.position.x + nextPos.x, this.transform.position.y + nextPos.y, this.transform.position.z));
        this.transform.position = new Vector3(nextPos.x, nextPos.y, this.transform.position.z);
        
    }


    //private void FixedUpdate()
    //{
    //    _updateDoFn();
    //}

    void Update ()
    {
        _updateDoFn();
    }
}
