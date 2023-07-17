using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using SignalBusNS;
using StateMachineNS;
using VEC2;
using MyHelperFunctions;
using PlayerInputsNS;

public class PlayerInputCtrl : MonoBehaviour{
	[SerializeField]
	public MyInputAction[] myInputActions = new MyInputAction[0];

	[HideInInspector]
	public Dictionary<string, MyInputAction> inputs = new Dictionary<string, MyInputAction>();

	void Awake(){
		foreach(MyInputAction input in myInputActions){
			inputs[input.name] = input;
		}
	}

	void Start(){

	}

	void Update(){
		float delta = Time.unscaledDeltaTime;

		PlayerInputs.ProcessInputActions(inputs.Values.ToArray(), delta);
	}

	public MyInputAction GetInputAction(string name){
		if(inputs.ContainsKey(name)){
			return inputs[name];
		}
		return null;
	}

	public bool HasOnlyInputPressed(string n){
		bool yes = false;
		if(!inputs.ContainsKey(n)) return false;
		yes = inputs[n].IsPressed();

		foreach(var input in inputs.Values){
			if(input.name == n) continue;
			if(input.IsPressed()){
				yes = false; break;
			}
		}
		
		return yes;
	}
	public bool HasOnlyTheseInputsPressed(string[] myInputActions){
		List<string> _myInputActions = new List<string>(myInputActions);

		foreach(var input in inputs.Values){
			if(input.IsPressed() && !_myInputActions.Contains(input.name)){
				return false;
			}
		}
		return HasTheseInputsPressed(myInputActions);
	}
	public bool HasTheseInputsPressed(string[] myInputActions){
		bool[] flags = new bool[myInputActions.Length];
		for(int i=0;i<myInputActions.Length;i++){
			var myInput = myInputActions[i];
			if(!inputs.ContainsKey(myInput)) return false;
			flags[i] = inputs[myInput].IsPressed();
		}
		foreach(var flag in flags){
			if(!flag) return false;
		}
		return true;
	}

	void OnMove(InputValue val){
		if(!inputs.ContainsKey("Move")){
			return;
		}
		Vector2 input = val.Get<Vector2>();

		inputs["Move"].Set(input);

		if(input!=Vector2.zero){
			inputs["Move"].Press();
		}else{
			inputs["Move"].Release();
		}
	}
}