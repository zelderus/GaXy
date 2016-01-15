using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using ZelderFramework.Helpers;

/// <summary>
/// Панель Скиллов со скроллом.
/// </summary>
public class PanelSkillLogic : MonoBehaviour, IDragHandler, IEndDragHandler
{


    public RectTransform ContentRect;
    public PanelSkillSkyLogic SkyTop;
    public PanelSkillSkyLogic SkyBottom;


    private float _realOffsetY = 0.0f;
    private float _minOffsetY = 0.0f;
    private float _maxOffsetY = 220.0f;


    private bool _isShowed = false;
    private float _moveY = 0.0f;

	// Use this for initialization
	void Start () 
    {
	
	}


    public void Init()
    {
        _minOffsetY = _minOffsetY * DisplayHelper.ScreenScale;
        _maxOffsetY = _maxOffsetY * DisplayHelper.ScreenScale;
        SkyTop.Init();
        SkyBottom.Init();

        SkyTop.Hide();
        SkyBottom.Show();
    }


    public void SetShowed(bool isShowed)
    {
        _isShowed = isShowed;
    }


    private Vector3 WrapPos(float offsetX, float offsetY)
    {
        var n = _realOffsetY + offsetY; // следующая позиция
        if (n < _minOffsetY)
        {
            offsetY = _minOffsetY - _realOffsetY;
            SkyTop.Hide();
        }
        else if (n > _maxOffsetY)
        {
            offsetY = _maxOffsetY - _realOffsetY;
            SkyBottom.Hide();
        }

        if (n > _minOffsetY) SkyTop.Show();
        if (n < _maxOffsetY) SkyBottom.Show();

        _realOffsetY += offsetY;    //+ запоминаем новое реальное смещение


        return new Vector3(ContentRect.transform.position.x + offsetX, ContentRect.transform.position.y + offsetY, ContentRect.transform.position.z);
    }


    private Vector2 _lastPos = Vector2.zero;
    private Vector2 _nextPos = Vector2.zero;
    public void OnDrag(PointerEventData eventData)
    {
        _moveY = eventData.delta.y;
        _nextPos = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _moveY = 0.0f;
        _lastPos = Vector2.zero;
        _nextPos = Vector2.zero;
    }


    // Update is called once per frame
    void Update ()
    {
        if (!_isShowed) return;

        if (_nextPos != _lastPos)
        {
            var newPos = WrapPos(0.0f, _moveY);
            ContentRect.transform.position = newPos;
        }
        _lastPos = _nextPos;


    }

    
}
