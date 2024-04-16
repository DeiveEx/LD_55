using UnityEngine;

public class UI_InstructionScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject[] _slides;
    
    public bool isOpen = false;
    private int _currentSlideIndex;

    private void Awake()
    {
        Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isOpen)
                Close();
            else
                Open();
        }
    }

    public void Open()
    {
        isOpen = true;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        ShowSlide(0);
    }

    public void Close()
    {
        isOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        for (int i = 0; i < _slides.Length; i++)
        {
            _slides[i].SetActive(false);
        }
    }

    public void ShowSlide(int slideIndex)
    {
        _currentSlideIndex = slideIndex;
        
        for (int i = 0; i < _slides.Length; i++)
        {
            _slides[i].SetActive(i == slideIndex);
        }
    }

    public void ShowNextSlide()
    {
        _currentSlideIndex++;

        if (_currentSlideIndex >= _slides.Length)
        {
            Close();
        }
        else
        {
            ShowSlide(_currentSlideIndex);
        }
    }
}
