using System.Drawing;
using System.Windows.Forms;

namespace TreeTestApp
{
	public partial class TestTreeForm : UserControl
	{
		public TestTreeForm()
		{
			InitializeComponent();

			Bitmap bmp = new Bitmap(typeof(Form1), "bitmapstrip.bmp");
			bmp.MakeTransparent(Color.Magenta);

			m_tree.Images = new ImageList();
			m_tree.Images.Images.AddStrip(bmp);
		}
	}
}
