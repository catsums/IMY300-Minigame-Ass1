using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using SignalBusNS;
using VEC2;
using MyHelperFunctions;

[ExecuteInEditMode]
public class ColliderLayerComp : MonoBehaviour
{
	public static List<string> allLayers = new List<string>();
	public SignalBus signalBus = new SignalBus();

	public string colliderName = "";

	public string[] collisionLayers = new string[1];
	public Dictionary<string, bool> collLayers = new Dictionary<string, bool>();
	public string[] collisionTargets = new string[1];
	public Dictionary<string, bool> collTargets = new Dictionary<string, bool>();

	public int priority = 0;

	public bool isEnabled = true;
	public bool canDetectNormalCollider = true;

	public bool useDepthDetection = false;
	public float depth = 1f;
	public float depthStart{
		set{
			Vector3 pos = transform.position;
			pos.z = value;
			transform.position = pos;
		}
		get{
			return transform.position.z;
		}
	}
	public float depthEnd{
		set{
			depth = value - depthStart;
		}
		get{
			return (depthStart + depth);
		}
	}

	public Vector2 depthRange{
		set{
			depthStart = value.x;
			depthEnd = value.y;
		}
		get{
			return new Vector2(depthStart, depthEnd);
		}
	}

	protected bool isInDepth(float val){
		return (val >= depthStart && val < depthEnd);
	}
	protected bool isInDepth(float min, float max){
		return (
			isInDepth(min) || isInDepth(max)
		);
	}
	protected bool isInDepth(Vector2 val){
		return (
			isInDepth(val.x) || isInDepth(val.y)
		);
	}

	void Awake()
	{
		foreach (string layer in collisionLayers)
		{

			collLayers[layer] = true;

			if (!allLayers.Contains(layer))
			{
				allLayers.Add(layer);
			}
		}
		foreach (string layer in collisionTargets)
		{

			collTargets[layer] = true;

			if (!allLayers.Contains(layer))
			{
				allLayers.Add(layer);
			}
		}
	}


	void Start()
	{

	}

	[SerializeField]
	private Vector2 _depthRange = Vector2.negativeInfinity;
	
	void Update(){
		if(_depthRange != depthRange){
			_depthRange = depthRange;
		}

	}

	void FixedUpdate()
	{

	}

	public bool CanDetectCollision(ColliderLayerComp other){
		if(!other) return false;
		if(!isEnabled || !other.isEnabled) return false;

		foreach (var layer in other.collLayers.Keys){
			if(collTargets.ContainsKey(layer) && collTargets[layer]){
				if(useDepthDetection && !isInDepth(other.depthRange)){
					// continue;
				}else{
					return true;
				}
			}
		}
		return false;
	}

	public List<string> GetCollisionsFrom(ColliderLayerComp other)
	{
		List<string> colls = new List<string>();
		if(!other) return colls;
		if (!isEnabled || !other.isEnabled) return colls;

		foreach (var layer in other.collLayers.Keys){
			if(collTargets.ContainsKey(layer) && collTargets[layer])
			{
				if(other.priority > this.priority) continue;

				colls.Add(layer);
			}
		}
		return colls;
	}
	public bool HasCollision(ColliderLayerComp other){
		if(!other) return false;
		if(!isEnabled || !other.isEnabled) return false;

		foreach (var layer in other.collLayers.Keys){
			if(collTargets.ContainsKey(layer) && collTargets[layer]){
				if(other.priority > this.priority) continue;

				if(useDepthDetection && !isInDepth(other.depthRange)){
					// continue;
				}else{
					return true;
				}
			}
		}
		return false;
	}
	public bool HasCollision(ColliderLayerComp other, string[] layers){
		if(!other || layers==null || layers.Length==0) return false;
		if(!isEnabled || !other.isEnabled) return false;

		int count = layers.Length;

		foreach (var layer in other.collLayers.Keys){
			// Debug.Log($"Layer:{layer}");
			// Debug.Log("abc");
			if(!layers.Contains<string>(layer)) continue;
			// Debug.Log("def");
			if(collTargets.ContainsKey(layer) && collTargets[layer]){
				// Debug.Log(message: "ghi");
				if(other.priority > this.priority) continue;
				// Debug.Log("jkl");
				if(useDepthDetection && !isInDepth(other.depthRange)){
					// continue;
				}else{
					count--;
				}
			}
			// Debug.Log("xyz");
		}
		// Debug.Log($"Count:{count}");
		return (count==0);
	}

	public bool DetectCollision(Collider2D collider){
		if(!collider) return false;
		if(!isEnabled) return false;
		
		bool detected = false;
		if (collider == null) return false;
		ColliderLayerComp other = null;
		collider.TryGetComponent<ColliderLayerComp>(out other);

		if (!other){
			detected = canDetectNormalCollider;
			// Debug.Log("X so detected: "+detected);
		}else{
			detected = HasCollision(other);
			// Debug.Log("Y so detected: "+detected);
		}

		if (detected){
			signalBus.Emit("DetectedCollision", other);
		}

		return detected;

	}
	public bool DetectCollision(Collider2D collider, string[] layers){
		if(!collider) return false;
		if(!isEnabled) return false;
		
		bool detected = false;
		if (collider == null) return false;
		ColliderLayerComp other = null;
		collider.TryGetComponent<ColliderLayerComp>(out other);

		if (!other) detected = canDetectNormalCollider;
		else detected = HasCollision(other, layers);

		if (detected){
			signalBus.Emit("DetectedCollision", other);
		}

		return detected;

	}

	public void EnableCollisionForLayer(string layer)
	{
		if (!collLayers.ContainsKey(layer)) return;
		collLayers[layer] = true;
	}
	public void DisableCollisionForLayer(string layer)
	{
		if (!collLayers.ContainsKey(layer)) return;
		collLayers[layer] = false;
	}
	public void ToggleCollisionForLayer(string layer)
	{
		if (!collLayers.ContainsKey(layer)) return;
		collLayers[layer] = !collLayers[layer];
	}
	public void EnableCollisionForTarget(string layer)
	{
		if (!collTargets.ContainsKey(layer)) return;
		collTargets[layer] = true;
	}
	public void DisableCollisionForTarget(string layer)
	{
		if (!collTargets.ContainsKey(layer)) return;
		collTargets[layer] = false;
	}
	public void ToggleCollisionForTarget(string layer)
	{
		if (!collTargets.ContainsKey(layer)) return;
		collTargets[layer] = !collTargets[layer];
	}
}