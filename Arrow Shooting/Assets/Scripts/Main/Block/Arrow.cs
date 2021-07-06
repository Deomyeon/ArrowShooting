using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arrow : Block
{

    public bool power;
    public bool arrowMove;

    private Arrow()
    {
        power = true;
        arrowMove = false;
    }

    public void Action(Vector2Int movePoint)
    {
        Vector2Int dest = this.position + new Vector2Int(movePoint.x, -movePoint.y);
        if (MapManager.Instance.blockData.ContainsKey(dest) && !(dest.x < 0 || dest.y < 0 || dest.x >= MapManager.Instance.mapSize.x || dest.y >= MapManager.Instance.mapSize.y))
        {
            if (MapManager.Instance.blockData[dest].type == BlockType.Arrow)
            {
                if (!(MapManager.Instance.blockData[dest] as Arrow).power && (MapManager.Instance.blockData[dest] as Arrow).arrowMove)
                {
                    MapManager.Instance.canMove[dest] = false;

                    arrowMove = true;
                    if (transform.childCount != 0)
                    {
                        if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                        {
                            transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                        }
                    }
                    transform.DOMove(MapManager.Instance.blockTransform[dest].position, MapManager.blockMoveTime).OnComplete(() =>
                    {
                        arrowMove = false;
                        MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
                        MapManager.Instance.blockData.Remove(this.position);

                        this.position = dest;
                        MapManager.Instance.canMove[this.position] = true;
                    });
                }
                else
                {

                    MapManager.Instance.canMove[this.position] = false;
                    if (transform.childCount != 0)
                    {
                        if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                        {
                            transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                        }
                    }
                    transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                    {
                        MapManager.Instance.canMove[this.position] = true;
                    });
                }
                return;
            }
        }
        if (MapManager.Instance.blockData.ContainsKey(dest) && !(dest.x < 0 || dest.y < 0 || dest.x >= MapManager.Instance.mapSize.x || dest.y >= MapManager.Instance.mapSize.y) && this.Rotation == movePoint)
        {
            GameObject RemoveObject = null;
            switch (MapManager.Instance.blockData[dest].type)
            {
                case BlockType.Target:
                    if (power)
                    {
                        MapManager.Instance.canMove[dest] = false;

                        RemoveObject = MapManager.Instance.blockData[dest].gameObject;

                        power = false;

                        arrowMove = true;

                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[dest].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            RemoveObject.SetActive(false);

                            arrowMove = false;

                            MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
                            MapManager.Instance.blockData.Remove(this.position);

                            this.position = dest;
                            this.GetComponent<SpriteRenderer>().sprite = MapManager.Instance.arrowImg[power ? 0 : 1];

                            if (transform.childCount != 0)
                            {
                                if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                                {
                                    transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color(0.25f, 0.3f, 1f, 1f);
                                    transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color(0.25f, 0.3f, 1f, 0f);
                                }
                            }
                            ParticleManager.Instance.MakeParticle(transform.position);

                            MapManager.Instance.GetComponent<AudioSource>().Play();
                            transform.DOScale(1.4f, 0.1f).OnComplete(() =>
                            {
                                transform.DOScale(1, 0.2f).OnComplete(() =>
                                {

                                    MapManager.Instance.canMove[this.position] = true;
                                });
                            });
                        });
                    }
                    else
                    {
                        MapManager.Instance.canMove[this.position] = false;
                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            MapManager.Instance.canMove[this.position] = true;
                        });
                    }
                    // ���� ��
                    break;
                case BlockType.Rotation:

                    MapManager.Instance.canMove[dest] = false;

                    RemoveObject = MapManager.Instance.blockData[dest].gameObject;

                    Vector2Int rotation = MapManager.Instance.blockData[dest].Rotation;
                    MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
                    MapManager.Instance.blockData.Remove(this.position);

                    arrowMove = true;

                    if (transform.childCount != 0)
                    {
                        if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                        {
                            transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                        }
                    }
                    transform.DOMove(MapManager.Instance.blockTransform[dest].position, MapManager.blockMoveTime).OnComplete(() =>
                    {

                        RemoveObject.SetActive(false);

                        arrowMove = false;

                        this.position = dest;
                        if (this.Rotation != rotation)
                        {
                            this.Rotation = rotation;

                            MapManager.Instance.GetComponent<AudioSource>().Play();
                            transform.DOScale(1.4f, 0.1f).OnComplete(() =>
                            {
                                transform.DOScale(1, 0.2f);
                            });
                            transform.DORotate(this.rotation, MapManager.blockMoveTime).OnComplete(() =>
                            {
                                MapManager.Instance.canMove[this.position] = true;
                            });
                        }
                        else
                        {
                            MapManager.Instance.GetComponent<AudioSource>().Play();
                            transform.DOScale(1.4f, 0.1f).OnComplete(() =>
                            {
                                transform.DOScale(1, 0.2f).OnComplete(() =>
                                {

                                    MapManager.Instance.canMove[this.position] = true;
                                });
                            });
                        }
                    });
                    // ȸ�� ��
                    break;
                case BlockType.Multiple:
                    if (power)
                    {
                        MapManager.Instance.canMove[this.position] = false;

                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            MapManager.Instance.blockData[dest].gameObject.SetActive(false);

                            MapManager.Instance.MakeBlock(dest, this.Rotation, this.type);

                            MapManager.Instance.GetComponent<AudioSource>().Play();
                            MapManager.Instance.blockData[dest].transform.DOScale(1.4f, 0.1f).OnComplete(() =>
                            {
                                MapManager.Instance.blockData[dest].transform.DOScale(1, 0.2f).OnComplete(() =>
                                {

                                    MapManager.Instance.canMove[this.position] = true;
                                });
                            });

                        });
                    }
                    else
                    {
                        MapManager.Instance.canMove[this.position] = false;
                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            MapManager.Instance.canMove[this.position] = true;
                        });
                    }
                    // ���� ��
                    break;
                case BlockType.Jump:

                    Vector2Int jumpBlock = dest;
                    dest = dest + new Vector2Int(MapManager.Instance.blockData[jumpBlock].Rotation.x, -MapManager.Instance.blockData[jumpBlock].Rotation.y) * 2;

                    if (!MapManager.Instance.blockData.ContainsKey(dest) && !(dest.x < 0 || dest.y < 0 || dest.x >= MapManager.Instance.mapSize.x || dest.y >= MapManager.Instance.mapSize.y))
                    {
                        MapManager.Instance.canMove[dest] = false;

                        RemoveObject = MapManager.Instance.blockData[jumpBlock].gameObject;
                        MapManager.Instance.blockData.Remove(jumpBlock);

                        MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
                        MapManager.Instance.blockData.Remove(this.position);

                        arrowMove = true;

                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[jumpBlock].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            RemoveObject.SetActive(false);
                            MapManager.Instance.GetComponent<AudioSource>().Play();
                            transform.DOScale(1.4f, 0.1f).OnComplete(() =>
                            {
                                transform.DOScale(1, 0.2f);
                            });

                            if (transform.childCount != 0)
                            {
                                if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                                {
                                    transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                                }
                            }
                            transform.DOMove(MapManager.Instance.blockTransform[dest].position, MapManager.blockMoveTime).OnComplete(() =>
                            {

                                arrowMove = false;

                                this.position = dest;

                                MapManager.Instance.canMove[this.position] = true;
                            });
                        });
                    }
                    else
                    {
                        MapManager.Instance.canMove[this.position] = false;
                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            MapManager.Instance.canMove[this.position] = true;
                        });
                    }
                    // ���� ��
                    break;
                case BlockType.Arrow:
                    if (!(MapManager.Instance.blockData[dest] as Arrow).power && (MapManager.Instance.blockData[dest] as Arrow).arrowMove)
                    {
                        MapManager.Instance.canMove[dest] = false;

                        arrowMove = true;
                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[dest].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            arrowMove = false;
                            MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
                            MapManager.Instance.blockData.Remove(this.position);

                            this.position = dest;
                            MapManager.Instance.canMove[this.position] = true;
                        });
                    }
                    else
                    {

                        MapManager.Instance.canMove[this.position] = false;
                        if (transform.childCount != 0)
                        {
                            if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                            {
                                transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                            }
                        }
                        transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                        {
                            MapManager.Instance.canMove[this.position] = true;
                        });
                    }
                    // ȭ�� ��
                    break;
                default:
                    MapManager.Instance.canMove[this.position] = false;
                    if (transform.childCount != 0)
                    {
                        if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                        {
                            transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                        }
                    }
                    transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                    {
                        MapManager.Instance.canMove[this.position] = true;
                    });

                    break;
            }
        }
        else
        {
            if (!MapManager.Instance.blockData.ContainsKey(dest) && !(dest.x < 0 || dest.y < 0 || dest.x >= MapManager.Instance.mapSize.x || dest.y >= MapManager.Instance.mapSize.y))
            {
                MapManager.Instance.canMove[dest] = false;
                if (transform.childCount != 0)
                {
                    if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                    {
                        transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                    }
                }
                transform.DOMove(MapManager.Instance.blockTransform[dest].position, MapManager.blockMoveTime).OnComplete(() =>
                {
                    try
                    {
                        MapManager.Instance.blockData[dest] = MapManager.Instance.blockData[this.position];
                    }
                    catch (System.Exception e)
                    {

                    }
                    MapManager.Instance.blockData.Remove(this.position);

                    this.position = dest;
                    MapManager.Instance.canMove[this.position] = true;
                });
            }
            else
            {
                MapManager.Instance.canMove[this.position] = false;
                if (transform.childCount != 0)
                {
                    if (transform.GetChild(0).GetComponent<TrailRenderer>() != null)
                    {
                        transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
                    }
                }
                transform.DOMove(MapManager.Instance.blockTransform[this.position].position, MapManager.blockMoveTime).OnComplete(() =>
                {
                    MapManager.Instance.canMove[this.position] = true;
                });
            }
        }
    }
}
