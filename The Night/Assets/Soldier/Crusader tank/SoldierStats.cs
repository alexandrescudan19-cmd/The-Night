using UnityEngine;

namespace MyGameNamespace
{
    public class SoldierStats : MonoBehaviour
    {
        public float baseAttack = 10f;
        public float baseDefense = 5f;
        public float baseHP = 100f;

        public float attackMultiplier = 1f;
        public float defenseMultiplier = 1f;
        public float regenRate = 0f;

        private float currentHP;

        void Start()
        {
            currentHP = baseHP;
        }

        void Update()
        {
            if (regenRate > 0f && currentHP < baseHP)
            {
                currentHP += regenRate * Time.deltaTime;
                currentHP = Mathf.Min(currentHP, baseHP);
            }
        }

        public float GetAttack() => baseAttack * attackMultiplier;
        public float GetDefense() => baseDefense * defenseMultiplier;
        public float GetHP() => currentHP;
    }
}
