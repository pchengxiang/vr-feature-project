using Mirror;

public class PlayerBehaviour : NetworkBehaviour
{
    [SyncVar]
    int playerNo;

    [SyncVar(hook = nameof(OnPlayerDataChanged))]
    public int playerData;
    void OnPlayerDataChanged(int oldPlayerData, int newPlayerData) { }

    public override void OnStartServer() { }
    [ServerCallback]
    void UpdateData() { }

    public override void OnStartClient() { }
    public override void OnStartLocalPlayer() { }
}
