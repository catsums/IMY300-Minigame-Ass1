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
	public PlayerInputCtrl InputCtrl;
	public PlayerMoveCtrl MoveCtrl;
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
	}

	void FixedUpdate()
	{
		
	}

	
}