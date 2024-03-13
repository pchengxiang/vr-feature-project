using Firebase.Extensions;
using Firebase.Storage;
using RobinBird.FirebaseTools.Storage.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class StorageLoader
{

    // Get a reference to the storage service, using the default Firebase App
    static FirebaseStorage storage = FirebaseStorage.DefaultInstance;

    // Create a storage reference from our storage service
    public static StorageReference StorageRef =
        storage.GetReferenceFromUrl("gs://gs://featureproject-aa73c.appspot.com/");

    //static StorageReference imagesRef = storageRef.Child("images");

    public static StorageReference PrefabsRef = StorageRef.Child("prefabs");

    const long maxAllowedSize = 1 * 1024 * 1024;

    public delegate void GainUriAfter(Uri uri);


  
    static public void UploadFile(StorageReference reference, string filePath)
    {
        var task = reference
    .PutFileAsync(filePath, null,
        new StorageProgress<UploadState>(state => {
            // called periodically during the upload
            Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.",
                state.BytesTransferred, state.TotalByteCount));
        }), CancellationToken.None, null);

        task.ContinueWithOnMainThread(resultTask => {
            if (!resultTask.IsFaulted && !resultTask.IsCanceled)
            {
                Debug.Log("Upload finished.");
            }
        });
    }

    public static void DownloadFile(StorageReference reference, string localUrl)
    {
        reference.GetFileAsync(localUrl).ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("File downloaded.");
            }
        });
    }

    static void GetURL(StorageReference reference,GainUriAfter doSomething )
    {
        // Fetch the download URL
        reference.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("Download URL: " + task.Result);
                doSomething(task.Result);
                // ... now download the file via WWW or UnityWebRequest.
            }
        });
    }

    static void DownloadBytes(StorageReference reference)
    {
        reference.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogException(task.Exception);
                // Uh-oh, an error occurred!
            }
            else
            {
                byte[] fileContents = task.Result;
                Debug.Log("Finished downloading!");
            }
        });
    }

    static void DownloadFromStream(StorageReference reference)
    {
        // Download via a Stream
        reference.GetStreamAsync(stream => {
            // Do something with the stream here.
            //
            // This code runs on a background thread which reduces the impact
            // to your framerate.
            //
            // If you want to do something on the main thread, you can do that in the
            // progress eventhandler (second argument) or ContinueWith to execute it
            // at task completion.
        }, null, CancellationToken.None);
    }

}
