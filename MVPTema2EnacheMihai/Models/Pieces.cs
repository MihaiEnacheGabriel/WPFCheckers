using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MVPTema2EnacheMihai.Models
{
    public class Pieces
    {
        public bool IsPlayerOne { get; set; }
        public bool IsKing { get; set; }
        public Brush Color
        {
            get
            {
                if (IsKing)
                {
                    return IsPlayerOne ? Brushes.Yellow : Brushes.Blue;
                }
                else
                {
                    return IsPlayerOne ? Brushes.Red : Brushes.Black;
                }
            }
        }
    }


}
