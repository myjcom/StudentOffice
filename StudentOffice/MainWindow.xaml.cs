using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using StudentOffice.DbCtx;
using StudentOffice.Entities;
using Word = Microsoft.Office.Interop.Word;
using System;
using System.Windows.Controls;
using StudentOffice.Settings;
using Newtonsoft.Json;
using System.IO;

namespace StudentOffice
{
    public partial class MainWindow : Window
    {

        private Word.Application application;
        private Word.Document document;

        private CollectionViewSource studentsViewSource;
        private CollectionViewSource repViewSource;

        private ClientDbContext _context; 

        public readonly Config appConfig;
        private readonly JsonSerializer serializer;

        public void AddClient(Student client)
        {
            _context.Add(client);
            _context.SaveChanges();
            clientDataGrid.Items.Refresh();
        }
        public MainWindow()
        {
            InitializeComponent();
            appConfig = new Config();
            serializer = new JsonSerializer();
            using (StreamReader sr = new("config.json"))
            using (JsonReader reader = new JsonTextReader(sr))
            {

                appConfig = serializer.Deserialize<Config>(reader);
            }
            _context = new ClientDbContext(appConfig.DbFileName);
            studentsViewSource = (CollectionViewSource)FindResource(nameof(studentsViewSource));
            repViewSource = (CollectionViewSource)FindResource(nameof(repViewSource));
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
            _context.Clients.Load();
            _context.Representant.Load();
            studentsViewSource.Source = _context.Clients.Local.ToObservableCollection();
            repViewSource.Source = _context.Representant.Local.ToObservableCollection();

            searchCBox.Items.Add("ФИО");
            searchCBox.Items.Add("ФИО Представителя");
            searchCBox.Items.Add("Телефон");
            searchCBox.Items.Add("Email");

            printDoc.Items.Add("Договор");
            printDoc.Items.Add("Справка");

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
            // this forces the grid to refresh to latest values
            clientDataGrid.Items.Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            using (StreamWriter file = File.CreateText("config.json"))
            {
                serializer.Serialize(file, appConfig);
            }
            _context.Dispose();
            base.OnClosing(e);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var NewStudent = new NewStudent(ref _context)
            {
                Owner = this
            };
            _ = NewStudent.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string sData = searchData.Text.Trim();

            switch (searchCBox.SelectedIndex)
            {
                case 0:
                    clientDataGrid.Items.Filter = new System.Predicate<object>(item => ((Student)item).FullName.Contains(sData));
                    break;
                case 1:
                    clientDataGrid.Items.Filter = new System.Predicate<object>(
                        item =>
                        {
                            return ((Student)item).Representant != null && ((Student)item).Representant.RepFullName.Contains(sData);
                        });
                    break;
                case 2:
                    clientDataGrid.Items.Filter = new System.Predicate<object>(item => ((Student)item).Phone.Contains(sData));
                    break;
                case 3:
                    clientDataGrid.Items.Filter = new System.Predicate<object>(item => ((Student)item).Email.Contains(sData));
                    break;
                default:
                    break;
            }
        }

        private void ClientDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (clientDataGrid.IsReadOnly)
            {
                var selItem = ((Student)clientDataGrid.SelectedItem)?.Representant;

                repDataGrid.Items.Filter = selItem != null
                    ? new Predicate<object>(item => ((Representant)item).RepFullName.Contains(selItem.RepFullName))
                    : new Predicate<object>(item => ((Representant)item).RepId == -1);
            }

        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            application = new Word.Application();

            switch (printDoc.SelectedIndex)
            {
                case 0:
                    PrintContract();
                    break;
                case 1:
                    PrintReference();
                    break;
                default:
                    break;
            }
        }

        private void PrintReference()
        {
            document = application.Documents.Add(appConfig.ReferenceDocFileName);
            document.Activate();

            Student clientData = (Student)clientDataGrid.SelectedItem;
            application.ActiveDocument.Bookmarks["CURRDATE"].Range.Text = DateTime.Now.ToShortDateString();
            application.ActiveDocument.Bookmarks["FIO"].Range.Text = clientData.FullName;
            application.ActiveDocument.Bookmarks["BDATE"].Range.Text = clientData.Passport.BrithDate;
            application.ActiveDocument.Bookmarks["FAC"].Range.Text = clientData.Faculty.FacName;
            application.ActiveDocument.Bookmarks["SPEC"].Range.Text = clientData.Specialization.SpecName;
            application.ActiveDocument.Bookmarks["COURSE"].Range.Text = clientData.Course.CourseNumber.ToString();
            application.ActiveDocument.Bookmarks["DATE"].Range.Text = clientData.Contract.ContractDateAdmission;
            application.ActiveDocument.Bookmarks["CON"].Range.Text = clientData.Contract.ContractNumber;
            application.ActiveDocument.Bookmarks["CONDATE"].Range.Text = clientData.Contract.ContractDate;
            application.Visible = true;
        }

