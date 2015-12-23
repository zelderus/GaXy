using System;
using UnityEngine;
using System.Collections;

public class LevelParalaxLogic : MonoBehaviour
{


    public float ParalaxNum;
    public Material Mat;

    public float RotY = 0;

    private ShipLife _ship;

	// Use this for initialization
	void Start () 
    {
	
	}


    public void Init(ShipLife ship)
    {
        _ship = ship;
    }




    private void RotateMat(float flySpeed)
    {
        // повертикали всегда скролл
        var xoffset = Time.deltaTime * (flySpeed * ParalaxNum);
        // погоризонтали
        var yoffset = 0.0f;
        if (RotY > 0.0f || RotY < 0.0f)
        {
            yoffset = (RotY*0.02f) * (Time.deltaTime *ParalaxNum);
        }

        Mat.mainTextureOffset = new Vector2(Mat.mainTextureOffset.x + xoffset, Mat.mainTextureOffset.y + yoffset);
    }


    public void Rotate(float flySpeed)
    {
        //var flySpeed = _ship.FlySpeed;
        RotateMat(flySpeed);

        //RotY = 0.0f;
    }

	
	// Update is called once per frame
	void Update ()
	{
        RotY = 0.0f;

	}
}
