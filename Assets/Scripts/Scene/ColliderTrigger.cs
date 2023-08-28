using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    [SerializeField]
    private Action<PortalTriggerState, GameObject> _actionWhenTrigger;

    [SerializeField]
    private int _idParent;

    [SerializeField]
    private PortalTriggerState _portalState = PortalTriggerState.IN_PORTAL;

    

    public int IdParent { get => _idParent; set => _idParent = value; }
    public Action<PortalTriggerState, GameObject> ActionWhenTrigger { get => _actionWhenTrigger; set => _actionWhenTrigger = value; }
    public PortalTriggerState PortalState { get => _portalState; set => _portalState = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("entro en el portal");
            ActionTriggerEnter(other.gameObject);
        }
    }

    public void ActionTriggerEnter(GameObject other) 
    {
        _actionWhenTrigger(_portalState, other);

        SceneGeneralController.Instance.ChangePlayerMovementState(MovementStatePlayer.CANNOTMOVE, true);

    }

}
