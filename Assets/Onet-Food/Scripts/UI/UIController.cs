using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIController : MonoBehaviour, IEventHandler, IEventHandlerWithData
{
    public Transform parentCanvas;

    [Header("Element")]
    public HomeCanvas homeCanvas;
    public GamePlayCanvas gamePlayCanvas;
    public PauseCanvas pauseCanvas;
    public WinCanvas winCanvas;
    public LoseCanvas loseCanvas;
    public LevelSelectionCanvas levelSelectionCanvas;
    public PlayLevelCanvas playLevelCanvas;
    public ShopCanvas shopCanvas;

    [Header("Prefab")]
    public TutorialCanvas tutorialCanvasPrefab;
    public SettingCanvas settingCanvasPrefab;
    public ThankYouForPlayingCanvas thankYouForPlayingCanvasPrefab;
    public SupportItemShopCanvas supportItemShopCanvasPrefab;

    private TutorialCanvas tutorialCanvas;
    private SettingCanvas settingCanvas;
    private ThankYouForPlayingCanvas thankYouForPlayingCanvas;
    private SupportItemShopCanvas supportItemShopCanvas;

    public enum MainPanelType
    {
        None,
        Home,
        GamePlay,
    }

    private void Start()
    {
        EventManager.Instance.Subcribe(EventID.Setting, this as IEventHandler);
        EventManager.Instance.Subcribe(EventID.ShopCanvas, this as IEventHandler);
        EventManager.Instance.Subcribe(EventID.SupportItemShop, this as IEventHandlerWithData);

        TutorialButton.OnTutorialEvent += ShowTutorialCanvas;
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubcribe(EventID.Setting, this as IEventHandler);
        EventManager.Instance.Unsubcribe(EventID.ShopCanvas, this as IEventHandler);
        EventManager.Instance.Unsubcribe(EventID.SupportItemShop, this as IEventHandlerWithData);

        TutorialButton.OnTutorialEvent -= ShowTutorialCanvas;
    }

    public void ShowMainPanel(MainPanelType type)
    {
        homeCanvas.gameObject.SetActive(type == MainPanelType.Home);
        gamePlayCanvas.gameObject.SetActive(type == MainPanelType.GamePlay);
    }

    public void ShowPauseCanvas()
    {
        pauseCanvas.gameObject.SetActive(true);
    }

    public void ShowWinCanvas()
    {
        winCanvas.gameObject.SetActive(true);
    }

    public void ShowLoseCanvas()
    {
        loseCanvas.gameObject.SetActive(true);
    }

    public void ShowLevelSelectionCanvas(bool show = true)
    {
        ShowMainPanel(MainPanelType.None);
        levelSelectionCanvas.gameObject.SetActive(show);
    }

    public void ShowPlayLevelCanvas(int levelIndex)
    {
        playLevelCanvas.Init(levelIndex);
        playLevelCanvas.gameObject.SetActive(true);
    }

    public void ShowSettingCanvas(bool show = true)
    {
        if (show)
        {
            if (settingCanvas == null)
            {
                settingCanvas = Instantiate(settingCanvasPrefab, transform);
            }
            else
            {
                settingCanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            if (settingCanvas != null)
            {
                settingCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void ShowShopCanvas(bool show = true)
    {
        shopCanvas.gameObject.SetActive(show);
    }

    public void ShowTutorialCanvas()
    {
        if (tutorialCanvas == null)
        {
            tutorialCanvas = Instantiate(tutorialCanvasPrefab, parentCanvas);
        }
        else
        {
            tutorialCanvas.gameObject.SetActive(true);
        }
    }

    public void ShowThankYouForPlayingCanvas()
    {
        if (thankYouForPlayingCanvas == null)
        {
            thankYouForPlayingCanvas = Instantiate(thankYouForPlayingCanvasPrefab, parentCanvas);
        }
        else
        {
            thankYouForPlayingCanvas.gameObject.SetActive(true);
        }
    }

    public void ShowSupportItemShopCanvas(ItemsData.ItemType itemType)
    {
        if (supportItemShopCanvas == null)
        {
            supportItemShopCanvas = Instantiate(supportItemShopCanvasPrefab, parentCanvas);
        }
        else
        {
            supportItemShopCanvas.gameObject.SetActive(true);
        }

        supportItemShopCanvas.Init(itemType);
    }

    public void EventHandler(EventID eventID)
    {
        switch (eventID)
        {
            case EventID.Setting:
                ShowSettingCanvas();
                break;
            
            case EventID.ShopCanvas:
                ShowShopCanvas();
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }

    public void EventHandler<T>(EventData<T> eventData)
    {
        switch (eventData.eventID)
        {
            case EventID.SupportItemShop:
                if (eventData.data is ItemsData.ItemType itemType)
                {
                    ShowSupportItemShopCanvas(itemType);
                }
                break;
            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }






#if UNITY_EDITOR
    [CustomEditor(typeof(UIController))]
    public class UIController_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UIController target = (UIController)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button("Show Tutorial Canvas"))
            {
                target.ShowTutorialCanvas();
            }

            if (GUILayout.Button(nameof(target.ShowThankYouForPlayingCanvas)))
            {
                target.ShowThankYouForPlayingCanvas();
            }
        }
    }
#endif
}
