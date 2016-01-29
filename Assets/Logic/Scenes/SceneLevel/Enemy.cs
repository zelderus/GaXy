using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;
using ZelderFramework.Helpers;


public enum EnemyType
{
    Noob    = 0,
    Boss1   = 1,
    Boss2   = 2,
    Boss3   = 3
}

public enum EnemySay
{
    Loh = 0,
    Pen = 1,
    Boss = 2
}

/// <summary>
/// Сопоставление названия врага к индексу.
/// </summary>
public enum EnemyIndexes    //! при добавлении нового, добавить выбор Префаба в LevelController.PlaceEnemy()
{
    Loh1    = 0,
    Loh2    = 1,
    Loh3    = 2,
    Loh4    = 3,
    Loh5    = 4,
    Loh6    = 5,

    Pen1    = 50,
    Pen2    = 51,
    Pen3    = 52,
    Pen4    = 53,
    Pen5    = 54,
    Pen6    = 55,

    Boss1   = 100,
    Boss2   = 101,
    Boss3   = 102
}


#region launch
/// <summary>
/// Данные о запуске врага.
/// </summary>
public class EnemyLaunch
{
    public float TimeToStart = 1.0f;
    public Int32 EnemyIndex = 0;
    public EnemyIndexes EnemyIndexType = EnemyIndexes.Loh1;
    public bool RouteInversed = false;
    public Int32 RouteIndex = 0;
    public EnemyType EnemyType = EnemyType.Noob;

    public EnemyLaunch(EnemyIndexes enemyIndex, Int32 routeIndex, float timeToStart)
    {
        TimeToStart = timeToStart;
        EnemyIndexType = enemyIndex;
        EnemyIndex = (Int32)enemyIndex;
        EnemyType = GetEnemyType(enemyIndex);
        RouteIndex = GetRouteIndex(routeIndex);
        RouteInversed = UnityEngine.Random.Range(0, 2) > 0 ? true : false;
    }
    public EnemyLaunch(EnemyIndexes enemyIndex, Int32 routeIndex, float timeToStart, bool routeInversed)
        : this(enemyIndex, routeIndex, timeToStart)
    {
        RouteInversed = routeInversed;
    }
    private static Int32 GetRouteIndex(Int32 routeIndex)
    {
        if (routeIndex >= 0) return routeIndex;
        //? все враги сами знают свой тип маршрута
        //? во всех типах маршрутов одинаковое количество маршрутов
        const int countOfRoutes = 6;    //! одинаковое количество маршрутов у каждого типа
        var route = UnityEngine.Random.Range(0, countOfRoutes);
        route = route >= countOfRoutes ? countOfRoutes-1 : route;
        return route;
    }

    public static Int32 GetRouteIndex(BrunchRouteNum route)
    {
        return GetRouteIndex((Int32)route);
    }

    private EnemyType GetEnemyType(EnemyIndexes enemyIndex)
    {
        switch (enemyIndex)
        {
            case EnemyIndexes.Boss1: return EnemyType.Boss1;
            case EnemyIndexes.Boss2: return EnemyType.Boss2;
            case EnemyIndexes.Boss3: return EnemyType.Boss3;
        }
        return EnemyType.Noob;
    }
}
#endregion

#region brunch

public enum BrunchRouteType
{
    Random  = -1,
    Right   = 0,
    Left    = 1
}

public enum BrunchRouteNum
{
    Random = -1,
    Num1 = 0,
    Num2 = 1,
    Num3 = 2,
    Num4 = 3,
    Num5 = 4,
    Num6 = 5,
}

/// <summary>
/// Пакет врагов.
/// </summary>
public class EnemyBrunch
{
    public List<EnemyLaunch> Enemies = new List<EnemyLaunch>();
    private float _timeOffset = 0.0f;

    public EnemyBrunch(float timeOffset)
    {
        _timeOffset = timeOffset;
    }


