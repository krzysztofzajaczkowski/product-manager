using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ProductManager.Web.Helpers
{
    public static class ElectronMenuHelper
    {
        public static MenuItem[] GetMenu()
        {
            var menu = new []
            {
                new MenuItem
                {
                    Label = "Login",
                    Click = () =>
                    {
                        Electron.WindowManager.BrowserWindows.First()
                            .LoadURL($"http://localhost:{BridgeSettings.WebPort}/Login");
                    }
                },
                new MenuItem
                {
                    Label = "Logout",
                    Click = () =>
                    {
                        Electron.WindowManager.BrowserWindows.First()
                            .LoadURL($"http://localhost:{BridgeSettings.WebPort}/Logout");
                    }
                },
                new MenuItem
                {
                    Label = "Info",
                    Click = async ()=>
                    {
                        await Electron.Dialog.ShowMessageBoxAsync("Welcome to Product Manager!\n\n Made by: \n Krzysztof Zajączkowski 175849 \n Rafał Kulik 175750");
                    }
                }
            };

            return menu;
        }
    }
}
