// See https://aka.ms/new-console-template for more information
/*long unixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
Console.WriteLine(unixSeconds);*/

using System.Globalization;

string dateString = DateTime.Now.ToShortDateString();
Console.WriteLine(dateString);
DateTime date = DateTime.Parse(dateString);
DateTime epochTime = DateTime.Parse("1970-01-01");

//07%2F28%2F2023

var milliseconds = date.Subtract(epochTime).TotalSeconds;
Console.WriteLine(milliseconds); // 1620907140
Console.WriteLine(DateTime.Parse("30.07.2023").ToString());
Console.WriteLine(DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
Console.WriteLine(DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).Replace("/", "%2F"));
Thread.Sleep(100000);