        private void PrintContract()
        {
            document = application.Documents.Add(Directory.GetCurrentDirectory() + "\\" + appConfig.ContractDocFileName);
            document.Activate();

            Student clientData = (Student)clientDataGrid.SelectedItem;
            //Шапка
            application.ActiveDocument.Bookmarks["CONNUMBER"].Range.Text = clientData.Contract.ContractNumber;
            application.ActiveDocument.Bookmarks["CONDATE"].Range.Text = clientData.Contract.ContractDate;

            //Студент
            application.ActiveDocument.Bookmarks["FIO"].Range.Text = clientData.FullName;
            application.ActiveDocument.Bookmarks["BDAY"].Range.Text = clientData.Passport.BrithDate;
            application.ActiveDocument.Bookmarks["ADR"].Range.Text = clientData.Passport.RegistrationAdress;
            application.ActiveDocument.Bookmarks["PHONE"].Range.Text = clientData.Phone;
            application.ActiveDocument.Bookmarks["MAIL"].Range.Text = clientData.Email;

            application.ActiveDocument.Bookmarks["FAC"].Range.Text = clientData.Faculty.FacName;
            application.ActiveDocument.Bookmarks["SPEC"].Range.Text = clientData.Specialization.SpecName;
            application.ActiveDocument.Bookmarks["COURSE"].Range.Text = clientData.Course.CourseNumber.ToString();
            application.ActiveDocument.Bookmarks["DATE"].Range.Text = clientData.Contract.ContractDateAdmission;

            //Паспорт студента
            application.ActiveDocument.Bookmarks["PSN"].Range.Text = 
                $"{clientData.Passport.PassSeries} {clientData.Passport.PassNumber}";

            application.ActiveDocument.Bookmarks["ISSUED"].Range.Text = 
                $"{clientData.Passport.Location} {clientData.Passport.DateOfIssue}";

            application.ActiveDocument.Bookmarks["DCODE"].Range.Text = clientData.Passport.PassCode;

            application.ActiveDocument.Bookmarks["NATION"].Range.Text = clientData.Passport.Sitizenship;


            //Данные представителя (если есть)

            if (clientData.Representant != null)
            {
                FillRepData(ref clientData);
            }

            application.Visible = true;
        }

        private void FillRepData(ref Student clientData)
        {
            
            application.ActiveDocument.Bookmarks["FIOR"].Range.Text = clientData.Representant.RepFullName;
            application.ActiveDocument.Bookmarks["ADDRR"].Range.Text = clientData.Representant.Passport.RegistrationAdress;
            application.ActiveDocument.Bookmarks["PHONER"].Range.Text = clientData.Representant.RepPhone;
            application.ActiveDocument.Bookmarks["MAILR"].Range.Text = clientData.Representant.RepEmail;

            //Паспортные данные представителя
            application.ActiveDocument.Bookmarks["PSNR"].Range.Text =
                $"{clientData.Representant.Passport.PassSeries} {clientData.Representant.Passport.PassNumber}";

            application.ActiveDocument.Bookmarks["ISSUEDR"].Range.Text = clientData.Representant.Passport.Location + 
                " " + clientData.Representant.Passport.DateOfIssue;
            application.ActiveDocument.Bookmarks["CODER"].Range.Text = clientData.Representant.Passport.PassCode;
            application.ActiveDocument.Bookmarks["NATIONR"].Range.Text = clientData.Representant.Passport.Sitizenship;
            application.ActiveDocument.Bookmarks["DOCDOCR"].Range.Text = clientData.Representant.RepDocType
                + " " + clientData.Representant.RepDoc;
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {

            var cmd = ((MenuItem)sender).Name;
            Student cl = (Student)clientDataGrid.SelectedItem;
            string copyText;

            switch (cmd)
            {
                case "all":
                    copyText = $"{cl.FullName} {cl.Phone} {cl.Email}";
                    break;
                case "email":
                    copyText = $"{cl.Email}";
                    break;
                case "phone":
                    copyText = $"{cl.Phone}";
                    break;
                default:
                    copyText = "";
                    break;
            }

            Clipboard.Clear();
            Clipboard.SetText(copyText);
        }
        private void RemMenuItem_Click(object sender, RoutedEventArgs e)
        {
           
            var cl = (Student)clientDataGrid.SelectedItem;
            if (cl != null)
            {
                var ps = cl.Passport;
                if(ps != null)
                {
                    _context.Remove(ps);
                }

                var rep = cl.Representant;
                if(rep != null)
                {
                    var pas = rep.Passport;
                    if(pas != null)
                    {
                        _context.Remove(pas);
                    }
                    _context.Remove(rep);
                }
                _context.Remove(cl);
                _context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Не выбран клиент!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            var cfgWindow = new ConfigWindow
            {
                Owner = this
            };
            cfgWindow.ShowDialog();
        }

        //private void Import_Click(object sender, RoutedEventArgs e)
        //{
        //    var pw = new ProgressWindow();
        //    pw.Show();
        //}

        //private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
