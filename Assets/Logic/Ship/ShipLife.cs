using System;
using System.Collections.Generic;
using Assets.Scripts.ZelderFramework.FileSystem;
using UnityEngine;
using System.Collections;


/// <summary>
/// Данные и способности корабля.
/// </summary>
public class ShipLife : FileManagedClass
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
        // map
        MaxSteps = 3;
        IsJopCompleted = false;
        // level
        MaxHealth = 10.0f;
        IsDied = false;
        MaxMoveSpeed = 10.0f;
        MoveSpeed = MaxMoveSpeed;
        ResistantBodyDamage = 1.0f;
        ResistantAir = 1.0f;
        ResistantRocket = 1.0f;
        LuckyForMaterials = 1.0f;
        // bullets
        Bullets = new List<Bullet>();
        // bonus
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



    /// <summary>
    /// Данные для сохранения.
    /// </summary>
    /// <returns></returns>
    public override List<FileManagerData> ConvertToSaveData()
    {
        // TODO: сохранение
        var datas = new List<FileManagerData>();
        datas.Add(new FileManagerData(FileManagerTypes.Single, UnityEngine.Random.Range(53.5f, 745.9999f)));
        datas.Add(new FileManagerData(FileManagerTypes.String, StringHelper.GetRandomString(3, 7)));   //+ rnd
        datas.Add(new FileManagerData(FileManagerTypes.Int32, UnityEngine.Random.Range(33, 444)));
        // map
        datas.Add(new FileManagerData(FileManagerTypes.Int32, MaxSteps));
        datas.Add(new FileManagerData(FileManagerTypes.Boolean, IsJopCompleted));
        // level
        datas.Add(new FileManagerData(FileManagerTypes.Single, MaxHealth));
        datas.Add(new FileManagerData(FileManagerTypes.Single, MaxMoveSpeed));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ResistantBodyDamage));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ResistantAir));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ResistantRocket));
        datas.Add(new FileManagerData(FileManagerTypes.Single, LuckyForMaterials));
        // bullets
        foreach (var bullet in Bullets)
        {
            datas.Add(new FileManagerData(FileManagerTypes.Boolean, bullet.ShipHave));
            datas.Add(new FileManagerData(FileManagerTypes.Single, bullet.TimeToGo));
            datas.Add(new FileManagerData(FileManagerTypes.Single, bullet.DamageDif));
            datas.Add(new FileManagerData(FileManagerTypes.Single, bullet.SpeedDif));
        }
        // bonus
        datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusHealth));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, ShipBonusHealthCount));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusHealthTime));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusShield));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, ShipBonusShieldCount));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusShieldTime));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusTree));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, ShipBonusTreeCount));
        datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusTreeTime));

        datas.Add(new FileManagerData(FileManagerTypes.Int32, UnityEngine.Random.Range(1, 10)));
        return datas;
    }
    /// <summary>
    /// Загрузка данных.
    /// </summary>
    /// <param name="datas"></param>
    public override void LoadFromSaveData(List<FileManagerData> datas)
    {
        var ind = 0;
        // TODO: загрузка
        var rnd1 = (Single)datas[ind++].DataValue;   //+ rnd
        var rndStr = (String)datas[ind++].DataValue.ToString();   //+ rnd text
        var rnd2 = (Int32)datas[ind++].DataValue;   //+ rnd
        
        // map
        MaxSteps = (Int32)datas[ind++].DataValue;
        IsJopCompleted = (Boolean)datas[ind++].DataValue;
        // level
        MaxHealth = (Single)datas[ind++].DataValue;
        MaxMoveSpeed = (Single)datas[ind++].DataValue;
        ResistantBodyDamage = (Single)datas[ind++].DataValue;
        ResistantAir = (Single)datas[ind++].DataValue;
        ResistantRocket = (Single)datas[ind++].DataValue;
        LuckyForMaterials = (Single)datas[ind++].DataValue;
        // bullets
        foreach (var bullet in Bullets)
        {
            var b1 = (Boolean) datas[ind++].DataValue;
            var b2 = (Single)datas[ind++].DataValue;
            var b3 = (Single)datas[ind++].DataValue;
            var b4 = (Single)datas[ind++].DataValue;
            bullet.LoadData(b1, b2, b3, b4);
        }
        // bonus
        ShipBonusHealth = (Single)datas[ind++].DataValue;
        ShipBonusHealthCount = (Int32)datas[ind++].DataValue;
        ShipBonusHealthTime = (Single)datas[ind++].DataValue;
        ShipBonusShield = (Single)datas[ind++].DataValue;
        ShipBonusShieldCount = (Int32)datas[ind++].DataValue;
        ShipBonusShieldTime = (Single)datas[ind++].DataValue;
        ShipBonusTree = (Single)datas[ind++].DataValue;
        ShipBonusTreeCount = (Int32)datas[ind++].DataValue;
        ShipBonusTreeTime = (Single)datas[ind++].DataValue;

        var rnd3 = (Int32)datas[ind++].DataValue;   //+ rnd
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
