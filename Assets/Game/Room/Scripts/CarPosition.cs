using UnityEngine;

namespace Aoi
{
    public class CarPosition : MonoBehaviour
    {
        
        [SerializeField] private bool m_showGizmo = true;
        [SerializeField] private Color m_gizmoColor = Color.cyan;
        [SerializeField] private Vector3 m_gizmoSize = new Vector3(2f, 1f, 4f); // ïù, çÇÇ≥, í∑Ç≥


        private void OnDrawGizmos()
        {
            if (!m_showGizmo)
                return;

            
            Color gizmoColorWithAlpha = m_gizmoColor;
            gizmoColorWithAlpha.a = 0.3f;
            Gizmos.color = gizmoColorWithAlpha;

            // åªç›ÇÃTransformÇÃà íuÇ∆âÒì]Çégóp
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;

           
            Gizmos.DrawCube(Vector3.zero, m_gizmoSize);

            
            Gizmos.color = m_gizmoColor;
            Gizmos.DrawWireCube(Vector3.zero, m_gizmoSize);

            Gizmos.matrix = Matrix4x4.identity;
        }

        private void OnDrawGizmosSelected()
        {
            
            if (!m_showGizmo)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
    } 
}