using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneObjectsValues 
{
    Dictionary<TypeOfCategoryObjects, float> _categoryObjectsProportions = new Dictionary<TypeOfCategoryObjects, float>() 
    {
        { TypeOfCategoryObjects.GROUND, 60f},
        { TypeOfCategoryObjects.OBSTACLE, 25f},
        { TypeOfCategoryObjects.TELEPORTER, 15f},
    };

    private Dictionary<TypeOfCategoryObjects, int> _ammountOfCategories = new Dictionary<TypeOfCategoryObjects, int>() 
    {
        { TypeOfCategoryObjects.GROUND, 0},
        { TypeOfCategoryObjects.OBSTACLE, 0},
        { TypeOfCategoryObjects.TELEPORTER, 0},
    };

    public Dictionary<TypeOfCategoryObjects, int> AmmountOfCategories { get => _ammountOfCategories; set => _ammountOfCategories = value; }
    internal Dictionary<TypeOfCategoryObjects, float> CategoryObjectsProportions { get => _categoryObjectsProportions; set => _categoryObjectsProportions = value; }
}
public class SceneGeneralController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _groundTiles = new List<GameObject>();

    [Space(6)]
    [SerializeField]
    private int _sceneSizeX = 40;
    [SerializeField]
    private int _sceneSizeY = 40;

    private SceneObjectsValues _objectValues = new SceneObjectsValues();

    [Space(5)]
    [SerializeField]
    private MovementStatePlayer _playerMoveState = MovementStatePlayer.MAYMOVE;

    private IEnumerator _coToBlockPlayer;

    static SceneGeneralController _instance;

    public static SceneGeneralController Instance { get => _instance; set => _instance = value; }
    public int SceneSizeX { get => _sceneSizeX; set => _sceneSizeX = value; }
    public int SceneSizeY { get => _sceneSizeY; set => _sceneSizeY = value; }
    public SceneObjectsValues ObjectValues { get => _objectValues; set => _objectValues = value; }
    public MovementStatePlayer PlayerMoveState { get => _playerMoveState; set => _playerMoveState = value; }
    public List<GameObject> GroundTiles { get => _groundTiles; set => _groundTiles = value; }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        DelegateUtils.OnCreateGroundScene.Invoke();

    }

    public void CheckSceneToEndLevel(GameObject player, int idPortal, Action<PortalTriggerState, GameObject> actionToChangeTeleport = null)
    {
        if (_objectValues.AmmountOfCategories[TypeOfCategoryObjects.TELEPORTER] < 1)
        {
            Debug.Log("No hay mas portales");

            ChangePlayerMovementState(MovementStatePlayer.CANNOTMOVE);

            DelegateUtils.OnShowVictoryPanel.Invoke(true);
        }
        else if(_objectValues.AmmountOfCategories[TypeOfCategoryObjects.TELEPORTER] > 1) 
        {
            MovePlayerToAnotherTeleport(player, idPortal);
        }
    }

    private void MovePlayerToAnotherTeleport(GameObject player, int idPortal) 
    {
        List<GameObject> newPos = new List<GameObject>();
        foreach (GameObject item in _groundTiles)
        {
            if (item.GetComponent<ObjectspawnedOnScene>().ObjectCategory == TypeOfCategoryObjects.TELEPORTER && 
                item.GetComponent<ObjectspawnedOnScene>().IdObject != idPortal)
            {
                Debug.Log("Portal encontrado ");
                newPos.Add(item);
            }

        }

        int index = UnityEngine.Random.Range(0, newPos.Count);

        newPos[index].GetComponent<ObjectspawnedOnScene>().ChangePortalState(PortalTriggerState.OUT_PORTAL);

        //Destroy(newPos[index].GetComponent(typeof(BoxCollider)));

        //newPos[index].GetComponent<ObjectspawnedOnScene>().ActiveAndUnactiveObject(PortalTriggerState.OUT_PORTAL, player);

        Vector3 newPosPlayer = newPos[index].transform.position;

        player.transform.position = new Vector3(newPosPlayer.x, player.transform.position.y + 0.1f, newPosPlayer.z);

        Debug.Log("Teleportando");
    }


    public bool CheckPlayerState() 
    {
        bool state = true;

        if (_playerMoveState != MovementStatePlayer.MAYMOVE) 
        {
            state = false;
        }

        return state;
    }

    public void ResetScene()
    {
        RemoveObjectFromScene();

        DelegateUtils.OnCreateGroundScene.Invoke();


    }

    private void RemoveObjectFromScene()
    {
        for (int i = 0; i < _groundTiles.Count; i++)
        {
            if(_groundTiles[i] != null)
                Destroy(_groundTiles[i]);
        }

        _groundTiles.Clear();
    }

    public void ChangePlayerMovementState(MovementStatePlayer playerState, bool starCoro = false) 
    {
        _playerMoveState = playerState;

        if (starCoro) 
        {
            _coToBlockPlayer = WaitToMovePlayer(1f);
            StartCoroutine(_coToBlockPlayer);
        }
            
    }

    private IEnumerator WaitToMovePlayer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ChangePlayerMovementState(MovementStatePlayer.MAYMOVE);
        StopCoroutine(_coToBlockPlayer);
    }

}
