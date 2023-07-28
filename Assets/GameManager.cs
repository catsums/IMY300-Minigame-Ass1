using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameObjectExt;
using VEC2;
using SignalBusNS;
using Serializables;

public class GameManager : MonoBehaviour
{
	protected static GameManager _inst = null;

	public SignalBus signalBus = new SignalBus();

	TimerManager timerManager;
	public TimerManager Timer{
		get{
			if(!timerManager){
				timerManager = GetComponentInChildren<TimerManager>();
				if(timerManager != null){
					timerManager = gameObject.AddComponent<TimerManager>();
				}
			}
			return timerManager;
		}
	}
	
	public static GameManager Manager{
		get{ return Inst; }
	}public static GameManager Instance{
		get{ return Inst; }
	}public static GameManager Inst{
		get{ return _inst; }
	}

	void Awake(){
		if(_inst != null && _inst != this){
			Destroy(_inst);
		}
		DontDestroyOnLoad(gameObject);
		_inst = this;

	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
