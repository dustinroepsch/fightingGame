using UnityEngine;
using System.Collections;

public class particleKiller : MonoBehaviour {
	public int lifetime =2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 if (lifetime <= 0) {
			Destroy(gameObject);
		}
		lifetime --;
	}
}
