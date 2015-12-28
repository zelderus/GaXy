﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using ZelderFramework.Helpers;


/// <summary>
/// Логика уровня.
/// </summary>
public class LevelManager
{

    public Boolean IsPaused { get; private set; }
    public ShipLife Ship { get; private set; }
    public City City { get; private set; }
    public Boolean LevelIsEnded { get; private set; }
    public Boolean LevelIsOnEnd { get; private set; }


    private LevelController _controller;

    private float _timeGo = 0.0f;
    public float _shipPosY = 0.0f;
    private float _shipPosEndY = 200.0f;    // дальность карты

    private Int32 _cityIndex = 0;
    private float _citFactor = 1.0f;
    
    private List<EnemyLaunch> _enemies;
    private Int32 _enemyIndex = 0;
    private EnemyLaunch _currentEnemy = null;

    private bool _boss1Launched = false;
    private bool _boss2Launched = false;
    private bool _boss3Launched = false;
 

    public LevelManager(LevelController controller, ShipLife ship, City city)
    {
        _controller = controller;

        IsPaused = true;
        LevelIsEnded = false;
        LevelIsOnEnd = false;
        Ship = ship;
        City = city;
        _timeGo = 0.0f;

        InitLevelObjects();
        InitEnemies();
    }



    private void InitLevelObjects()
    {
        var cityLevel = City.Model.Level;
        var cityRating = City.Model.Rating;
        var cityIndex = 0; // 0 - 16
        #region city index (old)
        //if (cityRating >= 10) cityIndex = 1;
        //if (cityRating >= 20) cityIndex = 2;
        //if (cityRating >= 40) cityIndex = 3;
        //if (cityRating >= 50) cityIndex = 4;
        //if (cityRating >= 80) cityIndex = 5;
        //if (cityRating >= 100) cityIndex = 6;
        //if (cityRating >= 150) cityIndex = 7;
        //if (cityRating >= 200) cityIndex = 8;
        //if (cityRating >= 250) cityIndex = 9;
        //if (cityRating >= 300) cityIndex = 10;
        //if (cityRating >= 350) cityIndex = 11;
        //if (cityRating >= 400) cityIndex = 12;
        //if (cityRating >= 450) cityIndex = 13;
        //if (cityRating >= 500) cityIndex = 14;
        //if (cityRating >= 550) cityIndex = 15;
        //if (cityRating >= 600) cityIndex = 16;
        //_cityIndex = cityIndex;
        #endregion
        #region city index
        if (cityRating >= 10) cityIndex = 1;
        if (cityRating >= 20) cityIndex = 2;
        if (cityRating >= 30) cityIndex = 3;
        if (cityRating >= 40) cityIndex = 4;
        if (cityRating >= 50) cityIndex = 5;
        if (cityRating >= 55) cityIndex = 6;
        if (cityRating >= 60) cityIndex = 7;
        if (cityRating >= 65) cityIndex = 8;
        if (cityRating >= 70) cityIndex = 9;
        if (cityRating >= 75) cityIndex = 10;
        if (cityRating >= 80) cityIndex = 11;
        if (cityRating >= 85) cityIndex = 12;
        if (cityRating >= 90) cityIndex = 13;
        if (cityRating >= 95) cityIndex = 14;
        if (cityRating >= 100) cityIndex = 15;
        if (cityRating >= 110) cityIndex = 16;
        _cityIndex = cityIndex;
        #endregion

        _citFactor = GetCityFactor(_cityIndex);
        // на основе города, уровня и прочего генерирует врагов и прочее
        _shipPosEndY = 180.0f / 10.0f;

        //- materials
        _timeMaterialInt = 30.0f * (1 / _citFactor);
        _materialCount = 10;

    }

    private void InitEnemies()
    {
        _enemies = new List<EnemyLaunch>();
        var lastTime = 0.0f;

        // создаем 6 волн
        var provider = new EnemyBrunchProvider(_cityIndex, _citFactor);
        lastTime = provider.CreateBrunch(1, lastTime, _cityIndex, _citFactor, Ship);
        lastTime = provider.CreateBrunch(2, lastTime, _cityIndex, _citFactor, Ship);
        lastTime = provider.CreateBrunch(3, lastTime, _cityIndex, _citFactor, Ship);
        lastTime = provider.CreateBrunch(4, lastTime, _cityIndex, _citFactor, Ship);
        lastTime = provider.CreateBrunch(5, lastTime, _cityIndex, _citFactor, Ship);
        lastTime = provider.CreateBrunch(6, lastTime, _cityIndex, _citFactor, Ship);
        // берем необходимое из всего этого
        foreach (var brunch in provider.Brunches)
        {
            _enemies.AddRange(brunch.Enemies);
        }

        //_enemies.Add(new EnemyLaunch(EnemyIndexes.Loh1, 0, 1.0f));
        //_enemies.Add(new EnemyLaunch(EnemyIndexes.Boss1, 0, 1.0f));

        //foreach (var en in _enemies)
        //{
        //    Debug.Log(String.Format("Time: {0}; Index: {1}; Side: {2}", en.TimeToStart, en.EnemyIndex, en.RouteInversed));
        //}
    }
    private const float MaxFactor = 5.0f;
    private float GetCityFactor(Int32 cityIndex)
    {
        var p = MathHelpers.Percent(0, 16, cityIndex);
        var f = MathHelpers.ByPercent(MaxFactor - 1.0f, p);
        f = f + 1.0f;   // сдвигаем от 0
        return f;
    }

