using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingItem : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemParent;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private int amount;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private void Start()
    {
        // Spawn item to a special location with random value
        for (int i = 0; i < amount; i++)
        {
            GameObject coinInstance = Instantiate(itemPrefab, spawnLocation.position, Quaternion.identity, itemParent);
        }
    }
}
