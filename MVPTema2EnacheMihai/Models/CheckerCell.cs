using System;
using System.ComponentModel;

namespace MVPTema2EnacheMihai.Models
{
    public class CheckerCell : INotifyPropertyChanged
    {
        private bool _isDark;
        public bool IsDark
        {
            get { return _isDark; }
            set { _isDark = value; OnPropertyChanged("IsDark"); }
        }
        private Pieces _pieces;
        public Pieces Pieces
        {
            get { return _pieces; }
            set
            {
                _pieces = value;
                if (_pieces != null && ((_pieces.IsPlayerOne && Row == 7) || (!_pieces.IsPlayerOne && Row == 0)))
                {
                    _pieces.IsKing = true;
                }
                OnPropertyChanged("Pieces");
            }
        }


        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set { _isHighlighted = value; OnPropertyChanged("IsHighlighted"); }
        }

            public int Row { get; set; }
            public int Column { get; set; }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set
            {
                _isAvailable = value;
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
