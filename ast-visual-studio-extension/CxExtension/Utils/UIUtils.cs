﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ast_visual_studio_extension.CxExtension.Utils
{
    internal class UIUtils
    {
        /// <summary>
        ///  Create header for tree view item
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static TextBlock CreateTreeViewItemHeader(string severity, string displayName)
        {
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            // Add highlight color on hovering each item
            stackPanel.MouseEnter += OnMouseOverResult;
            stackPanel.MouseLeave += OnMouseLeaveResult;

            if (!string.IsNullOrEmpty(severity))
            {
                Image severityIcon = new Image();
                BitmapImage severityBitmap = new BitmapImage(new Uri(CxUtils.GetIconPathFromSeverity(severity, false), UriKind.RelativeOrAbsolute));
                severityIcon.Source = severityBitmap;

                stackPanel.Children.Add(severityIcon);
            }

            Label resultDisplayName = new Label
            {
                Content = displayName
            };
            stackPanel.Children.Add(resultDisplayName);

            InlineUIContainer uiContainer = new InlineUIContainer(stackPanel);

            TextBlock resultUIElement = new TextBlock();
            resultUIElement.Inlines.Add(uiContainer);
            resultUIElement.Tag = displayName;

            return resultUIElement;
        }

        /// <summary>
        ///  Create a tree view item using items source
        /// </summary>
        /// <param name="headerText"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TreeViewItem CreateTreeViewItemWithItemsSource(string headerText, List<TreeViewItem> source)
        {
            return new TreeViewItem
            {
                Header = CreateTreeViewItemHeader(String.Empty, headerText),
                ItemsSource = source,
                Tag = headerText
            };
        }

        private static void OnMouseOverResult(object sender, RoutedEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush(Colors.DarkSlateGray);
        }

        private static void OnMouseLeaveResult(object sender, RoutedEventArgs e)
        {
            (sender as StackPanel).Background = new SolidColorBrush(Colors.Transparent);
        }

        public static TextBlock CreateTextBlock(string message)
        {
            return new TextBlock
            {
                Text = message
            };
        }

        public static StackPanel CreateSeverityLabelWithIcon(string severity)
        {
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image severityIcon = new Image();
            BitmapImage severityBitmap = new BitmapImage(new Uri(CxUtils.GetIconPathFromSeverity(severity, false), UriKind.RelativeOrAbsolute));
            severityIcon.Source = severityBitmap;

            stackPanel.Children.Add(severityIcon);

            Label severityLabel = new Label
            {
                Content = severity
            };

            stackPanel.Children.Add(severityLabel);

            return stackPanel;
        }

        public static StackPanel CreateLabelWithImage(string message, string icon)
        {
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image severityIcon = new Image();
            
            BitmapImage severityBitmap = new BitmapImage(new Uri(CxConstants.RESOURCES_BASE_DIR + icon, UriKind.RelativeOrAbsolute));
            severityIcon.Source = severityBitmap;

            stackPanel.Children.Add(severityIcon);

            Label severityLabel = new Label
            {
                Content = message
            };

            stackPanel.Children.Add(severityLabel);

            return stackPanel;
        }
    }
}
