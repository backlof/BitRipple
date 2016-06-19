using BitRipple.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRipple.Services
{
    public interface IWindowService
    {
        MainWindowSettings GetMainWindow();
        bool SaveMainWindow(MainWindowSettings mainWindow);

        WindowBase GetFeedEditorWindow();
        bool SaveFeedEditorWindow(WindowBase window);
    }
}
