using System.Collections.Generic;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services.ContextNavigation.ContextSearches.BaseSearches;
using JetBrains.ReSharper.Psi;

namespace CitizenMatt.ReSharper.FindUsagesOfSymbol
{
    public class FindUsagesOfElementContextSearch : FindUsagesContextSearch
    {
        private readonly IDeclaredElement declaredElement;

        public FindUsagesOfElementContextSearch(IDeclaredElement declaredElement)
        {
            this.declaredElement = declaredElement;
        }

        protected override IList<IDeclaredElement> GetCandidates(IDataContext context)
        {
            return new List<IDeclaredElement> { declaredElement };
        }
    }
}