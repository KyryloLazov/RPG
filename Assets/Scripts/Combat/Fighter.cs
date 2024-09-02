using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using PRG.Attributes;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider
    {
        private Health currentTarget;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;
        private Health health;

        public float TimeBetweenAtacks = 1f;
        private float timeSinceLastAtack = 0;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon DefaultWeapon = null;
        Weapon CurrentWeapon = null;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();

            EquipWeapon(DefaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAtack += Time.deltaTime;
            if (currentTarget == null) return;
            if (currentTarget.isDead) return;
            if (health.isDead) return;

            bool inRange = Vector3.Distance(transform.position, currentTarget.transform.position) <= CurrentWeapon.GetRange();
            if (!inRange)
            {
                mover.MoveTo(currentTarget.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                actionScheduler.StartAction(this);
                Atack();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
            weapon.SpawnWeapon(rightHandTransform, leftHandTransform, animator);
        }
        public bool CanAtack(GameObject target)
        {
            if (target == null) return false;
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth != null && targetHealth.isDead)
            {
                return false;
            }
            return true;
        }

        private void Atack()
        {
            transform.LookAt(currentTarget.transform);
            if (timeSinceLastAtack > TimeBetweenAtacks && !currentTarget.isDead)
            {
                animator.ResetTrigger("StopAtack");
                animator.SetTrigger("Atack");
                timeSinceLastAtack = 0;
            }

        }

        public void Hit()
        {
            if (currentTarget != null)
            {
                float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
                if (CurrentWeapon.HasProjectile())
                {
                    CurrentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, currentTarget, gameObject, damage);
                }
                else
                {
                    currentTarget.TakeDamage(gameObject, damage);
                }
            }
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return CurrentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return CurrentWeapon.GetPercentageBonus();
            }
        }

        public void Shoot()
        {
            Hit();
        }

        public void SetTarget(GameObject target)
        {
            currentTarget = target.GetComponent<Health>();
        }

        public Health GetTarget()
        {
            if (currentTarget != null)
            {
                return currentTarget;
            }
            return null;
        }

        public void Cancel()
        {
            animator.SetTrigger("StopAtack");
            currentTarget = null;
            animator.ResetTrigger("Atack");
            mover.Cancel();

        }       
    }
}
