using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinUI3Sample.Services;
public static class KSLocalSettingService
{
    static ApplicationDataContainer lst = ApplicationData.Current.LocalSettings;
    public static bool IsContainerExist(string container)
    {
        return lst.Containers.ContainsKey(container);
    }
    public static void CreatContainer(string container)
    {
        lst.CreateContainer(container, ApplicationDataCreateDisposition.Always);
    }
    public static bool IsKeyExist(string container,string key)
    {
        return lst.Containers[container].Values.ContainsKey(key);
    }
    public static void SaveSetting(string container,string key,string value)
    {
        lst.Containers[container].Values[key] = value;
    }
    public static string ReadSetting(string container, string key)
    {
        var t = lst.Containers[container].Values[key].ToString();
         return t==null?"":t;
    }
}
