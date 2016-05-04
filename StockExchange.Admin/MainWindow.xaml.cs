using StockExchange.Admin.ViewModel;
using StockExchange.Proxies;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StockExchange.Admin
{
    public partial class MainWindow : Window
    {
        #region Fields

        private ObservableCollection<StockViewModel> Stocks = new ObservableCollection<StockViewModel>();
        private readonly StockClient _proxy;

        #endregion

        #region C-Tor

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
            this._proxy = new StockClient();
            this.LbStocks.ItemsSource = this.Stocks;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var stocks = this._proxy.Get();
            foreach (var stock in stocks)
            {
                this.Stocks.Add(
                    new StockViewModel
                    {
                        Company = stock.Company,
                        Share = stock.Share,
                        Price = stock.CurrentPrice
                    });
            }
        }

        #endregion

        #region Events

        private void ButtonDownClicked(object sender, RoutedEventArgs e)
        {
            var stock = this.GetStockValues(sender);
            if (stock == null) return;

            stock.Price = this._proxy.UpdateStockPrice(stock.Share.ToUpper(), stock.Price, false);
        }

        private void ButtonUpClicked(object sender, RoutedEventArgs e)
        {
            var stock = this.GetStockValues(sender);
            if (stock == null) return;

            stock.Price = this._proxy.UpdateStockPrice(stock.Share.ToUpper(), stock.Price, true);
        }

        #endregion

        #region Overrides

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this._proxy.Close();
        }

        #endregion

        #region Helpers 

        private StockViewModel GetStockValues(object sender)
        {
            var btn = sender as Button;
            if (btn == null) return null;

            var share = btn.Tag.ToString().ToUpper();
            var stock = this.Stocks.SingleOrDefault(s => s.Share.ToUpper() == share);

            return stock;
        }

        #endregion
    }
}
