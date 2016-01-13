using UnityEngine;
using System.Collections;
using ZelderFramework.Animations;
using System;
using UnityEngine.UI;

public class LevelEndTextLogic : MonoBehaviour {

    public Text Txt;

    private EaseAnimations _animShow;
    private RectTransform _body;

    private float _startScale = 0.4f;
    private float _endScale = 1.0f;

	// Use this for initialization
	void Start () 
    {

	}

    public void Init(String text)
    {
        Txt.text = text;

        _body = this.gameObject.GetComponent<RectTransform>();
        _animShow = new EaseAnimations(EaseAnimationTypes.EaseOutBounce, _startScale, _endScale, 0.8f); 
    }


    public void Show()
    {
        this.gameObject.SetActive(true);

        // anim
        _body.localScale = new Vector3(_startScale, _startScale, _startScale);
        _animShow.Start();
    }
	
	// Update is called once per frame
	void Update () 
    {
        _animShow.Update(Time.deltaTime);
        if (_animShow.IsStarted())
        {
            var sc = _animShow.Value;
            _body.localScale = new Vector3(sc, sc, sc);
        }
	}
}
