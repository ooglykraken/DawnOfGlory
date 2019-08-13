using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	
	private Transform player;
	
	private Vector3 velocity = Vector3.zero;

	private Camera thisCamera;
	
	public void Awake(){
		player = Player.Instance().gameObject.transform;
		
		thisCamera = GetComponent<Camera>();
	}
	
	public void LateUpdate(){
		// Vector3 playerCameraPosition = thisCamera.WorldToScreenPoint(player.position);

		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), ref velocity, Time.deltaTime);

	}
	
	private static MainCamera instance = null;
	
	public static MainCamera Instance(){
		if(instance == null){
			instance = GameObject.FindObjectOfType<MainCamera>();
		}
		return instance;
	}
}
