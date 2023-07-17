using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class FadeableObject : MonoBehaviour
{
	public Collider2D meshCollider;
	public ColliderLayerComp colliderComp;
	public Renderer objectRenderer;

	private float oldAlpha;

	public float alpha = 0.45f;
    // Start is called before the first frame update
    void Start()
    {
        if(!meshCollider){
			meshCollider = GetComponent<Collider2D>();
		}
		if(!colliderComp){
			colliderComp = GetComponent<ColliderLayerComp>();
		}
		if(!objectRenderer){
			objectRenderer = GetComponentInParent<Renderer>();
		}
		if(objectRenderer){
			oldAlpha = (objectRenderer.material.color).a;
		}
    }
	void OnEnable(){
		if(!objectRenderer){
			objectRenderer = GetComponentInParent<Renderer>();
			oldAlpha = (objectRenderer.material.color).a;
		}
	}
	void OnDisable(){
		Color _color = objectRenderer.material.color;
		_color.a = oldAlpha;
	}

    // Update is called once per frame
    void Update(){
        CheckCollision();
    }

	bool CheckCollision(){
		float delta = Time.deltaTime;
		List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
		int count = meshCollider.Cast(Vector2.zero, new ContactFilter2D(), castCollisions, 0);
		count = 0;

		foreach (var castColl in castCollisions){
			if (!castColl) continue;
			Collider2D collider = castColl.collider;
			var xyz = collider?.GetComponent<ColliderLayerComp>();
			if (colliderComp.HasCollision(xyz)){
				float playerPosY = collider.transform.position.y;
				float objectY = transform.position.y;

				if(playerPosY>objectY) count++;
			}
		}

		Color _color = objectRenderer.material.color;
		if (count > 0){
			_color.a = alpha;
		}else{
			_color.a = oldAlpha;
		}
		objectRenderer.material.color = _color;
		return false;
	}
}
