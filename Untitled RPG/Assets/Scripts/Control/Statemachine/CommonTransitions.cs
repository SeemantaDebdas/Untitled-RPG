using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class CommonTransitions : MonoBehaviour
    {
        [field: SerializeField] public List<Transition> TransitionList { get; private set; }
    }
}
