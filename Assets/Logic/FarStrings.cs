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

        //+ SKILLS
        //- отдельно в своем методе GetSkillText()

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

    /// <summary>
    /// Текст скиллов.
    /// </summary>
    /// <param name="lng"></param>
    /// <param name="strKey"></param>
    /// <returns></returns>
    public String GetSkillText(GameLanguages lng, Int32 skillNum)
    {
        if (lng == GameLanguages.Russian)
        {
            switch (skillNum)
            {
                case 1: return "Основное оружие";
                case 2: return "Основное оружие";
                case 3: return "Основное оружие";
                case 4: return "Основное оружие";
                case 5: return "Основное оружие";
                case 6: return "Двойной ствол";
                case 7: return "Двойной ствол";
                case 8: return "Двойной ствол";
                case 9: return "Двойной ствол";
                case 10: return "Двойной ствол";
                case 11: return "скииииил....";
                case 12: return "скииииил....";
                case 13: return "скииииил....";
                case 14: return "скииииил....";
                case 15: return "скииииил....";
                case 16: return "скииииил....";
                case 17: return "скииииил....";
                case 18: return "скииииил....";
                case 19: return "скииииил....";
                case 20: return "скииииил....";
                case 21: return "скииииил....";
                case 22: return "скииииил....";
                case 23: return "скииииил....";
                case 24: return "скииииил....";
                case 25: return "скииииил....";
                case 26: return "скииииил....";
                case 27: return "скииииил....";
                case 28: return "скииииил....";
                case 29: return "скииииил....";
                case 30: return "скииииил....";
                case 31: return "скииииил....";
                case 32: return "скииииил....";
                case 33: return "скииииил....";
                case 34: return "скииииил....";
                case 35: return "скииииил....";
                case 36: return "скииииил....";
                case 37: return "скииииил....";
                case 38: return "скииииил....";
                case 39: return "скииииил....";
                case 40: return "скииииил....";
                case 41: return "скииииил....";
                case 42: return "скииииил....";
                case 43: return "скииииил....";
                case 44: return "скииииил....";
                case 45: return "скииииил....";
                case 46: return "скииииил....";
                case 47: return "скииииил....";
                case 48: return "скииииил....";
                case 49: return "скииииил....";
                case 50: return "скииииил....";
                case 51: return "скииииил....";
                case 52: return "скииииил....";
                case 53: return "скииииил....";
                case 54: return "скииииил....";
            }
        }
        else if (lng == GameLanguages.English)
        {
            switch (skillNum)
            {
                case 1: return "Main gun";
                case 2: return "Main gun";
                case 3: return "Main gun";
                case 4: return "Main gun";
                case 5: return "Main gun";
                case 6: return "Double gun";
                case 7: return "Double gun";
                case 8: return "Double gun";
                case 9: return "Double gun";
                case 10: return "Double gun";
                case 11: return "skillllll....";
                case 12: return "skillllll....";
                case 13: return "skillllll....";
                case 14: return "skillllll....";
                case 15: return "skillllll....";
                case 16: return "skillllll....";
                case 17: return "skillllll....";
                case 18: return "skillllll....";
                case 19: return "skillllll....";
                case 20: return "skillllll....";
                case 21: return "skillllll....";
                case 22: return "skillllll....";
                case 23: return "skillllll....";
                case 24: return "skillllll....";
                case 25: return "skillllll....";
                case 26: return "skillllll....";
                case 27: return "skillllll....";
                case 28: return "skillllll....";
                case 29: return "skillllll....";
                case 30: return "skillllll....";
                case 31: return "skillllll....";
                case 32: return "skillllll....";
                case 33: return "skillllll....";
                case 34: return "skillllll....";
                case 35: return "skillllll....";
                case 36: return "skillllll....";
                case 37: return "skillllll....";
                case 38: return "skillllll....";
                case 39: return "skillllll....";
                case 40: return "skillllll....";
                case 41: return "skillllll....";
                case 42: return "skillllll....";
                case 43: return "skillllll....";
                case 44: return "skillllll....";
                case 45: return "skillllll....";
                case 46: return "skillllll....";
                case 47: return "skillllll....";
                case 48: return "skillllll....";
                case 49: return "skillllll....";
                case 50: return "skillllll....";
                case 51: return "skillllll....";
                case 52: return "skillllll....";
                case 53: return "skillllll....";
                case 54: return "skillllll....";
            }
        }
        return "!BADSKSTR!";
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
