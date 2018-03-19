
using System;
[Serializable]
public class Data_PlayerPoints  {
    public int score;
    public int headshots;
    public int streakcount;
    public int maxstreak;
    public int kills;
    public int miss;
    public int totalshots;
    public int deaths;
    public int pointslost;
    public int numberofReloads;
    public int wavessurvived;
    public Data_PlayerPoints()
    {
        score = 0;
        headshots = 1;
        streakcount = 0;
        maxstreak = 0;
        kills = 1;
        miss = 1;
        totalshots = 1;
        deaths = 0;
        pointslost = 1;
        numberofReloads = 1;
        wavessurvived = 1;
    }
    public Data_PlayerPoints(int sc, int hs, int st, int ms, int ki, int mi, int ts,int dt,int pl, int nr, int ws) {
         score=sc;
         headshots=hs;
         streakcount=st;
         maxstreak=ms;
         kills=ki;
         miss=mi;
         totalshots=ts;
        deaths = dt;
        pointslost = pl;
        numberofReloads = nr;
        wavessurvived = ws;
    }

    public override string ToString()
    {
        return  " |score= " + score +
                " |headshots " + headshots +
                " |streakcount " + streakcount +
                " |maxstreak " + maxstreak +
                " |kills " + kills +
                " |miss " + miss +
                " |totalshots " + totalshots +
                " |deaths " + deaths +
                " |pointslost " + pointslost +
                " |reloads " + numberofReloads +
                " |waves " + wavessurvived +
                " |";
    }
 

}
