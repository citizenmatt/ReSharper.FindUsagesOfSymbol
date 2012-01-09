using System.Collections.Generic;
using JetBrains.ReSharper.Features.Finding.NavigateFromHere;
using JetBrains.ReSharper.Psi;

namespace CitizenMatt.ReSharper.FindUsagesOfSymbol
{
    public class FindUsagesOfElementAction : ContextNavigationActionBase<FindUsagesOfElementProvider>
    {
        private readonly FindUsagesOfElementProvider provider;

        public FindUsagesOfElementAction(IDeclaredElement declaredElement)
        {
            provider = new FindUsagesOfElementProvider(declaredElement);
        }

        protected override ICollection<FindUsagesOfElementProvider> GetWorkflowProviders()
        {
            return new[] { provider };
        }
    }
}