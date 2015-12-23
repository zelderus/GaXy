using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Спрайт количества собранного материала.
/// </summary>
public class MaterialCountLogic : MonoBehaviour
{
    private LevelController _controller;

    public Int32 Count;
    public float Speed;

    public Text NumTxt;
    public Animator Anim;
    //public Animation Animat;

	// Use this for initialization
	void Start () {
	
	}



    public void Init(LevelController controller, Int32 count, float speed)
    {
        _controller = controller;
        Count = count;
        NumTxt.text = String.Format("+{0}", count);

        Speed = speed * 20.0f;  // world to screen speed

        //Anim.Stop();
        //Anim.Play("MatCountAnimation");
        //this.GetComponent<Animator>().gameObject.SetActive(true);


    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_controller.Manager.IsPaused) return;

        // pos
        var def = -(Time.deltaTime * Speed);
        var nextY = this.transform.position.y + def;
        this.transform.position = new Vector3(this.transform.position.x, nextY, 0);
	}
}
