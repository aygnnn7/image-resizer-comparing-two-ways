# Image Resizer
## Overview
Create a WinForms application which down-sizes images.  
The app must let users select an image using the standard open file dialog, enter a downscaling factor (real number), and produce a new down-scaled image.  

You may not use GDI's image-resizing functions, or any other library for that matter.  You must implement one on your own.

You may not use Bitmap.SetPixel() or Bitmap.GetPixel().  
## Details
The downscaling factor is a percentage of the original size.  
The image aspect ratio must preserved. For example, if the original image size is 1000 x 500, a down-scaling factor of 50% will produce an image of size 500x250, a down-scaling factor of 10% will produce an image of size 100x50, and if the factor is 80%, the new image size will be 800 x 400.

## Task
Implement a downscaling algorithm in two ways: consequential and parallel, measure the performance with different image sizes and report the results. 
