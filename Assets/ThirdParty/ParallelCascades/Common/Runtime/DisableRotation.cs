using UnityEngine;

namespace ParallelCascades.Common.Runtime
{
    [ExecuteInEditMode]
    public class DisableRotation : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}