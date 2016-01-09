using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MenuStatPanelLogic : MonoBehaviour {

    public Boolean IsShowed { get; private set; }

    public Text LevelRunCount;
    public Text LevelWinsCount;
    public Text LevelTotalTimeCount;
    public Text LevelTotalWinCount;
    public Text EnemyDied;
    public Text EnemyDiedAvg;
    public Text BossDied;
    public Text BossDiedAvg;


	// Use this for initialization
	void Start () 
    {
        UpdateData();
	}



    private void UpdateData()
    {
        LevelRunCount.text = FarLife.FarStat.LevelRuns.ToString();
        LevelWinsCount.text = FarLife.FarStat.LevelWins.ToString();
        LevelTotalTimeCount.text = (FarLife.FarStat.LevelRuns <= 0 ? "0" : (FarLife.FarStat.LevelTotalTime / (float)FarLife.FarStat.LevelRuns).ToString("F1")) + " sec";
        LevelTotalWinCount.text = (FarLife.FarStat.LevelWins <= 0 ? "0" : (FarLife.FarStat.LevelWinsTime / (float)FarLife.FarStat.LevelWins).ToString("F1")) + " sec";

        EnemyDied.text = FarLife.FarStat.TotalEnemyDied.ToString();
        EnemyDiedAvg.text = (FarLife.FarStat.LevelRuns <= 0 ? "0" : (FarLife.FarStat.TotalEnemyDied / FarLife.FarStat.LevelRuns).ToString());
        BossDied.text = FarLife.FarStat.TotalBossDied.ToString();
        BossDiedAvg.text = (FarLife.FarStat.LevelRuns <= 0 ? "0" : (FarLife.FarStat.TotalBossDied / FarLife.FarStat.LevelRuns).ToString());
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
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
