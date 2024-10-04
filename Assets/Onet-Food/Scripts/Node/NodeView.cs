using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject tile;
    public GameObject arrow;
    public GameObject arrow2;
    public GameObject card;
    public GameObject path;

    public bool showTile;

    private Node m_node;

    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    public Node Node => m_node;

    public void Init(Node node)
    {
        if (tile != null)
        {
            m_node = node;

            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")";
            transform.position = node.position;

            if (showTile)
            {
                tile.transform.localScale = new Vector3(1f - borderSize, 1f - borderSize, 1f);
            }

            EnableObject(tile, showTile);

            EnableObject(arrow, false);
            EnableObject(arrow2, false);
            EnableObject(card, false);
            EnableObject(path, false);
        }
    }

    private void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            if (go.TryGetComponent<Renderer>(out Renderer goRenderer))
            {
                goRenderer.material.color = color;
            }
        }
    }

    public void ColorNode(Color color)
    {
        if (showTile)
        {
            ColorNode(color, tile);
        }
    }

    private void EnableObject(GameObject go, bool state)
    {
        if (go != null)
        {
            go.SetActive(state);
        }
    }

    public void ShowArrow(Color color)
    {
        if (m_node != null && arrow != null && m_node.previous != null)
        {
            EnableObject(arrow, true);

            Vector3 dirToPrevious = (m_node.previous.position - m_node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, dirToPrevious);

            if (arrow.TryGetComponent<Renderer>(out Renderer arrowRenderer))
            {
                arrowRenderer.material.color = color;
            }
        }
    }

    public void ShowArrow2(Color color)
    {
        if (m_node != null && arrow2 != null && m_node.secondPrevious != null)
        {
            EnableObject(arrow2, true);

            Vector3 dirToPrevious = (m_node.secondPrevious.position - m_node.position).normalized;
            arrow2.transform.rotation = Quaternion.LookRotation(Vector3.forward, dirToPrevious);

            if (arrow2.TryGetComponent<Renderer>(out Renderer arrowRenderer))
            {
                arrowRenderer.material.color = color;
            }
        }
    }

    public void ShowPath(Color color)
    {
        if (m_node != null && path != null && m_node.previous != null)
        {
            EnableObject(path, true);

            Vector3 dirToPrevious = (m_node.previous.position - m_node.position).normalized;
            path.transform.rotation = Quaternion.LookRotation(Vector3.forward, dirToPrevious);

            if (path.TryGetComponent<Renderer>(out Renderer pathRenderer))
            {
                pathRenderer.material.color = color;
            }
        }
    }

    public void ShowCard(int idCard, bool effect = true)
    {
        if (card != null)
        {
            EnableObject(card, true);

            if (card.TryGetComponent<Card>(out Card c))
            {
                c.SetModel(idCard, effect);
                c.Deselect();
                c.EnableHighlight(false);
            }
        }
    }

    public void MoveCard(Vector3 start)
    {
        if (card != null)
        {
            if (card.TryGetComponent<Card>(out Card c))
            {
                c.MoveCard(start);
            }
        }
    }

    public void Reset()
    {
        EnableObject(arrow, false);
        EnableObject(arrow2, false);
        EnableObject(path, false);
    }

    public void ClearCard()
    {
        EnableObject(card, false);
    }

    public int GetIDCard()
    {
        if (card != null && card.activeSelf)
        {
            if (card.TryGetComponent<Card>(out Card c))
            {
                return c.id;
            }
        }

        Debug.LogWarning("NODEVIEW GetCard missing card");

        return -1;
    }

    public void ShowHighlightCard(bool state)
    {
        if (card != null && card.activeSelf)
        {
            if (card.TryGetComponent<Card>(out Card c))
            {
                c.EnableHighlight(state);
            }
        }
    }

    public void ChangeCard(int idCard)
    {
        if (card != null)
        {
            if (card.TryGetComponent<Card>(out Card c))
            {
                c.SetModel(idCard);
                c.EnableHighlight(false);
            }
        }
    }
}
