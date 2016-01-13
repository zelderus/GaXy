using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using ZelderFramework;




public enum FarText
{
    Main_ResetText = 10,
    Main_ResetEndText = 11,
    Main_Sound = 12,
    Main_LanguageRu = 13,
    Main_LanguageEn = 14, //?
    
    Map_SkillTitle  = 1000,



    Level_Win   = 2000,
    Level_Lose  = 2001
}

/// <summary>
/// Текста.
/// </summary>
public class FarStrings
{

    //private readonly Dictionary<GameLanguages, List<FarStringValue>> _strings = new Dictionary<GameLanguages, List<FarStringValue>>();

    private List<FarStringValue> _rusTextes = new List<FarStringValue>();
    private List<FarStringValue> _engTextes = new List<FarStringValue>();

    public FarStrings()
    {
        
    }


    public void Init()
    {
        //! rus
        _rusTextes = new List<FarStringValue>();
        //_strings.Add(GameLanguages.Russian, _rusTextes);
        //! eng
        _engTextes = new List<FarStringValue>();
        //_strings.Add(GameLanguages.English, _engTextes);

        //! +++
        //+ MAIN
        AddText(FarText.Main_ResetText, "Нажмите и держите, чтобы сбросить прогресс игры.", "Hold the AtomButton to Reset the game progress.");
        AddText(FarText.Main_ResetEndText, "Прогресс игры сброшен.", "The game progress has been reseted.");
        AddText(FarText.Main_Sound, "ЗВУК", "SOUND");
        AddText(FarText.Main_LanguageRu, "РУССКИЙ", "RUSSIAN");
        AddText(FarText.Main_LanguageEn, "АНГЛИЙСКИЙ", "ENGLISH");

        //+ MAP
        AddText(FarText.Map_SkillTitle, "Навыки", "Skills");

        //+ LEVEL
        AddText(FarText.Level_Win, "Уровень пройден!", "You WIN!");
        AddText(FarText.Level_Lose, "Поражение", "LOSE");

    }




    private void AddText(FarText key, String rusText, String engText)
    {
        _rusTextes.Add(new FarStringValue(key, rusText));
        _engTextes.Add(new FarStringValue(key, engText));
    }

    
    /// <summary>
    /// Текста.
    /// </summary>
    /// <param name="lng"></param>
    /// <param name="strKey"></param>
    /// <returns></returns>
    public String GetText(GameLanguages lng, FarText strKey)
    {
        var strs = _rusTextes;
        switch (lng)
        {
            case GameLanguages.Russian: strs = _rusTextes; break;
            case GameLanguages.English: strs = _engTextes; break;
        }
        var s = strs.FirstOrDefault(f => f.Key.Equals(strKey));
        if (s != null) return s.Text;
        return "!BADSTR!";
    }
}


public class FarStringValue
{
    public FarText Key { get; set; }
    public String Text { get; set; }

    public FarStringValue()
    {
        
    }
    public FarStringValue(FarText key, String text)
    {
        Key = key;
        Text = text;
    }
}
