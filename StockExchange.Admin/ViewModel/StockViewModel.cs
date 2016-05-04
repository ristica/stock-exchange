using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockExchange.Admin.ViewModel
{
    public class StockViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string _share;
        private string _company;
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
        public string Company
        {
            get { return this._company; }
            set
            {
                if (this._company == value) return;
                this._company = value;
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
