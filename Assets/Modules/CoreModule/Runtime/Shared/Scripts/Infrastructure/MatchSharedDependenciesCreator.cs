using System.Collections.Generic;
using Modules.SharedModule.Runtime.Shared.Scripts.Services;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Shared.Scripts.Infrastructure
{
    public class MatchSharedDependenciesCreator : IStartable
    {
        public MatchSharedDependenciesCreator(IEnumerable<IMatchSharedService> services)
        {
            
        }
        
        public void Start()
        {
            
        }
    }
}