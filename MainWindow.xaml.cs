#region Imports
using Galileo6;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
#endregion
namespace Malin_Space_Science_Systems_Satellite_Data_Processor
{
    #region -->TO DO!<-- Summary
    /// <summary>
    /// 
    /// </summary>
    #endregion
    public partial class MainWindow : Window
    {
        #region Initialisation
        public MainWindow()
        {
            InitializeComponent();
            PopulateComboBoxes();
            DisableSearchTargetTextBox();
            //DisableSearchButtons();
            DisableSortButtons();
        }
        #endregion
        #region Global Methods (4.1 to 4.4)
        #region 4.1 Linked Lists
        /*
        Create two data structures using the LinkedList<T> class from the C# Systems.Collections.Generic
        namespace. The data must be of type “double”; you are not permitted to use any additional classes,
        nodes, pointers or data structures (array, list, etc) in the implementation of this application. 
        The two LinkedLists of type double are to be declared as global within the “public partial class”.
        */

        // Declare two global variables which represent the LinkedLists for Sensor A and Sensor B.
        public LinkedList<double> SensorA_LinkedList = new LinkedList<double>();
        public LinkedList<double> SensorB_LinkedList = new LinkedList<double>();
        #endregion
        #region 4.2 Load Data from DLL into Linked Lists
        /*
        Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate
        reference in the solution explorer. Create a method called “LoadData” which will populate both
        LinkedLists. Declare an instance of the Galileo library in the method and create the appropriate loop
        construct to populate the two LinkedList; the data from Sensor A will populate the first LinkedList,
        while the data from Sensor B will populate the second LinkedList. The LinkedList size will be
        hardcoded inside the method and must be equal to 400. The input parameters are empty, and the
        return type is void.
        */

