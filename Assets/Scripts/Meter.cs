using UnityEngine;
using System.Collections;

public class Meter : MonoBehaviour {

	private Transform meter;
	private Transform mask;
	
	private float modifierRatio;
	
	private Player player;
	
	// public float startingXScale;
	
	public void Awake(){
		meter = transform.Find("Meter");
		mask = meter.transform.Find("Mask");
		
		player = Player.Instance();
		
		// startingXScale = meter.localScale.x;
		
		// Debug.Log(transform.name);
	}
	
	public void Update(){
		if(Player.Instance()){
			
			
			if(transform.name == "Health"){
				modifierRatio = (player.maxHealth - player.health) / player.maxHealth;
			} else if(transform.name == "Stamina"){
				modifierRatio = (player.maxStamina - player.stamina) / player.maxStamina;
			}
			
			if(modifierRatio < 0){
				modifierRatio = 0;
			}
			
			mask.localScale = new Vector3(modifierRatio , mask.localScale.y, mask.localScale.z);
			mask.position = new Vector3(meter.GetComponent<MeshRenderer>().bounds.max.x - (mask.lossyScale.x * .5f), mask.position.y, mask.position.z);
			
		}
	}
}
