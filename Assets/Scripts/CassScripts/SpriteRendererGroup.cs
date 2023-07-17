using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using SignalBusNS;
using VEC2;
using GameObjectExt;
using MyHelperFunctions;

[ExecuteInEditMode]
public class SpriteRendererGroup : MonoBehaviour
{
	Component[] Children {
		get {
			List<Component> list = new List<Component>();

			var sprRenderers = this.GetComponentsInDirectChildren<SpriteRenderer>();
			foreach(var comp in sprRenderers){
				list.Add(comp);
				if(!cache.ContainsKey(comp)){
					var sett = new CompSettings();

					sett.color = comp.color;
					sett.flipX = comp.flipX;
					sett.flipY = comp.flipY;

					cache[comp] = sett;
				}
			}
			var sprRendererGroups = this.GetComponentsInDirectChildren<SpriteRendererGroup>();
			foreach(var comp in sprRendererGroups){
				list.Add(comp);
				if(!cache.ContainsKey(comp)){
					var sett = new CompSettings();

					sett.color = comp.settings.color;
					sett.flipX = comp.settings.flipX;
					sett.flipY = comp.settings.flipY;

					cache[comp] = sett;
				}
			}

			return list.ToArray();
		}
	}

	public enum BlendMode{
		Default, Multiply, Add, Average, Blender
	}

	public class BlendAlgo{
		public static Color Blend(Color colA, Color colB, float val){
			Color col = colB;
			return Color.Lerp(colA, col, val);
		}
	}public class BlendAlgoMultiply{
		public static Color Blend(Color colA, Color colB, float val){
			Color col = (colA*colB);
			col.a = Mathf.Clamp(colA.a+colB.b,0f,1f);
			return Color.Lerp(colA, col, val);
		}
	}public class BlendAlgoAverage{
		public static Color Blend(Color colA, Color colB, float val){
			Color col = (colA+colB)/2;
			col.a = Mathf.Clamp(colA.a+colB.b,0f,1f);
			return Color.Lerp(colA, col, val);
		}
	}public class BlendAlgoAdd{
		public static Color Blend(Color colA, Color colB, float val){
			Color col = (colA+colB);
			col.a = Mathf.Clamp(colA.a+colB.b,0f,1f);
			return Color.Lerp(colA, col, val);
		}
	}public class BlendAlgoBlender{
		public static Color Blend(Color colA, Color colB, float val){
			// var t = Mathf.Clamp((colB.a-colA.a),0f,1f);
			float t;
			if(colB.a >= colA.a){
				t = Mathf.Clamp((2*colB.a-colA.a),0f,1f);
			}else{
				t = Mathf.Clamp((2*colA.a-colB.a),0f,1f);
			}

			Color col = Color.Lerp(colA,colB,t);
			col.a = Mathf.Clamp(colA.a+colB.b,0f,1f);

			return Color.Lerp(colA, col, val);
		}
	}


	public Color ColorBlend(Color colA, Color colB, BlendMode mode, float intensity = 1f){
		switch(mode){
			case BlendMode.Multiply: return BlendAlgoMultiply.Blend(colA,colB, intensity);
			case BlendMode.Add: return BlendAlgoAdd.Blend(colA,colB, intensity);
			case BlendMode.Average: return BlendAlgoAverage.Blend(colA,colB, intensity);
			case BlendMode.Blender: return BlendAlgoBlender.Blend(colA,colB, intensity);
			default: return BlendAlgo.Blend(colA,colB, intensity);
		}
	}

	public BlendMode blendMode = BlendMode.Multiply;
	private BlendMode oldBlendMode;
	[Range(0f,1f)]
	public float blendIntensity = 1f;
	private float oldBlendIntensity;

	[Serializable]
	public struct CompSettings{
		public Color color;
		public bool flipX;
		public bool flipY;
		[Range(0f,1f)]
		public float alpha;
	}

	Dictionary<Component, CompSettings> cache = new Dictionary<Component, CompSettings>();

