using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SignalBusNS;
using VEC2;
using GameObjectExt;
using MyHelperFunctions;
using Unity.Collections;

[ExecuteInEditMode]
public class SpriteComposite : MonoBehaviour
{
	[Serializable]
	public class SpriteData{
		public Sprite sprite = null;
		public Transform2D transform = new Transform2D();
		public Color color = Color.clear;
	}
	[SerializeField]
	public SpriteData[] sprites = new SpriteData[0];
	private SpriteData[] _oldSprites = new SpriteData[0];
	public SpriteRenderer spriteRenderer;
	public Vector2 pivot = Vector2.zero;
	private Vector2 _oldPivot;

	[Range(0f,1f)]
	public float alpha = 1f;
	private float _oldAlpha;

	[ReadOnly][SerializeField]
	private Rect rrect = Rect.zero;

	public Rect rect{
		get{
			Rect newRect = new Rect(Vector2.positiveInfinity, Vector2.zero);

			List<Rect2D> rects = new List<Rect2D>();

			foreach(var spr in sprites){
				if(!spr.sprite) continue;

				Rect sprRect = spr.sprite.rect;

				Transform2D t0 = spr.transform;
				t0.anchor = spr.sprite.pivot;
				Rect2D r0 = new Rect2D(sprRect.position, sprRect.size);

				Rect2D r1 = Rect2D.From(new Vector2[]{
					// t0.ApplyTransform(r0.topLeft),
					// t0.ApplyTransform(r0.bottomRight)
					r0.min, r0.max
				});

				rects.Add(r1);

				// //anchor
				// Vector2 _pivot = spr.sprite.pivot;
				// //scale
				// sprRect.size *= spr.scale;
				// //rotate
				// sprRect.min.RotateAround(_pivot, spr.rotation);
				// sprRect.max.RotateAround(_pivot, spr.rotation);
				// //transform
				// sprRect.position += spr.position;

				// //SRT

				// if(sprRect.x < newRect.x) newRect.x = sprRect.x;
				// if(sprRect.y < newRect.y) newRect.y = sprRect.y;

				// if(sprRect.width > newRect.width) newRect.width = sprRect.width;
				// if(sprRect.height > newRect.height) newRect.height = sprRect.height;

			}

			if(rects.Count>0){
				Rect2D newRect2D = Rect2D.COMBINE(rects.ToArray());
				newRect = new Rect(newRect2D.position, newRect2D.size);
			}

			rrect = newRect;

			return newRect;

		}
	}
	void Start(){
		if(!spriteRenderer){
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		UpdateParams();
		CombineSprites();
	}
	void LateUpdate(){
		if(HasChanged()){
			CombineSprites();
		}
	}

	void CombineSprites(){
		Resources.UnloadUnusedAssets();
		// print($"rect: {rect}");
		var newTex = new Texture2D((int)(rect.width), (int)(rect.height));

		for(int x=0; x<newTex.width; x++){
			for(int y=0; y<newTex.height; y++){
				newTex.SetPixel(x,y, Color.clear);
			}
		}

		foreach(var spr in sprites){
			if(!spr.sprite) continue;
			var sprRect = spr.sprite.textureRect;

			// //anchor
			// Vector2 _pivot = spr.sprite.pivot;
			// //scale
			// sprRect.size *= spr.scale;
			// //rotate
			// sprRect.min.RotateAround(_pivot, spr.rotation);
			// sprRect.max.RotateAround(_pivot, spr.rotation);
			// //transform
			//Transform2D t0 = spr.transform;
			//	t0.anchor = spr.sprite.pivot;
			// sprRect.position += spr.position;

			for(int x=0; x<newTex.width; x++){
				if(sprRect.xMax <= x) break;

				for(int y=0; y<newTex.height; y++){
					if(sprRect.yMax <= y) break;

					var pixel = newTex.GetPixel(x,y);
					var sprPixel = spr.sprite.texture.GetPixel(
						(int) x + (int) sprRect.x,
						(int) y + (int) sprRect.y
					);
					var t = Mathf.Clamp(value: ( 2 * sprPixel.a - pixel.a ),0f,1f);

					Color col = Color.Lerp(pixel,sprPixel,t);
					col.a = Mathf.Clamp(sprPixel.a + pixel.a,0f,1f);

					var newPixel = col;
					newPixel.a *= alpha;
					newTex.SetPixel(x,y, newPixel);
				}

			}
		}

		newTex.Apply();

		Rect _rect = new Rect(Vector2.zero, new Vector2(newTex.width, newTex.height));

		var newSprite = Sprite.Create(newTex, _rect, pivot);
		newSprite.name = $"{name}-Sprite";

		if(spriteRenderer){
			spriteRenderer.sprite = newSprite;
		}

		UpdateParams();

	}

	void UpdateParams(){
		_oldSprites = (SpriteData[]) sprites.Clone();
		_oldPivot = new Vector2(pivot.x, pivot.y);
		_oldAlpha = alpha;
	}
	bool HasChanged(){
		if(_oldSprites==null) return true;
		if(_oldSprites.Length != sprites.Length) return true;
		for(int i=0;i<sprites.Length;i++){
			if(_oldSprites[i].sprite != sprites[i].sprite) return true;
			if(_oldSprites[i].color != sprites[i].color) return true;
			if(!_oldSprites[i].transform.Equals(sprites[i].transform)) return true;
		}

		if(_oldPivot.x != pivot.x || _oldPivot.y != pivot.y) return true;
		if(_oldAlpha != alpha) return true;

		return false;
	}
}