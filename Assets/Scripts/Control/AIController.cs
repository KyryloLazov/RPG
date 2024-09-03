using PRG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        public float ChaseDistance = 5f;
        public float SusTime = 5f;
        public float DwellTime = 2f;
        public float AggroTime = 5f;
        [SerializeField] float shoutDistance = 5f;
        public PatrolPath PatrolPath;
        public float WaypointTolerance = 1f;
        [Range(0,1)]
        public float PatrolSpeedFraction = 0.2f;

        private GameObject player;
        private Mover mover;
        private Animator animator;
        private Fighter fighter;
        private Health health;
        private ActionScheduler actionScheduler;
        private int CurrentWaypoint = 0;

        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeOnCheckPoint = Mathf.Infinity;
        private float timeSinceAggro = Mathf.Infinity;

        Vector3 guardLocation;
        void Start()
        {
            player = GameObject.FindWithTag("Player");

            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();

            guardLocation = transform.position;
        }

        void Update()
        {
            if (health.isDead) return;
            if (IsAggro() && fighter.CanAtack(player))
            {
                AtackBehaviour();
            }
            else if (timeSinceLastSawPlayer < SusTime)
            {
                SusBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggro()
        {
            timeSinceAggro = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeOnCheckPoint += Time.deltaTime;
            timeSinceAggro += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPos = guardLocation;

            if(PatrolPath != null)
            {
                if (AtWaypoint())
                {
                    GetNextWaypoint();
                    timeOnCheckPoint = 0;
                }                
                nextPos = GetCurrentWaypoint();

            }
            if (timeOnCheckPoint > DwellTime)
            {
                mover.MoveTo(nextPos, PatrolSpeedFraction);
            }
            
        }

        private bool AtWaypoint()
        {
            float Distance = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return Distance < WaypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return PatrolPath.GetWaypoint(CurrentWaypoint);
        }

        private void GetNextWaypoint()
        {
            CurrentWaypoint = PatrolPath.GetNextWaypoint(CurrentWaypoint);
        }

        private void SusBehaviour()
        {
            actionScheduler.CancelAction();
        }

        private void AtackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.SetTarget(player);

            AggroNearBy();
        }

        private void AggroNearBy()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController controller = hit.collider.GetComponent<AIController>();
                if (controller == null) continue;
                
                controller.Aggro();

            }
        }

        private bool IsAggro()
        {
            float dis = Vector3.Distance(transform.position, player.transform.position);
            return dis < ChaseDistance || timeSinceAggro < AggroTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, ChaseDistance);
        }
    }
}