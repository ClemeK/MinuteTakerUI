using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for PeopleUI.xaml
    /// </summary>
    public partial class PeopleUI : Window
    {
        private List<PersonModel> People = new();

        public PeopleUI()
        {
            InitializeComponent();

            WireUpPeopleList();
        }

        private void WireUpPeopleList()
        {
            lbPersonList.Items.Clear();

            People = MinuteTakerLibary.RefreshPeople();

            foreach (var person in People)
            {
                lbPersonList.Items.Add(person.FullName());
            }
        }

        private void btnAddPerson_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePerson())
            {
                MinuteTakerLibary.SavePerson(tbPersonFirst.Text, tbPersonLast.Text, tbPersonEmail.Text, tbPersonPhone.Text);
            }

            WireUpPeopleList();

            tbPersonFirst.Text = "";
            tbPersonLast.Text = "";
            tbPersonEmail.Text = "";
            tbPersonPhone.Text = "";
        }

        private void btnUpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            int index = lbPersonList.SelectedIndex;

            if (ValidatePerson())
            {
                People[index].FirstName = tbPersonFirst.Text;
                People[index].LastName = tbPersonLast.Text;
                People[index].EmailAddress = tbPersonEmail.Text;
                People[index].PhoneNbr = tbPersonPhone.Text;

                MinuteTakerLibary.UpdatePerson(People[index]);
            }

            WireUpPeopleList();

            tbPersonFirst.Text = "";
            tbPersonLast.Text = "";
            tbPersonEmail.Text = "";
            tbPersonPhone.Text = "";
        }

        private void btnDeletePerson_Click(object sender, RoutedEventArgs e)
        {
            int index = lbPersonList.SelectedIndex;

            MinuteTakerLibary.DeletePerson(People[index].Id);

            People.Clear();
            People = MinuteTakerLibary.RefreshPeople();

            WireUpPeopleList();

            tbPersonFirst.Text = "";
            tbPersonLast.Text = "";
            tbPersonEmail.Text = "";
            tbPersonPhone.Text = "";
        }

        private bool ValidatePerson()
        {
            bool output = true;

            tbPersonFirst.Text = tbPersonFirst.Text.Trim();
            tbPersonLast.Text = tbPersonLast.Text.Trim();
            tbPersonEmail.Text = tbPersonEmail.Text.Trim();
            tbPersonPhone.Text = tbPersonPhone.Text.Trim();

            if (tbPersonFirst.Text.Length == 0)
            {
                output = false;
                tbPersonFirst.BorderBrush = Brushes.Red;
            }
            else
            {
                tbPersonFirst.BorderBrush = Brushes.Gray;
            }

            if (tbPersonLast.Text.Length == 0)
            {
                output = false;
                tbPersonLast.BorderBrush = Brushes.Red;
            }
            else
            {
                tbPersonLast.BorderBrush = Brushes.Gray;
            }

            if (tbPersonEmail.Text.Length == 0)
            {
                output = false;
                tbPersonEmail.BorderBrush = Brushes.Red;
            }
            else
            {
                tbPersonEmail.BorderBrush = Brushes.Gray;
            }

            if (tbPersonPhone.Text.Length == 0)
            {
                output = false;
                tbPersonPhone.BorderBrush = Brushes.Red;
            }
            else
            {
                tbPersonPhone.BorderBrush = Brushes.Gray;
            }

            return output;
        }

        private void lbPersonList_Selected(object sender, RoutedEventArgs e)
        {
            int index = lbPersonList.SelectedIndex;

            if (index > -1)
            {
                People[index].CleanPerson();

                lblPersonId.Content = People[index].Id;
                tbPersonFirst.Text = People[index].FirstName;
                tbPersonLast.Text = People[index].LastName;
                tbPersonEmail.Text = People[index].EmailAddress;
                tbPersonPhone.Text = People[index].PhoneNbr;
            }
        }
    }
}