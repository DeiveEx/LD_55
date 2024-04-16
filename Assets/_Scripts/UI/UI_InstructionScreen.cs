using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InstructionScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    public bool isOpen = false;

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
    }

    public void Close()
    {
        isOpen = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
