using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(UICustomButton))]
public class UIHomeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerMoveHandler
{
    [Header("References")]
    [SerializeField] Image m_TopPanel;
    [SerializeField] TMP_Text m_Title;
    [SerializeField] Image icon;
    [SerializeField] UICustomButton m_Button;

    [Space(10)]
    [Header("Idle Values")]
    [SerializeField] float m_IdleTransitionDuration;
    [SerializeField] Color m_IdlePanelColor;
    [SerializeField] Color m_IdleTitleColor;
    [SerializeField] Vector2 m_IdleRectVector;
    
    [Space(10)]
    [Header("Hover Values")]
    [SerializeField] float m_HoverTransitionDuration;
    [SerializeField] Color m_HoverPanelColor;
    [SerializeField] Color m_HoverTitleColor;
    [SerializeField] Vector2 m_HoverRectVector;
    
    [Space(10)]
    [Header("Click Values")]        
    [SerializeField] float m_ClickTransitionDuration;
    [SerializeField] Color m_ClickPanelColor;
    [SerializeField] Color m_ClickTitleColor;
    [SerializeField] Vector2 m_ClickRectVector;

    public static event System.Action<UIHomeButton> ON_CLICK_HOME_BUTTON;
    private bool m_IsBlock = false;
    void Start()
    {
        UIHomeButton.ON_CLICK_HOME_BUTTON += HandleClick;
    }
    void OnDestroy()
    {
        UIHomeButton.ON_CLICK_HOME_BUTTON -= HandleClick;
    }
    void HandleClick(UIHomeButton _button)
    {
        m_IsBlock = _button != this;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_IsBlock) return;
        HandleClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_IsBlock) return;
        Hover();
        HandleHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_IsBlock) return;
        HandleIdle();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (m_IsBlock) return;
        HandleHover();
    }
    void ResetTween()
    {
        m_TopPanel.DOKill();
        m_Title?.DOKill();
        icon?.DOKill();
    }
    void HandleIdle()
    {
        ResetTween();
        // * Color
        m_TopPanel.DOColor(m_IdlePanelColor, m_IdleTransitionDuration).SetEase(Ease.OutBack);
        // * Anchor Position
        DOTween.To(() => m_TopPanel.GetComponent<RectTransform>().anchoredPosition, value => m_TopPanel.GetComponent<RectTransform>().anchoredPosition = value, m_IdleRectVector, m_IdleTransitionDuration).SetEase(Ease.OutBack);
        // * Title color
        m_Title?.DOColor(m_IdleTitleColor, m_IdleTransitionDuration).SetEase(Ease.OutBack);
        icon?.DOColor(m_IdleTitleColor, m_IdleTransitionDuration).SetEase(Ease.OutBack);

    }
    void HandleHover()
    {
        ResetTween();
        // * Color
        m_TopPanel.DOColor(m_HoverPanelColor, m_HoverTransitionDuration).SetEase(Ease.OutBack);
        // * Anchor Position
        DOTween.To(() => m_TopPanel.GetComponent<RectTransform>().anchoredPosition, value => m_TopPanel.GetComponent<RectTransform>().anchoredPosition = value, m_HoverRectVector, m_HoverTransitionDuration).SetEase(Ease.OutBack);
        // * Title color
        m_Title?.DOColor(m_HoverTitleColor, m_HoverTransitionDuration).SetEase(Ease.OutBack);
        icon?.DOColor(m_HoverTitleColor, m_HoverTransitionDuration).SetEase(Ease.OutBack);
    }
    void HandleClick()
    {
        ResetTween();
        // * Color
        m_TopPanel.DOColor(m_ClickPanelColor, m_ClickTransitionDuration).SetEase(Ease.OutBack);
        // * Anchor Position
        DOTween.To(() => m_TopPanel.GetComponent<RectTransform>().anchoredPosition, value => m_TopPanel.GetComponent<RectTransform>().anchoredPosition = value, m_ClickRectVector, m_ClickTransitionDuration).SetEase(Ease.OutBack);
        // * Title color
        m_Title?.DOColor(m_ClickTitleColor, m_ClickTransitionDuration).SetEase(Ease.OutBack);
        // * Play click sound
        icon?.DOColor(m_ClickTitleColor, m_ClickTransitionDuration).SetEase(Ease.OutBack);
        Invoke(nameof(Click), m_ClickTransitionDuration / 2);
    }
    void Click()
    {
        m_Button?.OnClick?.Invoke();
    }
    void Hover()
    {
        // * Play hover sound
    }
}
