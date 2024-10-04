using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackIAPView : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] protected RewardContainerView rewardContainerView;

    private void OnEnable()
    {
        Setup();
    }

    public virtual void Setup()
    {
        rewardContainerView.Setup();
    }
}
