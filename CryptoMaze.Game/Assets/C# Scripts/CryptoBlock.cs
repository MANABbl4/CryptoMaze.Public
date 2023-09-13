using CryptoMaze.Common;
using System;
using UnityEngine;

public class CryptoBlock : MonoBehaviour
{
    [SerializeField]
    private CryptoType _blockType;

    public CryptoType BlockType { get { return _blockType; } }
    public string Id { get; private set; }

    public void SetId(string id)
    {
        Id = id;
    }
}
