using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Controller;

namespace RPG.Cinematics
{
    public class ControlRemover : MonoBehaviour
    {
        private PlayableDirector director;
        private GameObject player;
        private PlayerController controller;
        private ActionScheduler scheduler;
        private void Start()
        {
            director = GetComponent<PlayableDirector>();
            director.played += DisableControl;
            director.stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
            scheduler = player.GetComponent<ActionScheduler>();
            controller = player.GetComponent<PlayerController>();
        }

        void DisableControl(PlayableDirector pb)
        {
            scheduler.CancelAction();
            controller.enabled = false;
        }

        void EnableControl(PlayableDirector pb)
        {
            controller.enabled = true;
        }
    }
}