    /// <summary>
    /// Добавление врага.
    /// </summary>
    /// <param name="enemyIndex"></param>
    /// <param name="routeIndex">-1 = случайный маршрут</param>
    /// <param name="timeToStart"></param>
    /// <param name="route"></param>
    public float AddEnemyLaunch(EnemyIndexes enemyIndex, BrunchRouteNum routeIndex, float timeToStart, BrunchRouteType route)
    {
        var t = _timeOffset + timeToStart;
        if (route == BrunchRouteType.Random) Enemies.Add(new EnemyLaunch(enemyIndex, (Int32)routeIndex, t));
        else Enemies.Add(new EnemyLaunch(enemyIndex, (Int32)routeIndex, t, route == BrunchRouteType.Left));
        return t;
    }
}
#endregion

public class EnemyBrunchProvider
{

    #region init
    public List<EnemyBrunch> Brunches = new List<EnemyBrunch>();

    private readonly List<WeightIntObject> _lohWeight = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _penWeight = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _bossWeight = new List<WeightIntObject>();

    private readonly List<WeightIntObject> _sayWeightTime1 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTime2 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTime3 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTime4 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTime5 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTime6 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTimeWithoutBoss4 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTimeWithoutBoss5 = new List<WeightIntObject>();
    private readonly List<WeightIntObject> _sayWeightTimeWithoutBoss6 = new List<WeightIntObject>();

    private bool _bossIncomed = false;

