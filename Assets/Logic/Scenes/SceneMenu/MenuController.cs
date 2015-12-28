using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework.Helpers;

public class MenuController : MonoBehaviour
{


    public Button PlayBtn;
    public Button OptionBtn;
    public MenuOptionPanelLogic OptionPanel;
    

    void Awake()
    {
        FarLife.Init();
        FarLife.ScreenInit(BackPressed);
    }


	// Use this for initialization
	void Start () {




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
        if (OptionPanel.IsShowed)
        {
            HideOptionPanel();
        }
        else
        {
            Application.Quit();
        }
    }



    public void ShowOptionPanel()
    {
        PlayBtn.enabled = false;
        OptionBtn.enabled = false;
        OptionPanel.Show();

    }
    public void HideOptionPanel()
    {
        PlayBtn.enabled = true;
        OptionBtn.enabled = true;
        OptionPanel.Hide();
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
