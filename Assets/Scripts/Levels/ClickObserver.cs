using Connection;
using Events;
using Player;
using Player.ActionHandlers;
using UnityEngine;

namespace Levels
{
    public class ClickObserver : MonoBehaviour
    {
        [SerializeField] private ColorConnectionManager colorConnectionManager;
        private ClickHandler _clickHandler;

        private void Start()
        {
            _clickHandler = ClickHandler.Instance;

            _clickHandler.PointerDownEvent += OnPointerDown;
            _clickHandler.PointerUpEvent += OnPointerUp;
            _clickHandler.DragStartEvent += OnStartDrag;
        }

        private void OnDestroy()
        {
            _clickHandler.PointerDownEvent -= OnPointerDown;
            _clickHandler.PointerUpEvent -= OnPointerUp;
            _clickHandler.DragStartEvent -= OnStartDrag;
        }

        private void OnPointerDown(Vector3 position)
        {
            colorConnectionManager.TryGetColorNodeInPosition(position, out var node);

            if (node != null)
                EventsController.Fire(new EventModels.Game.NodeTapped());
        }

        private void OnPointerUp(Vector3 position)
        {
            EventsController.Fire(new EventModels.Game.PlayerFingerRemoved());
        }

        private void OnStartDrag(Vector3 pointerPosition)
        {
            if (PlayerController.PlayerState != PlayerState.Connecting)
            {
                EventsController.Fire(new EventModels.Game.PlayerFingerPlaced());
            }
        }
    }
}