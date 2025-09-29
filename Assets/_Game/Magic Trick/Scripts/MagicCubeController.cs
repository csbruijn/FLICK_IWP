using UnityEngine;

namespace MagicCube { 
    public class MagicCubeController : MonoBehaviour
    {
        private Material m_Material;

        private void Awake()
        {
            m_Material = GetComponent<MeshRenderer>().material;
        }
        public void ChangeCubeColour(Component sender, System.Object newColour)
        {
            if (newColour is not CubeColour)
            {
                Debug.LogError($"ChangeCubeColour received invalid type: {newColour?.GetType().Name ?? "null"}");
                return;
            }

            switch (newColour)
            {
                case CubeColour.red:
                    m_Material.color = Color.red; break;
                case CubeColour.blue:
                    m_Material.color = Color.blue; break;
                case CubeColour.yellow:
                    m_Material.color = Color.yellow; break;
            }
        }
    }
}