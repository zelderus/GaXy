using UnityEngine;
using System.Collections;

public class ShipLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        var rotSpeed = 30;
        var rot = rotSpeed * Time.deltaTime;
        this.transform.Rotate(0, 0, rot);
	}
}
