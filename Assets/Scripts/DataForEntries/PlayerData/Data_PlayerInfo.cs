using System;

[Serializable]
public class Data_PlayerInfo  {
    public string PlayerFirstName;
    public string PlayerLastName;
    public string PlayerUserName;
    public string PlayerEmail;

    public Data_PlayerInfo()
    {
        PlayerFirstName = "fn";
        PlayerLastName = "ln";
        PlayerUserName = "un";
        PlayerEmail = "em";
    }
    public Data_PlayerInfo(string fn, string ln, string un, string em )
    {
        PlayerFirstName = fn;
        PlayerLastName = ln;
        PlayerUserName = un;
        PlayerEmail = em;
    }
    public override string ToString()
    {
        return "|pinfo= "+ PlayerFirstName + " " + PlayerLastName + " " + PlayerUserName + " " + PlayerEmail + "|";
    }
}
