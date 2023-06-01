using MySqlConnector;
using System;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Eurovizio
{
    public partial class MainView : Window
    {
        private string dbString = "datasource = 127.0.0.1; port=3306;username=root;password=qqq111;database=eurovizio";
        MySqlConnection dbConnection = new MySqlConnection();
        ObservableCollection<Singer> singerList = new ObservableCollection<Singer>();
        
        public MainView()
        {
            InitializeComponent();
            OpenDbConnection();
            LoadDataGrid();
            CloseDbConnection();
        }

        public void OpenDbConnection()
        {
            if (dbConnection != null)
            {
                dbConnection = new MySqlConnection(dbString);
            }
            dbConnection.Open();
        }

        public void LoadDataGrid()
        {
            MySqlCommand command = new MySqlCommand("SELECT * FROM dal", dbConnection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                singerList.Add(new Singer(Int32.Parse(reader.GetValue(1).ToString()), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), Int32.Parse(reader.GetValue(5).ToString()), Int32.Parse(reader.GetValue(6).ToString())));
            }
            gridSingers.ItemsSource = singerList;
            gridSingers.SelectedIndex = 0;
        }

        public void CloseDbConnection()
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }
        
        private void QueryFour(object sender, RoutedEventArgs e)
        {
            OpenDbConnection();
            int singerCount = 0;
            int topPosition = 0;
            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) from DAL where orszag = @orszag", dbConnection);
            cmd.Parameters.AddWithValue("@orszag", "Magyarország");
            MySqlDataReader readData = cmd.ExecuteReader();
            while(readData.Read())
            {
                singerCount = Int32.Parse(readData.GetValue(0).ToString());
            }
            readData.Close();
            cmd = new MySqlCommand("SELECT helyezes from DAL where orszag = @orszag ORDER BY helyezes ASC LIMIT 1;", dbConnection);
            cmd.Parameters.AddWithValue("@orszag", "Magyarország");
            readData = cmd.ExecuteReader();
            while (readData.Read())
            {
                topPosition = Int32.Parse(readData.GetValue(0).ToString());
            }
            CloseDbConnection();
            MessageBox.Show($"Magyarországi versenyzők száma: {singerCount}, legmagasabb helyezés: {topPosition}");
        }

        private void QueryFive(object sender, RoutedEventArgs e)
        {
            OpenDbConnection();
            float avgPoints = 0;
            MySqlCommand cmd = new MySqlCommand("SELECT ROUND(AVG(pontszam), 2) from DAL where orszag = @orszag;", dbConnection);
            cmd.Parameters.AddWithValue("@orszag", "Németország");
            MySqlDataReader readData = cmd.ExecuteReader();
            while (readData.Read())
            {
                avgPoints = float.Parse(readData.GetValue(0).ToString());
            }
            CloseDbConnection();
            MessageBox.Show($"Németországi versenyzők átlagos pontszáma: {avgPoints}");
        }

        private void QuerySix(object sender, RoutedEventArgs e)
        {
            OpenDbConnection();
            List<string> songsWithLuck = new List<string>();
            MySqlCommand cmd = new MySqlCommand("SELECT eloado, cim from dal WHERE cim LIKE '%luck%';", dbConnection);
            MySqlDataReader readData = cmd.ExecuteReader();
            while (readData.Read())
            {
                songsWithLuck.Add(readData.GetValue(0).ToString() + " - " + readData.GetValue(1).ToString());
            }
            CloseDbConnection();
            string songs = "";
            foreach (string s in songsWithLuck)
            {
                songs += s + "\n";
            }
            MessageBox.Show(songs);
        }

        private void QuerySeven(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> foundSongs = new ObservableCollection<string>();

            OpenDbConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT cim from dal WHERE eloado LIKE @input ORDER BY eloado ASC, cim ASC;", dbConnection);
            cmd.Parameters.AddWithValue("@input", "%" + textBoxSinger.Text + "%");
            MySqlDataReader readData = cmd.ExecuteReader();
            while (readData.Read())
            {
                foundSongs.Add(readData.GetValue(0).ToString());
            }
            CloseDbConnection();
            listBoxSinger.ItemsSource = foundSongs;
        }

        private void gridSingersSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenDbConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT datum FROM verseny WHERE ev = @ev", dbConnection);
            cmd.Parameters.AddWithValue("@ev", singerList[gridSingers.SelectedIndex].Year);
            MySqlDataReader readData = cmd.ExecuteReader();
            while (readData.Read())
            {
                lblContestDate.Content = readData.GetValue(0).ToString().Split(" ")[0];
            }
            CloseDbConnection();
        }

    }
}
