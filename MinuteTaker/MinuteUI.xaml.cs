using MinuteTaker.Models;
using System.Windows;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for MinuteUI.xaml
    /// </summary>
    public partial class MinuteUI : Window
    {
        private AgendaModel temp = new();

        public MinuteUI(AgendaModel agenda)
        {
            InitializeComponent();

            temp = agenda;

            tbTitle.Text = temp.Title;
            tbLocation.Text = temp.Location;

            tbDate.Text = $"{temp.Day}/{temp.Month}/{temp.Year}";
            tbTime.Text = $"{temp.Hour}:{temp.Minute}";

            WireUpAttendeesList();
            WireUpNoShowsList();
            WireUpApologiesList();

            if (temp.Attendees.Count > 0)
            {
                btnAddMbr.IsEnabled = false;
                btnClrMbr.IsEnabled = true;
            }
            else
            {
                btnAddMbr.IsEnabled = true;
                btnClrMbr.IsEnabled = false;
            }

            if (temp.Archive == 1)
            {
                cbArchive.IsChecked = true;
            }
            else
            {
                cbArchive.IsChecked = false;
            }

        }

        private void btnAddMbr_Click(object sender, RoutedEventArgs e)
        {
            GangModel g = new();

            g = MinuteTakerLibary.GetGang(temp.GangId);

            foreach (var m in g.Members)
            {
                temp.Attendees.Add(m);

                // Save an Attendee
                AgendaMembersModel amm = new();
                amm.AgendaId = temp.Id;
                amm.PersonId = m.Id;
                amm.Attend = 1; // Attending
                MinuteTakerLibary.SaveAgendaMembers(amm);
            }

            WireUpAttendeesList();

            // Don't allow to add Gang Members again
            btnAddMbr.IsEnabled = false;
            btnClrMbr.IsEnabled = true;
        }

        private void btnClrMbr_Click(object sender, RoutedEventArgs e)
        {
            temp.Attendees.Clear();
            temp.NonAttendees.Clear();
            temp.Apologies.Clear();

            WireUpAttendeesList();
            WireUpNoShowsList();
            WireUpApologiesList();

            // allow to Gang Members to be added
            btnAddMbr.IsEnabled = true;
            btnClrMbr.IsEnabled = false;

            // Delete ALL Agenda Members
            SQLiteDataAccess.DeleteAllAgendaMember(temp.Id);
        }

        private void WireUpAttendeesList()
        {
            lbPartList.Items.Clear();

            foreach (var person in temp.Attendees)
            {
                lbPartList.Items.Add(person.FullName());
            }
        }

        private void WireUpNoShowsList()
        {
            lbNoShowsList.Items.Clear();

            foreach (var person in temp.NonAttendees)
            {
                lbNoShowsList.Items.Add(person.FullName());
            }
        }

        private void WireUpApologiesList()
        {
            lbApoloList.Items.Clear();

            foreach (var person in temp.Apologies)
            {
                lbApoloList.Items.Add(person.FullName());
            }
        }

        // ***********************************************************
        private void btnPartOut_Click(object sender, RoutedEventArgs e)
        {
            int Index = lbPartList.SelectedIndex;

            if (Index > -1)
            {
                PersonModel p = new PersonModel();
                p = temp.Attendees[Index];
                temp.NonAttendees.Add(p);
                temp.Attendees.Remove(p);

                WireUpAttendeesList();
                WireUpNoShowsList();

                // Update an Attendees to NonAttendees
                AgendaMembersModel amm = new();
                amm.AgendaId = temp.Id;
                amm.PersonId = p.Id;
                amm.Attend = 0; // NonAttendees
                SQLiteDataAccess.UpdateAgendaMember(amm);
            }
        }

        private void btnPartIn_Click(object sender, RoutedEventArgs e)
        {
            int Index = lbNoShowsList.SelectedIndex;

            if (Index > -1)
            {
                PersonModel p = new PersonModel();
                p = temp.NonAttendees[Index];
                temp.Attendees.Add(p);
                temp.NonAttendees.Remove(p);

                WireUpAttendeesList();
                WireUpNoShowsList();

                // Update a NonAttendees to Attendees
                AgendaMembersModel amm = new();
                amm.AgendaId = temp.Id;
                amm.PersonId = p.Id;
                amm.Attend = 1; // Attendees
                SQLiteDataAccess.UpdateAgendaMember(amm);
            }
        }

        // ***********************************************************
        private void btnNonOut_Click(object sender, RoutedEventArgs e)
        {
            int Index = lbNoShowsList.SelectedIndex;

            if (Index > -1)
            {
                PersonModel p = new PersonModel();
                p = temp.NonAttendees[Index];
                temp.Apologies.Add(p);
                temp.NonAttendees.Remove(p);

                WireUpApologiesList();
                WireUpNoShowsList();

                // Update a NonAttendees to Apologies
                AgendaMembersModel amm = new();
                amm.AgendaId = temp.Id;
                amm.PersonId = p.Id;
                amm.Attend = 2; // Apologies
                SQLiteDataAccess.UpdateAgendaMember(amm);
            }
        }

        private void btnNonIn_Click(object sender, RoutedEventArgs e)
        {
            int Index = lbApoloList.SelectedIndex;

            if (Index > -1)
            {
                PersonModel p = new PersonModel();
                p = temp.Apologies[Index];
                temp.NonAttendees.Add(p);
                temp.Apologies.Remove(p);

                WireUpApologiesList();
                WireUpNoShowsList();

                // Update a Apologies to NonAttendees
                AgendaMembersModel amm = new();
                amm.AgendaId = temp.Id;
                amm.PersonId = p.Id;
                amm.Attend = 0; // NonAttendees
                SQLiteDataAccess.UpdateAgendaMember(amm);
            }
        }

        // ***********************************************************
        private void btnTake_Click(object sender, RoutedEventArgs e)
        {
            MinuteUI2 min2 = new MinuteUI2(temp);
            min2.Show();
        }

        private void cbArchive_Checked(object sender, RoutedEventArgs e)
        {
            temp.Archive = 1;
            MinuteTakerLibary.UpdateAgenda(temp);
        }

        private void cbArchive_Unchecked(object sender, RoutedEventArgs e)
        {
            temp.Archive = 0;
            MinuteTakerLibary.UpdateAgenda(temp);
        }
    }
}