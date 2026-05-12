using System.Collections.Generic;
using ProjectName.SharedModule.Runtime.Client.Scripts.Infrastructure;
using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;
using VContainer.Unity;

namespace ProjectName.CoreModule.Runtime.Client.Scripts.Infrastructure.Services
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