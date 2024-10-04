using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockButton : MyMonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Image[] m_starImages;
    [SerializeField] private TextMeshProUGUI m_levelTMP;
    [SerializeField] private GameObject[] bgImages;

    [Header("Config")]
    public Color normalStarColor = Color.white;
    public Color grayStarColor = Color.gray;

    public enum BackgroundType
    {
        Normal = 0,
        Newest = 1
    }

    public void Init(int level, int star)
    {
        m_levelTMP.SetText((level + 1).ToString());

        bool newest = star < 0;

        bgImages[(int)BackgroundType.Normal].SetActive(!newest);
        bgImages[(int)BackgroundType.Newest].SetActive(newest);

        if (newest)
        {

        }
        else
        {
            SetupStar(star);
        }
    }

    public void SetupStar(int star)
    {
        for (int i = 0; i < 3; i++)
        {
            m_starImages[i].color = (i < star) ? normalStarColor : grayStarColor;
        }
    }
}