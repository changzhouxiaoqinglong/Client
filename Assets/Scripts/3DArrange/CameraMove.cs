using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;


namespace NWH.VehiclePhysics
{
    /// <summary>
    /// Camera that can be dragged with the mouse.
    /// </summary>
    public class CameraMove : VehicleCamera
    {
        public float mouseX = 0.0f;
        public float mouseY = 0.0f;
        [Range(0, 15)]
        public float horizontalMouseSensitivity = 0.15f;
        [Range(0, 15)]
        public float verticalMouseSensitivity = 0.15f;
        [Range(-90, 90)]
        public float verticalMinAngle = -40.0f;
        [Range(-90, 90)]
        public float verticalMaxAngle = 80.0f;
        [Range(0, 1)]
        public float moveSpeed = 0.1f;
        [Range(0, 10)]
        public float shiftScale = 2f;

        [Range(0, 10000f)]
        public float maxHeight;
        [Range(-1000f, 1000f)]
        public float minHeight;
        [Range(0, 100)]
        public float scrolSpeed = 50.0f;

        private Quaternion rotation;
        private float moveScale = 1;
        void Start()
        {

        }
        // Running in fixed update due to rigidbody interpolation being none needed for WheelControllers
        void Update()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                mouseX += Mouse.current.delta.ReadValue().x * horizontalMouseSensitivity ;
                mouseY -= Mouse.current.delta.ReadValue().y * verticalMouseSensitivity ;

            }
            if (Keyboard.current.leftShiftKey.isPressed)
            {
                moveScale = shiftScale;
            }
            else
            {
                moveScale = 1;
            }
            float scroll = Mouse.current.scroll.ReadValue().y;
            Vector3 newPos = transform.position + transform.forward * scroll * scrolSpeed * moveScale;
            newPos.y = Mathf.Clamp(newPos.y, minHeight, maxHeight);
            transform.position = newPos;
            if (Keyboard.current.wKey.isPressed)
            {
                gameObject.transform.Translate(Vector3.forward * moveSpeed * moveScale, Space.Self);
            }
            if (Keyboard.current.sKey.isPressed)
            {
                gameObject.transform.Translate(Vector3.back * moveSpeed * moveScale, Space.Self);
            }
            if (Keyboard.current.aKey.isPressed)
            {
                gameObject.transform.Translate(Vector3.left * moveSpeed * moveScale, Space.Self);
            }
            if (Keyboard.current.dKey.isPressed)
            {
                gameObject.transform.Translate(Vector3.right * moveSpeed * moveScale, Space.Self);
            }
            if (Keyboard.current.qKey.isPressed)
            {
                gameObject.transform.Translate(Vector3.up * moveSpeed * moveScale, Space.Self);
            }
            if (Keyboard.current.eKey.isPressed)
            {
                gameObject.transform.Translate(Vector3.down * moveSpeed * moveScale, Space.Self);
            }
            mouseY = ClampAngle(mouseY, verticalMinAngle, verticalMaxAngle);
            rotation = Quaternion.Euler(mouseY, mouseX, 0);
            gameObject.transform.rotation = rotation;
        }
        public float ClampAngle(float angle, float min, float max)
        {
            while (angle < -360 || angle > 360)
            {
                if (angle < -360)
                    angle += 360;
                if (angle > 360)
                    angle -= 360;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}