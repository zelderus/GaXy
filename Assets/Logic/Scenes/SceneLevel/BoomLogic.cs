using UnityEngine;
using System.Collections;

public class BoomLogic : MonoBehaviour
{

    private float _times = 0.7f;
    private bool _floated = false;

    private Renderer _renderer;
    private Color _color;
    private Vector3 _scale;

	// Use this for initialization
	void Start ()
	{
	    _renderer = this.gameObject.GetComponent<Renderer>();
	    _color = _renderer.sharedMaterial.color;
        _scale = _renderer.transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (_floated) return;

	    _times -= Time.deltaTime;
        _renderer.material.color = _color * new Color(1, 1, 1, Mathf.Lerp(0.0f, 1.0f, _times));
        _renderer.transform.localScale = _scale * Mathf.Lerp(0.0f, 1.0f, _times);

	    if (_times <= 0.0f)
	    {
	        _floated = true;
	        _renderer.material.color = _color * new Color(1, 1, 1, 0);
            //_renderer.transform.localScale = _scale * new Vector3(1, 1, 1);
	    }
	}

}
