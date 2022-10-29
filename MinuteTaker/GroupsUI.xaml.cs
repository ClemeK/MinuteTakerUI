using System.Collections.Generic;
using System.Windows;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for Groups.xaml
    /// </summary>
    public partial class GroupsUI : Window
    {
        private List<GangModel> Gangs = new();

        private List<PersonModel> People = new();
        private List<PersonModel> NotSelectedMembers = new();

        public GroupsUI()
        {
            InitializeComponent();

            People = MinuteTakerLibary.RefreshPeople();

            WireUpGroupList();
            WirePersonList();
        }

        // ===================================
        private void WireUpGroupList()
        {
            lbGroupList.Items.Clear();

            Gangs = MinuteTakerLibary.RefreshGangs();

            foreach (var gang in Gangs)
            {
                lbGroupList.Items.Add(gang.Name);
            }
        }

        private void WirePersonList()
        {
            cbPerson.Items.Clear();

            foreach (var m in NotSelectedMembers)
            {
                cbPerson.Items.Add(m.FullName());
            }
        }

        private void WireSelectedList()
        {
            lbMembersList.Items.Clear();
            int index = lbGroupList.SelectedIndex;

            foreach (var m in Gangs[index].Members)
            {
                lbMembersList.Items.Add(m.FullName());
            }
        }

        // ===================================
        private void btnAddGroup_Click(object sender, RoutedEventArgs e)
        {
            if (tbGroupName.Text.Trim().Length > 0)
            {
                MinuteTakerLibary.SaveGang(tbGroupName.Text.Trim());

                WireUpGroupList();
            }
        }

        private void btnDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            int index = lbGroupList.SelectedIndex;

            if (index > -1)
            {
                MinuteTakerLibary.DeleteGang(Gangs[index].Id);
                WireUpGroupList();
            }
        }

        private void lbGroupList_Selected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbGroupList.SelectedIndex;

            if (index != -1)
            {
                tbGroupName.Text = Gangs[index].Name;

                NotSelectedMembers = People;

                foreach (var m in Gangs[index].Members)
                {
                    NotSelectedMembers = MinuteTakerLibary.RemoveSelected(NotSelectedMembers, m);
                }

                WireSelectedList();
                WirePersonList();

                btnAddMember.IsEnabled = true;
                btnRemoveMember.IsEnabled = false;
                cbPerson.IsEnabled = true;
            }
            else
            {
                NotSelectedMembers.Clear();

                WireSelectedList();
                WirePersonList();

                btnAddMember.IsEnabled = false;
                btnRemoveMember.IsEnabled = false;
                cbPerson.IsEnabled = false;
            }
        }

        // ===================================
        private void btnAddMember_Click(object sender, RoutedEventArgs e)
        {
            // Get the Gang ID
            int GIndex = lbGroupList.SelectedIndex;

            if (GIndex != -1)
            {
                GangModel g = new();
                g = Gangs[GIndex];

                // Get the Person ID
                int PIndex = cbPerson.SelectedIndex;

                PersonModel p = new();
                p = NotSelectedMembers[PIndex];

                // Save the to member list
                MinuteTakerLibary.SaveGangMember(g.Id, p.Id);

                // Add to the Gang
                g.Members.Add(p);

                NotSelectedMembers = People;

                // Remove people from the not-selected list that are now selected.
                foreach (var m in Gangs[GIndex].Members)
                {
                    NotSelectedMembers = MinuteTakerLibary.RemoveSelected(NotSelectedMembers, m);
                }

                WireSelectedList();
                WirePersonList();
            }
        }

        private void btnRemoveMember_Click(object sender, RoutedEventArgs e)
        {
            // Get the Gang ID
            int GIndex = lbGroupList.SelectedIndex;

            GangModel g = new();
            g = Gangs[GIndex];

            // Get the Person ID
            int PIndex = lbMembersList.SelectedIndex;

            PersonModel p = new();
            p = Gangs[GIndex].Members[PIndex];

            // Delete the Gang Member from the member list
            MinuteTakerLibary.DeleteGangMember(g.Id, p.Id);

            // Remove from the Gang
            g.Members.Remove(p);

            NotSelectedMembers = People;

            foreach (var m in Gangs[GIndex].Members)
            {
                NotSelectedMembers = MinuteTakerLibary.RemoveSelected(NotSelectedMembers, m);
            }

            WirePersonList();
            WireSelectedList();
        }

        private void lbMembersList_Selected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = lbMembersList.SelectedIndex;

            if (index != -1)
            {
                btnRemoveMember.IsEnabled = true;
            }
            else
            {
                btnRemoveMember.IsEnabled = false;
            }
        }
    }
}