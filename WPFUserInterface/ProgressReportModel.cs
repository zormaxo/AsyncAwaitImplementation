using System.Collections.Generic;

namespace WPFUserInterface;

public class ProgressReportModel
{
    public int PercentageComplete { get; set; }

    public List<WebsiteDataModel> SitesDownloaded { get; set; } = new List<WebsiteDataModel>();
}
