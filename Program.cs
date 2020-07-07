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

            // we can use a QR code generated on-the-fly when POST will be sent
            //var getRequestString = "http://api.qrserver.com/v1/create-qr-code/?data=Hi!GoodToHearFromYou!&size=200x200";


            var request = new HttpRequestMessage(HttpMethod.Post, "http://api.qrserver.com/v1/read-qr-code/");

            //POST method that can pass the QR generating URL string
            //var request = new HttpRequestMessage(HttpMethod.Post, "http://api.qrserver.com/v1/read-qr-code/?fileurl=" + getRequestString);


            var content = new MultipartFormDataContent();

            //Use local file 
            System.Drawing.Image img = System.Drawing.Image.FromFile("F:\\Work\\QR_Reader\\qrcode.jpeg", true);
            byte[] byteArray = ImageToByteArray(img);

            content.Add(new ByteArrayContent(byteArray));
            content.Headers.ContentDisposition.FileName = "qrcode.jpeg";

            request.Content = content;
            
            //the API call response is recorded and then returned to calling location
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }

      
        //Method used for local QR file
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
