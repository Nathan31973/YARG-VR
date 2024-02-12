using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using YARG.Gameplay.Player;

namespace YARG
{
    public class VrManager : MonoBehaviour
    {
        [HideInInspector] public static VrManager instance;
        public Transform UiVrLocation;
        public bool VrEnable = false;
        public GameObject XRPlayerControllerPrefab;
        private GameObject XRPlayer;
        public Canvas bootloaderCanvas;

        private void Awake()
        {
            if (instance == null)
            {
                SceneManager.sceneLoaded += VRFixOnSceneLoad;
                instance = this;
            }
            else
            {
                Destroy(this);
                Debug.Log("[VR Manager] Only 1 VR Manager can be loaded");
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        public void ToggleVR(bool enable)
        {
            if (enable)
            {
                EnableVR();
            }
            else
            {
                DisableVR();
            }
        }

        //startVR
        public void EnableVR()
        {
            VrEnable = true;
            try
            {


                XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                XRSettings.enabled = true;
                Debug.Log("[VRManager] Starting VR");
                XRPlayer = Instantiate(XRPlayerControllerPrefab);
                UpdateCanvas();

            }
            catch
            {

            }

        }
        public void DisableVR()
        {

            VrEnable = false;
            try
            {
                XRSettings.enabled = false;
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
                Debug.Log("[VRManager] Stopping VR");
                //destory the XR prefab
                Destroy(XRPlayer);
                UpdateCanvas();

            }
            catch
            {

            }
        }

        void VRFixOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            UpdateCanvas();
            if (XRPlayer == null && VrEnable)
            {
                XRPlayer = Instantiate(XRPlayerControllerPrefab);
            }
        }

        public void SetupGameplayVR()
        {
            if (!VrEnable) { return; }
            StartCoroutine(cameraSetup());
        }

        private IEnumerator cameraSetup()
        {
            yield return new WaitForSeconds(1);
            UpdateCanvas();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);


                // Check if the GameObject has a Camera component and its name is "Venue Camera"
                Camera[] cameras = FindObjectsOfType<Camera>();
                Debug.LogError("Cameras found" + cameras.Length);
                foreach (Camera camera in cameras)
                {
                    Debug.LogError("Camera found in scene " + scene.name + ": " + camera.gameObject.name);
                    if (camera.gameObject.name == "Venue Camera")
                    {
                        // Do something with the camera
                        Debug.LogError("Venue Camera found in scene " + scene.name + ": " + camera.gameObject.name);
                        camera.gameObject.SetActive(false);
                    }
                }
                var highway = FindObjectOfType<FiveFretPlayer>();
                if(highway != null)
                {
                    highway.transform.rotation = Quaternion.Euler(-15,0,0);
                }

            }
            XRPlayer.transform.position = new Vector3(0, 100, -5f);

            yield return null;
        }

        public void UpdateCanvas()
        {

            //getting all scenes
            Scene[] scenes = new Scene[SceneManager.sceneCount];
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes[i] = SceneManager.GetSceneAt(i);
            }

            // Iterate through all loaded scenes
            foreach (Scene s in scenes)
            {
                if (s.isLoaded) // Check if the scene is loaded
                {
                    Canvas[] canvas = FindObjectOfType<Canvas>().GetComponents<Canvas>();
                    foreach (Canvas c in canvas)
                    {
                        if (VrEnable)
                        {
                            //Debug.LogError("Canvas name " + c.name);
                            c.renderMode = RenderMode.WorldSpace;
                            RectTransform rectTransform = c.GetComponent<RectTransform>();
                            rectTransform.position = new Vector3(UiVrLocation.position.x, UiVrLocation.position.y, UiVrLocation.position.z);
                            rectTransform.sizeDelta = new Vector2(1920, 1080);
                            rectTransform.localScale = UiVrLocation.localScale;

                            //dumb hack to get bootloader canvas to setup in vr
                            bootloaderCanvas.renderMode = RenderMode.WorldSpace;
                            rectTransform = bootloaderCanvas.GetComponent<RectTransform>();
                            rectTransform.position = new Vector3(UiVrLocation.position.x, UiVrLocation.position.y, UiVrLocation.position.z);
                            rectTransform.sizeDelta = new Vector2(1920, 1080);
                            rectTransform.localScale = UiVrLocation.localScale;
                        }
                        else
                        {
                            c.renderMode = RenderMode.ScreenSpaceOverlay;
                        }
                    }
                    //ebug.LogError($"Canvas Found {canvas.Length}");

                    //finding any main cams and dissable then unless it the XR camera
                    //fetching XR camera
                    if (XRPlayer != null)
                    {
                        GameObject xrRig = FindObjectOfType<XROrigin>().gameObject;

                        Camera[] allCameras = FindObjectsOfType<Camera>();
                        foreach (Camera c in allCameras)
                        {
                            if (c != xrRig.GetComponentInChildren<Camera>())
                            {
                                c.gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        Camera[] allCameras = FindObjectsOfType<Camera>();
                        foreach (Camera c in allCameras)
                        {

                            c.gameObject.SetActive(true);

                        }
                    }
                }
            }



        }

        //TODO
        //ON Scene load Look for all canvas and set them to world space
        //Spawn the XR Prefab on scene load

        // Update is called once per frame
        private void OnApplicationQuit()
        {
            if(VrEnable)
            {
                ToggleVR(false);
            }
        }
    }
}
