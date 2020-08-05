using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Text winnersText;

    public void Open()
    {
        gameObject.SetActive(true);
        animator.Play("OpenPopup");

        SetDescription();
    }

    void SetDescription()
    {
        List<Turn> winners = new List<Turn>();
        int highestScore = 0;
        for(int i =0; i < Pyramid_Main.instance.Players.Count; i++)
        {
            if(highestScore == Pyramid_Main.instance.Players[i].Score)
            {
                winners.Add((Turn)i);
            }
            else if(highestScore < Pyramid_Main.instance.Players[i].Score)
            {
                winners.Clear();
                winners.Add((Turn)i);
                highestScore = Pyramid_Main.instance.Players[i].Score;
            }
        }

        winnersText.text = string.Join(",\n", winners);
    }

    public void Close()
    {
        animator.Play("ClosePopup");
    }

    public void OnCloseAnimEnd()
    {
        gameObject.SetActive(false);
    }

    public void RestartButtonClicked()
    {
        Pyramid_UIManager.instance.OnRestartClicked();
        Close();
    }
}
