using MVPTema2EnacheMihai.Models;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MVPTema2EnacheMihai.ViewModels;

namespace MVPTema2EnacheMihai.Views
{
    public partial class Board : Window
    {
        public Board()
        {
            InitializeComponent();
            DataContext = new CheckerBoardViewModel();
        }
    }



}