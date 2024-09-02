using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int StartingLevel = 1;
        [SerializeField] CharacterClass CharacterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject LevelUpEffectPrefab;
        [SerializeField] bool UseModifiers = false;

        public event Action OnLevelUp;

        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Expierence exp = GetComponent<Expierence>();
            if(exp != null)
            {
                exp.onXPGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(LevelUpEffectPrefab, transform.position, Quaternion.identity);
        }

        public float GetStat(Stat stat)
        {
            return GetBaseStat(stat) + GetAdditiveModifier(stat) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, CharacterClass, GetLevel());
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                return CalculateLevel();
            }
            return currentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!UseModifiers) return 0;
            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            float modifiers = 0;
            foreach(var provider in providers)
            {
                foreach(float modifier in provider.GetAdditiveModifier(stat))
                {
                    modifiers += modifier;
                }
                
            }
            return modifiers;
        }

        private float GetPercentageModifier(Stat stat)
        {
            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            float modifiers = 0;
            foreach (var provider in providers)
            {
                foreach (float modifier in provider.GetPercentageModifier(stat))
                {
                    modifiers += modifier;
                }

            }
            return modifiers;
        }

        private int CalculateLevel()
        {
            Expierence expierence = GetComponent<Expierence>();

            if (expierence == null) return StartingLevel;

            float currentXP = expierence.GetXP();
            int PenulimateLevel = progression.GetLevels(Stat.ExpToLevelUp, CharacterClass);
            for (int level = 1; level < PenulimateLevel; level++)
            {
                float XPtoLevelUp = progression.GetStat(Stat.ExpToLevelUp, CharacterClass, level);
                if(XPtoLevelUp > currentXP)
                {
                    return level;
                }
            }

            return PenulimateLevel + 1;
        }
    }
}