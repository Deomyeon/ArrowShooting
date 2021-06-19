using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBlock
{
    public Vector2Int position;
    public Vector2Int rotation;
    public BlockType type;

    public VirtualBlock(Vector2Int position, Vector2Int rotation, BlockType type)
    {
        this.position = position;
        this.rotation = rotation;
        this.type = type;
    }
}
