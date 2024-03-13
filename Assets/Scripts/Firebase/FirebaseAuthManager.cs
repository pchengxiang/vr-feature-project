using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.Events;
using System;
using static PageManager;

public class FirebaseAuthManager : MonoBehaviour
{
    static FirebaseAuthManager m_instance;
    public static FirebaseAuthManager instance
    {
        get
        {
            return m_instance;
        }
    }

    PageManager pages;



    // Firebase variable
    [Header("Firebase")]

    public FirebaseAuth auth;
    public FirebaseUser user;

    // Login Variables
    [Space]
    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;
    public Button LoginButton;
    public Button JumpToRegisterButton;
    public Button ForgetPasswordButton;
    [SerializeField]
    public UnityEvent LoginInSuccessEvent= new();
    // Registration Variables
    [Space]
    [Header("Registration")]
    public InputField nameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField confirmPasswordRegisterField;
    public Button RegisterButton;
    public Button JumpToLoginButton;
    [SerializeField]
    public UnityEvent RegisterSuccessEvent = new();

    [Space]
    [Header("User Information")]
    public Text DisplayNameText;
    public Text EmailText;
    public Button LogoutButton;

    [Space]
    [Header("Reset Password")]
    public InputField emailForResetField;
    public Button ResetPasswordButton;
    [SerializeField]
    public UnityEvent ResetPasswordSuccessEvent = new();

    [Space]
    [Header("Global Operation")]
    public Text FaultMessage;
    public Button BackToOriginalPage;

    private void Awake()
    {
        m_instance = this;
        // Check that all of the necessary dependencies for firebase are present on the system
        FirebaseUtils.CheckedAndFixedDependecies(() =>
        {
            InitializeFirebaseAuth();
        });
        pages = PageManager.instance;
        LoginButton.onClick.AddListener(Login);
        RegisterButton.onClick.AddListener(Register);
        LogoutButton.onClick.AddListener(Logout);
        ForgetPasswordButton.onClick.AddListener(() => pages.JumpToPage(Page.ResetPasswordPage));
        JumpToLoginButton.onClick.AddListener(()=> pages.JumpToPage(Page.LoginPage));
        JumpToRegisterButton.onClick.AddListener(() => pages.JumpToPage(Page.RegisterPage));
    }

    void InitializeFirebaseAuth()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnAuthStateChanged;
        OnAuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void OnAuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }
    #region Login Page
    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "Login Failed! Because ";
            GetFaultMessage(ref failedMessage, authError);
            FaultMessage.text = failedMessage;
        }
        else
        {
            LoginInSuccessEvent.Invoke();
            user = loginTask.Result;
            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);
            GetUserInformation();
        }
    }
    #endregion
    #region Reset Password Page
    public async void ForgetPassword()
    {
        var email = emailForResetField.text;
        var task = auth.SendPasswordResetEmailAsync(email);
        await task;

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);

            FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Reset Failed! Because ";
            GetFaultMessage(ref failedMessage, authError);
            FaultMessage.text = failedMessage;
        }
        else
        {
            Debug.Log("Reset password completely.");
            pages.JumpToPage(pages.mainPage);
        }  
        
    }
    #endregion
    #region Register Page
    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            FaultMessage.text = "User Name is empty";
            Debug.LogError(FaultMessage.text);
             
        }
        else if (email == "")
        {
            FaultMessage.text = "email field is empty";
            Debug.LogError(FaultMessage.text);
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            FaultMessage.text = "Password does not match";
            Debug.LogError(FaultMessage.text);
        }
        else if (password.Length < 6)
        {
            FaultMessage.text = "Password length must equal or more than six.";
            Debug.LogError(FaultMessage.text);
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                GetFaultMessage(ref failedMessage, authError);
                FaultMessage.text = failedMessage;
                Debug.LogError(failedMessage);
            }
            else
            {
                RegisterSuccessEvent.Invoke();
                // Get The User After Registration Success
                user = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;
                    string failedMessage = "Profile update Failed! Becuase ";
                    GetFaultMessage(ref failedMessage, authError);
                    FaultMessage.text = failedMessage;
                    Debug.LogError(failedMessage);
                }
                else
                {
                    Debug.Log("Registration Sucessful Welcome " + user.DisplayName);
                    pages.JumpToPage(pages.mainPage);
                }
            }
        }
    }
    #endregion

    #region User Information Page
    public void Logout()
    {
        auth.SignOut();
        pages.JumpToPage(pages.mainPage);
    }

    public void GetUserInformation()
    {
        if(user != null)
        {
            DisplayNameText.text = user.DisplayName;
            EmailText.text = user.Email;
        }
    }

    public void AccountValidate()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Fault Message
    public void GetFaultMessage(ref string failedMessage,AuthError error)
    {
        switch (error)
        {
            case AuthError.InvalidEmail:
                failedMessage += "Email is invalid.";
                break;
            case AuthError.WrongPassword:
                failedMessage += "Wrong Password.";
                break;
            case AuthError.MissingEmail:
                failedMessage += "Email is missing.";
                break;
            case AuthError.MissingPassword:
                failedMessage += "Password is missing.";
                break;
            case AuthError.UserNotFound:
                failedMessage += "Email is not match any user.";
                break;
            default:
                failedMessage += "Something error.";
                break;
        }
    }
    #endregion
}
