﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubInfomationPage : ContentPage
    {
        public ClubInfomationPage(string clubCode, string addDate)
        {
            InitializeComponent();
        }
    }
}