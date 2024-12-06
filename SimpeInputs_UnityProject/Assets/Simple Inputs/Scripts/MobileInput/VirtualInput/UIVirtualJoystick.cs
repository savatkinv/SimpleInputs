using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace SimpleInputs.MobileInput.VirtualInput
{
    public class UIVirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [System.Serializable]
        public class Event : UnityEvent<Vector2> { }

        [Header("Rect References")]
        public RectTransform containerRect;
        public RectTransform handleRect;

        [Header("Settings")]
        public float joystickRange = 50f;
        public float magnitudeMultiplier = 1f;
        public bool invertXOutputValue;
        public bool invertYOutputValue;

        public CanvasGroup canvasGroup;
        public float alphaMin = 0.2f;
        public float alphaMax = 0.8f;
        private float targetAlpha = 0.8f;

        [Header("Output")]
        public Event joystickOutputEvent;

        private void Start()
        {
            targetAlpha = alphaMin;

            SetupHandle();
        }

        private void Update()
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime * 4f);
        }

        private void SetupHandle()
        {
            if (handleRect)
            {
                UpdateHandleRectPosition(Vector2.zero);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);

            targetAlpha = alphaMax;
        }

        public void OnDrag(PointerEventData eventData)
        {

            RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);

            position = ApplySizeDelta(position);

            Vector2 clampedPosition = ClampValuesToMagnitude(position);

            Vector2 outputPosition = ApplyInversionFilter(position);

            OutputPointerEventValue(outputPosition * magnitudeMultiplier);

            if (handleRect)
            {
                UpdateHandleRectPosition(clampedPosition * joystickRange);
            }

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OutputPointerEventValue(Vector2.zero);

            if (handleRect)
            {
                UpdateHandleRectPosition(Vector2.zero);
            }

            targetAlpha = alphaMin;
        }

        private void OutputPointerEventValue(Vector2 pointerPosition)
        {
            joystickOutputEvent.Invoke(pointerPosition);
        }

        private void UpdateHandleRectPosition(Vector2 newPosition)
        {
            handleRect.anchoredPosition = newPosition;
        }

        Vector2 ApplySizeDelta(Vector2 position)
        {
            float x = (position.x / containerRect.sizeDelta.x) * 2.5f;
            float y = (position.y / containerRect.sizeDelta.y) * 2.5f;
            return new Vector2(x, y);
        }

        Vector2 ClampValuesToMagnitude(Vector2 position)
        {
            return Vector2.ClampMagnitude(position, 1);
        }

        Vector2 ApplyInversionFilter(Vector2 position)
        {
            if (invertXOutputValue)
            {
                position.x = InvertValue(position.x);
            }

            if (invertYOutputValue)
            {
                position.y = InvertValue(position.y);
            }

            return position;
        }

        float InvertValue(float value)
        {
            return -value;
        }

    }
}