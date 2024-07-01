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
using MessageBox = System.Windows.MessageBox;

namespace SimpleTextEditor
{
    /// <summary>
    /// Interaction logic for FindReplaceWindow.xaml
    /// </summary>
    public partial class FindReplaceWindow : Window
    {
        private MainWindow mainWindow;

        public FindReplaceWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void FindNext_Click(object sender, RoutedEventArgs e)
        {
            string textToFind = TextBoxFind.Text;
            if (!string.IsNullOrEmpty(textToFind))
            {
                int index = mainWindow.TextBoxEditor.Text.IndexOf(textToFind, mainWindow.TextBoxEditor.SelectionStart + mainWindow.TextBoxEditor.SelectionLength);
                if (index != -1)
                {
                    mainWindow.TextBoxEditor.Select(index, textToFind.Length);
                    mainWindow.TextBoxEditor.Focus();
                }
                else
                {
                    MessageBox.Show("Text not found.");
                }
            }
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow.TextBoxEditor.SelectedText == TextBoxFind.Text)
            {
                mainWindow.TextBoxEditor.SelectedText = TextBoxReplace.Text;
            }
            FindNext_Click(sender, e);
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            string textToFind = TextBoxFind.Text;
            string textToReplace = TextBoxReplace.Text;

            if (!string.IsNullOrEmpty(textToFind))
            {
                mainWindow.TextBoxEditor.Text = mainWindow.TextBoxEditor.Text.Replace(textToFind, textToReplace);
            }
        }
    }
}
