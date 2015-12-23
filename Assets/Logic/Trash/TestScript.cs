using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {


    public float Speed = 140.0f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.Rotate(0, 0, Speed * Time.deltaTime);
	}
}
