using Microsoft.BizTalk.SSOClient.Interop;
using System;
using System.Diagnostics;

namespace Microsoft.SSO.Utility
{
  public static class SSOConfigHelper
  {
    private static string idenifierGUID = "ConfigProperties";

    public static string Read(string appName, string propName)
    {
      try
      {
        SSOConfigStore ssoConfigStore = new SSOConfigStore();
        ConfigurationPropertyBag configurationPropertyBag = new ConfigurationPropertyBag();
        ((ISSOConfigStore) ssoConfigStore).GetConfigInfo(appName, SSOConfigHelper.idenifierGUID, 4, (IPropertyBag) configurationPropertyBag);
        object ptrVar = (object) null;
        configurationPropertyBag.Read(propName, out ptrVar, 0);
        return (string) ptrVar;
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
        throw;
      }
    }
  }
}
