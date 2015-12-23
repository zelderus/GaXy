using UnityEngine;
using System.Collections;


/// <summary>
/// Столкновение с кораблем.
/// <remarks>Объект должен иметь галочку IsTrigger и иметь Rigitbody.</remarks>
/// </summary>
public class WithShipColliderLogic : MonoBehaviour {

	
	void OnTriggerEnter(Collider other)
	{
	    if (other.gameObject.tag != "Ship") return;


	    var ship = other.gameObject.GetComponent<ShipFlyLogic>();
	    var mat = this.gameObject.GetComponent<MaterialLogic>();

        //mat.Count
        ship.Controller.AddMaterial(mat.Count);
	    mat.IsFloated = true;
	}

    //void OnTriggerStay(Collider other)
    //{

    //}

    //void OnTriggerExit(Collider other)
    //{

    //}

}
