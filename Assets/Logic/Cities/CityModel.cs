using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


public enum CityType
{
    Neutral = 1,
    Friend = 10,
    Alien = 20
}



/// <summary>
/// Данные города.
/// </summary>
public class CityModel
{
    public Int32 Id { get; set; }
    public CityType CityType { get; set; }
    public List<CityResourceFrom> QuestResources { get; private set; }
    public CityResourceProduct ResourceProduct { get; set; }
    public Int32 DaysToCollect { get { return ResourceProduct.ProductDays - CurrentDayCicle; } }
    public Int32 CurrentDayCicle { get; private set; }
    public Int32 Rating { get; private set; }
    public Int32 Level { get; private set; }
    public Boolean IsJop { get; private set; }
    public List<List<CityResourceFrom>> JopQuestes { get; private set; }
    public Int32 CurrentJopQuest { get; private set; }
    public bool IsJopCompleted { get; private set; }

    public float SpeedRot = 3.0f;
    public Int32 RotDir = 1;
    public float SizeScale = 1.0f;

    
    private Boolean _flagUpdateCityMapView = false;
    //x private static List<Int32> LevelUpRatings = new List<Int32>() { 10, 20, 50, 100 };

    public CityModel()
    {
        Id = 0;
        CurrentDayCicle = 0;
        Rating = 0;
        Level = 1;
        CityType = CityType.Neutral;
        ResourceProduct = new CityResourceProduct();
        QuestResources = new List<CityResourceFrom>();
        JopQuestes = new List<List<CityResourceFrom>>();
        CurrentJopQuest = 0;
        IsJopCompleted = false;
    }


    public void UpdateData(CityType cityType, Int32 currentDayCicle, Int32 rating, Int32 level, Int32 currentJopQuest, bool isJopCompleted)
    {
        CurrentDayCicle = currentDayCicle;
        Rating = rating;
        Level = level;
        CityType = cityType;
        CurrentJopQuest = currentJopQuest;
        IsJopCompleted = isJopCompleted;
    }

    /// <summary>
    /// Прошел цикл.
    /// </summary>
    public void NextDay()
    {
        if (HasProcess()) CurrentDayCicle++;

        if (CurrentDayCicle >= ResourceProduct.ProductDays) // + 1
        {
            CurrentDayCicle = 0;
            CheckCollect();
        }
        else //-  если собрали 
        {
            //if (CheckEnough()) _currentDayCicle++; //- увеличивем, если достаточно
        }
    }

    public void SetAsJop()
    {
        IsJop = true;
    }

    public void SetNextJopQuest()
    {
        CurrentJopQuest++;
        if (CurrentJopQuest >= JopQuestes.Count())
        {
            IsJopCompleted = true;
        }
    }
    /// <summary>
    /// Текущий квест.
    /// </summary>
    /// <returns></returns>
    public List<CityResourceFrom> GetCurrentJopQuestList()
    {
        if (IsJopCompleted)
        {
            var q = new List<CityResourceFrom>();
            q.Add(new CityResourceFrom() { Type = CityRecources.Res1, MustBeForProduct = 0 });
            q.Add(new CityResourceFrom() { Type = CityRecources.Res2, MustBeForProduct = 0 });
            q.Add(new CityResourceFrom() { Type = CityRecources.Res3, MustBeForProduct = 0 });
            q.Add(new CityResourceFrom() { Type = CityRecources.Res4, MustBeForProduct = 0 });
            return q;
        }
        var jopQuest = JopQuestes[CurrentJopQuest];
        return jopQuest;
    }


    /// <summary>
    /// Завершенный JOP.
    /// </summary>
    /// <returns></returns>
    public bool IsJopAndCompleted()
    {
        return IsJop && IsJopCompleted;
    }



    public void ResetUpdateViewFlag() { _flagUpdateCityMapView = false; }
    public Boolean GetUpdateViewFlag() { return _flagUpdateCityMapView; }

    /// <summary>
    /// Есть рабочий процесс.
    /// </summary>
    /// <returns></returns>
    public Boolean HasProcess()
    {
        return ResourceProduct.ProcessStarted > 0;
    }



