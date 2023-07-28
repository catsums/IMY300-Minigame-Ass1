using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
	public float speed = 1f;
	public Vector2 direction = Vector2.left;
	public GameObject templateBullet;
	public float spawnTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.Timer.SetInterval(()=>{
			SpawnBullet();
		}, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void SpawnBullet(){
		if(!templateBullet) return;
		var obj = Instantiate(templateBullet);
		obj.SetActive(false);
		obj.transform.SetParent(transform);

		var index = UnityEngine.Random.Range(0, GameGrid.CellSpecs.y);

		print($"index: {index}");

		var gridCell = GameGrid.GetCell(index, 0);
		var pos = obj.transform.position;
		pos.y = gridCell.tileObject.transform.position.y;
		pos.x = transform.position.x;
		pos.z = transform.position.z + index;

		obj.transform.position = pos;

		var bullet = obj.GetComponent<AttackBullet>();
		bullet.moveDir = direction;
		bullet.speed = speed;

		obj.SetActive(true);
	}
}
