using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuOptionPanelLogic : MonoBehaviour {

    public Boolean IsShowed { get; private set; }

    public Text ResetTimerTxt;
    public Button ResetBtn;

    // Use this for initialization
    void Start()
    {
        ResetBtn.gameObject.SetActive(!FarLife.GlobalData.IsNewGame);
    }



    public void Hide()
    {
        IsShowed = false;
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        IsShowed = true;
        this.gameObject.SetActive(true);
    }


    private bool _inReset = false;
    private float _totalResetTime = 3.0f;
    private float _onResetTime = 0.0f;
    public void ResetProgress()
    {
        _inReset = true;
        _onResetTime = 0.0f;
        ResetTimerTxt.gameObject.SetActive(true);
    }
    public void ResetProgressStop()
    {
        _inReset = false;
        _onResetTime = 0.0f;
        ResetTimerTxt.gameObject.SetActive(false);
    }

    private void OnReset()
    {
        ResetProgressStop();
        FarLife.GlobalData.IsNewGame = true;
        FarLife.SaveGlobalOnly();
        ResetBtn.gameObject.SetActive(false);
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
