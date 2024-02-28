using System;
using Events;
using Player;
using UnityEngine;
using Utils.Singleton;

namespace Camera
{
    public class CameraMovement : DontDestroyMonoBehaviourSingleton<CameraMovement>
    {
        [SerializeField] private float moveSpeed;


        private void Update()
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;
            
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), .0f);
            CameraHolder.Instance.MainCamera.transform.Translate(movement * (moveSpeed * Time.deltaTime));
        }
    }
}