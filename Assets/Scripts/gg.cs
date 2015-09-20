using UnityEngine;
using System.Collections;

public class gg : MonoBehaviour {

	public int timer = 120;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (Vector3.zero,Vector3.up,1f);

		if (Input.anyKeyDown) {
			if (timer <= 0)
				Application.LoadLevel ("arena1;");
		} else {
			timer--;
		}
	}
}
