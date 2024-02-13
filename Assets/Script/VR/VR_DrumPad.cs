using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YARG
{
    public class VR_DrumPad : MonoBehaviour
    {
        private VR_Drums DrumManager;
        public VR_DrumPadType Type;
        public enum VR_DrumPadType
        {
            Kick,

            RedDrum,
            YellowDrum,
            BlueDrum,
            GreenDrum,

            YellowCymbal,
            OrangeCymbal,
            BlueCymbal,
            GreenCymbal
        }
        private void Start()
        {
            DrumManager = FindObjectOfType<VR_Drums>();
            if(DrumManager == null ) { Debug.LogError($"Drums Pad {Type} Failed to find drum master"); }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.GetComponent<VR_DrumStick>())
            {
                if (DrumManager != null)
                {
                    DrumManager.RecivedInput(Type);
                    //play animations and mat change
                }
            }
        }
    }
}
