using System.Collections.Generic;
using System.Windows;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for SelecterUI.xaml
    /// </summary>
    public partial class SelectorUI : Window
    {
        private List<AgendaModel> Agendas = new();

        private int arch;

        public SelectorUI(int archive)
        {
            InitializeComponent();

            arch = archive;

            WireUpAgendaList();
        }

        private void WireUpAgendaList()
        {
            lbMinutesList.Items.Clear();

            Agendas = MinuteTakerLibary.RefreshAgendas(arch);

            foreach (var minute in Agendas)
            {
                lbMinutesList.Items.Add(minute.Descrption());
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            int MIndex = lbMinutesList.SelectedIndex;

            if (MIndex > -1)
            {
                MinuteUI minuteUI = new MinuteUI(Agendas[MIndex]);
                minuteUI.Show();
            }

            WireUpAgendaList();
        }
    }
}
