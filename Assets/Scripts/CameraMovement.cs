using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float lookSpeedH = 5f;
    [SerializeField] private float lookSpeedV = 5f;

    private float _yaw;
    private float _pitch;

    private void Awake()
    {
        var currentAngle = transform.eulerAngles;
        _pitch = currentAngle.x;
        _yaw = currentAngle.y;
    }

    private void Update()
    {
        ChangeLookDirectionIfNeeded();
    }

    private void ChangeLookDirectionIfNeeded()
    {
        _yaw += lookSpeedH * Input.GetAxis("Mouse X");
        _pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
    }
}
