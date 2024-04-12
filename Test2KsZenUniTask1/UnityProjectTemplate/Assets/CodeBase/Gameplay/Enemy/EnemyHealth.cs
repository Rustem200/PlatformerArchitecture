﻿using CodeBase.Data;
using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float _current;

        [SerializeField]
        private float _max;

        public event Action HealthChanged;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public void TakeDamage(float damage)
        {
            Current -= damage;

            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            throw new NotImplementedException();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            throw new NotImplementedException();
        }
    }
}