using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace QR
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();
            var file = "F:\\Work\\QR_Reader\\qrcode.jpeg";



            var filestream = new FileStream(file, FileMode.Open);
            var fileName = System.IO.Path.GetFileName(file);
            content.Add(new StreamContent(filestream), "file", fileName);


            message.Method = HttpMethod.Post;
            message.Content = content;
            message.RequestUri = new Uri("http://goqr.me/api/doc/read-qr-code/");

            var client = new HttpClient();

            client.SendAsync(message).ContinueWith(task =>
            {
                if (task.Result.IsSuccessStatusCode)
                {
                    var result = task.Result;
                    Console.WriteLine(result);
                }
            });

            Console.ReadLine();
        }

    }
}
