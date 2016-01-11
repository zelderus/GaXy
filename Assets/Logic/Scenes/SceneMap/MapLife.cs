using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using ZelderFramework.FileSystem;
using UnityEngine;



public enum FarStatusLevel
{
    Init = 1,
    LevelWin = 5,
    LevelLose = 6
}
public class MapGexPos
{
    public Int32 X;
    public Int32 Y;
    public MapGexPos(Int32 x, Int32 y)
    {
        X = x;
        Y = y;
    }
}

public class BehemotPos
{
    public Vector2 Position { get; set; }
    public Vector2 Direction { get; set; }

    public BehemotPos(float posX, float posY, float dirX, float dirY)
    {
        Position = new Vector2(posX, posY);
        Direction = new Vector2(dirX, dirY);
    }
}

/// <summary>
/// Гекс.
/// </summary>
public class MapGex
{
    public Int32 GexX;
    public Int32 GexY;
    public City City { get; private set; }
    public Vector2 Position { get; set; }
    public Boolean IsBlocked { get; set; }

    public MapGex()
    {
        IsBlocked = false;
    }
    public MapGex(Int32 gexX, Int32 gexY) : this()
    {
        GexX = gexX;
        GexY = gexY;
    }
    public void SetCity(City city)
    {
        City = city;
    }
}


public class MapLife : FileManagedClass
{
    /// <summary>
    /// Текущий день.
    /// </summary>
    public Int32 Day { get; set; }

    /// <summary>
    /// Ресурсы в наличии.
    /// </summary>
    public List<CityResourceShip> Resources;
    public City CurrenctCity = null;
    public City NextCity = null;
    public List<City> Cities { get; private set; }
    public FarStatusLevel LevelStatus { private set; get; }


    public Int32 GexMaxX { get { return 18; } }
    public Int32 GexMaxY { get { return 23; } }
    /// <summary>
    /// Гексо-поле.
    /// </summary>
    public MapGex[,] Gex { private set; get; }

    private Int32[,] _workGex;

    private ShipLife _ship;
    private Int32 _currentCityId = 0;
    private Int32 _currentNextCityId = 0;

    public MapLife()
    {
        Day = 0;
        LevelStatus = FarStatusLevel.Init;
    }

    public void Init(ShipLife ship)
    {
        _ship = ship;

        //+ datas
        InitDatas();
        //+ gex
        InitMapSots();
        //+ ress
        InitResources();
        //+ city
        InitCities();
    }

    #region load/init datas


    /// <summary>
    /// Загрузка/инициализация параметров.
    /// </summary>
    private void InitDatas()
    {
        Day = 0;
        LevelStatus = FarStatusLevel.Init;
    }

    /// <summary>
    /// Загрузка/инициализация ресурсов.
    /// </summary>
    private void InitResources()
    {
        Resources = new List<CityResourceShip>();
        Resources.Add(new CityResourceShip() { Type = CityRecources.Material, CurrentCount = 120 });
        Resources.Add(new CityResourceShip() { Type = CityRecources.Res1, CurrentCount = 5 });
        Resources.Add(new CityResourceShip() { Type = CityRecources.Res2, CurrentCount = 0 });
        Resources.Add(new CityResourceShip() { Type = CityRecources.Res3, CurrentCount = 0 });
        Resources.Add(new CityResourceShip() { Type = CityRecources.Res4, CurrentCount = 0 });
    }
    
