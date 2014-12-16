using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class SplashScreen : Form
	{
		public SplashScreen()
		{
			InitializeComponent();
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
