using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField]
    protected string name;

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

    [SerializeField]
    protected Slider slider;

    [SerializeField]
    protected Image barImage;

    [SerializeField]
    protected ParticleSystem healParticle;
    [SerializeField]
    protected ParticleSystem atackUpParticle;

    protected Animator anim;

    private int atackUp = 0;
    private int maxHP;
    private bool bDanger = false;

    protected void OnCallChangeFace()
    {
    }

    protected void Awake() {

        // HP表示初期化
        hpNumText.text = hp.ToString();
        maxHP = hp;

        healParticle.Stop();
        atackUpParticle.Stop();
        barImage.color = Color.green;

        anim = GetComponent<Animator>();

    }

    // ダイス生成
    public void CreateDice()
    {

        GameObject dice = Instantiate(prefDice, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0));
        dice.GetComponent<Rigidbody>().AddForce(0, Random.Range(160, 170), 120);
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

        SoundManager.Play(SoundManager.SE_TYPE.SELECTED_NUM);
        diceText.GetComponent<Animator>().SetTrigger("SelectedNum");
        diceText.text = (diceNum).ToString();
        StartCoroutine( battleManager.GetComponent<BattleManager>().AtackFase(diceNum) );
    }

    public void Miss()
    {
        anim.SetTrigger("Miss");
    }

    // 攻撃する
    public void Atack( BattleManager.ATACK_TYPE in_atackType )
    {
        // 攻撃パターンによりアニメーション変化
        switch (in_atackType)
        {
            case BattleManager.ATACK_TYPE.ATACK02:
                battleManager.GetComponent<BattleManager>().SetMessage(name+"の\nめちゃよわパンチ　こうげき！");
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK03:
                battleManager.GetComponent<BattleManager>().SetMessage(name + "の\nよわいパンチ　こうげき！");
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK04:
                battleManager.GetComponent<BattleManager>().SetMessage(name + "の\nパンチ　こうげき！");
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK06:
                battleManager.GetComponent<BattleManager>().SetMessage(name + "の\nつよいパンチ　こうげき！");
                anim.SetTrigger("punch");
                break;
            case BattleManager.ATACK_TYPE.ATACK08:
                battleManager.GetComponent<BattleManager>().SetMessage(name + "の\nめちゃつよパンチ　こうげき！");
                anim.SetTrigger("punch");
                break;
            default:
                break;
        }

        SoundManager.Play(SoundManager.SE_TYPE.ATACK);

    }

    // ダメージを受ける
    public int Dameged(BattleManager.ATACK_TYPE in_atackType)
    {
        // 相手側の攻撃パターンによりダメージを受ける
        switch (in_atackType)
        {
            case BattleManager.ATACK_TYPE.ATACK02:
                hp -= atackNum[0]+ atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(name + "に\n" + (atackNum[0] + atackUp) + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK03:
                hp -= atackNum[1] + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(name + "に\n" + (atackNum[1] + atackUp) + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK04:
                hp -= atackNum[2] + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(name + "に\n" + (atackNum[2] + atackUp) + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK06:
                hp -= atackNum[3] + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(name + "に\n" + (atackNum[3] + atackUp) + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            case BattleManager.ATACK_TYPE.ATACK08:
                hp -= atackNum[4] + atackUp;
                battleManager.GetComponent<BattleManager>().SetMessage(name + "に\n" + (atackNum[4] + atackUp) + "ダメージ！");
                anim.SetTrigger("dameged");
                break;
            default:
                break;
        }
        slider.GetComponent<Slider>().value = hp;
        if (hp <= 10 & hp > 5)
            barImage.color = Color.yellow;
        else if (hp <= 5 && hp > 0)
        {
            if (!bDanger)
            {
                bDanger = true;
                SoundManager.ChangeBGM();
            }
            barImage.color = Color.red;
        }
        else if (hp <= 0)
        {
            barImage.color = Color.clear;
            hp = 0;
            hpNumText.text = hp.ToString();
            anim.SetTrigger("Lose");
            battleManager.GetComponent<BattleManager>().SetClear(true);
            StartCoroutine(battleManager.GetComponent<BattleManager>().Result(this.gameObject.name));
            return 0;
        }
        hpNumText.text = hp.ToString();

        SoundManager.Play(SoundManager.SE_TYPE.DAMAGED);
        return 1;
    }

    // 回復する
    public void Heal(BattleManager.ATACK_TYPE in_atackType)
    {
        SoundManager.Play(SoundManager.SE_TYPE.HEAL);
        healParticle.Play();

        // 相手側の攻撃パターンによりダメージを受ける
        switch (in_atackType)
        {
            case BattleManager.ATACK_TYPE.HEAL02:
                battleManager.GetComponent<BattleManager>().SetMessage(name + "の\nHPが" + healNum[0] + "かいふくした！");
                hp += healNum[0];
                break;
            case BattleManager.ATACK_TYPE.HEAL05:
                battleManager.GetComponent<BattleManager>().SetMessage(name + "の\nHPが" + healNum[1] + "かいふくした！");
                hp += healNum[1];
                break;
            default:
                break;
        }

        if (hp > 20)
            hp = 20;
        if(bDanger && hp > 5)
        {
            bDanger = false;
        }
        hpNumText.text = hp.ToString();
        slider.GetComponent<Slider>().value = hp;

        if (hp > 5 && hp <= 10)
            ChangeBarColor(Color.yellow);
        else if (hp > 10)
            ChangeBarColor(Color.green);

    }

    public void AtackUp()
    {
        SoundManager.Play(SoundManager.SE_TYPE.POWER_UP);
        atackUp += 2;
        if( this.gameObject.name == "Player" )
            battleManager.GetComponent<BattleManager>().SetMessage("ゆうこ" + "の\nこうげきりょくが" + healNum[0]+"あがった！");
        else if ( this.gameObject.name == "Enemy" )
            battleManager.GetComponent<BattleManager>().SetMessage("こはく" + "の\nこうげきりょくが" + healNum[0] + "あがった！");
    }

    public void PlayHealParticle()
    {
        atackUpParticle.Play();
    }

    void ChangeBarColor( Color in_color )
    {
        barImage.color = in_color;
    }
}
