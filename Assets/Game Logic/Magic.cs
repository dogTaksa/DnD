using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public Dictionary<int, Spell> existingSpells;
}

[Serializable]
public class Spell
{
    //Props
    public string name;
    [Range(0, 9)] public int level;
    public AbilitiesCastingTime castingTime;
    public SpellsRange spellsRange;
    public SpellsComponents components;
    public TargetArea targetArea;
    public Character spellsOwner;

    //Usings
    public List<Spell> activeCopies;

    //Effects
    public void Effect()
    {
        
    }
    public void EffectWithScaling(int currentLevel)
    {
        if (currentLevel > level && currentLevel <= 9)
        {

        }
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
    }

    [Serializable]
    public struct TargetArea
    {
        public TargetAreaType areasType;
        [Min(0)] public int DistanceOrTargetsCount;
    }

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
}