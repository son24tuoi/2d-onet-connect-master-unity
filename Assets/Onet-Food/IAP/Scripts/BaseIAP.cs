using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseIAP : MonoBehaviour
{

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