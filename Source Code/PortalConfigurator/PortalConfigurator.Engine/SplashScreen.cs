using System;
using System.Reflection;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class SplashScreen : Form
	{
		public SplashScreen()
		{
			InitializeComponent();
			this.SuspendLayout();
			nameLabel.Text = Application.ProductName;
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			AssemblyCopyrightAttribute copyright = Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
			copyrightLabel.Text = String.Format("Version {0}, {1} {2}", version, copyright.Copyright, Application.CompanyName);
			this.ResumeLayout(false);
		}

		public new void Show(IWin32Window owner)
		{
			Opacity = 0;
			base.Show(owner);
			Application.DoEvents();
			Opacity = 1;
		}
	}
}
