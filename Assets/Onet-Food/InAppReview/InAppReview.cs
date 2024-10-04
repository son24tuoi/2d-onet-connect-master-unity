using System.Collections;
using System.Collections.Generic;
using Google.Play.Review;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InAppReview : MonoBehaviour
{
    [SerializeField] private RateCanvas rateCanvasPrefab;

    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private RateCanvas rateCanvas;

    private void Awake()
    {
        RateButton.OnClickEvent += Init;
    }

    private void OnDestroy()
    {
        RateButton.OnClickEvent -= Init;
    }

    public void Init()
    {
        Debug.Log("Start Rate");
        _reviewManager = new ReviewManager();

        StartCoroutine(IEReview());
    }

    private IEnumerator IEReview()
    {
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Debug.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Debug.LogError(launchFlowOperation.Error.ToString());
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }

    public void ShowRateCanvas()
    {
        if (rateCanvas == null)
        {
            rateCanvas = Instantiate(rateCanvasPrefab, transform);
        }
        rateCanvas.gameObject.SetActive(true);
    }



















#if UNITY_EDITOR

    [CustomEditor(typeof(InAppReview))]
    public class InAppReview_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InAppReview target = (InAppReview)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.ShowRateCanvas)))
            {
                target.ShowRateCanvas();
            }
        }
    }

#endif
}
