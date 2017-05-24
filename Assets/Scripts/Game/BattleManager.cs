using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private Text message;

    [SerializeField]
    private Camera[] camera = new Camera[3];

    // ターンタイプ
    public enum TURN_TYPE{
        TURN_PLAYER = 0,
        TURN_ENEMY,
        TURN_NONE
    };
    public TURN_TYPE turnType;

    // アタックタイプ
    public enum ATACK_TYPE
    {
        ATACK08 = 0,
        MISS01,
        ATACK02,
        ATACK04,
        HEAL02,
        HEAL05,
        ATACK06,
        ATACKUP,
        ATACK03,
        MISS02,
    };
    ATACK_TYPE atackType;

    private bool standbyCamera_isRunning = false;

	void Awake () {

        StartCoroutine( StandbyFase() );
		
	}

    IEnumerator StandbyCameraMove()
    {
        standbyCamera_isRunning = true;
        yield return new WaitForSeconds(5);
        int rand = (int)Random.Range(1, 3);
        camera[rand].GetComponent<Camera>().enabled = true;
        yield return new WaitForSeconds(3);
        camera[rand].GetComponent<Camera>().enabled = false;
    }

    // スタンバイフェイズ
    IEnumerator StandbyFase()
    {
        SetActiveCamera(0);
        if (turnType == TURN_TYPE.TURN_PLAYER)
        {
            // 入力待ち
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return 0;
                if (!standbyCamera_isRunning)
                    StartCoroutine(StandbyCameraMove() );
            }

            StopCoroutine(StandbyCameraMove());
            standbyCamera_isRunning = false;

            player.GetComponent<Player>().CreateDice();
        }
        else if(turnType == TURN_TYPE.TURN_ENEMY)
        {
            yield return new WaitForSeconds(2);

            enemy.GetComponent<Enemy>().CreateDice();
        }

        yield return null;
    }

    public IEnumerator AtackFase(int in_diceNum)
    {
        atackType = (ATACK_TYPE)in_diceNum;

        Debug.Log(atackType);

        // 攻撃カメラ有効
        //SetActiveCamera(1);
        //yield return new WaitForSeconds(1);
        SetActiveCamera(4);
        yield return new WaitForSeconds(2.5f);

        if(atackType == ATACK_TYPE.MISS01 || atackType == ATACK_TYPE.MISS02)
        {
            if (turnType == TURN_TYPE.TURN_PLAYER)
            {
                message.text = "ミス！";
                SetActiveCamera(1);
                yield return new WaitForSeconds(3);
            }
            else if (turnType == TURN_TYPE.TURN_ENEMY)
            {
                message.text = "ミス！";
                SetActiveCamera(2);
                yield return new WaitForSeconds(3);
            }
        }
        // 攻撃
        else if (atackType == ATACK_TYPE.ATACK02 || atackType == ATACK_TYPE.ATACK03
            || atackType == ATACK_TYPE.ATACK04 || atackType == ATACK_TYPE.ATACK06
            || atackType == ATACK_TYPE.ATACK08)
        {
            SetActiveCamera(3);
            if (turnType == TURN_TYPE.TURN_PLAYER)
            {
                player.GetComponent<Player>().Atack(atackType);
                yield return new WaitForSeconds(1.5f);
                SetActiveCamera(2);
                enemy.GetComponent<Enemy>().Dameged(atackType);
            }
            else if( turnType == TURN_TYPE.TURN_ENEMY )
            {
                enemy.GetComponent<Enemy>().Atack(atackType);
                yield return new WaitForSeconds(1.5f);
                player.GetComponent<Player>().Dameged(atackType);
            }
        }
        // 回復
        else if( atackType == ATACK_TYPE.HEAL02 || atackType == ATACK_TYPE.HEAL05 )
        {
            if (turnType == TURN_TYPE.TURN_PLAYER)
            {
                player.GetComponent<Player>().Heal(atackType);
            }
            else if (turnType == TURN_TYPE.TURN_ENEMY)
            {
                enemy.GetComponent<Enemy>().Heal(atackType);
            }
        }
        // 攻撃アップ
        else if( atackType == ATACK_TYPE.ATACKUP)
        {
            if (turnType == TURN_TYPE.TURN_PLAYER)
            {
                enemy.GetComponent<Enemy>().AtackUp();
            }
            else if (turnType == TURN_TYPE.TURN_ENEMY)
            {
                player.GetComponent<Player>().AtackUp();
            }
        }

        if (turnType == TURN_TYPE.TURN_PLAYER)
            turnType = TURN_TYPE.TURN_ENEMY;
        else
            turnType = TURN_TYPE.TURN_PLAYER;

        Destroy(GameObject.FindGameObjectWithTag("Dice"));

        StartCoroutine( StandbyFase() );
        yield return null;
    }

    public void SetDiceNum( int in_diceNum )
    {
        atackType = (ATACK_TYPE)in_diceNum;
    }

    public IEnumerator LoadResult()
    {
        yield return new WaitForSeconds(1);

        SimpleSceneFader.ChangeSceneWithFade("Result", 1.0f);
    }

    void SetActiveCamera( int num )
    {
        for( int i=1; i<3; i++)
        {
            if (i == num)
                camera[i].GetComponent<Camera>().enabled = true;
            else
                camera[i].GetComponent<Camera>().enabled = false;
        }
    }

    public void SetMessage( string in_message)
    {
        message.text = in_message;
    }
}
