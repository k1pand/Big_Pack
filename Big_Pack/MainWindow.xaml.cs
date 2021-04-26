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
                SqlCommand command = new SqlCommand($@"SELECT Material.ID,  
                                                              MaterialType.TitleofMaterial, 
                                                              Material.Title, 
                                                              Material.MinCount, 
                                                              Material.Description, 
                                                              Material.CountInStock, 
                                                              Material.Image
                                                       FROM   MaterialType INNER JOIN
                                                              Material ON MaterialType.ID = Material.MaterialTypeID", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Material_Control materials = new Material_Control();
                        materials.id = reader[0].ToString();
                        materials.Type_material.Content = reader[1].ToString();
                        materials.Name_material.Content = reader[2];
                        materials.Min_count.Content = String.Format("{0:D}", Convert.ToInt32(reader[3]));
                        materials.Description.Content = reader[4].ToString();
                        materials.In_Stock.Content = reader[5];
                        try
                        {
                            materials.Photo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + reader[6].ToString().Replace(" materials ", "materials")));
                        }
                        catch 
                        {
                        }
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

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            Search.Text = "";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Edit open = new Edit();
            open.MainWindow = this;
            open.Show();
            this.Hide();

        }
    }
}
