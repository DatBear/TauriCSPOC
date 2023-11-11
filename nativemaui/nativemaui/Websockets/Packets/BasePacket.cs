namespace nativemaui.Websockets.Packets;

public class BasePacket<T>
{
    public Guid? Id { get; set; }
    public T Data { get; set; }
}