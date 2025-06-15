using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Character target;
    public float battleDistance;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Character>().maxHitPoints = 300;
        GetComponent<Character>().currentHitPoints = GetComponent<Character>().maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.battle && Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position) <= battleDistance)
        {
            var list = GameManager.instance.initiativeOrder;
            if (!list.Contains(target))
            {
                list.Add(target);
            }

            list.Add(gameObject.GetComponent<Character>());

            GameManager.instance.BattleStarts(list);
        }

        transform.LookAt(target.transform.position);
    }

    public void TakeMove()
    {
        //print(GetComponent<Character>().characterName);
        GetComponent<Animator>().SetBool("isCasting", true);
        target.currentHitPoints -= 15;
        GetComponent<Animator>().SetBool("isCasting", false);
        
        GameManager.instance.currentInitiativeOrder++;
    }
}
