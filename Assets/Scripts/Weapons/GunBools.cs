

public class GunBools  {

 


    private bool _bIsRealoading;
    public bool ThisGunIsReloading
    {
        get { return _bIsRealoading; }
        set { _bIsRealoading = value; }
    }

    private bool _bCAnAcceptCLip;
    public bool CanAcceptNewClip
    {
        get { return _bCAnAcceptCLip; }
        set { _bCAnAcceptCLip = value; }
    }

    private bool _bCAnManuallyDropMag;
    public bool CAnManuallyDropMag
    {
        get { return _bCAnManuallyDropMag; }
        set { _bCAnManuallyDropMag = value; }
    }

    public GunBools( bool isreleading, bool canaccept, bool candropmag) {
       _bIsRealoading = isreleading; _bCAnAcceptCLip = canaccept; _bCAnManuallyDropMag = candropmag;

    }

}
