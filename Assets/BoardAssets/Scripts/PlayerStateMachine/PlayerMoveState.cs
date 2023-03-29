using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * Changes:
 * changed _spd to use a flat rate (otherwise you can get really wierd speed shifting around bends or unusual spacing)
 * added _currentTileBezier
 * added _bezierPoint to tileclass
 * added OnDrawGizmosSelected() to tileclass that draws bezier curves when inspecting a tile
 */

public class PlayerMoveState : PlayerBaseState
{
    Vector3 _nextTilePos;
    Vector3 _currentTileBezier;
    Vector3 _moveVector; //unused in bezier implementation

    //float _spd;
    float _startTime;
    float _timeToCoverDistance;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("EntersState");
        _nextTilePos = player.currentTile.GetComponent<TileClass>().GetNextTilePos();
        //Vector3 _travelVector = _nextTilePos - player.transform.position;
        //Vector3 _travelDir = _travelVector.normalized;
        //float _spd = _travelVector.magnitude / player.moveTime;
        //_moveVector = _spd * _travelDir;
        _currentTileBezier = player.currentTile.GetComponent<TileClass>().bezierOffset;
        _timeToCoverDistance = BezierCurve.GetBezierLength(player.currentTile.transform.position, _nextTilePos, player.currentTile.transform.position + _currentTileBezier, 5) / player.moveTime;
        _startTime = Time.time;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.toMove > 0)
        {
            if ((Time.time - _startTime + Time.deltaTime) / _timeToCoverDistance > 1)//(_nextTilePos - player.transform.position).magnitude <= (_moveVector * Time.deltaTime).magnitude)
            {
                //Debug.Log("toMove = " + player.toMove + ", currentTile = " + player.currentTile);
                player.transform.position = _nextTilePos;
                player.currentTile = player.currentTile.GetComponent<TileClass>().nextTile[0];
                player.SwitchState(player.idleState);
                //is this debug or are we keeping this material switching?
                if(player.currentTile.GetComponent<TileClass>().previousTile[0].GetComponent<BasicTile>().tileOccupied)
                {
                    player.currentTile.GetComponent<TileClass>().previousTile[0].transform.GetChild(0).GetComponent<Renderer>().material  = (Material)AssetDatabase.LoadAssetAtPath("Assets/BoardAssets/Materials/TestTileUnoccupiedMat.mat", typeof(Material));
                }
                player.currentTile.GetComponent<BasicTile>().tileOccupied = true;
                player.currentTile.transform.GetChild(0).GetComponent<Renderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/BoardAssets/Materials/TestTileOccupiedMat.mat", typeof(Material));

                //calculates the time to cross the curve
                _currentTileBezier = player.currentTile.GetComponent<TileClass>().bezierOffset;
                _timeToCoverDistance = BezierCurve.GetBezierLength(player.currentTile.transform.position, _nextTilePos,player.currentTile.transform.position + _currentTileBezier, 5) / player.moveTime;
                _startTime = Time.time;

                player.toMove-=1;
            }
            else
            {
                //Debug for using the Bezier Curves
                //Debug.DrawLine(player.transform.position,player.currentTile.transform.position,Color.blue);
                //Debug.DrawLine(player.transform.position,_nextTilePos,Color.magenta);
                //Debug.DrawLine(player.transform.position, player.currentTile.transform.position + _currentTileBezier, Color.green);

                //moves to position on bezier curve
                player.transform.position = BezierCurve.PositionOnCurve(player.currentTile.transform.position, _nextTilePos,player.currentTile.transform.position+_currentTileBezier,Mathf.Clamp((Time.time-_startTime)/_timeToCoverDistance,0,1));
                Vector3 lookDirection = player.transform.position - BezierCurve.PositionOnCurve(player.currentTile.transform.position, _nextTilePos, player.currentTile.transform.position + _currentTileBezier, Mathf.Clamp((Time.time - _startTime + Time.deltaTime) / _timeToCoverDistance,0,1));
                player.transform.rotation = Quaternion.LookRotation(lookDirection);
                //player.transform.position += _moveVector * Time.deltaTime; //If you do keep this approach, use Vector3.Lerp to make it feel like moving pieces on a board
            }
            
        }

    }
    public override void ExitState(PlayerStateManager player)
    {

    }

    private void MoveToNextTile(PlayerStateManager player)
    {
        
    }

}
