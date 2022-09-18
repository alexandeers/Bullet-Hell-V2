using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

    [SerializeField] Image image;

    public event System.Action<ItemSlot> OnPointerEnterEvent;
    public event System.Action<ItemSlot> OnPointerExitEvent;
    public event System.Action<ItemSlot> OnRightClickEvent;
    public event System.Action<ItemSlot> OnBeginDragEvent;
    public event System.Action<ItemSlot> OnEndDragEvent;
    public event System.Action<ItemSlot> OnDragEvent;
    public event System.Action<ItemSlot> OnDropEvent;

    Color normalColor = Color.white;
    Color disabledColor = new Color(1f, 1f, 1f, 0f);

    private Item _item;
    public Item item {
        get { return _item ; }
        set {
            _item = value;

            if(_item == null) {
                image.color = disabledColor;
                } else {
                    image.sprite = _item.icon;
                    image.color = normalColor;
                }
            }
        }

    protected virtual void OnValidate() {
        if (image == null) {
            image =GetComponent<Image>();
        }
    }

    public virtual bool CanReceiveItem(Item item) 
    {
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Right) {
            if(OnRightClickEvent != null) {
                OnRightClickEvent(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(OnPointerEnterEvent != null) {
            OnPointerEnterEvent(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(OnPointerExitEvent != null) {
            OnPointerExitEvent(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(OnBeginDragEvent != null) {
            OnBeginDragEvent(this);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(OnEndDragEvent != null) {
            OnEndDragEvent(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(OnDragEvent != null) {
            OnDragEvent(this);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(OnDropEvent != null) {
            OnDropEvent(this);
        }
    }

}


