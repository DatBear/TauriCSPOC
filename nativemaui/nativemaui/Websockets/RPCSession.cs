using NetCoreServer;
using Newtonsoft.Json;
using System.Text;
using nativemaui.Websockets.Packets;
using Newtonsoft.Json.Linq;

namespace nativemaui.Websockets;

public class RPCSession : WsSession
{
    private readonly AssemblyLoader _loader;
    public RPCSession(WsServer server, AssemblyLoader loader) : base(server)
    {
        _loader = loader;
    }

    public bool Send<T>(T? obj) where T : IResponsePacket
    {
        if (obj == null) return false;
        return SendTextAsync(JsonConvert.SerializeObject(obj));
    }

    public override void OnWsReceived(byte[] buffer, long offset, long size)
    {
        var message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);

        if (message == null)
        {
            return;
        }

        var packet = JsonConvert.DeserializeObject<BaseRequestPacket<JObject>>(message);

        var response = _loader.InvokeFromAssembly(packet);
        Send(response);

    }
}