using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Search.SearchRequests;
using JetBrains.ReSharper.Features.Common.Occurences.ExecutionHosting;
using JetBrains.ReSharper.Features.Finding.ExecutionHosting;
using JetBrains.ReSharper.Features.Finding.FindUsages;
using JetBrains.ReSharper.Features.Finding.Usages;
using JetBrains.ReSharper.Psi;

namespace CitizenMatt.ReSharper.FindUsagesOfSymbol
{
    public class FindUsagesOfElementProvider : UsagesContextSearchProviderBase<FindUsagesOfElementContextSearch, SearchUsagesDescriptor, SearchUsagesRequest>, IExtensibleNavigationProvider
    {
        private readonly IDeclaredElement declaredElement;

        public FindUsagesOfElementProvider(IDeclaredElement declaredElement) : base(null)
        {
            this.declaredElement = declaredElement;
        }

        public override Action GetSearchesExecution(IDataContext dataContext, INavigationExecutionHost host)
        {
            var searches = new[] { new FindUsagesOfElementContextSearch(declaredElement) };
            if (searches.Any())
                return () => Execute(dataContext, searches, host);
            return null;
        }

        public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
        {
            var execution = GetSearchesExecution(dataContext, DefaultNavigationExecutionHost.Instance);
            if (execution != null)
                yield return new ContextNavigation("&Find Usages of Symbol", "FindUsages", NavigationActionGroup.Important, execution);
        }
    }
}