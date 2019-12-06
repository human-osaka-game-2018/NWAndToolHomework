using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Core.Models.Services;

namespace WPF_Core.Views
{
    /// <summary>
    /// EditorView.xaml の相互作用ロジック
    /// </summary>
    public partial class EditorView : Window
    {
        public EditorView()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LogInService.LogOut();
        }
    }
}
