using System;
using UnityEngine;

namespace MomoCoop
{
    public sealed class PlayerControls : MonoBehaviour
    {
        private Action _changeColor;
        private Action<Vector2> _setLookDelta;
        private Action<Vector2> _movePlayer;

        private void Update()
        {
            if (_movePlayer is not null)
            {
                _movePlayer(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            }

            if (Input.GetKeyDown(KeyCode.C) && _changeColor is not null)
            {
                _changeColor();
            }

            if (_setLookDelta is not null)
            {
                _setLookDelta(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        public void SetPlayerChangeColor(Action changeColor) => _changeColor = changeColor;
        public void SetPlayerMovement(Action<Vector2> movePlayer) => _movePlayer = movePlayer;
        public void SetPlayerLookRotation(Action<Vector2> setLookDelta) => _setLookDelta = setLookDelta;
        public void RemovePlayerMovement() => _movePlayer = null;
        public void RemoveChangeColor() => _changeColor = null;
    }
}