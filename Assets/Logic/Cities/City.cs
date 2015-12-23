using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Модель города на карте.
/// </summary>
public class City
{
    public String Title { get; set; }
    public Vector2 Position { get; set; }
    public ResForCityImgLogic IconLogic { get; set; }
    public CityMapItem CityMap { get; private set; }

    public CityModel Model { get; set; }

    public MapGex MapGex { get; private set; }
    public Boolean CanFly { get; set; } // достаточно ходов

    
    public City()
    {
        Model = new CityModel();
    }

    public void SetCityMap(CityMapItem cityMap)
    {
        CityMap = cityMap;
        UpdateCityView();
    }

    public void SetGex(MapGex gex)
    {
        MapGex = gex;
    }

    /// <summary>
    /// Прошел цикл. Считаем все производство.
    /// <remarks>Выполняется до расчета коробля.</remarks>
    /// </summary>
    public void NextDay()
    {
        Model.ResetUpdateViewFlag();
        Model.NextDay();
        var mustViewUpdate = Model.GetUpdateViewFlag();

        if (mustViewUpdate)
        {
            UpdateCityView();
        }

        //UpdateView();
    }

    /// <summary>
    /// Обновление вида.
    /// </summary>
    public void UpdateView()
    {
        IconLogic.UpdateIcon();
    }

    /// <summary>
    /// Обновление вида города на основе ресурса и рейтинга.
    /// </summary>
    private void UpdateCityView()
    {
        var resType = Model.ResourceProduct.Type;
        var rating = Model.Rating;
        // вид города
        if (CityMap != null)
        {
            CityMap.SetCityView(resType, rating);
        }
    }


    /// <summary>
    /// Установка нового типа города.
    /// </summary>
    public void SetFriendCityType(CityRecources res)
    {
        Model.CityType = CityType.Friend;
        Model.ResourceProduct = CityResourceProduct.ProviderFrom(res);

        IconLogic.SetCityModel(this);
        UpdateCityView();
    }


}
