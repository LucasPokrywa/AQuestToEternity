using ParallelCascades.ProceduralPlanetGenerationCommon.Runtime;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGeneration.Runtime.RuntimeGenerationSample
{
    public class SystemRandomizer : MonoBehaviour
    {
        [SerializeField] private ProceduralBodyBase[] m_proceduralBodies;
        
        public void RandomizeSystem()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            foreach (var body in m_proceduralBodies)
            {
                body.SetSeeds(Random.Range(0, int.MaxValue));
                body.SetFeaturesFromSeed();
                body.SetColorFromSeed();
            }
        }
    }
}