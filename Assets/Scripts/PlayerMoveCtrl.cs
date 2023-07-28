using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SignalBusNS;
using StateMachineNS;
using VEC2;
using MyHelperFunctions;
using PlayerInputsNS;
using GameObjectExt;

public class PlayerMoveCtrl : MonoBehaviour
{
	public PlayerMain playerMain;
	public Rigidbody2D kinematicBody;

	public Vector2 inputDir = Vector2.zero;
	public Vector2 moveDir = Vector2.zero;
	public Vector2Int cellPos = Vector2Int.zero;

	public float moveSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        if(!playerMain){
			playerMain = GetComponent<PlayerMain>();
		}
        if(!kinematicBody){
			kinematicBody = GetComponent<Rigidbody2D>();
		}

		cellPos = GameGrid.Instance.cellSpecs / 2;

		TryMove(Vector2.zero);
    }

    // Update is called once per frame
    void FixedUpdate(){
		var moveInput = playerMain.InputCtrl.GetInputAction("Move");

        if(moveInput!=null && moveInput.IsJustPressed()){
			inputDir = moveInput.Get<Vector2>();
			TryMove(inputDir);
		}else{
			inputDir = Vector2.zero;
		}
    }

	void TryMove(Vector2 moveVec){
		moveVec = moveVec.normalized;


		Vector2Int moveDir = new Vector2Int(Mathf.RoundToInt(moveVec.x), Mathf.RoundToInt(moveVec.y));

		Vector2Int newPos = cellPos + moveDir;

		var gridCell = GameGrid.GetCell(newPos.y, newPos.x);

		if(gridCell==null){
			return;
		}

		var newDepth = (float) newPos.y;

		cellPos = newPos;
		// print($"moveVec: {moveVec}");

		var newPosition = gridCell.tileObject.transform.position;
		newPosition.z = transform.parent.position.z + newDepth;

		var initPosition = kinematicBody.transform.position;
		initPosition.z = newPosition.z;

		kinematicBody.transform.position = initPosition;

		var moveTime = (1/(Mathf.Abs(moveSpeed)+0.001f)) * Time.fixedDeltaTime;


		var x = LeanTween.move(kinematicBody.gameObject, newPosition, moveTime);
		// x.setOnComplete(()=>{
		// 	print("milk");
		// });
		// LeanTween.addListener()

		// kinematicBody.MovePosition(newPosition);

	}
}
