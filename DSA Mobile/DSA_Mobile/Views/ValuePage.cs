﻿using System;
using System.Diagnostics;
using DSLink.Request;
using Xamarin.Forms;

namespace DSAMobile.Views
{
    public class ValuePage : ContentPage
    {
        private string _path;
        private Label _value;

        public ValuePage(string path)
        {
            _path = path;

            _value = new Label
            {
                Text = "Loading value..."
            };

            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    _value
                }
            };

            Appearing += async delegate
            {
                await App.Instance.DSLink.Requester.Subscribe(_path, ValueUpdate);
            };

            Disappearing += delegate
            {
                App.Instance.DSLink.Requester.Unsubscribe(_path);
            };
        }

        public void ValueUpdate(SubscriptionUpdate update)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _value.Text = update.Value.ToString();
            });
        }
    }
}
