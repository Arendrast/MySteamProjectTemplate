using System.Collections.Generic;
using Modules.SharedModule.Runtime.Client.Scripts.Infrastructure;
using VContainer.Unity;

namespace Modules.AppModule.Runtime.Client.Scripts.Infrastructure.Services
{
    public class MatchClientDependenciesCreator : IStartable
    {
        public MatchClientDependenciesCreator(IEnumerable<IMatchClientService> services)
        {
            
        }
        
        public void Start()
        {
            
        }
    }
}