using System;
using System.Linq;
using UnityEngine;
using System.Collections;


/// <summary>
/// Снаряд корабля.
/// </summary>
public class Bullet2ShipLogic : MonoBehaviour
{
    public Int32 GunIndex = 1;
    public Boolean Work;
    public float Damage = 1.0f;
    public float Speed = 4.0f;
    public Boolean TypeIsAir = true;
    public Boolean TypeIsRocket = false;

    //public Bullet Bullet { get; private set; }


    private LevelController _controller;
    private bool _inFloated = false;

    private Vector3 _direction = new Vector3(0, 0.1f, 0);

    //private Vector3 _lastPos = null;

	// Use this for initialization
	void Start ()
    {
	
	}


    public void Init(LevelController controller, ShipLife ship, Vector3 pos, Vector3 dir)
    {
        _controller = controller;
        _direction = dir;
        Work = true;
        //Bullet = new Bullet(ship, Damage, Speed, TypeIsAir, TypeIsRocket);
        //+ на основе ship
        var gun = ship.Bullets.FirstOrDefault(f => f.GunIndex == GunIndex);
        if (gun != null)
        {
            Damage *= gun.DamageDif;
            Speed *= gun.SpeedDif;
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


    private void UpdateFlyDo()
    {
        if (!Work) return;
        if (_controller.Manager.IsPaused) return;
        if (IsFloated(this.transform.position))
        {
            Destroy(this.gameObject, 1.0f);
            //x return;
        }
        // <--- общее для всех снарядов

        //! своя логика полета
        // просто летим по прямой со своей скоростью
        var def = (Time.deltaTime * Speed);
        //var nextY = this.transform.position.y + def;
        //this.transform.position = new Vector3(this.transform.position.x, nextY, 0);

        Vector3 n = new Vector3(_direction.x * def, _direction.y * def, 0);
        var nextPos = this.transform.position + n;

        this.transform.position = new Vector3(nextPos.x, nextPos.y, 0);
    }


    //private void FixedUpdate()
    //{
    //    UpdateFlyDo();
    //}

    void Update ()
    {
        UpdateFlyDo();
    }
}
