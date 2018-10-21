using Microsoft.BizTalk.SSOClient.Interop;
using System.Collections.Specialized;

namespace Microsoft.SSO.Utility
{
  public class ConfigurationPropertyBag : IPropertyBag
  {
    private HybridDictionary properties;

    internal ConfigurationPropertyBag()
    {
      this.properties = new HybridDictionary();
    }

    public void Read(string propName, out object ptrVar, int errLog)
    {
      ptrVar = this.properties[(object) propName];
    }

    public void Write(string propName, ref object ptrVar)
    {
      this.properties.Add((object) propName, ptrVar);
    }

    public bool Contains(string key)
    {
      return this.properties.Contains((object) key);
    }

    public void Remove(string key)
    {
      this.properties.Remove((object) key);
    }
  }
}
