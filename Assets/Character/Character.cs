using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Character Properties

    [Header("Main Properties")]
    public string characterName;
    public CharacterClass characterClass; //TODO: data's type 'CharacterClass' 
    public string characterSubclass; //should be included by switch-case to 'CharacterClass' 
    public CreatureType creatureType;
    [Space]
    [Range(1, 30)] public int level;
    [Min(0)] public int XP;
    [Min(0)] public int armorClass;

    [Header("Hit Points")]
    [Min(0)] public int maxHitPoints;
    [Min(0)] public int currentHitPoints;
    [Min(0)] public int temporaryHitPoints;

    [Header("Hit Dices")]
    [Tooltip("{count: diceType}")] public SeveralDices maxHitDices;
    [Tooltip("{count: diceType}")] public SeveralDices currentHitDices;

    [Header("Other")]
    public int proficiencyBonus;
    public int passivePerception; //NOT INCLUDE PROFICIENCY IN SKILL 'PERCEPTION'
    public int initiativeModifier;
    public int initiativeRoll;
    public Speed speed;
    public GameLogic.Size size;
    public int mainStatMod;
    public Character player;
    public GameObject gameOverScreen;
    public GameObject canvas;
    public CameraController cameraController;

    [Space]
    public StatBlock stats;

    //Private props
    private bool isEpic;
    //Private props - Replacements for stat's modifier call
    private int strMod;
    private int dexMod;
    private int conMod;
    private int intMod;
    private int wisMod;
    private int chaMod;


    //Unity's functions

    void Awake()
    {
        //Setup stats' modifiers
        stats.Strength.FillModifier();
        stats.Dexterity.FillModifier();
        stats.Constitution.FillModifier();
        stats.Intelligence.FillModifier();
        stats.Wisdom.FillModifier();
        stats.Charisma.FillModifier();
        //Setup replacements for stats
        strMod = stats.Strength.modifier;
        dexMod = stats.Dexterity.modifier;
        conMod = stats.Constitution.modifier;
        intMod = stats.Intelligence.modifier;
        wisMod = stats.Wisdom.modifier;
        chaMod = stats.Charisma.modifier;

        //Setup other props
        level = CalculateLevelByXP();
        SetupStatBlock();

        maxHitDices.diceType = characterClass.hitDiceType;
        maxHitDices.count = level;
        currentHitDices = maxHitDices;

        maxHitPoints = GameLogic.RollDice(maxHitDices);
        currentHitPoints = maxHitPoints;

        //Main stat's modifier
        switch (characterClass.MainStat)
        {
            case StatType.Strength:
                mainStatMod = stats.Strength.modifier;
                break;
            case StatType.Dextirity:
                mainStatMod = stats.Dexterity.modifier;
                break;
            case StatType.Constitution:
                mainStatMod = stats.Constitution.modifier;
                break;
            case StatType.Intelligence:
                mainStatMod = stats.Intelligence.modifier;
                break;
            case StatType.Wisdom:
                mainStatMod = stats.Wisdom.modifier;
                break;
            case StatType.Charisma:
                mainStatMod = stats.Charisma.modifier;
                break;
        }
    }

    private void Update()
    {
        if (maxHitPoints < currentHitPoints)
        {
            currentHitPoints = maxHitPoints;
        }

        if (currentHitPoints <= 0)
        {
            if (!gameObject.CompareTag("Player"))
            {
                GameManager.instance.enemiesDestroyed++;
                GameManager.instance.battle = false;
                player.LevelUp();

                Destroy(gameObject);
            }
            else
            {
                gameOverScreen.SetActive(true);
                canvas.SetActive(false);
                Destroy(cameraController);
                Destroy(gameObject);
            }
        }
    }

    //Custom functions

    //Custom functions - Character's stats presetting
    public int CalculateLevelByXP()
    {
        if (!isEpic)
        {
            var savedLevel = level;

            if (XP < 300)
                level = 1;
            else if (XP < 600)
                level = 2;
            else if (XP < 2700)
                level = 3;
            else if (XP < 6500)
                level = 4;
            else if (XP < 14000)
                level = 5;
            else if (XP < 23000)
                level = 6;
            else if (XP < 34000)
                level = 7;
            else if (XP < 48000)
                level = 8;
            else if (XP < 64000)
                level = 9;
            else if (XP < 85000)
                level = 10;
            else if (XP < 100000)
                level = 11;
            else if (XP < 120000)
                level = 12;
            else if (XP < 140000)
                level = 13;
            else if (XP < 165000)
                level = 14;
            else if (XP < 195000)
                level = 15;
            else if (XP < 225000)
                level = 16;
            else if (XP < 265000)
                level = 17;
            else if (XP < 305000)
                level = 18;
            else if (XP < 355000)
                level = 19;
            else
                level = 20;

            if (savedLevel != level)
            {
                savedLevel = level;
                LevelUp();
            }
        }

        return level;
    }

    public int CalculateXPbyLevel()
    {
        var XP = 0;

        if (!isEpic)
        {
            if (level <= 1)
                XP = 0;
            else if (level == 2)
                XP = 300;
            else if (level == 3)
                XP = 600;
            else if (level >= 4 && level < 5)
                XP = 2700;
            else if (level >= 5 && level < 6)
                XP = 6500;
            else if (level >= 6 && level < 7)
                XP = 14000;
            else if (level >= 7 && level < 8)
                XP = 23000;
            else if (level >= 8 && level < 9)
                XP = 34000;
            else if (level >= 9 && level < 10)
                XP = 48000;
            else if (level >= 10 && level < 11)
                XP = 64000;
            else if (level >= 11 && level < 12)
                XP = 85000;
            else if (level >= 12 && level < 13)
                XP = 100000;
            else if (level >= 13 && level < 14)
                XP = 120000;
            else if (level >= 14 && level < 15)
                XP = 140000;
            else if (level >= 15 && level < 16)
                XP = 165000;
            else if (level >= 16 && level < 17)
                XP = 195000;
            else if (level >= 17 && level < 18)
                XP = 225000;
            else if (level >= 18 && level < 19)
                XP = 265000;
            else if (level >= 19 && level < 20)
                XP = 305000;
            else if (level >= 20)
                XP = 355000;
        }

        return XP;
    }

    public void SetupStatBlock()
    {
        initiativeModifier = dexMod;
        passivePerception = 10 + wisMod;
    }

    //Custom functions - Other - LevelUp management
    public void LevelUp()
    {
        maxHitPoints += GameLogic.RollDice(SeveralDices.StringToDices($"1{characterClass.hitDiceType.ToString()}"));
    }
    public void ReachEpicLevel(int epicLevel)
    {
        level = epicLevel;
    }

    //Custom functions - Battle
    public void RollForInitiative()
    {
        initiativeRoll = GameLogic.RollDice(SeveralDices.StringToDices("1d20")) + initiativeModifier;
    }

    //Enums
    [Serializable]
    public enum CreatureType
    {
        Aberration,
        Beast,
        Celestial,
        Construct,
        Dragon,
        Elemental,
        Fey,
        Fiend,
        Giant,
        Humanoid,
        Monstrosity,
        Ooze,
        Plant,
        Undead,
    }

    [Serializable]
    public enum ActionTypes
    {
        Attack,
        Magic,
        Dash,
        Disengage,  //Позволяет переместиться, не провоцируя opportunity attacks
        Dodge,  //Все атаки против тебя проходят с disadvantage, ты получаешь advantage на спасброски Ловкости.
        Help,  //Даёт союзнику advantage на проверку характеристики или атаку.
        Hide,
        Ready,  //TODO: Add triggers (enum?)
        Search,
        UseAnObject,
    }

    [Serializable]
    public enum Conditions  //TODO: ConditionsManager
    {
        Blinded,
        Charmed,
        Deafened,
        Frightened,
        Grappled,
        Incapacitated,
        Invisible,
        Paralyzed,
        Petrified,
        Poisoned,
        Prone,
        Restrained,
        Stunned,
        Unconscious,
    }
}


[Serializable]
public enum StatType
{
    Strength,
    Dextirity,
    Constitution,
    Intelligence,
    Wisdom,
    Charisma,
}

[Serializable]
public struct SingleStat
{
    public int score;
    public int modifier;
    public StatType type;

    public void FillModifier()
    {
        modifier = (int)Mathf.Floor((score - 10) / 2);
    }
}

[Serializable]
public struct StatBlock
{
    public SingleStat Strength;
    public SingleStat Dexterity;
    public SingleStat Constitution;
    public SingleStat Intelligence;
    public SingleStat Wisdom;
    public SingleStat Charisma;
}

[Serializable]
public struct Speed
{
    public int walkingSpeed;
    public int swimmingSpeed;
    public int burrowingSpeed;
    public int flyingSpeed;
}
