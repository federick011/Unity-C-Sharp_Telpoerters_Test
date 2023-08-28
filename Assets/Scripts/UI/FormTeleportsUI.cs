using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormTeleportsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _teleportingMessage;

    [SerializeField]
    private Slider _progressBar;

    private IEnumerator _coroShowMessage;

    private float _maxValueBar;
    // Start is called before the first frame update
    void Start()
    {
        _teleportingMessage = transform.Find("Teleporting").gameObject;
        _progressBar = transform.Find("Progress").GetComponent<Slider>();
    }

    private void SetProgressBar(bool setMaxValue = false)
    {
        if (setMaxValue) 
        {
            _maxValueBar = SceneGeneralController.Instance.ObjectValues.AmmountOfCategories[TypeOfCategoryObjects.TELEPORTER];
            _progressBar = transform.Find("Progress").GetComponent<Slider>();
        }

        _progressBar.value = SceneGeneralController.Instance.ObjectValues.AmmountOfCategories[TypeOfCategoryObjects.TELEPORTER] / _maxValueBar;
    }

    private void ShowHideTeleportingMessage(bool stateMessage) 
    {
        _teleportingMessage.SetActive(stateMessage);

        _coroShowMessage = WaitToHideMessage(0.8f);
        StartCoroutine(_coroShowMessage);
    }

    private IEnumerator WaitToHideMessage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _teleportingMessage.SetActive(false);
        StopCoroutine(_coroShowMessage);
    }

    private void OnEnable()
    {
        DelegateUtils.OnShowTeleportingPanel += ShowHideTeleportingMessage;
        DelegateUtils.OnChangeProgressBarVBalue += SetProgressBar;
    }

    private void OnDisable()
    {
        DelegateUtils.OnShowTeleportingPanel -= ShowHideTeleportingMessage;
        DelegateUtils.OnChangeProgressBarVBalue -= SetProgressBar;
    }
}
