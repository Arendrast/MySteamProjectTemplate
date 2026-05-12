using System.Collections.Generic;
using Modules.SharedModule.Runtime.Server.Scripts.Infrastructure;
using VContainer.Unity;

namespace Modules.CoreModule.Runtime.Server.Scripts.Infrastructure.Services
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