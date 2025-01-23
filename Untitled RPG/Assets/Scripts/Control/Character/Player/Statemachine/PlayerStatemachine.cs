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
                };

                return playerContext;
            }
        }

        private void Start()
        {
            SwitchState(initialState);
        }

        public Context GetContext() => Context;
    }
}