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

    }

    public virtual void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {

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