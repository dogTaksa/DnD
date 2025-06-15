using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SpellEffect(List<Character> targets, Character owner);
public delegate void SpellEffectWithScaling(List<Character> targets, Character owner, int currentLevel);
public class Magic : MonoBehaviour
{

    public static List<Character> targets = new List<Character>();
    //  EffectWithScaling
    /*  
        if (currentLevel > level && currentLevel <= 9)
        {

        }
    */

    public static Dictionary<string, Spell> existingSpells = new Dictionary<string, Spell>();

    private void Awake()
    {
        var magicMissile = new Spell()
        {
            name = "Magic missile",
            level = 1,
            castingTime = new AbilitiesCastingTime(new InGameTime(0, InGameTime.TimeScales.Minute)),
            duration = new Spell.Duration(new InGameTime(0, InGameTime.TimeScales.Minute)),
            spellsRange = new Spell.SpellsRange(distance: 120),
            components = new Spell.SpellsComponents(true, true, false),
            targetArea = new Spell.TargetArea(Spell.TargetArea.TargetAreaType.Target, 0, 3),
            spellsOwner = GetComponent<Character>(),
            effect = MagicMissileEffect,
            effectWithScaling = MagicMissileEffectWithScaling
        };
        existingSpells.Add("Magic missile", magicMissile);


        var powerWordHeal = new Spell()
        {
            name = "Healing word",
            level = 1,
            castingTime = new AbilitiesCastingTime(new InGameTime(0, InGameTime.TimeScales.Minute)),
            duration = new Spell.Duration(new InGameTime(0, InGameTime.TimeScales.Minute)),
            spellsRange = new Spell.SpellsRange(distance: 60),
            components = new Spell.SpellsComponents(true, false, false),
            targetArea = new Spell.TargetArea(Spell.TargetArea.TargetAreaType.Target, 0, 1),
            spellsOwner = GetComponent<Character>(),
            effect = HealingWordEffect,
            effectWithScaling = HealingWordEffectWithScaling
        };
        existingSpells.Add("Healing word", powerWordHeal);

        //testing
        //magicMissile.effect.Invoke(testTargets, GetComponent<Character>());
        //healingWord.effectWithScaling.Invoke(testTargets, GetComponent<Character>(), 9);
    }

    private void MagicMissileEffect(List<Character> targets, Character owner)
    {
        foreach (var target in targets)
        {
            target.currentHitPoints -= GameLogic.RollDice(SeveralDices.StringToDices("1d4")) + 1;
            target.GetComponent<Animator>().SetBool("isCasting", false);
        }
    }

    private void MagicMissileEffectWithScaling(List<Character> targets, Character owner, int currentLevel)
    {
        foreach (var target in targets)
        {
            if (currentLevel > 1 && currentLevel <= 9)
            {
                target.currentHitPoints -= (GameLogic.RollDice(SeveralDices.StringToDices("1d4")) + 1);
                target.GetComponent<Animator>().SetBool("isCasting", false);
            }
        }
    }

    private void HealingWordEffect(List<Character> targets, Character owner)
    {
        foreach (var target in targets)
        {
            target.currentHitPoints += (GameLogic.RollDice(SeveralDices.StringToDices($"2d4")) + owner.mainStatMod);
            target.GetComponent<Animator>().SetBool("isHeal", false);
        }
    }

    private void HealingWordEffectWithScaling(List<Character> targets, Character owner, int currentLevel)
    {
        foreach (var target in targets)
        {
            if (currentLevel > 1 && currentLevel <= 9)
            {
                target.currentHitPoints += (GameLogic.RollDice(SeveralDices.StringToDices($"{2 * (1 + currentLevel)}d4")) + owner.mainStatMod);
                target.GetComponent<Animator>().SetBool("isHeal", false);
            }
        }
    }
}

[Serializable]
public class Spell
{
    //Props
    public string name;
    [Range(0, 9)] public int level;
    public AbilitiesCastingTime castingTime;
    public Duration duration;
    public SpellsRange spellsRange;
    public SpellsComponents components;
    public TargetArea targetArea;
    public Character spellsOwner;
    public SpellEffect effect;
    public SpellEffectWithScaling effectWithScaling;


    //GameManager's calling (for getting targets from area) - TODO
    public void ActivateSpell()
    {
        //TODO: Get target
        //effect.Invoke(_____, spellsOwner);
    }
    public void ActivateSpellWithScaling(int currentLevel)
    {
        //TODO: Get target
        //effectWithScalig.Invoke(_____, spellsOwner,currentLevel);
    }

    //Constructors
    public Spell()
    {
        
    }

    public Spell(string name, int level, AbilitiesCastingTime castingTime, Duration duration, SpellsRange spellsRange, SpellsComponents components, TargetArea targetArea, Character spellsOwner, SpellEffect effect, SpellEffectWithScaling effectWithScaling)
    {
        this.name = name;
        this.level = level;
        this.castingTime = castingTime;
        this.duration = duration;
        this.spellsRange = spellsRange;
        this.components = components;
        this.targetArea = targetArea;
        this.spellsOwner = spellsOwner;
        this.effect = effect;
        this.effectWithScaling = effectWithScaling;
    }

    //Types (info containers)
    [Serializable]
    public struct SpellsRange
    {
        public bool Self;
        public bool Touch;
        [Min(0)] public int Distance;
        
        public SpellsRange(bool isTouch = false, int distance = 0)
        {
            if (distance == 0)
            {
                Self = !isTouch;
            }
            else
            {
                Self = false;
            }

            Touch = isTouch;
            Distance = distance;
        }
    }

    [Serializable]
    public struct SpellsComponents
    {
        public bool V;
        public bool S;
        public bool M;

        public string MaterialComponents;

        public SpellsComponents(bool v, bool s, bool m) : this()
        {
            V = v;
            S = s;
            M = m;
        }
    }

    [Serializable]
    public struct TargetArea
    {
        [Serializable]
        public enum TargetAreaType
        {
            Cone,
            Cube,
            Cylinder,
            Line,
            Sphere,
            Aura,
            Target
        }

        public TargetAreaType areasType;
        [Min(0)] public int Distance;
        [Min(0)] public int TargetsCount;

        public TargetArea(TargetAreaType areasType, int distance, int targetsCount)
        {
            this.areasType = areasType;
            Distance = distance;
            TargetsCount = targetsCount;
        }
    }

    [Serializable]
    public struct Duration
    {
        public bool isInstantaneous;
        public InGameTime time;
        public bool concentration;

        public Duration(InGameTime time, bool isInstantaneous = false, bool isConcentrationRequired = false)
        {
            this.time = time;

            if (time.time == 0)
            {
                isInstantaneous = true;
            }
            this.isInstantaneous = isInstantaneous;

            concentration = isConcentrationRequired;
        }
    }
}