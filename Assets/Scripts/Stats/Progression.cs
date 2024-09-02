using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClass = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;

        public float GetStat(Stat stat, CharacterClass Class, int level)
        {
            BuildLookUp();

            float[] levels = lookUpTable[Class][stat];

            if (levels.Length < level) return 0;

            return levels[level - 1];
        }

        private void BuildLookUp()
        {
            if (lookUpTable != null) return;

            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass character in characterClass)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach(var progressionStat in character.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }

                lookUpTable[character.Class] = statLookUpTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass Class)
        {
            BuildLookUp();
            
            float[] levels = lookUpTable[Class][stat];
            return levels.Length;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass Class;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}