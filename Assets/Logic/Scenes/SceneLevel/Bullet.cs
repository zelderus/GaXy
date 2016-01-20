using System;
using UnityEngine;
using System.Collections;


/// <summary>
/// Параметры снаряда у корабля.
/// </summary>
public class Bullet
{
    /// <summary>
    /// Индекс префаба снаряда.
    /// </summary>
    public Int32 GunIndex { get; private set; }
    public Boolean ShipHave { get; private set; }
    public float TimeToGo { get; private set; }
    public float DamageDif { get; private set; }    // разница от 1.0 - 999
    public float SpeedDif { get; private set; }     // разница от 1.0 - 999

    public float TimeToGoWorking = 0.0f;

    public Bullet()
    {
        GunIndex = 0;
        ShipHave = false;
        TimeToGo = 1.0f;

        DamageDif = 1.0f;
        SpeedDif = 1.0f;
    }

    public Bullet(Int32 gunIndex, Boolean shipHave, float timeToGo, float damageDif = 1.0f, float speedDif = 1.0f)
    {
        GunIndex = gunIndex;
        ShipHave = shipHave;
        TimeToGo = timeToGo;
        DamageDif = damageDif;
        SpeedDif = speedDif;
    }

    //public void LoadData(Boolean shipHave, float timeToGo, float damageDif, float speedDif)
    //{
    //    ShipHave = shipHave;
    //    TimeToGo = timeToGo;
    //    DamageDif = damageDif;
    //    SpeedDif = speedDif;
    //}

    public void SkillUpdate(Boolean shipHave, float timeToGo, float damageDif, float speedDif)
    {
        ShipHave = shipHave;
        TimeToGo = timeToGo;
        DamageDif = damageDif;
        SpeedDif = speedDif;
    }
}
