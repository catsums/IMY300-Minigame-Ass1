using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SignalBusNS;
using StateMachineNS;
using VEC2;
using MyHelperFunctions;
using PlayerInputsNS;
using GameObjectExt;

public class GameGrid : MonoBehaviour
{
	public static GameGrid Instance{
		get{
			return _inst;
		}
	}

	static GameGrid _inst = null;

	[Serializable]
	public class GridCell{
		public GameObject tileObject;
		public GridCell(GameObject obj){
			tileObject = obj;
		}
	}

	public GameObject[] cells = new GameObject[0];

	public Vector2Int cellSpecs = new Vector2Int(3,3);
	public static Vector2Int CellSpecs{
		set{ 
			Instance.cellSpecs = value;
		}get{ 
			return Instance.cellSpecs;
		}
	}
	public List<List<GridCell>> gridCells = new List<List<GridCell>>();
	public static List<List<GridCell>> GridCells{
		set{ 
			Instance.gridCells = value;
		}get{ 
			return Instance.gridCells;
		}
	}

	void OnEnable(){
		InitializeGrid();
	}
	void Awake(){
        if (_inst != null && _inst != this){
        	Destroy(this.gameObject);
        }else{
            _inst = this;
        }
    }

	void InitializeGrid(){
		Instance.initializeGrid();
	}void initializeGrid(){
		List<GameObject> cells = new List<GameObject>();
		for(int k=0;k<transform.childCount;k++){
			var child = transform.GetChild(k);
			var obj = child.gameObject;
			cells.Add(obj);
		}

		cells.Sort((a,b)=>{
			if(a==b) return 0;
			if(a==null) return 1;
			if(b==null) return -1;

			var aPos = a.transform.position;
			var bPos = b.transform.position;

			if(aPos.y < bPos.y){
				return -1;
			}else if(aPos.y == bPos.y){
				if(aPos.x < bPos.x) return -1;
				if(aPos.x > bPos.x) return 1;
				return 0;
			}else{
				return 1;
			}
		});

		this.cells = cells.ToArray();

		int i=0;
		for(int r=0;r<cellSpecs.y;r++){
			if(i>=cells.Count){
				break;
			}
			List<GridCell> row = new List<GridCell>();
			for(int c=0;c<cellSpecs.x;c++){
				if(i>=cells.Count){
					break;
				}
				GridCell cell = new GridCell(cells[i]);
				row.Add(cell);
				i++;
			}
			gridCells.Add(row);
		}
	}

	public static GridCell GetCell(int x, int y){
		return Instance.getCell(x,y);
	}GridCell getCell(int x, int y){
		if(x<0 || x>=gridCells.Count) return null;
		
		List<GridCell> row = gridCells[x];

		if(y<0 || y>=row.Count) return null;

		return row[y];
	}

	void Start(){

	}

	void Update(){

	}
}