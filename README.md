# Shape Editor


The Shape Editor is an innovative tool designed for the interactive creation of shapes. It seamlessly generates a corresponding array of points in code that represent the drawn shape, making it an essential resource for developers looking to integrate shape data into their projects.



![053](https://github.com/user-attachments/assets/bc39cae3-2eb1-4c32-8bc2-a8236abcba9b)



## Features

- **Code Generation**: Automatically generates an array of points for the drawn shape, simplifying integration into your codebase.
- **Copy and Paste Functionality**: Easily copy the generated code from the code view and paste it into your preferred code editor for further development.
- **Immediate Feedback**: Quickly test and adjust shapes as you create them, enhancing your workflow.
- **User-Friendly Interface**: Simplifies the process of integrating shape data, making it accessible for users of all skill levels.
- **Interactive Development**: Encourages hands-on shape creation and coding, fostering creativity and experimentation.

## Point Mirroring



![037](https://github.com/user-attachments/assets/c92103e6-f9fc-4396-8321-c7c1d6be40cd)




### How It Works
- **Symmetrical Design**: Draw a shape on one side of the horizontal axis, and the Shape Editor will automatically generate the mirrored counterpart on the opposite side.
- **Real-Time Updates**: Adjust points or modify the shape on one side, and the mirrored shape updates instantly, providing immediate visual feedback.

### Benefits
- **Efficiency**: Saves time in designing symmetrical shapes, making your workflow faster.
- **Creativity**: Facilitates experimentation with shapes, allowing for complex designs without manual adjustments.
- **Precision**: Ensures both sides of the shape are aligned and proportional, enhancing design quality.
- **User-Friendly**: Accessible for beginners while still being valuable for experienced developers.
















## Comma-Separated Values (CSV) Files

### File Format
CSV files, are a simple and widely used format for storing and exchanging data in a tabular structure. For the Shape Editor, each line represents a point in the shape, formatted as:
```
x,y
```


![005](https://github.com/user-attachments/assets/9afb34ea-7b22-43c3-8b1f-22740c38cc40)



- The first row often serves as a header, describing the column names.
- Each subsequent row contains data corresponding to the columns.

### Usage
- **Saving Shapes**: Users can save drawn shapes as CSV files for easy storage and retrieval.
- **Loading Shapes**: The application can read CSV files to reconstruct shapes, facilitating project continuity.
- **Data Integration**: CSV format allows for easy integration with other applications or programming environments.

### Benefits
- **Plain Text**: CSV files are plain text files, which means they are lightweight and easy to read and write.
- **Simplicity**: Easy to read and write, making it user-friendly.
- **Compatibility**: CSV files can be opened and edited in many applications, including text editors (like Notepad) and spreadsheet software (like Excel or Google Sheets).
- **Portability**: Easily shared and transferred between different systems.
- **Human-Readable**: The simplicity of CSV files makes them understandable without specialized tools.
















































## Dark Mode



![055](https://github.com/user-attachments/assets/503ace80-b5f2-4651-bd9e-153f02aea60a)




- **Visual Comfort**: Reduces eye strain in low-light environments with a darker interface.
- **Aesthetic Appeal**: Offers a sleek, modern design that many users prefer.
- **Energy Efficiency**: Saves battery life on OLED screens by using less power for darker pixels.
- **User Preference**: Easily toggle between Light and Dark Modes to suit your environment or personal taste.








## Getting Started
To begin using the Shape Editor, launch the application and start creating shapes. The generated code will be available for you to copy and use in your projects.














# Code Walkthrough


This document will guide you through each part of the code, explaining what it does and how it works. This will be useful for both beginners and those looking to improve their understanding of VB.NET programming. 

## Overview

This code defines a Windows Forms application that allows users to draw shapes on a canvas. Users can add points, manipulate them, and save/load shapes from CSV files. The application also supports light and dark themes.

## Code Breakdown

### Imports and Class Definition

```vb
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1
```

- **Imports System.IO**: This imports the `System.IO` namespace, which provides functionality for working with files and data streams.
- **Imports System.Runtime.InteropServices**: This imports the `System.Runtime.InteropServices` namespace, which is used for interoperability with unmanaged code, allowing the use of Windows API functions.
- **Public Class Form1**: This defines a public class named `Form1`, which represents the main form of the application.

### Enum Definitions

#### Tool Enum

```vb
Enum Tool
    Add
    Subtract
    Move
End Enum

Private CurrentTool As Tool = Tool.Add
```

- **Enum Tool**: This defines an enumeration called `Tool`, which includes three options: `Add`, `Subtract`, and `Move`. These represent different tools that can be used in the application.
- **Private CurrentTool**: This variable stores the currently selected tool, initialized to `Tool.Add`.

#### DwmWindowAttribute Enum

```vb
Public Enum DwmWindowAttribute
    dwmwa_invalid = -1
    DWMWA_NCRENDERING_ENABLED = 1
    ...
    DWMWA_LAST = 13
    dwmwa_use_dark_theme = 19
    ...
End Enum
```

- This enumeration defines various attributes for window management, specifically for the Desktop Window Manager (DWM). These attributes control the appearance and behavior of the window.

### DllImport Declarations

```vb
<DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
Public Shared Function DwmSetWindowAttribute(hWnd As IntPtr, dwAttribute As DwmWindowAttribute, ByRef pvAttribute As Integer, cbAttribute As Integer) As Integer
End Function

<DllImport("uxtheme.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
Public Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
End Function
```

- **DllImport**: This attribute is used to import functions from unmanaged DLLs. Here, two functions are imported:
  - `DwmSetWindowAttribute`: Sets various attributes for a window.
  - `SetWindowTheme`: Changes the theme of a window.

### Variable Declarations

```vb
Private points As New List(Of Point)()
Private isDrawing As Boolean = False
Private selectedPointIndex As Integer = -1
...
Private ScaleFactor As Double = 1.0
```

- **points**: A list to store the points that define the shape.
- **isDrawing**: A boolean to track whether the user is currently drawing.
- **selectedPointIndex**: An integer to track the index of the currently selected point.
- **ScaleFactor**: A double that determines the scaling factor for drawing.

### Pen and Brush Definitions

```vb
Private ShapePen As New Pen(Color.Black, 2)
Private HandleBrush As New SolidBrush(Color.FromArgb(255, Color.DarkGray))
Private HoverBrush As New SolidBrush(Color.FromArgb(255, Color.Gray))
...
```

- **ShapePen**: A pen used to draw shapes, initialized to black with a width of 2.
- **HandleBrush**: A brush used to draw handles for points, initialized to dark gray.
- **HoverBrush**: A brush used to indicate when a point is hovered over, initialized to gray.

### Color Definitions for Light and Dark Modes

The code contains multiple color definitions for different UI elements in both light and dark modes:

```vb
Private MenuItemBackgroundColor_LightMode As Color = Color.FromArgb(255, 240, 240, 240)
Private MenuItemBackgroundColor_DarkMode As Color = Color.FromArgb(255, 32, 32, 32)
...
```

These colors are used to style the application's UI based on the user's theme preference.

### Form Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
```

- This subroutine runs when the form loads. It initializes various components and settings for the application.

#### Key Operations in Form Load

1. **Load Button Image**:
   ```vb
   Dim imageBytes As Byte() = My.Resources.Resource1.AddPointToolButton
   Using ms As New MemoryStream(imageBytes)
       Button2.Image = Image.FromStream(ms)
   End Using
   ```
   - Converts a byte array from resources to an image for a button.

2. **Double Buffering**:
   ```vb
   Me.DoubleBuffered = True
   ```
   - Enables double buffering to reduce flickering during drawing.

3. **Set Window Theme**:
   ```vb
   SetWindowTheme(Me.Handle, "Explorer", Nothing)
   ```
   - Sets the window theme to "Explorer".

4. **Initialize UI Elements**:
   - Various UI components like scrollbars and buttons are themed and styled.

5. **Event Handlers**:
   ```vb
   AddHandler HideControlHandlesCheckBox.CheckedChanged, AddressOf HideControlHandlesCheckBox_CheckedChanged
   ```
   - Attaches event handlers to checkboxes for dynamic UI updates.

### Paint Event

```vb
Protected Overrides Sub OnPaint(e As PaintEventArgs)
    MyBase.OnPaint(e)
```

- This method is overridden to customize the painting of the form. It handles all the drawing operations on the form's graphics.

#### Key Drawing Operations

1. **Translate Graphics Origin**:
   ```vb
   e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)
   ```

2. **Clear Background**:
   ```vb
   e.Graphics.Clear(If(DarkModeCheckBox.Checked, Color.Black, Color.White))
   ```

3. **Draw Grid and Coordinate System**:
   - Draws the grid and the coordinate axes based on the current mode.

4. **Draw Shapes**:
   ```vb
   e.Graphics.DrawPolygon(ShapePen, scaledPoints)
   ```
   - Draws the polygon based on the points stored in the `points` list.

5. **Draw Point Handles**:
   - Draws the handles for each point based on their state (selected, hovered, etc.).

### Mouse Events

#### Mouse Down Event

```vb
Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
```

- This method is triggered when the mouse button is pressed. It handles point selection and addition.

#### Mouse Move Event

```vb
Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
```

- This method updates the position of selected points as the mouse moves.

#### Mouse Up Event

```vb
Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
```

- This method is triggered when the mouse button is released. It finalizes the drawing action.

### Key Events

```vb
Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
```

- This method handles keyboard inputs for specific actions like closing shapes or deleting points.

### Resize Event

```vb
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
```

- This method adjusts the layout of UI components when the form is resized.

### TrackBar Scroll Event

```vb
Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
```

- This method updates the scale factor based on the position of the trackbar.

### Save and Open Functionality

The code includes methods for saving and loading shapes from CSV files, allowing users to persist their drawings.

```vb
Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
```

- This method opens a save dialog and writes the points to a CSV file.

```vb
Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
```

- This method opens a file dialog to read points from a CSV file and populate the `points` list.



This code provides a comprehensive framework for a shape editor application in VB.NET. It covers essential programming concepts such as event handling, graphics rendering, and file I/O, making it a valuable learning resource for beginners. 

By understanding each part of this code, you will gain insights into Windows Forms applications, object-oriented programming, and the use of external libraries in VB.NET. Happy coding!


















## License
This project is licensed under the MIT License.




![038](https://github.com/user-attachments/assets/9ccae402-4d60-4032-9696-6770b2872a27)






---




# Tool Buttons

![056](https://github.com/user-attachments/assets/91829b77-57cc-4323-b2b7-78f926e05df2)


## Making icons ( transparent .png files ) in GIMP.

1. **Open GIMP**:
   - Launch the GIMP application on your computer.

2. **Create New File for the Icon**:
   - Go to **File** > **New** enter your icon size like width 41, height 41 then select ok.

3. **Select the Area to Make Transparent**:
   - Use the **Select Tool** (e.g., Fuzzy Select Tool, Color Select Tool, or Rectangle Select Tool) to select the area of the icon you want to make transparent.
   - Adjust the selection as needed.

4. **Delete the Selected Area**:
   - Press the **Delete** key on your keyboard. The selected area should now be transparent (indicated by a checkerboard pattern).

5. **Refine Edges (Optional)**:
   - If needed, use tools like the **Eraser Tool** or **Feather** to smooth the edges of your selection.

6. **Export the Image**:
   - Go to **File** > **Export As**.
   - In the dialog, choose **PNG** as the file format.
   - Ensure that the **Save color values from transparent pixels** option is checked.
   - Click **Export** and adjust any settings as needed.

7. **Save Your Work**:
   - Save your project in GIMP format (.xcf) if you want to retain layers for future editing.

### Tips
- **Undo**: If you make a mistake, use **Ctrl + Z** to undo.
- **Zoom In**: Use the zoom tool to work on detailed areas more easily.
- **Layer Management**: Keep an eye on the layers panel to manage multiple layers if you’re working with more complex images.



























## Adding Icons to a Resource File

1. **Open Your Project**:
   - Launch Visual Studio and open your project.

2. **Locate the Resource File**:
   - In the **Solution Explorer**, navigate to the `My Project` folder.
   - Open the `Resources.resx` file (it may be named `resource1.resx` or similar).

3. **Open the Resource Editor**:
   - Double-click on the `Resources.resx` file to open it in the resource editor.


![050](https://github.com/user-attachments/assets/80edabed-3b00-490f-a0c0-1fee9dc83104)


4. **Add an Icon File**:
   - In the resource editor, right-click in the blank area or on any existing resource.
   - Select **Add Resource** > **Add Existing File...**.

5. **Select Your Icon File**:
   - Navigate to the location of your icon file (e.g., .ico or .png), select it, and click **Open**.

6. **Rename the Resource (Optional)**:
   - After adding the icon, you can rename it for easier reference. Click on the name in the resource editor to edit it.

7. **Accessing the Icon in Code**:
   - You can access the icon in your code using the following syntax:



```vb.net

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Convert the byte array to an Image
        Dim ImageBytes As Byte() = My.Resources.Resource1.YourIconName

        ' Set the icon for the form
        Using ms As New MemoryStream(ImageBytes)
            Me.Icon = Image.FromStream(ms)
        End Using

        ' Optionally, set the icon for a button
        Using ms As New MemoryStream(ImageBytes)
            Button1.Image = Image.FromStream(ms)
        End Using

    End Sub

End Class

```



This code snippet converts a byte array from the resources into an `Image` object and assigns it to a button's image property.

### Explanation of the Code

1. **Byte Array Retrieval**:
   - `Dim imageBytes As Byte() = My.Resources.Resource1.AddPointToolButtonSelected`
   - This line retrieves the byte array for the image stored in your resources. Ensure that `AddPointToolButtonSelected` is the correct name of the resource.

2. **MemoryStream Usage**:
   - `Using ms As New MemoryStream(imageBytes)`
   - A `MemoryStream` is created to hold the byte array in memory, allowing it to be used as a stream.

3. **Image Creation**:
   - `Button2.Image = Image.FromStream(ms)`
   - The `Image.FromStream` method converts the stream into an `Image` object, which is then assigned to `Button2`.



![057](https://github.com/user-attachments/assets/64737783-951f-4f02-8b1a-20af085a8e91)


### Important Notes

- **Error Handling**: Consider adding error handling to manage any issues that may arise when loading the image.
- **Image Size**: Ensure that the size of `Button2` is appropriate for the image to avoid distortion.
- **Resource Management**: The `Using` statement ensures that the `MemoryStream` is disposed of properly after use, which is a good practice for resource management.
- **File Format**: Ensure your icon files are in an appropriate format (.ico or .png) for compatibility.
- **Resource Management**: The resources are embedded in your application, making them accessible at runtime without needing to manage external files.
- **Rebuild Your Project**: After adding resources, it’s a good practice to rebuild your project to ensure everything is up to date.











# Clones


![051](https://github.com/user-attachments/assets/f3e72a16-16c3-4e62-b978-ebf4219a12a7)



![044](https://github.com/user-attachments/assets/4f6a7ce3-7102-407a-9a16-f12672c225d8)













