using Model.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace UnitBrains
{
    public class BuffsSys : MonoBehaviour
    {

        public Dictionary<Unit, List<Buff>> Effects { get; private set; }  = new Dictionary<Unit, List<Buff>>();

        private void Awake()
        {
            ServiceLocator.RegisterAs(this, typeof(BuffsSys));
        }

        public void AddBuff(Unit unit, Buff buff)
        {
            if (!Effects.ContainsKey(unit)) Effects[unit] = new List<Buff>();

            Effects[unit].Add(buff);
            StartCoroutine(ReducedDuration(unit, buff));
        }

        private IEnumerator ReducedDuration(Unit unit, Buff buff)
        {
            float elapsedTime = 0f;

            while (elapsedTime < buff.Duration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (Effects.ContainsKey(unit))
            {
                Effects[unit].Remove(buff);
                if (Effects[unit].Count == 0)
                {
                    Effects.Remove(unit);
                }
            }
        }

        public float GetSpeedModifier(Unit unit)
        {
            if (!Effects.ContainsKey(unit) || Effects[unit].Count == 0) return 1f;

            float modifier = 1f;
            foreach (Buff buff in Effects[unit])
            {
                modifier *= buff.ModifierSpeed;
            }
            return modifier;
        }

        public float GetAttackModifier(Unit unit)
        {
            if (!Effects.ContainsKey(unit) || Effects[unit].Count == 0) return 1f;

            float modifier = 1f;
            foreach (Buff buff in Effects[unit])
            {
                modifier *= buff.ModifierAttack;
            }
            return modifier;
        }

        public class Buff
        {
            public float Duration { get; set; }
            public float ModifierSpeed { get; set; }
            public float ModifierAttack { get; set; }

            public Buff(float duration, float modifierSpeed, float modifierAttack)
            {
                Duration = duration;
                ModifierSpeed = modifierSpeed;
                ModifierAttack = modifierAttack;
            }
        }
    }
}
