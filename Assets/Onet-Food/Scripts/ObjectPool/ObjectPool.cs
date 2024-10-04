using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [Header("Element")]
    public Transform lineParent;
    public Transform starParent;
    public GameObject linePrefab;
    public GameObject starPrefab;
    public GameObject textPrefab;

    [Header("Data")]
    public int amountToPool = 20;
    private List<GameObject> linePool;
    private List<GameObject> starPool;
    private List<GameObject> textPool;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        linePool = new List<GameObject>();
        ExtendPool(linePool, linePrefab, lineParent, amountToPool);

        starPool = new List<GameObject>();
        ExtendPool(starPool, starPrefab, starParent, amountToPool);

        textPool = new List<GameObject>();
        ExtendPool(textPool, textPrefab, starParent, amountToPool);
    }

    public void ExtendPool(List<GameObject> pool, GameObject prefab, Transform parent, int amount)
    {
        GameObject go;
        for (int i = 0; i < amount; i++)
        {
            go = Instantiate(prefab, parent);
            go.SetActive(false);

            pool.Add(go);
        }
    }

    public GameObject GetObject(List<GameObject> pool, GameObject prefab, Transform parent)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        int index = pool.Count;
        ExtendPool(pool, prefab, parent, 5);

        return pool[index];
    }

    public GameObject GetLineObject()
    {
        return GetObject(linePool, linePrefab, lineParent);
    }

    public GameObject GetStarObject()
    {
        return GetObject(starPool, starPrefab, starParent);
    }

    public GameObject GetTextObject()
    {
        return GetObject(textPool, textPrefab, starParent);
    }
}
