using Events;
using Player;
using UnityEngine;
using Utils.Singleton;

namespace Camera
{
    public class CameraMovement : DontDestroyMonoBehaviourSingleton<CameraMovement>
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 cameraBounds;
        private Vector3 _lastMousePosition;
        private bool _dragging;

        private void Start()
        {
            EventsController.Subscribe<EventModels.Game.PlayerFingerPlaced>(this, OnDragStart);
            EventsController.Subscribe<EventModels.Game.PlayerFingerRemoved>(this, OnDragEnd);
        }

        private void OnDestroy()
        {
            EventsController.Unsubscribe<EventModels.Game.PlayerFingerPlaced>(OnDragStart);
            EventsController.Unsubscribe<EventModels.Game.PlayerFingerRemoved>(OnDragEnd);
        }
        
        public void SetMovementBounds(Vector2 bounds)
        {
            cameraBounds = bounds;
        }

        private void OnDragStart(EventModels.Game.PlayerFingerPlaced e)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;

            _lastMousePosition = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            _dragging = true;
        }

        private void OnDragEnd(EventModels.Game.PlayerFingerRemoved e)
        {
            _dragging = false;
        }

        private void LateUpdate()
        {
            if (_dragging)
            {
                var mousePosition = CameraHolder.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                var deltaPosition = _lastMousePosition - mousePosition;

                var cameraTransform = CameraHolder.Instance.MainCamera.transform;
                var newPosition = Vector3.Lerp(
                    cameraTransform.position,
                    cameraTransform.position - deltaPosition,
                    moveSpeed);

                //Using cinemachine would have allowed for a more flexible approach
                //Using a polygon collider per scene, instead of hard coded values
                var newPositionClamped = new Vector3(
                    Mathf.Clamp(newPosition.x, -cameraBounds.x, cameraBounds.x),
                    Mathf.Clamp(newPosition.y, -cameraBounds.y, cameraBounds.y),
                    newPosition.z
                );

                CameraHolder.Instance.MainCamera.transform.position = newPositionClamped;
                _lastMousePosition = mousePosition;
            }
        }
    }
}