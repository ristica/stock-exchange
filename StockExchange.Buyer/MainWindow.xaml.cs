using StockExchange.Buyer.ViewModels;
using StockExchange.Contracts;
using StockExchange.Proxies;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace StockExchange.Buyer
{
    // force it to work on separate thread
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class MainWindow : Window, IStockCallback
    {
        #region Fields

        private readonly PubSubClient _proxyPubSub;
        private readonly StockClient _proxyStock;
        private readonly SynchronizationContext _syncContext;

        #endregion

        #region Properties

        private readonly ObservableCollection<BuyerViewModel> Buyers = new ObservableCollection<BuyerViewModel>();

        #endregion

        #region C-Tor

        public MainWindow()
        {
            InitializeComponent();

            this._proxyPubSub = new PubSubClient(new InstanceContext(this));
            this._proxyStock = new StockClient();
            this._syncContext = SynchronizationContext.Current;
            this.LbShare.ItemsSource = this.Buyers;
        }

        #endregion

        #region Events

        private void BtnUnsubscribeClicked(object sender, RoutedEventArgs e)
        {
            var btn = sender as Hyperlink;
            if (btn == null) return;

            var share = btn.Tag.ToString();

            Task.Run(() =>
            {
                this._proxyPubSub.Unsubscribe(share);

                this._syncContext.Send(arg =>
                {
                    var vm = this.Buyers.SingleOrDefault(b => b.Share.ToUpper() == btn.Tag.ToString().ToUpper());
                    if (vm == null) return;

                    this.Buyers.Remove(vm);
                }, null);
            });
        }

        private void ButtonSubscribeClicked(object sender, RoutedEventArgs e)
        {
            // is anything in the share text box...
            var shareToSubscribe = this.TxtShareToWatch.Text;
            if (string.IsNullOrWhiteSpace(shareToSubscribe))
            {
                MessageBox.Show("There is no Share to subscribe to");
                return;
            }

            // is it allready in the subscriber's list...
            var vm = this.Buyers.SingleOrDefault(b => b.Share.ToUpper() == shareToSubscribe.ToUpper());
            if (vm != null)
            {
                MessageBox.Show("Allready subscribed to the Share");
                return;
            }

            // is it in the stock list anywhere...
            var stock = this._proxyStock.GetStock(shareToSubscribe);
            if (stock == null)
            {
                MessageBox.Show("There is no Stock with this Share");
                return;
            }

            vm = new BuyerViewModel
            {
                Share = shareToSubscribe.ToUpper(),
                Price = stock.CurrentPrice
            };

            this.Buyers.Add(vm);

            // this has to run on another thread because we have locked
            // the current subscribers list (StockWatchers) in the PubSubManager
            // or we run into dead lock!!!
            Task.Run(() =>
            {
                // call the proxy to retrieve the current count of the
                // stock watchers in the StockWatcher dictionary for the corresponding "share"
                var count = this._proxyPubSub.Subscribe(shareToSubscribe);

                // marshall this to the ui thread
                this._syncContext.Send(arg =>
                {
                    vm.Subscribers = count;
                    this.TxtShareToWatch.Text = string.Empty;
                }, null);
            });
        }

        #endregion

        #region Overrides

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // housekeeping
            foreach (var buyer in this.Buyers)
            {
                this._proxyPubSub.Unsubscribe(buyer.Share);
            }

            this._proxyPubSub.Close();
            this._proxyStock.Close();
        }

        #endregion

        #region IStockCallback implementation

        /// <summary>
        ///  update every subscriber of the corresponding "share" about subscriptions
        /// </summary>
        /// <param name="share"></param>
        /// <param name="subscribers"></param>
        public void RefreshSubscriptions(string share, int subscribers)
        {
            this._syncContext.Send(arg =>
            {
                var vm = this.Buyers.SingleOrDefault(b => b.Share.ToUpper() == share.ToUpper());
                if (vm == null) return;

                vm.Subscribers = subscribers;
            }, null);
        }

        /// <summary>
        /// update stock's current price if it changed by admin client
        /// </summary>
        /// <param name="share"></param>
        /// <param name="price"></param>
        public void RefreshStockState(string share, decimal price)
        {
            this._syncContext.Send(arg =>
            {
                var vm = this.Buyers.SingleOrDefault(b => b.Share.ToUpper() == share.ToUpper());
                if (vm == null) return;

                vm.Change = vm.Price > price;
                vm.Price = price;
            }, null);
        }

        #endregion
    }
}
