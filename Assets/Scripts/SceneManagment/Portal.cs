using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.ScenesManager
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdent 
        { 
            A, B, C, D, E
        }
        
        public Transform spawnpoint;
        public int scene;
        public float FadeOuttime;
        public float FadeIntime;
        public float FadeWaittime;
        [SerializeField]private DestinationIdent destination;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }
    
        IEnumerator Transition()
        { 
            DontDestroyOnLoad(gameObject);

            Fader fader = FindAnyObjectByType<Fader>();

            yield return fader.FadeOut(FadeOuttime);

            SavignWrapper wrapper = FindObjectOfType<SavignWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(scene);

            Portal otherPortal = GetOtherPortal();
            wrapper.Load();

            UpdatePlayer(otherPortal);
            wrapper.Save();

            yield return new WaitForSeconds(FadeWaittime);

            yield return fader.FadeIn(FadeIntime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.rotation = otherPortal.spawnpoint.rotation;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnpoint.position);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this)
                {
                    continue;
                }
                if(portal.destination != this.destination)
                {
                    continue;
                }
                return portal;
            }
            return null;
        }
    }
}