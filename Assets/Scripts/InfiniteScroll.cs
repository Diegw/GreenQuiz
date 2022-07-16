using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static Action<EMenuCategory, EMenuMode, EMenuCourse> OnEndDragEvent;
    
    [SerializeField] private RectTransform itemPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform itemsHolder;
    [SerializeField] private float itemSpacing;
    [SerializeField] private bool horizontal, vertical;
    [SerializeField] private float hideThreshold;
    [SerializeField] private float startOffset;
    [SerializeField] private List<SMenuItem> itemsInsideHolder = new List<SMenuItem>();
    private SettingsMenu _menuSettings = null;
    private ScrollRect _scrollRect;
    private Vector2 _lastDragPosition;
    private bool _positiveDrag;
    private float _initialWidth, _initialHeight;
    private float _childWidth, _childHeight;
    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData;
    private EMenuState _currentState;
    [Serializable] private struct SMenuItem
    {
        public SMenuItem(EMenuCategory category, EMenuMode mode, EMenuCourse course, RectTransform rect)
        {
            _category = category;
            _mode = mode;
            _course = course;
            _rectTransform = rect;
            _image = rect == null ? null : rect.GetComponent<Image>();
        }
        public EMenuCategory Category => _category;
        public EMenuMode Mode => _mode;
        public EMenuCourse Course => _course;
        public RectTransform RectTransform => _rectTransform;
        public Image Image => _image;

        [SerializeField] private EMenuCategory _category;
        [SerializeField] private EMenuMode _mode;
        [SerializeField] private EMenuCourse _course;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
    }

    private void Awake()
    {
        _menuSettings = SettingsManager.Menu;
        if(_menuSettings == null)
        {
            Debug.LogError("Settings Menu is null");
            return;
        }
        _scrollRect = GetComponentInChildren<ScrollRect>();
        if(canvas != null)
        {
            _graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        }
    }

    private void OnEnable()
    {
        Menu.OnStateChangedEvent += SetCurrentState;
        if(_scrollRect != null)
        {
            _scrollRect.onValueChanged.AddListener(HandleScroll);
        }
    }

    private void OnDisable()
    {
        Menu.OnStateChangedEvent -= SetCurrentState;
        if(_scrollRect != null)
        {
            _scrollRect.onValueChanged.RemoveListener(HandleScroll);
        }
    }

    private void SetCurrentState(EMenuState newState)
    {
        _currentState = newState;
    }

    public void Initialize()
    {
        _scrollRect.vertical = vertical;
        _scrollRect.horizontal = horizontal;
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

        if(itemsHolder == null || itemsHolder.childCount <= 0)
        {
            return;
        }

        Rect rect = itemsHolder.rect;
        _initialWidth = rect.width;
        _initialHeight = rect.height;

        if(itemsInsideHolder != null || itemsInsideHolder.Count > 0)
        {
            _childWidth = itemsInsideHolder[0].RectTransform.rect.width;
            _childHeight = itemsInsideHolder[0].RectTransform.rect.height;
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
            itemsInsideHolder = new List<SMenuItem>();
        }

        SMenuItem menuItem;
        for (int i = 0; i < itemsInsideHolder.Count; i++)
        {
            menuItem = itemsInsideHolder[i];
            Destroy(menuItem.RectTransform.gameObject);
        }
        itemsInsideHolder.Clear();
        
        EMenuCategory[] categories = null;
        EMenuMode[] modes = null;
        EMenuCourse[] courses = null;
        switch (_currentState)
        {
            case EMenuState.CATEGORIES:
            {
                categories = _menuSettings.GetCategories();
                break;
            }
            case EMenuState.MODES:
            {
                modes = _menuSettings.GetModes();
                break;
            }
            case EMenuState.COURSES:
            {
                courses = _menuSettings.GetCategoryCourses(GameManager.Category);
                break;
            } 
        }
        for (int i = 0; i < quantity; i++)
        {
            EMenuCategory category = categories == null ? EMenuCategory.NONE : categories[i];
            EMenuMode mode = modes == null ? EMenuMode.NONE : modes[i];
            EMenuCourse course = courses == null ? EMenuCourse.NONE : courses[i];
            
            RectTransform rect = Instantiate(itemPrefab, Vector2.zero, Quaternion.identity, itemsHolder.transform);
            SMenuItem newMenuItem = new SMenuItem(category, mode, course, rect);
            itemsInsideHolder.Add(newMenuItem);
        }
    }

    public void SetItemsSprites(Sprite[] sprites)
    {
        if(sprites.Length != itemsInsideHolder.Count)
        {
            Debug.LogError($"Trying to assign {sprites.Length} sprites into {itemsInsideHolder.Count} items");
            return;
        }
        for (int i = 0; i < itemsInsideHolder.Count; i++)
        {
            Image image = itemsInsideHolder[i].Image;
            if(image)
            {
                image.sprite = sprites[i];       
            }
        }
    }

    private void InitializeContentHorizontal()
    {
        float initialX = 0 - (_initialWidth * 0.5f);
        float positionOffset = _childWidth * 0.5f;
        for (int i = 0; i < itemsInsideHolder.Count; i++)
        {
            Vector2 childPos = Vector2.zero;
            if(i == 0)
            {
                childPos.x -= _childWidth + itemSpacing;
            }
            else if(i > 1)
            {
                childPos.x += ((i - 1) * (_childWidth + itemSpacing));
            }
            itemsInsideHolder[i].RectTransform.localPosition = childPos;
            // Vector2 childPos = childItemsHolder[i].localPosition;
            // childPos.x = initialX + positionOffset + i * (childWidth + itemSpacing) - startOffset;
            // childItemsHolder[i].localPosition = childPos;
        }
    }

    private void InitializeContentVertical()
    {
        float initialY = 0 - (_initialHeight * 0.5f);
        float positionOffset = _childHeight * 0.5f;
        for (int i = 0; i < itemsInsideHolder.Count; i++)
        {
            Vector2 childPos = itemsInsideHolder[i].RectTransform.localPosition;
            childPos.y = initialY + positionOffset + i * (_childHeight + itemSpacing);
            itemsInsideHolder[i].RectTransform.localPosition = childPos;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastDragPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Image imageHit = AdjustAndPick();
        SearchAndInform(imageHit);
        HandleScroll(itemsHolder.position);
    }

    private Image AdjustAndPick()
    {
        Image imageHit = null;
        Image firstImageHit = GetImageFromRaycast(EDirection.NONE);
        Vector2 newImagesPosition = Vector2.zero;
        if(firstImageHit == null)
        {
            Image rightImage = GetImageFromRaycast(EDirection.NEXT);
            Image leftImage = GetImageFromRaycast(EDirection.PREVIOUS);
            if(rightImage != null && leftImage != null)
            {
                float distanceLeftImage = (Screen.width / 2) - leftImage.transform.position.x;
                float distanceRightImage = rightImage.transform.position.x - (Screen.width / 2);
                if(distanceLeftImage > distanceRightImage)
                {
                    imageHit = rightImage;
                    newImagesPosition = new Vector2(itemsHolder.anchoredPosition.x - distanceRightImage, itemsHolder.anchoredPosition.y);
                }
                else
                {
                    imageHit = leftImage;
                    newImagesPosition = new Vector2(itemsHolder.anchoredPosition.x + distanceLeftImage, itemsHolder.anchoredPosition.y);
                }
            }
        }
        else
        {
            imageHit = firstImageHit;
            float distance = (Screen.width / 2) - firstImageHit.transform.position.x;
            newImagesPosition = new Vector2(itemsHolder.anchoredPosition.x + distance, itemsHolder.anchoredPosition.y);
        }
        itemsHolder.anchoredPosition = newImagesPosition;
        return imageHit;
    }

    private void SearchAndInform(Image image)
    {
        SMenuItem newMenuItem = new SMenuItem();
        if (image == null)
        {
            return;
        }
        foreach (var menuItem in itemsInsideHolder)
        {
            if (menuItem.Image != null && menuItem.Image == image)
            {
                newMenuItem = menuItem;
                break;
            }
        }
        OnEndDragEvent?.Invoke(newMenuItem.Category, newMenuItem.Mode, newMenuItem.Course);
    }

    private Image GetImageFromRaycast(EDirection side = EDirection.NONE)
    {
        Image image = null;
        if (InputManager.EventSystem != null && _graphicRaycaster != null)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            float positionX = (Screen.width / 2) + ((itemSpacing + 5) * (int)side);
            _pointerEventData = new PointerEventData(InputManager.EventSystem);
            _pointerEventData.position = new Vector3(positionX, Screen.height / 2, 0);
            _graphicRaycaster.Raycast(_pointerEventData, results);
            foreach (RaycastResult result in results)
            {
                image = result.gameObject.GetComponent<Image>();
                if (image != null)
                {
                    break;
                }
            }
        }
        return image;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (vertical)
        {
            _positiveDrag = eventData.position.y > _lastDragPosition.y;
        }
        else if (horizontal)
        {
            _positiveDrag = eventData.position.x > _lastDragPosition.x;
        }

        _lastDragPosition = eventData.position;
    }

    public void HandleScroll(Vector2 position)
    {
        int currentIndex = _positiveDrag ? _scrollRect.content.childCount - 1 : 0;
        var currentItem = _scrollRect.content.GetChild(currentIndex);
        if (!ReachedThreshold(currentItem))
        {
            return;
        }

        int lastIndex = _positiveDrag ? 0 : _scrollRect.content.childCount - 1;
        Transform lastItem = _scrollRect.content.GetChild(lastIndex);
        Vector2 newPosition = lastItem.position;

        if (_positiveDrag)
        {
            if(vertical)
            {
                newPosition.y = lastItem.position.y - _childHeight + itemSpacing;
            }
            else
            {
                newPosition.x = lastItem.position.x - _childWidth - itemSpacing;
            }
        }
        else
        {
            if(vertical)
            {
                newPosition.y = lastItem.position.y + _childHeight - itemSpacing;
            }
            else
            {
                newPosition.x = lastItem.position.x + _childWidth + itemSpacing;
            }
        }

        currentItem.position = newPosition;
        currentItem.SetSiblingIndex(lastIndex);
    }

    private bool ReachedThreshold(Transform item)
    {
        if (vertical)
        {
            float posYThreshold = transform.position.y + _initialHeight * 0.5f + hideThreshold;
            float negYThreshold = transform.position.y - _initialHeight * 0.5f - hideThreshold;
            return _positiveDrag ? item.position.y - _childWidth * 0.5f > posYThreshold : item.position.y + _childWidth * 0.5f < negYThreshold;
        }
        else
        {
            float posXThreshold = transform.position.x + _initialWidth * 0.5f + hideThreshold;
            float negXThreshold = transform.position.x - _initialWidth * 0.5f - hideThreshold;
            return _positiveDrag ? item.position.x - _childWidth * 0.5f > posXThreshold : item.position.x + _childWidth * 0.5f < negXThreshold;
        }
    }
}