using OpenCvSharp;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        // Look for all images starting with "Input" and ending in .jpg or .png
        var images = Directory.GetFiles(Directory.GetCurrentDirectory())
                              .Where(f => (f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || 
                                           f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) &&
                                           Path.GetFileName(f).ToLower().StartsWith("input"))
                              .ToArray();

        if (images.Length == 0)
        {
            Console.WriteLine("No input images found.");
            return;
        }

        int count = 1;

        foreach (var path in images)
        {
            Console.WriteLine($"\nWorking on: {Path.GetFileName(path)}");

            // Load image in grayscale
            var gray = Cv2.ImRead(path, ImreadModes.Grayscale);
            if (gray.Empty())
            {
                Console.WriteLine("Skipped: Unable to read this image.");
                continue;
            }

            // Blur and threshold to prepare for edge detection
            var blurred = new Mat();
            Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

            var thresh = new Mat();
            Cv2.Threshold(blurred, thresh, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

            // Find outer contour
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(thresh, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0)
            {
                Console.WriteLine("No edges found.");
                continue;
            }

            // Find the biggest contour
            int maxIdx = 0;
            double maxArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area > maxArea)
                {
                    maxArea = area;
                    maxIdx = i;
                }
            }

            // Draw blue contour on white background
            var output = new Mat(thresh.Size(), MatType.CV_8UC3, Scalar.White);
            Cv2.DrawContours(output, contours, maxIdx, new Scalar(255, 0, 0), 2, LineTypes.AntiAlias);

            // Save with unique name
            string outputName = $"Output {count}.png";
            Cv2.ImWrite(outputName, output);
            Console.WriteLine($"Saved as: {outputName}");

            // Try to open the image after ensuring it's saved and available
            try
            {
                // Short delay to ensure the file is fully written
                System.Threading.Thread.Sleep(500);

                if (File.Exists(outputName))
                {
                    Process.Start(new ProcessStartInfo(outputName) { UseShellExecute = true });
                }
                else
                {
                    Console.WriteLine("File was not found when attempting to open.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open the output image. Reason: {ex.Message}");
            }

            count++;
        }

        Console.WriteLine("\nDone! All images processed.");
    }
}
