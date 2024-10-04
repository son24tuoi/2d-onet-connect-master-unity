using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Element")]
    public GameObject nextButton;
    public GameObject prevButton;

    [Header("Setting")]
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    public int totalPages = 1;

    private float _difference;
    private Vector3 _panelLocation;
    private int _currentPage = 1;
    private bool _isMoving = false;
    private List<LevelHolder> _levelHolders;
    private int _amountLevelPerPage;
    private Rect _panelDimensions;
    private Vector3 _originalPanelLocation;

    public void Init(int totalPages, int amountLevelPerPage, LevelHolder[] levelHolders, Rect panelDimensions)
    {
        this.totalPages = totalPages;
        _levelHolders = new List<LevelHolder>(levelHolders);
        _amountLevelPerPage = amountLevelPerPage;
        _panelDimensions = panelDimensions;

        _panelLocation = transform.position;
        _originalPanelLocation = transform.position;
        ShowButton();
    }

    public void OnDrag(PointerEventData data)
    {
        _difference = data.pressPosition.x - data.position.x;
        transform.position = _panelLocation - new Vector3(_difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (_isMoving)
            return;

        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = _panelLocation;

            if (percentage > 0 && _currentPage < totalPages - 1)
            {
                _currentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
            }
            else if (percentage < 0 && _currentPage > 0)
            {
                _currentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
            }
            StartCoroutine(IESmoothMove(transform.position, newLocation, easing));
            _panelLocation = newLocation;

            ShowButton();
        }
        else
        {
            StartCoroutine(IESmoothMove(transform.position, _panelLocation, easing));
        }
    }

    private IEnumerator IESmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        _isMoving = true;

        float t = 0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        LoadPages(_currentPage);
        MovePage(_currentPage);

        _isMoving = false;
    }

    public void OnClickNextButton()
    {
        if (_currentPage < totalPages && !_isMoving)
        {
            _currentPage++;

            Vector3 newLocation = _panelLocation;
            newLocation += new Vector3(-Screen.width, 0, 0);

            StartCoroutine(IESmoothMove(transform.position, newLocation, easing));
            _panelLocation = newLocation;

            ShowButton();
        }
    }

    public void OnClickPrevButton()
    {
        if (_currentPage > 0 && !_isMoving)
        {
            _currentPage--;

            Vector3 newLocation = _panelLocation;
            newLocation += new Vector3(Screen.width, 0, 0);

            StartCoroutine(IESmoothMove(transform.position, newLocation, easing));
            _panelLocation = newLocation;

            ShowButton();
        }
    }

    private void ShowButton()
    {
        nextButton.SetActive(_currentPage < totalPages - 1);
        prevButton.SetActive(_currentPage > 0);
    }

    public void MoveToPage(int page)
    {
        if (page >= 0 && page < totalPages)
        {
            Vector3 newLocation = _panelLocation + (page - _currentPage) * new Vector3(-Screen.width, 0, 0);
            _currentPage = page;
            StartCoroutine(IESmoothMove(transform.position, newLocation, easing));
            _panelLocation = newLocation;
            ShowButton();
        }
    }

    public void LoadPages(int pageIndex)
    {
        _currentPage = pageIndex;

        int firstIndex = GetFirstIndex(pageIndex);

        for (int i = 0; i < _levelHolders.Count; i++)
        {
            _levelHolders[i].LoadLevels(firstIndex + (i * _amountLevelPerPage));
        }
    }

    public void MovePage(int pageIndex)
    {
        _panelLocation = _originalPanelLocation;
        transform.position = _panelLocation;

        for (int i = 0; i < _levelHolders.Count; i++)
        {
            _levelHolders[i].SetLocalPosition(GetLocalPositionPage(pageIndex, i));
            _levelHolders[i].transform.SetSiblingIndex(i);
            _levelHolders[i].name = "Page_" + i;
        }

        ShowButton();
    }

    private Vector2 GetLocalPositionPage(int pageIndex, int levelHolderIndex)
    {
        if (pageIndex == 0)
            return new Vector2(_panelDimensions.width * levelHolderIndex, 0);

        if (pageIndex == totalPages - 1)
            return new Vector2(_panelDimensions.width * (levelHolderIndex - 3), 0);

        if (pageIndex == totalPages - 2)
            return new Vector2(_panelDimensions.width * (levelHolderIndex - 2), 0);

        return new Vector2(_panelDimensions.width * (levelHolderIndex - 1), 0);
    }

    private int GetFirstIndex(int pageIndex)
    {
        if (pageIndex == 0)
            return 0;

        if (pageIndex > totalPages - 3)
            return (totalPages - 4) * _amountLevelPerPage;

        return (pageIndex - 1) * _amountLevelPerPage;
    }
}
