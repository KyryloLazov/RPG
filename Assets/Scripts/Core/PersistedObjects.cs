using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistedObjects : MonoBehaviour
    {
        [SerializeField] GameObject Prefab;

        static bool hasSpawned = false;
        private void Awake()
        {
            if (hasSpawned)
            {
                return;
            }
            SpawnPersistantObjects();

            hasSpawned = true;
        }

        private void SpawnPersistantObjects()
        {
            GameObject PersistanObject = GameObject.Instantiate(Prefab);
            DontDestroyOnLoad(PersistanObject);
        }
    }
}