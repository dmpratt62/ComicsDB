using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace ComicsDB
{
    /// <summary>
    /// Interaction logic for AuthorsDialog.xaml
    /// </summary>
    public partial class AuthorsDialog : Window
    {
        public ICollection<Author> Authors { get; set; }
        public Comic PresentComic { get; set; }

        public AuthorsDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // attach the datagrid to the comics list of authors.
            dgAuthors.ItemsSource = Authors;
        }
    }
}