    public Int32 GetCityIndex()
    {
        return _cityIndex;
    }


    public void Pause(Boolean isPause)
    {
        IsPaused = isPause;
    }


    #region materials
    private float _timeMaterialInt = 10.0f;
    private float _timeMaterialGo = 0.0f;
    private Int32 _materialCount = 15;

    private void MaterialUpdate(float deltaTime)
    {
        _timeMaterialGo += deltaTime;
        if (_timeMaterialGo >= _timeMaterialInt)
        {
            _timeMaterialGo = 0.0f;

            var matCount = UnityEngine.Random.Range(_materialCount - 10, _materialCount + 10);
            if (matCount > 0)
            {
                _controller.PlaceMaterial(new Vector2(UnityEngine.Random.Range(-3.0f, 3.0f), 7.0f), matCount);
            }
        }
    }

    #endregion
    #region enemies

    private Int32 _totalEnemyOnMap = 0;
    private bool _enemyHasNot = false;

    /// <summary>
    /// На врага меньше.
    /// </summary>
    public void EnemyRemoved()
    {
        _totalEnemyOnMap--;
    }

    private float _enemyIntervalTime = 0.0f;

    private EnemyLaunch GetNextEnemy()
    {
        if (_enemyIndex >= _enemies.Count)
        {
            //_enemyIndex = 0;
            //_enemyIntervalTime = 0.0f;  //- сбрасываем таймер
            _enemyHasNot = true;        // хватит
            return null;
        }
        //- берем
        var enemy = _enemies[_enemyIndex];
        _enemyIndex++;
        ////- если босс и уже улетел
        //if (enemy.EnemyType == EnemyType.Boss1)
        //{
        //    if (_boss1Launched)
        //    {
        //        _enemyIndex++;
        //        enemy = GetNextEnemy();
        //    }
        //    _boss1Launched = true;
        //}
        //if (enemy.EnemyType == EnemyType.Boss2)
        //{
        //    if (_boss2Launched)
        //    {
        //        _enemyIndex++;
        //        enemy = GetNextEnemy();
        //    }
        //    _boss2Launched = true;
        //}
        //if (enemy.EnemyType == EnemyType.Boss3)
        //{
        //    if (_boss3Launched)
        //    {
        //        _enemyIndex++;
        //        enemy = GetNextEnemy();
        //    }
        //    _boss3Launched = true;
        //}
        return enemy;
    }
    
    private void EnemiesUpdate(float deltaTime)
    {
        if (LevelIsOnEnd) return;
        if (_enemyHasNot) return;   // если кончились враги

        _enemyIntervalTime += deltaTime;
        //- берем следующего
        if (_currentEnemy == null)
        {
            _currentEnemy = GetNextEnemy();
            if (_currentEnemy == null) return;
        }
        //- запускаем
        if (_enemyIntervalTime >= _currentEnemy.TimeToStart)
        {
            _controller.PlaceEnemy(_currentEnemy.EnemyIndexType, _currentEnemy.RouteIndex, _currentEnemy.RouteInversed);
            _totalEnemyOnMap++;
            _currentEnemy = null; //- хотим следующего
        }
    }

    #endregion


    private float _timeToEnd = 5.0f;

    /// <summary>
    /// Обновление. Вызывается если нет паузы.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        if (LevelIsEnded) return;
        if (LevelIsOnEnd)
        {
            _timeToEnd -= deltaTime;
            if (_timeToEnd <= 0.0f)
            {
                LevelIsEnded = true;
            }
        }
        
        // позиция
        _timeGo += deltaTime;
        _shipPosY += (Ship.FlySpeed * deltaTime);
        //x if (_shipPosY >= _shipPosEndY && _totalEnemyOnMap <= 0) // долетели до конца и кончились враги
        if (_enemyHasNot && _totalEnemyOnMap <= 0) // кончились враги
        {
            LevelIsOnEnd = true;
            //LevelIsEnded = true;
            return;
        }

        _controller.Log.SetText(String.Format("Time: {0}; Dist: {1}/{2}", _timeGo.ToString("F2"), _shipPosY.ToString("F2"), _shipPosEndY.ToString("F2")));

        //! логика появления всего и вся
        //- materials
        MaterialUpdate(deltaTime);
        //- enemies
        EnemiesUpdate(deltaTime);


    }
    



}
