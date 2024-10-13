using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erray.ServicesScanning
{
    /// <summary>
    /// Attribute to exclude class or interface from automatic services registration. 
    /// If used on class, the class and all of its interfaces wont be automaticaly registred. 
    /// If used on interface, the interface wont be automaticaly registred.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class SuppressAutomaticRegistration : Attribute
    {
        
    }
}
