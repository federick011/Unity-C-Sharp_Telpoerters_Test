using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerView : MonoBehaviour
{
    [SerializeField]
    private Vector2 _rotateSpeed = new Vector2(90f, 90f);

    [SerializeField]
    private AxisToMove _typeOfAxis;

    private float _rotationX;

    // Update is called once per frame
    void Update()
    {
        CharacterStateMovement();
    }

    private void CharacterStateMovement()
    {
        switch (SceneGeneralController.Instance.PlayerMoveState)
        {
            case MovementStatePlayer.MAYMOVE:
                CameraViewMovement();
                break;
            case MovementStatePlayer.CANNOTMOVE:
                break;
        }
    }

    private void CameraViewMovement() 
    {
        float inputMouse = GetMouseAxis(_typeOfAxis);

        ClampValuesTorotate((inputMouse * _rotateSpeed.y * Time.deltaTime), out Vector3 posMouse);

        transform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);

        transform.Rotate(posMouse);
    }

    private void ClampValuesTorotate(float mouseValues, out Vector3 posMouse) 
    {
        _rotationX -= mouseValues;
        _rotationX = Mathf.Clamp(_rotationX, -40f, 40f);

        posMouse = new Vector3(_rotationX, 0f, 0f);
    }


    private float GetMouseAxis(AxisToMove typeOfAxis) 
    {
        Vector2 inputMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        float inputAxis = 0f;

        switch (typeOfAxis) 
        {
            case AxisToMove.X_AXIS:
                inputAxis = inputMouse.x;
                break;
            case AxisToMove.Y_AXIS:
                inputAxis = inputMouse.y;
                break;
            default:
                inputAxis = inputMouse.x;
                break;
                  
        }

        return inputAxis;
    }
}