    public EnemyBrunchProvider(Int32 cityIndex, float citFactor)
    {
        // 3 степени сложности
        var hardLevel123 = 1;
        if (cityIndex >= 5) hardLevel123 = 2;
        if (cityIndex >= 11) hardLevel123 = 3;
        // вероятность типа врага
        #region weight enemy index
        const int bigStep = 250;
        const int smallStep = 160;
        _lohWeight.Add(new WeightIntObject(EnemyIndexes.Loh1, 10 + (hardLevel123 == 1 ? bigStep : 0)));
        _lohWeight.Add(new WeightIntObject(EnemyIndexes.Loh2, 10 + (hardLevel123 == 1 ? bigStep : 0)));
        _lohWeight.Add(new WeightIntObject(EnemyIndexes.Loh3, 10 + (hardLevel123 == 2 ? bigStep : hardLevel123 == 3 ? smallStep : 0)));
        _lohWeight.Add(new WeightIntObject(EnemyIndexes.Loh4, 10 + (hardLevel123 == 2 ? bigStep : hardLevel123 == 3 ? smallStep : 0)));
        _lohWeight.Add(new WeightIntObject(EnemyIndexes.Loh5, 10 + (hardLevel123 == 3 ? bigStep : 0)));
        _lohWeight.Add(new WeightIntObject(EnemyIndexes.Loh6, 10 + (hardLevel123 == 3 ? bigStep : 0)));
        //foreach(var lw in _lohWeight)
        //{
        //    Debug.Log(lw.Obj + ": " + lw.Weight + " (вес)");
        //}

        _penWeight.Add(new WeightIntObject(EnemyIndexes.Pen1, 10 + (hardLevel123 == 1 ? bigStep : 0)));
        _penWeight.Add(new WeightIntObject(EnemyIndexes.Pen2, 10 + (hardLevel123 == 1 ? bigStep : 0)));
        _penWeight.Add(new WeightIntObject(EnemyIndexes.Pen3, 10 + (hardLevel123 == 2 ? bigStep : hardLevel123 == 3 ? smallStep : 0)));
        _penWeight.Add(new WeightIntObject(EnemyIndexes.Pen4, 10 + (hardLevel123 == 2 ? bigStep : hardLevel123 == 3 ? smallStep : 0)));
        _penWeight.Add(new WeightIntObject(EnemyIndexes.Pen5, 10 + (hardLevel123 == 3 ? bigStep : 0)));
        _penWeight.Add(new WeightIntObject(EnemyIndexes.Pen6, 10 + (hardLevel123 == 3 ? bigStep : 0)));

        _bossWeight.Add(new WeightIntObject(EnemyIndexes.Boss1, 10 + (hardLevel123 == 1 ? 250 : 0)));
        _bossWeight.Add(new WeightIntObject(EnemyIndexes.Boss2, 10 + (hardLevel123 == 2 ? 250 : hardLevel123 == 3 ? 100 : 0)));
        _bossWeight.Add(new WeightIntObject(EnemyIndexes.Boss3, 10 + (hardLevel123 == 3 ? 250 : 0)));
        #endregion
        
        // тип врага
        #region weight enemy type
        _sayWeightTime1.Add(new WeightIntObject(EnemySay.Loh, 200));
        _sayWeightTime1.Add(new WeightIntObject(EnemySay.Pen, 10 + (hardLevel123 == 2 ? 100 : hardLevel123 == 3 ? 200 : 0)));

        _sayWeightTime2.Add(new WeightIntObject(EnemySay.Loh, 200 + (hardLevel123 == 1 ? 150 : 0)));
        _sayWeightTime2.Add(new WeightIntObject(EnemySay.Pen, 100 + (hardLevel123 == 2 ? 100 : hardLevel123 == 3 ? 250 : 0)));

        _sayWeightTime3.Add(new WeightIntObject(EnemySay.Loh, 100 + (hardLevel123 == 1 ? 150 : 0)));
        _sayWeightTime3.Add(new WeightIntObject(EnemySay.Pen, 100 + (hardLevel123 == 2 ? 100 : hardLevel123 == 3 ? 300 : 0)));

        _sayWeightTime4.Add(new WeightIntObject(EnemySay.Loh, 100 + (hardLevel123 == 1 ? 80 : 0)));
        _sayWeightTime4.Add(new WeightIntObject(EnemySay.Pen, 100 + (hardLevel123 == 2 ? 120 : hardLevel123 == 3 ? 300 : 0)));
        _sayWeightTime4.Add(new WeightIntObject(EnemySay.Boss, 20));
        _sayWeightTimeWithoutBoss4.Add(new WeightIntObject(EnemySay.Loh, 100 + (hardLevel123 == 1 ? 80 : 0)));
        _sayWeightTimeWithoutBoss4.Add(new WeightIntObject(EnemySay.Pen, 100 + (hardLevel123 == 2 ? 120 : hardLevel123 == 3 ? 300 : 0)));

        _sayWeightTime5.Add(new WeightIntObject(EnemySay.Loh, 200 + (hardLevel123 == 1 ? 100 : 0)));
        _sayWeightTime5.Add(new WeightIntObject(EnemySay.Pen, 200 + (hardLevel123 == 2 ? 200 : hardLevel123 == 3 ? 300 : 0)));
        _sayWeightTime5.Add(new WeightIntObject(EnemySay.Boss, 100 + (hardLevel123 == 3 ? 200 : 0)));
        _sayWeightTimeWithoutBoss5.Add(new WeightIntObject(EnemySay.Loh, 200 + (hardLevel123 == 1 ? 100 : 0)));
        _sayWeightTimeWithoutBoss5.Add(new WeightIntObject(EnemySay.Pen, 200 + (hardLevel123 == 2 ? 200 : hardLevel123 == 3 ? 300 : 0)));

        _sayWeightTime6.Add(new WeightIntObject(EnemySay.Loh, 100));
        _sayWeightTime6.Add(new WeightIntObject(EnemySay.Pen, 300));
        _sayWeightTime6.Add(new WeightIntObject(EnemySay.Boss, 300 + (hardLevel123 == 3 ? 800 : hardLevel123 == 2 ? 400 : 0)));
        _sayWeightTimeWithoutBoss6.Add(new WeightIntObject(EnemySay.Loh, 100));
        _sayWeightTimeWithoutBoss6.Add(new WeightIntObject(EnemySay.Pen, 300));

        #endregion

    }

    /// <summary>
    /// Создание набора врагов.
    /// </summary>
    /// <param name="brunchNum"></param>
    /// <param name="timeOffset"></param>
    /// <param name="cityIndex">0-16</param>
    /// <param name="citFactor"></param>
    /// <param name="ship"></param>
    /// <returns></returns>
    public float CreateBrunch(Int32 brunchNum, float timeOffset, Int32 cityIndex, float citFactor, ShipLife ship)
    {
        var brunch = new EnemyBrunch(timeOffset);
        Brunches.Add(brunch);

        var t = FillBrunch(brunchNum, brunch, timeOffset, cityIndex, citFactor, ship);

        return t; //30.0f; // t; // мы взяли менно 30 сек для своей волны (для любой волны)
    }
    #endregion

