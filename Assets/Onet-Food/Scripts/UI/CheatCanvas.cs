using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatCanvas : MonoBehaviour
{
    public TMP_Dropdown levelDropdown;

    public LevelManagerProfileSO levelManagerProfileSO;

    private void Start()
    {
        levelDropdown.options.Clear();

        for (int i = 0; i <= levelManagerProfileSO.MaxLevelIndex; i++)
        {
            levelDropdown.options.Add(new TMP_Dropdown.OptionData($"Level {i + 1}"));
        }

        levelDropdown.value = DataManager.Instance.Data.levelData.LevelIndex;

        levelDropdown.onValueChanged.RemoveAllListeners();
        levelDropdown.onValueChanged.AddListener(OnValueChanged_Level);
    }

    public void OnValueChanged_Level(int index)
    {
        DataManager.Instance.Data.levelData.LevelIndex = index;
    }

    public void OnClick_Add100Star()
    {
        DataManager.Instance.AddItem(ItemsData.ItemType.Star, 100);
    }

    public void OnClick_Add100Coin()
    {
        DataManager.Instance.AddItem(ItemsData.ItemType.Coin, 100);
    }
}
