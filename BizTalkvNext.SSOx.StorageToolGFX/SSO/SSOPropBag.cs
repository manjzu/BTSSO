


using Microsoft.EnterpriseSingleSignOn.Interop;
using System.Collections.Specialized;

namespace BizTalkvNext.SSOx
{
  public class SSOPropBag : IPropertyBag
    {
    internal HybridDictionary properties;

    public int PropertyCount
    {
      get
      {
        return this.properties.Count;
      }
    }

    public SSOPropBag()
    {
      this.properties = new HybridDictionary();
    }

    public void Read(string propName, out object ptrVar, int errorLog)
    {
      ptrVar = this.properties[(object) propName];
    }

    public void Write(string propName, ref object ptrVar)
    {
      this.properties.Add((object) propName, ptrVar);
    }
  }
}
