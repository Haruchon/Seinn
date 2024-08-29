using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPointer : MonoBehaviour
{
    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    private GameObject pointerImage;
    private bool isQuestActive;
    private bool isShowing;
    
    private void Start()
    {
        pointerRectTransform = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        pointerImage = transform.GetChild(0).gameObject;
        GameEvents.current.onQuestAssign += FollowQuestPoint;
        GameEvents.current.onQuestCompleted += QuestCompleted;
        isQuestActive = false;
        isShowing = false;
        HidePointer();
    }

    private void Update()
    {
        if (isQuestActive)
        {
            float borderSize = 0f;
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= borderSize ||
                               targetPositionScreenPoint.x >= Screen.width - borderSize ||
                               targetPositionScreenPoint.y <= borderSize ||
                               targetPositionScreenPoint.y >= Screen.height - borderSize;

            if(isOffScreen)
            {
                RotateArrow();
                ShowPointer();
            }
            else
            {
                HidePointer();
            }
        }
    }

    private void RotateArrow()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = CodeMonkey.Utils.UtilsClass.GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void ShowPointer()
    {
        if (!isShowing)LeanTween.scaleX(this.gameObject, 1f, 0.5f);
        isShowing = true;
    }

    private void HidePointer()
    {
        if(isShowing)LeanTween.scaleX(this.gameObject, 0f, 0.5f);
        isShowing = false;
    }

    private void FollowQuestPoint(Transform targetPosition)
    {
        this.targetPosition = targetPosition.position;
        isQuestActive = true;
    }

    private void QuestCompleted(Quest quest)
    {
        isQuestActive = false;
    }
}
