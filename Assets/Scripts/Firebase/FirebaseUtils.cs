using Firebase;
using Firebase.Database;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class FirebaseUtils 
{
    public static DependencyStatus dependencyStatus;

    public static FirebaseDatabase database = FirebaseDatabase.DefaultInstance;
    public static FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;



    public static void CheckedAndFixedDependecies(UnityAction completed)
    {
        if(dependencyStatus == DependencyStatus.Available)
            completed();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                completed();
                Debug.Log("Firebase dependencies checked.");
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
            }
        });
    }
}
