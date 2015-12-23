using System;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BombLogic : MonoBehaviour {


    public Boolean Work;
    public float Damage = 1.0f;
    public float TimeLife = 1.0f;
    public float SpeedFloat = 40.0f;

    public Int32 BombId { get; private set; }

    private LevelController _controller;
    
	// Use this for initialization
	void Start ()
    {
	    
	}

    public void Init(LevelController controller, ShipLife ship, Vector3 pos, Int32 bombId)
    {
        _controller = controller;
        BombId = bombId;
        Work = true;

        //+ на основе ship
        //Damage *= gun.DamageDif;
        

        this.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }


	// Update is called once per frame
	void Update () 
    {
        if (_controller.Manager.IsPaused) return;
        if (!Work) return;
	    
	    TimeLife -= Time.deltaTime;
	    if (TimeLife <= 0.0f)
	    {
	        Work = false;
            Destroy(this.gameObject, 1.0f);
            return;
	    }
        // scale
        var scaleDelta = SpeedFloat * Time.deltaTime;
        this.transform.localScale = new Vector3(this.transform.localScale.x + scaleDelta, this.transform.localScale.y + scaleDelta, this.transform.localScale.z + scaleDelta);

    }
}
