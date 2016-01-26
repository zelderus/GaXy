using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using ZelderFramework;




public enum FarText
{
    Main_Title = 1,
    Main_ResetText = 10,
    Main_ResetEndText = 11,
    Main_Sound = 12,
    Main_LanguageRu = 13,
    Main_LanguageEn = 14, //?
    
    
    Map_SkillTitle  = 1000,
    Map_InfoWnd_DayTitle = 1010,
    Map_InfoWnd_ResTitle = 1011,
    Map_InfoWnd_ProdTitle = 1012,
    Map_InfoWnd_NeutralTitle = 1021,
    Map_InfoWnd_ActiveTitle = 1022,

    Map_Help_WelcomeTitle       = 1501,
    Map_Help_WelcomeDesc        = 1502,

    Map_Help_HelpTitle = 1601,
    Map_Help_HelpDesc1 = 1602,
    Map_Help_HelpDesc2 = 1603,
    Map_Help_HelpDesc3 = 1604,
    Map_Help_HelpDesc4 = 1605,

    Level_Win   = 2000,
    Level_Lose  = 2001,
    Level_MarketTitle   = 2010,
    Level_Pause         = 2011

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
        //AddText(FarText.Main_Title, "Игра полностью бесплатная и открытая. Используйте ее в равлекательных или позновательных целях, или как вам вздумается", "Full free, open game for fun, for education or what you want for");
        AddText(FarText.Main_Title, "космическая леталка", "space fly");
        AddText(FarText.Main_ResetText, "Нажмите и держите, чтобы сбросить прогресс игры.", "Hold the AtomButton to Reset the game progress.");
        AddText(FarText.Main_ResetEndText, "Прогресс игры сброшен.", "The game progress has been reseted.");
        AddText(FarText.Main_Sound, "ЗВУК", "SOUND");
        AddText(FarText.Main_LanguageRu, "РУССКИЙ", "RUSSIAN");
        AddText(FarText.Main_LanguageEn, "АНГЛИЙСКИЙ", "ENGLISH");

