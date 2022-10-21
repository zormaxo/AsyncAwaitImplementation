using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace WPFUserInterface;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow() { InitializeComponent(); }

    private void executeSync_Click(object sender, RoutedEventArgs e)
    {
        var watch = Stopwatch.StartNew();

        RunDownloadSync();

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;

        resultsWindow.Text += $"Total execution time: {elapsedMs}";
    }

    private async void executeAsync_Click(object sender, RoutedEventArgs e)
    {
        var watch = Stopwatch.StartNew();

        await RunDownloadParallelAsync();
        //await RunDownloadAsync();

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;

        resultsWindow.Text += $"Total execution time: {elapsedMs}";
    }

    private List<string> PrepData()
    {
        var output = new List<string>();

        resultsWindow.Text = string.Empty;

        output.Add("https://www.yahoo.com");
        output.Add("https://www.google.com");
        output.Add("https://www.apple.com");
        output.Add("https://www.cnn.com");
        output.Add("https://www.codeproject.com");
        output.Add("https://en.wikipedia.org");

        return output;
    }

    private void RunDownloadSync()
    {
        foreach(string site in PrepData())
        {
            WebsiteDataModel results = DownloadWebsite(site);
            ReportWebsiteInfo(results);
        }
    }

    private async Task RunDownloadAsync()
    {
        foreach(string site in PrepData())
        {
            WebsiteDataModel results = await Task.Run(() => DownloadWebsite(site));
            ReportWebsiteInfo(results);
        }
    }

    private async Task RunDownloadParallelAsync()
    {
        var tasks = new List<Task<WebsiteDataModel>>();

        foreach(string site in PrepData())
        {
            //tasks.Add(Task.Run(() => DownloadWebsite(site)));
            tasks.Add(DownloadWebsiteAsync(site));
        }

        var results = await Task.WhenAll(tasks);

        foreach(var item in results)
        {
            ReportWebsiteInfo(item);
        }
    }

    private WebsiteDataModel DownloadWebsite(string websiteURL)
    {
        var output = new WebsiteDataModel();
        using var client = new WebClient();

        output.WebsiteUrl = websiteURL;
        output.WebsiteData = client.DownloadString(websiteURL);

        return output;
    }

    private async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
    {
        var output = new WebsiteDataModel();
        using var client = new WebClient();

        output.WebsiteUrl = websiteURL;
        output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

        return output;
    }

    private void ReportWebsiteInfo(WebsiteDataModel data)
    {
        resultsWindow.Text += $"{data.WebsiteUrl} downloaded: {data.WebsiteData.Length} characters long.{Environment.NewLine}";
    }
}
