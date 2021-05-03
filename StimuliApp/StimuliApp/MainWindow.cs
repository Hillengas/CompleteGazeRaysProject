using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;

namespace StimuliApp
{
    public partial class MainWindow : Form
    {
        private string _arg;
        private string _gazeRay_DataCollection = Directory.GetParent(Application.StartupPath).FullName; // @"C:\Users\Alex\Desktop\GazeRay_DataCollection";        // "C:\\Users\\Alex\\Desktop\\StimuliApp\\StimuliApp\\bin\\Debug"
        private string _chosenStimulus = "chosenStimulus.txt";
        private string _folderWithExtraInformationStimulus = "z_Extra_Information_Stimulus";
        private string _folderWithExtraInformation = "z_Extra_Information";
        private string _extraInformation = "";
        private string _stimulusNameWithExtension;

        public MainWindow()
        {
            InitializeComponent();

            // panelLeft anpassen
            panelLeft.Height = LaunchRayCasting_Button.Height;
            panelLeft.Top = LaunchRayCasting_Button.Top;
        }

        private void LaunchRayCasting_Button_Click(object sender, EventArgs e)
        {
            // panelLeft anpassen
            panelLeft.Height = LaunchRayCasting_Button.Height;
            panelLeft.Top = LaunchRayCasting_Button.Top;

            _arg = "-projectPath \"" + _gazeRay_DataCollection + "\"";

            if (!string.IsNullOrEmpty(_arg))
            {
                // Start Unity and open the project
                Process.Start(@"C:\Program Files\Unity\Hub\Editor\2020.3.0f1\Editor\Unity.exe", _arg);
            }
        }

        private void ChooseEyeTrackingData_Button_Click(object sender, EventArgs e)
        {
            // panelLeft anpassen
            panelLeft.Height = ChooseEyeTrackingData_Button.Height;
            panelLeft.Top = ChooseEyeTrackingData_Button.Top;

            if (folderBrowserDialog_chooseProband.ShowDialog() == DialogResult.OK)
            {
                var folderPathProband = folderBrowserDialog_chooseProband.SelectedPath;

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(_gazeRay_DataCollection, _folderWithExtraInformation, "ProbandenPath.txt")))
                {
                    outputFile.WriteLine(folderPathProband);
                }

                pictureBox_checkmark.Visible = true;
            }
        }

        private void ChooseStimulus_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Bild Auswahl";
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.MP4)|*.BMP;*.JPG;*.GIF;*.PNG;*.MP4|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _stimulusNameWithExtension = ofd.SafeFileName;
                var stimulusFullPath = ofd.FileName;
                var stimulusExtension = Path.GetExtension(_stimulusNameWithExtension);

                var pathToChosenStimulustxt = Path.Combine(_gazeRay_DataCollection, _folderWithExtraInformation, _chosenStimulus); 


                if (stimulusExtension.Equals(".mp4")) // ist Video
                {
                    Bitmap imageBitmap;

                    // TODO: fix it: bricht mit Error ab, wenn nochmals ein Video gewählt wird
                    // extract first frame from chosen Video for the UIs-Thumbnail
                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                    var pathToThumbnail = Path.Combine(_gazeRay_DataCollection, _folderWithExtraInformationStimulus);
                    try
                    {
                        ffMpeg.GetVideoThumbnail(stimulusFullPath,
                            Path.Combine(pathToThumbnail, "ExtractedFrame.jpeg"), 0);   //@"C:\Users\Alex\Desktop\StimuliApp\Z_extra_Information\ExtractedFrame.jpeg", 0);
                        imageBitmap = new Bitmap(Path.Combine(pathToThumbnail, "ExtractedFrame.jpeg"));
                    }
                    catch(Exception)
                    {
                        ffMpeg.GetVideoThumbnail(stimulusFullPath,
                            Path.Combine(pathToThumbnail, "ExtractedFrame2.jpeg"), 0);
                        imageBitmap = new Bitmap(Path.Combine(pathToThumbnail, "ExtractedFrame2.jpeg"));
                    }

                    int newWidth = 400;
                    int newHeight = 200;


                    Bitmap imageBitmapResized = ResizeImage(imageBitmap, newWidth, newHeight);

                    pictureBox_showStimuli.Image = imageBitmapResized;

                    // TODO: ffMpeg schließen
                    ffMpeg.Abort();
                    ffMpeg.Stop();

                    _extraInformation += $"{stimulusFullPath}\tempty\tempty\tDatei ist ein Video\t{Path.GetFileNameWithoutExtension(_stimulusNameWithExtension)}";
                }
                else // ist Bild
                {
                    var img = Image.FromFile(stimulusFullPath);

                    Bitmap imageBitmap = new Bitmap(ofd.FileName);

                    var oldHeight = imageBitmap.Height;
                    double oldWidth = imageBitmap.Width;

                    int newWidth = 200;

                    double differenceWidthPercent = newWidth / oldWidth;


                    Bitmap imageBitmapResized = ResizeImage(imageBitmap, newWidth, (int)(oldHeight * differenceWidthPercent));

                    pictureBox_showStimuli.Image = imageBitmapResized;

                    _extraInformation += $"{Path.GetFileNameWithoutExtension(_stimulusNameWithExtension)}\t{img.Width}\t{img.Height}\tDatei ist ein Bild\t{Path.GetFileNameWithoutExtension(_stimulusNameWithExtension)}";
                }

                File.WriteAllText(pathToChosenStimulustxt, _extraInformation);

                // empty _extraInformation
                _extraInformation = "";
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


    }
}
