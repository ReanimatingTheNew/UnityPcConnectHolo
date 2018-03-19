public class UDPInitSctuct  {

    string _myEarPortInCaseIamclient;
    public string GetInternalPort() { return this._myEarPortInCaseIamclient; }

    string _myAudienceIP;
    public string GetExternalIP() { return this._myAudienceIP; }

    string _myAudienceEarPort;
    public string GetExternalPort() { return this._myAudienceEarPort; }

    public UDPInitSctuct(string argInternalport, string argExternalIP, string argExternalPort) {
        _myEarPortInCaseIamclient = argInternalport;
        _myAudienceIP = argExternalIP;
        _myAudienceEarPort = argExternalPort;
    }
}
