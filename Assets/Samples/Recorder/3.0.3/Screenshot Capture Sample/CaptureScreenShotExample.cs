#if UNITY_EDITOR

using System.IO;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

namespace UnityEngine.Recorder.Examples
{
    /// <summary>
    /// This example shows how to set up a recording session via script.
    /// To use this example, add the CaptureScreenShotExample component to a GameObject.
    ///
    /// Entering playmode will display a "Capture ScreenShot" button.
    ///
    /// Recorded images are saved in [Project Folder]/SampleRecordings
    /// </summary>
    public class CaptureScreenShotExample : MonoBehaviour
    {
        RecorderController m_RecorderController;

        void OnEnable()
        {
            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            m_RecorderController = new RecorderController(controllerSettings);

            var mediaOutputFolder = Path.Combine(Application.streamingAssetsPath, "SampleRecordings");

            // Image
            var imageRecorder = ScriptableObject.CreateInstance<ImageRecorderSettings>();
            imageRecorder.name = "My Image Recorder";
            imageRecorder.Enabled = true;
            imageRecorder.OutputFormat = ImageRecorderSettings.ImageRecorderOutputFormat.PNG;
            imageRecorder.CaptureAlpha = false;

            imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, "image_") + DefaultWildcard.Take;

            imageRecorder.imageInputSettings = new GameViewInputSettings
            {   
                OutputWidth = 3840,
                OutputHeight = 2160,
            };

            // Setup Recording
            controllerSettings.AddRecorderSettings(imageRecorder);
            controllerSettings.SetRecordModeToSingleFrame(0);
        }

        private void FixedUpdate()
        {
            if(Input.GetKey(KeyCode.I))
            {
                m_RecorderController.PrepareRecording();
                m_RecorderController.StartRecording();
            }
        }
    }
}

#endif
