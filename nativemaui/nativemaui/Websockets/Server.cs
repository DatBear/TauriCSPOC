using System.Net;
using NetCoreServer;

namespace nativemaui.Websockets;

public class Server : WsServer
{
    private readonly AssemblyLoader _loader;
    public Server(IPAddress address, int port, AssemblyLoader loader) : base(address, port)
    {
        _loader = loader;
    }

    protected override TcpSession CreateSession()
    {
        return new RPCSession(this, _loader);
    }
}