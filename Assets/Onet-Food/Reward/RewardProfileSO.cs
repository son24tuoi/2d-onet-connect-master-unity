using UnityEngine;

[CreateAssetMenu(fileName = "RewardProfileSO", menuName = "Scriptable Object/Reward Profile")]
public class RewardProfileSO : ScriptableObject
{
    public Reward[] rewards;

    public int Amount
    {
        get => rewards.Length;
    }

    public Reward GetReward(int index)
    {
        if (rewards == null || rewards.Length == 0)
            return null;

        index = Mathf.Clamp(index, 0, rewards.Length);
        return rewards[index];
    }

    public Reward[] GetRewards(bool shuffle = true)
    {
        if (!shuffle)
        {
            return rewards;
        }

        return GetShuffleRewards();
    }

    public Reward[] GetShuffleRewards()
    {
        Reward[] shuffleRewards = rewards;
        int randomIndex;
        Reward value;

        for (int i = 0; i < shuffleRewards.Length; i++)
        {
            randomIndex = Random.Range(i, shuffleRewards.Length);
            value = shuffleRewards[randomIndex];
            shuffleRewards[randomIndex] = shuffleRewards[i];
            shuffleRewards[i] = value;
        }

        return shuffleRewards;
    }
}