	public CompSettings settings = new CompSettings();
	private CompSettings oldSettings = new CompSettings();
	// public Color color = Color.white;
	// private Color oldColor;
	// public bool flipX = false;
	// private bool oldFlipX;
	// public bool flipY = false;
	// private bool oldFlipY;
	// private Color _color;
	// public Color color{
	// 	get { return _color; }
	// 	set {
	// 		_color = value;
	// 		UpdateGroup();
	// 	}
	// }
	// private bool _flipX;
	// public bool flipX{
	// 	get { return _flipX; }
	// 	set {
	// 		_flipX = value;
	// 		UpdateGroup();
	// 	}
	// }
	// private bool _flipY;
	// public bool flipY{
	// 	get { return _flipY; }
	// 	set {
	// 		_flipY = value;
	// 		UpdateGroup();
	// 	}
	// }
	void Start() {
		settings.color = Color.white;
		settings.flipX = false;
		settings.flipY = false;
		settings.alpha = 1f;
	}

	void OnEnable(){
		UpdateGroup();
	}

	void OnDisable(){
		var sprRenderers = this.GetComponentsInDirectChildren<SpriteRenderer>();
		foreach(var comp in sprRenderers){
			if(cache.ContainsKey(comp)){
				var sett = cache[comp];

				comp.color = sett.color;
				comp.flipX = sett.flipX;
				comp.flipY = sett.flipY;

				cache.Remove(comp);
			}
		}
		var sprRendererGroups = this.GetComponentsInDirectChildren<SpriteRendererGroup>();
		foreach(var comp in sprRendererGroups){
			if(cache.ContainsKey(comp)){
				var sett = cache[comp];

				comp.settings.color = sett.color;
				comp.settings.flipX = sett.flipX;
				comp.settings.flipY = sett.flipY;

				cache.Remove(comp);
			}
		}
	}
	
	void Update()
	{
		if(HasChanges()){
			UpdateGroup();
			UpdateSettings();
		}
	}
	void OnValidate() {
		UpdateGroup();
		UpdateSettings();
	}

	private CompSettings[] oldChilds = new CompSettings[0];

	bool HasChanges(){

		var childs = Children;
		if(childs.Length != oldChilds.Length) return true;
		// foreach(var compSett in oldChilds){
			
		// }

		if(oldSettings.flipX != settings.flipX) return true;
		if(oldSettings.flipY != settings.flipY) return true;
		if(oldSettings.color != settings.color) return true;
		if(oldSettings.alpha != settings.alpha) return true;

		if(oldBlendMode != blendMode) return true;
		if(oldBlendIntensity != blendIntensity) return true;

		return false;
	}

	void UpdateSettings(){
		oldSettings = (CompSettings) settings;
		oldChilds = cache.Values.ToArray<CompSettings>();
		
		oldBlendMode = blendMode;
		oldBlendIntensity = blendIntensity;
	}

	void UpdateGroup(){
		if(!enabled) return;

		var sprRenderers = this.GetComponentsInDirectChildren<SpriteRenderer>();
		foreach(var comp in sprRenderers){
			CompSettings sett;
			if(!cache.ContainsKey(comp)){
				sett = new CompSettings();

				sett.color = comp.color;
				sett.flipX = comp.flipX;
				sett.flipY = comp.flipY;

				cache[comp] = sett;
			}

			sett = cache[comp];

			Color col = ColorBlend(sett.color, settings.color, blendMode, blendIntensity);
			col.a *= settings.alpha;
			comp.color = col;
			comp.flipX = sett.flipX != settings.flipX;
			comp.flipY = sett.flipY != settings.flipY;

		}
		var sprRendererGroups = this.GetComponentsInDirectChildren<SpriteRendererGroup>();
		foreach(var comp in sprRendererGroups){
			CompSettings sett;
			if(!cache.ContainsKey(comp)){
				sett = new CompSettings();

				sett.color = comp.settings.color;
				sett.flipX = comp.settings.flipX;
				sett.flipY = comp.settings.flipY;

				cache[comp] = sett;
			}

			sett = cache[comp];

			Color col = ColorBlend(sett.color, settings.color, blendMode, blendIntensity);
			col.a *= settings.alpha;
			comp.settings.color = col;
			comp.settings.flipX = sett.flipX != settings.flipX;
			comp.settings.flipY = sett.flipY != settings.flipY;
		}
	}

}