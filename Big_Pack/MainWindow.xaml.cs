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
                SqlCommand command = new SqlCommand($@"SELECT Material.Type, 
                                                              Material.Title, 
                                                              Material.MinCount, 
                                                              Material.MainImagePath, 
                                                              Material.IsActive, 
                                                              Manufacturer.Name
                                                    FROM Material INNER JOIN Manufacturer 
                                                    ON Material.ManufacturerID = Manufacturer.ID 
                                                    WHERE Material.Title like '{search.Text}%'" + s, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Yacheyka products = new Yacheyka();
                        products.name.Text = reader[0].ToString();
                        products.id.Content = reader[1];
                        products.price.Content = String.Format("{0:D}", Convert.ToInt32(reader[2]));
                        products.description.Text = reader[3].ToString();
                        products.photo.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + reader[4].ToString().Replace(" Товары салона красоты", "Товары салона красоты")));
                        products.fon.Background = !(bool)reader[5] ? new SolidColorBrush(Color.FromRgb(229, 229, 229)) : Brushes.Transparent;
                        products.MainWindow = this;
                        products.manufactor.Content = reader[6];
                        list.Children.Add(products);
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
