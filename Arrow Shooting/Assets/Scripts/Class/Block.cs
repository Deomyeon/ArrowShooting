using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    protected Vector2Int position;
    protected Vector3 rotation;
    public BlockType type;

    public Vector2Int Rotation 
    {
        get
        { 
            switch(rotation.z)
            {
                case 0:
                    return Vector2Int.right;
                case 90:
                    return Vector2Int.up;
                case 180:
                    return Vector2Int.left;
                case 270:
                    return Vector2Int.down;
            }
            return Vector2Int.zero;
        }
        set 
        {
            if(value == Vector2Int.right)
            {
                rotation = new Vector3(0, 0, 0);
            }
            else if(value == Vector2Int.up)
            {
                rotation = new Vector3(0, 0, 90);
            }
            else if(value == Vector2Int.left)
            {
                rotation = new Vector3(0, 0, 180);
            }
            else if(value == Vector2Int.down)
            {
                rotation = new Vector3(0, 0, 270);
            }
        }
    }

    public Block()
    {

    }

    public void SetBlock(Vector2Int position, Vector2Int rotation, BlockType type)
    {
        this.position = position;
        this.Rotation = rotation;
        this.type = type;
        transform.position = MapManager.Instance.blockTransform[this.position].position;
        transform.rotation = Quaternion.Euler(this.rotation);
    }

    public void Move(Vector2Int position, Vector2Int rotation, BlockType type, bool arrow)
    {
        MapManager.Instance.canMove[position] = false;

        if (this.type == BlockType.Arrow)
        {
            (this as Arrow).power = arrow;
            this.GetComponent<SpriteRenderer>().sprite = MapManager.Instance.arrowImg[(this as Arrow).power ? 0 : 1];
        }

        transform.DOMove(MapManager.Instance.blockTransform[position].position, MapManager.blockMoveTime).OnComplete(() =>
        {
            this.position = position;
            if (this.Rotation != rotation)
            {
                this.Rotation = rotation;
                transform.DORotate(this.rotation, MapManager.blockMoveTime).OnComplete(() =>
                {
                    this.type = type;
                    MapManager.Instance.canMove[this.position] = true;
                });
            }
            else
            {
                this.type = type;
                MapManager.Instance.canMove[this.position] = true;
            }
        });
    }

    public virtual void MoveBlock(Vector2Int movePoint)
    {
        Vector2Int dest = this.position + new Vector2Int(movePoint.x, -movePoint.y);
        while (!MapManager.Instance.blockData.ContainsKey(dest) && !(dest.x < 0 || dest.y < 0 || dest.x >= MapManager.Instance.mapSize.x || dest.y >= MapManager.Instance.mapSize.y))
        {
            MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
            MapManager.Instance.blockData.Remove(this.position);

            this.position = dest;
            dest = this.position + new Vector2Int(movePoint.x, -movePoint.y);
        }
        if (!(MapManager.Instance.blockData[this.position].type == BlockType.Arrow))
        {
            MapManager.Instance.canMove[this.position] = false;
            transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
            {
                MapManager.Instance.canMove[this.position] = true;
            });
        }
    }

}

public enum BlockType
{
    None,
    Arrow,
    Target,
    Wall,
    Rotation,
    Single,
    Multiple,
    Jump
}