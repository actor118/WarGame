using UnityEngine;
using System.Collections;

namespace WorldMapStrategyKit {
    public class TiltedCameraMove : MonoBehaviour {

        public float cameraSensitivity = 150;
        public float climbSpeed = 20;
        public float normalMoveSpeed = 20;
        public float slowMoveFactor = 0.25f;
        public float fastMoveFactor = 3;

        void Update() {
            Vector2 mousePos = Input.mousePosition;
            if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
                return;

            transform.localRotation = Quaternion.Euler(45, 0, 0);

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                transform.position += Vector3.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += Vector3.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            } else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
                transform.position += Vector3.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += Vector3.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            } else {
                transform.position += Vector3.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += Vector3.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.Q)) {
                transform.position -= transform.forward * climbSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E)) {
                transform.position += transform.forward * climbSpeed * Time.deltaTime;
            }

        }

    }
}