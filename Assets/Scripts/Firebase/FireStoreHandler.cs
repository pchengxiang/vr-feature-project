using Firebase.Firestore;
using Firebase.Sample.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FireStoreHandler : MonoBehaviour
{
    private static FireStoreHandler instance;
    public static FireStoreHandler Instance
    {
        get { return instance; }   
    }

    private const int kMaxLogSize = 16382;

    private bool isInitialized;
    public bool IsInitialized
    {
        get
        {
            return isInitialized;
        }
        set
        {
            isInitialized = value;
        }
    }

    // Currently enabled logging verbosity.
    protected Firebase.LogLevel logLevel = Firebase.LogLevel.Info;
    // Whether an operation is in progress.
    protected bool operationInProgress;
    // Cancellation token source for the current operation.
    protected CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private string logText = "";
    private Vector2 scrollViewVector = Vector2.zero;
    // Previously completed task.
    protected Task previousTask;

    [SerializeField]
    GameObject Monitor;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        FirebaseUtils.CheckedAndFixedDependecies(()=>Debug.Log("Firestore has checked Dependencies finish."));
        isInitialized = FirebaseUtils.dependencyStatus == Firebase.DependencyStatus.Available;
    }
    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        Debug.Log(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }

        scrollViewVector.y = int.MaxValue;
    }

    // Render the log output in a scroll view.
    void GUIDisplayLog()
    {
        scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
        GUILayout.Label(logText);
        GUILayout.EndScrollView();
    }


    // Wait for task completion, throwing an exception if the task fails.
    // This could be typically implemented using
    // yield return new WaitUntil(() => task.IsCompleted);
    // however, since many procedures in this sample nest coroutines and we want any task exceptions
    // to be thrown from the top level coroutine (e.g GetKnownValue) we provide this
    // CustomYieldInstruction implementation wait for a task in the context of the coroutine using
    // common setup and tear down code.
    class WaitForTaskCompletion : CustomYieldInstruction
    {
        Task task;
        FireStoreHandler uiHandler;

        // Create an enumerator that waits for the specified task to complete.
        public WaitForTaskCompletion(FireStoreHandler uiHandler, Task task)
        {
            uiHandler.previousTask = task;
            uiHandler.operationInProgress = true;
            this.uiHandler = uiHandler;
            this.task = task;
        }

        // Wait for the task to complete.
        public override bool keepWaiting
        {
            get
            {
                if (task.IsCompleted)
                {
                    //uiHandler.operationInProgress = false;
                    //uiHandler.cancellationTokenSource = new CancellationTokenSource();
                    if (task.IsFaulted)
                    {
                        string s = task.Exception.ToString();
                        uiHandler.DebugLog(s);
                    }
                    return false;
                }
                return true;
            }
        }
    }

    // Cancel the currently running operation.
    protected void CancelOperation()
    {
        if (operationInProgress && cancellationTokenSource != null)
        {
            DebugLog("*** Cancelling operation *** ...");
            cancellationTokenSource.Cancel();
            cancellationTokenSource = null;
        }
    }

    /**
     * Tests a *very* basic trip through the Firestore API.
     */
    //protected IEnumerator GetKnownValue()
    //{
    //    DocumentReference doc1 = db.Collection("col1").Document("doc1");
    //    var task = doc1.GetSnapshotAsync();
    //    yield return new WaitForTaskCompletion(this, task);
    //    if (!(task.IsFaulted || task.IsCanceled))
    //    {
    //        DocumentSnapshot snap = task.Result;
    //        IDictionary<string, object> dict = snap.ToDictionary();
    //        if (dict.ContainsKey("field1"))
    //        {
    //            fieldContents = dict["field1"].ToString();
    //        }
    //        else
    //        {
    //            DebugLog("ERROR: Successfully retrieved col1/doc1, but it doesn't contain 'field1' key");
    //        }
    //    }
    //}


    public void UploadJsonData(string jsonData)
    {
    }

    private static string DictToString(IDictionary<string, object> d)
    {
        return "{ " + d
            .Select(kv => "(" + kv.Key + ", " + kv.Value + ")")
            .Aggregate("", (current, next) => current + next + ", ")
            + "}";
    }

    protected FirebaseFirestore db
    {
        get
        {
            return FirebaseFirestore.DefaultInstance;
        }
    }

    private CollectionReference GetCollectionReference(string path)
    {
        return db.Collection(path);
    }

    public void DownloadModuleData()
    {

    }


}
