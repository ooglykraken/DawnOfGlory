using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private Rigidbody enemyRigidbody;
	
	public int damageTextTimeLeft;
	private int damageTextDuration = 40;
	
	public int health = 3;
	
	private float speed = 2f;
	private float halfSpeed;
	
	private bool hasFoundPlayer = false;
	
	private Transform playerTransform;
	
	private float xMovement;
	private float zMovement;
	
	private float timeUntilDirectionChange;
	
	private MeshRenderer damageTextRenderer;
	private TextMesh damageText;

	public void Awake(){
		enemyRigidbody = gameObject.GetComponent<Rigidbody>();
		
		damageTextRenderer = transform.Find("DamageText").gameObject.GetComponent<MeshRenderer>();
		damageText = transform.Find("DamageText").gameObject.GetComponent<TextMesh>();
		
		if(Player.Instance()){
			playerTransform = Player.Instance().gameObject.transform;
		}
		
		timeUntilDirectionChange = Random.Range(0, 100);
		
		halfSpeed = speed / 2;
	}
	
	public void Update(){
		if(damageTextTimeLeft > 0){
			damageTextTimeLeft--;
		} else if(damageTextTimeLeft<= 0){
			damageTextRenderer.enabled = false;
		}
		
		if(timeUntilDirectionChange > 0){
			timeUntilDirectionChange--;
		}
		
		if(timeUntilDirectionChange <= 0){
			xMovement = Random.Range(0, 2) - 1;
			zMovement = Random.Range(0, 2) - 1;
			timeUntilDirectionChange = Random.Range(0, 100);
		}
		
		if(enemyRigidbody.velocity != Vector3.zero){
			enemyRigidbody.velocity = Vector3.Lerp(enemyRigidbody.velocity, Vector3.zero, 4f * Time.deltaTime);
		}
	}
	
	public void FixedUpdate(){
		if(hasFoundPlayer){
			MoveTowardPlayer();
		} else {
			Wander();
		}
		
		enemyRigidbody.velocity = Vector3.Lerp(enemyRigidbody.velocity, new Vector3(enemyRigidbody.velocity.x, 0f, enemyRigidbody.velocity.z), Time.deltaTime);
	}
	
	public void OnTriggerEnter(Collider c){
		if(c.transform.parent != null){
			if(c.transform.parent.name == "Player"){
				hasFoundPlayer = true;
			}
		}
	}
	
	public void OnTriggerExit(Collider c){
		if(c.transform.parent != null){
			if(c.transform.parent.name == "Player"){
				hasFoundPlayer = false;
			}
		}
	}
	
	public bool TakeDamage(int amountOfDamage, Vector3 pointOfContact){
		bool didDie = false;
		
		damageTextRenderer.enabled = true;
		damageText.text = "-" + amountOfDamage;
		damageTextTimeLeft = damageTextDuration;
		
		if((health - amountOfDamage) <= 0){
			Die();
			didDie = true;
		} else {
			health -= amountOfDamage;
			KnockBack(pointOfContact);
		}
		
		return didDie;
	}
	
	private void KnockBack(Vector3 point){
		float knockbackForce = -50f;
		
		Vector3 dir = point - transform.position; 
		dir.y = transform.position.y; // keep the force horizontal if (other.rigidbody){ 
		enemyRigidbody.AddForce(dir.normalized * knockbackForce, ForceMode.VelocityChange); 
		
	}
	
	private void Wander(){
		float netSpeed = speed;
		
		if(xMovement == 1 && zMovement == 1){
			netSpeed = halfSpeed;
		}
		
		transform.position = Vector3.Lerp(transform.position , new Vector3(transform.position.x + (netSpeed * xMovement), transform.position.y, transform.position.z + (netSpeed * zMovement)), speed * Time.deltaTime);
	}
	
	private void MoveTowardPlayer(){
		transform.position = Vector3.Lerp(transform.position , new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z), halfSpeed * Time.deltaTime);
	}
	
	private void Die(){
		GameObject.Destroy(this.gameObject);
		// Die procedure
	}
}
