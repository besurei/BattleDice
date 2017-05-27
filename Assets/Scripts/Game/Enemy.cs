using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Player{

    void Awake()
    {
        base.Awake();
    }

    // ダイス生成
    public void CreateDice()
    {
    
        GameObject dice = Instantiate(prefDice, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z - 0.1f), new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0));
        dice.GetComponent<Rigidbody>().AddForce(0, Random.Range(160, 170), -120);
        dice.GetComponent<Rigidbody>().maxAngularVelocity = 60;
        dice.GetComponent<Rigidbody>().AddTorque(Random.Range(50, 60), Random.Range(50, 60), Random.Range(50, 60), ForceMode.Force);
        StartCoroutine(GetDiceNum(dice));
    }
}
