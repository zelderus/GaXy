using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework;
using ZelderFramework.Helpers;

public class MenuController : MonoBehaviour
{


    public Button PlayBtn;
    public UIButtonLng PlayUiBtn;

    public Button OptionBtn;
    public Button StatBtn;
    public MenuOptionPanelLogic OptionPanel;
    public MenuStatPanelLogic StatPanel;

    public Text TitleText;

    public Text ResetTxt;
    public Text SoundTxt;

    void Awake()
    {
        FarLife.Init();
        FarLife.ScreenInit(BackPressed);
    }


	// Use this for initialization
	void Start () {

        //+ сохраняем (если не начальная загрузка игры)
	    if (!FarLife.GameOnRun)
	    {
	        FarLife.SaveGame();
	    }
        FarLife.GameNotInRun();

        UpdateLanguage();
        
        
        // preload
        OptionPanel.Show();
        OptionPanel.Hide();


        //- сообщаем движку что готовы к сцене
        FarLife.OnScreenLoaded();
	}
    /// <summary>
    /// Suspending.
    /// </summary>
    /// <param name="pausing"></param>
    public void OnApplicationPause(bool pausing)
    {
        FarLife.GameLife.OnApplicationPause(pausing);

    }


    /// <summary>
    /// Нажали Back.
    /// </summary>
    private void BackPressed()
    {
        if (StatPanel.IsShowed)
        {
            HideStatPanel();
            return;
        }

        if (OptionPanel.IsShowed)
        {
            HideOptionPanel();
        }
        else
        {
            Application.Quit();
        }
    }


    public void OptionSoundToggle(bool isOn)
    {
        FarLife.SetSound(isOn);
    }


    public void OptionLanguageRusToggle(bool isOn)
    {
        OptionPanel.SetLanguage(GameLanguages.Russian);
        FarLife.SetLanguage(GameLanguages.Russian);
        UpdateLanguage();
    }
    public void OptionLanguageEngToggle(bool isOn)
    {
        OptionPanel.SetLanguage(GameLanguages.English);
        FarLife.SetLanguage(GameLanguages.English);
        UpdateLanguage();
    }


    public void UpdateLanguage()
    {
        // textes
        PlayUiBtn.UpdateLanguage();
        //ResetTxt.text = FarLife.GetText(FarText.Main_ResetText);
        SoundTxt.text = FarLife.GetText(FarText.Main_Sound);
        TitleText.text = FarLife.GetText(FarText.Main_Title);
        OptionPanel.UpdateLanguage();
    }

    public void ShowOptionPanel()
    {
        PlayBtn.enabled = false;
        OptionBtn.enabled = false;
        StatBtn.enabled = false;
        OptionPanel.Show();
    }
    public void HideOptionPanel()
    {
        PlayBtn.enabled = true;
        OptionBtn.enabled = true;
        StatBtn.enabled = true;
        OptionPanel.Hide();
    }


    public void ShowStatPanel()
    {
        PlayBtn.enabled = false;
        OptionBtn.enabled = false;
        StatBtn.enabled = false;
        StatPanel.Show();
    }
    public void HideStatPanel()
    {
        PlayBtn.enabled = true;
        OptionBtn.enabled = true;
        StatBtn.enabled = true;
        StatPanel.Hide();
    }


    public void GoToMap()
    {
        FarLife.GoToMap();
    }


    


	
	// Update is called once per frame
	void Update () {
        FarLife.Update();

        DisplayHelper.UpdateScreenSacle();

	}



    void OnGUI()
    {
        //- рисует движок
        FarLife.OnGUI();
    }


}
