﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UIElements
{
    /// <summary>
    /// Interaction logic for ImageAdjustmentMenu.xaml
    /// </summary>
    ///
    public partial class ImageAdjustmentMenu : UserControl
    {
        public Button ResetButton { get; set; }
        public Button ApplyButton { get; set; }
        public ImageSource TitleLogo { get; set; }
        public StackPanel Host { get; set; }
        public string TitleText { get; set; }
        public DoubleAnimation OutroAnimation { get; set; }

        public ImageAdjustmentMenu(string titleText, ImageSource title, ItemCollection items = null)
        {
            InitializeComponent();
            TitleText = titleText;
            TitleLogo = title;
            titleLabel.Content = TitleText;
            image.Source = TitleLogo;
            Host = host;
            AddApplyButton();
            AddResetButton();
            OutroAnimation = new DoubleAnimation();
            DoIntroAnimation();
            Margin = new Thickness(9, 10, 9, 10);
        }

        private void DoIntroAnimation()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = -300;
            da.To = 0;
            AddApplyButton();
            da.DecelerationRatio = 0.9;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            TranslateTransform tr = new TranslateTransform();
            this.RenderTransform = tr;
            tr.BeginAnimation(TranslateTransform.XProperty, da);
        }

        public void DoOutroAnimation()
        {
            OutroAnimation.From = 0;
            OutroAnimation.To = -300;

            OutroAnimation.DecelerationRatio = 0.9;
            OutroAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            TranslateTransform tr = new TranslateTransform();
            this.RenderTransform = tr;
            tr.BeginAnimation(TranslateTransform.XProperty, OutroAnimation);
        }

        private void AddApplyButton()
        {
            ApplyButton = new Button(45, 20, 5, 5, "Apply", 13, "#FF0288D1",
                "#4fc3f7", "#0277bd", "#FFFFFFFF", "#FF000000", "#FFFFFFFF")
            {
                HorizontalAlignment = HorizontalAlignment.Left
            };
            applyButtonGrid.Children.Add(ApplyButton);
        }
        private void AddResetButton()
        {
            ResetButton = new Button(45, 20, 5, 5, "Reset", 13, "#FF0288D1",
                "#4fc3f7", "#0277bd", "#FFFFFFFF", "#FF000000", "#FFFFFFFF")
            {
                HorizontalAlignment = HorizontalAlignment.Left
            };
            resetButtonGrid.Children.Add(ResetButton);
        }
    }
}

