using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework;

public class MenuOptionPanelLogic : MonoBehaviour {

    public Boolean IsShowed { get; private set; }

    public Text ResetTimerTxt;
    public Button ResetBtn;
    public Text ResetTxt;

    public UIToggleSpriteSwap SoundToggle;
    public UIToggleSpriteSwap LanguageRuToggle;
    public UIToggleSpriteSwap LanguageEnToggle;

    private MenuController _controller;

    // Use this for initialization
    void Start()
    {
        ResetBtn.gameObject.SetActive(!FarLife.GlobalData.IsNewGame);
        if (FarLife.GlobalData.IsNewGame) ResetTxt.gameObject.SetActive(false);

        SoundToggle.SetIsOnStyles(FarLife.SoundEnabled);
        SetLanguage(FarLife.Language);
    }

    public void Init(MenuController controller)
    {
        _controller = controller;
    }


    public void Hide()
    {
        IsShowed = false;
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        ResetProgressStop();
        IsShowed = true;
        this.gameObject.SetActive(true);
    }



    /// <summary>
    /// Установка текущего языка.
    /// </summary>
    /// <param name="lng"></param>
    public void SetLanguage(GameLanguages lng)
    {
        if (lng == GameLanguages.Russian)
        {
            //LanguageRuToggle.targetToggle.isOn = true;
            LanguageRuToggle.SetIsOnStyles(true);
            //LanguageEnToggle.targetToggle.isOn = false;
            LanguageEnToggle.SetIsOnStyles(false);

            LanguageRuToggle.SetActive(false);
            LanguageEnToggle.SetActive(true);
        }
        else if (lng == GameLanguages.English)
        {
            //LanguageRuToggle.targetToggle.isOn = false;
            LanguageRuToggle.SetIsOnStyles(false);
            //LanguageEnToggle.targetToggle.isOn = true;
            LanguageEnToggle.SetIsOnStyles(true);

            LanguageRuToggle.SetActive(true);
            LanguageEnToggle.SetActive(false);
        }
        UpdateLanguage();
    }



    public void UpdateLanguage()
    {
        ResetTxt.text = FarLife.GetText(_reseted ? FarText.Main_ResetEndText : FarText.Main_ResetText);
    }


    private bool _inReset = false;
    private float _totalResetTime = 3.0f;
    private float _onResetTime = 0.0f;
    public void ResetProgress()
    {
        _inReset = true;
        _onResetTime = 0.0f;
        ResetTimerTxt.gameObject.SetActive(true);
        ResetTxt.gameObject.SetActive(false);
    }
    public void ResetProgressStop()
    {
        _inReset = false;
        _onResetTime = 0.0f;
        ResetTimerTxt.gameObject.SetActive(false);
        ResetTxt.gameObject.SetActive(true);
    }

    private bool _reseted = false;
    private void OnReset()
    {
        _reseted = true;
        ResetProgressStop();
        //FarLife.GlobalData.IsNewGame = true;
        FarLife.ResetGameProgress();
        FarLife.SaveGlobalOnly();
        ResetBtn.gameObject.SetActive(false);
        //ResetTxt.gameObject.SetActive(false);
        UpdateLanguage();
        _controller.SoundBow();
    }

	
	// Update is called once per frame
	void Update () 
    {
	    if (!IsShowed) return;

        // reset update
	    if (_inReset)
	    {
	        _onResetTime += Time.deltaTime;
	        ResetTimerTxt.text = (_totalResetTime - _onResetTime).ToString("F1");
	        if (_onResetTime >= _totalResetTime)
	        {
	            OnReset();
	        }
	    }
    }


}
