using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private PlayerSelection playerSelection;
    public GameProfileSO gameProfileSO;

    [Header("Setting")]
    [SerializeField] private LayerMask layerMask;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit2D hit;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!gameProfileSO.enablePlayerController)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GetHitCollider())
            {
                playerSelection.OnPointerDown(hit.collider.gameObject);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (GetHitCollider())
            {
                playerSelection.OnPointerUp(hit.collider.gameObject);
            }
            playerSelection.ReleasePointerDownBlock();
        }
    }

    private bool GetHitCollider()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

        return (hit.collider != null);
    }
}
