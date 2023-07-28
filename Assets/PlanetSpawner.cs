using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Serializables;
using MyHelperFunctions;

public class PlanetSpawner : MonoBehaviour
{
	public SerialList<GameObject> templateObjects = new SerialList<GameObject>();

	public Vector2 direction = Vector2.left;

	public Vector2 speedRange = Vector2.right;
	public Vector2 spawnTime = Vector2.right;

	public Vector2 sizeRange = Vector2.right;
	public Vector2 yRange = new Vector2(-50,50);
    void Start()
    {
		print($"EGGS");
		TimerManager.TimerInstance timer = null;
    	timer = GameManager.Inst.Timer.SetInterval(()=>{

			print($"X");

			SpawnPlanet();
			timer.waitTime = Random.Range(spawnTime.x, spawnTime.y);

		}, Random.Range(spawnTime.x, spawnTime.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SpawnPlanet(){
		
		var template = templateObjects[Random.Range(0, templateObjects.Count)];

		var size = Random.Range(sizeRange.x, sizeRange.y);
		var speed = Random.Range(speedRange.x, speedRange.y);
		var yPos = Random.Range(yRange.x, yRange.y);

		var obj = Instantiate(template);
		
		
		obj.SetActive(false);
		obj.transform.SetParent(transform);

		var pos = obj.transform.position;
		var scale = obj.transform.localScale;
		pos.y = yPos;
		pos.x = transform.position.x;
		pos.z = size;
		scale = (Vector2.one * size);

		obj.transform.localScale = scale;
		obj.transform.position = pos;

		var planet = obj.GetComponent<MovingPlanet>();

		planet.speed = speed;
		planet.moveDir = direction;

		obj.SetActive(true);


		// obj.transform.position = pos;

		// var bullet = obj.GetComponent<AttackBullet>();
		// bullet.moveDir = direction;
		// bullet.speed = speed;

		// obj.SetActive(true);
	}
}
