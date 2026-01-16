using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Core.Configuration
{
    public sealed class ConfigurationConverter : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            var sCleanAssemblyName = assemblyName;
            var sCleanTypeName = typeName;

            //Sample Assembly Name: BAUERGROUP.bgShippingManager.Application, Version=1.25.6183.13447, Culture=neutral, PublicKeyToken=8aa9b91ff6f9b054
            //Remove Version Information & PublicKeyToken from Assemby Name
            if (assemblyName.StartsWith("BAUERGROUP."))
            {
                var iStartRemove = assemblyName.IndexOf(", Version");
                var iEndRemove = assemblyName.IndexOf(", Culture");

                //Remove Version
                if ((iStartRemove > 0) || (iEndRemove > 0))
                {
                    var sRemoveableString = assemblyName.Substring(iStartRemove, iEndRemove - iStartRemove);
                    sCleanAssemblyName = assemblyName.Replace(sRemoveableString, "");
                }

                //Remove PublicKeyToken
                var iTokenStart = sCleanAssemblyName.IndexOf(", PublicKeyToken");
                sCleanAssemblyName = sCleanAssemblyName.Remove(iTokenStart);
            }

            //Sample Type Name: System.Collections.Generic.Dictionary`2[[BAUERGROUP.Shared.Shipping.ShippingRuleControllerConfigurationItemKey, BAUERGROUP.Shared.Shipping, Version=1.0.6183.13442, Culture=neutral, PublicKeyToken=0d37b4f7da07fa4d],[BAUERGROUP.Shared.Shipping.ShippingRuleControllerConfigurationItemValue, BAUERGROUP.Shared.Shipping, Version=1.0.6183.13442, Culture=neutral, PublicKeyToken=0d37b4f7da07fa4d]]
            //Rewrite Namespace
            if (typeName.Contains("BAUERGROUP."))
            {
                var sStartRemove = typeName.IndexOf(", Version");
                var sEndRemove = typeName.IndexOf(", Culture");

                if ((sStartRemove > 0) || (sEndRemove > 0))
                {
                    var sRemoveableString = typeName.Substring(sStartRemove, sEndRemove - sStartRemove);
                    sCleanTypeName = typeName.Replace(sRemoveableString, "");
                }
            }

            return Type.GetType(String.Format("{0}, {1}", sCleanTypeName, sCleanAssemblyName));
        }
    }
}
