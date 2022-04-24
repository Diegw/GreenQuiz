using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform itemPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform itemsHolder;
    [SerializeField] private float itemSpacing;
    [SerializeField] private bool horizontal, vertical;
    [SerializeField] private float hideThreshold;
    [SerializeField] private float startOffset;
    private List<RectTransform> itemsInsideHolder = new List<RectTransform>();
    private ScrollRect scrollRect;
    private Vector2 lastDragPosition;
    private bool positiveDrag;
    private float initialWidth, initialHeight;
    private float childWidth, childHeight;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;

    private void Awake()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        if(canvas != null)
        {
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        }
    }

    private void OnEnable()
    {
        if(scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(HandleScroll);
        }
        // MenuUI.OnSetCategoriesUIEvent += Initialize;
    }

    private void OnDisable()
    {
        if(scrollRect != null)
        {
            scrollRect.onValueChanged.RemoveListener(HandleScroll);
        }
        // MenuUI.OnSetCategoriesUIEvent -= Initialize;
    }

    public void Initialize()
    {
        scrollRect.vertical = vertical;
        scrollRect.horizontal = horizontal;
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

        if(itemsHolder == null || itemsHolder.childCount <= 0)
        {
            return;
        }
        // RectTransform[] itemsInsideHolder = InstantiateItems(categories.Count);
        // for (int i = 0; i < itemsHolder.childCount; i++)
        // {
        //     itemsInsideHolder[i] = itemsHolder.GetChild(i) as RectTransform;
        // }

        initialWidth = itemsHolder.rect.width;
        initialHeight = itemsHolder.rect.height;

        if(itemsInsideHolder != null || itemsInsideHolder.Count > 0)
        {
            childWidth = itemsInsideHolder[0].rect.width;
            childHeight = itemsInsideHolder[0].rect.height;
        }

        horizontal = !vertical;
        if (vertical)
        {
            InitializeContentVertical();
        }
        else
        {
            InitializeContentHorizontal();
        }
    }

    public void InstantiateItems(int quantity)
    {
        if(itemsInsideHolder == null)
        {
            itemsInsideHolder = new List<RectTransform>();
        }

        int quantityLeft = quantity - itemsInsideHolder.Count;
        if(quantityLeft == 0)
        {
            return;
        }

        if(quantityLeft < 0)
        {
            for (int i = Mathf.Abs(quantityLeft) - 1; i >= 0 ; i--)
            {
                RectTransform rectTransform = itemsInsideHolder[i];
                itemsInsideHolder.Remove(rectTransform);
                Destroy(rectTransform.gameObject);
            }
        }
        else
        {
            for (int i = 0; i < quantityLeft; i++)
            {
                itemsInsideHolder.Add(Instantiate(itemPrefab, Vector2.zero, Quaternion.identity, itemsHolder.transform));
            }
        }
    }

    public void SetItemsSprites(Sprite[] sprites)
    {
        if(sprites.Length != itemsInsideHolder.Count)
        {
            Debug.LogError($"Trying to assing {sprites.Length} sprites into {itemsInsideHolder.Count} items");
            return;
        }
        for (int i = 0; i < itemsInsideHolder.Count; i++)
        {
            Image image = itemsInsideHolder[i].GetComponent<Image>();
            if(image)
            {
                image.sprite = sprites[i];       
            }
        }
    }

    private void InitializeContentHorizontal()
    {
        float initialX = 0 - (initialWidth * 0.5f);
        float positionOffset = childWidth * 0.5f;
        for (int i = 0; i < itemsInsideHolder.Count; i++)
        {
            Vector2 childPos = Vector2.zero;
            if(i == 0)
            {
                childPos.x -= childWidth + itemSpacing;
            }
            else if(i > 1)
            {
                childPos.x += ((i - 1) * (childWidth + itemSpacing));
            }
            itemsInsideHolder[i].localPosition = childPos;
            // Vector2 childPos = childItemsHolder[i].localPosition;
            // childPos.x = initialX + positionOffset + i * (childWidth + itemSpacing) - startOffset;
            // childItemsHolder[i].localPosition = childPos;
        }
    }

    private void InitializeContentVertical()
    {
        float initialY = 0 - (initialHeight * 0.5f);
        float positionOffset = childHeight * 0.5f;
        for (int i = 0; i < itemsInsideHolder.Count; i++)
        {
            Vector2 childPos = itemsInsideHolder[i].localPosition;
            childPos.y = initialY + positionOffset + i * (childHeight + itemSpacing);
            itemsInsideHolder[i].localPosition = childPos;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AdjustImages();
        HandleScroll(itemsHolder.position);
    }

    private Image GetImageFromRaycast(EDirection side = EDirection.NONE)
    {
        Image image = null;
        if (InputManager.EventSystem != null && graphicRaycaster != null)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            float positionX = (Screen.width / 2) + ((itemSpacing + 5) * (int)side);
            pointerEventData = new PointerEventData(InputManager.EventSystem);
            pointerEventData.position = new Vector3(positionX, Screen.height / 2, 0);
            graphicRaycaster.Raycast(pointerEventData, results);
            if (results != null && results.Count == 1)
            {
                image = results[0].gameObject.GetComponent<Image>();
            }
        }
        return image;
    }

    private void AdjustImages()
    {
        Image imageHit = GetImageFromRaycast(EDirection.NONE);
        Vector2 newImagesPosition = Vector2.zero;
        if(imageHit == null)
        {
            Image rightImage = GetImageFromRaycast(EDirection.NEXT);
            Image leftImage = GetImageFromRaycast(EDirection.PREVIOUS);
            if(rightImage != null && leftImage != null)
            {
                float distanceLeftImage = (Screen.width / 2) - leftImage.transform.position.x;
                float distanceRightImage = rightImage.transform.position.x - (Screen.width / 2);
                if(distanceLeftImage > distanceRightImage)
                {
                    newImagesPosition = new Vector2(itemsHolder.anchoredPosition.x - distanceRightImage, itemsHolder.anchoredPosition.y);
                }
                else
                {
                    newImagesPosition = new Vector2(itemsHolder.anchoredPosition.x + distanceLeftImage, itemsHolder.anchoredPosition.y);
                }
            }
        }
        else
        {
            float distance = (Screen.width / 2) - imageHit.transform.position.x;
            newImagesPosition = new Vector2(itemsHolder.anchoredPosition.x + distance, itemsHolder.anchoredPosition.y);
        }
        itemsHolder.anchoredPosition = newImagesPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (vertical)
        {
            positiveDrag = eventData.position.y > lastDragPosition.y;
        }
        else if (horizontal)
        {
            positiveDrag = eventData.position.x > lastDragPosition.x;
        }

        lastDragPosition = eventData.position;
    }

    public void HandleScroll(Vector2 position)
    {
        int currentIndex = positiveDrag ? scrollRect.content.childCount - 1 : 0;
        var currentItem = scrollRect.content.GetChild(currentIndex);
        if (!ReachedThreshold(currentItem))
        {
            return;
        }

        int lastIndex = positiveDrag ? 0 : scrollRect.content.childCount - 1;
        Transform lastItem = scrollRect.content.GetChild(lastIndex);
        Vector2 newPosition = lastItem.position;

        if (positiveDrag)
        {
            if(vertical)
            {
                newPosition.y = lastItem.position.y - childHeight + itemSpacing;
            }
            else
            {
                newPosition.x = lastItem.position.x - childWidth - itemSpacing;
            }
        }
        else
        {
            if(vertical)
            {
                newPosition.y = lastItem.position.y + childHeight - itemSpacing;
            }
            else
            {
                newPosition.x = lastItem.position.x + childWidth + itemSpacing;
            }
        }

        currentItem.position = newPosition;
        currentItem.SetSiblingIndex(lastIndex);
    }

    private bool ReachedThreshold(Transform item)
    {
        if (vertical)
        {
            float posYThreshold = transform.position.y + initialHeight * 0.5f + hideThreshold;
            float negYThreshold = transform.position.y - initialHeight * 0.5f - hideThreshold;
            return positiveDrag ? item.position.y - childWidth * 0.5f > posYThreshold : item.position.y + childWidth * 0.5f < negYThreshold;
        }
        else
        {
            float posXThreshold = transform.position.x + initialWidth * 0.5f + hideThreshold;
            float negXThreshold = transform.position.x - initialWidth * 0.5f - hideThreshold;
            return positiveDrag ? item.position.x - childWidth * 0.5f > posXThreshold : item.position.x + childWidth * 0.5f < negXThreshold;
        }
    }
}