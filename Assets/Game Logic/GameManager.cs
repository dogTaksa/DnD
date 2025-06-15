using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isMove;
    public bool battle;
    public List<Character> initiativeOrder = new List<Character>();
    public int currentInitiativeOrder;

    public List<Character> test;
    public int enemiesDestroyed;

    public GameObject congratulations;

    // Start is called before the first frame update
    void Awake()
    {
        isMove = false;
        battle = false;

        instance = this;

        Magic.targets = test;

        //BattleStarts(test);
    }

    // Update is called once per frame
    void Update()
    {
        if (battle)
        {
            Cursor.lockState = CursorLockMode.None;


            if (!isMove)
            {
                var currentMove = initiativeOrder[currentInitiativeOrder % initiativeOrder.Count];

                if (!currentMove.CompareTag("Player"))
                {
                    isMove = true;
                    currentMove.gameObject.GetComponent<Enemy>().TakeMove();
                }
            }
        }

        Cursor.visible = battle;

        if (enemiesDestroyed == 20)
        {
            congratulations.SetActive(true);
        }
    }

    public void BattleStarts(List<Character> members)
    {
        foreach (Character c in members)
        {
            if (c.GetComponent<ThirdPersonController>() != null)
            {
                c.GetComponent<ThirdPersonController>().enabled = false;
            }
            
            c.RollForInitiative();
            initiativeOrder.Add(c);
        }

        initiativeOrder.Sort((a, b) => b.initiativeRoll.CompareTo(a.initiativeRoll));

        //foreach (Character c in initiativeOrder)
        //{
        //    print($"{c.characterName}: {c.initiativeRoll}");
        //}
        //foreach (var item in initiativeOrder)
        //{
        //    print(item);
        //}

        battle = true;
        currentInitiativeOrder = 0;
    }

    public IEnumerator Move()
    {
        isMove = false;
        yield return new WaitForSeconds(5); //TODO: Other conditions
    }
}
