using RPG.Data;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerStatemachine : Statemachine
    {
        PlayerContext playerContext;
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
                    PlayerInput = GetComponent<PlayerInput>(),
                    PhysicsHandler = GetComponent<CharacterPhysicsHandler>(),
                    EnvironmentScanner = GetComponent<EnvironmentScanner>(),
                    FieldOfView = GetComponent<FieldOfView>(),
                    WeaponHandler = GetComponent<WeaponHandler>()
                };

                return playerContext;
            }
        }

        private void Start()
        {
            SwitchState(initialState);
        }

    }
}