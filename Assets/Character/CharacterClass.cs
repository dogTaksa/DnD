using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Character class", fileName = "CharacterClass")]
public class CharacterClass : MonoBehaviour
{
    public StatType MainStat; //Charisma, Wisdom, Strength, etc.
    public GameLogic.Dice hitDiceType;
    public Dictionary<int, AbilitySetup> levelAbilityPairs;
}
