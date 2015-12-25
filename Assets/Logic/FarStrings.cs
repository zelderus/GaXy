using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using ZelderFramework;




public enum FarText
{
    Map_SkillTitle  = 100
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
        AddText(FarText.Map_SkillTitle, "Навыки", "Skills");


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
