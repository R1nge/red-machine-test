using Events;
using Player;
using UnityEngine;
using Utils.Singleton;

namespace Camera
{
    public class CameraMovement : DontDestroyMonoBehaviourSingleton<CameraMovement>
    {
        [SerializeField] private float moveSpeed;
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

        private void OnDragStart(EventModels.Game.PlayerFingerPlaced e)
        {
            if(PlayerController.PlayerState != PlayerState.Scrolling)
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
                var deltaPosition = _lastMousePosition - mousePosition; // This is the change in position
                
                var cameraTransform = CameraHolder.Instance.MainCamera.transform;
                var newPosition = Vector3.Lerp(
                    cameraTransform.position,
                    cameraTransform.position - deltaPosition,
                    moveSpeed);

                CameraHolder.Instance.MainCamera.transform.position = newPosition;
                _lastMousePosition = mousePosition;
            }
        }
    }
}