﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Api.Aggregator;
using Telegram.Api.Services;
using Telegram.Api.Services.Cache;
using Telegram.Api.TL;
using Windows.UI.Xaml.Navigation;

namespace Unigram.ViewModels.Payments
{
    public class PaymentReceiptViewModel : UnigramViewModelBase
    {
        public PaymentReceiptViewModel(IMTProtoService protoService, ICacheService cacheService, ITelegramEventAggregator aggregator) 
            : base(protoService, cacheService, aggregator)
        {
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var buffer = parameter as byte[];
            if (buffer != null)
            {
                using (var from = new TLBinaryReader(buffer))
                {
                    var tuple = new TLTuple<TLMessage, TLPaymentsPaymentReceipt>(from);

                    Invoice = tuple.Item1.Media as TLMessageMediaInvoice;
                    Receipt = tuple.Item2;
                }
            }

            return Task.CompletedTask;
        }

        private TLMessageMediaInvoice _invoice;
        public TLMessageMediaInvoice Invoice
        {
            get
            {
                return _invoice;
            }
            set
            {
                Set(ref _invoice, value);
            }
        }

        private TLPaymentsPaymentReceipt _receipt;
        public TLPaymentsPaymentReceipt Receipt
        {
            get
            {
                return _receipt;
            }
            set
            {
                Set(ref _receipt, value);
            }
        }
    }
}