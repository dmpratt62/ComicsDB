using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {
        MainWindow _mainWindow = null;
        public ComicsModel1Container ComicsDB { get; set; }
        public Comic Comic { get; set; }
        public bool IsEditMode { get; set; }
        private Valuation Value { get; set; }

        public DetailsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            ComicsDB = MainWindow.DB;
            IsEditMode = false;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Details page loaded");
            if (ComicsDB != null)
            {
                if (ComicsDB.Publishers.Count() > 0)
                {
                    ComicsDB.Publishers.Load();
                    cbxPublishers.ItemsSource = ComicsDB.Publishers.Local;
                    ComicsDB.StorageBoxes.Load();
                    cbxLocations.ItemsSource = ComicsDB.StorageBoxes.Local;

                    if (Comic != null)
                    {
                        // if the Comic roperty has a PublisherId, find it and make it the selected item
                        if (Comic.PublisherId != 0)
                        {
                            foreach (Publisher pub in ComicsDB.Publishers.Local)
                            {
                                if (pub.PublisherId == Comic.PublisherId)
                                {
                                    cbxPublishers.SelectedItem = pub;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            cbxPublishers.SelectedIndex = 0;
                        }

                        // if the COmic property has a StorageBoxId, find it and make it the selected item.
                        if (Comic.StorageBoxId != 0)
                        {
                            foreach(StorageBox box in ComicsDB.StorageBoxes.Local)
                            {
                                if (box.StorageBoxId == Comic.StorageBoxId)
                                {
                                    cbxLocations.SelectedItem = box;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            cbxLocations.SelectedIndex = 0;
                        }

                        // bind the Comic to the fields group box
                        DataContext = Comic;
                        Value = Comic.Valuations.First();
                        tbEstValue.DataContext = Value;
                        tbEstSource.DataContext = Value;
                        tbEstCondition.DataContext = Value;
                        dpEstDate.DataContext = Value;
                        cbxAuthor.ItemsSource = Comic.Authors;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine($"The main character is now: {Comic.MainCharacter}");
            Console.WriteLine($"The Publisher is now: {Comic.PublisherId}");
            Console.WriteLine($"The location id is {Comic.StorageBoxId}");
            
            if(IsEditMode)
            {
                ComicsDB.Comics.Local.Add(Comic);
                ComicsDB.SaveChanges();
                _mainWindow.ViewComicsTable();
            }

        }
        private void btnDiscard_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ViewComicsTable();
        }

        private void btnEditAuthors_Click(object sender, RoutedEventArgs e)
        {
            AuthorsDialog AuthorsDlg = new AuthorsDialog();
            AuthorsDlg.Owner = _mainWindow;
            AuthorsDlg.Authors = Comic.Authors;

            if (AuthorsDlg.ShowDialog() == true)
            {

            }
        }

    }

    public class PublisherToID : IValueConverter
    {
        /// <summary>
        /// Converts the Comic.PublisherID to a Publisher item
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value is the PublisherId, find the matching object in the DB
            foreach (Publisher pub in MainWindow.DB.Publishers)
            {
                if (pub.PublisherId == (int)value)
                {
                    return pub;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value is the Publisher, return the PublisherId
            return ((Publisher)value).PublisherId;
        }
    }
    public class LocationToID : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (StorageBox box in MainWindow.DB.StorageBoxes)
            {
                if (box.StorageBoxId == (int)value)
                {
                    return box;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StorageBox)value).StorageBoxId;
        }
    }
}
