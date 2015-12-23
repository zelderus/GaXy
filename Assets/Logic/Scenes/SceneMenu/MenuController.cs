using UnityEngine;
using System.Collections;
using ZelderFramework.Helpers;

public class MenuController : MonoBehaviour {


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
        //if (_isOptionsShowed)
        //{
        //    CloseOptionsPanel();
        //}
        //else
        {
            Application.Quit();
        }
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
