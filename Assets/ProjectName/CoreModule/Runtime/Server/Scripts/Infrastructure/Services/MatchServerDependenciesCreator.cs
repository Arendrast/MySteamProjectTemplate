using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Server.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using VContainer.Unity;

namespace ProjectName.CoreModule.Runtime.Server.Scripts.Infrastructure.Services
{
    public class MatchServerDependenciesCreator : IStartable
    {
        public MatchServerDependenciesCreator(IEnumerable<IMatchServerService> services)
        {
            
        }
        
        public void Start()
        {
            
        }
    }
}