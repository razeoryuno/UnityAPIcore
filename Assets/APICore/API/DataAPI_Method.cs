using System.IO;
using UnityEngine;

public abstract class DataAPI_Method : API_Method
{
#if UNITY_IOS && !UNITY_EDITOR
    private static string folderPath =  "private/" + Application.persistentDataPath + "/Data/";
#else
    private static string folderPath = Application.persistentDataPath + "/Data/";
#endif

    static DataAPI_Method()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
    }

    public static string hashKey = "";

    protected abstract string fileName { get; }

    public string filePath
    {
        get { return folderPath + fileName + ".txt"; }
    }

    public string localSaveJsonString { get; private set; }
    public string jsonHash { get; private set; }

    public DataAPI_Method()
    {
        if (File.Exists(filePath))
            localSaveJsonString = File.ReadAllText(filePath);
        else
            localSaveJsonString = string.Empty;

        jsonHash = SHA_Encryptor.EncryptSHA256(localSaveJsonString, hashKey);
    }

    public override void AddDefaultToServerRequestForm(ServerRequestForm serverRequestForm)
    {
        base.AddDefaultToServerRequestForm(serverRequestForm);
        serverRequestForm.AddField("hash", jsonHash);
    }

    public override void ValidateServerRequestForm(ServerRequestForm serverRequestForm)
    {
        base.ValidateServerRequestForm(serverRequestForm);
        ValidateServerRequestFormByKey(serverRequestForm, "hash");
    }

    protected override void DataParser(JSONObject jsonObject)
    {
        int code = API_Helper.GetCode(jsonObject);
        string message = API_Helper.GetMessage(jsonObject);
        JSONObject result = null;

        if (API_Code.CodeIsNoContentUpdated(code))
        {
            var localSaveJson = new JSONObject(localSaveJsonString);
            result = API_Helper.GetResult(localSaveJson);
        }
        else if (jsonObject.HasField("result"))
        {
            result = API_Helper.GetResult(jsonObject);

            if (API_Code.CodeIsNormalSuccess(code))
            {
                var fileStream = File.CreateText(filePath);
                fileStream.Write(jsonObject.Print());
                fileStream.Close();
            }
        }

        OnRequestDataSuccess(code, message, result);
    }
}
