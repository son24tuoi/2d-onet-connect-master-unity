using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestProfileSO", menuName = "Scriptable Object/Chest Profile")]
public class ChestProfileSO : ScriptableObject
{
    public int[] numberOfRequests;

    public RewardProfileSO[] rewards;

    public bool enableShowRewardCard = true;

    public int GetRequire(int chestIndex)
    {
        if (numberOfRequests.Length == 0)
        {
            return 0;
        }

        chestIndex = Mathf.Clamp(chestIndex, 0, numberOfRequests.Length - 1);
        return numberOfRequests[chestIndex];
    }

    public Reward[] GetRewards(int chestIndex)
    {
        if (rewards.Length == 0)
        {
            return new Reward[0];
        }

        chestIndex = Mathf.Clamp(chestIndex, 0, rewards.Length - 1);
        return rewards[chestIndex].rewards;
    }

    public RewardProfileSO GetRewardProfile(int chestIndex)
    {
        if (rewards.Length == 0)
        {
            return null;
        }

        chestIndex = Mathf.Clamp(chestIndex, 0, rewards.Length - 1);
        return rewards[chestIndex];
    }
}
