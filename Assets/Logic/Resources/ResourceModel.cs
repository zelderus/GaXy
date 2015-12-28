using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;




public class ResourceModel 
{

	
}



public enum CityRecources
{
    Material = 1,
    Res1 = 3,
    Res2 = 4,
    Res3 = 5,
    Res4 = 6,
    Black = 11
}


/// <summary>
/// Ресурс для производства.
/// </summary>
public class CityResource
{
    public CityRecources Type { get; set; }
    //public Int32 CurrentCount { get; set; }

    public CityResource()
    {
        Type = CityRecources.Material;
        //CurrentCount = 0;
    }
}

/// <summary>
/// Ресрс в наличии.
/// </summary>
public class CityResourceShip : CityResource
{
    public Int32 CurrentCount { get; set; }

    public CityResourceShip()
    {
        CurrentCount = 0;
    }


}

/// <summary>
/// Ресурс для производства.
/// </summary>
public class CityResourceFrom : CityResource
{
    /// <summary>
    /// Необходимо количества на цикл производства.
    /// </summary>
    public Int32 MustBeForProduct { get; set; }


    public CityResourceFrom()
    {
        Type = CityRecources.Material;
        //CurrentCount = 0;
        MustBeForProduct = 1;
    }

    ///// <summary>
    ///// Необходимо ресурса для производства.
    ///// </summary>
    ///// <returns></returns>
    //public Int32 GetCheckEnough()
    //{
    //    return MustBeForProduct - this.CurrentCount;
    //}
}



/// <summary>
/// Ресурс производимый.
/// </summary>
public class CityResourceProduct
{
    public CityRecources Type { get; set; }
    public Int32 CurrentCount { get; set; }
    /// <summary>
    /// Ресурсы для производства.
    /// </summary>
    public List<CityResourceFrom> Resources { get; set; }
    /// <summary>
    /// Количество произведенного за цикл.
    /// </summary>
    public Int32 ProductionCount { get; set; }
    /// <summary>
    /// Количество дней на цикл производства.
    /// </summary>
    public Int32 ProductDays { get; set; }


    public Int32 MaxProcesses { get; private set; }
    /// <summary>
    /// Запущено процессов.
    /// </summary>
    public Int32 ProcessStarted { get; private set; }


    public CityResourceProduct()
    {
        Type = CityRecources.Material;
        CurrentCount = 0;
        Resources = new List<CityResourceFrom>();
        ProductionCount = 1;
        ProductDays = 1;

        MaxProcesses = 1;
        ProcessStarted = 0;
    }

    public void UpdateData(Int32 currentCount, Int32 maxProcesses, Int32 processStarted)
    {
        CurrentCount = currentCount;
        MaxProcesses = maxProcesses;
        ProcessStarted = processStarted;
    }

    /// <summary>
    /// Есть свободный слот для запуска процесса.
    /// </summary>
    /// <returns></returns>
    public Boolean CanProccess()
    {
        return ProcessStarted < MaxProcesses;
    }
    /// <summary>
    /// Запуск нового процесса.
    /// </summary>
    public void StartProccess()
    {
        if (ProcessStarted >= MaxProcesses) return;
        ProcessStarted++;
    }
    /// <summary>
    /// Закончился процесс.
    /// </summary>
    public void EndProccess()
    {
        ProcessStarted--;
        ProcessStarted = ProcessStarted < 0 ? 0 : ProcessStarted;
    }

    /// <summary>
    /// Увеличение уровня.
    /// </summary>
    public void LevelUp()
    {
        MaxProcesses = MaxProcesses >= 5 ? 5 : MaxProcesses + 1;
        //?+ количество произведенного и прочее

    }

    #region provider
    public static CityResourceProduct ProviderFrom(CityRecources resource, Int32 currentCount = 0)
    {
        var res = new CityResourceProduct();
        res.Type = resource;
        res.CurrentCount = currentCount;
        res.ProductionCount = 1;
        res.ProductDays = 1;

        if (resource == CityRecources.Res1)
        {
            res.Resources.Add(new CityResourceFrom() { Type = CityRecources.Material, MustBeForProduct = 100 });
            res.ProductionCount = 10;
            res.ProductDays = 5;
        }
        else if (resource == CityRecources.Res2)
        {
            res.Resources.Add(new CityResourceFrom() { Type = CityRecources.Res1, MustBeForProduct = 12 });
            res.ProductionCount = 6;
            res.ProductDays = 8;
        }
        else if (resource == CityRecources.Res3)
        {
            res.Resources.Add(new CityResourceFrom() { Type = CityRecources.Res1, MustBeForProduct = 2 });
            res.Resources.Add(new CityResourceFrom() { Type = CityRecources.Res2, MustBeForProduct = 5 });
            res.ProductionCount = 3;
            res.ProductDays = 10;
        }
        else if (resource == CityRecources.Res4)
        {
            res.Resources.Add(new CityResourceFrom() { Type = CityRecources.Res2, MustBeForProduct = 2 });
            res.Resources.Add(new CityResourceFrom() { Type = CityRecources.Res3, MustBeForProduct = 5 });
            res.ProductionCount = 1;
            res.ProductDays = 20;
        }
        else if (resource == CityRecources.Black)
        {
            res.Resources.Add(new CityResourceFrom() { Type = CityRecources.Res4, MustBeForProduct = 99 });
            res.ProductionCount = 1;
            res.ProductDays = 1;
        }

        return res;
    }
    #endregion


}