    private void CheckCollect()
    {
        if (!HasProcess()) return;

        // освобождаем слот
        ResourceProduct.EndProccess();
        // производим
        ResourceProduct.CurrentCount += ResourceProduct.ProductionCount;
        // рейтинг годора увеличиваем
        AddCityPower();
    }


    private void AddCityPower()
    {
        Rating++;
        Rating = Rating > 900 ? 900 : Rating;
        // check new level
        var mustForLevel = LevelManager.LevelUpRatings[Level - 1];
        if (Rating >= mustForLevel)
        {
            LevelUp();
        }
    }


    private void LevelUp()
    {
        Level = Level >= 5 ? 5 : Level + 1;
        ResourceProduct.LevelUp();
        _flagUpdateCityMapView = true;
    }


    /// <summary>
    /// Инициализация квеста.
    /// </summary>
    /// <param name="res1"></param>
    /// <param name="res2"></param>
    /// <param name="res3"></param>
    /// <param name="res4"></param>
    public void InitQuest(Int32 res1, Int32 res2, Int32 res3, Int32 res4)
    {
        this.QuestResources.Add(new CityResourceFrom() { Type = CityRecources.Res1, MustBeForProduct = res1 });
        this.QuestResources.Add(new CityResourceFrom() { Type = CityRecources.Res2, MustBeForProduct = res2 });
        this.QuestResources.Add(new CityResourceFrom() { Type = CityRecources.Res3, MustBeForProduct = res3 });
        this.QuestResources.Add(new CityResourceFrom() { Type = CityRecources.Res4, MustBeForProduct = res4 });
    }

    /// <summary>
    /// Квесты JOP.
    /// </summary>
    public void AddJopQuestes(Int32 res1, Int32 res2, Int32 res3, Int32 res4)
    {
        var q = new List<CityResourceFrom>();
        q.Add(new CityResourceFrom() {Type = CityRecources.Res1, MustBeForProduct = res1});
        q.Add(new CityResourceFrom() {Type = CityRecources.Res2, MustBeForProduct = res2});
        q.Add(new CityResourceFrom() {Type = CityRecources.Res3, MustBeForProduct = res3});
        q.Add(new CityResourceFrom() {Type = CityRecources.Res4, MustBeForProduct = res4});
        this.JopQuestes.Add(q);
    }
}








public class CityModelProvider
{
    public CityModelProvider()
    {
        
    }

    public CityModel CreateCity(CityType cityType, CityRecources recources, Int32 resCount, Int32 res1, Int32 res2, Int32 res3, Int32 res4)
    {
        var model = new CityModel();
        model.CityType = cityType;
        model.ResourceProduct = CityResourceProduct.ProviderFrom(recources, resCount);
        if (cityType == CityType.Neutral)
        {
            model.InitQuest(
                res1,
                res2,
                res3,
                res4
                );
        }

        //
        model.SizeScale = 1.0f;
        switch (model.ResourceProduct.Type)
        {
            case CityRecources.Res1: model.SizeScale = 0.7f; break;
            case CityRecources.Res2: model.SizeScale = 1.0f; break;
            case CityRecources.Res3: model.SizeScale = 0.9f; break;
            case CityRecources.Res4: model.SizeScale = 1.3f; break;
        }

        model.SpeedRot = UnityEngine.Random.Range(6.0f, 12.0f);
        var dirRands = UnityEngine.Random.Range(0, 2);
        model.RotDir = dirRands > 0 ? 1 : -1;
        

        return model;
    }