    #region enemy type
    private EnemyIndexes GetLoh()
    {
        var w = MathHelpers.GetByWeight(_lohWeight);
        //Debug.Log(w);
        return (EnemyIndexes) w;
    }
    private EnemyIndexes GetPen()
    {
        return (EnemyIndexes)MathHelpers.GetByWeight(_penWeight);
    }
    private EnemyIndexes GetBoss()
    {
        _bossIncomed = true;    //- босс только один на уровень
        return (EnemyIndexes)MathHelpers.GetByWeight(_bossWeight);
    }
    private EnemySay GetEnemySay(Int32 timeNum)
    {
        var sw = _sayWeightTime1;
        switch (timeNum)
        {
            case 1: sw = _sayWeightTime1; break;
            case 2: sw = _sayWeightTime2; break;
            case 3: sw = _sayWeightTime3; break;
            case 4: sw = _bossIncomed ? _sayWeightTimeWithoutBoss4 : _sayWeightTime4; break;
            case 5: sw = _bossIncomed ? _sayWeightTimeWithoutBoss5 : _sayWeightTime5; break;
            case 6: sw = _bossIncomed ? _sayWeightTimeWithoutBoss6 : _sayWeightTime6; break;
        }
        return (EnemySay)MathHelpers.GetByWeight(sw);
    }

    private EnemyIndexes GetEnemyBySay(EnemySay es)
    {
        switch (es)
        {
            case EnemySay.Loh: return GetLoh();
            case EnemySay.Pen: return GetPen();
            case EnemySay.Boss: return GetBoss();
        }
        return GetLoh();
    }
    #endregion
    
    private float FillBrunch(Int32 timeNum, EnemyBrunch brunch, float timeOffset, Int32 cityIndex, float citFactor, ShipLife ship)
    {
        const float timeBlock = 30.0f;
        var t = 0.0f;
        // нужно заполнить 30 сек
        while (t < timeBlock)
        {
            // берем тип врага (на основе времени)
            var enemySay = GetEnemySay(timeNum);
            //Debug.Log(enemySay);

            // берем индекс врага (на основе сложности)
            //var enemyIndex = GetEnemyBySay(EnemySay.Pen); //?++ !!!!! DEBUG !!!!!
            var enemyIndex = GetEnemyBySay(enemySay);

            // достаем пачку этого типа
            switch (enemySay)
            {
                case EnemySay.Loh: t += FillBranchLoh(enemyIndex, brunch, t); break;
                case EnemySay.Pen: t += FillBranchPen(enemyIndex, brunch, t); break;
                case EnemySay.Boss: t += FillBranchBoss(enemyIndex, brunch, t); break;
            }
        }
        return timeOffset + t;// timeBlock;
    }

    private BrunchRouteNum GetRandomRoute()
    {
        return (BrunchRouteNum)EnemyLaunch.GetRouteIndex(BrunchRouteNum.Random);
    }
    private BrunchRouteType GetRandomRouteType()
    {
        return UnityEngine.Random.Range(0, 2) > 0 ? BrunchRouteType.Left : BrunchRouteType.Right;
    }

    #region loh
    // возвращаем сколько занимаем времени именно наша пачка
    private float FillBranchLoh(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var bindex = UnityEngine.Random.Range(1, 7+1);
        //?++ возможно делать выбор по сложности города и тайм зоне
        switch (bindex)
        {
            case 1: return FillLoh1(enemyIndex, brunch, timeOffsetFromBranch);
            case 2: return FillLoh2(enemyIndex, brunch, timeOffsetFromBranch);
            case 3: return FillLoh3(enemyIndex, brunch, timeOffsetFromBranch);
            case 4: return FillLoh4(enemyIndex, brunch, timeOffsetFromBranch);
            case 5: return FillLoh5(enemyIndex, brunch, timeOffsetFromBranch);
            case 6: return FillLoh6(enemyIndex, brunch, timeOffsetFromBranch);
            case 7: return FillLoh7(enemyIndex, brunch, timeOffsetFromBranch);
        }
        return 0.0f;
    }

