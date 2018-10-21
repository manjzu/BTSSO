using Microsoft.EnterpriseSingleSignOn.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace BizTalkvNext.SSOx
{
  public static class SSOConfigManager
  {
    private static string idenifierGUID = "ConfigProperties";

    public static void CreateConfigStoreApplication(string appName, string description, string uAccountName, string adminAccountName, SSOPropBag propertiesBag, ArrayList maskArray)
    {
      int flags1 = 0 | 1048576 | 4 | 262144;
      ISSOAdmin ssoAdmin = (ISSOAdmin) new SSOAdmin();
      ssoAdmin.CreateApplication(appName, description, "", uAccountName, adminAccountName, flags1, propertiesBag.PropertyCount);
      int index = 0;
      ssoAdmin.CreateFieldInfo(appName, "dummy", 0);
      foreach (DictionaryEntry property in propertiesBag.properties)
      {
        string label = property.Key.ToString();
        int flags2 = 0 | Convert.ToInt32(maskArray[index]);
        ssoAdmin.CreateFieldInfo(appName, label, flags2);
        ++index;
      }
      ssoAdmin.UpdateApplication(appName, (string) null, (string) null, (string) null, (string) null, 2, 2);
    }

    public static void SetConfigProperties(string appName, SSOPropBag propertyBag)
    {
      ((ISSOConfigStore) new SSOConfigStore()).SetConfigInfo(appName, SSOConfigManager.idenifierGUID, (IPropertyBag) propertyBag);
    }

    public static HybridDictionary GetConfigProperties(string appName, out string description, out string contactInfo, out string appUserAcct, out string appAdminAcct)
    {
      int flags;
      int numFields;
      ((ISSOAdmin) new SSOAdmin()).GetApplicationInfo(appName, out description, out contactInfo, out appUserAcct, out appAdminAcct, out flags, out numFields);
      ISSOConfigStore ssoConfigStore = (ISSOConfigStore) new SSOConfigStore();
      SSOPropBag ssoPropBag = new SSOPropBag();
      ssoConfigStore.GetConfigInfo(appName, SSOConfigManager.idenifierGUID, 4, (IPropertyBag) ssoPropBag);
      return ssoPropBag.properties;
    }

    public static void DeleteApplication(string appName)
    {
      ((ISSOAdmin) new SSOAdmin()).DeleteApplication(appName);
    }

    public static IDictionary<string, string> GetApplications()
    {
      ISSOMapper ssoMapper = (ISSOMapper) new SSOMapper();
      AffiliateApplicationType affiliateApplicationType = AffiliateApplicationType.ConfigStore;
      IPropertyBag propertyBag = (IPropertyBag) ssoMapper;
      uint num = 1;
      object ptrVar1 = (object) (uint) affiliateApplicationType;
      object ptrVar2 = (object) num;
      propertyBag.Write("AppFilterFlags", ref ptrVar1);
      propertyBag.Write("AppFilterFlagMask", ref ptrVar2);
      string[] applications = (string[]) null;
      string[] descriptions = (string[]) null;
      string[] contactInfo = (string[]) null;
      ssoMapper.GetApplications(out applications, out descriptions, out contactInfo);
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>(applications.Length);
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>(applications.Length);
      for (int index = 0; index < applications.Length; ++index)
      {
        if (applications[index].StartsWith("{"))
          dictionary2.Add(applications[index], descriptions[index]);
        else
          dictionary1.Add(applications[index], descriptions[index]);
      }
      foreach (string key in dictionary2.Keys)
        dictionary1.Add(key, dictionary2[key]);
      return (IDictionary<string, string>) dictionary1;
    }
  }
}
