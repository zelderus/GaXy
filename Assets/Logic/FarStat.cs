using UnityEngine;
using System.Collections;
using ZelderFramework.FileSystem;
using System.Collections.Generic;
using System;

/// <summary>
/// Статистика.
/// </summary>
public class FarStat : FileManagedClass
{

    public Int32 LevelRuns { get; private set; }    // всего запущено уровней
    public Int32 LevelWins { get; private set; }    // всего выиграно уровней
    public float LevelTotalTime { get; private set; }    // время игры на уровне (и победы и поражения)
    public float LevelWinsTime { get; private set; }     // время игры на уровне (только победы)
    public Int32 TotalEnemyDied { get; private set; }       // всего врагов убито
    public Int32 TotalBossDied { get; private set; }        // всего боссов убито


    public FarStat()
    {
        LevelRuns = 0;
        LevelWins = 0;
        LevelTotalTime = 0.0f;
        LevelWinsTime = 0.0f;
        TotalEnemyDied = 0;
        TotalBossDied = 0;
    }


    #region file data
    /// <summary>
    /// Данные для сохранения.
    /// </summary>
    /// <returns></returns>
    public override List<FileManagerData> ConvertToSaveData()
    {
        var datas = new List<FileManagerData>();

        datas.Add(new FileManagerData(FileManagerTypes.Int32, LevelRuns));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, LevelWins));
        datas.Add(new FileManagerData(FileManagerTypes.Single, LevelTotalTime));
        datas.Add(new FileManagerData(FileManagerTypes.Single, LevelWinsTime));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, TotalEnemyDied));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, TotalBossDied));

        return datas;
    }
    /// <summary>
    /// Загрузка данных.
    /// </summary>
    /// <param name="datas"></param>
    public override void LoadFromSaveData(List<FileManagerData> datas)
    {
        var ind = 0;

        LevelRuns = (Int32)datas[ind++].DataValue;
        LevelWins = (Int32)datas[ind++].DataValue;
        LevelTotalTime = (float)datas[ind++].DataValue;
        LevelWinsTime = (float)datas[ind++].DataValue;
        TotalEnemyDied = (Int32)datas[ind++].DataValue;
        TotalBossDied = (Int32)datas[ind++].DataValue;
    }
    #endregion


    public void OnLevelRun() { LevelRuns++; }
    public void OnLevelWin() { LevelWins++; }
    public void OnLevelEndTime(float addTime, bool isWin) { LevelTotalTime += addTime; if (isWin) LevelWinsTime += addTime; }
    public void OnEnemyDie() { TotalEnemyDied++; }
    public void OnBossDie() { TotalBossDied++; }
}
