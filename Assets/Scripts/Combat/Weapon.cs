using PRG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float Range = 2f;
        [SerializeField] float Damage = 5f;
        [SerializeField] float PercenteDamage = 0;
        [SerializeField] bool isRight = true;
        [SerializeField] Projectile projectile = null;

        const string WeaponName = "Weapon";

        public float GetDamage()    
        {
            return Damage;
        }

        public float GetPercentageBonus()
        {
            return PercenteDamage;
        }

        public float GetRange()
        {
            return Range;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform RightHand, Transform LeftHand, Health target, GameObject instigator, float damage)
        {              
            Projectile instance = Instantiate(projectile, GetHand(RightHand, LeftHand).position, Quaternion.identity);
            instance.SetTarget(target, instigator, damage);
        }

        public void SpawnWeapon(Transform RightHand, Transform LeftHand, Animator animator)
        {
            DestroyOldWeapon(RightHand, LeftHand);

            if (equippedPrefab != null)
            {               
                GameObject weapon = Instantiate(equippedPrefab, GetHand(RightHand, LeftHand));
                weapon.name = WeaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;  
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            //else if(overrideController != null) 
            //{  
            //    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController; 
            //}
            
        }

        private void DestroyOldWeapon(Transform RightHand, Transform LeftHand)
        {
            Transform OldWeapon = RightHand.Find(WeaponName);
            if (OldWeapon == null)
            {
                OldWeapon = LeftHand.Find(WeaponName);
            }
            if (OldWeapon == null) return;

            OldWeapon.name = "Destroy";
            Destroy(OldWeapon.gameObject);
        }

        private Transform GetHand(Transform RightHand, Transform LeftHand)
        {
            Transform HandPos;

            if (isRight) return HandPos = RightHand;
            else return HandPos = LeftHand;
            
        }
    }
}