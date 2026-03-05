
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game
{
    public class CameraMgr
    {
        public static Camera MainCamera {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return mainCamera; } 
        }

        public static Transform CameraTrans{
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return mainCamera.transform; }
        }

        public static Transform CameraRoot {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return mainCamera.transform.parent; } 
        } 

        private static Camera mainCamera = null;
        private static Camera newMainCamera = null;
        public static void ReFindMainCamera() {

            //return;

            //GameObject mainCameraObj = GameObject.Find("MainCamera");
            //if (mainCameraObj != null)
            //    newMainCamera = mainCameraObj.GetComponent<Camera>();
            //else
            newMainCamera = UnityGameEntry.Instance.MainCamera;

            if (mainCamera != null)
                mainCamera.enabled = false;

            mainCamera = newMainCamera;

            AddStackCamera(UnityGameEntry.Instance.UICamera);

            mainCamera.enabled = true;
        }

        public static void AddStackCamera(Camera camera) {
            List<Camera> cameraStack = mainCamera.GetUniversalAdditionalCameraData().cameraStack;
            if (cameraStack.Contains(camera))
                return;
            cameraStack.Add(camera);
        }
    }
}
