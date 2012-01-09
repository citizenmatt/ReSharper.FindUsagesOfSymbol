using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Occurences;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.ReSharper.Features.Common.GoToByName.Controllers;
using JetBrains.ReSharper.Psi;
using JetBrains.Threading;
using JetBrains.UI.PopupMenu.Impl;

namespace CitizenMatt.ReSharper.FindUsagesOfSymbol
{
    public class FindUsagesOfSymbolController : GotoSymbolController
    {
        private readonly DataContexts dataContexts;
        private readonly IThreading threading;

        public FindUsagesOfSymbolController(Lifetime lifetime, ISolution solution, LibrariesFlag librariesFlag, IShellLocks locks, DataContexts dataContexts, IThreading threading)
            : base(lifetime, solution, librariesFlag, locks)
        {
            this.dataContexts = dataContexts;
            this.threading = threading;
        }

        protected override bool ExecuteItem(JetPopupMenuItem item)
        {
            var occurence = item.Key as IOccurence;
            using (CommitCookie.Commit(Solution))
            {
                var elementOccurence = occurence as DeclaredElementOccurence;
                if (elementOccurence != null)
                {
                    var validDeclaredElement = elementOccurence.DisplayElement.GetValidDeclaredElement();
                    if (validDeclaredElement != null)
                    {
                        var action = new FindUsagesOfElementAction(validDeclaredElement);
                        threading.ExecuteOrQueue("FindUsagesOfSymbolController",
                            () => Lifetimes.Using(lifetime => action.Execute(dataContexts.CreateOnActiveControl(lifetime), () => { })));
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
