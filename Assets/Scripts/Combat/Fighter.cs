using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using PRG.Attributes;
using RPG.Stats;
using RPG.Saving;
using GameDevTV.Utils;
using Unity.VisualScripting;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider, ISaveable
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
        [SerializeField] WeaponConfig DefaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";

        WeaponConfig CurrentWeaponConfig;
        LazyValue<Weapon> CurrentWeapon;

        private void Awake()
        {
            CurrentWeaponConfig = DefaultWeapon;
            CurrentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);

            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        private Weapon SetupDefaultWeapon()
        {           
            return AtachWeapon(DefaultWeapon);
        }

        private void Start()
        {
            CurrentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAtack += Time.deltaTime;
            if (currentTarget == null) return;
            if (currentTarget.isDead) return;
            if (health.isDead) return;

            if (!GetInRange(currentTarget.transform))
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

        private bool GetInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) <= CurrentWeaponConfig.GetRange();
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            CurrentWeaponConfig = weapon;
            CurrentWeapon.value = AtachWeapon(weapon);
        }

        private Weapon AtachWeapon(WeaponConfig weapon)
        {
            return weapon.SpawnWeapon(rightHandTransform, leftHandTransform, animator);
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

                if(CurrentWeapon.value != null)
                {
                    CurrentWeapon.value.OnHit();
                }

                if (CurrentWeaponConfig.HasProjectile())
                {
                    CurrentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, currentTarget, gameObject, damage);
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
                yield return CurrentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return CurrentWeaponConfig.GetPercentageBonus();
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

        public object CaptureState()
        {
            return CurrentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;

            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);

            EquipWeapon(weapon);
        }
    }
}
