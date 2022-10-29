using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace MinuteTaker
{
    /// <summary>
    /// Interaction logic for AgendaReport.xaml
    /// </summary>
    public partial class AgendaReport : Window
    {
        private static string temp = "";
        private static bool minutes = false;
        private static int gangId = -1;
        private static bool changed = false;
        private string fileName = "";

        private List<SmtpsModel> SmtpList = new();

        public AgendaReport(AgendaModel agenda, bool type)
        {
            InitializeComponent();

            temp = agenda.Title;
            minutes = type;
            gangId = agenda.GangId;

            if (minutes == false)
            {
                tbSubject.Text = "Meeting Reminder";
                tbBody.Text = $"Reminder that the {agenda.Title} meeting is on the {agenda.Day}/{agenda.Month}/{agenda.Year}" +
                              $" at {agenda.Hour}:{agenda.Minute}, I have attached the agenda for the meeting.";
            }
            else
            {
                tbSubject.Text = "Meeting Minutes";
                tbBody.Text = $"Please find attached the minutes for the {agenda.Title} meeting that took place" +
                    $" on the {agenda.Day}/{agenda.Month}/{agenda.Year} at {agenda.Hour}:{agenda.Minute}.";
            }

            WireUpSmtpList();

            tbUserName.Text = MinuteTakerLibary.AppKeyLookup("senderName");
            tbUseremail.Text = MinuteTakerLibary.AppKeyLookup("senderEmail");
            cbSmtp.SelectedItem = MinuteTakerLibary.AppKeyLookup("Host");
            pbPassword.Password = MinuteTakerLibary.AppKeyLookup("senderPassword");

            CreateRTF(agenda);

            SaveRFT();

            tbFilename.Text = fileName;

            changed = false;
        }

        private void WireUpSmtpList()
        {
            cbSmtp.Items.Clear();

            SmtpList = MinuteTakerLibary.LoadSmtps();

            foreach (var s in SmtpList)
            {
                cbSmtp.Items.Add(s.Name);
            }

            cbSmtp.SelectedValue = "Local Host";
        }

        private void CreateRTF(AgendaModel agenda)
        {
            FlowDocument mcFlowDoc = new FlowDocument();

            // Add the Title and Date & Time of the meeting
            Paragraph head = new Paragraph();
            head.FontSize = 48;
            head.Inlines.Add(new Bold(new Run(agenda.Title)));
            head.TextAlignment = TextAlignment.Center;

            // This is done to add a line feed to the RTF out \n\r does appear to work
            string newline = "\u2028";

            Paragraph para = new Paragraph();
            para.FontSize = 14;
            para.Inlines.Add(new Bold(new Run("Date: ")));
            para.Inlines.Add(new Run(agenda.Day + "/" + agenda.Month + "/" + agenda.Year + newline));
            para.Inlines.Add(new Bold(new Run("Time: ")));
            para.Inlines.Add(new Run(agenda.Hour + ":" + agenda.Minute.ToString("00") + newline));
            para.Inlines.Add(new Bold(new Run("Location: ")));
            para.Inlines.Add(new Run(agenda.Location + newline + newline));

            if (minutes == true)
            {
                if (agenda.Attendees.Count > 0)
                {
                    // List the expected attendees
                    para.Inlines.Add(new Bold(new Run("Attendees: ")));
                    string attn = "";

                    foreach (var m in agenda.Attendees)
                    {
                        attn += m.FullName() + ", ";
                    }

                    attn = attn.Substring(0, attn.Length - 2);
                    para.Inlines.Add(new Run(attn + newline + newline));
                }

                if (agenda.NonAttendees.Count > 0)
                {
                    // List the Non-attendees
                    para.Inlines.Add(new Bold(new Run("Not Present: ")));
                    string nonattn = "";

                    foreach (var m in agenda.NonAttendees)
                    {
                        nonattn += m.FullName() + ", ";
                    }

                    if (agenda.Apologies.Count > 0)
                    {
                        foreach (var m in agenda.Apologies)
                        {
                            nonattn += m.FullName() + ", ";
                        }

                        nonattn = nonattn.Substring(0, nonattn.Length - 2);
                        para.Inlines.Add(new Run(nonattn + newline + newline));
                    }
                }
            }
            else
            {
                GangModel gm = new();
                gm = MinuteTakerLibary.GetGang(agenda.GangId);

                para.Inlines.Add(new Bold(new Run("Attendees: ")));
                string attn = "";

                foreach (var m in gm.Members)
                {
                    attn += m.FullName() + ", ";
                }

                attn = attn.Substring(0, attn.Length - 2);
                para.Inlines.Add(new Run(attn + newline + newline));
            }

            // List the Topic of the meeting
            foreach (var t in agenda.Topics)
            {
                para.Inlines.Add(new Bold(new Run(t.ItemNbr.ToString() + ") " + t.Heading + newline)));

                if (minutes == true)
                {
                    para.Inlines.Add(new Run(t.Detail + newline + newline));
                }
            }

            // Display the Document
            mcFlowDoc.Blocks.Add(head);
            mcFlowDoc.Blocks.Add(para);
            RTBox.Document = mcFlowDoc;
        }

        private void cbSmtp_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int SIndex = cbSmtp.SelectedIndex;

            if (SIndex > -1)
            {
                if (SmtpList[SIndex].Name != "Other")
                {
                    tbHost.Text = SmtpList[SIndex].Url;
                    tbHost.IsEnabled = false;
                    tbPort.Text = SmtpList[SIndex].Port.ToString();
                    tbPort.IsEnabled = false;
                    tbSSL.Text = SmtpList[SIndex].Ssl;
                    tbSSL.IsEnabled = false;
                }
                else
                {
                    tbHost.Text = "";
                    tbHost.IsEnabled = true;
                    tbPort.Text = "";
                    tbPort.IsEnabled = true;
                    tbSSL.Text = "";
                    tbSSL.IsEnabled = true;
                }

                changed = true;
            }
        }

        private void DataChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            changed = true;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {

            if (changed)
            {
                // Save the User\Password
                MinuteTakerLibary.UpdateKeyLookup("senderName", tbUserName.Text.Trim());
                MinuteTakerLibary.UpdateKeyLookup("senderEmail", tbUseremail.Text.Trim());
                MinuteTakerLibary.UpdateKeyLookup("senderPassword", pbPassword.Password.Trim());
                MinuteTakerLibary.UpdateKeyLookup("Host", tbHost.Text.Trim());

                changed = false;
            }

            //SaveRFT();

            GangModel g = new GangModel();
            g = MinuteTakerLibary.GetGang(gangId);

            int SIndex = cbSmtp.SelectedIndex;

            if (SIndex > -1)
            {
                if (SmtpList[SIndex].Name == "Other")
                {
                    SmtpList[SIndex].Url = tbHost.Text.Trim();
                    SmtpList[SIndex].Port = int.Parse(tbPort.Text);
                    SmtpList[SIndex].Url = tbHost.Text.Trim();
                }

                FluentEmailLogic.SendEmailToGroupWithAttachment(SmtpList[SIndex], g.Members, tbSubject.Text, tbBody.Text, fileName);
            }
        }

        private void SaveRFT()
        {
            string reportType = "agenda";

            if (minutes == true)
            {
                reportType = "minutes";
            }

            string path = @"Docs\";
            fileName = path + temp + $" - {reportType}.rtf";

            // Check if the directory exist and if not create it
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            // Save the Document
            TextRange range;
            FileStream fStream;
            range = new TextRange(RTBox.Document.ContentStart, RTBox.Document.ContentEnd);
            fStream = new FileStream(fileName, FileMode.Create);
            range.Save(fStream, DataFormats.Rtf);
            fStream.Close();
        }

        private void lbFilename_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (fileName != "")
            {
                FileInfo fileInfo = new FileInfo(fileName);
                var folderPath = Path.GetDirectoryName(fileName);

                if (Directory.Exists(folderPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        Arguments = folderPath,
                        FileName = "explorer.exe"
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
                }
            }
        }
    }
}