using System;
using UnityEngine;
using System.Collections;
using ZelderFramework.Helpers;

public class Boss1Gun1Logic : MonoBehaviour
{

    public Boss1Logic Boss;
    public bool IsDied = false;

    public Int32 GunIndex = 0;
    public float MaxHealth = 10.0f;
    public float ResistantAir = 1.0f;       // 1.0 - 0.001
    public float ResistantRocket = 1.0f;    // 1.0 - 0.001
    
    public float Health { get; private set; }

    public ParticleSystem GunBoomParts;

    private LevelController _controller;
    private float _cityFactor;

	// Use this for initialization
	void Start ()
	{
	    Health = MaxHealth;
	}

    public void Init(LevelController controller, float cityFactor)
    {
        _controller = controller;
        _cityFactor = cityFactor;
    }

    /// <summary>
    /// Выстрел.
    /// </summary>
    public void Fire()
    {
        if (IsDied) return;
        _controller.PlaceBulletEnemy(GunIndex, this.transform.position, _cityFactor, _cityFactor / 2);
    }

	
	// Update is called once per frame
	void Update () {
	
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
        if (!bomb.Work) return;
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
        if (IsDied) return 0.0f;

        var oldHealth = Health;
        Health -= count;
        Health = Health < 0 ? 0 : Health > MaxHealth ? MaxHealth : Health;

        // boom anim
        _controller.PlaceBoomEnemy(0, this.transform.position);

        if (Health <= 0)
        {
            IsDied = true;
            Boss.Gun1Die();
            BoomDestroy();
        }

        return oldHealth - Health; // разница
    }


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


    #region Collider
    void OnTriggerEnter(Collider other)
    {
        if (IsDied) return;

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
}
