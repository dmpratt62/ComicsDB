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
using System.Windows.Shapes;
using System.ComponentModel;

namespace ComicsDB
{
    /// <summary>
    /// Interaction logic for AddPublisher.xaml
    /// </summary>
    public partial class AddPublisherDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Publisher AddedPublisher { get; } = new Publisher();

        public AddPublisherDialog()
        {
            InitializeComponent();
            AddedPublisher.Name = "";
            DataContext = AddedPublisher;
            //tbName.DataContext = AddedPublisher;
            //tbUrl.DataContext = AddedPublisher;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //using (ComicsModel1Container db = new ComicsModel1Container())
            //{
            //    db.Publishers.Add(AddedPublisher);
            //    db.SaveChanges();
            //    DialogResult = true;
            //}
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
