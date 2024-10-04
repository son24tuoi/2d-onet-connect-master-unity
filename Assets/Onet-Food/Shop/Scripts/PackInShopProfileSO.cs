using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PackInShopProfileSO", menuName = "Scriptable Object/Pack In Shop Profile")]
public class PackInShopProfileSO : ScriptableObject
{
    [SerializeField] private RewardProfileSO rewardProfileSO;
    [SerializeField] private int price;

    public RewardProfileSO RewardProfileSO
    {
        get => rewardProfileSO;
    }

    public int Price
    {
        get => price;
    }

}
