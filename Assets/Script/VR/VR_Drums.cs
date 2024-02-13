using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static YARG.VR_DrumPad;

namespace YARG
{
    public class VR_Drums : MonoBehaviour
    {
        public VR_DrumPad[] vrDrumspad;
        public VR_DrumStick[] vrDrumsStick;
        // Start is called before the first frame update
        void Start()
        {
            vrDrumspad = FindObjectsOfType<VR_DrumPad>();
            vrDrumsStick = FindObjectsOfType<VR_DrumStick>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void RecivedInput(VR_DrumPadType drumnote)
        {
            switch (drumnote)
            {
                case VR_DrumPadType.GreenDrum:
                    
                    break;
                default:
                    // code block
                    break;
            }
        }
    }
}
