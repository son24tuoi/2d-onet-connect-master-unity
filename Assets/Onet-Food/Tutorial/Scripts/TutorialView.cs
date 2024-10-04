using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialView : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private GameObject[] states;
    [SerializeField] private GameObject[] pageNavi;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private int _index;

    public void SetState(int index)
    {
        index = Mathf.Clamp(index, 0, states.Length - 1);

        for (int i = 0; i < states.Length; i++)
        {
            states[i].SetActive(i == index);
            pageNavi[i].SetActive(i == index);
        }

        _index = index;

        UpdateButton();
    }

    public void UpdateButton()
    {
        nextButton.interactable = _index < states.Length - 1;
        prevButton.interactable = _index > 0;
    }

    public void OnClickNextButton()
    {
        SetState(_index + 1);
    }

    public void OnClickPrevButton()
    {
        SetState(_index - 1);
    }
}
