using UnityEngine;

namespace Moein.ChainBone
{
    public class HeadController : MonoBehaviour
    {
        [Header("Head")] [SerializeField] private Transform target;
        [SerializeField] private Transform headBone;
        [SerializeField] private float headMaxTurnAngle = 180;
        [SerializeField] private float headTrackingSmoothSpeed = 10;

        private float deltaTime = 0;

        void Update()
        {
            deltaTime = Time.deltaTime;
            HeadTracking();
        }

        void HeadTracking()
        {
            Quaternion currentLocalRotation = headBone.localRotation;
            headBone.localRotation = Quaternion.identity;

            Vector3 targetWorldLookDir = target.position - headBone.position;
            Vector3 targetLocalLookDir = headBone.InverseTransformDirection(targetWorldLookDir);

            targetLocalLookDir = Vector3.RotateTowards(
                Vector3.forward,
                targetLocalLookDir,
                Mathf.Deg2Rad * headMaxTurnAngle,
                0
            );

            Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

            headBone.localRotation = Quaternion.Slerp(
                currentLocalRotation,
                targetLocalRotation,
                1 - Mathf.Exp(-headTrackingSmoothSpeed * deltaTime)
            );
        }
    }
}