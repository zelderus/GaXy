using System;
using UnityEngine;
using System.Collections;


/// <summary>
/// Материал в пространстве.
/// </summary>
public class MaterialLogic : MonoBehaviour
{
    private LevelController _controller;

    public float Speed = 1.0f;
    public Boolean IsFloated = false;
    public Int32 Count = 1;


    private float _rotSpeedZ;
    private float _rotSpeed;
    private Int32 _rotDirZ = 1;
    private Int32 _rotDir = 1;

    private const float _sidePos = 3.2f;    //- минимальное (максимальное) положение по горизонтали, иначе сдвигаемся на экран видимости
    private const float _vertPos = -5.0f;   //- минимальное значение, когда считаемся, что улетели (не успели собрать)

	// Use this for initialization
	void Start()
    {
        _rotSpeedZ = UnityEngine.Random.Range(30.0f, 40.0f);
        _rotSpeed = UnityEngine.Random.Range(0.0f, 30.0f);
	    _rotDirZ = UnityEngine.Random.Range(0, 2) > 0 ? 1 : -1;
        _rotDir = UnityEngine.Random.Range(0, 2) > 0 ? 1 : -1;
    }

    public void Init(LevelController controller)
    {
        _controller = controller;
    }

    /// <summary>
    /// Позиция.
    /// <para>-3,7 : 3,-4</para>
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosition(Vector2 pos)
    {
        this.transform.position = new Vector3(pos.x, pos.y, 0);
    }



	
	// Update is called once per frame
	void Update()
    {
        if (IsFloated) return;
        if (_controller.Manager.IsPaused) return;

        //+ rot
        var rotZ = _rotSpeedZ * Time.deltaTime * _rotDirZ;
        var rot = _rotSpeed * Time.deltaTime * _rotDir;
        this.transform.Rotate(rot, 0, rotZ);

        //+ pos
        // y
        var def = (Time.deltaTime * Speed);
	    var nextY = this.transform.position.y - def;
        // x
        var nextX = this.transform.position.x;
        if (nextX <= -_sidePos) nextX += def;  //- смещаемся на экран
        if (nextX >= _sidePos) nextX -= def;

        this.transform.position = new Vector3(nextX, nextY, 0);

        if (nextY <= _vertPos)
	    {
	        IsFloated = true;
            Destroy(this.gameObject, 1.0f);
	    }
    }
}
