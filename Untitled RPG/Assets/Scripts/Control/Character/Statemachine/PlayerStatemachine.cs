using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    Animator = GetComponent<Animator>()
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