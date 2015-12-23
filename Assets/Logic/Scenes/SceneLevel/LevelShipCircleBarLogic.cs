using UnityEngine;
using System.Collections;

public class LevelShipCircleBarLogic : MonoBehaviour
{


    public Material Mat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        //this.GetComponent<Renderer>().material.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x)); 
        Mat.SetFloat("_Cutoff", Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x)); 
	}
}
