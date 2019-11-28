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
using System.Data.Entity;
//using System.ComponentModel;
using System.Collections.Specialized;


namespace ComicsDB
{
    /// <summary>
    /// Interaction logic for StorageBoxes.xaml
    /// </summary>
    public partial class StorageBoxesDialog : Window
    {
        public ComicsModel1Container ComicsDB { get; set; }

        public StorageBoxesDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ComicsDB.StorageBoxes.Count() > 0)
            {
                ComicsDB.StorageBoxes.Load();
            }
            // load the local dataview into the data grid
            dgStorageBoxes.ItemsSource = ComicsDB.StorageBoxes.Local;
            // add an event handler for the collection changed event
            ComicsDB.StorageBoxes.Local.CollectionChanged += OnStorageBoxCollectionChanged;
        }

        /// <summary>
        /// Handles the collection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="aArgs"></param>
        void OnStorageBoxCollectionChanged(Object sender, NotifyCollectionChangedEventArgs aArgs)
        {
            switch(aArgs.Action)
            {
                // Handle the remove action to update the databes when the user deletes an entry
                case NotifyCollectionChangedAction.Remove:
                    Console.WriteLine("Removed storage box number {0}", ((StorageBox)aArgs.OldItems[0]).Number);
                    ComicsDB.StorageBoxes.Remove((StorageBox)aArgs.OldItems[0]);
                    break;
            }
        }


        private void DgStorageBoxes_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Console.WriteLine("Cell changed");
            if (e.AddedCells.Count > 0)
            {
                DataGridCellInfo info = e.AddedCells[0];
                StorageBox box = e.AddedCells[0].Item as StorageBox;
                //just save the changed to the default view collection
                try
                {
                    ComicsDB.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Caught exception on ComicsDB Save Changes [{0}]", ex.Message);
                }
            }
        }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Handles the preview of keydown to catch a delete key press in the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgStorageBoxes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                if (e.Key == Key.Delete)
                {
                    // user is attempting to delete the row
                    DataGridRow dgr = dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex) as DataGridRow;
                    // don't delete row if user is just editing the contenets when they hit delete
                    if (!dgr.IsEditing)
                    {
                        if (dg.SelectedItem.GetType() == typeof(StorageBox))
                        {
                            // check the database to make sure the record being delete isnt referenced elsewhere in the db
                            int SelectedID = ((StorageBox)dg.SelectedItem).StorageBoxId;
                            // query to find any entries int eh comics db with this storage id
                            var FoundStorageBox = from item in ComicsDB.Comics
                                                  where item.StorageBoxId.Equals(SelectedID)
                                                  select item;
                            // convret the query to a list
                            List<Comic> foundItems = FoundStorageBox.ToList();
                            // marke the event handler as handled if any itmes in the list
                            e.Handled = (foundItems.Count > 0);
                        }
                        else e.Handled = true;
                    }
                }
            }
        }

        private void DgStorageBoxes_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            StorageBox item = (StorageBox)e.NewItem;
            item.Number = 1;
            item.Address = "New Address";
        }
    }
}
