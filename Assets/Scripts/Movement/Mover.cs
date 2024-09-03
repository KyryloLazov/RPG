using PRG.Attributes;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        public float MaxSpeed = 6f;
        
        private NavMeshAgent agent;
        private Animator animator;
        private ActionScheduler actionScheduler;
        private Health health;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = !health.isDead;

            ControlAnim();
        }

        public void MoveTo(Vector3 dest, float speedFraction)
        {
            actionScheduler.StartAction(this);
            agent.SetDestination(dest);
            agent.speed = MaxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;           
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        private void ControlAnim()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("ForwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            agent.enabled = false;
            transform.position = position.ToVector();
            agent.enabled = true;
        }
    }
}