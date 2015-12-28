using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ZelderFramework;

public class UIButtonLng : MonoBehaviour
{


    public Sprite TextureEn;
    public Sprite TextureRu;

    
	// Use this for initialization
	void Start () 
    {
        this.GetComponent<Image>().sprite = FarLife.Language == GameLanguages.English ? TextureEn : TextureRu; 
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
