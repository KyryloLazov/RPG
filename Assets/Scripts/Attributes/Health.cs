using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Attributes;
using UnityEngine.Events;

namespace PRG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float MaxHealth = -1f;
        public float health;
        private Animator animator;
        public bool isDead;
        private ActionScheduler actionScheduler;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent OnDie;

        private void Awake()
        {
            if (health <= 0)
            {
                MaxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
                health = MaxHealth;
            }
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();

            BaseStats stats = GetComponent<BaseStats>();
            stats.OnLevelUp += HealOnLvlUp;
        }

        public float getHealth()
        {
            return health;
        }

        public float getMaxHealth()
        {
            return MaxHealth;
        }

        public void HealOnLvlUp()
        {
            health = MaxHealth;
        }

        public void Heal(float healthToRestore)
        {
            health += Mathf.Max(health + healthToRestore, MaxHealth);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health = Mathf.Max(health - damage, 0);           
            if(health == 0)
            {
                Die();
                AwardXP(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        private void AwardXP(GameObject instigator)
        {
            Expierence exp = instigator.GetComponent<Expierence>();
            if (exp == null) return;

            exp.GainExp(GetComponent<BaseStats>().GetStat(Stat.ExpReward));
            
        }

        public float GetPercentage()
        {
            return (health / MaxHealth);
        }
            
        private void Die()
        {
            if (isDead) return;
            OnDie.Invoke();
            isDead = true;
            animator.SetTrigger("Die");               
            actionScheduler.CancelAction(); 
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if(health == 0)
            {
                Die();
            }
            else
            {
                isDead = false;
                animator.Play("Blend Tree");
            }
        }
    }
}
