using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Motapart_Core
{
    internal class Text_To_Speech
    {
        public async static void SpeechLogic(string input)
        {

            // Hello umändern
            string url = "http://localhost:5002/api/tts?text=" + input;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] responseData = await response.Content.ReadAsByteArrayAsync();

                        using (MemoryStream memoryStream = new MemoryStream(responseData))
                        {
                            using (WaveStream waveStream = new WaveFileReader(memoryStream))
                            {
                                using (WaveOutEvent waveOut = new WaveOutEvent())
                                {
                                    waveOut.Init(waveStream);
                                    waveOut.Play();
                                    while (waveOut.PlaybackState == PlaybackState.Playing)
                                    {
                                        System.Threading.Thread.Sleep(100);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending the request: {ex.Message}");
                }
            }

        }

        public static void SpeechToMe(string parameter)
        {
            SpeechLogic(parameter);
        }
    }
}
