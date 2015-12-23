using System;
using UnityEngine;
using System.Collections;


/// <summary>
/// Комета на карте.
/// </summary>
public class CometLogic : MonoBehaviour
{
    public Boolean Died = false;

    public float Speed = 2.0f;
    public Vector2 Dir = new Vector2(1, 1);
    public float TimeLife = 0.0f;
    public float PosZ = 0.0f;

    private Boolean _started = false;

	// Use this for initialization
	void Start ()
    {
	    
	}

    public void Go()
    {
        _started = true;

        transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), new Vector3(Dir.x, Dir.y, 0));
        //transform.rotation = Quaternion.LookRotation(new Vector3(Dir.x, Dir.y, PosZ));
    }

    public void Die()
    {
        Died = true;
        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (!_started || Died) return;
	    TimeLife += Time.deltaTime;
	    if (TimeLife >= 10.0f)
	    {
	        Die();
            return;
	    }

	    var def = Time.deltaTime*Speed;
	    //this.transform.Translate(Dir.x*def, Dir.y*def, this.transform.position.z);
        this.transform.position = new Vector3(this.transform.position.x + Dir.x * def, this.transform.position.y + Dir.y * def, PosZ);
        //Vector3 newDir = Vector3.RotateTowards(transform.forward, Dir, step, 0.0F);

        //transform.rotation = Quaternion.LookRotation(Dir);
        //transform.LookAt(this.transform.position + new Vector3(Dir.x, Dir.y, 0));
	}


}
