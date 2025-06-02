using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Wizard : CharacterClass
{
    private int level;
    
    public Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
        level = character.level;
    }
}
