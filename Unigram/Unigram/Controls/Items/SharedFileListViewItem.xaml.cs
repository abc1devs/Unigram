﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Telegram.Api.Helpers;
using Telegram.Api.Services;
using Telegram.Api.TL;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Unigram.Controls.Items
{
    public sealed partial class SharedFileListViewItem : UserControl
    {
        public TLMessage ViewModel => DataContext as TLMessage;
        private TLMessage _oldViewModel;

        public SharedFileListViewItem()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (_oldViewModel != null)
            {
                //_oldViewModel.PropertyChanged -= OnPropertyChanged;
                _oldViewModel = null;
            }

            if (ViewModel != null)
            {
                _oldViewModel = ViewModel;
                //ViewModel.PropertyChanged += OnPropertyChanged;

                // To semplify future x:Bind implementation.
                EllipseIcon.Background = UpdateEllipseBrush(ViewModel);
                TimeLabel.Text = UpdateTimeLabel(ViewModel);
                SizeLabel.Text = UpdateSizeLabel(ViewModel);
            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private Brush UpdateEllipseBrush(TLMessage message)
        {
            var brushes = new[]
            {
                App.Current.Resources["Placeholder0Brush"],
                App.Current.Resources["Placeholder1Brush"],
                App.Current.Resources["Placeholder2Brush"],
                App.Current.Resources["Placeholder3Brush"]
            };

            var mediaDocument = message.Media as TLMessageMediaDocument;
            if (mediaDocument == null)
            {
                return brushes[0] as SolidColorBrush;
            }

            var document = mediaDocument.Document as TLDocument;
            if (document == null)
            {
                return brushes[0] as SolidColorBrush;
            }

            var name = document.FileName.ToString();
            if (name.Length > 0)
            {
                var color = -1;
                if (name.EndsWith(".doc") || name.EndsWith(".txt") || name.EndsWith(".psd"))
                {
                    color = 0;
                }
                else if (name.EndsWith(".xls") || name.EndsWith(".csv"))
                {
                    color = 1;
                }
                else if (name.EndsWith(".pdf") || name.EndsWith(".ppt") || name.EndsWith(".key"))
                {
                    color = 2;
                }
                else if (name.EndsWith(".zip") || name.EndsWith(".rar") || name.EndsWith(".ai") || name.EndsWith(".mp3") || name.EndsWith(".mov") || name.EndsWith(".avi"))
                {
                    color = 3;
                }
                else
                {
                    int idx;
                    var extension = (idx = name.LastIndexOf(".", StringComparison.Ordinal)) == -1 ? string.Empty : name.Substring(idx + 1);
                    if (extension.Length != 0)
                    {
                        color = extension[0] % brushes.Length;
                    }
                    else
                    {
                        color = name[0] % brushes.Length;
                    }
                }

                return brushes[color] as SolidColorBrush;
            }

            return brushes[0] as SolidColorBrush;
        }

        private string UpdateSizeLabel(TLMessage message)
        {
            var mediaDocument = message.Media as TLMessageMediaDocument;
            if (mediaDocument == null)
            {
                return "0 B";
            }

            var document = mediaDocument.Document as TLDocument;
            if (document == null)
            {
                return "0 B";
            }

            var bytesCount = document.Size;
            if (bytesCount < 1024)
            {
                return string.Format("{0} B", bytesCount);
            }

            if (bytesCount < 1024 * 1024)
            {
                return string.Format("{0} KB", ((double)bytesCount / 1024).ToString("0.0", CultureInfo.InvariantCulture));
            }

            if (bytesCount < 1024 * 1024 * 1024)
            {
                return string.Format("{0} MB", ((double)bytesCount / 1024 / 1024).ToString("0.0", CultureInfo.InvariantCulture));
            }

            return string.Format("{0} GB", ((double)bytesCount / 1024 / 1024 / 1024).ToString("0.0", CultureInfo.InvariantCulture));
        }

        private string UpdateTimeLabel(TLMessage message)
        {
            var clientDelta = MTProtoService.Current.ClientTicksDelta;
            var utc0SecsLong = message.Date * 4294967296 - clientDelta;
            var utc0SecsInt = utc0SecsLong / 4294967296.0;
            var dateTime = Utils.UnixTimestampToDateTime(utc0SecsInt);

            var cultureInfo = (CultureInfo)CultureInfo.CurrentUICulture.Clone();
            var shortTimePattern = Utils.GetShortTimePattern(ref cultureInfo);

            if (dateTime.Year == DateTime.Now.Year)
            {
                return string.Format($"{{0:dd MMM}} at {{0:{shortTimePattern}}}", dateTime);
            }

            return string.Format($"{{0:dd MMM yyyy}} at {{0:{shortTimePattern}}}", dateTime);
        }
    }
}
