using System;

namespace ProjectName.SharedModule.Runtime.Shared.Scripts.Configs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigScopeAttribute: Attribute
    {
        public string Scope { get; set; }
        
        public ConfigScopeAttribute(string scope)
        {
            Scope = scope;
        }
    }
}