        //+ MAP
        AddText(FarText.Map_SkillTitle, "Навыки", "Skills");
        AddText(FarText.Map_InfoWnd_DayTitle, "Дней на производство", "Days in the production");
        AddText(FarText.Map_InfoWnd_ResTitle, "Необходимые ресурсы", "Necessary resources");
        AddText(FarText.Map_InfoWnd_ProdTitle, "Производство", "Production");
        AddText(FarText.Map_InfoWnd_NeutralTitle, "Необходимые ресурсы", "Necessary resources");
        AddText(FarText.Map_InfoWnd_ActiveTitle, "Активировать", "Activate");
        //- welcome
        AddText(FarText.Map_Help_WelcomeTitle, "Добро пожаловать!", "Welcome!");
        AddText(FarText.Map_Help_WelcomeDesc, 
@"В этой игре Вам предстоит путешествовать от планеты к планете, от планеты к планете, и еще дальше. 

Планеты бывают разного типа. Из-за жесткой нехватки во вселенной природных ресурсов, разные планеты умеют производить некоторый ресурс. Для его производства необходимы материалы с других планет.

Собственно, Вам предстоит доставлять необходимое для планет.

Вперёд!",
@"In this game you have to travel from planet to planet, from planet to planet, and beyond.

The planets are of different types. Due to the lack of hard in the universe of natural resources, are able to produce different planets some resource. For its production need materials from other planets.

Actually, you will be required to deliver to the planets.

Let's rock!");
        //- help
        AddText(FarText.Map_Help_HelpTitle, "Легенда", "Legend");
        AddText(FarText.Map_Help_HelpDesc1, "В зеленом кружке, обозначено сколько ресурсов уже доступно", "The green circle is indicated how many resources already available");
        AddText(FarText.Map_Help_HelpDesc2, "Если под планетой есть желтый кружок, значит достаточно ресуров для нее", "If a planet has a yellow circle, then enough resources for it");
        AddText(FarText.Map_Help_HelpDesc3, "Если под планетой красный кружок, значит НЕдостаточно ресуров", "If a planet has a red circle that means insufficient resources");
        AddText(FarText.Map_Help_HelpDesc4, "Никто не знает, что будет, если ему обеспечить все необходимое", "Nobody knows what will happen if provide all the necessary to him");

        //+ LEVEL
        AddText(FarText.Level_Win, "Уровень пройден!", "You WIN!");
        AddText(FarText.Level_Lose, "Поражение", "LOSE");
        AddText(FarText.Level_MarketTitle, "Покупка бонусов", "Purchase bonuses");
        AddText(FarText.Level_Pause, "Пауза", "Pause");

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
                case 6: return "Основное оружие";
                case 7: return "Основное оружие";
                case 8: return "Основное оружие";
                case 9: return "Основное оружие";
                case 10: return "Основное оружие";
                case 11: return "Двойной ствол";
                case 12: return "Двойной ствол";
                case 13: return "Двойной ствол";
                case 14: return "Двойной ствол";
                case 15: return "Двойной ствол";
                case 16: return "Двойной ствол";
                case 17: return "Двойной ствол";
                case 18: return "Двойной ствол";
                case 19: return "Двойной ствол";
                case 20: return "Двойной ствол";
                case 21: return "Веерное оружие";
                case 22: return "Веерное оружие";
                case 23: return "Веерное оружие";
                case 24: return "Веерное оружие";
                case 25: return "Веерное оружие";
                case 26: return "Веерное оружие";
                case 27: return "Веерное оружие";
                case 28: return "Веерное оружие";
                case 29: return "Веерное оружие";
                case 30: return "Веерное оружие";

                case 51: return "Броня корабля при полете";
                case 52: return "Броня корабля при полете";
                case 53: return "Броня корабля при полете";
                case 54: return "Броня корабля при полете";
                case 55: return "Броня корабля при полете";
                case 56: return "Скорость перемещения корабля при полете";
                case 57: return "Скорость перемещения корабля при полете";
                case 58: return "Скорость перемещения корабля при полете";
                case 59: return "Скорость перемещения корабля при полете";
                case 60: return "Скорость перемещения корабля при полете";
                case 61: return "Дальность перелета корабля между планетами";
                case 62: return "Дальность перелета корабля между планетами";
                case 63: return "Дальность перелета корабля между планетами";
                case 64: return "Дальность перелета корабля между планетами";
                case 65: return "Дальность перелета корабля между планетами";

                case 101: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 102: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 103: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 104: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 105: return "Бонус приобретение перед полетом: Пополнение энергии";
                case 106: return "Бонус приобретение перед полетом: Неуязвимость";
                case 107: return "Бонус приобретение перед полетом: Неуязвимость";
                case 108: return "Бонус приобретение перед полетом: Неуязвимость";
                case 109: return "Бонус приобретение перед полетом: Неуязвимость";
                case 110: return "Бонус приобретение перед полетом: Неуязвимость";
                case 111: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 112: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 113: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 114: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
                case 115: return "Бонус приобретение перед полетом: Большой прибольшой Бум";
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
                case 6: return "Main gun";
                case 7: return "Main gun";
                case 8: return "Main gun";
                case 9: return "Main gun";
                case 10: return "Main gun";
                case 11: return "Double gun";
                case 12: return "Double gun";
                case 13: return "Double gun";
                case 14: return "Double gun";
                case 15: return "Double gun";
                case 16: return "Double gun";
                case 17: return "Double gun";
                case 18: return "Double gun";
                case 19: return "Double gun";
                case 20: return "Double gun";
                case 21: return "Fun gun";
                case 22: return "Fun gun";
                case 23: return "Fun gun";
                case 24: return "Fun gun";
                case 25: return "Fun gun";
                case 26: return "Fun gun";
                case 27: return "Fun gun";
                case 28: return "Fun gun";
                case 29: return "Fun gun";
                case 30: return "Fun gun";

                case 51: return "Armor of the ship during the flight";
                case 52: return "Armor of the ship during the flight";
                case 53: return "Armor of the ship during the flight";
                case 54: return "Armor of the ship during the flight";
                case 55: return "Armor of the ship during the flight";
                case 56: return "Traversing speed of the ship during the flight";
                case 57: return "Traversing speed of the ship during the flight";
                case 58: return "Traversing speed of the ship during the flight";
                case 59: return "Traversing speed of the ship during the flight";
                case 60: return "Traversing speed of the ship during the flight";
                case 61: return "Range of flight of the ship between the planets";
                case 62: return "Range of flight of the ship between the planets";
                case 63: return "Range of flight of the ship between the planets";
                case 64: return "Range of flight of the ship between the planets";
                case 65: return "Range of flight of the ship between the planets";

                case 101: return "Bonus acquisition before the flight: Replenishing energy";
                case 102: return "Bonus acquisition before the flight: Replenishing energy";
                case 103: return "Bonus acquisition before the flight: Replenishing energy";
                case 104: return "Bonus acquisition before the flight: Replenishing energy";
                case 105: return "Bonus acquisition before the flight: Replenishing energy";
                case 106: return "Bonus acquisition before the flight: Shield";
                case 107: return "Bonus acquisition before the flight: Shield";
                case 108: return "Bonus acquisition before the flight: Shield";
                case 109: return "Bonus acquisition before the flight: Shield";
                case 110: return "Bonus acquisition before the flight: Shield";
                case 111: return "Bonus acquisition before the flight: Big Boom";
                case 112: return "Bonus acquisition before the flight: Big Boom";
                case 113: return "Bonus acquisition before the flight: Big Boom";
                case 114: return "Bonus acquisition before the flight: Big Boom";
                case 115: return "Bonus acquisition before the flight: Big Boom";
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
