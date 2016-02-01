using System;
using System.Collections.Generic;
using ZelderFramework.FileSystem;
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
    public Int32 ShipBonusHealthCost { get; private set; }  // цена материалов

    /// <summary>
    /// Покупка щита.
    /// </summary>
    public float ShipBonusShield { get; private set; }
    public Int32 ShipBonusShieldCount { get; private set; }
    public float ShipBonusShieldTime { get; private set; }
    public Int32 ShipBonusShieldCost { get; private set; }

    /// <summary>
    /// Покупка бомбы.
    /// </summary>
    public float ShipBonusTree { get; private set; }
    public Int32 ShipBonusTreeCount { get; private set; }
    public float ShipBonusTreeTime { get; private set; }
    public Int32 ShipBonusTreeCost { get; private set; }
    #endregion

    #region Skills
    public List<FarSkill> Skills { get; private set; }
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
        ShipBonusHealthTime = 20.0f;
        ShipBonusHealthCost = 100;

        ShipBonusShield = 3.0f;
        ShipBonusShieldCount = 4;
        ShipBonusShieldTime = 40.0f;
        ShipBonusShieldCost = 300;

        ShipBonusTree = 10.0f;
        ShipBonusTreeCount = 1;
        ShipBonusTreeTime = 60.0f;
        ShipBonusTreeCost = 1500;
        // skills
        Skills = new List<FarSkill>();
    }


    public void Init()
    {
        //! инициализация
        //- bullets
        Bullets.Add(new Bullet(1, true, 1.0f));
        Bullets.Add(new Bullet(2, false, 1.0f));
        Bullets.Add(new Bullet(3, false, 1.0f));

        // normal: 287DA6FF
        // disabled: 879AA5FF

        // skill nums
        /*
            G1: 1-10
            G2: 11-20
            G3: 21-30

            //: 51-55
            <>: 56-60
            X>: 61-65

            HP: 101-105
            SH: 106-110
            BB: 111-115
        */
        //- skills
        Skills.Add(new FarSkill(1, true, FarSkillTypes.Gun1));
        Skills.Add(new FarSkill(2, FarSkillTypes.Gun1, 200, 0, 0, 0, 0));

        Skills.Add(new FarSkill(3, FarSkillTypes.Gun1, 500, 5, 0, 0, 0));
        Skills.Add(new FarSkill(4, FarSkillTypes.Gun1, 300, 50, 10, 0, 0));
        Skills.Add(new FarSkill(5, FarSkillTypes.Gun1, 300, 60, 50, 1, 0));
        Skills.Add(new FarSkill(6, FarSkillTypes.Gun1, 300, 80, 60, 10, 0));
        Skills.Add(new FarSkill(7, FarSkillTypes.Gun1, 300, 20, 20, 30, 0));
        Skills.Add(new FarSkill(8, FarSkillTypes.Gun1, 300, 60, 30, 10, 1));
        Skills.Add(new FarSkill(9, FarSkillTypes.Gun1, 300, 70, 50, 20, 10));
        Skills.Add(new FarSkill(10, FarSkillTypes.Gun1, 300, 100, 60, 50, 30));

        Skills.Add(new FarSkill(11, FarSkillTypes.Gun2, 500, 10, 0, 0, 0));        // line 2
        Skills.Add(new FarSkill(12, FarSkillTypes.Gun2, 300, 30, 5, 5, 0));         // line 2
        Skills.Add(new FarSkill(13, FarSkillTypes.Gun2, 300, 40, 10, 0, 0));
        Skills.Add(new FarSkill(14, FarSkillTypes.Gun2, 300, 50, 30, 10, 0));
        Skills.Add(new FarSkill(15, FarSkillTypes.Gun2, 300, 60, 50, 3, 1));
        Skills.Add(new FarSkill(16, FarSkillTypes.Gun2, 300, 80, 60, 10, 0));
        Skills.Add(new FarSkill(17, FarSkillTypes.Gun2, 300, 20, 20, 30, 0));
        Skills.Add(new FarSkill(18, FarSkillTypes.Gun2, 300, 60, 30, 10, 1));
        Skills.Add(new FarSkill(19, FarSkillTypes.Gun2, 300, 70, 50, 20, 10));
        Skills.Add(new FarSkill(20, FarSkillTypes.Gun2, 300, 100, 60, 50, 30));

        Skills.Add(new FarSkill(21, FarSkillTypes.Gun3, 500, 10, 10, 0, 0));        // line 2
        Skills.Add(new FarSkill(22, FarSkillTypes.Gun3, 300, 30, 5, 5, 0));         // line 2
        Skills.Add(new FarSkill(23, FarSkillTypes.Gun3, 300, 40, 10, 0, 0));
        Skills.Add(new FarSkill(24, FarSkillTypes.Gun3, 300, 50, 30, 10, 0));
        Skills.Add(new FarSkill(25, FarSkillTypes.Gun3, 300, 60, 50, 3, 1));
        Skills.Add(new FarSkill(26, FarSkillTypes.Gun3, 300, 80, 60, 10, 0));
        Skills.Add(new FarSkill(27, FarSkillTypes.Gun3, 300, 20, 20, 30, 0));
        Skills.Add(new FarSkill(28, FarSkillTypes.Gun3, 300, 60, 30, 10, 1));
        Skills.Add(new FarSkill(29, FarSkillTypes.Gun3, 300, 70, 50, 20, 10));
        Skills.Add(new FarSkill(30, FarSkillTypes.Gun3, 300, 100, 60, 50, 30));


        Skills.Add(new FarSkill(51, FarSkillTypes.Block, 300, 10, 0, 0, 0));
        Skills.Add(new FarSkill(52, FarSkillTypes.Block, 300, 10, 0, 0, 0));
        Skills.Add(new FarSkill(53, FarSkillTypes.Block, 300, 20, 5, 0, 0));
        Skills.Add(new FarSkill(54, FarSkillTypes.Block, 300, 20, 10, 4, 0));
        Skills.Add(new FarSkill(55, FarSkillTypes.Block, 300, 50, 30, 10, 5));
        Skills.Add(new FarSkill(56, FarSkillTypes.Speed, 300, 10, 0, 0, 0));
        Skills.Add(new FarSkill(57, FarSkillTypes.Speed, 300, 10, 0, 0, 0));
        Skills.Add(new FarSkill(58, FarSkillTypes.Speed, 300, 20, 5, 0, 0));
        Skills.Add(new FarSkill(59, FarSkillTypes.Speed, 300, 20, 10, 4, 0));
        Skills.Add(new FarSkill(60, FarSkillTypes.Speed, 300, 50, 30, 10, 5));
        Skills.Add(new FarSkill(61, FarSkillTypes.Parsek, 300, 10, 0, 0, 0));              // line 2
        Skills.Add(new FarSkill(62, FarSkillTypes.Parsek, 300, 10, 0, 0, 0));              // line 2
        Skills.Add(new FarSkill(63, FarSkillTypes.Parsek, 300, 20, 5, 0, 0));
        Skills.Add(new FarSkill(64, FarSkillTypes.Parsek, 300, 20, 10, 3, 0));
        Skills.Add(new FarSkill(65, FarSkillTypes.Parsek, 300, 50, 30, 10, 2));

        Skills.Add(new FarSkill(101, FarSkillTypes.BonusHealth, 300, 1, 0, 0, 0));        // line 2
        Skills.Add(new FarSkill(102, FarSkillTypes.BonusHealth, 300, 10, 0, 0, 0));
        Skills.Add(new FarSkill(103, FarSkillTypes.BonusHealth, 300, 30, 20, 5, 0));
        Skills.Add(new FarSkill(104, FarSkillTypes.BonusHealth, 300, 50, 40, 10, 0));
        Skills.Add(new FarSkill(105, FarSkillTypes.BonusHealth, 300, 50, 50, 50, 10));
        Skills.Add(new FarSkill(106, FarSkillTypes.BonusShield, 300, 1, 0, 0, 0));        // line 2
        Skills.Add(new FarSkill(107, FarSkillTypes.BonusShield, 300, 30, 20, 0, 0));
        Skills.Add(new FarSkill(108, FarSkillTypes.BonusShield, 300, 50, 30, 3, 0));
        Skills.Add(new FarSkill(109, FarSkillTypes.BonusShield, 300, 50, 40, 10, 0));
        Skills.Add(new FarSkill(110, FarSkillTypes.BonusShield, 300, 50, 50, 50, 10));
        Skills.Add(new FarSkill(111, FarSkillTypes.BonusBomb, 300, 1, 0, 0, 0));          // line 2
        Skills.Add(new FarSkill(112, FarSkillTypes.BonusBomb, 300, 30, 20, 0, 0));
        Skills.Add(new FarSkill(113, FarSkillTypes.BonusBomb, 300, 50, 30, 1, 0));
        Skills.Add(new FarSkill(114, FarSkillTypes.BonusBomb, 300, 50, 40, 10, 0));
        Skills.Add(new FarSkill(115, FarSkillTypes.BonusBomb, 300, 50, 50, 50, 10));


    }


    #region file data
    /// <summary>
    /// Данные для сохранения.
    /// </summary>
    /// <returns></returns>
    public override List<FileManagerData> ConvertToSaveData()
    {
        //! сохранение
        var datas = new List<FileManagerData>();
        datas.Add(new FileManagerData(FileManagerTypes.Single, UnityEngine.Random.Range(53.5f, 745.9999f)));
        datas.Add(new FileManagerData(FileManagerTypes.String, StringHelper.GetRandomString(3, 7)));   //+ rnd
        datas.Add(new FileManagerData(FileManagerTypes.Int32, UnityEngine.Random.Range(33, 444)));
        //+ map
        //datas.Add(new FileManagerData(FileManagerTypes.Int32, MaxSteps));
        datas.Add(new FileManagerData(FileManagerTypes.Boolean, IsJopCompleted));
        //+ level
        //datas.Add(new FileManagerData(FileManagerTypes.Single, MaxHealth));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, MaxMoveSpeed));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ResistantBodyDamage));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ResistantAir));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ResistantRocket));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, LuckyForMaterials));
        //+ bullets
        //foreach (var bullet in Bullets)
        //{
        //    datas.Add(new FileManagerData(FileManagerTypes.Boolean, bullet.ShipHave));
        //    datas.Add(new FileManagerData(FileManagerTypes.Single, bullet.TimeToGo));
        //    datas.Add(new FileManagerData(FileManagerTypes.Single, bullet.DamageDif));
        //    datas.Add(new FileManagerData(FileManagerTypes.Single, bullet.SpeedDif));
        //}
        datas.Add(new FileManagerData(FileManagerTypes.Int32, UnityEngine.Random.Range(1, 888))); // rnd3
        //+ bonus
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusHealth));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, ShipBonusHealthCount));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusHealthTime));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusShield));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, ShipBonusShieldCount));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusShieldTime));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusTree));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, ShipBonusTreeCount));
        //datas.Add(new FileManagerData(FileManagerTypes.Single, ShipBonusTreeTime));
        //+ skills
        foreach (var skill in Skills)
        {
            datas.Add(new FileManagerData(FileManagerTypes.Boolean, skill.IsActivated));
        }

        datas.Add(new FileManagerData(FileManagerTypes.Int32, UnityEngine.Random.Range(1, 10)));    // rnd4
        return datas;
    }
    /// <summary>
    /// Загрузка данных.
    /// </summary>
    /// <param name="datas"></param>
    public override void LoadFromSaveData(List<FileManagerData> datas)
    {
        var ind = 0;
        //! загрузка
        var rnd1 = (Single)datas[ind++].DataValue;   //+ rnd
        var rndStr = (String)datas[ind++].DataValue.ToString();   //+ rnd text
        var rnd2 = (Int32)datas[ind++].DataValue;   //+ rnd
        
        //+ map
        //MaxSteps = (Int32)datas[ind++].DataValue;
        IsJopCompleted = (Boolean)datas[ind++].DataValue;
        //+ level
        //MaxHealth = (Single)datas[ind++].DataValue;
        //MaxMoveSpeed = (Single)datas[ind++].DataValue;
        //ResistantBodyDamage = (Single)datas[ind++].DataValue;
        //ResistantAir = (Single)datas[ind++].DataValue;
        //ResistantRocket = (Single)datas[ind++].DataValue;
        //LuckyForMaterials = (Single)datas[ind++].DataValue;
        //+ bullets
        //foreach (var bullet in Bullets)
        //{
        //    var b1 = (Boolean) datas[ind++].DataValue;
        //    var b2 = (Single)datas[ind++].DataValue;
        //    var b3 = (Single)datas[ind++].DataValue;
        //    var b4 = (Single)datas[ind++].DataValue;
        //    bullet.LoadData(b1, b2, b3, b4);
        //}
        var rnd3 = (Int32)datas[ind++].DataValue;   //+ rnd
        //+ bonus
        //ShipBonusHealth = (Single)datas[ind++].DataValue;
        ShipBonusHealthCount = (Int32)datas[ind++].DataValue;
        //ShipBonusHealthTime = (Single)datas[ind++].DataValue;
        //ShipBonusShield = (Single)datas[ind++].DataValue;
        ShipBonusShieldCount = (Int32)datas[ind++].DataValue;
        //ShipBonusShieldTime = (Single)datas[ind++].DataValue;
        //ShipBonusTree = (Single)datas[ind++].DataValue;
        ShipBonusTreeCount = (Int32)datas[ind++].DataValue;
        //ShipBonusTreeTime = (Single)datas[ind++].DataValue;
        //+ skills
        foreach (var skill in Skills)
        {
            var b1 = (Boolean)datas[ind++].DataValue;
            skill.LoadData(b1);
        }
        var rnd4 = (Int32)datas[ind++].DataValue;   //+ rnd
    }
    #endregion



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
    public void AddBonusHealth()
    {
        ShipBonusHealthCount++;
    }

    /// <summary>
    /// Трата бонуса Shield.
    /// </summary>
    public void UseBonusShield()
    {
        ShipBonusShieldCount--;
    }
    public void AddBonusShield()
    {
        ShipBonusShieldCount++;
    }

    /// <summary>
    /// Трата бонуса Tree.
    /// </summary>
    public void UseBonusTree()
    {
        ShipBonusTreeCount--;
    }
    public void AddBonusTree()
    {
        ShipBonusTreeCount++;
    }
    #endregion

    #region Skills logic
    public FarSkill GetSkill(Int32 num)
    {
        if (num <= 0) return null;
        return Skills.Find(f => f.Num == num);
    }
    /// <summary>
    /// Скилл приобретен.
    /// </summary>
    /// <param name="skillNum"></param>
    /// <returns></returns>
    public bool SkillIsActivated(Int32 skillNum)
    {
        if (skillNum <= 0) return false;
        var skill = GetSkill(skillNum);
        return skill.IsActivated;
    }
    /// <summary>
    /// Приобритение скилла.
    /// </summary>
    /// <param name="skillNum">от 1</param>
    public void SkillBuy(Int32 skillNum)
    {
        if (skillNum <= 0) return;
        var skill = GetSkill(skillNum);
        skill.IsActivated = true;
        SkillUpdateMath();
    }
    /// <summary>
    /// Обновление данных на основе скиллов.
    /// </summary>
    public void SkillUpdateMath()
    {
        //+ gun 1
        var gun1skills = Skills.FindAll(f => f.SkillType == FarSkillTypes.Gun1 && f.IsActivated).Count;
        var gun1 = Bullets.Find(f => f.GunIndex == 1);
        if (gun1 != null && gun1skills > 0)
        {
            switch (gun1skills)
            {
                case 1: gun1.SkillUpdate(true, 1.00f, 1.0f, 1.00f); break;
                case 2: gun1.SkillUpdate(true, 0.95f, 1.2f, 1.02f); break;
                case 3: gun1.SkillUpdate(true, 0.90f, 1.4f, 1.04f); break;
                case 4: gun1.SkillUpdate(true, 0.85f, 1.6f, 1.06f); break;
                case 5: gun1.SkillUpdate(true, 0.80f, 1.8f, 1.08f); break;
                case 6: gun1.SkillUpdate(true, 0.75f, 2.0f, 1.10f); break;
                case 7: gun1.SkillUpdate(true, 0.70f, 2.2f, 1.12f); break;
                case 8: gun1.SkillUpdate(true, 0.65f, 2.4f, 1.13f); break;
                case 9: gun1.SkillUpdate(true, 0.60f, 2.6f, 1.14f); break;
                case 10: gun1.SkillUpdate(true, 0.50f, 3.0f, 1.16f); break;
            }
        }
        //+ gun 2
        var gun2skills = Skills.FindAll(f => f.SkillType == FarSkillTypes.Gun2 && f.IsActivated).Count;
        var gun2 = Bullets.Find(f => f.GunIndex == 2);
        if (gun2 != null && gun2skills > 0)
        {
            switch (gun2skills)
            {
                case 1: gun2.SkillUpdate(true, 0.90f, 1.0f, 1.00f); break;
                case 2: gun2.SkillUpdate(true, 0.85f, 1.1f, 1.02f); break;
                case 3: gun2.SkillUpdate(true, 0.80f, 1.2f, 1.04f); break;
                case 4: gun2.SkillUpdate(true, 0.70f, 1.3f, 1.06f); break;
                case 5: gun2.SkillUpdate(true, 0.60f, 1.4f, 1.08f); break;
                case 6: gun2.SkillUpdate(true, 0.55f, 1.5f, 1.10f); break;
                case 7: gun2.SkillUpdate(true, 0.45f, 1.6f, 1.12f); break;
                case 8: gun2.SkillUpdate(true, 0.40f, 1.7f, 1.13f); break;
                case 9: gun2.SkillUpdate(true, 0.30f, 1.8f, 1.14f); break;
                case 10: gun2.SkillUpdate(true, 0.20f, 2.0f, 1.16f); break;
            }
        }
        //+ gun 3
        var gun3skills = Skills.FindAll(f => f.SkillType == FarSkillTypes.Gun3 && f.IsActivated).Count;
        var gun3 = Bullets.Find(f => f.GunIndex == 3);
        if (gun3 != null && gun3skills > 0)
        {
            switch (gun3skills)
            {
                case 1: gun3.SkillUpdate(true, 2.10f, 1.0f, 1.00f); break;
                case 2: gun3.SkillUpdate(true, 2.05f, 1.2f, 1.01f); break;
                case 3: gun3.SkillUpdate(true, 2.00f, 1.4f, 1.03f); break;
                case 4: gun3.SkillUpdate(true, 1.95f, 1.6f, 1.04f); break;
                case 5: gun3.SkillUpdate(true, 1.90f, 1.9f, 1.06f); break;
                case 6: gun3.SkillUpdate(true, 1.85f, 2.2f, 1.07f); break;
                case 7: gun3.SkillUpdate(true, 1.80f, 2.5f, 1.08f); break;
                case 8: gun3.SkillUpdate(true, 1.75f, 2.8f, 1.10f); break;
                case 9: gun3.SkillUpdate(true, 1.70f, 3.1f, 1.11f); break;
                case 10: gun3.SkillUpdate(true, 1.60f, 3.5f, 1.12f); break;
            }
        }

        //+ parsek
        var parseks = Skills.FindAll(f => f.SkillType == FarSkillTypes.Parsek && f.IsActivated).Count;
        switch(parseks)
        {
            case 1: MaxSteps = 4; break;
            case 2: MaxSteps = 5; break;
            case 3: MaxSteps = 6; break;
            case 4: MaxSteps = 8; break;
            case 5: MaxSteps = 14; break;
        }
        //+ speed
        var speeds = Skills.FindAll(f => f.SkillType == FarSkillTypes.Speed && f.IsActivated).Count;
        switch (speeds)
        {
            case 1: MaxMoveSpeed = 10.5f; break;
            case 2: MaxMoveSpeed = 12.0f; break;
            case 3: MaxMoveSpeed = 14.0f; break;
            case 4: MaxMoveSpeed = 16.0f; break;
            case 5: MaxMoveSpeed = 20.0f; break;
        }
        //+ block
        var resists = Skills.FindAll(f => f.SkillType == FarSkillTypes.Block && f.IsActivated).Count;
        switch (resists)
        {
            case 1: ResistantBodyDamage = ResistantAir= ResistantRocket = 0.95f; break;
            case 2: ResistantBodyDamage = ResistantAir= ResistantRocket = 0.9f; break;
            case 3: ResistantBodyDamage = ResistantAir= ResistantRocket = 0.85f; break;
            case 4: ResistantBodyDamage = ResistantAir= ResistantRocket = 0.8f; break;
            case 5: ResistantBodyDamage = ResistantAir = ResistantRocket = 0.6f; break;
        }

        //+ BonusHealth 
        var bhps = Skills.FindAll(f => f.SkillType == FarSkillTypes.BonusHealth && f.IsActivated).Count;
        switch (bhps)
        {
            case 1: ShipBonusHealth = 2.5f; ShipBonusHealthTime = 19.0f; break;
            case 2: ShipBonusHealth = 3.0f; ShipBonusHealthTime = 18.0f; break;
            case 3: ShipBonusHealth = 4.0f; ShipBonusHealthTime = 16.0f; break;
            case 4: ShipBonusHealth = 6.0f; ShipBonusHealthTime = 14.0f; break;
            case 5: ShipBonusHealth = 15.0f; ShipBonusHealthTime = 10.0f; break;
        }
        //+ BonusShield 
        var bshlds = Skills.FindAll(f => f.SkillType == FarSkillTypes.BonusShield && f.IsActivated).Count;
        switch (bshlds)
        {
            case 1: ShipBonusShield = 3.2f; ShipBonusShieldTime = 38.0f; break;
            case 2: ShipBonusShield = 3.5f; ShipBonusShieldTime = 35.0f; break;
            case 3: ShipBonusShield = 4.0f; ShipBonusShieldTime = 32.0f; break;
            case 4: ShipBonusShield = 5.0f; ShipBonusShieldTime = 28.0f; break;
            case 5: ShipBonusShield = 8.0f; ShipBonusShieldTime = 20.0f; break;
        }
        //+ BonusBomb 
        var bbbs = Skills.FindAll(f => f.SkillType == FarSkillTypes.BonusBomb && f.IsActivated).Count;
        switch (bbbs)
        {
            case 1: ShipBonusTree = 12.0f; ShipBonusTreeTime = 55.0f; break;
            case 2: ShipBonusTree = 14.0f; ShipBonusTreeTime = 50.0f; break;
            case 3: ShipBonusTree = 16.0f; ShipBonusTreeTime = 45.0f; break;
            case 4: ShipBonusTree = 20.0f; ShipBonusTreeTime = 35.0f; break;
            case 5: ShipBonusTree = 40.0f; ShipBonusTreeTime = 20.0f; break;
        }
    }
    #endregion

}
