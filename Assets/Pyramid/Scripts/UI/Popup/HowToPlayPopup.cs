using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPopup : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public void Open()
    {
        gameObject.SetActive(true);
        animator.Play("OpenPopup");

        Pyramid_TurnManager.instance.SetPause(true);
    }

    public void Close()
    {
        animator.Play("ClosePopup");

        if(Pyramid_Main.instance.isPlayFirstTime)
            PlayerPrefs.SetInt("PlayFirstTime", 1);
    }

    public void OnCloseAnimEnd()
    {
        gameObject.SetActive(false);

        Pyramid_TurnManager.instance.SetPause(false);
    }


}
