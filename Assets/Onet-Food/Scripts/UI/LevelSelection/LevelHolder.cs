using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHolder : MyMonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    public LevelData LevelData => DataManager.Data.levelData;

    public int LevelMax { get; set; }

    private List<LevelButton> _levelButtonList;

    public void SetupGrid(Vector2 cellSize, RectOffset padding, Vector2 spacing)
    {
        gridLayoutGroup.cellSize = cellSize;
        gridLayoutGroup.padding = padding;
        gridLayoutGroup.spacing = spacing;
    }

    public void CreateLevels(int numberOfLevels, LevelButton levelPrefab)
    {
        _levelButtonList = new List<LevelButton>();

        for (int i = 1; i <= numberOfLevels; i++)
        {
            LevelButton levelInstance = Instantiate(levelPrefab, transform, false);
            levelInstance.name = "Level_" + i;
            _levelButtonList.Add(levelInstance);
        }
    }

    public void LoadLevels(int firstIndex)
    {
        if (_levelButtonList != null)
        {
            for (int i = 0; i < _levelButtonList.Count; i++)
            {
                if (i + firstIndex < LevelMax)
                {
                    _levelButtonList[i].gameObject.SetActive(true);
                    _levelButtonList[i].Setup(i + firstIndex, LevelData.GetStarWin(i + firstIndex));
                }
                else
                {
                    _levelButtonList[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetLocalPosition(Vector2 localPos)
    {
        rectTransform.localPosition = localPos;
    }
}