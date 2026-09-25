using Inventory.Container;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{

    public abstract class InventoryView : MonoBehaviour
    {
        public InventoryObject inventory;
        public GameObject slotPrefab;

        private Vector2 _spriteSizeVisual = new Vector2(64, 64);

        protected InventorySlot[] slots;

        private static readonly MouseItem mouseItem = new MouseItem();
        private static GameObject _dragVisual;
        private RectTransform _dragVisualRect;
        private Canvas _dragVisualCanvas;
        private static Image _dragVisualImage;

        public virtual void OnLeftClick(InventorySlot slot)
        {
        }

        public virtual void OnRightClick(InventorySlot slot)
        {
        }

        public abstract void CreateSlots();

        private void Start()
        {
            CreateSlots();
            SlotEventBinder.BindInventoryUIEvent(gameObject, this);
            _dragVisual = GetDragVisual();
        }

        public void OnEnterInterface(GameObject obj)
        {
            mouseItem.ui = this;
        }

        public void OnExitInterface(GameObject obj)
        {
            mouseItem.ui = null;
        }

        public void OnEnter(InventorySlot slot)
        {
            mouseItem.toSlot = slot;
        }

        public void OnExit(InventorySlot slot)
        {
            if (mouseItem != null && mouseItem.toSlot != null)
                mouseItem.toSlot = null;
        }

        public void OnDragStart(InventorySlot slot)
        {
            if (slot.ID >= 0)
            {
                _dragVisualImage.sprite = inventory.database.GetItem[slot.ID].uiDisplay;
                _dragVisual.SetActive(true);
            }

            mouseItem.obj = _dragVisual;
            mouseItem.toSlot = slot;
            _dragVisualCanvas = mouseItem.obj.GetComponentInParent<Canvas>();
            _dragVisualRect = mouseItem.obj.GetComponent<RectTransform>();
        }

        public void OnDrag(InventorySlot slot, PointerEventData eventData)
        {
            if (!mouseItem.obj)
                return;
            // Преобразуем координаты мыши с экрана в координаты UI-пространства.
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _dragVisualRect.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector3 worldPoint
            );

            _dragVisualRect.position = worldPoint;
        }

        public void OnDragEnd(InventorySlot fromSlot)
        {
            InventorySlot toSlot = mouseItem.toSlot ??
                                   fromSlot; //проверка на null, если toSlot не пустой берет то, что в нем уже лежит, если null то берет значение из fromSlot

            inventory.MoveItem(toSlot, fromSlot);

            _dragVisual.SetActive(false);
            mouseItem.toSlot = null;
        }

        private GameObject GetDragVisual()
        {
            if (!_dragVisual)
            {
                _dragVisual = new GameObject("dragVisual");
                var rt = _dragVisual.AddComponent<RectTransform>();
                rt.sizeDelta = _spriteSizeVisual;
                _dragVisualImage = _dragVisual.AddComponent<Image>();
                _dragVisualImage.raycastTarget = false;
            }

            Canvas rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
            _dragVisual.transform.SetParent(rootCanvas.transform);
            _dragVisual.transform.SetAsLastSibling();
            _dragVisual.SetActive(false);

            return _dragVisual;
        }
    }

    public class MouseItem
    {
        public InventoryView ui;
        public GameObject obj;
        public InventorySlot item;
        public InventorySlot toSlot;
    }
}