using ProjectName.SharedModule.Runtime.Shared.Scripts.Services;

namespace ProjectName.PlayerModule.Runtime.Shared.Scripts.Operator
{
    public class IsOperatorRepository : IPersistentService
    {
        public bool IsOperator { get; private set; }
        
        public void SetIsOperator(bool isOperator)
        {
            IsOperator = isOperator;
        }
    }
}