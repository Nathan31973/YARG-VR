using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YARG
{
    public class VR_Smooth_SpecCam : MonoBehaviour
    {
        private Camera m_Camera;
        [SerializeField]private Transform target;

        [Range(0, 1)]
        [SerializeField] private float positionDamping;
        [Range(0, 1)]
        [SerializeField] private float rotationDamping;


        // Start is called before the first frame update
        void OnEnable()
        {
            m_Camera = gameObject.GetComponent<Camera>();
            transform.position = target.position;
            transform.rotation = target.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, positionDamping);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotationDamping);
        }
    }
}
