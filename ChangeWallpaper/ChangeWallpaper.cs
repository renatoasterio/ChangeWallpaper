using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChangeWallpaper
{
    public partial class ChangeWallpaper : Form
    {
        public ChangeWallpaper()
        {
            InitializeComponent();
        }


        private void btnBrower_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            if (dialog.ShowDialog() == DialogResult.OK)
                txtUrl.Text = dialog.FileName;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        private void btnApply_Click(object sender, EventArgs e)
        {
            string path = txtUrl.Text;

            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Plese, enter the URL.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            uint SPI_SETDESKWALLPAPER = 0x14;
            uint SPIF_UPDATEINIFILE = 0x01;
            uint SPIF_SENDWININICHANGE = 0x02;

            Stream download = new System.Net.WebClient().OpenRead(path);
            Image img = Image.FromStream(download);

            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, tempPath,
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            MessageBox.Show("Wallpaper changed successfully");
        }
    }
}
