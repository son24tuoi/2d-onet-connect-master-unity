using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] protected SpriteRenderer bgSR;
    [SerializeField] protected SpriteRenderer modelSR;
    [SerializeField] protected GameObject highlight;
    [SerializeField] protected TweenScale tweenScale;

    public int id;

    [Header("Config")]
    [SerializeField] protected CardsProfileSO cardsProfileSO;
    [SerializeField] protected Ease showCardEase;
    [SerializeField] protected Ease moveCardEase;

    public virtual void SetModel(int idCard, bool effect = true)
    {
        id = idCard;
        modelSR.sprite = cardsProfileSO.GetSprite(idCard);

        if (effect)
        {
            ShowCardEffect();
        }
    }

    public virtual void Select()
    {
        bgSR.sprite = cardsProfileSO.selectBG;
    }

    public virtual void Deselect()
    {
        bgSR.sprite = cardsProfileSO.defaultBG;
    }

    public virtual void ScaleDown()
    {
        transform.localScale = Vector3.one * 0.9f;
    }

    public virtual void ScaleDefault()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void EnableHighlight(bool state)
    {
        if (highlight != null)
        {
            highlight.SetActive(state);
        }
    }

    public void ShowCardEffect()
    {
        tweenScale.Scale(Vector3.zero, Vector3.one, Random.Range(0.2f, 0.4f), showCardEase, null);
    }

    public void MoveCard(Vector3 start)
    {
        Tween.Position(transform, start, transform.position, 0.3f, moveCardEase);
    }
}
