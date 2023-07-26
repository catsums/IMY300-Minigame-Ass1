using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SignalBusNS;
using GameObjectExt;
using MyHelperFunctions;

public class SignalBusComp : MonoBehaviour
{
	static Dictionary<GameObject,SignalBusComp> buses = new Dictionary<GameObject,SignalBusComp>();
	
	public string[] signals = new string[0];
	SignalBus signalBus = new SignalBus();

	void Awake(){
		buses[gameObject] = this;
	}

	void Start(){
		InitializeSignals();
	}

	void InitializeSignals(){
		foreach(var signal in signals){
			signalBus.CreateSignal(signal);
		}
	}

	void OnValidate() {
		InitializeSignals();
	}
	
	public void Emit(string n){
		signalBus.Emit(n);
	}public void Emit(string n, object x){
		signalBus.Emit(n,x);
	}public void Emit<T>(string n, T x){
		signalBus.Emit(n,x);
	}

	public SignalBusComp Subscribe(string n, Action func){
		signalBus.Subscribe(n,func);
		return this;
	}public SignalBusComp Subscribe(string n, Action<object> func){
		signalBus.Subscribe(n,func);
		return this;
	}public SignalBusComp Subscribe<T>(string n, Action<T> func){
		signalBus.Subscribe(n,func);
		return this;
	}
	public SignalBusComp Unsubscribe(string n, Action func){
		signalBus.Unsubscribe(n,func);
		return this;
	}public SignalBusComp Unsubscribe(string n, Action<object> func){
		signalBus.Unsubscribe(n,func);
		return this;
	}public SignalBusComp Unsubscribe<T>(string n, Action<T> func){
		signalBus.Unsubscribe(n,func);
		return this;
	}

	public bool IsSubscribed(string n, Action func){
		return signalBus.IsSubscribed(n,func);
	}public bool IsSubscribed(string n, Action<object> func){
		return signalBus.IsSubscribed(n,func);
	}public bool IsSubscribed<T>(string n, Action<T> func){
		return signalBus.IsSubscribed(n,func);
	}

}