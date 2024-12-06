using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;

namespace SimpleInputs.MobileInput.VirtualInput
{
    public class UIVirtualTouchZone : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [System.Serializable]
        public class Event : UnityEvent<Vector2> { }

        [Header("Rect References")]
        public RectTransform containerRect;
        public RectTransform handleRect;

        [Header("Settings")]
        public bool clampToMagnitude;
        public float magnitudeMultiplier = 1f;
        public float lerpSpeed = 1f;
        public float deadZone = 1f;
        public bool invertXOutputValue;
        public bool invertYOutputValue;

        private Vector2 pointerDownPosition;
        private Vector2 currentPointerPosition;
        private bool needReset = false;
        private bool skipFirstDrag = false;
        private int pointerId = 0;
        private bool isDragged = false;
        private Queue<Vector2> outputQueue = new Queue<Vector2>();
        private Vector2 currentOutputValue;
        private Vector2 targetOutputValue;

        [Header("Output")]
        public Event touchZoneOutputEvent;
        public UnityEvent touchZoneOutputStart;
        public UnityEvent touchZoneOutputEnd;

        void Start()
        {
            SetupHandle();
        }

        private void Update()
        {
            if (needReset)
                touchZoneOutputEvent.Invoke(Vector2.zero);

            if (outputQueue.Count > 0)
            {
                targetOutputValue = outputQueue.Dequeue();
            } else
            {
                targetOutputValue = Vector2.zero;
            }

            if (currentOutputValue.magnitude > deadZone * 0.01f || targetOutputValue.magnitude > 0)
            {
                currentOutputValue = Vector2.Lerp(currentOutputValue, targetOutputValue, Time.deltaTime * lerpSpeed);
                touchZoneOutputEvent.Invoke(currentOutputValue);
            }
        }

        private void OnEnable()
        {
            Reset();
        }

        private void OnDisable()
        {
            Reset();
        }

        private void Reset()
        {
            touchZoneOutputEvent.Invoke(Vector2.zero);
            touchZoneOutputEnd?.Invoke();
            outputQueue.Clear();
            needReset = false;
            isDragged = false;
        }

        private void SetupHandle()
        {
            if (handleRect)
            {
                SetObjectActiveState(handleRect.gameObject, false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isDragged)
                return;

            pointerId = eventData.pointerId;
            isDragged = true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out pointerDownPosition);

            if (handleRect)
            {
                SetObjectActiveState(handleRect.gameObject, true);
                UpdateHandleRectPosition(pointerDownPosition);
            }

            skipFirstDrag = true;

            touchZoneOutputStart?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (pointerId != eventData.pointerId)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out currentPointerPosition);
            Vector2 positionDelta = GetDeltaBetweenPositions(pointerDownPosition, currentPointerPosition);

            if (clampToMagnitude)
                positionDelta = ClampValuesToMagnitude(positionDelta);

            Vector2 outputPosition = ApplyInversionFilter(positionDelta);

            if (outputPosition.magnitude < deadZone)
            {
                outputPosition = Vector2.zero;
            }

            if (!skipFirstDrag)
                OutputPointerEventValue(outputPosition * magnitudeMultiplier);

            pointerDownPosition = currentPointerPosition;

            if (handleRect)
            {
                SetObjectActiveState(handleRect.gameObject, true);
                UpdateHandleRectPosition(pointerDownPosition);
            }

            skipFirstDrag = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (pointerId != eventData.pointerId)
                return;

            pointerDownPosition = Vector2.zero;
            currentPointerPosition = Vector2.zero;

            OutputPointerEventValue(Vector2.zero);

            if (handleRect)
            {
                SetObjectActiveState(handleRect.gameObject, false);
                UpdateHandleRectPosition(Vector2.zero);
            }

            isDragged = false;
            touchZoneOutputEnd?.Invoke();
        }

        void OutputPointerEventValue(Vector2 pointerPosition)
        {
            outputQueue.Enqueue(pointerPosition);
            // touchZoneOutputEvent.Invoke(pointerPosition);
        }

        void UpdateHandleRectPosition(Vector2 newPosition)
        {
            handleRect.anchoredPosition = newPosition;
        }

        void SetObjectActiveState(GameObject targetObject, bool newState)
        {
            targetObject.SetActive(newState);
        }

        Vector2 GetDeltaBetweenPositions(Vector2 firstPosition, Vector2 secondPosition)
        {
            return secondPosition - firstPosition;
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
