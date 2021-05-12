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
            Load_data("");
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Edit open = new Edit();
            open.MainWindow = this;
            open.Show();
            this.Hide();

        }

        

        private void Search_MouseEnter(object sender, MouseEventArgs e)
        {
            Search.Text = "";
        }

        private void Search_MouseLeave(object sender, MouseEventArgs e)
        {
             Search.Text = "Поиск...";
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Load_data("");
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            Scrollcheck.PageUp();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            Scrollcheck.PageDown();
        }
    }
}
