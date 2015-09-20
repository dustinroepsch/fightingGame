using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public Transform target;
	public Transform counterEffect;
	public Transform gameOgre;

	public float rotSpeed = 10f;
	public float zMove = .1f;

	public KeyCode rotLeft;
	public KeyCode rotRight;
	public KeyCode forward;
	public KeyCode back;

	public KeyCode leftPow;
	public KeyCode rightPow;
	public KeyCode block;

	public bool isDead = false;
	public string badfist;
	public int maxHealth = 100;
	public int currentHealth;
	public float maxStamina = 100f;
	public float currentStamina;
	public TextMesh healthRead;
	public bool attacking = false;
	public bool blocking = false;
	public float knockBackRemaining = 0;
	public float knockBackConstant = 5;
	public int staminaRechargeConstant = 10; //TODO different values different characters
	public int comboCount = 0;
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth <= 0 && !isDead) {
			gameOgre.gameObject.SetActive(true);
			currentHealth = 0;
			gameObject.GetComponent<Rigidbody> ().AddForce(Vector3.up*1250);
			isDead = true;

		}
		if (knockBackRemaining > 0) {
			knockBackRemaining -= knockBackConstant*Time.deltaTime;
			gameObject.GetComponent<Animator>().Play ("staggered", 0);

		}
		healthRead.text = currentHealth.ToString();
		transform.LookAt (target.position);
		if (currentStamina > maxStamina)
			currentStamina = maxStamina;
		currentStamina += Time.deltaTime * staminaRechargeConstant;
	

		if (gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			attacking = false;
			blocking = false;
		}
		if (knockBackRemaining <= 0 && gameObject.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("idle")) {
			if (Input.GetKeyDown (leftPow) && currentStamina >= 20 && attacking == false) {
				gameObject.GetComponent<Animator> ().Play ("leftfisting", 0);
				attacking = true;
				currentStamina -= 20;
				comboCount++;
			}

			if (Input.GetKeyDown (rightPow) && currentStamina >= 20 && attacking == false) {
				gameObject.GetComponent<Animator> ().Play ("rightfisting", 0);
				attacking = true;
				currentStamina -= 20;
				comboCount++;
			}

			if (Input.GetKeyDown (block)) {
				if (currentStamina > 10 && blocking == false) {
					gameObject.GetComponent<Animator> ().Play ("parry", 0);
					blocking = true;
					currentStamina -= 50;
				}
			}
		

			if (Input.GetKey (rotLeft))
				transform.RotateAround (target.position, Vector3.up, rotSpeed);
			if (Input.GetKey (rotRight))
				transform.RotateAround (target.position, Vector3.up, -rotSpeed);
			if (Input.GetKey (forward))
				gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (transform.forward.x, 0, transform.forward.z) * zMove);
			if (Input.GetKey (back))
				gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (transform.forward.x, 0, transform.forward.z) * -zMove);

		}
	}

	void OnCollisionEnter(Collision col){
		if (col.collider.gameObject.CompareTag (badfist) && blocking == true && col.collider.gameObject.GetComponentInParent<PlayerMovement> ().attacking == true) {
			col.collider.gameObject.GetComponentInParent<PlayerMovement> ().comboCount = 0;

			if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo (0).IsName ("parry") && col.collider.gameObject.GetComponentInParent<PlayerMovement> ().knockBackRemaining <= 0){
				col.collider.gameObject.GetComponentInParent<PlayerMovement> ().knockBackRemaining = knockBackConstant;
				Transform clone = Instantiate(counterEffect);
				clone.position = transform.position;
				col.collider.gameObject.GetComponentInParent<Rigidbody> ().AddExplosionForce(300f,new Vector3(col.collider.gameObject.transform.position.x, col.collider.gameObject.transform.position.y-.1f, col.collider.gameObject.transform.position.z),100f);
				Debug.Log("Where's parry?");
			}
		}
		if (col.collider.gameObject.CompareTag (badfist) && blocking == false && col.collider.gameObject.GetComponentInParent<PlayerMovement> ().attacking == true){

			currentHealth -= 5*col.collider.gameObject.GetComponentInParent<PlayerMovement> ().comboCount;
			if (currentHealth <= 0 ) {
				col.collider.gameObject.GetComponentInParent<Animator> () .Play("player has winned", 0);
			}
			comboCount = 0;
			col.collider.gameObject.GetComponentInParent<PlayerMovement> ().attacking = false;
			}
		}
}
