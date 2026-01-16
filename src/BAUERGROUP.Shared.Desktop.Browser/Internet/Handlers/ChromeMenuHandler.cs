using CefSharp;
using CefSharp.Wpf.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAUERGROUP.Shared.Desktop.Browser.Internet.Handlers
{
    internal class ChromeMenuHandler : ContextMenuHandler
    {
        public ChromeMenuHandler(bool addDevtoolsMenuItems = false)
            : base(addDevtoolsMenuItems)
        { 
            
        }

        protected override void ExecuteCommand(IBrowser browser, ContextMenuExecuteModel model)
        {
            base.ExecuteCommand(browser, model);

            /*//Custom item
            if (model.MenuCommand == (CefMenuCommand)26501)
            {
                Console.WriteLine("Custom menu used");
            }
            else
            {
                base.ExecuteCommand(browser, model);
            }*/
        }

        protected override void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            base.OnBeforeContextMenu(chromiumWebBrowser, browser, frame, parameters, model);

            /*Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            if (model.Count > 0)
            {
                model.AddSeparator();
            }

            //For this menu handler 28440 and 28441 are used by the Show/Close DevTools commands
            model.AddItem((CefMenuCommand)26501, "Do Something");

            //To disable context menu then clear
            // model.Clear();*/
        }
    }
}
