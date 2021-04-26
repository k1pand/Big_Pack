using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Big_Pack
{
    /// <summary>
    /// Логика взаимодействия для Material_Control.xaml
    /// </summary>
    public partial class Material_Control : UserControl
    {
        public Material_Control()
        {
            InitializeComponent();
        }

        public MainWindow MainWindow;
        public string id;

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Edit open = new Edit();
            open.MainWindow = MainWindow;
            MainWindow.Hide();
            open.ShowDialog();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Supplier.Content = "";
            using (SqlConnection connection = new SqlConnection(Connection.String))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($@"SELECT Supplier.Title
                                                       FROM   MaterialSupplier INNER JOIN
                                                              Supplier ON MaterialSupplier.SupplierID = Supplier.ID 
                                                       WHERE MaterialSupplier.MaterialID = {id}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Supplier.Content += reader[0].ToString();
                    }
                }
            }
        }
    }
}
