using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] private float initialRotation = 45f;
    [SerializeField] private float speed = 500f;
    private float currentRotation;
    private float direction = 1f;
    private bool isDown = true;
    private Coroutine rutina = null;
    public float movementSpeed = 0f;
    public bool isMoving = false;
    public float startZ = 0f;
    public float trajectoryMin;
    public float trajectoryMax;
    public float stepZ = 1f;

    private void Start()
    {
        initialRotation = isDown ? 90f : 0f;
        transform.eulerAngles = new Vector3(0, 0, initialRotation);
        currentRotation = initialRotation;
        startZ = transform.position.z;
        Debug.Log(startZ);
    }

    private void Update()
    {
        if (!isDown && isMoving)
        {
            TargetMovement();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (rutina != null) StopCoroutine(rutina);
            if (isDown)
            {
                rutina = StartCoroutine(TargetUp());
            }
            else
            {
                rutina = StartCoroutine(TargetDown());
            }

        }
    }
    public void TargetMovement()
    {
        if(transform.position.z > trajectoryMax || transform.position.z < trajectoryMin)
        {
            stepZ *= -1;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + stepZ * movementSpeed);
    }

    public void Hit()
    {
        if (rutina != null) StopCoroutine(rutina);
        rutina = StartCoroutine(TargetDown());
    }

    private IEnumerator TargetDown()
    {
        isDown = true;
        if (currentRotation == 90f) yield break;
        while(currentRotation < 90f)
        {
            RotateTarget(1f);
            yield return null;
        }
    }

    private IEnumerator TargetUp()
    {
        isDown = false;
        if (currentRotation == 0f) yield break;
        while(currentRotation > 0f)
        {
            RotateTarget(-1f);
            yield return null;
        }
    }

    private void RotateTarget(float direction)
    {
        currentRotation += speed * Time.deltaTime * direction;
        currentRotation = Mathf.Clamp(currentRotation, 0, 90);
        transform.eulerAngles = new Vector3(0, 0, currentRotation);
    }
}