        // Create a method called “LoadData” which will populate both LinkedLists.
        private void LoadData()
        {
            // Declare an instance of the Galileo library.
            Galileo6.ReadData galileoInstance = new Galileo6.ReadData();

            // Clear the linked lists before they are loaded.
            SensorA_LinkedList.Clear();
            SensorB_LinkedList.Clear();

            // Load selected values for 'sigma' and 'mu' from the combo box.
            double mu = (double) MuComboBox.SelectedItem;
            double sigma = (double) SigmaComboBox.SelectedItem;

            // Populate the two linked lists.
            // The LinkedList size is hardcoded and must be equal to 400.
            for (int i = 0; i < 400; i++)
            {
                SensorA_LinkedList.AddLast(galileoInstance.SensorA(mu, sigma));
                SensorB_LinkedList.AddLast(galileoInstance.SensorB(mu, sigma));
            }
        }
        #endregion
        #region 4.3 Display Linked Lists in List View
        /*
        Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a
        ListView. Add column titles “Sensor A” and “Sensor B” to the ListView. The input parameters are
        empty, and the return type is void.
        */
        private void ShowAllSensorData()
        {
            // Clear the list view before it is loaded.
            SensorDataListView.Items.Clear();

            // Populate the list view with items from linked lists.
            for (int i = 0; i < NumberOfNodes(SensorA_LinkedList); i++)
            {
                SensorDataListView.Items.Add(new { SensorA_ListView = SensorA_LinkedList.ElementAt(i), SensorB_ListView = SensorB_LinkedList.ElementAt(i)});
            }

            // Clear all text boxes
            SensorA_InsertionSortTextBox.Clear();
            SensorB_InsertionSortTextBox.Clear();
            SensorA_SelectionSortTextBox.Clear();
            SensorB_SelectionSortTextBox.Clear();
            SensorA_SearchTargetTextBox.Clear();
            SensorB_SearchTargetTextBox.Clear();
            SensorA_IterativeSearchTextBox.Clear();
            SensorB_IterativeSearchTextBox.Clear();
            SensorA_RecursiveSearchTextBox.Clear();
            SensorB_RecursiveSearchTextBox.Clear();
        }
        #endregion
        #region 4.4 Call methods from 4.2 & 4.3
        /*
        Create a button and associated click method that will call the LoadData and ShowAllSensorData methods. 
        The input parameters are empty, and the return type is void.
        */
        private void LoadSensorDataButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowAllSensorData();
            DisplayListboxData(SensorA_LinkedList, SensorA_ListBox);
            DisplayListboxData(SensorB_LinkedList, SensorB_ListBox);
            //DisableSearchButtons();
            DisableSearchTargetTextBox();
            EnableSortButtons();
        }
        #endregion
        #endregion
        #region Utility Methods (4.5 to 4.6)
        #region 4.5 Count Linked List Elements
        /*
        Create a method called “NumberOfNodes” that will return an integer which is the number of
        nodes(elements) in a LinkedList. The method signature will have an input parameter of type
        LinkedList, and the calling code argument is the linkedlist name.
        */
        private int NumberOfNodes(LinkedList<double> linkedListParameter)
        {
            return linkedListParameter.Count;
        }
        #endregion
        #region 4.6 Display Linked Lists in List Boxes
        /*
        Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the
        appropriate ListBox. The method signature will have two input parameters; a LinkedList, and the
        ListBox name. The calling code argument is the linkedlist name and the listbox name.
        */
        private void DisplayListboxData(LinkedList<double> linkedListParameter, ListBox listBoxParameter)
        {
            // Clear the list view before it is loaded.
            listBoxParameter.Items.Clear();

            // Populate the list view with items from linked lists.
            for (int i = 0; i < NumberOfNodes(SensorA_LinkedList); i++)
            {
                listBoxParameter.Items.Add(linkedListParameter.ElementAt(i));
            }
        }
        #endregion
        #endregion
        #region -->TO DO!<-- Sort and Search Methods (4.7 to 4.10)
        #region 4.7 Sort: Selection
        /*
        Create a method called “SelectionSort” which has a single input parameter of type LinkedList, while
        the calling code argument is the linkedlist name. The method code must follow the pseudo code
        supplied below in the Appendix. The return type is Boolean.
        */
        private bool SelectionSort(LinkedList<double> linkedListParameter)
        {
            int min;
            int max = NumberOfNodes(linkedListParameter);
            for (int i = 0; i < max -1; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++)
                {
                    if (linkedListParameter.ElementAt(j) < linkedListParameter.ElementAt(min))
                    {
                        min = j;
                    }
                }
                LinkedListNode<double> currentMin = linkedListParameter.Find(linkedListParameter.ElementAt(min));
                LinkedListNode<double> currentI = linkedListParameter.Find(linkedListParameter.ElementAt(i));
                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }
            return true;
        }
        #endregion
        #region 4.8 Sort: Insertion
        /*
        Create a method called “InsertionSort” which has a single parameter of type LinkedList, while the
        calling code argument is the linkedlist name. The method code must follow the pseudo code supplied
        below in the Appendix. The return type is Boolean.
        */
        private bool InsertionSort(LinkedList<double> linkedListParameter)
        {
            int max = NumberOfNodes(linkedListParameter);
            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (linkedListParameter.ElementAt(j - 1) > linkedListParameter.ElementAt(j))
                    {
                        LinkedListNode<double> current = linkedListParameter.Find(linkedListParameter.ElementAt(j));
                        var temp = current.Previous.Value;
                        current.Previous.Value = current.Value;
                        current.Value = temp;
                    }
                }
            }
            return true;
        }
        #endregion
        #region 4.9 Search: Iterative
        /*
        Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList,
        SearchValue, Minimum and Maximum. This method will return an integer of the linkedlist element
        from a successful search or the nearest neighbour value. The calling code argument is the linkedlist
        name, search value, minimum list size and the number of nodes in the list. The method code must
        follow the pseudo code supplied below in the Appendix.
        */
        private int BinarySearchIterative(LinkedList<double> linkedListParameter, int searchValueParameter, int minimumParameter, int maximumParameter)
        {
            while (minimumParameter <= maximumParameter - 1)
            {
                int middle = (minimumParameter + maximumParameter) / 2;
                if(searchValueParameter == linkedListParameter.ElementAt(middle))
                {
                    return ++middle;
                }
                else if (searchValueParameter < linkedListParameter.ElementAt(middle))
                {
                    maximumParameter = middle - 1;
                }
                else
                    minimumParameter = middle + 1;
            }
            return minimumParameter;
        }
        #endregion
        #region -->TO DO!<-- 4.10 Search: Recursive
        /*
        Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList,
        SearchValue, Minimum and Maximum. This method will return an integer of the linkedlist element
        from a successful search or the nearest neighbour value. The calling code argument is the linkedlist
        name, search value, minimum list size and the number of nodes in the list. The method code must
        follow the pseudo code supplied below in the Appendix.
        */

        /*
        private bool BinarySearchRecursive()
        {
            return;
        }
        */
        #endregion
        #endregion
        #region -->TO DO!<-- UI Button Methods (4.11 to 4.15)
        #region -->TO DO!<-- 4.11 Buttons: Search
        /*
        Create four button click methods that will search the LinkedList for an integer value entered into a
        textbox on the form. The four methods are:
            1. Method for Sensor A and Binary Search Iterative
            2. Method for Sensor A and Binary Search Recursive
            3. Method for Sensor B and Binary Search Iterative
            4. Method for Sensor B and Binary Search Recursive
        The search code must check to ensure the data is sorted, then start a stopwatch before calling the
        search method. Once the search is complete the stopwatch will stop, and the number of ticks will be
        displayed in a read only textbox. Finally, the code/method will call the “DisplayListboxData” method
        and highlight the search target number and two values on each side
        */

        // Method to reduce repetition.
        private void CountSearchDisplay(Func<LinkedList<double>, int, int, int, int> searchTypeParameter, LinkedList<double> linkedListParameter, int searchValueParameter, int minimumParameter, int maximumParameter, TextBox outputTextBoxParamater, ListBox listBoxParameter)
        {
            // Start timer.
            Stopwatch sw = Stopwatch.StartNew();

            // Search.
            int searchResultIndex = searchTypeParameter(linkedListParameter, searchValueParameter, minimumParameter, maximumParameter);

            // Stop timer.
            sw.Stop();

            // Display sort time in ticks in text box.
            outputTextBoxParamater.Clear();
            outputTextBoxParamater.Text = sw.ElapsedTicks.ToString() + " ticks";

            // Enable multi-select on list box.
            listBoxParameter.SelectionMode = SelectionMode.Multiple;

            // Clear list box.
            listBoxParameter.SelectedItems.Clear();

            // Select list box items.
            // Highlight the search target number and two values on each side.
            // Done in stages to aviod calling an index that does not exist, which would cause a crash.
            switch (searchResultIndex)
            {
                // 2 values on the greater side.
                case (0):
                    SelectListBoxItems(listBoxParameter, 0, 2, searchResultIndex);
                    break;
                // 1 value on the lesser side, 2 on the greater.
                case (1):
                    SelectListBoxItems(listBoxParameter, -1, 2, searchResultIndex);
                    break;
                // 2 values on each side.
                case (<398):
                    SelectListBoxItems(listBoxParameter, -2, 2, searchResultIndex);
                    break;
                // 2 values on the lesser side, 1 on the greater.
                case (398):
                    SelectListBoxItems(listBoxParameter, -2, 1, searchResultIndex);
                    break;
                // 2 values on the lesser side.
                case (399):
                    SelectListBoxItems(listBoxParameter, -2, 0, searchResultIndex);
                    break;
            }
        }

        // Selects list box items.
        // minParameter represents the index values to be printed to be selected that are less than the selected value,
        // and maxParameter are those that are greater.
        // This must be done carefully to avoid calling an index that does not exist, which would cause a crash.
        private void SelectListBoxItems(ListBox listBoxParameter, int minParameter, int maxParameter, int searchResultIndexParameter)
        {
            for(int i = minParameter; i <= maxParameter; i++)
                listBoxParameter.SelectedItems.Add(listBoxParameter.Items.GetItemAt(searchResultIndexParameter + i));
        }

        // 1. Method for Sensor A and Binary Search Iterative.
        private void SensorA_IterativeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            CountSearchDisplay(BinarySearchIterative, SensorA_LinkedList, Int32.Parse(SensorA_SearchTargetTextBox.Text), 0, NumberOfNodes(SensorA_LinkedList)-1, SensorA_IterativeSearchTextBox, SensorA_ListBox);
        }

        // 2. Method for Sensor A and Binary Search Recursive.
        private void SensorA_RecursiveSearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // 3. Method for Sensor B and Binary Search Iterative.
        private void SensorB_IterativeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            CountSearchDisplay(BinarySearchIterative, SensorB_LinkedList, Int32.Parse(SensorB_SearchTargetTextBox.Text), 0, NumberOfNodes(SensorB_LinkedList) - 1, SensorB_IterativeSearchTextBox, SensorB_ListBox);
        }

        // 4. Method for Sensor B and Binary Search Recursive.
        private void SensorB_RecursiveSearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EnableSearchButtons()
        {
            SensorA_IterativeSearchButton.IsEnabled = true;
            SensorB_IterativeSearchButton.IsEnabled = true;
            SensorA_RecursiveSearchButton.IsEnabled = true;
            SensorB_RecursiveSearchButton.IsEnabled = true;
        }
        private void DisableSearchButtons()
        {
            SensorA_IterativeSearchButton.IsEnabled = false;
            SensorB_IterativeSearchButton.IsEnabled = false;
            SensorA_RecursiveSearchButton.IsEnabled = false;
            SensorB_RecursiveSearchButton.IsEnabled = false;
        }
        #endregion
        #region 4.12 Buttons: Sort
        /*
        Create four button click methods that will sort the LinkedList using the Selection and Insertion
        methods. The four methods are:
            1. Method for Sensor A and Selection Sort
            2. Method for Sensor A and Insertion Sort
            3. Method for Sensor B and Selection Sort
            4. Method for Sensor B and Insertion Sort
        The button method must start a stopwatch before calling the sort method. Once the sort is complete
        the stopwatch will stop, and the number of milliseconds will be displayed in a read only textbox.
        Finally, the code/method will call the “ShowAllSensorData” method and “DisplayListboxData” for the
        appropriate sensor.
        */

        // Method to reduce repetition.
        private void TimeSortDisplay(Func<LinkedList<double>, bool> sortTypeParameter, LinkedList<double> linkedListParameter, ListBox listBoxParameter, TextBox textBoxParamater)
        {
            // Start timer.
            Stopwatch sw = Stopwatch.StartNew();

            // Sort.
            sortTypeParameter(linkedListParameter);

            // Stop timer.
            sw.Stop();

            // Display sort time in milliseconds in text box.
            textBoxParamater.Clear();
            textBoxParamater.Text = sw.ElapsedMilliseconds.ToString() + " ms";

            // Display linked list in list box.
            DisplayListboxData(linkedListParameter, listBoxParameter);

            // Enable searching elements.
            EnableSearchTargetTextBox();
        }

        // 1. Button click method for Sensor A and Selection Sort.
        private void SensorA_SelectionSortButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSortDisplay(SelectionSort, SensorA_LinkedList, SensorA_ListBox, SensorA_SelectionSortTextBox);
        }

        // 2. Button click method for Sensor A and Insertion Sort.
        private void SensorA_InsertionSortButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSortDisplay(InsertionSort, SensorA_LinkedList, SensorA_ListBox, SensorA_InsertionSortTextBox);
        }

        // 3. Button click method for Sensor B and Selection Sort.
        private void SensorB_SelectionSortButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSortDisplay(SelectionSort, SensorB_LinkedList, SensorB_ListBox, SensorB_SelectionSortTextBox);
        }

        // 4. Button click method for Sensor B and Insertion Sort.
        private void SensorB_InsertionSortButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSortDisplay(InsertionSort, SensorB_LinkedList, SensorB_ListBox, SensorB_InsertionSortTextBox);
        }

        private void EnableSortButtons()
        {
            SensorA_SelectionSortButton.IsEnabled = true;
            SensorB_SelectionSortButton.IsEnabled = true;
            SensorA_InsertionSortButton.IsEnabled = true;
            SensorB_InsertionSortButton.IsEnabled = true;
        }
        private void DisableSortButtons()
        {
            SensorA_SelectionSortButton.IsEnabled = false;
            SensorB_SelectionSortButton.IsEnabled = false;
            SensorA_InsertionSortButton.IsEnabled = false;
            SensorB_InsertionSortButton.IsEnabled = false;
        }
        #endregion
        #region 4.13 Combo Boxes: Sigma and Mu
        /*
        Add two numeric input controls for Sigma and Mu. The value for Sigma must be limited with a
        minimum of 10 and a maximum of 20. Set the default value to 10. The value for Mu must be limited
        with a minimum of 35 and a maximum of 75. Set the default value to 50.
        */
        private void PopulateComboBoxes()
        {
            // The value for Sigma must be limited with a minimum of 10 and a maximum of 20.
            for (double sigma = 10.0; sigma <= 20.0; sigma++)
            {
                SigmaComboBox.Items.Add(sigma);
            }
            // Set the default value to 10.
            SigmaComboBox.SelectedItem = 10.0;

            // The value for Mu must be limited with a minimum of 35 and a maximum of 75.
            for (double mu = 35.0; mu <= 75.0; mu++)
            {
                MuComboBox.Items.Add(mu);
            }
            // Set the default value to 50.
            MuComboBox.SelectedItem = 50.0;
        }
        #endregion
        #region -->TO DO!<-- 4.14 Text Boxes: Interger Input for Search
        /*
        Add two textboxes for the search value; one for each sensor, ensure only numeric integer values can
        be entered.
        */
        private void EnableSearchTargetTextBox()
        {
            SensorA_SearchTargetTextBox.IsEnabled = true;
            SensorB_SearchTargetTextBox.IsEnabled = true;
        }
        private void DisableSearchTargetTextBox()
        {
            SensorA_SearchTargetTextBox.IsEnabled = false;
            SensorB_SearchTargetTextBox.IsEnabled = false;
        }
        #endregion
        #endregion
    }
}
