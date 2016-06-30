using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using BitRipple.ViewModel;
using BitRipple.Services;

namespace BitRipple.Utilities
{
	public class ViewModelLocator
	{
		private readonly IKernel Container; //Can turn into static if dependencies need to be gathered

		public ViewModelLocator()
		{
			Container = new StandardKernel();
			Container.Bind<IDataService>().To<XmlDataService>().InSingletonScope();
			Container.Bind<IWindowService>().To<XmlWindowService>().InSingletonScope();

			// Put the .xml files in different locations
			Container.Bind<IFileHandler>().To<PortableFileHandler>().WhenInjectedExactlyInto<XmlWindowService>().WithConstructorArgument("saveFolder", "Window");
			Container.Bind<IFileHandler>().To<PortableFileHandler>().WhenInjectedExactlyInto<XmlDataService>().WithConstructorArgument("saveFolder", "Data");

			Container.Bind<ApplicationService>().ToSelf().InSingletonScope();
			Container.Bind<IDefaultService>().To<MininovaDefaultData>().InSingletonScope();

			Container.Bind<MainViewModel>().ToSelf().InSingletonScope();
			Container.Bind<FeedEditorViewModel>().ToSelf().InTransientScope();
		}

		public MainViewModel MainViewModel
		{
			get
			{
				try
				{
					return Container.Get<MainViewModel>();
				}
				catch (Exception e)
				{
					Errors.Print(e);
					throw;
				}
			}
		}

		public FeedEditorViewModel EditOrCreateFeedViewModel
		{
			get
			{
				try
				{
					return Container.Get<FeedEditorViewModel>();
				}
				catch (Exception e)
				{
					Errors.Print(e);
					throw;
				}
			}
		}
	}
}
