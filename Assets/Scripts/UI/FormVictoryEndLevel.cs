using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormVictoryEndLevel : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelVictory;

    [SerializeField]
    private Button _playButton;

    // Start is called before the first frame update
    private void Awake()
    {
        _panelVictory = transform.Find("FormVictoryLevel").gameObject;

        _panelVictory.transform.Find("Play").GetComponent<Button>().onClick.AddListener(() => ButtonPlayActions());
    }

    private void ShowHideVictoryUi(bool statePanel) 
    {
        _panelVictory.SetActive(statePanel);

        if (statePanel) 
        {
            SceneGeneralController.Instance.ChangePlayerMovementState(MovementStatePlayer.CANNOTMOVE);
        }
    }

    private void ButtonPlayActions() 
    {
        SceneGeneralController.Instance.ResetScene();

        ShowHideVictoryUi(false);
    }

    private void OnEnable()
    {
        DelegateUtils.OnShowVictoryPanel += ShowHideVictoryUi;
    }

    private void OnDisable()
    {
        DelegateUtils.OnShowVictoryPanel -= ShowHideVictoryUi;
    }
}
