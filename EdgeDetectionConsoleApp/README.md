# Edge Detection in a .NET Console Application

## Overview

This C# console application utilizes OpenCvSharp to detect the largest outer edge in input images and render a blue border around it on a white background. The application processes images to identify the outer contour of objects, ignoring inner details and noise, and saves the result as a new image.

## How It Works

- Searches for image files named `Input*.jpg` or `Input*.png` in the current working directory.
- Loads each image in grayscale for processing.
- Applies Gaussian blur to reduce noise and performs binary thresholding using Otsu's method to prepare for edge detection.
- Identifies the largest outer contour, excluding inner markings and extraneous objects.
- Draws a blue outline, anti-aliased for smoothness, around the identified contour on a white background.
- Saves the result as `Output 1.png`, `Output 2.png`, etc., incrementing for each input image.
- Attempts to automatically open each output image after saving, with error handling for file access issues.

## Prerequisites

To run this application, you must install the `OpenCvSharp4` library and its runtime dependencies via NuGet. Execute the following commands in your terminal or Package Manager Console:

```bash
dotnet add package OpenCvSharp4
dotnet add package OpenCvSharp4.runtime.win
```

Ensure you are using a .NET environment compatible with the project, such as .NET 8.0.

## Project Structure and Output Location

### Input Images
Place your input images, named in the format `Input*.jpg` or `Input*.png`, in the same directory as the executable. For example:
- `Input1.jpg`
- `Input2.png`

### Output Location
When the program is executed, it will process the input images and save the results in the same directory as the executable. If you run the project using Visual Studio or the `dotnet run` command, the output files will be saved in the following default locations, depending on your build configuration:

- Debug: `<YourProjectFolder>\bin\Debug\net8.0\`
- Release: `<YourProjectFolder>\bin\Release\net8.0\`

### Output Files
The processed images will be named sequentially as follows:
- `Output 1.png`
- `Output 2.png`

The numbering increments for each input image processed.

## Usage

1. Place your input images (e.g., `Input1.jpg`, `Input2.png`) in the folder containing the executable (typically `bin\Debug\net8.0\` or `bin\Release\net8.0\`).
2. Build and run the program using Visual Studio, the `dotnet run` command, or by executing the compiled executable directly.
3. The program will process each input image, generate the corresponding output with a blue-bordered edge, and save it in the same directory.
4. Each output image will be automatically opened after processing, provided the system supports this functionality. If the file cannot be opened, an error message will be displayed.


## Notes

This application is designed to handle multiple input images in a single execution, ensuring flexibility for batch processing. It includes error handling for image loading, contour detection, and file operations to enhance reliability.
