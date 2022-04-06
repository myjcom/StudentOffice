using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using StudentOffice.Entities;
using StudentOffice.Utils;
using StudentOffice.DbCtx;
using NLog;
using System.Linq;

namespace StudentOffice
{
    public partial class NewStudent
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Student newStudent = new();
        public ClientDbContext Context;
        private bool isSaved;

        public NewStudent(ref ClientDbContext ctx)
        {
            InitializeComponent();
            Context = ctx;
            Closing += CheckBeforeClosed;

            newStudent.Passport = new();
            newStudent.Representant = new();
            newStudent.Representant.Passport = new();
            newStudent.Contract = new();
            newStudent.Faculty = new();
            newStudent.Specialization = new();
            newStudent.Course = new();


            DataContext = newStudent;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            docdoc.Items.Add("Свидетельство о рождении");
            docdoc.Items.Add("Доверенность");
            docdoc.Items.Add("Другое");

            gender.Items.Add("М");
            gender.Items.Add("Ж");

            genderR.Items.Add("М");
            genderR.Items.Add("Ж");

            docType.Items.Add("Паспорт");
            repDoc.Items.Add("Паспорт");

            sitizenship.Items.Add("РФ");
            sitizenshipR.Items.Add("РФ");

            
            dep.ItemsSource = (from o in Context.Faculites
                               select o.FacName).ToList();
            course.ItemsSource = (from o in Context.Courses
                                  select o.CourseNumber).ToList();
        }

        private void OverRep_Checked(object sender, RoutedEventArgs e)
        {
            rep.Visibility = overRep.IsChecked == true ? Visibility.Visible : Visibility.Hidden;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(!CheckFields())
            {
                return;
            }
            
            if (overRep.IsChecked == true)
            {
                newStudent.Representant.RepFullName = newStudent.Representant.Passport.Family
                + " " + newStudent.Representant.Passport.Name + " "
                + newStudent.Representant.Passport.Patronymic;
            }
            else
            {
                newStudent.Representant.Passport = null;
                newStudent.Representant = null;
            }

            newStudent.FullName = newStudent.Passport.Family + " " + newStudent.Passport.Name + " " + newStudent.Passport.Patronymic;


            newStudent.Faculty = Context.Faculites.First(f => f.FacName == newStudent.Faculty.FacName);
            newStudent.Specialization = Context.Specializations.First(f => f.SpecName == newStudent.Specialization.SpecName);
            newStudent.Course = Context.Courses.First(f => f.CourseNumber == newStudent.Course.CourseNumber);


            ((MainWindow)Owner).AddClient(newStudent);
            isSaved = true;
            Close();
        }

        private void CheckBeforeClosed(object sender, CancelEventArgs e)
        {
            if (!isSaved)
            {
                MessageBoxResult result = MessageBox.Show("Закрыть окно? (данные не сохранятся)", "Закрыть", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                MessageBox.Show("Клиент добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void CheckAny(ref TextBox box, ref bool result, Func<string, bool> pred)
        {
            
            if (ValidateValue(box.Text, pred))
            {
                Logger.Debug($"is valid {box.Text}");
                SetBgColor(box, System.Windows.Media.Brushes.LightGreen);
            }
            else
            {
                Logger.Debug($"invalid {box.Text}");
                SetBgColor(box, System.Windows.Media.Brushes.Red);
                result &= false;
            }
        }

        private void CheckAny(ref Xceed.Wpf.Toolkit.MaskedTextBox box, ref bool result, Func<string, bool> pred)
        {

            if (ValidateValue(box.Text, pred))
            {
                //Logger.Debug($"is valid value: {box.Text}");
                SetBgColor(box, System.Windows.Media.Brushes.White);
            }
            else
            {
                //Logger.Debug($"invalid value: {box.Text}");
                SetBgColor(box, System.Windows.Media.Brushes.Red);
                result &= false;
            }
        }


        private bool CheckFields()
        {
            bool result = true;
            CheckAny(ref family, ref result, Checker.IsValidName);
            CheckAny(ref name, ref result, Checker.IsValidName);
            CheckAny(ref patron, ref result, Checker.IsValidName);

            CheckAny(ref series, ref result, Checker.IsValidNumber);
            CheckAny(ref number, ref result, Checker.IsValidNumber);

            CheckAny(ref regaddr, ref result, Checker.IsValidAdress);

            CheckAny(ref brithdate, ref result, Checker.IsValidDate);
            CheckAny(ref dateIs, ref result, Checker.IsValidDate);
            CheckAny(ref email, ref result, Checker.IsValidEmail);
            CheckAny(ref phone, ref result, Checker.IsValidPhone);

            CheckAny(ref location, ref result, Checker.IsValidAdress);
            CheckAny(ref code, ref result, Checker.IsValidCode);

            CheckAny(ref conNumber, ref result, Checker.IsValidAdress);

            CheckAny(ref conDate, ref result, Checker.IsValidDate);

            CheckAny(ref dateAdm, ref result, Checker.IsValidDate);

            if (overRep.IsChecked.Value)
            {
                CheckAny(ref familyR, ref result, Checker.IsValidName);
                CheckAny(ref nameR, ref result, Checker.IsValidName);
                CheckAny(ref patronR, ref result, Checker.IsValidName);

                CheckAny(ref seriesR, ref result, Checker.IsValidNumber);
                CheckAny(ref numberR, ref result, Checker.IsValidNumber);

                CheckAny(ref regaddrR, ref result, Checker.IsValidAdress);
                CheckAny(ref locationR, ref result, Checker.IsValidAdress);
                CheckAny(ref codeR, ref result, Checker.IsValidCode);
                CheckAny(ref brithdateR, ref result, Checker.IsValidDate);
                CheckAny(ref dateIsR, ref result, Checker.IsValidDate);
                CheckAny(ref emailR, ref result, Checker.IsValidEmail);
                CheckAny(ref phoneR, ref result, Checker.IsValidPhone);
                CheckAny(ref docIs, ref result, Checker.IsValidAdress);
            }


            return result;
        }

        public static bool ValidateValue(string val, Func<string, bool> pred)
        {
            return !string.IsNullOrEmpty(val) && pred(val);
        }

        private static void SetBgColor(object tb, System.Windows.Media.Brush color)
        {
            ((TextBox)tb).Background = color;
        }

        private void Xbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Background = System.Windows.Media.Brushes.White;
        }

        private void Dep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fac.ItemsSource = (from f in Context.Specializations where f.Faculty.FacName == dep.SelectedItem.ToString() select f.SpecName).ToList();
            fac.SelectedIndex = 0;
        }
    }
}
