using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePublisher : MonoBehaviour
{
    #region Fields

    [SerializeField] private Color color = Color.blue;

    [SerializeField] private SampleEventData sampleEventData;

    #endregion Fields

    // -------------------------------------------------------------------------------------------------

    #region Properties



    #endregion Properties

    // -------------------------------------------------------------------------------------------------

    #region Unity Lifecycle Methods



    #endregion Unity Lifecycle Methods

    // -------------------------------------------------------------------------------------------------

    public void TriggerWithoutData()
    {
        EventManager.Instance.Trigger(
            EventID.Template
        );
    }

    public void TriggerWithoutData1()
    {
        // EventManager.Instance.Trigger(
        //     EventID.Template1
        // );
    }

    public void TriggerWithData()
    {
        EventManager.Instance.Trigger(new EventData<Color>(
            EventID.Template,
            color
        ));
    }

    public void TriggerWithSpecialData()
    {
        EventManager.Instance.Trigger(new EventData<SampleEventData>(
            EventID.Template,
            sampleEventData
        ));
    }

    // -------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(SamplePublisher))]
    public class SamplePublisher_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SamplePublisher target = (SamplePublisher)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.TriggerWithoutData)))
            {
                target.TriggerWithoutData();
            }
            
            if (GUILayout.Button(nameof(target.TriggerWithoutData1)))
            {
                target.TriggerWithoutData1();
            }

            if (GUILayout.Button(nameof(target.TriggerWithData)))
            {
                target.TriggerWithData();
            }

            if (GUILayout.Button(nameof(target.TriggerWithSpecialData)))
            {
                target.TriggerWithSpecialData();
            }
        }
    }

#endif

    // -------------------------------------------------------------------------------------------------

}
