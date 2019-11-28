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
using System.Data.Entity;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace ComicsDB
{
    /// <summary>
    /// Interaction logic for Publishers.xaml
    /// </summary>
    public partial class PublishersDialog : Window, INotifyPropertyChanged
    {
        // interface events
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // the data context for the comics database
        public ComicsModel1Container ComicsDB { get; set; }

        public PublishersDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(ComicsDB.Publishers.Count() > 0)
            {
                ComicsDB.Publishers.Load();
            }

            // load the observable collection to the data grid
            dgPublishers.ItemsSource = ComicsDB.Publishers.Local;

            ComicsDB.Publishers.Local.CollectionChanged += OnPublisherCollectionChanged;
        }

        /// <summary>
        /// Event handler for the Collectionchanged event of ComicsDB.Publishers.Local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="aArgs"></param>
        void OnPublisherCollectionChanged(Object sender, NotifyCollectionChangedEventArgs aArgs)
        {
            switch (aArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Console.WriteLine("NotifyCollectionChangedAction.Add");
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    Console.WriteLine("Removed publisher returned new publisher name {0}", ((Publisher)aArgs.OldItems[0]).Name);
                    // remove the publisher from the Publishers database
                    //ComicsDB.Publishers.Remove((Publisher)aArgs.OldItems[0]);
                    ComicsDB.Publishers.Local.Remove((Publisher)aArgs.OldItems[0]);
                    ComicsDB.SaveChanges();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    Console.WriteLine("NotifyCollectionChangedAction.Replace");
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Console.WriteLine("NotifyCollectionChangedAction.Reset");
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Handles the user edit of a cel contents, commits the changes to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgPublishers_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Console.WriteLine("Cell changed");
            if (e.AddedCells.Count > 0)
            {
                try
                {
                    //just save the changed to the default view collection
                    //ComicsDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Caught exception on ComicsDB Save Changes [{0}]", ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the preview keydown to catch the Delete key in the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgPublishers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                if (e.Key == Key.Delete)
                {
                    // User is attempting to delete the row
                    DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                    if (!dgr.IsEditing)
                    {
                        if (dg.SelectedItem.GetType() == typeof(Publisher))
                        {
                            Console.WriteLine("Attmpting to delete a publisher");
                            // check the database to verify if this publisher has a reference
                            int SelectedID = ((Publisher)dg.SelectedItem).PublisherId;
                            // query to find entries in Comics with the same selected PublisherId
                            var FoundPublishers = from item in ComicsDB.Comics
                                                  where item.PublisherId.Equals(SelectedID)
                                                  select item;
                            // runs the query and loads results into a list
                            List<Comic> found = FoundPublishers.ToList();
                            // mark the event handled to prevent deletion of a found record
                            if (found.Count > 0)
                            {
                                e.Handled = true;
                                // warn the user that a used publisher is being deleted
                            }
                        }
                        else e.Handled = true;  // don't delete the non match
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the name of a new Publisher (which cannot be null)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgPublishers_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            Publisher item = ((Publisher)e.NewItem);
            item.Name = "New Publisher";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // try adding a row
            Publisher newPublisher = new Publisher();
            newPublisher.Name = "New Publisher";
            ComicsDB.Publishers.Local.Add(newPublisher);
            // save and reload to get the table index updated for the new item
            ComicsDB.SaveChanges();
            ComicsDB.Publishers.Load();
            dgPublishers.ItemsSource = ComicsDB.Publishers.Local;

            // move the focus to the new item.
            dgPublishers.UpdateLayout();
            dgPublishers.SelectedIndex = dgPublishers.Items.Count - 1;
            dgPublishers.ScrollIntoView(dgPublishers.SelectedItem);
            DataGridRow row = (DataGridRow)dgPublishers.ItemContainerGenerator.ContainerFromItem(dgPublishers.SelectedItem);
            if (row != null)
            {
                bool success = row.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                Console.WriteLine("MoveFocus returned {0}", success);
            }
            else Console.WriteLine("row is null");
        }

        //private void BtnRemove_Click(object sender, RoutedEventArgs e)
        //{
        //    // check the database to verify if this publisher has a reference
        //    int SelectedID = ((Publisher)dgPublishers.SelectedItem).PublisherId;
        //    // query to i find entries in Comics with the same selected PublisherId
        //    var FoundPublishers = from item in ComicsDB.Comics
        //                          where item.PublisherId.Equals(SelectedID)
        //                          select item;
        //    // runs the query and loads results into a list
        //    List<Comic> found = FoundPublishers.ToList<Comic>();

        //    if (found.Count > 0)
        //    {
        //        // tell the user the publisher cannot be removed

        //    }
        //    else
        //    {
        //        // remove the publisher from the Publishers database
        //        ComicsDB.Publishers.Remove((Publisher)dgPublishers.SelectedItem);
        //        ComicsDB.SaveChanges();
        //    }

        //}

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
