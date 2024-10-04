using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsProfileSO", menuName = "Scriptable Object/Items Profile")]
public class ItemsProfileSO : ScriptableObject
{
    public Sprite[] itemsIcons;

    public Sprite GetIcon(ItemsData.ItemType itemType)
    {
        int index = Mathf.Clamp((int)itemType, 0, itemsIcons.Length - 1);

        return itemsIcons[index];
    }
}
