using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using VContainer.Unity;

namespace ProjectName.CoreModule.Runtime.Shared.Scripts.Infrastructure
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