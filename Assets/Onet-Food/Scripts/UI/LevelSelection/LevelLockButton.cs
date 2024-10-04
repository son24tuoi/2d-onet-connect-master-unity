using UnityEngine;
using TMPro;

public class LevelLockButton : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI m_levelTMP;

    public void Init(int level)
    {
        m_levelTMP.SetText((level + 1).ToString());
    }
}