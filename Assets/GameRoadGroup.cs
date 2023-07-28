using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Serializables;

public class GameRoadGroup : MonoBehaviour
{
	public SerialList<GameObject> gameRoads = new SerialList<GameObject>();

	public float speed = 1f;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void FixedUpdate(){
		MoveRoads(Time.fixedDeltaTime);
		CheckRoads();
    }

	public void CheckRoads(){
		var roads = gameRoads.ToArray();
		foreach(var road in roads){
			if(!road){
				gameRoads.Remove(road);
				continue;
			}
			if(!road.activeSelf){
				gameRoads.Remove(road);
				RecycleRoad(road);
			}
		}
	}

	public void RecycleRoad(GameObject obj){
		
		var road = obj?.GetComponent<TileGridComp>();
		if(road){
			var lastRoad = gameRoads.PeekLast();
			gameRoads.Add(obj);
			obj.transform.SetParent(transform);
			obj.SetActive(true);
			
			var coll = lastRoad.GetComponentInChildren<Collider2D>();
			var objPosition = obj.transform.position;
			objPosition.x = coll.bounds.max.x;
			obj.transform.position = objPosition;
			
		}
	}

	public void MoveRoads(float delta){
		var roads = gameRoads.ToArray();
		foreach(var road in roads){
			var rigidBody = road.GetComponentInChildren<Rigidbody2D>();
			var dist = (speed * delta);
			Vector2 currPosition = rigidBody.transform.position;

			rigidBody?.MovePosition(
				(currPosition) + (Vector2.left * dist)
			);
		}
	}

}
