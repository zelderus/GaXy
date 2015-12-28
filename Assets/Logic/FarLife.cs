using System;
using System.Collections.Generic;
using Assets.Scripts.ZelderFramework.FileSystem;
using UnityEngine;
using System.Collections;
using ZelderFramework;



public class FarLifeGlobalData : FileManagedClass
{
    public Int32 FileVersion = 12;
    public Int32 FileVersionLoaded = 0;
    public Boolean IsNewGame = false;

    public FarLifeGlobalData()
    {
        
    }

    /// <summary>
    /// Данные для сохранения.
    /// </summary>
    /// <returns></returns>
    public override List<FileManagerData> ConvertToSaveData()
    {
        var datas = new List<FileManagerData>();
        datas.Add(new FileManagerData(FileManagerTypes.Int32, FileVersion));
        datas.Add(new FileManagerData(FileManagerTypes.Boolean, IsNewGame));
        return datas;
    }
    /// <summary>
    /// Загрузка данных.
    /// </summary>
    /// <param name="datas"></param>
    public override void LoadFromSaveData(List<FileManagerData> datas)
    {
        var ind = 0;
        FileVersionLoaded = (Int32)datas[ind++].DataValue;
        IsNewGame = (Boolean)datas[ind++].DataValue;
    }
}

/// <summary>
/// Основная логика игры.
/// </summary>
public static class FarLife
{
    public static FarLifeGlobalData GlobalData { private set; get; }
    public static GameLife GameLife { private set; get; }
    public static ShipLife ShipLife { private set; get; }
    public static GameLanguages Language { private set; get; }
    public static MapLife MapLife { private set; get; }
    //public static PlayerData PlayerData { private set; get; }
    public static Boolean SoundEnabled { private set; get; }
    public static FarStrings Strings { private set; get; }

    private static bool _inited = false;
    /// <summary>
    /// Инициализация движка. Необходимо выполнить в первой сцене в Awake().
    /// </summary>
    public static void Init()
    {
        if (_inited) return;
        
        GlobalData = new FarLifeGlobalData();

        if (GameLife == null)
        {
            GameLife = new GameLife();
            GameLife.Init("Images/LoadingTxt");
        }

        Strings = new FarStrings();
        Strings.Init();

        ShipLife = new ShipLife();
        ShipLife.Init();

        MapLife = new MapLife();
        MapLife.Init(ShipLife);

        //! LOAD
        LoadGame();

        //if (GameLogic == null)
        //{
        //    GameLogic = new GameLogic();
        //}
        //+ options
        InitLanguage();
        InitSound();

        //+ data
        //if (PlayerData == null)
        //{
        //    PlayerData = new PlayerData();
        //}
        //PlayerData.LoadData();
        //GameLogic.SetPlayerData(PlayerData);
        //MapLife.Load

        _inited = true;
    }
    /// <summary>
    /// Инициализация для каждой сцены.
    /// <remarks>Вызывать в каждой сцене в Start()</remarks>
    /// </summary>
    /// <param name="onBackPress"></param>
    public static void ScreenInit(Action onBackPress)
    {
        GameLife.ScreenInit(onBackPress);
    }


    private static Int32 _fileVersion = 10;
    /// <summary>
    /// Загрузка.
    /// </summary>
    public static void LoadGame()
    {
        FileManager.Load("gld.zld", new List<FileManagedClass>() { GlobalData });
        if (GlobalData.FileVersionLoaded < GlobalData.FileVersion)
        {
            FileManager.Delete("gld.zld");
            FileManager.Delete("sl.zld");
            FileManager.Delete("ml.zld");
            return;
        }
        if (!GlobalData.IsNewGame)
        {
            FileManager.Load("sl.zld", new List<FileManagedClass>() {ShipLife});
            FileManager.Load("ml.zld", new List<FileManagedClass>() {MapLife});
        }
    }

    /// <summary>
    /// Сохранение.
    /// </summary>
    public static void SaveGame()
    {
        GlobalData.IsNewGame = false;

        FileManager.Save("gld.zld", new List<FileManagedClass>() { GlobalData });
        FileManager.Save("sl.zld", new List<FileManagedClass>() { ShipLife });
        FileManager.Save("ml.zld", new List<FileManagedClass>() { MapLife });
    }

    public static void SaveGlobalOnly()
    {
        FileManager.Save("gld.zld", new List<FileManagedClass>() { GlobalData });
    }

    #region Strings
    /// <summary>
    /// Текст.
    /// </summary>
    /// <param name="strKey"></param>
    /// <returns></returns>
    public static String GetText(FarText strKey)
    {
        return Strings.GetText(Language, strKey);
    }
    #endregion

    #region Options
    /// <summary>
    /// Определение языка.
    /// </summary>
    private static void InitLanguage()
    {
        Language = GameLanguages.Russian;

        var lngInt = UnityEngine.PlayerPrefs.GetInt("Lng", -1);
        if (lngInt > 0)
        {
            Language = (GameLanguages)lngInt;
        }
        else
        {
            //+ определение по системе
            Language = GameLife.Platforms.GetDeviceLanguage();
        }
    }
    /// <summary>
    /// Установка языка.
    /// </summary>
    /// <param name="language"></param>
    public static void SetLanguage(GameLanguages language)
    {
        Language = language;
        UnityEngine.PlayerPrefs.SetInt("Lng", (Int32)Language);
    }

    /// <summary>
    /// Инит звука.
    /// </summary>
    private static void InitSound()
    {
        SoundEnabled = true;
        var enInt = UnityEngine.PlayerPrefs.GetInt("Snd", 1);
        SoundEnabled = enInt == 1;
        SetAudioSound();
    }
    /// <summary>
    /// Включение/выключение звука.
    /// </summary>
    public static void SetSound(Boolean enabled)
    {
        SoundEnabled = enabled;
        UnityEngine.PlayerPrefs.SetInt("Snd", SoundEnabled ? 1 : 0);
        SetAudioSound();
    }

    private static void SetAudioSound()
    {
        AudioListener.volume = SoundEnabled ? 1.0f : 0.0f;
    }
    #endregion

    #region Screens
    /// <summary>
    /// Главное меню.
    /// </summary>
    public static void GoToMenu()
    {
        GameLife.GoToScreen(0);
    }
    /// <summary>
    /// На карту.
    /// </summary>
    public static void GoToMap()
    {
        GameLife.GoToScreen(1);
    }
    /// <summary>
    /// Запуск уровня.
    /// </summary>
    public static void GoToLevel()
    {
        GameLife.GoToScreen(2);
    }
    #endregion



    /// <summary>
    /// Вызывает каждое окно по завершении загрузки.
    /// </summary>
    public static void OnScreenLoaded()
    {
        GameLife.OnScreenLoaded();
    }

    /// <summary>
    /// Обновление игры.
    /// </summary>
    public static void Update()
    {
        GameLife.Update(Time.deltaTime);
    }
    /// <summary>
    /// Рендер движка.
    /// <remarks>Вызывать в каждой сцене.</remarks>
    /// </summary>
    public static void OnGUI()
    {
        GameLife.OnGUI();
    }
	
}
