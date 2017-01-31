using UnityEngine;

public class WWWRequestForm : ServerRequestForm
{
    public WWWForm wwwForm { get; private set; }

    public WWWRequestForm() : base()
    {
        headers.Add("Content-Type", "application/json");
        wwwForm = new WWWForm();
    }

    #region Fields Help
    public override void AddField(string name, bool val)
    {
        wwwForm.AddField(name, val.ToString());
        base.AddField(name, val);
    }

    public override void AddField(string name, float val)
    {
        wwwForm.AddField(name, val.ToString());
        base.AddField(name, val);
    }

    public override void AddField(string name, int val)
    {
        wwwForm.AddField(name, val.ToString());
        base.AddField(name, val);
    }

    public override void AddField(string name, string val)
    {
        wwwForm.AddField(name, val);
        base.AddField(name, val);
    }

    public override void AddField(string name, JSONObject obj)
    {
        wwwForm.AddField(name, obj.Print());
        base.AddField(name, obj);
    }
    #endregion
}
