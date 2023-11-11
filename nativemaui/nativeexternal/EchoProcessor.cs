using System.Diagnostics;
using nativecore;

namespace nativeexternal;

[Processor]
public class EchoProcessor
{
    [Command]
    public string Echo(string str)
    {
        Debug.WriteLine(str);
        return str;
    }
}