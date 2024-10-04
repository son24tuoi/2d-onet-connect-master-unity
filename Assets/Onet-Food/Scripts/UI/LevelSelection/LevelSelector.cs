using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MyMonoBehaviour
{
    [Header("Elements")]
    public GameObject levelHolder;
    public LevelHolder[] levelHolders;
    public LevelButton levelPrefab;
    public PageSwiper pageSwiper;
    public LevelManagerProfileSO levelManagerProfileSO;
    public RectOffset paddingGrid;
    public Vector2 spacingGrid = new Vector2(50, 50);

    private Rect _panelDimensions;
    private Rect _iconDimensions;
    private int _amountPerPage;
    private int m_currentLevelCount;

    private int _numberOfLevels;

    public LevelData LevelData => DataManager.Data.levelData;

    private void OnEnable()
    {
        pageSwiper.LoadPages(GetPageIndex(LevelData.LevelIndex));
        pageSwiper.MovePage(GetPageIndex(LevelData.LevelIndex));
        // pageSwiper.LoadPages(24);
        // pageSwiper.MovePage(24);
    }

    private void Awake()
    {
        _panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        _iconDimensions = levelPrefab.GetComponent<RectTransform>().rect;
        int maxInARow = Mathf.FloorToInt((_panelDimensions.width - paddingGrid.left - paddingGrid.right) / (_iconDimensions.width + spacingGrid.x));
        int maxInACol = Mathf.FloorToInt((_panelDimensions.height - paddingGrid.top - paddingGrid.bottom) / (_iconDimensions.height + spacingGrid.y));
        _amountPerPage = maxInARow * maxInACol;
        _numberOfLevels = levelManagerProfileSO.levelProfiles.Length;
        int totalPages = Mathf.CeilToInt((float)_numberOfLevels / _amountPerPage);

        pageSwiper.Init(totalPages, _amountPerPage, levelHolders, _panelDimensions);
        LoadPanels();
    }

    private void LoadPanels()
    {
        for (int i = 0; i < levelHolders.Length; i++)
        {
            levelHolders[i].LevelMax = _numberOfLevels;
            levelHolders[i].SetupGrid(
                cellSize: new Vector2(_iconDimensions.width, _iconDimensions.height),
                padding: paddingGrid,
                spacing: spacingGrid
            );
            levelHolders[i].CreateLevels(_amountPerPage, levelPrefab);
            // levelHolders[i].SetLocalPosition(new Vector2(_panelDimensions.width * i, 0));
        }
    }

    private int GetPageIndex(int levelIndex)
    {
        return levelIndex / _amountPerPage;
    }
}
