
using System;
[Serializable]
public class Data_PlayerSession  {
    public string SerrionTime;
    public Data_PlayerInfo PInfo;
    public Data_PlayerPoints PPoints;
    public Data_PlayerSession()
    {
        SerrionTime = SessionTimeStampFormatter(DateTime.Now);
        PInfo =new Data_PlayerInfo();
        PPoints = new Data_PlayerPoints();
    }
    public Data_PlayerSession(DateTime dt, Data_PlayerInfo argPInfo, Data_PlayerPoints argPPoints) {
        SerrionTime = SessionTimeStampFormatter(dt);
        PInfo = argPInfo;
        PPoints = argPPoints;
    }

    string SessionTimeStampFormatter(DateTime argDatetime) {
        //'s' is "Sortable" date format. Output looks like 2008-04-10T06:30:00
        return "sessiontime=" + argDatetime.ToString("s");
    }
}
