﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace WPFUserInterface
{
    /// <summary>
    /// Interaction logic for MainWindowAdvanced.xaml
    /// </summary>
    public partial class MainWindowAdvanced : Window
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        public MainWindowAdvanced() { InitializeComponent(); }

        private void executeSync_Click(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            var results = DemoMethods.RunDownloadSync();
            //var results = DemoMethods.RunDownloadParallelSync();
            PrintResults(results);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: {elapsedMs}";
        }

        private async void executeAsync_Click(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            var results = await DemoMethods.RunDownloadAsync();
            PrintResults(results);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: {elapsedMs}";


            //Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            //progress.ProgressChanged += ReportProgress;

            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //try
            //{
            //    var results = await DemoMethods.RunDownloadAsync(progress, cts.Token);
            //    PrintResults(results);
            //} catch(OperationCanceledException)
            //{
            //    resultsWindow.Text += $"The async download was cancelled. {Environment.NewLine}";
            //}

            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;

            //resultsWindow.Text += $"Total execution time: {elapsedMs}";
        }

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            dashboardProgress.Value = e.PercentageComplete;
            PrintResults(e.SitesDownloaded);
        }

        private async void executeParallelAsync_Click(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            var results = await DemoMethods.RunDownloadParallelAsync();
            PrintResults(results);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: {elapsedMs}";

            //Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            //progress.ProgressChanged += ReportProgress;

            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //var results = await DemoMethods.RunDownloadParallelAsyncV2(progress);
            //PrintResults(results);

            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;

            //resultsWindow.Text += $"Total execution time: {elapsedMs}";
        }

        private void cancelOperation_Click(object sender, RoutedEventArgs e) { cts.Cancel(); }

        private void PrintResults(List<WebsiteDataModel> results)
        {
            resultsWindow.Text = string.Empty;
            foreach(var item in results)
            {
                resultsWindow.Text += $"{item.WebsiteUrl} downloaded: {item.WebsiteData.Length} characters long.{Environment.NewLine}";
            }
        }
    }
}
