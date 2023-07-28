using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TGridSpawner : MonoBehaviour
{
	ColliderLayerComp colliderComp;
    // Start is called before the first frame update
    void Start()
    {
		if(!colliderComp){
			colliderComp = GetComponent<ColliderLayerComp>();
		}
		if(colliderComp){
			colliderComp.OnDetectCollisionEnter(OnDetectCollisionEnter);
			// colliderComp.signalBus.On(ColliderLayerComp.Signals.CollisionEnter, (Action<Collider2D>) OnDetectCollisionEnter);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnDetectCollisionEnter(Collider2D coll){
		var collComp = coll.GetComponent<ColliderLayerComp>();
		if(collComp.collLayers.ContainsKey("tGridPart")){
			var gridPart = coll.GetComponentInParent<TileGridComp>();
			if(gridPart){
				gridPart.gameObject.SetActive(false);
			}
			// Destroy(coll.gameObject);
		}
	}
	

}
