
using Newtonsoft.Json;
using HtmlAgilityPack;

public static class Scraper
{
    public static async Task ScrapeVehicles()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync("https://www.eezycars.co.ke/auction/index");

        var document = new HtmlDocument();
        document.LoadHtml(html);

        var vehicles = new List<Vehicle>();
        var rows = document.DocumentNode.SelectNodes("//tbody/tr");

        foreach (var row in rows)
        {
            var vehicle = new Vehicle
            {
                ImageUrl = row.SelectSingleNode(".//td[@class='vehicle_img']/img").GetAttributeValue("src", ""),
                Seller = row.SelectSingleNode(".//td[@class='hidden-phone vehicle_seller']").InnerText.Trim(),
                Year = int.Parse(row.SelectSingleNode(".//td[@class='vehicle_year']").InnerText.Trim()),
                Model = row.SelectSingleNode(".//td[@class='vehicle_model']/a").InnerText.Trim(),
                // Remove the line for RegistrationNumber if it doesn't exist in the HTML.
                Mileage = int.Parse(row.SelectSingleNode(".//td[@class='vehicle_mileage group_a']").InnerText.Trim()),
                Color = row.SelectSingleNode(".//td[@class='hidden-phone vehicle_color group_a']").InnerText.Trim(),
                Rating = row.SelectNodes(".//td[@class='vehicle_rating']/li[i[contains(@class, 'fa-star')]]").Count(),
                Location = row.SelectSingleNode(".//td[@class='vehicle_location hidden-phone']").InnerText.Trim(),
                BidPrice = row.SelectSingleNode(".//td[@class='bid_price']").InnerText.Trim(),
                // Remove the line for Bids if it doesn't exist in the HTML.
                TimeLeft = row.SelectSingleNode(".//td[@class='time_left']").InnerText.Trim()
            };

            vehicles.Add(vehicle);
        }

        SaveAsJson(vehicles, "/eezycars.json");
    }

    private static void SaveAsJson(List<Vehicle> vehicles, string path)
    {
        var json = JsonConvert.SerializeObject(vehicles, Formatting.Indented);
        File.WriteAllText(path, json);
    }
}
public class Vehicle
{
    public string ImageUrl { get; set; }
    public string Seller { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }
    //public string RegistrationNumber { get; set; }
    public int Mileage { get; set; }
    public string Color { get; set; }
    public int Rating { get; set; }
    public string Location { get; set; }
    public string BidPrice { get; set; }
    //public int Bids { get; set; }
    public string TimeLeft { get; set; }
}