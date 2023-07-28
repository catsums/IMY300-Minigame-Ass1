using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlanet : MonoBehaviour
{
	public float speed = 1f;
	public Collider2D hitBox;

	public Vector2 moveDir = Vector2.left;

	Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        if(!hitBox) hitBox = GetComponent<Collider2D>();
		var hitBoxComp = hitBox.GetComponent<ColliderLayerComp>();
		if(hitBoxComp) hitBoxComp.OnDetectCollisionEnter(OnDetectCollisionEnter);
		rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveBullet(moveDir, Time.fixedDeltaTime);
    }

	void MoveBullet(Vector2 moveDir, float delta){
		if(!rigidbody) return;

		var dist = (speed * delta);
		Vector2 currPos = transform.position;

		var x = LeanTween.move(rigidbody.gameObject, currPos + (dist * moveDir), delta);
		var collComp = hitBox.GetComponent<ColliderLayerComp>();
		// LeanTween.add()
		var first = collComp.depthStart;
		var last = first + (dist * moveDir.y);
		x.setOnUpdate((Action<float>)((t)=>{
			collComp.depthStart = Mathf.Lerp(first, last, t);
		}));
		x.setOnComplete(()=>{
			var collComp = hitBox.GetComponent<ColliderLayerComp>();
			collComp.depthStart += (dist * moveDir.y);
		});

	}
	void OnDetectCollisionEnter(Collider2D coll){
		var collComp = coll.GetComponent<ColliderLayerComp>();
		if(!collComp) return;

		if(collComp.collLayers.ContainsKey("despawner")){
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
