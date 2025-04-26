# Shape Editor


The Shape Editor is an innovative tool designed for the interactive creation of shapes. It seamlessly generates a corresponding array of points in code that represent the drawn shape, making it an essential resource for developers looking to integrate shape data into their projects.



![095](https://github.com/user-attachments/assets/f5436a99-e48e-4b6e-b68e-91893183fe79)



## Features

- **Code Generation**: Automatically generates an array of points for the drawn shape, simplifying integration into your codebase.
- **Copy and Paste Functionality**: Easily copy the generated code from the code view and paste it into your preferred code editor for further development.
- **Immediate Feedback**: Quickly test and adjust shapes as you create them, enhancing your workflow.
- **User-Friendly Interface**: Simplifies the process of integrating shape data, making it accessible for users of all skill levels.
- **Interactive Development**: Encourages hands-on shape creation and coding, fostering creativity and experimentation.

[Index](#index)












## Point Mirroring

### How It Works
- **Symmetrical Design**: Draw a shape on one side of the horizontal axis, and the Shape Editor will automatically generate the mirrored counterpart on the opposite side.
- **Real-Time Updates**: Adjust points or modify the shape on one side, and the mirrored shape updates instantly, providing immediate visual feedback.



![088](https://github.com/user-attachments/assets/e97c5d38-2376-484f-b8a1-dea229126355)



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

![089](https://github.com/user-attachments/assets/f7c4602b-4210-4685-91a2-2026a678016d)




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



![010](https://github.com/user-attachments/assets/b3a93d24-8a81-4280-a4b0-76cc97c611df)




- **Visual Comfort**: Reduces eye strain in low-light environments with a darker interface.
- **Aesthetic Appeal**: Offers a sleek, modern design that many users prefer.
- **Energy Efficiency**: Saves battery life on OLED screens by using less power for darker pixels.
- **User Preference**: Easily toggle between Light and Dark Modes to suit your environment or personal taste.








## Getting Started
To begin using the Shape Editor, launch the application and start creating shapes. The generated code will be available for you to copy and use in your projects.













# Code Walkthrough



## Imports

```vb
Imports System.IO
Imports System.Runtime.InteropServices
```
- `Imports System.IO`: This imports the `System.IO` namespace, which contains classes for handling file input and output operations.
- `Imports System.Runtime.InteropServices`: This imports the `System.Runtime.InteropServices` namespace, used for interoperability with unmanaged code, enabling the use of Windows API functions.

## Class Definition

```vb
Public Class Form1
```
This line defines a public class named `Form1`, which represents the main form of the application. All the functionality of the shape editor will be encapsulated within this class.

## Enum Definitions

```vb
Enum Tool
    Add
    Subtract
    Move
End Enum
```
This `Enum` defines three tools that the user can select:
- `Add`: For adding points to the shape.
- `Subtract`: For removing points from the shape.
- `Move`: For moving existing points.

```vb
Private CurrentTool As Tool = Tool.Add
```
This line initializes a variable `CurrentTool` to `Tool.Add`, meaning the default tool when the application starts is the "Add" tool.

### DWM Window Attributes

```vb
Public Enum DwmWindowAttribute
    DWMWA_USE_IMMERSIVE_DARK_MODE = 21
    DWMWA_MICA_EFFECT = 1029
End Enum
```
This `Enum` defines attributes for window management, specifically for the Desktop Window Manager (DWM). These attributes control the appearance and behavior of the window.

### DllImport Declarations

```vb
<DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
Public Shared Function DwmSetWindowAttribute(hWnd As IntPtr, dwAttribute As DwmWindowAttribute, ByRef pvAttribute As Integer, cbAttribute As Integer) As Integer
End Function
```
This `DllImport` attribute allows the function `DwmSetWindowAttribute` to be called from the unmanaged `dwmapi.dll`. It sets various attributes for a window.

```vb
<DllImport("uxtheme.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
Public Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
End Function
```
Similarly, this imports the `SetWindowTheme` function from `uxtheme.dll`, which changes the theme of a window.

### Variables and Constants

```vb
Private points As New List(Of Point)()
```
This initializes a new list to store the points that define the shapes drawn by the user.

```vb
Private isDrawing As Boolean = False
```
This boolean variable tracks whether the user is currently drawing a shape.

```vb
Private selectedPointIndex As Integer = -1
Private hoveredPointIndex As Integer = -1
```
These integers track the index of the currently selected point and the point currently hovered over by the mouse.

```vb
Private Const handleSize As Integer = 15
```
This constant defines the size of the handles used to manipulate the points.

```vb
Private ScaleFactor As Double = 1.0
```
This variable determines the scaling factor for drawing shapes, allowing for zooming in and out.

### Color and Brush Definitions

```vb
Private ShapePen As New Pen(Color.Black, 2)
```
This creates a pen with a black color and a width of 2 pixels, used for drawing shapes.

```vb
Private HandleBrush As New SolidBrush(Color.FromArgb(255, Color.DarkGray))
Private HoverBrush As New SolidBrush(Color.FromArgb(255, Color.Gray))
```
These brushes are used to fill the handles for the points and indicate when a point is hovered over.

### Color Definitions for Light and Dark Modes

```vb
Private MenuItemBackgroundColor_LightMode As Color = Color.FromArgb(255, 240, 240, 240)
Private MenuItemBackgroundColor_DarkMode As Color = Color.FromArgb(255, 23, 23, 23)
```
These variables define the background colors for menu items in light and dark modes.

### Form Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
```
This method is called when the form loads. It initializes various components and settings for the application.

```vb
KeyPreview = True
```
This allows the form to receive key events before they are passed to the control that has focus.

```vb
DoubleBuffered = True
```
This enables double buffering to reduce flickering during drawing operations.

```vb
Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled
Application.EnableVisualStyles()
```
These lines enable visual styles for the application, allowing it to have a modern look.

```vb
ApplyUITheme()
```
This method applies the current UI theme (light or dark) to the application.

```vb
Text = "Shape Editor - Code with Joe"
```
This sets the title of the application window.

```vb
ScaleFactor = TrackBar1.Value / 100.0
Label1.Text = $"Scale: {ScaleFactor:N2}"
```
This initializes the scale factor based on the value of a trackbar and updates a label to display the current scale.

### OnPaint Method

```vb
Protected Overrides Sub OnPaint(e As PaintEventArgs)
```
This method is overridden to customize the painting of the form. It handles all the drawing operations on the form's graphics.

```vb
e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)
```
This translates the origin of the drawing area to the center, making it easier to draw shapes relative to the center of the form.

```vb
e.Graphics.Clear(If(DarkMode, Color.Black, Color.White))
```
This clears the background with either black or white, depending on the current theme.

### Drawing the Grid and Coordinate System

```vb
DrawGrid(e.Graphics)
```
This method is called to draw a grid on the drawing area.

```vb
e.Graphics.DrawLine(If(DarkMode, CoordinateSystemPenDarkMode, CoordinateSystemPenLightMode), -ClientSize.Width * 8, 0, ClientSize.Width * 8, 0) ' X-axis
```
This draws the X-axis of the coordinate system.

```vb
e.Graphics.DrawLine(If(DarkMode, CoordinateSystemPenDarkMode, CoordinateSystemPenLightMode), 0, -ClientSize.Height * 8, 0, ClientSize.Height * 8) ' Y-axis
```
This draws the Y-axis of the coordinate system.

### Drawing the Shape

```vb
If points.Count > 1 Then
    Dim orderedPoints = GetOrderedPoints()
    Dim scaledPoints = orderedPoints.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()
```
This checks if there are enough points to draw a shape and scales them based on the current scale factor.

```vb
If FillShape Then
    e.Graphics.FillPolygon(ShapeFillBrush, scaledPoints)
End If
```
If the fill shape option is enabled, this fills the shape with the specified brush.

```vb
e.Graphics.DrawPolygon(ShapePen, scaledPoints)
```
This draws the outline of the shape using the previously defined pen.

### Drawing Point Handles

```vb
If Not HideControlHandles Then
    For i As Integer = 0 To points.Count - 1 Step 2
        Dim point = points(i)
        Dim scaledPoint = New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
```
This loop iterates through the points to draw handles for each point, allowing the user to manipulate them.

```vb
If i = selectedPointIndex OrElse i = hoveredPointIndex Then
    e.Graphics.FillRectangle(HoverBrush, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
Else
    e.Graphics.FillRectangle(HandleBrush, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
End If
```
This checks if the point is selected or hovered over to change its appearance accordingly.

### Mouse Events

#### Mouse Down Event

```vb
Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
```
This method is triggered when the mouse button is pressed. It handles point selection and addition.

```vb
If e.Button = MouseButtons.Left Then
```
This checks if the left mouse button was pressed.

```vb
AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))
```
This calculates the adjusted mouse location based on the scale factor, allowing accurate point placement.

### Point Manipulation

```vb
If CurrentTool = Tool.Add Then
    If selectedPointIndex = -1 Then
        AddPoint(AdjustedMouseLocation)
    End If
```
If the current tool is "Add" and no point is selected, a new point is added at the adjusted mouse location.

```vb
ElseIf CurrentTool = Tool.Move Then
    If selectedPointIndex <> -1 Then
        MovePoint(AdjustedMouseLocation)
    End If
```
If the current tool is "Move" and a point is selected, that point is moved to the adjusted mouse location.

```vb
ElseIf CurrentTool = Tool.Subtract Then
    If selectedPointIndex <> -1 Then
        RemovePoint(selectedPointIndex)
    End If
End If
```
If the current tool is "Subtract" and a point is selected, that point is removed.

### Mouse Move Event

```vb
Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
```
This method updates the position of selected points as the mouse moves.

```vb
If isDrawing AndAlso selectedPointIndex <> -1 Then
    MovePoint(AdjustedMouseLocation)
    GeneratePointArrayText()
    Invalidate()
End If
```
If the user is drawing and a point is selected, the point is moved, and the point array text is generated.

### Key Events

```vb
Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
```
This method handles keyboard inputs for specific actions like closing shapes or deleting points.

```vb
If e.KeyCode = Keys.Delete AndAlso selectedPointIndex <> -1 Then
    RemovePoint(selectedPointIndex)
```
If the Delete key is pressed and a point is selected, that point is removed.

### Resizing the Form

```vb
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
```
This method adjusts the layout of UI components when the form is resized.

```vb
LayoutForm()
Invalidate()
```
This calls the `LayoutForm` method to reposition controls and invalidates the form to trigger a repaint.

### Saving and Opening Shapes

The `SaveToolStripMenuItem_Click` and `OpenToolStripMenuItem_Click` methods handle saving shapes to CSV files and opening them, respectively. They ensure the points are written in a readable format and can be easily reconstructed.












# Save and Open Functionality in Shape Editor

In this section, we will break down the code that handles saving and opening shape files in the Shape Editor application. This functionality allows users to save their drawn shapes as CSV files and load them back into the application.

## Save Functionality

### Method Definition

```vb
Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
```
This method is triggered when the user clicks the "Save" option from the menu. It handles the process of saving the current shape points to a file.

### Using SaveFileDialog

```vb
Using saveFileDialog As New SaveFileDialog()
```
This creates a new instance of `SaveFileDialog`, which provides a dialog for the user to specify the file name and location to save the shape.

### Setting Dialog Properties

```vb
saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
saveFileDialog.Title = "Save Shape"
saveFileDialog.InitialDirectory = Application.StartupPath
```
- `Filter`: This specifies the types of files that can be saved. The user can choose to save as CSV files or any file type.
- `Title`: This sets the title of the dialog window.
- `InitialDirectory`: This sets the starting directory of the dialog to the application's startup path.

### Showing the Dialog

```vb
If saveFileDialog.ShowDialog(Me) = DialogResult.OK Then
```
This line displays the dialog to the user. If the user selects a file and clicks "OK", the following code block executes.

### Writing to the File

```vb
Using writer As New StreamWriter(saveFileDialog.FileName)
```
This creates a `StreamWriter` to write text to the specified file.

```vb
' Write the CSV headers (optional).
writer.WriteLine("X,Y")
```
This writes the headers "X,Y" to the CSV file, indicating the format of the data that follows.

### Looping Through Points

```vb
For Each point As Point In points
    writer.WriteLine($"{point.X},{point.Y}")
Next
```
This loop iterates through each point in the `points` list and writes its X and Y coordinates to the file in CSV format.

### Updating the Title Bar

```vb
Text = $"{Path.GetFileName(saveFileDialog.FileName)} - Shape Editor - Code with Joe"
```
This updates the title of the form to include the name of the saved file, providing feedback to the user about the current file being edited.

### Closing the Dialog

```vb
End If
```
This ends the conditional block for the `SaveFileDialog`.

## Open Functionality

### Method Definition

```vb
Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
```
This method is triggered when the user clicks the "Open" option from the menu. It handles the process of loading shape points from a file.

### Using OpenFileDialog

```vb
Using openFileDialog As New OpenFileDialog()
```
This creates a new instance of `OpenFileDialog`, which allows the user to select a file to open.

### Setting Dialog Properties

```vb
openFileDialog.AutoUpgradeEnabled = True
openFileDialog.ShowReadOnly = False
openFileDialog.ShowHelp = False
```
These properties configure the dialog's behavior:
- `AutoUpgradeEnabled`: Automatically upgrades the dialog's appearance if needed.
- `ShowReadOnly`: Allows the user to open files in read-only mode.
- `ShowHelp`: Disables the help button in the dialog.

```vb
openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
openFileDialog.Title = "Open Shape"
openFileDialog.InitialDirectory = Application.StartupPath
```
Similar to the save dialog, this sets the file types, title, and initial directory.

### Showing the Dialog

```vb
If openFileDialog.ShowDialog() = DialogResult.OK Then
```
This displays the dialog. If the user selects a file and clicks "OK", the following code block executes.

### Clearing Existing Points

```vb
points.Clear()
```
This clears the existing points from the `points` list to prepare for loading new points from the file.

### Reading from the File

```vb
Dim fileIsValid As Boolean = False

Try
    Using reader As New StreamReader(openFileDialog.FileName)
```
This initializes a boolean to track validity and attempts to read the selected file using a `StreamReader`.

### Parsing the Points

```vb
While Not reader.EndOfStream
    Dim line As String = reader.ReadLine()
    Dim parts As String() = line.Split(","c)
```
This loop reads the file line by line until the end. Each line is split by commas into an array of strings.

```vb
If parts.Length = 2 Then
    Dim x As Integer
    Dim y As Integer

    If Integer.TryParse(parts(0), x) AndAlso Integer.TryParse(parts(1), y) Then
        points.Add(New Point(x, y))
```
If the line contains two parts (X and Y), it attempts to parse them into integers and adds them as a new `Point` to the `points` list.

### Validating the Points

```vb
fileIsValid = Integer.TryParse(parts(0), x)
fileIsValid = Integer.TryParse(parts(1), y)
```
This validates the parsed integers, ensuring they are valid coordinates.

### Exception Handling

```vb
Catch ex As Exception
```
This block catches any exceptions that might occur during file reading.

```vb
Select Case True
    Case TypeOf ex Is IOException
        MessageForm.Show("This file is in use by another app. Close the file and try again.", "File In Use - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
    ' Other cases for different exceptions...
End Select
```
This switch statement handles specific exceptions, providing user-friendly error messages based on the type of error encountered.

### Updating the Title Bar

```vb
Text = $"{Path.GetFileName(openFileDialog.FileName)} - Shape Editor - Code with Joe"
```
This updates the title to reflect the name of the opened file.

### Final Adjustments

```vb
CurrentTool = Tool.Move
RefreshToolIcons()
ScaleFactor = 8
TrackBar1.Value = CInt(ScaleFactor * 100)
UpdateUIScaleFactor()
GeneratePointArrayText()
Invalidate()
```
These lines reset the current tool to "Move", refresh the tool icons, set the scale factor, and generate the point array text for display. Finally, it calls `Invalidate()` to refresh the form.

This section of code effectively handles saving and opening shapes in the Shape Editor application. It provides a user-friendly interface for managing shape data while ensuring robust error handling and feedback. Understanding this functionality is essential for any VB.NET developer looking to create interactive applications. Happy coding!

















---











# Creating Transparent Icons in GIMP

Here’s a step-by-step guide to making transparent .png icons in GIMP.

## Step 1: Open GIMP
- Launch the GIMP application on your computer.

## Step 2: Create New File for the Icon
- Go to **File** > **New**.
- Enter your desired icon size and select **Fill with Transparency**, then click **OK**.

![Creating New File](https://github.com/user-attachments/assets/a0ae2244-6441-4947-815e-e6565e5f3526)

- You should now see a transparent background layer (indicated by a checkerboard pattern).

![Transparent Background](https://github.com/user-attachments/assets/699bfa95-ddd5-4665-b8a7-666e2b87ab42)

## Step 3: Zoom In
- Use the zoom tool to focus on detailed areas for easier drawing.

![Zoom In](https://github.com/user-attachments/assets/4629c209-16e5-44db-89dd-94220e782b94)

## Step 4: Create New Layer
- Go to **Layer** > **New**.
- Enter your layer name, select **Fill with Transparency**, then click **OK**.

![New Layer](https://github.com/user-attachments/assets/1f7dd5da-8eb3-4427-8ccd-c5951b01f4f9)

![Layer Management](https://github.com/user-attachments/assets/aca0386b-a044-4273-8aa0-2db721a6f6d3)

## Step 5: Click to Draw
- Use the **Pencil Tool** to draw. Set your pencil size to 1 pixel.

![Pencil Tool](https://github.com/user-attachments/assets/a2d8df8d-5511-413c-af27-24530296c971)

## Step 6: Undo Mistakes
- If you make a mistake, use **Ctrl + Z** to undo.

## Step 7: Export the Image
- Go to **File** > **Export As**.
- Choose **PNG** as the file format in the dialog.
- Click **Export**.

![Export Image](https://github.com/user-attachments/assets/6350105d-06b2-4c0e-ab14-eea2caa6788d)

## Step 8: Save Your Work
- Save your project in GIMP format (.xcf) if you want to retain layers for future editing.

### Tip
- **Layer Management**: Keep an eye on the layers panel to manage multiple layers effectively if you’re working with more complex images.






























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

        ' Set the icon for the form
        me.Icon = ResourceToImage(My.Resources.Resource1.YourIconName)

        ' Optionally, set the icon for a button
        Button2.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelected)

    End Sub

    Private Function ResourceToImage(resource As Byte()) As Image
        ' Convert the byte array to an Image using a MemoryStream and the
        ' Image.FromStream method to create an Image object from the byte array.
        ' This allows you to use the byte array as an image.

        Using ms As New MemoryStream(resource)

            Return Image.FromStream(ms)

        End Using

        ' The MemoryStream is disposed of automatically when it goes out of scope,
        ' so you don't need to worry about memory management.

    End Function

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











## Clones



![090](https://github.com/user-attachments/assets/88254d62-f74a-4d53-951d-b1337cad01b3)


![073](https://github.com/user-attachments/assets/0c224dc3-dd5a-40b8-b161-6829be5c9caf)









![044](https://github.com/user-attachments/assets/4f6a7ce3-7102-407a-9a16-f12672c225d8)








## License
This project is licensed under the MIT License.




![038](https://github.com/user-attachments/assets/9ccae402-4d60-4032-9696-6770b2872a27)




---































### Index for Shape Editor README



 **[Features](#features)**
   - Code Generation
   - Copy and Paste Functionality
   - Immediate Feedback
   - User-Friendly Interface
   - Interactive Development
   - Point Mirroring

 **[How It Works](#how-it-works)**
   - Symmetrical Design
   - Real-Time Updates

 **[Benefits](#benefits)**
   - Efficiency
   - Creativity
   - Precision
   - User-Friendly

 **[File Format](#file-format)**
   - Comma-Separated Values (CSV) Files
     - Usage
     - Benefits

 **[Dark Mode](#dark-mode)**
   - Visual Comfort
   - Aesthetic Appeal
   - Energy Efficiency
   - User Preference

 **[Getting Started](#getting-started)**
   - Launching the Application
   - Creating Shapes

 **[Code Walkthrough](#code-walkthrough)**
   - Imports
   - Class Definition
   - Enum Definitions
   - DWM Window Attributes
   - Variables and Constants
   - Color and Brush Definitions
   - Form Load Event
   - OnPaint Method
   - Drawing Operations
   - Mouse Events
   - Key Events
   - Resizing the Form
   - Saving and Opening Shapes

 **[Save and Open Functionality](#save-and-open-functionality)**
 - Save Functionality
 - Open Functionality

 **[Creating Transparent Icons in GIMP](#creating-transparent-icons-in-gimp)**
 - Step-by-Step Guide

 **[Adding Icons to a Resource File](#adding-icons-to-a-resource-file)**
 - Open Your Project
 - - Accessing the Icon in Code

**[License](#license)**
- License Information







