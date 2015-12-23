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

    //public Bullet Bullet { get; private set; }


    private LevelController _controller;
    private bool _inFloated = false;

	// Use this for initialization
	void Start ()
    {
	
	}


    public void Init(LevelController controller, ShipLife ship, Vector3 pos)
    {
        _controller = controller;
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

	// Update is called once per frame
	void Update ()
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
        var nextY = this.transform.position.y + def;
        this.transform.position = new Vector3(this.transform.position.x, nextY, 0);
	}
}
