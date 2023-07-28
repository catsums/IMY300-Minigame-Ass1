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
        GameManager.Inst.Timer.SetInterval(()=>{
			print($"X");
			SpawnPlanet();
		}, Random.Range(spawnTime.x, spawnTime.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SpawnPlanet(){

		var template = MY.RandomArrayItem(templateObjects);

		var size = Random.Range(sizeRange.x, sizeRange.y);
		var yPos = Random.Range(yRange.x, yRange.y);

		var obj = Instantiate(template);
		
		
		obj.SetActive(false);
		obj.transform.SetParent(transform);

		var pos = obj.transform.position;
		var scale = obj.transform.localScale;
		pos.y = yPos;
		scale = (Vector2.one * size);

		obj.transform.localScale = scale;
		obj.transform.position = pos;

		obj.SetActive(true);


		// obj.transform.position = pos;

		// var bullet = obj.GetComponent<AttackBullet>();
		// bullet.moveDir = direction;
		// bullet.speed = speed;

		// obj.SetActive(true);
	}
}