    private float FillLoh1(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        var routeType = GetRandomRouteType();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 5.0f, routeType);
        return 5.0f;
    }
    private float FillLoh2(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        brunch.AddEnemyLaunch(enemyIndex, BrunchRouteNum.Random, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        return 1.0f;
    }
    private float FillLoh3(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        brunch.AddEnemyLaunch(enemyIndex, BrunchRouteNum.Random, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        return 1.0f;
    }
    private float FillLoh4(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.0f, BrunchRouteType.Left);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 5.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 7.0f, BrunchRouteType.Left);
        return 7.0f;
    }
    private float FillLoh5(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        var routeType = GetRandomRouteType();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 2.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 4.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 5.0f, routeType);
        return 5.0f;
    }
    private float FillLoh6(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.1f, BrunchRouteType.Left);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 2.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 2.1f, BrunchRouteType.Left);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.1f, BrunchRouteType.Left);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 4.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 4.1f, BrunchRouteType.Left);
        return 5.0f;
    }
    private float FillLoh7(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        brunch.AddEnemyLaunch(enemyIndex, BrunchRouteNum.Random, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        return 1.0f;
    }
    #endregion

    #region pen
    // возвращаем сколько занимаем времени именно наша пачка
    private float FillBranchPen(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var bindex = UnityEngine.Random.Range(1, 7 + 1);
        switch (bindex)
        {
            case 1: return FillPen1(enemyIndex, brunch, timeOffsetFromBranch);
            case 2: return FillPen2(enemyIndex, brunch, timeOffsetFromBranch);
            case 3: return FillPen3(enemyIndex, brunch, timeOffsetFromBranch);
            case 4: return FillPen4(enemyIndex, brunch, timeOffsetFromBranch);
            case 5: return FillPen5(enemyIndex, brunch, timeOffsetFromBranch);
            case 6: return FillPen6(enemyIndex, brunch, timeOffsetFromBranch);
            case 7: return FillPen7(enemyIndex, brunch, timeOffsetFromBranch);
        }
        return 0.0f;
    }

    private float FillPen1(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        var routeType = GetRandomRouteType();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.0f, BrunchRouteType.Random);
        return 3.0f;
    }
    private float FillPen2(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        var routeType = GetRandomRouteType();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 5.0f, BrunchRouteType.Left);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 7.0f, BrunchRouteType.Left);
        return 7.0f;
    }
    private float FillPen3(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        brunch.AddEnemyLaunch(enemyIndex, BrunchRouteNum.Random, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        return 2.0f;
    }
    private float FillPen4(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        brunch.AddEnemyLaunch(enemyIndex, BrunchRouteNum.Random, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        return 2.0f;
    }
    private float FillPen5(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        var routeType = GetRandomRouteType();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, BrunchRouteType.Right);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.1f, BrunchRouteType.Left);
        return 2.0f;
    }
    private float FillPen6(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        var route = GetRandomRoute();
        var routeType = GetRandomRouteType();
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 1.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 3.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 5.0f, routeType);
        brunch.AddEnemyLaunch(enemyIndex, route, timeOffsetFromBranch + 6.0f, routeType);
        return 6.0f;
    }
    private float FillPen7(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        brunch.AddEnemyLaunch(enemyIndex, BrunchRouteNum.Random, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        return 2.0f;
    }
    #endregion

    #region boss
    // возвращаем сколько занимаем времени именно наша пачка
    private float FillBranchBoss(EnemyIndexes enemyIndex, EnemyBrunch brunch, float timeOffsetFromBranch)
    {
        brunch.AddEnemyLaunch(enemyIndex, BrunchRouteNum.Random, timeOffsetFromBranch + 1.0f, BrunchRouteType.Random);
        return 15.0f;
    }
    #endregion

}