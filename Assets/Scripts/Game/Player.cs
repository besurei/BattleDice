using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField]
    protected GameObject prefDice;
    [SerializeField]
    protected Text diceText;
    [SerializeField]
    protected Text hpNumText;
    [SerializeField]
    protected int hp = 0;

    [SerializeField]
    protected int[] atackNum = new int[3];
    [SerializeField]
    protected int[] healNum = new int[2];

    [SerializeField]
    protected GameObject battleManager;

    protected Animator anim;

    private int atackUp = 0;

    protected void OnCallChangeFace()
    {
    }

    protected void Awake() {

        // HP表示初期化
        hpNumText.text = hp.ToString();

        anim = GetComponent<Animator>();

    }

    // ダイス生成
    public void CreateDice()
    {

        GameObject dice = Instantiate(prefDice, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0));
        dice.GetComponent<Rigidbody>().AddForce(0, Random.Range(160, 200), 130);
        dice.GetComponent<Rigidbody>().maxAngularVelocity = 60;
        dice.GetComponent<Rigidbody>().AddTorque(Random.Range(50, 60), Random.Range(50, 60), Random.Range(50, 60), ForceMode.Force);
        StartCoroutine(GetDiceNum(dice));
    }

    // ダイス番号取得
    protected IEnumerator GetDiceNum( GameObject in_dice )
    {
        yield return new WaitForSeconds(3.5f);

        Die_d10 dice = in_dice.GetComponent<Die_d10>();
        int diceNum = dice.GetValueNum();

        if(diceNum >= 10)
        {
            diceNum = 0;
        }

        diceText.text = (diceNum).ToString();
        StartCoroutine( battleManager.GetComponent<BattleManager>().AtackFase(diceNum) );
    }

    // 攻撃する
    public void Atack( BattleManager.ATACK_TYPE in_atackType )
    {
        // 攻撃パターンによりアニメーション変化
        switch (in_atackType)
        {
            case BattleManager.ATACK_TYPE.ATACK02:
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK03:
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK04:
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK06:
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK08:
                anim.SetTrigger("punch");
                break;
            default:
                break;
        }
    }

    // ダメージを受ける
    public void Dameged(BattleManager.ATACK_TYPE in_atackType)
    {
        int rand = Random.Range(0, 99);

        int multiNum = 1;
        //if(rand <= 10)
        //{
        //    multiNum = 2;
        //}

        // 相手側の攻撃パターンによりダメージを受ける
        switch (in_atackType)
        {
            case BattleManager.ATACK_TYPE.ATACK02:
                hp -= atackNum[0] * multiNum + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(atackNum[0] * multiNum + atackUp + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK03:
                hp -= atackNum[1] * multiNum + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(atackNum[1] * multiNum + atackUp + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK04:
                hp -= atackNum[2] * multiNum + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(atackNum[2] * multiNum + atackUp + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK06:
                hp -= atackNum[3] * multiNum + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(atackNum[3] * multiNum + atackUp + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK08:
                hp -= atackNum[4] * multiNum + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(atackNum[4] * multiNum + atackUp + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            default:
                break;
        }
        if (hp <= 0) {
            hp = 0;
            StartCoroutine(battleManager.GetComponent<BattleManager>().LoadResult());
        }
        hpNumText.text = hp.ToString();
    }

    // 回復する
    public void Heal(BattleManager.ATACK_TYPE in_atackType)
    {
        // 相手側の攻撃パターンによりダメージを受ける
        switch (in_atackType)
        {
            case BattleManager.ATACK_TYPE.HEAL02:
                hp += healNum[0];
                break;
            case BattleManager.ATACK_TYPE.HEAL05:
                hp += healNum[1];
                break;
            default:
                break;
        }

        if (hp > 20)
            hp = 20;
        hpNumText.text = hp.ToString();

    }

    public void AtackUp()
    {
        atackUp += 2;
    }
}
