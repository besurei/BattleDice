using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        this.transform.Rotate( (Vector3.forward * 300) * Time.deltaTime );
	}
}
