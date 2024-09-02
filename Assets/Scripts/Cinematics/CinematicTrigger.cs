using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace PRG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isPlayed = false;
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && !isPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                isPlayed = true;
            }
        }
    }
}
