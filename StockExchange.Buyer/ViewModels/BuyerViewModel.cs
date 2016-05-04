using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockExchange.Buyer.ViewModels
{
    public class BuyerViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string _share;
        public int _subscribers;
        private decimal _price;

        #endregion

        #region Properties

        public string Share
        {
            get { return this._share; }
            set
            {
                if (this._share == value) return;
                this._share = value;
                OnPropertyChanged();
            }
        }

        public int Subscribers
        {
            get { return this._subscribers; }
            set
            {
                if (this._subscribers == value) return;
                this._subscribers = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get { return this._price; }
            set
            {
                if (this._price == value) return;
                this._price = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public int CallerMemberName { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
