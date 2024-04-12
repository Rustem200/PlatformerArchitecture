using CodeBase.Data;
using CodeBase.Services.PlayerProgressService;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HeroHealth : MonoBehaviour, IHealth, IProgressSaver
{
    private State _state = new State();
    //[SerializeField] private float _health;
    public event Action HealthChanged;

    public float Current
    {
        get => _state.CurrentHP;
        set
        {
            if (value != _state.CurrentHP)
            {
                _state.CurrentHP = value;

                HealthChanged?.Invoke();
            }
        }
    }

    public float Max
    {
        get => _state.MaxHP;
        set => _state.MaxHP = value;
    }


    public void LoadProgress(PlayerProgress progress)
    {
        _state = progress.HeroState;
        HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.HeroState.CurrentHP = Current;
        progress.HeroState.MaxHP = Max;
    }

    public void TakeDamage(float damage)
    {
        /*if (Current <= 0)
            return;*/
        if (_state == null)
            Debug.Log("nulllll");

        Current -= damage;
        Debug.Log(Current);
        //Animator.PlayHit();
    }
}

public interface IHealth : IProgressSaver
{
    event Action HealthChanged;
    float Current { get; set; }
    float Max { get; set; }
    void TakeDamage(float damage);
}
