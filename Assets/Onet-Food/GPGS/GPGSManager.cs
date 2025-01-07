using System;
using System.Collections;

using UnityEngine;
using TMPro;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Newtonsoft.Json;

public class GPGSManager : MonoBehaviour
{
    [SerializeField] private GameObject testCanvas;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private LoadCloudCanvas loadCloudCanvasPrefab;
    [SerializeField] private bool debugLog;

    private string authCode; // mã xác thực tài khoản người chơi
    private string token;
    private string error;
    private string message;
    private bool isSaving;
    public bool showNotify;

    private Action onCompleteLoginEvent;

    private const string SaveGameFileName = "SaveGame";

    private IDataController dataController;

    public static GPGSManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        dataController = DataManager.Instance.Controller;

        // Khởi tạo PlayGamesPlatform
        PlayGamesPlatform.DebugLogEnabled = debugLog;
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    #region Login

    public void ManuallyLoginGooglePlayGames()
    {
        EventManager.Instance.Trigger(new EventData<bool>(EventID.Wait, true));

        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        EventManager.Instance.Trigger(new EventData<bool>(EventID.Wait, false));

        if (status == SignInStatus.Success)
        {
            Log("Login with Google Play games successful.");

            string text = "Authentication user name: " + Social.localUser.userName + ", id: " + Social.localUser.id;
            Log(text);

            PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>
            {
                Log("Authorization code: " + code);
                token = code;
                authCode = code;
                // This token serves as an example to be used for SignInWithGooglePlayGames
            });

            if (onCompleteLoginEvent != null)
            {
                EventManager.Instance.Trigger(new EventData<bool>(EventID.Wait, true));
                onCompleteLoginEvent?.Invoke();
                onCompleteLoginEvent = null;
            }
        }
        else
        {
            error = "Failed to retrieve Google play games authorization code";
            Log("Login Unsuccessful");

            TriggerEventNotification(new NotificationData(
                    "Failed to connect to cloud",
                    NotificationColorType.Red
                ));
        }
    }

    #endregion

    #region Save Game

    // Hiển thị màn hình lựa chọn bản lưu game của google
    public void ShowSaveGameManagerUI()
    {
        uint maxNumToDisplay = 5;
        bool allowCreateNew = false;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Save game manager",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnSavedGameSelected);

        EventManager.Instance.Trigger(new EventData<bool>(EventID.Wait, true));
    }

    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        EventManager.Instance.Trigger(new EventData<bool>(EventID.Wait, false));

        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
        }
        else
        {
            // handle cancel or error
        }
    }

    /// <summary>
    /// Lưu hoặc tải dữ liệu
    /// </summary>
    /// <param name="saving">true is save game, false is load game</param>
    public void OpenSave(bool saving)
    {
        if (Social.localUser.authenticated)
        {
            isSaving = saving;
            OpenSavedGame(SaveGameFileName);
        }
        else
        {

        }
    }

    private void OpenSavedGame(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (isSaving) // lưu game
            {
                // chuyển đổi data sang byte array
                byte[] myData;
                myData = System.Text.ASCIIEncoding.ASCII.GetBytes(dataController.GetData());

                // lưu game
                SaveGame(game, myData);
            }
            else // đọc data
            {
                LoadGameData(game);
            }
        }
        else
        {
            // handle error
            TriggerEventNotification(new NotificationData(
                    "Failed to connect to cloud",
                    NotificationColorType.Red
                ));
        }
    }

    private void SaveGame(ISavedGameMetadata game, byte[] savedData)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedDescription("Saved game at " + DateTime.Now.ToString());
        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    private void LoadGameData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Log("Succesfully saved to the cloud");

            TriggerEventNotification(new NotificationData(
                    "Succesfully saved to the cloud",
                    NotificationColorType.Green
                ));
        }
        else
        {
            Log("Failed to save to the cloud");

            TriggerEventNotification(new NotificationData(
                    "Failed to save to the cloud",
                    NotificationColorType.Red
                ));
        }
    }

    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Log("Successfully load from the cloud, attempting to read data ...");
            EventManager.Instance.Trigger(new EventData<bool>(EventID.Wait, false));

            string savedData = System.Text.ASCIIEncoding.ASCII.GetString(data);
            if (string.IsNullOrEmpty(savedData))
            {
                TriggerEventNotification(new NotificationData(
                    "No data load from the cloud",
                    NotificationColorType.Yellow
                ));
            }
            else
            {
                dataController.LoadData(savedData);
                Instantiate(loadCloudCanvasPrefab, transform);
            }
        }
        else
        {
            // handle error
            Log("Failed to load from the cloud");

            TriggerEventNotification(new NotificationData(
                    "Failed to load from the cloud",
                    NotificationColorType.Red
                ));
        }
    }

    public Texture2D GetScreenshot()
    {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be
        // extracted
        Texture2D screenShot = new Texture2D(1024, 700);

        // Takes the screenshot from top left hand corner of screen and maps to top
        // left hand corner of screenShot texture
        screenShot.ReadPixels(
            new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);
        return screenShot;
    }

    public void DeleteGameData()
    {
        DeleteGameData(SaveGameFileName);
    }

    private void DeleteGameData(string filename)
    {
        // Open the file to get the metadata.
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, DeleteSavedGame);
    }

    private void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.Delete(game);
            Log("Successfully delete save game");
        }
        else
        {
            // handle error
            Log("Failed to delete save game");
        }
    }

    public void TryOpenSave(bool isSaving)
    {
        showNotify = true;

        if (Social.localUser.authenticated)
        {
            EventManager.Instance.Trigger(new EventData<bool>(EventID.Wait, true));
            OpenSave(isSaving);
        }
        else
        {
            onCompleteLoginEvent = () => OpenSave(isSaving);
            ManuallyLoginGooglePlayGames();
        }
    }

    #endregion

    public void ShowTestPopup()
    {
        testCanvas.SetActive(true);
    }

    private void Log(string message)
    {
        if (debugLog)
        {
            this.message += message + "\n";
            statusText.SetText(this.message);
            Debug.Log(message);
        }
    }

    private void TriggerEventNotification(NotificationData notificationData)
    {
        if (!showNotify)
            return;

        EventManager.Instance.Trigger(new EventData<NotificationData>(
            eventID: EventID.Notification,
            notificationData
        ));
    }
}
