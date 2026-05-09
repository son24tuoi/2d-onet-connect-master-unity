using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseIAP : MonoBehaviour
{
    private Button button;
    private CodelessIAPButton iapButton;

    public Button Button
    {
        get
        {
            if (button == null)
                button = GetComponent<Button>();
            return button;
        }
    }

    public CodelessIAPButton IAPButton
    {
        get
        {
            if (iapButton == null)
                iapButton = GetComponent<CodelessIAPButton>();
            return iapButton;
        }
    }

    private void Awake()
    {
        IAPButton.enabled = false;

        Button.onClick.AddListener(() =>
            IAPManager.Instance.ShowFakeStorePopup(TestBuySuccess, TestBuyFailed));
    }

    private void TestBuySuccess()
    {
        OnPurchaseCompleted(null);
    }

    private void TestBuyFailed()
    {
        OnPurchaseFailed(null, null);
    }

    public virtual void OnPurchaseCompleted(Product purchasedProduct)
    {
        EventManager.Instance.Trigger(new EventData<NotificationData>(
            EventID.Notification,
            new NotificationData(
                "Purchase Completed",
                NotificationColorType.Green
            )
        ));
    }

    public virtual void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        EventManager.Instance.Trigger(new EventData<NotificationData>(
            EventID.Notification,
            new NotificationData(
                "Purchase Failed",
                NotificationColorType.Red
            )
        ));
    }












#if UNITY_EDITOR
    [CustomEditor(typeof(BaseIAP), true)]
    public class BaseIAP_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BaseIAP target = (BaseIAP)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);
        }
    }
#endif
}