using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectspawnedOnScene : MonoBehaviour
{
    [SerializeField]
    private int _idObject;
    [SerializeField]
    private TypeOfCategoryObjects _objectCategory;

    [SerializeField]
    private ColliderTrigger _childObjectToCollide;



    public TypeOfCategoryObjects ObjectCategory { get => _objectCategory; set => _objectCategory = value; }
    public int IdObject { get => _idObject; set => _idObject = value; }

    public void SetIdObject(int id) 
    {
        _idObject = id;
        _childObjectToCollide.ActionWhenTrigger = ActiveAndUnactiveObject;
        _childObjectToCollide.IdParent = _idObject;
    }
    public void ActiveObjectToSpawn(TypeOfCategoryObjects categoryObjectType)
    {
        _objectCategory = categoryObjectType;

        switch (categoryObjectType)
        {
            case TypeOfCategoryObjects.GROUND:
                Debug.Log("Es Ground");
                break;
            case TypeOfCategoryObjects.OBSTACLE:
                transform.Find("Obsta").gameObject.SetActive(true);
                break;
            case TypeOfCategoryObjects.TELEPORTER:
                transform.Find("Portal").gameObject.SetActive(true);
                
                break;
        }
    }

    

    public void ActiveAndUnactiveObject(PortalTriggerState portalState, GameObject playerObject) 
    {
        switch (portalState) 
        {
            case PortalTriggerState.IN_PORTAL:
                transform.Find("Portal").gameObject.SetActive(false);
                transform.Find("Obsta").gameObject.SetActive(true);

                SceneGeneralController.Instance.ObjectValues.AmmountOfCategories[_objectCategory]--;

                DelegateUtils.OnShowTeleportingPanel.Invoke(true);

                SceneGeneralController.Instance.CheckSceneToEndLevel(playerObject, _idObject);

                _objectCategory = TypeOfCategoryObjects.OBSTACLE;
                break;
            case PortalTriggerState.OUT_PORTAL:
                transform.Find("Portal").gameObject.SetActive(false);

                SceneGeneralController.Instance.ObjectValues.AmmountOfCategories[_objectCategory]--;

                _objectCategory = TypeOfCategoryObjects.GROUND;
                break;
        }

        DelegateUtils.OnChangeProgressBarVBalue.Invoke(false);
        

        Debug.Log("La categoria " + _objectCategory + " tiene la cantidad de " + SceneGeneralController.Instance.ObjectValues.AmmountOfCategories[_objectCategory]);
        
    }


    public void ChangePortalState(PortalTriggerState portalState) 
    {
        transform.Find("Portal").GetComponent<ColliderTrigger>().PortalState = portalState;
    }
}
