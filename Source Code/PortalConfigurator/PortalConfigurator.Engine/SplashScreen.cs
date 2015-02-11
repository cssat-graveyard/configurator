using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class SplashScreen : Form
	{
		public SplashScreen()
		{
			InitializeComponent();
			PreInitializeComponent();
		}

		public new void Show(IWin32Window owner)
		{
			Opacity = 0;
			base.Show(owner);
			Application.DoEvents();
			Opacity = 1;
		}

		private void PreInitializeComponent()
		{
			this.SuspendLayout();
//			this.ClientSize = new System.Drawing.Size(284, 261);
//			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
		}
	}
}
