using System.Text;

namespace Bot.Api.Services
{
    public class CallCCS : ICallCCS
    {
        public async Task WaitAndPostToApi()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));

            string apiUrl = "http://host.docker.internal:5002/api/Bot/bot";
            var containerName = Environment.GetEnvironmentVariable("HOSTNAME");
            Console.WriteLine($"[INFO] Running inside container: {containerName}");
            string jsonData = $"\"{containerName}\"";
            int maxAttempts = 5;
            var random = new Random();

            using HttpClient client = new HttpClient();

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                Console.WriteLine($"Attempt {attempt} of {maxAttempts}");
                try
                {
                    using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("API POST successful!");
                        return;
                    }
                    else
                    {
                        Console.WriteLine($"API POST failed with status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                if (attempt < maxAttempts)
                {
                    int delaySeconds = random.Next(3, 6); 
                    Console.WriteLine($"Waiting {delaySeconds} seconds before retrying...");
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            Console.WriteLine("All attempts failed.");
        }
    }
}
