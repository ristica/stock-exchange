using StockExchange.Contracts;
using System.Collections.Generic;
using System.ServiceModel;
using System;
using System.Linq;

namespace StockExchange.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class PubSubManager : IPubSubService
    {
        #region Properties

        /// <summary>
        /// this will be the list which holds all subscripted watchers on stock change
        /// important: musst be static because if not, each service call would
        /// instantiate new one and it will be emtpy
        /// </summary>
        private static Dictionary<string, List<IStockCallback>> StockWatchers { get; }

        #endregion

        #region C-Tor

        /// <summary>
        /// IMPORTANT:
        ///  the c-tor musst be static, then if not every client
        ///  that triggers the service will recreate it over and over again
        /// </summary>
        static PubSubManager()
        {
            StockWatchers = new Dictionary<string, List<IStockCallback>>();
        }

        #endregion

        #region IPubSubService implementation

        /// <summary>
        /// add new watcher into the list
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        public int Subscribe(string share)
        {
            lock (StockWatchers)
            {
                // at first get the callback chanel
                var callbackChannel = OperationContext.Current.GetCallbackChannel<IStockCallback>();
                if (callbackChannel == null) return 0;

                // create temp list with all subscribed stock watchers of the particular key (share)
                List<IStockCallback> subscribers;

                // if StockWatchers dictionary contains key with the corresponding "share"...
                if (StockWatchers.ContainsKey(share.ToUpper()))
                {
                    // ... then save the list in temp 
                    subscribers = StockWatchers[share.ToUpper()];
                }
                else
                {
                    // ... then create new empty list and add the "share" and the list to the StockWatchers
                    subscribers = new List<IStockCallback>();
                    StockWatchers.Add(share.ToUpper(), subscribers);
                }

                if (!subscribers.Contains(callbackChannel))
                {
                    // add this callback channel to the temp list
                    subscribers.Add(callbackChannel);
                }

                // update subscriber's count on this share
                NotifySubscribersAboutSubscriptions(share);

                return subscribers.Count;
            }
        }

        /// <summary>
        /// remove the watcher from the list if not allready down
        /// </summary>
        /// <param name="share"></param>
        public void Unsubscribe(string share)
        {
            lock (StockWatchers)
            {
                // at first get the callback chanel
                var callbackChannel = OperationContext.Current.GetCallbackChannel<IStockCallback>();
                if (callbackChannel == null) return;

                // check if StockWatchers contain the corresponding "share"
                if (!StockWatchers.ContainsKey(share.ToUpper())) return;

                // get the list of the subscribers for the corresponding "share"
                var shareSubscribers = StockWatchers[share.ToUpper()];
                if (!shareSubscribers.Contains(callbackChannel)) return;

                // remove the callback channel from the list of the subscribers for the corresponding "share"
                shareSubscribers.Remove(callbackChannel);

                // update subscriber's count on this share
                NotifySubscribersAboutSubscriptions(share);
            }
        }

        #endregion

        #region Helpers

        private static void RemoveDownClients(IEnumerable<IStockCallback> downSubscribers)
        {
            if (!downSubscribers.Any()) return;

            // get all share keys in the StockWatcher list
            foreach (var key in StockWatchers)
            {
                // get subscriber list for the corresponding "share" key
                var subscribers = key.Value;
                foreach (var s in subscribers)
                {
                    if (!subscribers.Contains(s)) continue;

                    subscribers.Remove(s);
                }
            }
        }

        #endregion

        #region Callback helpers

        private void NotifySubscribersAboutSubscriptions(string share)
        {
            // need to know if one of the suscribers went down in meantime
            var downSubscribers = new List<IStockCallback>();

            if (StockWatchers.ContainsKey(share.ToUpper()))
            {
                var subscribers = StockWatchers[share.ToUpper()];
                foreach (var s in subscribers)
                {
                    try
                    {
                        // update subscriber's count of the share subscribers
                        s.RefreshSubscriptions(share, subscribers.Count);
                    }
                    catch (Exception)
                    {
                        downSubscribers.Add(s);
                    }
                }
            }

            RemoveDownClients(downSubscribers);
        }

        public static void NotifySubscribersAboutStockPriceChange(string share, decimal price)
        {
            // need to know if one of the suscribers went down in meantime
            var downSubscribers = new List<IStockCallback>();

            lock (StockWatchers)
            {
                if (StockWatchers.ContainsKey(share.ToUpper()))
                {
                    var shareSubscribers = StockWatchers[share.ToUpper()];
                    foreach (var s in shareSubscribers)
                    {
                        try
                        {
                            s.RefreshStockState(share, price);
                        }
                        catch (Exception)
                        {
                            downSubscribers.Add(s);
                        }
                    }
                }
            }

            RemoveDownClients(downSubscribers);
        }

        #endregion
    }
}
