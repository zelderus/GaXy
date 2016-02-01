using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Некоторые глобальные данные важные для баланса игры.
/// </summary>
public static class FarBalance
{

    /// <summary>
    /// Количество попыток (поражений) на одном уровне, чтобы сбросить сложность.
    /// </summary>
    public static Int32 NumLastCityCountOfRuns = 3;

    // ресурсы на старте игры
    public static Int32 StartResMat = 120;
    public static Int32 StartRes1 = 7;
    public static Int32 StartRes2 = 0;
    public static Int32 StartRes3 = 0;
    public static Int32 StartRes4 = 0;

    /// <summary>
    /// Автоматическое прохождение уровня с победой. Включается в статистике. Необходимо для балансировки.
    /// </summary>
    public static Boolean LevelAutoCompleteWithWin = false;
    public static Int32 MaterialCountOnAutoLevelWin = 100;

    // Необходимые ресурсы для разблокировки планеты
    public static Int32 NeutralPlanet1MustRes1 = 2;
    public static Int32 NeutralPlanet1MustRes2 = 0;
    public static Int32 NeutralPlanet1MustRes3 = 0;
    public static Int32 NeutralPlanet1MustRes4 = 0;
    public static Int32 NeutralPlanet2MustRes1 = 4;
    public static Int32 NeutralPlanet2MustRes2 = 2;
    public static Int32 NeutralPlanet2MustRes3 = 0;
    public static Int32 NeutralPlanet2MustRes4 = 0;
    public static Int32 NeutralPlanet3MustRes1 = 8;
    public static Int32 NeutralPlanet3MustRes2 = 4;
    public static Int32 NeutralPlanet3MustRes3 = 2;
    public static Int32 NeutralPlanet3MustRes4 = 0;
    public static Int32 NeutralPlanet4MustRes1 = 10;
    public static Int32 NeutralPlanet4MustRes2 = 8;
    public static Int32 NeutralPlanet4MustRes3 = 4;
    public static Int32 NeutralPlanet4MustRes4 = 1;

}
