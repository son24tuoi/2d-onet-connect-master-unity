using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Background
{
    public class BackgroundScroller : MonoBehaviour
    {
        [Header("Setting")]
        [Range(-30f, 30f)] public float scrollSpeed = 0.5f;

        private void FixedUpdate()
        {
            transform.Translate(Vector3.left * Time.deltaTime * scrollSpeed);
        }
    }
}
