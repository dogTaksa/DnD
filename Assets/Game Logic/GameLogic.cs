using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

//Delegates
[Serializable]
public delegate void AbilitySetup();

/// <summary>
/// As-static-defined methods which are used in every part of the game (dice rolls, etc.); main DnD logic's methods
/// </summary>
public class GameLogic : MonoBehaviour
{
    //Functions
    public static int d20()
    {
        return (int)Mathf.Floor(Random.Range(1, 21));
    }
    public static int RollDice(SeveralDices dicePrompt)   //"4d6", "5d20", "100d10"
    {
        var result = 0; 
        for (int i = 0; i < dicePrompt.count; i++)
        {
            result += (int)Mathf.Floor(Random.Range(1, (int)dicePrompt.diceType+1));
        }
        
        return result;
    }

    //Other tools
    [Serializable]
    public enum Dice
    {
        d4 = 4,
        d6 = 6,
        d8 = 8,
        d10 = 10,
        d12 = 12,
        d20 = 20,
        d100 = 100,
    }

    //Not only character's so invented here

    /// <summary>
    /// 'Tiny' is not compared to the ft. count (2.5 ft)
    /// </summary>
    [Serializable]
    public enum Size
    {
        Tiny,
        Small = 5,
        Medium = 5,
        Large = 10,
        Huge = 15,
        Gargantuan = 20,
    }
}

//Structures

[Serializable]
public struct SeveralDices
{
    [Min(1)]public int count;
    public GameLogic.Dice diceType;

    public void StringToDices(string dicePrompt)
    {
        var data = dicePrompt.Split('d');
        count = Convert.ToInt32(data[0]);
        diceType = (GameLogic.Dice)Convert.ToInt32(data[1]);
    }

    public string DicesToString(SeveralDices dicePrompt)
    {
        return $"{dicePrompt.count}d{diceType.ToString()}";
    }
}

[Serializable]
public struct AbilitiesCastingTime
{
    public bool OneAction;
    public bool OneBonusAction;
    [Tooltip("Time in minutes")][Min(0)] public int Time;

    public AbilitiesCastingTime(bool isBonusAction = false, int timeByMinutes = 0)
    {
        if (timeByMinutes == 0)
        {
            OneAction = !isBonusAction;
        }
        else
        {
            OneAction = false;
        }
        
        OneBonusAction = isBonusAction;
        Time = timeByMinutes;
    }
}