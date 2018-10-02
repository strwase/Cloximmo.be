using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using MoreLinq;

namespace Cloximmo.be
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly List<HttpResponseMessage> errorlist = new List<HttpResponseMessage>();

        private static void Main(string[] args)
        {
            var xml = new XmlDocument();
            xml.Load(ConfigurationManager.ConnectionStrings["SpainKeys4YouXml"].ConnectionString);

            using (var sr = new StringReader(xml.InnerXml))
            {
                var serializer = new XmlSerializer(typeof(Root));
                var response = (Root)serializer.Deserialize(sr);
                var distinctResponse = response.Properties.DistinctBy(x => x.Id).ToList();
                DeleteFromWix(distinctResponse).Wait();
                SendToWix(distinctResponse).Wait();
            }

            Console.WriteLine("\r\n");
            Console.WriteLine("All done, press any key to exit.");
            Console.ReadKey();
        }

        private static async Task DeleteFromWix(List<Property> Properties)
        {
            var existing = await client.GetAsync($"{ConfigurationManager.ConnectionStrings["Cloximmo"].ConnectionString}spainKeys4You");
            var list = JsonConvert.DeserializeObject<CloximmoItems>(await existing.Content.ReadAsStringAsync());

            var toDelete = list.items.Select(x => x._id.ToString()).Except(Properties.Select(x => x.Id)).ToList();

            if (!toDelete.Any()) return;

            var counter = 0;
            Console.WriteLine("Removing deleted properties");
            foreach (var id in toDelete)
            {
                var deleteResult = await client.DeleteAsync($"{ConfigurationManager.ConnectionStrings["Cloximmo"].ConnectionString}spainKeys4You/{id}");
                counter++;
                DrawTextProgressBar(counter, toDelete.Count);
            }
        }

        private static async Task SendToWix(List<Property> Properties)
        {
            var responselist = new List<HttpResponseMessage>();
            var putResponselist = new List<HttpResponseMessage>();
            var imgResponselist = new List<HttpResponseMessage>();
            var imgPutResponselist = new List<HttpResponseMessage>();
            var counter = 0;

            Console.WriteLine("\r\n Adding/updating properties");

            foreach (var prop in Properties)
            {
                var json = JsonConvert.SerializeObject(prop);

                var result = await client.PostAsync($"{ConfigurationManager.ConnectionStrings["Cloximmo"].ConnectionString}spainKeys4You", new StringContent(json));
                var resultBody = await result.Content.ReadAsStringAsync();
                responselist.Add(result);

                if (resultBody.Contains("WD_ITEM_ALREADY_EXISTS"))
                {
                    var putResult = await client.PutAsync($"{ConfigurationManager.ConnectionStrings["Cloximmo"].ConnectionString}spainKeys4You", new StringContent(json));
                    putResponselist.Add(putResult);
                }

                foreach (var img in prop.Images.Image)
                {
                    img.PropertyId = prop.Id;
                    var imgJson = JsonConvert.SerializeObject(img);

                    var imgResult = await client.PostAsync($"{ConfigurationManager.ConnectionStrings["Cloximmo"].ConnectionString}spainKeys4YouImages", new StringContent(imgJson));
                    var imgResultBody = await result.Content.ReadAsStringAsync();
                    imgResponselist.Add(imgResult);

                    if (resultBody.Contains("WD_ITEM_ALREADY_EXISTS"))
                    {
                        var imgPutResult = await client.PutAsync($"{ConfigurationManager.ConnectionStrings["Cloximmo"].ConnectionString}spainKeys4YouImages", new StringContent(imgJson));
                        imgPutResponselist.Add(imgPutResult);
                    }
                }

                counter++;
                DrawTextProgressBar(counter, Properties.Count);
            }
        }

        private static async Task DeleteOldProperties(string id)
        {
            var result = await client.DeleteAsync($"{ConfigurationManager.ConnectionStrings["Cloximmo"].ConnectionString}spainKeys4YouImages/{id}");
        }

        private static void DrawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}