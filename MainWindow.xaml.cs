using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;
using FontFamily = System.Windows.Media.FontFamily;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace SimpleTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentFilePath;
        private const int MaxRecentFiles = 5;
        private List<string> recentFiles = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            UpdateRecentFilesMenu();
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEditor.Clear();
            currentFilePath = null;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                currentFilePath = openFileDialog.FileName;
                TextBoxEditor.Text = File.ReadAllText(currentFilePath);
                AddToRecentFiles(currentFilePath);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (currentFilePath == null)
            {
                SaveAsFile_Click(sender, e);
            }
            else
            {
                File.WriteAllText(currentFilePath, TextBoxEditor.Text);
            }
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                currentFilePath = saveFileDialog.FileName;
                File.WriteAllText(currentFilePath, TextBoxEditor.Text);
                AddToRecentFiles(currentFilePath);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxEditor.CanUndo)
            {
                TextBoxEditor.Undo();
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxEditor.CanRedo)
            {
                TextBoxEditor.Redo();
            }
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEditor.Cut();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEditor.Copy();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            TextBoxEditor.Paste();
        }

        private void FindReplace_Click(object sender, RoutedEventArgs e)
        {
            FindReplaceWindow findReplaceWindow = new FindReplaceWindow(this);
            findReplaceWindow.Show();
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBoxEditor.FontFamily = new FontFamily(fontDialog.Font.Name);
                TextBoxEditor.FontSize = fontDialog.Font.Size * 96.0 / 72.0; // Convert points to WPF font size
                TextBoxEditor.FontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                TextBoxEditor.FontStyle = fontDialog.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void BackgroundColor_Click(object sender, RoutedEventArgs e)
        {
           ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color = colorDialog.Color;
                TextBoxEditor.Background = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
            }
        }

        private void AddToRecentFiles(string filePath)
        {
            if (recentFiles.Contains(filePath))
            {
                recentFiles.Remove(filePath);
            }
            recentFiles.Insert(0, filePath);
            if (recentFiles.Count > MaxRecentFiles)
            {
                recentFiles.RemoveAt(MaxRecentFiles);
            }
            UpdateRecentFilesMenu();
        }

        private void UpdateRecentFilesMenu()
        {
            RecentFilesMenu.Items.Clear();
            foreach (var filePath in recentFiles)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = filePath;
                menuItem.Click += (sender, e) => OpenRecentFile(filePath);
                RecentFilesMenu.Items.Add(menuItem);
            }
        }

        private void OpenRecentFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                currentFilePath = filePath;
                TextBoxEditor.Text = File.ReadAllText(filePath);
            }
        }
    }
}
