using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


#region Emailstuff
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
#endregion
public class SystemManager : MonoBehaviour
{
   
    private string user;
    private string password;
    private string email;
   
    public string serverText;
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject forgotPasswordPannel;
    public GameObject forgotpasswordemail;
    public GameObject emailCodePanel;
    public GameObject newPasswordPanel;
    public GameObject createUserPanel;
    [Header("CreateUser")]
    public InputField createUsername;
    public InputField createPassword;
    public InputField createEmail;
    [Header("LoginUser")]
    public InputField loginUsername;
    public InputField loginPassword;
    [Header("ResetUser")]
    public InputField resetPassword;
    public InputField resetEmail;
    public InputField resetUsername;
    [Header("ErrorText")]
    public TextMeshProUGUI LoginError;
    public TextMeshProUGUI createError;
    public TextMeshProUGUI resetError;
    

    #region Create
    IEnumerator CreateUser(string username, string password, string email)
    {
        string createUserURL = "http://localhost/nsirpg/insertuser.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("email", email);
        UnityWebRequest webRequest = UnityWebRequest.Post(createUserURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
    }

    public void CreateNewUser()
    {
        StartCoroutine(CreateUser(createUsername.text, createPassword.text, createEmail.text));
        user = createUsername.text;
        password = createPassword.text;
        email = createEmail.text;
    }
    #endregion
    #region Login
    IEnumerator LoginUser(string username, string password)
    {
        string loginUserURL = "http://localhost/nsirpg/Login.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        UnityWebRequest webRequest = UnityWebRequest.Post(loginUserURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        serverText = webRequest.downloadHandler.text;
    }
    public void LoginUser()
    {
        StartCoroutine(LoginUser(loginUsername.text, loginPassword.text));
        if (serverText == "Login Successful")
        {
            SceneManager.LoadScene(1);
        }
        if (serverText == "Incorrect Password")
        {
            LoginError.alpha = 255;
            LoginError.text = serverText;
        }
    }
    #endregion
    #region ForgotPasscode
    IEnumerator ForgotPassword( InputField email)
    {
        string ForgotURL = "http://localhost/nsirpg/Email.php";
       
        WWWForm form = new WWWForm();
        form.AddField("email_Post", email.text);
        UnityWebRequest webRequest = UnityWebRequest.Post(ForgotURL, form);
       
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        if (webRequest.downloadHandler.text == "User Not Found")
        {
            resetError.alpha = 255;
            resetError.text = webRequest.downloadHandler.text;
        }
        else
        {

            user = webRequest.downloadHandler.text;
            SendEmail(email);
        }
    }
    public void ForgotPassword()
    {
        StartCoroutine(ForgotPassword(resetEmail));
        if (serverText == "Login Successful")
        {
            Debug.Log("you did it");
        }
        if (serverText == "Incorrect Password")
        {
            Debug.Log("you didn't it");
        }
    }
    #endregion
    #region Email
  
    public void SendEmail(InputField email)
    {
       
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("sqlunityclasssydney@gmail.com");
        mail.To.Add(email.text);
        mail.Subject = "NSIRPG Password Reset";
        mail.Body = "Hello Bingus Boingus " + user + "\nReset using this code: " + code;

        //connect to google
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        //ne able to send through ports
        smtpServer.Port = 25; //numbors
        //login to google
        smtpServer.Credentials = new NetworkCredential("sqlunityclasssydney@gmail.com", "sqlpassword") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        { return true; };
        //send message
        smtpServer.Send(mail);
        Debug.Log("Sending Email");

    }
    #endregion
    public void CorrectCode(InputField codefield)
    {
        if (codefield.text == code)
        {
            Debug.Log("yes this is correct");
            emailCodePanel.SetActive(false);
            newPasswordPanel.SetActive(true);
        }
        else { Debug.Log("no this is not correct"); }
       //either make button that resends email or send them back to previous panel
    }
    
   IEnumerator ConfirmPassword(InputField newPassword1, InputField username)
    {
        string confirmPasswordURL = "http://localhost/nsirpg/updatepassword.php";
        WWWForm form = new WWWForm();

        form.AddField("password_Post", newPassword1.text);
        form.AddField("username_Post", username.text);
        UnityWebRequest webRequest = UnityWebRequest.Post(confirmPasswordURL, form);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.downloadHandler.text);
        if (newPassword1.text == "")
        {
            Debug.Log("Please Enter your new password the field");
        }
        if (newPassword1.text != "")
        {
            Debug.Log("gongrats you made your new password");
            password = newPassword1.text;
           
            newPasswordPanel.SetActive(false);
            forgotPasswordPannel.SetActive(false);
            forgotpasswordemail.SetActive(true);
            loginPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Error");
        }
    }
    public void ConfirmNewPassword( )
    {
        StartCoroutine(ConfirmPassword(resetPassword,resetUsername));
    }



    #region generators
    #region mydumbgenerator


    //public string[] lettersLowerCase = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    //public string[] lettersUpperCase = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    //public int letterNum;
    //public int numbers;
    //public int chance;
    //public string fullCode;
    //public void CodeGen()
    //{
    //    fullCode = "";
    //    for (int i = 0; i < 26; i++)
    //    {
    //        chance = UnityEngine.Random.Range(0, 2);
    //        numbers = UnityEngine.Random.Range(0, 10);
    //        letterNum = UnityEngine.Random.Range(0, 27);
    //        fullCode += numbers;         
    //    }
    //    Debug.Log(fullCode);
    //}
    #endregion
    #region jamieusingbraingenerator
    
    private string characters = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*";
    private string code = "";
    public void CreateCode()
    {
        code = "";
        for (int i = 0; i < 10; i++)
        {
            int a = UnityEngine.Random.Range(0, characters.Length);
            code = code + characters[a];
        }
        Debug.Log(code);
    }
    #endregion
    #endregion


}
