 
public class SegmentBools  {

    private bool _sbHasStarted;
    public bool Has_Started
    {
        get { return _sbHasStarted; }
        set { _sbHasStarted = value; }
    }

    private bool _sbHasFinished;
    public bool Has_Finished
    {
        get { return _sbHasFinished; }
        set { _sbHasFinished = value; }
    }

    public SegmentBools(bool argStarted, bool argFinished)
    {
        _sbHasStarted = argStarted; _sbHasFinished = argFinished;

    }
}
