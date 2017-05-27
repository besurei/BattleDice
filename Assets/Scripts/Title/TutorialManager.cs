using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    [SerializeField]
    Image contantsParent;

    [SerializeField]
    Image tutorialImage;

    [SerializeField]
    Text tutorialText;

    [SerializeField]
    Text buttonText;

    [SerializeField]
    GameObject background;

    Animator animator;

    private int tutorialNum = 0;

    void Awake()
    {
        animator = contantsParent.gameObject.GetComponent<Animator>();
    }

    public void Active()
    {
        tutorialNum = 0;
        animator.SetTrigger("Open");
        On_SetTutorialImage();
        background.SetActive(true);
    }

	public void On_SetTutorialImage()
    {
        SoundManager.Play(SoundManager.SE_TYPE.SELECT);
        switch (tutorialNum)
        {
            case 0:
                tutorialImage.sprite = Resources.Load<Sprite>("Sprites/Tutorial00");
                tutorialText.text = "きみのターンになったら\n\nひだりクリックして\n\nおさらにむかってダイスをふろう！";
                buttonText.text = "つづき";
                break;


            case 1:
                tutorialImage.sprite = Resources.Load<Sprite>("Sprites/Tutorial01");
                tutorialText.text = "ダイスのすうじでこうどうがかわる！\n\nきみのうんがためされるぞ！";
                buttonText.text = "とじる";
                break;

            case 2:
                CloseModal();
                break;
        }

        tutorialNum++;
    }

    void CloseModal()
    {
        background.SetActive(false);
        animator.SetTrigger("Close");
    }
}
