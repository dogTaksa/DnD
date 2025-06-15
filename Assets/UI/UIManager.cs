using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Spell slots")]
    public GameObject spellSlotsGO;
    public List<Button> level1;
    public List<Button> level2;
    public List<Button> level3;
    public List<Button> level4;
    public List<Button> level5;
    public List<Button> level6;
    public List<Button> level7;
    public List<Button> level8;
    public List<Button> level9;

    [Header("HP")]
    public Slider hPSlider;
    public TMP_Text hPText;

    [Header("Other linkers")]
    public GameObject instructions;
    public GameObject castButton;
    public TMP_InputField inputForSpellsName;
    public Character pc;

    private List<List<Button>> slots;
    private int currentLevelSelected;
    private List<Spell> possibleToCastSpells = new List<Spell>();
    private Button currentSpellSlot;

    private void Awake()
    {
        castButton.GetComponent<Button>().interactable = false;

        spellSlotsGO.SetActive(false);
        instructions.SetActive(false);
        castButton.SetActive(false);
        inputForSpellsName.gameObject.SetActive(false);
    }
    private void Update()
    {
        hPSlider.value = (float)pc.currentHitPoints / (float)pc.maxHitPoints;
        print(hPSlider.value);
        hPText.text = $"{pc.currentHitPoints}/{pc.maxHitPoints}";

        if (Input.GetKeyDown(KeyCode.Q))
        {
            spellSlotsGO.SetActive(!spellSlotsGO.activeSelf);
            instructions.SetActive(!instructions.activeSelf);
            castButton.SetActive(!castButton.activeSelf);
            inputForSpellsName.gameObject.SetActive(!inputForSpellsName.gameObject.activeSelf);
        }
    }

    public void UseSpellSlot(Button spellSlot)
    {
        var level = spellSlot.gameObject.GetComponent<Spellslot>().level;

        currentLevelSelected = level;
        currentSpellSlot = spellSlot;

        foreach (var spell in Magic.existingSpells)
        {
            if (spell.Value.level <= level)
            {
                possibleToCastSpells.Add(spell.Value);
            }
        }

        currentSpellSlot.interactable = false;
        castButton.GetComponent<Button>().interactable = true;
    }

    public void OnCast()
    {
        foreach (var item in Magic.existingSpells)
        {
            print(item.Key);
        }

        if (inputForSpellsName.text != "" && Magic.existingSpells.Keys.Contains(inputForSpellsName.text))
        {
            var spell = Magic.existingSpells[inputForSpellsName.text];

            if (possibleToCastSpells.Contains(spell))
            {
                if (spell.level < currentLevelSelected)
                {
                    castButton.GetComponent<Button>().interactable = false;

                    if (spell.name == "Healing word")
                    {
                        spell.effectWithScaling.Invoke(new List<Character>() { pc }, pc, currentLevelSelected);
                        pc.GetComponent<Animator>().SetBool("isHeal", true);
                    }
                    else
                    {
                        spell.effectWithScaling.Invoke(new List<Character>() { pc }, pc, currentLevelSelected);
                    }
                }
                else
                {
                    if (spell.name == "Healing word")
                    {
                        spell.effect.Invoke(new List<Character>() { pc }, pc);
                        pc.GetComponent<Animator>().SetBool("isHeal", true);
                    }
                    else
                    {
                        spell.effect.Invoke(new List<Character>() { pc }, pc);
                    }
                }

                GameManager.instance.isMove = false;
            }
            else
            {
                print("Please use higher level spell slot to cast this spell");
            }
        }
        else
        {
            print("Please use existing spells");
        }
        
    }
}
