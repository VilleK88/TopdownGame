using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public TextMeshProUGUI countText;
    Button button;

    [Header("UI")]
    [HideInInspector] public Image image;
    [HideInInspector] public string name;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    bool dragging = false;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        name = newItem.name;
        image.sprite = newItem.icon;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling(); //
        image.raycastTarget = false;
        countText.raycastTarget = false;
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        countText.raycastTarget = true;
        dragging = false;
    }

    public void UseItem()
    {
        if (item != null)
        {
            if(!dragging)
            {
                item.Use();
                if(name != "Wooden axe")
                {
                    count--;
                }
                else
                {

                }

                if (count > 0)
                {
                    RefreshCount();
                }
                else
                {
                    if(name != "Wooden axe")
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
