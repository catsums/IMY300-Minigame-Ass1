using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using SignalBusNS;
using StateMachineNS;
using VEC2;
using MyHelperFunctions;
using PlayerInputsNS;
using GameObjectExt;

public class PlayerMain : MonoBehaviour
{
	public int hp = 3;
	public PlayerInputCtrl InputCtrl;
	public PlayerMoveCtrl MoveCtrl;

	public Collider2D hitBox;
	public SignalBus signalBus = new SignalBus();
	void Awake() {
		
	}
	void Start(){
		var timerComp = GetComponent<TimerComp>();
		timerComp.signalBus.Subscribe("timeout", (x)=>{
			print("meow");
		});

		if(!InputCtrl){
			InputCtrl = GetComponent<PlayerInputCtrl>();
		}
		if(!MoveCtrl){
			MoveCtrl = GetComponent<PlayerMoveCtrl>();
		}

		if(hitBox){
			var hitBoxComp = hitBox.GetComponent<ColliderLayerComp>();
			hitBoxComp.OnDetectCollisionEnter(OnDetectCollisionEnter);
		}
	}

	void FixedUpdate()
	{
		
	}

	void OnDetectCollisionEnter(Collider2D coll){
		var collComp = coll.GetComponent<ColliderLayerComp>();
		if(!collComp) return;

		if(collComp.collLayers.ContainsKey("bullet")){
			TakeDamage();
		}
	}

	void TakeDamage(){
		if(hp > 0){
			hp--;
		}else{
			hitBox.enabled = false;
		}
	}

	
}