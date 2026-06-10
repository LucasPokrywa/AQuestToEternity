using UnityEngine;

namespace ParallelCascades.Common.Runtime
{
    /// <summary>
    /// This custom attribute adds a button above its property's serialized field in the inspector.
    /// Multiple buttons can be added above single field by using this attribute multiple times.
    /// Methods have to be public.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public readonly string methodName;
        public readonly string buttonText;

        public InspectorButtonAttribute(string methodName, string buttonText)
        {
            this.methodName = methodName;
            this.buttonText = buttonText;
        }
    }
}