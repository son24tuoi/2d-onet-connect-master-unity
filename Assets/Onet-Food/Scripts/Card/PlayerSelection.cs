using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MyMonoBehaviour
{
    public static event Action<GameObject, GameObject> OnPathFindingEvent;

    [Header("Data")]
    private GameObject firstObject;
    private GameObject secondObject;

    private GameObject pointerDownObject;

    public void OnPointerDown(GameObject go)
    {
        if (go != null)
        {
            pointerDownObject = go;

            if (go.TryGetComponent<Card>(out Card c))
            {
                c.ScaleDown();
            }
        }
    }

    public void OnPointerUp(GameObject go)
    {
        if (go != null)
        {
            if (go == pointerDownObject)
            {
                AddObject(go);
            }
        }
    }

    public void ReleasePointerDownBlock()
    {
        if (pointerDownObject == null)
            return;

        if (pointerDownObject.TryGetComponent<Card>(out Card c))
        {
            c.ScaleDefault();
        }
        pointerDownObject = null;
    }

    public void AddObject(GameObject go)
    {
        if (firstObject == null)
        {
            SelectFirstObject(go);
        }
        else if (firstObject == go)
        {
            ReleaseFirstObject();
        }
        else
        {
            Card secondCard = go.GetComponent<Card>();
            Card firstCard = firstObject.GetComponent<Card>();
            if (secondCard != null &&
                firstCard != null &&
                secondCard.id == firstCard.id)
            {
                SelectSecondObject(go);
                PathFinding();
            }
            else // Chọn lại block đầu tiên
            {
                ReleaseFirstObject();
                SelectFirstObject(go);
            }
        }

        AudioManager.PlaySFX(SFXType.Click);
    }

    public void PathFinding()
    {
        // Debug.Log("Path Finding");
        OnPathFindingEvent?.Invoke(firstObject, secondObject);

        ReleaseObjects();
    }

    public void SelectFirstObject(GameObject go)
    {
        firstObject = go;

        if (go.TryGetComponent<Card>(out Card c))
        {
            c.Select();
        }
    }

    public void SelectSecondObject(GameObject go)
    {
        secondObject = go;

        if (go.TryGetComponent<Card>(out Card c))
        {
            c.Select();
        }
    }

    public void ReleaseObjects()
    {
        ReleaseFirstObject();
        ReleaseSecondObject();
    }

    public void ReleaseFirstObject()
    {
        if (firstObject == null)
            return;

        if (firstObject.TryGetComponent<Card>(out Card c))
        {
            c.Deselect();
        }
        firstObject = null;
    }

    public void ReleaseSecondObject()
    {
        if (secondObject == null)
            return;

        if (secondObject.TryGetComponent<Card>(out Card c))
        {
            c.Deselect();
        }
        secondObject = null;
    }
}
