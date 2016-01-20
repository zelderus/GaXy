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
                case 11: return "Веерное оружие";
                case 12: return "Веерное оружие";
                case 13: return "Веерное оружие";
                case 14: return "Веерное оружие";
                case 15: return "Веерное оружие";
                case 16: return "Броня корабля при полете";
                case 17: return "Броня корабля при полете";
                case 18: return "Броня корабля при полете";
                case 19: return "Броня корабля при полете";
                case 20: return "Броня корабля при полете";
                case 21: return "Скорость перемещения корабля при полете";
                case 22: return "Скорость перемещения корабля при полете";
                case 23: return "Скорость перемещения корабля при полете";
                case 24: return "Скорость перемещения корабля при полете";
                case 25: return "Скорость перемещения корабля при полете";
                case 26: return "Дальность перелета корабля между планетами";
                case 27: return "Дальность перелета корабля между планетами";
                case 28: return "Дальность перелета корабля между планетами";
                case 29: return "Дальность перелета корабля между планетами";
                case 30: return "Дальность перелета корабля между планетами";
                case 31: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 32: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 33: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 34: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 35: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 36: return "Бонус приобретение перед полетом: Неуязвимость";
                case 37: return "Бонус приобретение перед полетом: Неуязвимость";
                case 38: return "Бонус приобретение перед полетом: Неуязвимость";
                case 39: return "Бонус приобретение перед полетом: Неуязвимость";
                case 40: return "Бонус приобретение перед полетом: Неуязвимость";
                case 41: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 42: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 43: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 44: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 45: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
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
                case 11: return "Fun gun";
                case 12: return "Fun gun";
                case 13: return "Fun gun";
                case 14: return "Fun gun";
                case 15: return "Fun gun";
                case 16: return "Armor of the ship during the flight";
                case 17: return "Armor of the ship during the flight";
                case 18: return "Armor of the ship during the flight";
                case 19: return "Armor of the ship during the flight";
                case 20: return "Armor of the ship during the flight";
                case 21: return "Traversing speed of the ship during the flight";
                case 22: return "Traversing speed of the ship during the flight";
                case 23: return "Traversing speed of the ship during the flight";
                case 24: return "Traversing speed of the ship during the flight";
                case 25: return "Traversing speed of the ship during the flight";
                case 26: return "Range of flight of the ship between the planets";
                case 27: return "Range of flight of the ship between the planets";
                case 28: return "Range of flight of the ship between the planets";
                case 29: return "Range of flight of the ship between the planets";
                case 30: return "Range of flight of the ship between the planets";
                case 31: return "Bonus acquisition before the flight: Replenishing energy";
                case 32: return "Bonus acquisition before the flight: Replenishing energy";
                case 33: return "Bonus acquisition before the flight: Replenishing energy";
                case 34: return "Bonus acquisition before the flight: Replenishing energy";
                case 35: return "Bonus acquisition before the flight: Replenishing energy";
                case 36: return "Bonus acquisition before the flight: Shield";
                case 37: return "Bonus acquisition before the flight: Shield";
                case 38: return "Bonus acquisition before the flight: Shield";
                case 39: return "Bonus acquisition before the flight: Shield";
                case 40: return "Bonus acquisition before the flight: Shield";
                case 41: return "Bonus acquisition before the flight: Big Boom";
                case 42: return "Bonus acquisition before the flight: Big Boom";
                case 43: return "Bonus acquisition before the flight: Big Boom";
                case 44: return "Bonus acquisition before the flight: Big Boom";
                case 45: return "Bonus acquisition before the flight: Big Boom";
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
