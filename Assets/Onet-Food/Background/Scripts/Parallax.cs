using UnityEngine;

namespace Background
{
    public class Parallax : MonoBehaviour
    {
        public float speed;

        private float m_length;
        private float m_startPos;
        private float m_currentPos;
        private Vector2 m_region;

        private void Awake()
        {
            m_startPos = transform.position.x;
            m_length = GetComponent<SpriteRenderer>().bounds.size.x;
            m_region = new Vector2(m_startPos - m_length, m_startPos + m_length);
        }

        public void UpdatePosition()
        {
            if (speed == 0f)
                return;

            transform.Translate(speed * Time.deltaTime * Vector3.left);

            m_currentPos = transform.position.x;

            if (m_currentPos > m_region.y)
            {
                transform.position += Vector3.left * m_length;
            }
            else if (m_currentPos < m_region.x)
            {
                transform.position += Vector3.right * m_length;
            }
        }
    }
}
