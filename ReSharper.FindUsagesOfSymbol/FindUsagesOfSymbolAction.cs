using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings.Store.Implementation;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Search;
using JetBrains.Threading;
using JetBrains.UI.Application;
using JetBrains.UI.CommonControls.Fonts;
using JetBrains.UI.Controls.GotoByName;
using JetBrains.UI.GotoByName;
using JetBrains.UI.PopupWindowManager;
using JetBrains.Util;
using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace CitizenMatt.ReSharper.FindUsagesOfSymbol
{
    [ActionHandler("matt")]
    public class FindUsagesOfSymbolAction : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            var checkAllNotNull = context.CheckAllNotNull(DataConstants.SOLUTION);
            return checkAllNotNull;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var solution = context.GetData(DataConstants.SOLUTION);
            if (solution == null)
            {
                MessageBox.ShowError("Cannot execute Go To action because there's no solution open");
                return;
            }

            Lifetimes.Define(solution.GetLifetime(), null, (definition, lifetime) => Execute(context, solution, definition, lifetime));
        }

        private static void Execute(IDataContext context, ISolution solution, LifetimeDefinition definition, Lifetime lifetime)
        {
            var locks = Shell.Instance.GetComponent<IShellLocks>();
            var actionManager = Shell.Instance.Components.ActionManager();
            var shortcutManager = Shell.Instance.GetComponent<IShortcutManager>();
            var globalSettingsTable = Shell.Instance.GetComponent<SettingsStore>();
            var fontsManager = Shell.Instance.GetComponent<FontsManager>();
            var tooltipMananger = Shell.Instance.Components.Tooltips();
            var dataContexts = Shell.Instance.GetComponent<DataContexts>();
            var threading = Shell.Instance.GetComponent<IThreading>();
            var mainWindow = Shell.Instance.GetComponent<UIApplication>().MainWindow;
            var popupWindowManager = Shell.Instance.GetComponent<PopupWindowManager>();
            var windowsMessageHookManager = Shell.Instance.GetComponent<WindowsMessageHookManager>();
            var mainWindowPopupWindowContext = Shell.Instance.GetComponent<MainWindowPopupWindowContext>();

            var controller = new FindUsagesOfSymbolController(lifetime, solution, LibrariesFlag.SolutionOnly, locks,
                                                              dataContexts, threading);

            new GotoByNameMenu(definition, controller.Model, locks, globalSettingsTable, fontsManager, tooltipMananger,
                               mainWindow, popupWindowManager, windowsMessageHookManager, mainWindowPopupWindowContext,
                               context.GetData(GotoByNameDataConstants.CurrentSearchText),
                               actionManager, shortcutManager);
        }
    }
}
