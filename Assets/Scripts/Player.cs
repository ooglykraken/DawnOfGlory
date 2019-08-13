using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private int horizontalDirection = 0;
	private int verticalDirection = 0;
	
	public float speed = 10f;
	
	public float health;
	public float maxHealth = 10f;
	
	public float stamina;
	public float maxStamina = 100f;
	
	private float rollSpeed = 8f;
	private float rollDistance = 10f;
	public int rollCooldown = 0;
	private int rollCooldownTime = 10;
	private int rollStaminaCost = 25;
	public int rollingTime = 0;
	private int rollDuration = 25;
	
	public bool canRoll = true;
	
	// windmill speed private float swingSpeed = 2000f;
	private float swingSpeed = 1000f;
	public int swingingTime = 0;
	private int swingDuration = 10;
	
	public int swingCooldown = -1;
	private int swingCooldownTime = 4;
	private int swingStaminaCost = 20;
	
	public bool canAttack = true;
	
	public int attackDamage = 1;
	
	public Vector3 swordNormalAngle;
	private Vector3 stopPointForSwing;
	private Vector3 swordNormalPosition;
	
	private Vector3 rollDestination;
	
	public Rigidbody playerRigidbody;
	
	private Collider playerCollider;
	
	private Transform swordTransform;
	
	public void Awake(){
		// horizontalDirection = 0;
		// verticalDirection = 0;
		
		playerRigidbody = GetComponent<Rigidbody>();
		
		health = maxHealth;
		stamina = maxStamina;
		
		swordTransform = transform.Find("Sword");
		
		playerCollider = transform.Find("Collider").gameObject.GetComponent<Collider>();
		
		swordNormalPosition = swordTransform.localPosition;
		stopPointForSwing = swordTransform.localPosition - Vector3.right;
		swordNormalAngle = swordTransform.localEulerAngles;
		
		swordTransform.gameObject.GetComponent<Collider>().enabled = false;
	}
	
	public void Update(){
		if(stamina < maxStamina){
			stamina++;
		}
		
		
		if(Input.GetKeyDown("k") && stamina > 0){
			if(canAttack){
				Attack();
			}
			return;
		}
		
		if(stamina < 0){
			stamina = 0;
		}
	
		if(Input.GetKeyDown("j") && stamina > 0){
			if(canRoll){
				Evasion();
			}
		}

		if(stamina < 0){
			stamina = 0;
		}
		
		if(Input.GetKey("a")){
			horizontalDirection = -1;
		} else if(Input.GetKey("d")){
			horizontalDirection = 1;
		} else {
			horizontalDirection = 0;
		}
		
		
		
		if(Input.GetKey("w")){
			verticalDirection = 1;
		} else if(Input.GetKey("s")){
			verticalDirection = -1;
		} else {
			verticalDirection = 0;
		}
		
		Turn();
		
		if(rollingTime > 0){
			transform.position = Vector3.Lerp(transform.position, rollDestination, rollSpeed * Time.deltaTime);
			rollingTime--;
			// rollCooldown--;
			
		} else if(rollingTime <= 0){
			rollCooldown = rollCooldownTime;
		}
		
		if(rollCooldown > 0){
			rollCooldown--;
		} else if(rollCooldown <= 0){
			canRoll = true;
			if(!playerCollider.enabled){
				playerCollider.enabled = true;
			}
		}
		
		if(swingCooldown > 0){
			swingCooldown--;
		} else if(swingCooldown >= 0){
			canAttack = true;
			swingCooldown = -1;
		} else if(swingCooldown == -1){
			
		}

		if(swingingTime > 0){
			// swordTransform.localPosition = Vector3.Lerp(swordTransform.localPosition, stopPointForSwing, swingSpeed * Time.deltaTime);
			swordTransform.RotateAround(transform.position, Vector3.up, -swingSpeed * Time.deltaTime);
			swingingTime--;
		} else if(swingingTime <= 0){
			swordTransform.localEulerAngles = swordNormalAngle;
			swordTransform.localPosition = swordNormalPosition;
			
			if(swordTransform.gameObject.GetComponent<Collider>().enabled){
				swingCooldown = swingCooldownTime;
				swordTransform.gameObject.GetComponent<Collider>().enabled = false;
			}
		}
	}
	
	public void FixedUpdate () {
		// if(rollingTime > 0 || swingingTime > 0){
			// return;
		// }
		
		
		
		
		Move();
		
		
		
		
	}
	
	private void Evasion(){
		rollDestination = transform.position + (transform.forward * rollDistance);
		// rollCooldown = rollCooldownTime;
		rollingTime = rollDuration;
		
		canRoll = false;
		
		stamina -= rollStaminaCost;
		
		playerCollider.enabled = false;
	}
	
	private void Attack(){
		// Vector3 tempPosition = swordTransform.position;
		// Vector3 tempAngles = swordTransform.eulerAngles;
		
		swingingTime = swingDuration;
		canAttack = false;

		stamina -= swingStaminaCost;
		
		swordTransform.gameObject.GetComponent<Collider>().enabled = true;
	}
	
	public void Hit(GameObject objectHit){
		
		
		if(objectHit.transform.parent != null){
			switch(objectHit.transform.parent.tag){
				case "Enemy":
					DamageEnemy(objectHit.transform.parent.gameObject.GetComponent<Enemy>());
					break;
				default:
					break;
			}
		} else {
			switch(objectHit.tag){
				case "Enemy":
					DamageEnemy(objectHit.transform.parent.gameObject.GetComponent<Enemy>());
					break;
				default:
					break;
			}
		}
	}
	
	private void DamageEnemy(Enemy enemyHit){
		bool isKilled;
		
		isKilled = enemyHit.TakeDamage(attackDamage, transform.position);
	}
	
	private void Damage(GameObject randomObject){
		
	}
	
	private void Turn(){
		if(horizontalDirection == 0 && verticalDirection == 0){
			return;
		}
		
		int newFacing = 0;
		
		if(verticalDirection == 1 && horizontalDirection == 1){
			newFacing = 45;
		} else if(verticalDirection == -1 && horizontalDirection == 1){
			newFacing = 135;
		} else if(verticalDirection == 1 && horizontalDirection == -1){
			newFacing = 315;
		} else if(verticalDirection == -1 && horizontalDirection == -1){
			newFacing = 225;
		} else if(verticalDirection == 1 && horizontalDirection == 0){
			newFacing = 0;
		} else if(verticalDirection == 0 && horizontalDirection == 1){
			newFacing = 90;
		} else if(verticalDirection == -1 && horizontalDirection == 0){
			newFacing = 180;
		} else if(verticalDirection == 0 && horizontalDirection == -1){
			newFacing = 270;
		}
		
		transform.eulerAngles = new Vector3(0f, newFacing, 0f);
	}
	
	private void Move(){
		if(verticalDirection != 0 || horizontalDirection != 0){
			float movementSpeed = speed;
			if((verticalDirection == 0 && horizontalDirection != 0) || (verticalDirection != 0 && horizontalDirection == 0)){
				movementSpeed *= 1.5f;
			}
			playerRigidbody.velocity = new Vector3(horizontalDirection * movementSpeed, 0f, verticalDirection * movementSpeed);
		} else {
			playerRigidbody.velocity = Vector3.Lerp(playerRigidbody.velocity, Vector3.zero, Time.deltaTime * 6f);
		}
	}
	
	private static Player instance;
	
	public static Player Instance(){
		if(instance == null){
			instance = GameObject.FindObjectOfType<Player>();
		}
		
		return instance;
	}
}
