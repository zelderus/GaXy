using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


/// <summary>
/// Данные и способности корабля.
/// </summary>
public class ShipLife
{
    #region Global
    /// <summary>
    /// Врагов всего уничтожено.
    /// </summary>
    public Int32 EnemyDestoyed { get; private set; }
    #endregion

    #region Map
    /// <summary>
    /// Количество шагов за день.
    /// </summary>
    public Int32 MaxSteps { get; private set; }
    /// <summary>
    /// JOP улетел.
    /// </summary>
    public Boolean IsJopCompleted { get; private set; }
    #endregion

    #region Level
    /// <summary>
    /// Здоровье.
    /// </summary>
    public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    /// <summary>
    /// Уничтожен.
    /// </summary>
    public Boolean IsDied { get; private set; }

    ///// <summary>
    ///// Максимальная вертикальная скорость.
    ///// </summary>
    public float FlySpeed { get { return 0.1f; } }

    /// <summary>
    /// Максимальная скорость перемещения.
    /// </summary>
    public float MaxMoveSpeed { get; private set; }
    public float MoveSpeed { get; private set; }

    public List<Bullet> Bullets { get; private set; }

    public float ResistantBodyDamage { get; private set; }  // от 1.0 - 0.001
    public float ResistantAir { get; private set; }         // от 1.0 - 0.001
    public float ResistantRocket { get; private set; }      // от 1.0 - 0.001

    public float LuckyForMaterials { get; private set; }      // от 1.0 - 999.0

    /// <summary>
    /// Покупка здоровья.
    /// </summary>
    public float ShipBonusHealth { get; private set; }      // сам бонус (прибавка или мощность)
    public Int32 ShipBonusHealthCount { get; private set; } // количество бонусов
    public float ShipBonusHealthTime { get; private set; }  // перезарядка бонуса

    /// <summary>
    /// Покупка щита.
    /// </summary>
    public float ShipBonusShield { get; private set; }
    public Int32 ShipBonusShieldCount { get; private set; }
    public float ShipBonusShieldTime { get; private set; }

    /// <summary>
    /// Покупка бомбы.
    /// </summary>
    public float ShipBonusTree { get; private set; }
    public Int32 ShipBonusTreeCount { get; private set; }
    public float ShipBonusTreeTime { get; private set; }
    #endregion


    public ShipLife()
    {
        MaxSteps = 3;
        IsJopCompleted = false;

        MaxHealth = 10.0f;
        IsDied = false;
        MaxMoveSpeed = 10.0f;
        MoveSpeed = MaxMoveSpeed;
        Bullets = new List<Bullet>();
        ResistantBodyDamage = 1.0f;
        ResistantAir = 1.0f;
        ResistantRocket = 1.0f;
        LuckyForMaterials = 1.0f;

        ShipBonusHealth = 2.0f;
        ShipBonusHealthCount = 2;
        ShipBonusHealthTime = 10.0f;

        ShipBonusShield = 3.0f;
        ShipBonusShieldCount = 4;
        ShipBonusShieldTime = 10.0f;

        ShipBonusTree = 30.0f;
        ShipBonusTreeCount = 1;
        ShipBonusTreeTime = 60.0f;
    }


    public void Init()
    {
        // TODO: инициализация оружия
        //- bullets
        Bullets.Add(new Bullet(0, true, 0.9f, 1.0f, 1.0f));

    }


    public void Load()
    {
        // TODO: загрузка из файла

    }


    #region Map logic
    /// <summary>
    /// JOP выполнен.
    /// </summary>
    public void SetJopCompleted()
    {
        IsJopCompleted = true;
    }
    #endregion

    #region Level logic
    /// <summary>
    /// Подготовка перед полетом.
    /// </summary>
    public void PrepareToFly()
    {
        Health = MaxHealth;
        IsDied = false;
        //FlySpeed = MaxFlySpeed;
        MoveSpeed = MaxMoveSpeed;
    }

    /// <summary>
    /// Нанесение/получение урона.
    /// </summary>
    /// <param name="count"></param>
    public float AddHealth(float count)
    {
        if (IsDied) return 0.0f;

        var oldHealth = Health;
        Health += count;
        Health = Health < 0 ? 0 : Health > MaxHealth ? MaxHealth : Health;

        if (Health <= 0)
        {
            IsDied = true;
        }

        return oldHealth - Health; // разница
    }

    /// <summary>
    /// Уничтожили врага.
    /// </summary>
    public void EnemyDestoy()
    {
        EnemyDestoyed++;
    }
    #endregion

    #region Bonuses logic
    /// <summary>
    /// Трата бонуса HP.
    /// </summary>
    public void UseBonusHealth()
    {
        ShipBonusHealthCount--;
    }

    /// <summary>
    /// Трата бонуса Shield.
    /// </summary>
    public void UseBonusShield()
    {
        ShipBonusShieldCount--;
    }

    /// <summary>
    /// Трата бонуса Tree.
    /// </summary>
    public void UseBonusTree()
    {
        ShipBonusTreeCount--;
    }
    #endregion


}
