using System.Windows;
using System.Windows.Documents;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for MinuteUI2.xaml
    /// </summary>
    public partial class MinuteUI2 : Window
    {
        private AgendaModel temp = new();

        public MinuteUI2(AgendaModel agenda)
        {
            InitializeComponent();

            temp = agenda;

            FillInApoligies();

            WireUpTopicpList();

            btnUpdate.IsEnabled = false;
        }

        private void FillInApoligies()
        {
            string peeps = " ";

            if (temp.Apologies.Count > 0)
            {
                foreach (var a in temp.Apologies)
                {
                    peeps += a.FullName() + ", ";
                }
                peeps = peeps.Substring(0, peeps.Length - 2);
                temp.Topics[0].Detail = "Those giving apologies are:" + peeps;

                MinuteTakerLibary.UpdateTopics(temp.Topics);
            }
            else
            {
                if (temp.Topics.Count > 0)
                {
                    temp.Topics[0].Detail = "There were no apologies received.";

                    MinuteTakerLibary.UpdateTopics(temp.Topics);
                }
            }
        }

        private void WireUpTopicpList()
        {
            lbTopicList.Items.Clear();

            foreach (var topic in temp.Topics)
            {
                lbTopicList.Items.Add(topic.Descrption());
            }
        }

        private void LoadRTFBox()
        {
            int tIndex = lbTopicList.SelectedIndex;

            if (tIndex > -1)
            {
                rtfDetail.Document.Blocks.Clear();
                rtfDetail.Document.Blocks.Add(new Paragraph(new Run(temp.Topics[tIndex].Detail)));
            }
        }
        // *****************************
        private void lbTopicList_Selected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadRTFBox();

            btnSave.IsEnabled = false;
            btnReload.IsEnabled = false;
        }

        private void rtfDetail_SelectionChanged(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = true;
            btnReload.IsEnabled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int tIndex = lbTopicList.SelectedIndex;

            if (tIndex > -1)
            {
                temp.Topics[tIndex].Detail = new TextRange(rtfDetail.Document.ContentStart, rtfDetail.Document.ContentEnd).Text;
            }

            btnSave.IsEnabled = false;
            btnReload.IsEnabled = false;
            btnUpdate.IsEnabled = true;
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadRTFBox();
        }

        // *****************************
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            MinuteTakerLibary.UpdateTopics(temp.Topics);

            btnSave.IsEnabled = false;
            btnReload.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        private void btnReporte_Click(object sender, RoutedEventArgs e)
        {
            AgendaReport ar = new(temp, true);
            ar.Show();
        }
    }
}