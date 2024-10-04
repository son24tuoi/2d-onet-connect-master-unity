using System;

using ItemType = ItemsData.ItemType;

[Serializable]
public class ItemStack
{
    public ItemType itemType;
    public int amount;

    public ItemStack(ItemType itemType = ItemType.Coin, int amount = 0)
    {
        this.itemType = itemType;
        this.amount = amount;
    }
}