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
	public bool alive = true;
	public PlayerInputCtrl InputCtrl;
	public PlayerMoveCtrl MoveCtrl;

	public Collider2D hitBox;

	public SpriteRenderer renderer;
	public Animator animator;
	public SignalBus signalBus = new SignalBus();

	public TimerManager timer;

	public float hurtTime = 1f;
	public float afterHurtTime = 3f;

	public string state = "run";
	void Awake() {
		
	}
	void Start(){
		// var timerComp = GetComponent<TimerComp>();
		// timerComp.signalBus.Subscribe("timeout", (x)=>{
		// 	print("meow");
		// });

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
		if(!animator){
			animator = GetComponentInChildren<Animator>();
		}
		if(!renderer){
			renderer = GetComponentInChildren<SpriteRenderer>();
		}
		if(!timer){
			timer = GetComponentInChildren<TimerManager>();
		}
		
	}

	void FixedUpdate()
	{
		if(!alive) return;

		switch(state){
			case "hurt":
				animator.Play("PlayerHurt");
				// print("Milk");
				break;
			case "dash":
				animator.Play("PlayerDash");
				break;
			case "run":
				animator.Play("PlayerRun");
				break;
			case "idle":
			default:
				animator.Play("PlayerIdle"); break;
		}
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
			state = "hurt";
			hitBox.enabled = false;
			timer.SetTimeout(()=>{
				var col = renderer.color;
				col.a = 0.5f;
				state = "run";
				timer.SetTimeout(()=>{
					hitBox.enabled = true;
					col.a = 1f;
				}, afterHurtTime);
			}, hurtTime);
		}else{
			DoDie();
		}
	}

	void DoDie(){
		state = "dead";
		alive = false;
		hitBox.enabled = false;
		animator.Play("PlayerDead");
	}
	
}