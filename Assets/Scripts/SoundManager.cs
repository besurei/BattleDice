using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    static bool bBGMChanged = false;

    public enum SE_TYPE
    {
        GAME_START = 0,
        ATACK,
        DAMAGED,
        WIN,
        LOSE,
        SELECTED_NUM,
        MISS,
        DANGER,
        POWER_UP,
        HEAL,

    };

    SE_TYPE seType;

    static AudioSource audioSource;
    AudioClip audioClip;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = this.GetComponent<AudioSource>();
    }

    public static void Play( SE_TYPE in_se)
    {
        switch (in_se)
        {
            case SE_TYPE.GAME_START:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/GameStart");
                break;

            case SE_TYPE.ATACK:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/Atack");
                break;

            case SE_TYPE.DAMAGED:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/Dameged");
                break;

            case SE_TYPE.WIN:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/GameStart");
                break;

            case SE_TYPE.LOSE:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/GameStart");
                break;

            case SE_TYPE.SELECTED_NUM:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/SelectedNum");
                break;
            
            case SE_TYPE.MISS:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/Miss");
                break;

            case SE_TYPE.DANGER:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/Danger");
                break;

            case SE_TYPE.POWER_UP:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/PowerUp");
                break;

            case SE_TYPE.HEAL:
                audioSource.clip = Resources.Load<AudioClip>("Sounds/Heal");
                break;
        }

        if(audioSource.clip != null)
            audioSource.Play();

    }

    public static void ChangeBGM()
    {
        if (!bBGMChanged)
        {
            GameObject bgm = GameObject.Find("Music");
            bgm.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/Pinch");
            bgm.GetComponent<AudioSource>().pitch = 1.0f;
            bgm.GetComponent<AudioSource>().Play();
            bBGMChanged = true;
        }
    }

    public static void SetResultBGM(bool in_bWin)
    {
        if (in_bWin){
            GameObject bgm = GameObject.Find("Music");
            bgm.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/Win");
            bgm.GetComponent<AudioSource>().Play();
        }
        else{
            GameObject bgm = GameObject.Find("Music");
            bgm.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/Lose");
            bgm.GetComponent<AudioSource>().Play();
        }
    }
	
}
