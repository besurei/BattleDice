using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public void On_StartGame()
    {
        SoundManager.Play(SoundManager.SE_TYPE.SELECT);
        SimpleSceneFader.ChangeSceneWithFade("Game", 1.0f);
    }

}
