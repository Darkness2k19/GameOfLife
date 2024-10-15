using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
struct CameraBounds
{
    public Vector2 horizontal;
    public Vector2 vertical;
    public Vector2 zoom;
};

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float m_movementSpeed;

    [SerializeField]
    [Range(0.05f, 1f)]
    private float m_movementResponse;

    [SerializeField]
    private float m_mouseWheelSensitivity;

    [SerializeField]
    private CameraBounds m_inputBounds;

    private Transform m_transform;
    private Vector3 m_movement = Vector3.zero;
    private Vector3 m_movementInput = Vector3.zero;

    private void processInput()
    {
        m_movementInput = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            m_movementInput.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_movementInput.y += -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_movementInput.x += -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_movementInput.x += 1;
        }

        m_movementInput.z = Input.mouseScrollDelta.y * m_mouseWheelSensitivity;
        checkMovementBounds();

        Vector3 delta = m_movementResponse * Time.unscaledDeltaTime * (Vector2)(m_movementInput - m_movement);
        if (m_movementInput.z != 0)
        {
            delta.z = m_movementInput.z * 0.1f;
        }
        else
        {
            delta.z = 1.5f * m_movementResponse * Time.unscaledDeltaTime * (m_movementInput - m_movement).z;
        }
        m_movement += delta * 5;
        m_transform.position += m_movementSpeed * Time.unscaledDeltaTime * m_movement;
    }

    private void setMovementInBounds(float position, Vector2 positionBounds, ref float movementValue, Vector2 bounds)
    {
        if (position < positionBounds.x)
        {
            bounds.x = 0;
        }
        if (position > positionBounds.y)
        {
            bounds.y = 0;
        }
        movementValue = Mathf.Clamp(movementValue, bounds.x, bounds.y);
    }

    private void checkMovementBounds()
    {
        setMovementInBounds(m_transform.position.x, m_inputBounds.horizontal, ref m_movementInput.x, new(-1, 1));
        setMovementInBounds(m_transform.position.y, m_inputBounds.vertical, ref m_movementInput.y, new(-1, 1));
        setMovementInBounds(m_transform.position.z, m_inputBounds.zoom, ref m_movementInput.z, new(-20, 20));
    }

    void Start()
    {
        m_transform = GetComponent<Transform>();
    }

    void Update()
    {
        processInput();
    }
}
