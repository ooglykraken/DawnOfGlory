using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public int weaponInUse;
	
	public GameObject weaponOwner;
	
	public void Awake(){
		if(transform.parent != null){
			weaponOwner = transform.parent.gameObject;
		}
	}
	
	public void FixedUpdate(){
		
	}
	
	public void Update(){
		
	}
	
	public void OnTriggerEnter(Collider c){
		
		if(c.gameObject != weaponOwner){
			
			if(c.transform.parent != null && c.transform.parent.gameObject != weaponOwner){
				if(weaponOwner.tag == "Player"){
					
					weaponOwner.GetComponent<Player>().Hit(c.gameObject);
				}
			}
		}
	}
}
