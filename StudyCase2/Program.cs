using StudyCase2;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project
{
    class Program
    {
        // Please, change the responce.json file path before run
        public static void Main()
        {
            string plainText;
            List<string> parsedDescription = new List<string>();
            List<string> temp = new List<string>();
            List<RootObject> deserializeJson;

            // read file into a string and deserialize JSON to a type
            using (StreamReader r = new StreamReader("C:\\Users\\hasibe.capraz\\source\\repos\\StudyCase2\\StudyCase2\\response.json"))
            {
                string json = r.ReadToEnd();
                deserializeJson = JsonConvert.DeserializeObject<List<RootObject>>(json);

            }

            // Here is my algorithm for requested text:
            for (int i = 1; i < deserializeJson.Count(); i++)
            {
                plainText = deserializeJson[i].description;
                //Checking last y values
                var vertices = deserializeJson[i].boundingPoly.vertices;

                if (i == deserializeJson.Count() - 1)
                {
                    string concatValues = String.Join(" ", temp);
                    concatValues += plainText;
                    parsedDescription.Add(concatValues);
                }
                else
                {
                    var vertices2 = deserializeJson[i + 1].boundingPoly.vertices;

                    // Then I'm looking at the difference between consecutive y values
                    //I parse the text if the difference is too much
                    if (Math.Abs(vertices[3].y - vertices2[3].y) < 5 || Math.Abs(vertices[0].y - vertices2[0].y) < 5)
                    {
                        temp.Add(plainText);
                    }
                    else
                    {
                        if (temp.Count() > 0)
                        {
                            string concatValues = String.Join(" ", temp);
                            concatValues += plainText;
                            parsedDescription.Add(concatValues);
                            temp.Clear();
                        }
                        else
                        {
                            parsedDescription.Add(plainText);
                        }
                    }

                    //var consecutiveY = deserializeJson[i + 1].boundingPoly.vertices.Select(s => s.y);

                    //Then I'm looking at the difference between x values ​​if consecutive y values ​​exist
                    //I parse the text if the difference is too much
                }
            }

            foreach (string item in parsedDescription)
            {
                Console.WriteLine(item);

            }
        }
    }
}
