using System;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public string Id { get; private set; }

    public void SetId(string id)
    {
        Id = id;
    }
}
