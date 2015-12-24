using System;
using UnityEngine;
using System.Collections;
using ZelderFramework;



/// <summary>
/// Основная логика игры.
/// </summary>
public static class FarLife
{
    public static GameLife GameLife { private set; get; }
    public static ShipLife ShipLife { private set; get; }
    public static GameLanguages Language { private set; get; }
    public static MapLife MapLife { private set; get; }
    //public static PlayerData PlayerData { private set; get; }
    public static Boolean SoundEnabled { private set; get; }
    

    private static bool _inited = false;
    /// <summary>
    /// Инициализация движка. Необходимо выполнить в первой сцене в Awake().
    /// </summary>
    public static void Init()
    {
        if (_inited) return;

        if (GameLife == null)
        {
            GameLife = new GameLife();
            GameLife.Init("Images/LoadingTxt");
        }

        ShipLife = new ShipLife();
        ShipLife.Init();
        ShipLife.Load();

        MapLife = new MapLife();
        MapLife.Init(ShipLife);
        MapLife.Load();

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


    /// <summary>
    /// Сохранение.
    /// </summary>
    public static void SaveGame()
    {
        // TODO

    }



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
