using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for HelpUI.xaml
    /// </summary>
    public partial class HelpUI : Window
    {
        public HelpUI()
        {
            InitializeComponent();

            string fileName = @"C:\Users\we364\source\repos\MinuteTakerUI\MinuteTaker\Help.rtf";

            TextRange range;
            FileStream fStream;

            if (File.Exists(fileName))
            {
                range = new TextRange(HelpText.Document.ContentStart, HelpText.Document.ContentEnd);
                fStream = new FileStream(fileName, FileMode.Open);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }
        }
    }
}
