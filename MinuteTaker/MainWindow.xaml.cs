using System.Collections.Generic;
using System.Windows;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<AgendaModel> Agendas = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTaker_Click(object sender, RoutedEventArgs e)
        {
            SelectorUI selectorUI = new SelectorUI(0);
            selectorUI.Show();
        }

        private void btnAgenda_Click(object sender, RoutedEventArgs e)
        {
            AgendaUI agendaUI = new AgendaUI();
            agendaUI.Show();
        }

        private void btnGroups_Click(object sender, RoutedEventArgs e)
        {
            GroupsUI groupsUI = new GroupsUI();
            groupsUI.Show();
        }

        private void btnPeople_Click(object sender, RoutedEventArgs e)
        {
            PeopleUI peopleUI = new PeopleUI();
            peopleUI.Show();
        }

        private void btnArchive_Click(object sender, RoutedEventArgs e)
        {
            SelectorUI selectorUI = new SelectorUI(1);
            selectorUI.Show();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            HelpUI helpUI = new HelpUI();
            helpUI.Show();
        }
    }
}
