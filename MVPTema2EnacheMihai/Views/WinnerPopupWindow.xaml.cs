using MVPTema2EnacheMihai.ViewModels;
using System;
using System.Windows;

namespace MVPTema2EnacheMihai.Views
{
    public partial class WinnerPopupWindow : Window
    {
        public WinnerPopupWindow(string winnerMessage)
        {
            InitializeComponent();
            DataContext = new WinnerPopupViewModel(winnerMessage);
            // Subscribe to the RestartRequested event
            ((WinnerPopupViewModel)DataContext).RestartRequested += WinnerPopupViewModel_RestartRequested;
        }

        private void WinnerPopupViewModel_RestartRequested(object sender, EventArgs e)
        {
            // Close the dialog window
            Close();
        }
    }
}
