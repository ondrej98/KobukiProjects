using KobukiCore.KobukiAssets.Lidar.Call;
using KobukiCore.Services;
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World with Kobuki :)");
        Console.WriteLine();

        var kobukiService = new KobukiService(enableSkeleton: false);
        Console.WriteLine($"Trying to initialize lidar (Expects {LidarCall.InitNewMeasurementCount} measurements where each contains around 480 data).");
        kobukiService.StartWorkers();
        var lidarData = kobukiService.LidarDataMeasurements;
        if (lidarData != null)
        {
            Console.WriteLine($"Received {lidarData.CountAllLaserMeasurement()} measurements | Expected {LidarCall.InitNewMeasurementCount} measurements");
            Console.WriteLine();
        }

        Thread.Sleep(100);
        Console.WriteLine("Trying to set up translation speed to: 0 mm/s");
        if (kobukiService.SetTranslationSpeed(0))
        {
            Console.WriteLine("Translation speed was set to: 0 mm/s");

            var data = kobukiService.LastKobukiData;

            Console.WriteLine($"Time stamp: {data?.Timestamp}");
            Console.WriteLine($"Encoders(left|right): {data?.EncoderLeft}|{data?.EncoderRight}");
        }
        Console.WriteLine();
        Console.WriteLine("Sleep for 100 ms");
        Thread.Sleep(100);
        Console.WriteLine();

        Console.WriteLine("Trying to set up translation speed to: 100 mm/s");
        if (kobukiService.SetTranslationSpeed(100))
        {
            Console.WriteLine("Translation speed was set to: 100 mm/s");

            var data = kobukiService.LastKobukiData;

            Console.WriteLine($"Time stamp: {data.Timestamp}");
            Console.WriteLine($"Encoders(left|right): {data.EncoderLeft}|{data.EncoderRight}");
        }
        Console.WriteLine();
        Console.WriteLine("Sleep for 5000 ms");
        Thread.Sleep(5000);
        Console.WriteLine();

        Console.WriteLine("Trying to set up translation speed to: -100 mm/s");
        if (kobukiService.SetTranslationSpeed(-100))
        {
            Console.WriteLine("Translation speed was set to: -100 mm/s");

            var data = kobukiService.LastKobukiData;

            Console.WriteLine($"Time stamp: {data.Timestamp}");
            Console.WriteLine($"Encoders(left|right): {data.EncoderLeft}|{data.EncoderRight}");
        }
        Console.WriteLine();
        Console.WriteLine("Sleep for 5000 ms");
        Thread.Sleep(5000);
        Console.WriteLine();

        Console.WriteLine("Trying to set up translation speed to: 0 mm/s");
        if (kobukiService.SetTranslationSpeed(0))
        {
            Console.WriteLine("Translation speed was set to: 0 mm/s");

            var data = kobukiService.LastKobukiData;

            Console.WriteLine($"Time stamp: {data.Timestamp}");
            Console.WriteLine($"Encoders(left|right): {data.EncoderLeft}|{data.EncoderRight}");
        }
        Console.WriteLine();
        Console.WriteLine("Sleep for 100 ms");
        Thread.Sleep(100);
        Console.WriteLine();

        Console.WriteLine("Trying to set up rotation speed to: 1 rad/s");
        if (kobukiService.SetRotationSpeed(1))
        {
            Console.WriteLine("Translation speed was set to: 1 rad/s");

            var data = kobukiService.LastKobukiData;

            Console.WriteLine($"Time stamp: {data.Timestamp}");
            Console.WriteLine($"Encoders(left|right): {data.EncoderLeft}|{data.EncoderRight}");
        }
        Console.WriteLine();
        Console.WriteLine("Sleep for 5000 ms");
        Thread.Sleep(5000);
        Console.WriteLine();

        Console.WriteLine("Trying to set up rotation speed to: -1 rad/s");
        if (kobukiService.SetRotationSpeed(-1))
        {
            Console.WriteLine("Translation speed was set to: -1 rad/s");

            var data = kobukiService.LastKobukiData;

            Console.WriteLine($"Time stamp: {data.Timestamp}");
            Console.WriteLine($"Encoders(left|right): {data.EncoderLeft}|{data.EncoderRight}");
        }
        Console.WriteLine();
        Console.WriteLine("Sleep for 5000 ms");
        Thread.Sleep(5000);
        Console.WriteLine();

        Console.WriteLine("Trying to set up rotation speed to: 0 rad/s");
        if (kobukiService.SetRotationSpeed(0))
        {
            Console.WriteLine("Translation speed was set to: 0 rad/s");

            var data = kobukiService.LastKobukiData;

            Console.WriteLine($"Time stamp: {data.Timestamp}");
            Console.WriteLine($"Encoders(left|right): {data.EncoderLeft}|{data.EncoderRight}");
        }

        lidarData = kobukiService.LidarDataMeasurements;
        if (lidarData != null)
        {
            Console.WriteLine($"Received {lidarData.CountAllLaserMeasurement()} measurements | Expected {LidarCall.InitNewMeasurementCount} measurements");
            Console.WriteLine();
        }

        kobukiService.CloseWorkers();
    }
}