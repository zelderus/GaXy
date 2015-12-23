using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    
    public Color Color = new Color(0.0f, 0.0f, 0.5f, 1.0f);

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    private Text _guiText;

    void Start()
    {
        _guiText = this.GetComponent<Text>();

        if (!_guiText)
        {
            Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
            enabled = false;
            return;
        }
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            //string format = System.String.Format("{0:F2} FPS", fps);
            string format = System.String.Format("{0:F2}", fps);
            _guiText.text = format;

            //if (fps < 30)
            //    guiText.material.color = Color.yellow;
            //else
            //    if (fps < 10)
            //        guiText.material.color = Color.red;
            //    else
            //        guiText.material.color = Color.green;

            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
