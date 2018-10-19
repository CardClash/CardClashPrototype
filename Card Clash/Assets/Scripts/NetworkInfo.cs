using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkInfo
{
    private static bool host;
    private static string ipAddress;

    public static bool Host
    {
        get { return host; }
        set { host = value; }
    }

    public static string IP
    {
        get { return ipAddress; }
        set { ipAddress = value; }
    }
}
