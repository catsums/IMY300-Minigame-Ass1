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

		var obj = MY.RandomArrayItem(templateObjects);
		
		// obj.SetActive(false);
		// obj.transform.SetParent(transform);

		// var index = UnityEngine.Random.Range(0, GameGrid.CellSpecs.y);

		// print($"index: {index}");

		// var gridCell = GameGrid.GetCell(index, 0);
		// var pos = obj.transform.position;
		// pos.y = gridCell.tileObject.transform.position.y;
		// pos.x = transform.position.x;
		// pos.z = transform.position.z + index;

		// obj.transform.position = pos;

		// var bullet = obj.GetComponent<AttackBullet>();
		// bullet.moveDir = direction;
		// bullet.speed = speed;

		// obj.SetActive(true);
	}
}
