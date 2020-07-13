using Microsoft.AspNetCore.Http;
using System;
using System.Buffers.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QR
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main()
        {
            Program p = new Program();

            string responsePayload = p.Upload().GetAwaiter().GetResult();

            //Display API POST response in console -- JSON that contains required data
            Console.WriteLine(responsePayload);
        }

        private async Task<string> Upload()
        {

            var request = new HttpRequestMessage(HttpMethod.Post, "http://api.qrserver.com/v1/read-qr-code/");


            //var multiPartContent = new MultipartFormDataContent();

            using (System.Drawing.Image image = System.Drawing.Image.FromFile("F:\\Work\\QR_Reader\\Resources\\qrcode.jpeg"))
            {


                using (MemoryStream m = new MemoryStream())
                {


                    image.Save(m, image.RawFormat);
                    byte[] byteArray = m.ToArray();


                    //Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(byteArray);

                    //use string source
                    //multiPartContent.Add(new StringContent(base64String));

                    //use byte array source
                    //multiPartContent.Add(new ByteArrayContent(byteArray));


                    //request.Content = multiPartContent;

                    request.Content = new StringContent(base64String, Encoding.UTF8, "multipart/form-data");
                    request.Content.Headers.ContentLength = base64String.Length;
                    

                    //the API call response is recorded and then returned to calling location
                    var response = await client.SendAsync(request);

                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }


        //Method used for local QR file
        //public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        imageIn.Save(ms, ImageFormat.Jpeg);
        //        return ms.ToArray();
        //    }
        //}
    }
}
