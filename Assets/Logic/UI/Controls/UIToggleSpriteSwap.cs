using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIToggleSpriteSwap : MonoBehaviour {

    public Toggle targetToggle;

    public bool WithCheckUpdate = true; //- обновление статуса при смене автоматом (иначе сами этим управляем, не будет реагировать на смену isOn. Нужно самому вызывать SetIsOnStyles())

    public Sprite SpriteOn;
    public Sprite SpriteOff;
    
    public Text TitleText;
    public Color TitleColorOn;
    public Color TitleColorOff;


    public bool IsCkecked { get; private set; }

    // Use this for initialization
    void Start()
    {
        targetToggle.toggleTransition = Toggle.ToggleTransition.None;

        if (WithCheckUpdate) targetToggle.onValueChanged.AddListener(OnTargetToggleValueChanged);
    }

    void OnTargetToggleValueChanged(bool newValue)
    {
        UpdateToggle(newValue);
    }

    private void UpdateToggle(bool newValue)
    {
        IsCkecked = newValue;

        //+ ON
        if (newValue)
        {
            //- img
            Image targetImage = targetToggle.targetGraphic as Image;
            if (targetImage != null) targetImage.overrideSprite = SpriteOn;
            //- title
            TitleText.color = TitleColorOn;
        }
        //+ OFF
        else
        {
            //- img
            Image targetImage = targetToggle.targetGraphic as Image;
            if (targetImage != null) targetImage.overrideSprite = SpriteOff;
            //- title
            TitleText.color = TitleColorOff;
        }
    }

    /// <summary>
    /// Включение/выключение.
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        targetToggle.interactable = isActive;
    }

    /// <summary>
    /// Установка начального значения.
    /// </summary>
    /// <param name="isOn"></param>
    public void SetIsOnStyles(bool isOn)
    {
        UpdateToggle(isOn);
    }

}
