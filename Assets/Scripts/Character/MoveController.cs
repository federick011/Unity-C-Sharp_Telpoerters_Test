using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]
public class MoveController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controllcharacter;

    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private float _gravityValue = -9.81f;

    [SerializeField]
    private float _CharacterSpeed = 20f;

    [SerializeField]
    private float _rotSpeed = 90f;

    private bool _mayJump = true;

    private void Awake()
    {
        Initcomponents();
    }

    private void Initcomponents() 
    {
        _controllcharacter = GetComponent<CharacterController>();
    }

    private void Update()
    {

        CharacterStateMovement();

    }

    private void CharacterStateMovement()
    {
        switch (SceneGeneralController.Instance.PlayerMoveState)
        {
            case MovementStatePlayer.MAYMOVE:
                MoveCharacter();
                break;
            case MovementStatePlayer.CANNOTMOVE:
                break;
        }
    }

    private void MoveCharacter() 
    {
        
        RotateObject();

        _groundedPlayer = _controllcharacter.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = transform.right * move.x + transform.forward * move.z;

        _controllcharacter.Move(move * _CharacterSpeed * Time.deltaTime);


        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controllcharacter.Move(_playerVelocity * Time.deltaTime);
    }

    private void RotateObject() 
    {
        float rotateInput = Input.GetAxis("Mouse X") * Time.deltaTime * _rotSpeed;
        transform.Rotate(new Vector3(0f, rotateInput, 0f));
    }
}
