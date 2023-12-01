using TMPro;
using UnityEngine;

public class SlideInEditorPanel : MonoBehaviour
{
    [SerializeField] private float slideSpeed = 10f;
    private bool isSlidIn = false;

    private RectTransform rectTransform;
    private float slidInAnchorMinX = 0;
    private float slidInAnchorMaxX = 0;
    private float slidOutAnchorMinX = 0;
    private float slidOutAnchorMaxX = 0;

    private float desiredAnchorMinX = 0;
    private float desiredAnchorMaxX = 0;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        var anchorXDiff = rectTransform.anchorMax.x - rectTransform.anchorMin.x;
        slidOutAnchorMinX = rectTransform.anchorMin.x;
        slidInAnchorMinX = rectTransform.anchorMin.x - anchorXDiff;
        slidOutAnchorMaxX = rectTransform.anchorMax.x;
        slidInAnchorMaxX = rectTransform.anchorMax.x - anchorXDiff;

        desiredAnchorMinX = slidOutAnchorMinX;
        desiredAnchorMaxX = slidOutAnchorMaxX;
    }

    void Update()
    {
        rectTransform.anchorMin = new Vector2(Mathf.Lerp(rectTransform.anchorMin.x, desiredAnchorMinX, slideSpeed * Time.deltaTime), rectTransform.anchorMin.y);
        rectTransform.anchorMax = new Vector2(Mathf.Lerp(rectTransform.anchorMax.x, desiredAnchorMaxX, slideSpeed * Time.deltaTime), rectTransform.anchorMax.y);
    }

    public void ToggleSlide()
    {

        if (isSlidIn)
        {
            SlideOut();
        }
        else
        {
            SlideIn();
        }

        isSlidIn = !isSlidIn;
    }

    private void SlideIn()
    {
        desiredAnchorMinX = slidInAnchorMinX;
        desiredAnchorMaxX = slidInAnchorMaxX;
    }

    private void SlideOut()
    {
        desiredAnchorMinX = slidOutAnchorMinX;
        desiredAnchorMaxX = slidOutAnchorMaxX;
    }
}
