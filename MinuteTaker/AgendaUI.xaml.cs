using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for Agenda.xaml
    /// </summary>
    public partial class AgendaUI : Window
    {
        private List<GangModel> Gangs = new();
        private List<AgendaModel> Agendas = new();
        private List<TopicModel> Topics = new();

        public AgendaUI()
        {
            InitializeComponent();

            WireUpGroupList();
            WireUpAgendaList();

            CleanAgendaInput();
            CleanTopicInput();

            btnUpdateAgenda.IsEnabled = false;
            btnClearAgenda.IsEnabled = false;
        }

        private void WireUpGroupList()
        {
            cbGang.Items.Clear();

            Gangs = MinuteTakerLibary.RefreshGangs();

            foreach (var gang in Gangs)
            {
                cbGang.Items.Add(gang.Name);
            }
        }

        private void WireUpAgendaList()
        {
            lbAgendaList.Items.Clear();

            Agendas = MinuteTakerLibary.RefreshAgendas(0);

            foreach (var agenda in Agendas)
            {
                lbAgendaList.Items.Add(agenda.Descrption());
            }
        }

        private bool ValidateAgenda()
        {
            bool output = true;

            tbTitle.Text = tbTitle.Text.Trim();
            tbLocation.Text = tbLocation.Text.Trim();
            cbGang.Text = cbGang.Text.Trim();

            if (tbTitle.Text.Length == 0)
            {
                output = false;
                tbTitle.BorderBrush = Brushes.Red;
            }
            else
            {
                tbTitle.BorderBrush = Brushes.Gray;
            }

            if (tbLocation.Text.Length == 0)
            {
                output = false;
                tbLocation.BorderBrush = Brushes.Red;
            }
            else
            {
                tbLocation.BorderBrush = Brushes.Gray;
            }

            if (cbGang.SelectedIndex == -1)
            {
                output = false;
                cbGang.BorderBrush = Brushes.Red;
            }
            else
            {
                cbGang.BorderBrush = Brushes.Gray;
            }

            if (dpDate.Value < DateTime.Today)
            {
                output = false;
                dpDate.BorderBrush = Brushes.Red;
            }
            else
            {
                dpDate.BorderBrush = Brushes.Gray;
            }

            return output;
        }

        //==============================================
        //               A G E N D A S
        //==============================================
        private void btnAddAgenda_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAgenda())
            {
                AgendaModel a = new();
                a.Title = tbTitle.Text.Trim();
                a.RealDate = (DateTime)dpDate.Value;
                a.Location = tbLocation.Text.Trim();

                int GIndex = cbGang.SelectedIndex;
                a.GangId = Gangs[GIndex].Id;

                MinuteTakerLibary.SaveAgenda(a);

                WireUpAgendaList();

                int AId = FindAgendaId(tbTitle.Text.Trim());

                a.AddDefaultTopics(AId);

                int AIndex = lbAgendaList.SelectedIndex;

                if (AIndex > -1)
                {
                    WireUpTopicpList();
                }

                CleanAgendaInput();
            }
        }

        private int FindAgendaId(string name)
        {
            int output = -1;

            foreach (var a in Agendas)
            {
                if (a.Title == name)
                {
                    return a.Id;
                }
            }

            return output;
        }

        private void btnUpdateAgenda_Click(object sender, RoutedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;

            if (AIndex > -1)
            {
                if (ValidateAgenda())
                {
                    Agendas[AIndex].Title = tbTitle.Text.Trim();
                    Agendas[AIndex].RealDate = (DateTime)dpDate.Value;
                    Agendas[AIndex].Location = tbLocation.Text.Trim();

                    int GIndex = cbGang.SelectedIndex;
                    Agendas[AIndex].GangId = Gangs[GIndex].Id;

                    MinuteTakerLibary.UpdateAgenda(Agendas[AIndex]);

                    WireUpAgendaList();

                    CleanAgendaInput();
                }
            }
        }

        private void btnDeleteAgenda_Click(object sender, RoutedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;

            if (AIndex > -1)
            {
                MinuteTakerLibary.DeleteAgenda(Agendas[AIndex].Id);

                WireUpAgendaList();

                CleanAgendaInput();
            }
        }

        private void btnClearAgenda_Click(object sender, RoutedEventArgs e)
        {
            CleanAgendaInput();
        }

        private void CleanAgendaInput()
        {
            tbTitle.Text = "";
            DateTime today = DateTime.Now;
            dpDate.Value = today;
            tbLocation.Text = "";
            cbGang.SelectedIndex = 0;

            btnAddAgenda.IsEnabled = true;
            btnUpdateAgenda.IsEnabled = false;
            btnDeleteAgenda.IsEnabled = false;
            btnClearAgenda.IsEnabled = false;
        }

        private void lbAgendaList_Selected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;

            if (AIndex > -1)
            {
                tbTitle.Text = Agendas[AIndex].Title;

                dpDate.Value = Agendas[AIndex].RealDate;

                tbLocation.Text = Agendas[AIndex].Location;
                cbGang.Text = GetGangName(Agendas[AIndex].GangId);

                Agendas[AIndex].Topics = MinuteTakerLibary.RefreshTopics(Agendas[AIndex].Id);
                WireUpTopicpList();

                btnAddAgenda.IsEnabled = false;
                btnUpdateAgenda.IsEnabled = true;
                btnDeleteAgenda.IsEnabled = true;
                btnClearAgenda.IsEnabled = true;

                CleanTopicInput();

                btnAddTopic.IsEnabled = true;
                btnUpdateTopic.IsEnabled = false;
                btnRemoveTopic.IsEnabled = false;
                btnClearTopic.IsEnabled = false;

                btnReport.IsEnabled = true;
            }
            else
            {
                btnAddAgenda.IsEnabled = true;
                btnUpdateAgenda.IsEnabled = false;
                btnDeleteAgenda.IsEnabled = false;
                btnClearAgenda.IsEnabled = false;

                CleanTopicInput();

                btnAddTopic.IsEnabled = false;
                btnUpdateTopic.IsEnabled = false;
                btnRemoveTopic.IsEnabled = false;
                btnClearTopic.IsEnabled = false;

                btnReport.IsEnabled = false;
            }
        }

        //==============================================
        //                  T O P I C S
        //==============================================
        private void WireUpTopicpList()
        {
            lbTopicList.Items.Clear();
            int AIndex = lbAgendaList.SelectedIndex;

            if (AIndex > -1)
            {
                foreach (var topic in Agendas[AIndex].Topics)
                {
                    lbTopicList.Items.Add(topic.Descrption());
                }
            }
        }

        private bool ValidateTopic()
        {
            bool output = true;

            if (tbTopicNbr.Text == "")
            {
                output = false;
            }

            if (tbTopicDesc.Text == "")
            {
                output = false;
            }

            return output;
        }

        private void btnAddTopic_Click(object sender, RoutedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;
            int TIndex = lbTopicList.SelectedIndex;

            if (AIndex > -1)
            {
                if (ValidateTopic())
                {
                    TopicModel t = new();
                    t.AgendaId = Agendas[AIndex].Id;
                    t.ItemNbr = int.Parse(tbTopicNbr.Text);
                    t.Heading = tbTopicDesc.Text;

                    MinuteTakerLibary.SaveTopic(t);

                    CleanTopicInput();

                    Agendas[AIndex].Topics = MinuteTakerLibary.RefreshTopics(Agendas[AIndex].Id);
                    WireUpTopicpList();

                    btnAddTopic.IsEnabled = false;
                    btnUpdateTopic.IsEnabled = false;
                    btnRemoveTopic.IsEnabled = false;
                    btnClearTopic.IsEnabled = false;
                }
            }
        }

        private void btnUpdateTopic_Click(object sender, RoutedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;
            int TIndex = lbTopicList.SelectedIndex;

            if (AIndex > -1)
            {
                if (ValidateTopic())
                {
                    Agendas[AIndex].Topics[TIndex].AgendaId = Agendas[AIndex].Id;
                    Agendas[AIndex].Topics[TIndex].ItemNbr = int.Parse(tbTopicNbr.Text);
                    Agendas[AIndex].Topics[TIndex].Heading = tbTopicDesc.Text;

                    MinuteTakerLibary.UpdateTopic(Agendas[AIndex].Topics[TIndex]);

                    CleanTopicInput();

                    Agendas[AIndex].Topics = MinuteTakerLibary.RefreshTopics(Agendas[AIndex].Id);
                    WireUpTopicpList();

                    btnAddTopic.IsEnabled = true;
                    btnUpdateTopic.IsEnabled = false;
                    btnRemoveTopic.IsEnabled = false;
                    btnClearTopic.IsEnabled = false;
                }
            }
        }

        private void btnRemoveTopic_Click(object sender, RoutedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;
            int TIndex = lbTopicList.SelectedIndex;

            if (AIndex > -1)
            {
                MinuteTakerLibary.DeleteTopic(Agendas[AIndex].Topics[TIndex].Id);

                CleanTopicInput();

                Agendas[AIndex].Topics = MinuteTakerLibary.RefreshTopics(Agendas[AIndex].Id);
                WireUpTopicpList();

                btnAddTopic.IsEnabled = false;
                btnUpdateTopic.IsEnabled = false;
                btnRemoveTopic.IsEnabled = false;
                btnClearTopic.IsEnabled = false;
            }
        }

        private void btnClearTopic_Click(object sender, RoutedEventArgs e)
        {
            CleanTopicInput();
        }

        private void lbTopicList_Selected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;
            int TIndex = lbTopicList.SelectedIndex;

            if (TIndex > -1)
            {
                tbTopicNbr.Text = Agendas[AIndex].Topics[TIndex].ItemNbr.ToString();
                tbTopicDesc.Text = Agendas[AIndex].Topics[TIndex].Heading;

                btnAddTopic.IsEnabled = true;
                btnUpdateTopic.IsEnabled = true;
                btnRemoveTopic.IsEnabled = true;
                btnClearTopic.IsEnabled = true;
            }
            else
            {
                CleanTopicInput();

                btnAddTopic.IsEnabled = false;
                btnUpdateTopic.IsEnabled = false;
                btnRemoveTopic.IsEnabled = false;
                btnClearTopic.IsEnabled = false;
            }
        }

        private void CleanTopicInput()
        {
            tbTopicNbr.Text = "";
            tbTopicDesc.Text = "";

            btnAddTopic.IsEnabled = true;
            btnUpdateTopic.IsEnabled = false;
            btnRemoveTopic.IsEnabled = false;
            btnClearTopic.IsEnabled = false;
        }

        //==============================================
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            int AIndex = lbAgendaList.SelectedIndex;

            if (AIndex > -1)
            {
                AgendaReport ar = new(Agendas[AIndex], false);
                ar.Show();
            }
        }

        //==============================================
        //             G E N E R A L
        //==============================================
        private string GetGangName(int id)
        {
            foreach (var g in Gangs)
            {
                if (g.Id == id)
                {
                    return g.Name;
                }
            }

            return "";
        }

        private void tbNumberValidation(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Only allow numbers in the TextBox
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}