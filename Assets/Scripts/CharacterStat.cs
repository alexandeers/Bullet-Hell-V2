using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class CharacterStat
{
    public float BaseValue;
    float lastBaseValue = float.MinValue;
    public float Value {
        get { 
            if(isDirty || BaseValue != lastBaseValue) {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue(); 
                isDirty = false;
            }
            return _value;
        } 
    }

    private readonly List<StatModifier> statModifiers;
    private readonly ReadOnlyCollection<StatModifier> StatModifiers;
    private bool isDirty = true; //För att inte man ska upprepa CalculateFinalValue hela tiden.
    private float _value; //Senast beräknade värdet

    public CharacterStat() {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    } 

    public CharacterStat(float baseValue) : this() {
        BaseValue = baseValue;
    }

    public void AddModifier(StatModifier stat) {
        statModifiers.Add(stat);
        isDirty = true;
        statModifiers.Sort(CompareOrder);
    }

    public int CompareOrder(StatModifier a, StatModifier b) {
        if(a.Order < b.Order) return -1;
        else if (a.Order > b.Order) return 1;
        return 0;
    }

    public bool RemoveModifier(StatModifier stat) {
        if (statModifiers.Remove(stat)) {
            isDirty = true;
            return true;
        }
        return false;
    }

    public bool RemoveAllModifiersFromSource(object source) {
        bool didRemove = false;
        for(int i = statModifiers.Count -1; i >= 0; i--) {
            if(statModifiers[i].Source == source) {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    private float CalculateFinalValue() {
        float finalValue = BaseValue;
        float sumPercent = 0;
        for(int i=0; i < statModifiers.Count; i++) {
            StatModifier stat = statModifiers[i];

            if(stat.Type == StatModType.Flat) finalValue += statModifiers[i].Value;
            if(stat.Type == StatModType.PercentAdd) {
                sumPercent += stat.Value;

                if(i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd) {
                    finalValue *= 1 + sumPercent;
                    sumPercent = 0;
                }
            }
            if(stat.Type == StatModType.PercentMult) finalValue *= 1 + statModifiers[i].Value;
        }

        return (float) Mathf.Round(finalValue);
    }
}
