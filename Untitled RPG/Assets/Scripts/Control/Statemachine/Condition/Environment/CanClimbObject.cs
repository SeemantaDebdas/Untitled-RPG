using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "CanClimbObject", menuName = "Condition/Can Climb Object", order = 1)]
    public class CanClimbObject : ConditionSO
    {
        [Range(0f, 3f)]
        [SerializeField] float minHeight, maxHeight;

        [Range(0f, 1f)]
        [field: SerializeField] public float MatchStartTime, MatchEndTime;   
        [field: SerializeField] public string AnimationName { get; private set; }
        [field: SerializeField] public Vector3 MatchPositionOffset { get; private set; }
        [field: SerializeField] public AvatarTarget AvatarTarget { get; private set; }


        public override void Initialize(Context context)
        {

        }

        public override void Reset()
        {

        }

        protected override bool ProcessCondition(Context context)
        {
            CharacterContext characterContext = context as CharacterContext;

            EnvironmentScanner scanner = characterContext.EnvironmentScanner;

            RaycastHit? objHit = scanner.GetObjectInfront(context.Transform.position);
            Vector3 positionOnObject = scanner.GetObjectBelow(objHit.Value.point).Value.point;

            Vector3 heightDiff = positionOnObject - context.Transform.position;


            return heightDiff.y <= maxHeight && heightDiff.y >= minHeight;
        }
    }
}
