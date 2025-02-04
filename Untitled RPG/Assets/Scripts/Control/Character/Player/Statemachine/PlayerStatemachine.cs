using System;
using RPG.Data;
using RPG.Core;
using UnityEngine;
using RPG.Combat.Rework;
using RPG.DialogueSystem;
using RPG.Quest;

namespace RPG.Control
{
    public class PlayerStatemachine : Statemachine, IContextProvider
    {
        PlayerContext playerContext;
        [SerializeField] private CharacterHurtState hurtState;
        public override Context Context
        {
            get
            {
                if (playerContext != null)
                    return playerContext;
                
                playerContext = new PlayerContext
                {
                    Transform = transform,
                    Animator = GetComponent<Animator>(),
                    CharacterController = GetComponent<CharacterController>(),
                    InputReader = GetComponent<InputReader>(),
                    PhysicsHandler = GetComponent<CharacterPhysicsHandler>(),
                    EnvironmentScanner = GetComponent<EnvironmentScanner>(),
                    FieldOfView = GetComponent<FieldOfView>(),
                    WeaponHandler = GetComponent<WeaponHandler>(),
                    VFXHandler = GetComponent<VFXHandler>(),
                    Interactor = GetComponent<Interactor>(),
                    CombatHandler = GetComponent<CombatHandler>(),
                    PlayerConversant = GetComponent<PlayerConversant>(),
                    QuestList = GetComponent<QuestList>(),
                    Health = GetComponent<Health>(),
                };

                return playerContext;
            }
        }

        private void Start()
        {
            SwitchState(initialState);
        }

        private void OnEnable()
        {
            PlayerContext context = GetContext() as PlayerContext;
            context.Health.OnDamage += Health_OnDamage;
        }

        private void OnDisable()
        {
            PlayerContext context = GetContext() as PlayerContext;
            context.Health.OnDamage -= Health_OnDamage;
        }

        private void Health_OnDamage(DamageData damageData)
        {
            hurtState.SetDamageData(damageData);
            SwitchState(hurtState);
        }

        public Context GetContext() => Context;
    }
}