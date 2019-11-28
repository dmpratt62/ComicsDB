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

namespace ComicsDB
{
    /// <summary>
    /// Interaction logic for TablePage.xaml
    /// </summary>
    public partial class TablePage : Page
    {
        MainWindow _mainWindow = null;
        public ComicsModel1Container ComicsDB { get; set; }

        public TablePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            ComicsDB = MainWindow.DB;
            // set the data grid source
            if (ComicsDB != null)
            {
                dgComics.ItemsSource = ComicsDB.Comics.Local;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            // send a new comic to the details view
            Comic newComic = new Comic();
            newComic.StorageBoxId = 4;
            newComic.MainCharacter = "Main Character";
            newComic.PublishDate = System.DateTime.Today;
            Author aut = new Author();
            aut.FirstName = "Bob";
            aut.LastName = "Darterita";
            newComic.Authors.Add(aut);

            _mainWindow.EditComicDetails(newComic);
        }
    }
}
