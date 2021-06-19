using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Single : Block
{



    public override void MoveBlock(Vector2Int movePoint)
    {
        if (this.Rotation == movePoint)
        {
            Vector2Int dest = this.position + new Vector2Int(movePoint.x, -movePoint.y);
            while (!MapManager.Instance.blockData.ContainsKey(dest) && !(dest.x < 0 || dest.y < 0 || dest.x >= MapManager.Instance.mapSize.x || dest.y >= MapManager.Instance.mapSize.y))
            {
                MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
                MapManager.Instance.blockData.Remove(this.position);

                this.position = dest;
                dest = this.position + new Vector2Int(movePoint.x, -movePoint.y);
            }

            MapManager.Instance.canMove[this.position] = false;
            transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
            {
                MapManager.Instance.canMove[this.position] = true;
            });
        }
    }

}
