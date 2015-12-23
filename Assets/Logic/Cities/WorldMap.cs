using UnityEngine;
using System.Collections;

public class WorldMap : MonoBehaviour
{


    public float MinX = -30.0f;
    public float MaxX = -6.0f;
    public float MinY = -30.0f;
    public float MaxY = -9.0f;


    public Transform Paralax;

	// Use this for initialization
	void Start () {
	    //- 
	}


    private Vector2 SuccessRangePosition(float posX, float posY)
    {
        posX = posX < MinX ? MinX : posX;
        posX = posX > MaxX ? MaxX : posX;

        posY = posY < MinY ? MinY : posY;
        posY = posY > MaxY ? MaxY : posY;

        return new Vector2(posX, posY);
    }
    private Vector2 SuccessRangePosition(Vector2 pos)
    {
        return SuccessRangePosition(pos.x, pos.y);
    }


    /// <summary>
    /// Установка позиции карты.
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosition(Vector2 pos)
    {
        var newPos = SuccessRangePosition(pos.x, pos.y);
        this.transform.position = newPos;

        var posParalax = newPos / 4;
        Paralax.transform.position = new Vector3(posParalax.x, posParalax.y, Paralax.transform.position.z);
    }
    /// <summary>
    /// Смещение карты.
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <param name="dir"></param>
    public void Move(float deltaTime, Vector2 dir)
    {
        var speed = 2.0f;

        var offsetX = dir.x*speed*deltaTime;
        var offsetY = dir.y*speed*deltaTime;
        //this.transform.Translate(dir.x * speed * deltaTime, dir.y * speed * deltaTime, 0);

        //var newPos = SuccessRangePosition(this.transform.position.x + offsetX, this.transform.position.y + offsetY);
        //this.transform.position = newPos;
        SetPosition(new Vector2(this.transform.position.x + offsetX, this.transform.position.y + offsetY));


    }

	


	// Update is called once per frame
	void Update () {
	
	}
}
