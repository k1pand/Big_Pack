using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.IO;

namespace Big_Pack
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal void Load_data(string s)
        {
            list.Children.Clear();
            using (SqlConnection connection = new SqlConnection(Connection.String))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"SELECT Material.MaterialTypeID, 
                                                              Material.Title, 
                                                              Material.MinCount, 
                                                              Material.Description, 
                                                              Material.CountInStock,
                                                              Material.Image
                                                    FROM Material", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Material_Control materials = new Material_Control();
                        materials.Type_material.Content = reader[0].ToString();
                        materials.Name_material.Content = reader[1];
                        materials.Min_count.Content = String.Format("{0:D}", Convert.ToInt32(reader[2]));
                        materials.Description.Content = reader[3].ToString();
                        materials.In_Stock.Content = reader[4];
                        //materials.Photo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + reader[4].ToString().Replace(" materials", "materials")));
                        //materials.fon.Background = !(bool)reader[5] ? new SolidColorBrush(Color.FromRgb(229, 229, 229)) : Brushes.Transparent;
                        materials.MainWindow = this;
                        list.Children.Add(materials);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_data("");
        }
    }
}
