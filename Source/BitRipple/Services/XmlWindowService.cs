using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitRipple.Model;

namespace BitRipple.Services
{
    public class XmlWindowService : IWindowService
    {
        public static readonly string MAINWINDOW_FILENAME = "MainWindow.xml";
        public static readonly string EDITFEED_WINDOW = "EditFeed.xml";

        IFileHandler FileHandler;
        IDefaultService DefaultService;

        public XmlWindowService(IFileHandler fileHandler, IDefaultService defaultService)
        {
            FileHandler = fileHandler;
            DefaultService = defaultService;
        }

        public MainWindowSettings GetMainWindow()
        {
            MainWindowSettings mainWindow = FileHandler.ReadSerializedXml(MAINWINDOW_FILENAME, typeof(MainWindowSettings)) as MainWindowSettings;
            if (mainWindow == null)
            {
                mainWindow = DefaultService.GetDefaultMainWindow();
            }
            return mainWindow;
        }

        public bool SaveMainWindow(MainWindowSettings mainWindow)
        {
            return FileHandler.WriteSerializedXml(mainWindow, MAINWINDOW_FILENAME);
        }



        public WindowBase GetFeedEditorWindow()
        {
            WindowBase editFeedWindow = FileHandler.ReadSerializedXml(EDITFEED_WINDOW, typeof(WindowBase)) as WindowBase;
            if (editFeedWindow == null)
            {
                editFeedWindow = DefaultService.GetDefaultEditFeedWindow();
            }
            return editFeedWindow;
        }

        public bool SaveFeedEditorWindow(WindowBase editFeedWindow)
        {
            return FileHandler.WriteSerializedXml(editFeedWindow, EDITFEED_WINDOW);
        }
    }
}