    /// <summary>
    /// Загрузка/Инициализация городов.
    /// </summary>
    private void InitCities()
    {
        Cities = LoadCities();
        SetCurrentCity(Cities.FirstOrDefault());
    }
    private List<City> LoadCities()
    {
        var provider = new CityModelProvider();
        var list = new List<City>();

        //! края 1,1 - 17,22
        AddCity(list, "Moscow", 5, 5, provider.CreateFiendCity(CityRecources.Res1));
        //if (!_ship.IsJopCompleted) AddCity(list, "JOP", 5, 5, provider.CreateJop());

        AddCity(list, "Kiev", 7, 4, provider.CreateFiendCity(CityRecources.Res1, 9));
        AddCity(list, "Riga", 7, 6, provider.CreateFiendCity(CityRecources.Res2, 6));
        AddCity(list, "Laja", 8, 8, provider.CreateFiendCity(CityRecources.Res3, 5));
        AddCity(list, "Laja", 6, 8, provider.CreateFiendCity(CityRecources.Res2, 1));
        AddCity(list, "Opa", 7, 10, provider.CreateFiendCity(CityRecources.Res4, 1));
        AddCity(list, "Laja", 4, 9, provider.CreateNeutralCity(CityRecources.Res1, 1, 0, 0, 0));
        AddCity(list, "Laja", 10, 6, provider.CreateNeutralCity(CityRecources.Res3));

        AddCity(list, "Asd", 2, 3, provider.CreateFiendCity(CityRecources.Res2));
        AddCity(list, "Asd", 1, 5, provider.CreateNeutralCity(CityRecources.Res3));

        AddCity(list, "Laja", 13, 6, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Laja", 16, 6, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Laja", 16, 7, provider.CreateNeutralCity(CityRecources.Res2));
        AddCity(list, "Laja", 15, 4, provider.CreateNeutralCity(CityRecources.Res2));
        AddCity(list, "Laja", 11, 1, provider.CreateNeutralCity(CityRecources.Res4));
        AddCity(list, "Laja", 11, 8, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Laja", 13, 9, provider.CreateNeutralCity(CityRecources.Res2));
        AddCity(list, "Laja", 14, 12, provider.CreateNeutralCity(CityRecources.Res2));
        AddCity(list, "Smll", 10, 16, provider.CreateNeutralCity(CityRecources.Res2));
        AddCity(list, "Smll", 16, 19, provider.CreateNeutralCity(CityRecources.Res3));
        AddCity(list, "Laja", 16, 9, provider.CreateNeutralCity(CityRecources.Res3));
        AddCity(list, "Smll", 12, 15, provider.CreateNeutralCity(CityRecources.Res3));
        AddCity(list, "Smll", 17, 2, provider.CreateNeutralCity(CityRecources.Res3));
        AddCity(list, "Smll", 13, 22, provider.CreateNeutralCity(CityRecources.Res2));
        AddCity(list, "Smll", 15, 20, provider.CreateNeutralCity(CityRecources.Res2));


        // trash
        AddCity(list, "Smll", 15, 10, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 16, 13, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 16, 16, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 14, 18, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 16, 22, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 10, 11, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 12, 12, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 14, 14, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 13, 20, provider.CreateNeutralCity(CityRecources.Res1));
        AddCity(list, "Smll", 11, 19, provider.CreateNeutralCity(CityRecources.Res1));
        
        // top left mass
        AddCity(list, "Addd", 6, 22, provider.CreateNeutralCity(CityRecources.Res4));
        AddCity(list, "Addd", 2, 21, provider.CreateNeutralCity(CityRecources.Res2));
        AddCity(list, "Addd", 4, 19, provider.CreateNeutralCity(CityRecources.Res4));
        AddCity(list, "Fggg", 4, 21, provider.CreateNeutralCity(CityRecources.Res3));



        // JOP
        if (!_ship.IsJopCompleted) AddCity(list, "JOP", 10, 13, provider.CreateJop());

        //AddCity(list, "Popa", 17, 22, provider.CreateFiendCity(CityRecources.Res1));
        return list;
    }

    private Int32 _cityIdGlobal = 700;
    private void AddCity(List<City> list, String title, Int32 x, Int32 y, CityModel cityModel)
    {
        var gexX = WrapGexX(x);
        var gexY = WrapGexY(y);
        // гекс
        var gex = Gex[gexX, gexY];
        // город
        //Debug.Log(_cityIdGlobal);
        _cityIdGlobal++;
        cityModel.Id = _cityIdGlobal;
        var city = new City() { Title = title, Position = gex.Position, Model = cityModel };
        city.SetGex(gex);

        gex.SetCity(city);
        // в список
        list.Add(city);
    }
    #endregion

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
        
        //
        datas.Add(new FileManagerData(FileManagerTypes.Int32, (Int32)Day));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, (Int32)LevelStatus));
        // resources
        foreach (var res in Resources)
        {
            datas.Add(new FileManagerData(FileManagerTypes.Int32, res.CurrentCount));
        }
        // cities
        datas.Add(new FileManagerData(FileManagerTypes.Int32, _currentCityId));
        datas.Add(new FileManagerData(FileManagerTypes.Int32, _currentNextCityId));
        foreach (var city in Cities)
        {
            datas.Add(new FileManagerData(FileManagerTypes.Int32, (Int32)city.Model.CityType));
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.CurrentDayCicle));
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.Rating));
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.Level));
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.CurrentJopQuest));
            datas.Add(new FileManagerData(FileManagerTypes.Boolean, city.Model.IsJopCompleted));
            //- trash
            datas.Add(new FileManagerData(FileManagerTypes.Single, city.Model.SpeedRot));
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.RotDir));
            datas.Add(new FileManagerData(FileManagerTypes.Single, city.Model.SizeScale));
            //
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.ResourceProduct.CurrentCount));
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.ResourceProduct.MaxProcesses));
            datas.Add(new FileManagerData(FileManagerTypes.Int32, city.Model.ResourceProduct.ProcessStarted));
        }

        datas.Add(new FileManagerData(FileManagerTypes.Int32, UnityEngine.Random.Range(1, 10)));
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
        //! загрузка
        var rnd1 = (Single)datas[ind++].DataValue;   //+ rnd
        var rndStr = (String)datas[ind++].DataValue.ToString();   //+ rnd text

        //
        Day = (Int32)datas[ind++].DataValue;
        LevelStatus = (FarStatusLevel)(Int32)datas[ind++].DataValue;
        // resources
        foreach (var res in Resources)
        {
            var b1 = (Int32)datas[ind++].DataValue;
            res.CurrentCount = b1;
        }
        // cities
        _currentCityId = (Int32)datas[ind++].DataValue;
        _currentNextCityId = (Int32)datas[ind++].DataValue;
        foreach (var city in Cities)
        {
            var c1 = (CityType)(Int32)datas[ind++].DataValue;
            var c2 = (Int32)datas[ind++].DataValue;
            var c3 = (Int32)datas[ind++].DataValue;
            var c4 = (Int32)datas[ind++].DataValue;
            var c5 = (Int32)datas[ind++].DataValue;
            var c6 = (Boolean)datas[ind++].DataValue;
            city.Model.UpdateData(c1, c2, c3, c4, c5, c6);
            var t1 = (Single)datas[ind++].DataValue;
            var t2 = (Int32)datas[ind++].DataValue;
            var t3 = (Single)datas[ind++].DataValue;
            city.Model.SpeedRot = t1;
            city.Model.RotDir = t2;
            city.Model.SizeScale = t3;
            //
            var r1 = (Int32)datas[ind++].DataValue;
            var r2 = (Int32)datas[ind++].DataValue;
            var r3 = (Int32)datas[ind++].DataValue;
            city.Model.ResourceProduct.UpdateData(r1, r2, r3);
        }
        SetCurrentCity(Cities.FirstOrDefault(f => f.Model.Id == _currentCityId));
        SetNextCity(Cities.FirstOrDefault(f => f.Model.Id == _currentNextCityId));


        var rnd3 = (Int32)datas[ind++].DataValue;   //+ rnd
        var rnd4 = (Int32)datas[ind++].DataValue;   //+ rnd
    }
    #endregion


    #region map sots
    /// <summary>
    /// Соты карты (гексы).
    /// </summary>
    private void InitMapSots()
    {
        // генерация всех гексов
        Gex = new MapGex[GexMaxX, GexMaxY];
        for (Int32 x = 0; x < GexMaxX; x++)
        {
            for (Int32 y = 0; y < GexMaxY; y++)
            {
                var gex = new MapGex(x, y);
                gex.Position = GetMapPositionByGex(x, y);

                Gex[x, y] = gex;
            }
        }
        // для алгоритма поиска пути
        _workGex = new Int32[GexMaxX, GexMaxY];

        //?+ установка конкретным гексам значений
        //Gex[1,2].Set(
        //Gex[0, 1].IsBlocked = true;

    }

    /// <summary>
    /// Освобождение ячейки карты.
    /// </summary>
    /// <param name="gex"></param>
    public void FreeMapGex(MapGex gex)
    {
        var x = gex.GexX;
        var y = gex.GexY;

        var newGex = new MapGex(x, y);
        newGex.Position = GetMapPositionByGex(x, y);
        Gex[x, y] = newGex;
    }

    #region search route
    /// <summary>
    /// Поиск пути.
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="finishX"></param>
    /// <param name="finishY"></param>
    public List<MapGexPos> SearchRouteWorkGex(Int32 startX, Int32 startY, Int32 finishX, Int32 finishY)
    {
        var arrWay = new List<MapGexPos>();
        Int32 num = 0;
        // цикл присваивает массиву _workGex значения: 0 = проходимо; -1 = нет
        for (Int32 x = 0; x < GexMaxX; x++)
        {
            for (Int32 y = 0; y < GexMaxY; y++)
            {
                var gex = Gex[x, y];
                if (gex.IsBlocked || gex.City != null) //?++ планеты - препятствия
                {
                    _workGex[x, y] = -1;
                }
                else
                {
                    _workGex[x, y] = 0;
                    num++;
                }
            }
        }
        // откуда, не препятствие
        _workGex[startX, startY] = 0;
        // куда ищем путь присваиваем 1
        _workGex[finishX, finishY] = 1;

        // элеменентам массива, начиная от цели(элемнт 1) веером во все стороны, пока не встретится преграда, присваиваем числа, значения которых на единицу большие, чем у соседнего элемента
        var colBreaked = false;
        for (Int32 val = 1; val < num + 1; val++)
        {
            colBreaked = false;
            for (Int32 col = 0; col < GexMaxY; col++)
            {
                for (Int32 row = 0; row < GexMaxX; row++)
                {
                    var isNormal = col%2 == 0;  // четные ряды своя логика, нечетные - своя
                    if (_workGex[row, col] == val)
                    {
                        if (col != GexMaxY - 1 && _workGex[row, col + 1] == 0) { _workGex[row, col + 1] = val + 1; }
                        if (col != 0 && _workGex[row, col - 1] == 0) { _workGex[row, col - 1] = val + 1; }
                        if (row != GexMaxX - 1 && _workGex[row + 1, col] == 0) { _workGex[row + 1, col] = val + 1; }
                        if (row != 0 && _workGex[row - 1, col] == 0) { _workGex[row - 1, col] = val + 1; }
                        // добавить подиагонали - как для гексов
                        if (isNormal)
                        {
                            if (row != 0 && col != 0 && _workGex[row - 1, col - 1] == 0) { _workGex[row - 1, col - 1] = val + 1; }              // лево низ
                            if (row != 0 && col != GexMaxY - 1 && _workGex[row - 1, col + 1] == 0) {  _workGex[row - 1, col + 1] = val + 1; }   // лево верх
                        }
                        else
                        {
                            if (row != GexMaxX - 1 && col != 0 && _workGex[row + 1, col - 1] == 0) { _workGex[row + 1, col - 1] = val + 1; }                // право низ
                            if (row != GexMaxX - 1 && col != GexMaxY - 1 && _workGex[row + 1, col + 1] == 0) { _workGex[row + 1, col + 1] = val + 1; }      // право верх
                        }
                    }
                    //if (colBreaked) break;
                }
                //if (colBreaked) break;
            }
        }



        // нахождение кратчайшего пути
        Int32 way = 0;
        Int32 wayLength = 0;
			
		//heroe.arrWay = [];
        
        way = ++_workGex[startX, startY];
        wayLength = --_workGex[startX, startY];

        //CurrenctCity.CityMap.MainLogicObject.Log.SetText(way.ToString());

        var xxSt = startX;
        var yySt = startY;

        //// включаем и начало
        //arrWay.Add(new MapGexPos(startX, startY));
        

        while (way > 1)
        {
            way--;
            var isNormal = yySt % 2 == 0;  // четные ряды своя логика, нечетные - своя

            // определение пути и добавляем его в массив
            if (xxSt != GexMaxX - 1 && _workGex[xxSt + 1, yySt] == _workGex[xxSt, yySt] - 1)
            {
                xxSt++;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
            else if (xxSt != 0 && _workGex[xxSt - 1,yySt] == _workGex[xxSt,yySt] - 1)
            {
                xxSt--;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
            else if (yySt != GexMaxY - 1 && _workGex[xxSt,yySt + 1] == _workGex[xxSt,yySt] - 1)
            {
                yySt++;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
            else if (yySt != 0 && _workGex[xxSt,yySt - 1] == _workGex[xxSt,yySt] - 1)
            {
                yySt--;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
            // подиагонали - как для гексов
            else if (!isNormal && yySt != 0 && xxSt != GexMaxX - 1 && _workGex[xxSt + 1, yySt - 1] == _workGex[xxSt, yySt] - 1) // право низ
            {
                yySt--;
                xxSt++;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
            else if (!isNormal && yySt != GexMaxY - 1 && xxSt != GexMaxX - 1 && _workGex[xxSt + 1, yySt + 1] == _workGex[xxSt, yySt] - 1) // право верх
            {
                yySt++;
                xxSt++;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
            else if (isNormal && yySt != GexMaxY - 1 && xxSt != 0 && _workGex[xxSt - 1, yySt + 1] == _workGex[xxSt, yySt] - 1)  // лево верх
            {
                yySt++;
                xxSt--;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
            else if (isNormal && yySt != 0 && xxSt != 0 && _workGex[xxSt - 1, yySt - 1] == _workGex[xxSt, yySt] - 1)  // лево низ
            {
                yySt--;
                xxSt--;
                arrWay.Add(new MapGexPos(xxSt, yySt));
            }
        }

        //// если есть путь (включаем и начало)
        //if (arrWay.Any())
        //{
        //    arrWay.Add(new MapGexPos(startX, startY));
        //}
        return arrWay;
    }
    #endregion


    private Int32 WrapGexX(Int32 x)
    {
        return x < 0 ? 0 : x > GexMaxX ? GexMaxX : x;
    }
    private Int32 WrapGexY(Int32 y)
    {
        return y < 0 ? 0 : y > GexMaxY ? GexMaxY : y;
    }

    /// <summary>
    /// Позиция на карте по координатам гекса.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector2 GetMapPositionByGex(Int32 x, Int32 y)
    {
        const float gexWidth = 1.908f;//2.12f;
        const float gexHeight = 2.12f;
        var rowOffsetX = gexWidth/2;
        var rowOffsetY = gexHeight/4;
        // позиция гекса
        var posX = (x*gexWidth) + (rowOffsetX*(y%2==0?0:1));    // нечетный ряд смещаем
        var posY = (y*gexHeight) - (rowOffsetY*y);
        var pos = new Vector2(posX, posY);
        //?+ смещение на глобальной карте (левый верхний угол)


        return pos;
    }
    #endregion



    /// <summary>
    /// Ресурс в наличии.
    /// </summary>
    /// <param name="resType"></param>
    /// <returns></returns>
    public CityResourceShip GetSelfResource(CityRecources resType)
    {
        return Resources.FirstOrDefault(f => f.Type == resType);
    }

    public void SetCurrentCity(City city)
    {
        CurrenctCity = city;
        _currentCityId = city.Model.Id;
    }
    public void SetNextCity(City city)
    {
        if (city == null || city.Model == null)
        {
            NextCity = CurrenctCity;
            return;
        }

        NextCity = city;
        _currentNextCityId = city.Model.Id;
    }

    /// <summary>
    /// Прошел день.
    /// </summary>
    public void AddDay()
    {
        Day++;
        foreach (var city in Cities)
        {
            if (city.Model.IsJopAndCompleted()) continue;
            city.NextDay();
        }
    }

    /// <summary>
    /// Обновление вида за сутки.
    /// </summary>
    public void UpdateViewDay()
    {
        foreach (var city in Cities)
        {
            if (city.Model.IsJopAndCompleted()) continue;
            city.UpdateView();
        }
    }

    /// <summary>
    /// Статус уровня.
    /// </summary>
    public void SetLevelStatus(FarStatusLevel status)
    {
        LevelStatus = status;
    }


    /// <summary>
    /// Добавление материала.
    /// </summary>
    /// <param name="count"></param>
    public void AddMaterial(Int32 count)
    {
        var mat = Resources.FirstOrDefault(f => f.Type == CityRecources.Material);
        if (mat == null) return;
        mat.CurrentCount += count;
    }

    /// <summary>
    /// Сбор ресурса.
    /// </summary>
    /// <param name="res"></param>
    public void ResourceCollect(CityResourceProduct res)
    {
        var selfRes = Resources.FirstOrDefault(f => f.Type == res.Type);
        if (selfRes == null) return;

        selfRes.CurrentCount += res.CurrentCount;
        res.CurrentCount = 0;
    }

    /// <summary>
    /// Есть возможность запустить процесс в городе. Достаточно ресурсов и есть слот.
    /// </summary>
    /// <param name="cityModel"></param>
    /// <returns></returns>
    public Boolean ShipCanStartProductInCity(CityModel cityModel)
    {
        if (cityModel.CityType != CityType.Friend) return false;
        // если есть слот для запуска
        if (!cityModel.ResourceProduct.CanProccess()) return false;
        // если достаточно для запуска
        if (!ShipHaveEnoughForCity(cityModel)) return false;
        return true;
    }
    /// <summary>
    /// Заряжаем город.
    /// </summary>
    /// <param name="cityModel"></param>
    public void CityStart(CityModel cityModel)
    {
        // если есть возможность
        if (!ShipCanStartProductInCity(cityModel)) return;
        
        // тратим ресурсы
        foreach (var res in cityModel.ResourceProduct.Resources)
        {
            var selfRes = GetSelfResource(res.Type);
            if (selfRes == null || selfRes.CurrentCount <= 0) continue;
            //- добавляем сколько НАДО
            var mustBe = res.MustBeForProduct;
            var selfHave = selfRes.CurrentCount;
            if (mustBe > selfHave) continue;    // только если есть сколько надо для цикла
            //var toAddCount = mustBe <= selfHave ? mustBe : selfHave;
            //- add
            //res.CurrentCount += toAddCount;
            selfRes.CurrentCount -= mustBe;
        }

        // запускаем
        cityModel.ResourceProduct.StartProccess();
    }
    /// <summary>
    /// Достаточно ресурсов для запуска производства в городе.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    public Boolean ShipHaveEnoughForCity(City city)
    {
        return ShipHaveEnoughForCity(city.Model);
    }
    public Boolean ShipHaveEnoughForCity(CityModel cityModel)
    {
        foreach (var res in cityModel.ResourceProduct.Resources)
        {
            if (!IsResourceEnough(res)) return false;
        }
        return true;
    }



    /// <summary>
    /// Выполнение квеста в городе.
    /// </summary>
    /// <param name="cityModel"></param>
    public void QuestCityStart(CityModel cityModel)
    {
        // если есть возможность (уже проверили на панели)
        //if (!ShipHaveEnoughForQuestCity(cityModel)) return;

        // тратим ресурсы
        foreach (var res in cityModel.QuestResources)
        {
            var selfRes = GetSelfResource(res.Type);
            if (selfRes == null || selfRes.CurrentCount <= 0) continue;
            //- добавляем сколько НАДО
            var mustBe = res.MustBeForProduct;
            var selfHave = selfRes.CurrentCount;
            if (mustBe > selfHave) continue;    // только если есть сколько надо для цикла
            //var toAddCount = mustBe <= selfHave ? mustBe : selfHave;
            //- add
            //res.CurrentCount += toAddCount;
            selfRes.CurrentCount -= mustBe;
        }
    }
    /// <summary>
    /// Выполнение квеста в JOP.
    /// </summary>
    /// <param name="cityModel"></param>
    public void QuestJopStart(CityModel cityModel)
    {
        var jop = cityModel.GetCurrentJopQuestList();
        foreach (var res in jop)
        {
            var selfRes = GetSelfResource(res.Type);
            if (selfRes == null || selfRes.CurrentCount <= 0) continue;
            //- добавляем сколько НАДО
            var mustBe = res.MustBeForProduct;
            var selfHave = selfRes.CurrentCount;
            if (mustBe > selfHave) continue;    // только если есть сколько надо для цикла
            //var toAddCount = mustBe <= selfHave ? mustBe : selfHave;
            //- add
            //res.CurrentCount += toAddCount;
            selfRes.CurrentCount -= mustBe;
        }
    }

    /// <summary>
    /// Достаточно ресурсов для выполнения квеста.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    public Boolean ShipHaveEnoughForQuestCity(City city)
    {
        return city.Model.IsJop ? ShipHaveEnoughForQuestJop(city.Model) : ShipHaveEnoughForQuestCity(city.Model);
    }
    public Boolean ShipHaveEnoughForQuestCity(CityModel cityModel)
    {
        foreach (var res in cityModel.QuestResources)
        {
            if (!IsResourceEnough(res)) return false;
        }
        return true;
    }
    public Boolean ShipHaveEnoughForQuestJop(CityModel cityModel)
    {
        var jop = cityModel.GetCurrentJopQuestList();
        foreach (var res in jop)
        {
            if (!IsResourceEnough(res)) return false;
        }
        return true;
    }

    /// <summary>
    /// Достаточно ли ресурса в наличии.
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    public Boolean IsResourceEnough(CityResourceFrom res)
    {
        var selfRes = GetSelfResource(res.Type);
        if (selfRes == null) return false;

        //var mustRes = res.GetCheckEnough();
        var mustRes = res.MustBeForProduct;

        var enough = mustRes <= 0 || selfRes.CurrentCount >= mustRes;
        if (!enough) return false;

        return true;
    }
    /// <summary>
    /// Сбор ресурсов у текущего города.
    /// </summary>
    public void CurrentResourceCollect()
    {
        if (this.CurrenctCity == null) return;
        this.ResourceCollect(this.CurrenctCity.Model.ResourceProduct);
    }



    /// <summary>
    /// Закончен уровень.
    /// <remarks>Выроботка ресурсов за день, сбор ресурсов и прочее.</remarks>
    /// </summary>
    /// <param name="isWin"></param>
    public void LevelEnd(Boolean isWin)
    {
        LevelStatus = FarStatusLevel.Init;

        //+ следующий день (подсчет ресурсов)
        this.AddDay();

        if (isWin)
        {
            //- city
            this.SetCurrentCity(this.NextCity);
        }
        else
        {

        }

        //- собираем
        CurrentResourceCollect();

        //+ пост обновление вида (после всех расчетов)
        this.UpdateViewDay();
    }

    

}