    public CityModel CreateNeutralCity(CityRecources res, Int32 res1, Int32 res2, Int32 res3, Int32 res4)
    {
        return CreateCity(CityType.Neutral, res, 0, res1, res2, res3, res4);
    }
    public CityModel CreateNeutralCity(CityRecources res)
    {
        Int32 res1 = 0;
        Int32 res2 = 0;
        Int32 res3 = 0;
        Int32 res4 = 0;
        switch(res)
        {
            case CityRecources.Res1: res1 = FarBalance.NeutralPlanet1MustRes1; res2 = FarBalance.NeutralPlanet1MustRes2; res3 = FarBalance.NeutralPlanet1MustRes3; res4 = FarBalance.NeutralPlanet1MustRes4; break;
            case CityRecources.Res2: res1 = FarBalance.NeutralPlanet2MustRes1; res2 = FarBalance.NeutralPlanet2MustRes2; res3 = FarBalance.NeutralPlanet2MustRes3; res4 = FarBalance.NeutralPlanet2MustRes4; break;
            case CityRecources.Res3: res1 = FarBalance.NeutralPlanet3MustRes1; res2 = FarBalance.NeutralPlanet3MustRes2; res3 = FarBalance.NeutralPlanet3MustRes3; res4 = FarBalance.NeutralPlanet3MustRes4; break;
            case CityRecources.Res4: res1 = FarBalance.NeutralPlanet4MustRes1; res2 = FarBalance.NeutralPlanet4MustRes2; res3 = FarBalance.NeutralPlanet4MustRes3; res4 = FarBalance.NeutralPlanet4MustRes4; break;
        }
        return CreateCity(CityType.Neutral, res, 0, res1, res2, res3, res4);
    }

    public CityModel CreateFiendCity(CityRecources recources, Int32 resCount = 0)
    {
        return CreateCity(CityType.Friend, recources, resCount, 0, 0, 0, 0);
    }

    //public CityModel CreateFiendCity(CityRecources recources)
    //{
    //    return CreateCity(CityType.Friend, recources, 
    //        UnityEngine.Random.Range(5, 10),
    //            UnityEngine.Random.Range(1, 5),
    //            UnityEngine.Random.Range(0, 3),
    //            UnityEngine.Random.Range(0, 1));
    //}

    public CityModel CreateJop()
    {
        var model = new CityModel();
        model.CityType = CityType.Neutral;
        model.ResourceProduct = CityResourceProduct.ProviderFrom(CityRecources.Black, 0);
        model.SetAsJop();
        // TODO: JOP quests //?++ JOP QUESTs
        model.AddJopQuestes(1, 0, 0, 0);
        model.AddJopQuestes(2, 0, 0, 0);
        model.AddJopQuestes(0, 1, 0, 0);
        model.AddJopQuestes(0, 2, 0, 0);
        model.AddJopQuestes(0, 0, 1, 0);
        model.AddJopQuestes(0, 0, 2, 0);
        model.AddJopQuestes(0, 0, 0, 1);
        model.AddJopQuestes(0, 0, 0, 2);
        model.AddJopQuestes(10, 0, 0, 0);
        model.AddJopQuestes(20, 0, 0, 0);
        model.AddJopQuestes(10, 10, 0, 0);
        model.AddJopQuestes(20, 10, 0, 0);
        model.AddJopQuestes(10, 10, 5, 0);
        model.AddJopQuestes(20, 10, 10, 0);
        model.AddJopQuestes(20, 10, 10, 0);
        model.AddJopQuestes(20, 20, 10, 0);
        model.AddJopQuestes(20, 20, 20, 10);
        model.AddJopQuestes(20, 20, 20, 20);
        model.AddJopQuestes(1, 0, 30, 0);
        model.AddJopQuestes(2, 40, 0, 0);
        model.AddJopQuestes(1, 30, 0, 0);
        model.AddJopQuestes(2, 0, 40, 0);
        model.AddJopQuestes(5, 0, 0, 10);
        model.AddJopQuestes(6, 0, 0, 20);
        model.AddJopQuestes(100, 50, 30, 5);
        model.AddJopQuestes(200, 40, 20, 10);
        model.AddJopQuestes(100, 100, 50, 10);
        model.AddJopQuestes(200, 100, 60, 20);
        model.AddJopQuestes(200, 200, 100, 50);
        model.AddJopQuestes(300, 200, 20, 100);
        model.AddJopQuestes(600, 600, 300, 200);


        //
        model.SizeScale = 1.0f;
        model.SpeedRot = 8.0f;
        model.RotDir = 1;
        
        return model;
    }
}