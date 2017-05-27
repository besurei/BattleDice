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

    // スタンバイフェイズ
    IEnumerator StandbyFase()
    {
        SetActiveCamera(0);
        if (turnType == TURN_TYPE.TURN_PLAYER)
        {
            // 入力待ち
            while (!Input.GetMouseButtonDown(0) )
            {
                SetMessage("きみのターンだ！");
                yield return 0;
            }

            player.GetComponent<Player>().CreateDice();
        }
        else if(turnType == TURN_TYPE.TURN_ENEMY)
        {
            SetMessage("てきのターンだ！");
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
        SetActiveCamera(4);
        yield return new WaitForSeconds(0.7f);

        if (atackType == ATACK_TYPE.MISS01 || atackType == ATACK_TYPE.MISS02)
        {
            SoundManager.Play(SoundManager.SE_TYPE.MISS);
            if (turnType == TURN_TYPE.TURN_PLAYER)
            {
                SetMessage("ミス！");
                player.GetComponent<Player>().Miss();
                yield return new WaitForSeconds(1.0f);
            }
            else if (turnType == TURN_TYPE.TURN_ENEMY)
            {
                SetMessage("ミス！");
                enemy.GetComponent<Enemy>().Miss();
                yield return new WaitForSeconds(1.0f);
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
                SetMessage("こはくの\nパンチ　こうげき！");
                player.GetComponent<Player>().Atack(atackType);
                yield return new WaitForSeconds(1f);
                SetActiveCamera(2);
                enemy.GetComponent<Enemy>().Dameged(atackType);
            }
            else if (turnType == TURN_TYPE.TURN_ENEMY)
            {
                SetMessage("ゆうこの\nパンチ　こうげき！");
                enemy.GetComponent<Enemy>().Atack(atackType);
                yield return new WaitForSeconds(1f);
                player.GetComponent<Player>().Dameged(atackType);
            }
            yield return new WaitForSeconds(2f);
        }
        // 回復
        else if (atackType == ATACK_TYPE.HEAL02 || atackType == ATACK_TYPE.HEAL05)
        {
            if (turnType == TURN_TYPE.TURN_PLAYER)
            {
                SetMessage("こはくの\nかいふくまほう！");
                player.GetComponent<Player>().Heal(atackType);
            }
            else if (turnType == TURN_TYPE.TURN_ENEMY)
            {
                SetMessage("ゆうこの\nかいふくまほう！");
                enemy.GetComponent<Enemy>().Heal(atackType);
            }
            yield return new WaitForSeconds(2f);
        }
        // 攻撃アップ
        else if (atackType == ATACK_TYPE.ATACKUP)
        {
            if (turnType == TURN_TYPE.TURN_PLAYER)
            {
                SetMessage("こはくの\nかわいいポーズ！");
                player.GetComponent<Player>().PlayHealParticle();
                enemy.GetComponent<Enemy>().AtackUp();
            }
            else if (turnType == TURN_TYPE.TURN_ENEMY)
            {
                SetMessage("ゆうこの\nかわいいポーズ！");
                enemy.GetComponent<Enemy>().PlayHealParticle();
                player.GetComponent<Player>().AtackUp();
            }
            yield return new WaitForSeconds(2f);
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
