using System.Reflection;
using nativecore;
using nativemaui.Websockets;
using nativemaui.Websockets.Data;
using nativemaui.Websockets.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace nativemaui;

public class AssemblyLoader
{
    private List<MethodInfo> _methods = new();

    public void Load(string path)
    {
        var assembly = Assembly.LoadFrom(path);
        var processors = assembly.GetTypes().Where(x => Attribute.IsDefined(x, typeof(ProcessorAttribute)));
        var requests = processors.SelectMany(x => x.GetMethods().Where(x => Attribute.IsDefined(x, typeof(CommandAttribute))));
        _methods.AddRange(requests);
    }

    public IResponsePacket InvokeFromAssembly(BaseRequestPacket<JObject> packet)
    {
        var method = _methods.FirstOrDefault(x => x.Name == packet.Command);
        if (method == null)
        {
            return new ErrorResponse
            {
                Id = packet.Id,
                Data = new ErrorData
                {
                    Message = $"command not found: {packet.Command}",
                    ErrorCode = ErrorCode.CommandNotFound
                }
            };
        }

        var instance = Activator.CreateInstance(method.DeclaringType!);
        var parameters = method.GetParameters().Select(x => Convert.ChangeType(packet.Data[x.Name!], x.ParameterType));
        var responseData = method?.Invoke(instance, parameters.ToArray());

        return new BaseResponsePacket<dynamic>
        {
            Id = packet.Id,
            Data = responseData
        };
    }

}