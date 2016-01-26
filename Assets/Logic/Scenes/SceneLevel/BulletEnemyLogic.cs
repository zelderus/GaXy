using System;
using System.Linq;
using UnityEngine;
using System.Collections;

/// <summary>
/// Снаряд врага.
/// </summary>
public class BulletEnemyLogic : MonoBehaviour {

    public Boolean Work;

    public float Damage = 1.0f;
    public float Speed = 4.0f;
    public Boolean TypeIsAir = true;
    public Boolean TypeIsRocket = false;

    public Boolean InTarget = false;

    public Boolean WithRotate = false;
    public Transform ModelTransform;
    //public Bullet Bullet { get; private set; }


    private LevelController _controller;
    private bool _inFloated = false;

    private Vector3 _direction = new Vector3(0, -1, 0);

    // Use this for initialization
    void Start()
    {

    }

    private Int32 _rotDir = 1;

    public void Init(LevelController controller, Vector3 pos, float damageFactor, float speedFactor)
    {
        _controller = controller;
        Work = true;

        Damage = Damage*damageFactor;
        Speed = Speed * speedFactor;

        //- вращение
        var r = UnityEngine.Random.Range(0, 10);
        _rotDir = r >= 5 ? 1 : -1;


        //- направление полета
        _direction = new Vector3(0, -1, 0);
        if (InTarget)
        {
            _direction = controller.ShipLogic.gameObject.transform.position - pos;
            _direction.Normalize();
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
    void Update()
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
        //var def = -(Time.deltaTime * Speed);    // вниз
        //var nextY = this.transform.position.y + def;
        //this.transform.position = new Vector3(this.transform.position.x, nextY, 0);

        if (WithRotate)
            ModelTransform.transform.Rotate(0, 0, 500.0f * _rotDir * Time.deltaTime);

        // летим по направлению
        var def = _direction * (Time.deltaTime * Speed);
        this.transform.position += def;
    }
}
