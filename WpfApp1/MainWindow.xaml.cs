using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace WpfApp10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadProjectsFromFile();
        }
        private void cmNewP(object sender, RoutedEventArgs e)
        {
            labelStrings.Visibility = Visibility.Visible;
            strings.Visibility = Visibility.Visible;
            butonStrings.Visibility = Visibility.Visible;
            otmena.Visibility = Visibility.Visible;
            stringa.Clear();
            labelStringa.Visibility = Visibility.Hidden;
            stringa.Visibility = Visibility.Hidden;
            butonStringa.Visibility = Visibility.Hidden;
        }
        string s;
        private void strings_TextChanged(object sender, TextChangedEventArgs e)
        {
            s = ((TextBox)sender).Text;
        }
        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();
        private void cmAdd(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(s))
            {
                MessageBox.Show("Вы не ввели данные");
                return;
            }
            string[] mas = s.Split(',');
            if (mas.Length > 3 || mas.Length < 3)
            {
                MessageBox.Show("Введены некорректные данные, попробуйте снова"); return;
            }
            string nameUsluga = mas[0].Trim();
            string clientName = mas[1].Trim();
            string status = mas[2].Trim();
            Projects.Add(new Project(nameUsluga, clientName, status));
            strings.Clear();
        }
        private void cmDeletProject(object sender, RoutedEventArgs e)
        {
            labelStringa.Visibility = Visibility.Visible;
            stringa.Visibility = Visibility.Visible;
            butonStringa.Visibility = Visibility.Visible;
            otmena.Visibility = Visibility.Visible;
            strings.Clear();
            labelStrings.Visibility = Visibility.Hidden;
            strings.Visibility = Visibility.Hidden;
            butonStrings.Visibility = Visibility.Hidden;
        }
        string p;
        private void stringa_TextChanged(object sender, TextChangedEventArgs e)
        {
            p = ((TextBox)sender).Text;
        }
        private void cmOtmena(object sender, RoutedEventArgs e)
        {
            stringa.Clear();
            strings.Clear();
            labelStrings.Visibility = Visibility.Hidden;
            strings.Visibility = Visibility.Hidden;
            butonStrings.Visibility = Visibility.Hidden;
            otmena.Visibility = Visibility.Hidden;
            labelStringa.Visibility = Visibility.Hidden;
            stringa.Visibility = Visibility.Hidden;
            butonStringa.Visibility = Visibility.Hidden;
        }
        private void cmDelet(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(p))
            {
                MessageBox.Show("Вы не ввели данные");
                return;
            }
            string[] masiv = p.Split(',');
            if (masiv.Length > 1 || masiv.Length < 1)
            {
                MessageBox.Show("Введены некорректные данные, попробуйте снова"); return;
            }
            if (int.TryParse(masiv[0], out int idd))
            {
                idd = int.Parse(masiv[0]);
            }
            else { MessageBox.Show("Значение должно быть числом!"); return; }
            var toRemove = Projects.FirstOrDefault(p => p.Id == idd);
            if (toRemove != null)
            {
                Projects.Remove(toRemove);
                MessageBox.Show($"Проект с ID={idd} успешно удалён.");
            }
            else
            {
                MessageBox.Show($"Нет проекта с ID={idd}. Проверьте введённое значение."); return;
            }
            nextId--;
            stringa.Clear();
        }
        private void SaveProjectsToFile()
        {
            string filePath = "projects.csv";
            List<string> lines = new List<string>();

            foreach (var project in Projects)
            {
                lines.Add($"\"{project.NameUsluga}\",\"{project.ClientName}\",\"{project.Status}\"");
            }

            File.WriteAllLines(filePath, lines);
        }
        private void LoadProjectsFromFile()
        {
            string filePath = "projects.csv";
            if (File.Exists(filePath))
            {
                string[] data = File.ReadAllLines(filePath);
                foreach (var line in data)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {

                        string nameUsluga = parts[0].Trim('"');
                        string clientName = parts[1].Trim('"');
                        string status = parts[2].Trim('"');
                        Projects.Add(new Project(nameUsluga, clientName, status));
                    }
                }
                if (Projects.Count > 0)
                {
                    nextId = Projects.Max(project => project.Id) + 1;
                }
            }
        }

        private void cmSave(object sender, RoutedEventArgs e)
        {
            SaveProjectsToFile();
            MessageBox.Show("Данные успешно сохранены!");
        }
        public static int nextId = 1;
    }
    public class Project
    {
        public int Id { get; }
        public string NameUsluga { get; set; }
        public string ClientName { get; set; }
        public string Status { get; set; }
        public Project(string NameUsluga, string ClientName, string Status)
        {
            Id = MainWindow.nextId++;
            this.NameUsluga = NameUsluga;
            this.ClientName = ClientName;
            this.Status = Status;
        }
    }
}
