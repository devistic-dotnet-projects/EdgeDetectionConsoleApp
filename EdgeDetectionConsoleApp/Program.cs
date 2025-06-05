using System;
using System.IO;
using OpenCvSharp;

class Program
{
    static void Main(string[] args)
    {
        // Accept both input.jpg or input.png
        string[] possibleFiles = { "input.jpg", "input.png" };
        string inputPath = null;
        foreach (var file in possibleFiles)
        {
            if (File.Exists(file))
            {
                inputPath = file;
                break;
            }
        }

        if (inputPath == null)
        {
            Console.WriteLine("Error: No input image found.");
            return;
        }

        // Load image in grayscale
        Mat gray = Cv2.ImRead(inputPath, ImreadModes.Grayscale);
        if (gray.Empty())
        {
            Console.WriteLine("Error: Could not load input image.");
            return;
        }

        // Blur to reduce noise
        Mat blurred = new Mat();
        Cv2.GaussianBlur(gray, blurred, new Size(5, 5), 0);

        // Apply binary threshold (Otsu) to get silhouette
        Mat thresh = new Mat();
        Cv2.Threshold(blurred, thresh, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

        // Find external contours only
        Point[][] contours;
        HierarchyIndex[] hierarchy;
        Cv2.FindContours(thresh, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

        // Create white canvas
        Mat output = new Mat(thresh.Size(), MatType.CV_8UC3, Scalar.White);

        if (contours.Length == 0)
        {
            Console.WriteLine("No contours found.");
            return;
        }

        // Find the largest contour by area
        int maxContourIdx = 0;
        double maxArea = 0;
        for (int i = 0; i < contours.Length; i++)
        {
            double area = Cv2.ContourArea(contours[i]);
            if (area > maxArea)
            {
                maxArea = area;
                maxContourIdx = i;
            }
        }

        // Draw only the outermost contour in blue
        //Cv2.DrawContours(output, contours, maxContourIdx, new Scalar(255, 0, 0), 1);
        Cv2.DrawContours(output, contours, maxContourIdx, new Scalar(255, 0, 0), 2, LineTypes.AntiAlias);

        // Save final output
        Cv2.ImWrite("output.png", output);
        Console.WriteLine("Edge detection complete. Output saved as 'output.png'.");
    }
}
