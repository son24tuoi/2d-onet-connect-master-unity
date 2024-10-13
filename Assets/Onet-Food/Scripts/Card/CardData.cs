using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public List<int> idCards = new List<int>();

    [Header("Setting")]
    [SerializeField] private bool shuffleCard;

    public void Init(List<int> idCards)
    {
        this.idCards = idCards;

        if (shuffleCard)
        {
            ListShuffler.Shuffle(idCards);
        }
    }
}
