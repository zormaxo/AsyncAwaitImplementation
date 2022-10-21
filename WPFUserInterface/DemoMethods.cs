using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WPFUserInterface;

public static class DemoMethods
{
    public static List<string> PrepData()
    {
        return new List<string>
        {
            "https://www.yahoo.com",
            "https://www.google.com",
            "https://www.linux.com",
            "https://www.cnn.com",
            "https://www.amazon.com",
            "https://www.facebook.com",
            "https://www.twitter.com",
            "https://www.codeproject.com",
            "https://www.apple.com",
            "https://en.wikipedia.org/wiki/.NET_Framework"
        };
    }

    public static List<WebsiteDataModel> RunDownloadSync()
    {
        var output = new List<WebsiteDataModel>();

        foreach(string site in PrepData())
        {
            WebsiteDataModel results = DownloadWebsite(site);
            output.Add(results);
        }

        return output;
    }

    public static List<WebsiteDataModel> RunDownloadParallelSync()
    {
        List<string> websites = PrepData();
        var output = new List<WebsiteDataModel>();

        Parallel.ForEach(
            websites,
            (site) =>
            {
                WebsiteDataModel results = DownloadWebsite(site);
                output.Add(results);
            });

        return output;
    }

    public static async Task<List<WebsiteDataModel>> RunDownloadAsync(
        IProgress<ProgressReportModel> progress,
        CancellationToken cancellationToken)
    {
        List<string> websites = PrepData();
        var output = new List<WebsiteDataModel>();
        var report = new ProgressReportModel();

        foreach(string site in websites)
        {
            WebsiteDataModel results = await DownloadWebsiteAsync(site);
            output.Add(results);

            cancellationToken.ThrowIfCancellationRequested();

            report.SitesDownloaded = output;
            report.PercentageComplete = (output.Count * 100) / websites.Count;

            progress.Report(report);
        }

        return output;
    }

    public static async Task<List<WebsiteDataModel>> RunDownloadParallelAsync()
    {
        var tasks = new List<Task<WebsiteDataModel>>();

        foreach(string site in PrepData())
        {
            tasks.Add(DownloadWebsiteAsync(site));
        }

        var results = await Task.WhenAll(tasks);

        return new List<WebsiteDataModel>(results);
    }

    public static async Task<List<WebsiteDataModel>> RunDownloadParallelAsyncV2(IProgress<ProgressReportModel> progress)
    {
        List<string> websites = PrepData();
        var output = new List<WebsiteDataModel>();
        var report = new ProgressReportModel();

        await Task.Run(
            () => Parallel.ForEach(
                websites,
                (site) =>
                {
                    WebsiteDataModel results = DownloadWebsite(site);
                    output.Add(results);

                    report.SitesDownloaded = output;
                    report.PercentageComplete = (output.Count * 100) / websites.Count;
                    progress.Report(report);
                }));

        return output;
    }

    private static async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
    {
        var output = new WebsiteDataModel();
        using var client = new WebClient();

        output.WebsiteUrl = websiteURL;
        output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

        return output;
    }

    private static WebsiteDataModel DownloadWebsite(string websiteURL)
    {
        var output = new WebsiteDataModel();
        using var client = new WebClient();

        output.WebsiteUrl = websiteURL;
        output.WebsiteData = client.DownloadString(websiteURL);

        return output;
    }
}
