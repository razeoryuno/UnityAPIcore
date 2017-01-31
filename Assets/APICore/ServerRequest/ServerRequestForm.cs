using System.Collections.Generic;

public abstract class ServerRequestForm
{
    public Dictionary<string, string> headers { get; set; }
    public JSONObject fields { get; private set; }
    public string methodName { get; set; }

    public ServerRequestForm()
    {
        headers = new Dictionary<string, string>();
        fields = new JSONObject();
    }

    #region Fields Help
    public virtual void AddField(string name, bool val) { fields.AddField(name, val); }
    public virtual void AddField(string name, float val) { fields.AddField(name, val); }
    public virtual void AddField(string name, int val) { fields.AddField(name, val); }
    public virtual void AddField(string name, string val) { fields.AddField(name, val); }
    public virtual void AddField(string name, JSONObject obj) { fields.AddField(name, obj); }
    #endregion
}
