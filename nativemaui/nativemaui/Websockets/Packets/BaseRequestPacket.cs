namespace nativemaui.Websockets.Packets;

public class BaseRequestPacket<T> : BasePacket<T>, IRequestPacket
{
    public string Command { get; set; }
}