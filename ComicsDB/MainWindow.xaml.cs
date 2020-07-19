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
using System.Data.Entity;

namespace ComicsDB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DetailsPage _detailsPage;
        TablePage _tablePage;

        public static ComicsModel1Container DB { get; set; } = new ComicsModel1Container();

        public MainWindow()
        {
            InitializeComponent();

            int count = DB.Comics.Count<Comic>();
            if (count == 0)
            {
                //Open the New Entry window to add an entry to the table
            }
            else
            {
                DB.Comics.Load();
            }
        }

        private void MnuPublishers_Click(object sender, RoutedEventArgs e)
        {
            PublishersDialog dlg = new PublishersDialog();
            dlg.Owner = this;
            dlg.ComicsDB = DB;
            if (dlg.ShowDialog() == true)
            {
                // do something here if necessary
            }
            
        }

        private void MnuStorage_Click(object sender, RoutedEventArgs e)
        {
            StorageBoxesDialog dlg = new StorageBoxesDialog();
            dlg.Owner = this;
            dlg.ComicsDB = DB;
            if (dlg.ShowDialog() == true)
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // add the default page to the frame
            _detailsPage = new DetailsPage(this);
            _tablePage = new TablePage(this);

            frmMainFrame.NavigationService.Navigate(_tablePage);
            mnuTableView.IsChecked = true;
        }

        private void MnuTableView_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Table view clicked");
            frmMainFrame.NavigationService.Navigate(_tablePage);
            mnuDetailsView.IsChecked = false;
        }

        private void MnuDetailsView_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Details view clicked");
            
            // get the comic from the selected item in the tables datagrid

            frmMainFrame.NavigationService.Navigate(_detailsPage);
            mnuTableView.IsChecked = false;
        }

        public void ViewComicDetails(Comic comic)
        {
            _detailsPage.Comic = comic;
            frmMainFrame.NavigationService.Navigate(_detailsPage);
        }

        public void EditComicDetails(Comic comic)
        {
            _detailsPage.IsEditMode = true;
            _detailsPage.Comic = comic;
            frmMainFrame.NavigationService.Navigate(_detailsPage);
            if (!mnuDetailsView.IsChecked) mnuDetailsView.IsChecked = true;
            if (mnuTableView.IsChecked) mnuTableView.IsChecked = false;
        }
        public void ViewComicsTable()
        {
            frmMainFrame.NavigationService.Navigate(_tablePage);
        }
    }
}
