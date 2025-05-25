# Shape Editor


Shape Editor is a tool for the interactive creation of shapes. Shape Editor generates an array of points in code that represent the drawn shape, making it an essential resource for developers looking to integrate shape data into their projects.



![111](https://github.com/user-attachments/assets/dc2812d3-786e-4ddb-96e7-79eef9f17d73)



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




![101](https://github.com/user-attachments/assets/845bf830-11d0-42ae-b6c9-19cae3fc9051)


### Benefits
- **Efficiency**: Saves time in designing symmetrical shapes, making your workflow faster.
- **Creativity**: Facilitates experimentation with shapes, allowing for complex designs without manual adjustments.
- **Precision**: Ensures both sides of the shape are aligned and proportional, enhancing design quality.
- **User-Friendly**: Accessible for beginners while still being valuable for experienced developers.

[Index](#index)















## Comma-Separated Values (CSV) Files

### File Format
CSV files, are a simple and widely used format for storing and exchanging data in a tabular structure. For the Shape Editor, each line represents a point in the shape, formatted as:
```
x,y
```


![108](https://github.com/user-attachments/assets/adb9cd85-1537-42ef-b634-c52a071aeaf8)



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

[Index](#index)















































## Dark Mode



![109](https://github.com/user-attachments/assets/92a94e62-d6d8-432f-a285-dbd36144c2f4)




- **Visual Comfort**: Reduces eye strain in low-light environments with a darker interface.
- **Aesthetic Appeal**: Offers a sleek, modern design that many users prefer.
- **Energy Efficiency**: Saves battery life on OLED screens by using less power for darker pixels.
- **User Preference**: Easily toggle between Light and Dark Modes to suit your environment or personal taste.

[Index](#index)







## Getting Started
To begin using the Shape Editor, launch the application and start creating shapes. The generated code will be available for you to copy and use in your projects.













# Code Walkthrough

This document provides a detailed walkthrough of a Visual Basic .NET (VB.NET) code for a shape editor application. This application allows users to draw shapes, add or remove points, and manipulate these shapes in a graphical interface. We'll go through the code line by line to understand its functionality and structure.

## Table of Contents
1. [Imports and Class Declaration](#imports-and-class-declaration)
2. [Enumerations](#enumerations)
3. [DLL Imports](#dll-imports)
4. [Class Variables](#class-variables)
5. [Form Load Event](#form-load-event)
6. [Paint Event](#paint-event)
7. [Mouse Events](#mouse-events)
8. [Key Events](#key-events)
9. [Drawing Methods](#drawing-methods)
10. [Shape Manipulation Methods](#shape-manipulation-methods)
11. [File Operations](#file-operations)
12. [Utility Methods](#utility-methods)
13. [UI Theme Management](#ui-theme-management)
14. [Layout Management](#layout-management)

---

## Imports and Class Declaration

```vb
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1
```
- **Imports System.IO**: This namespace is included for handling input and output operations, such as reading from and writing to files.
- **Imports System.Runtime.InteropServices**: This namespace is used for interoperation with unmanaged code, allowing the application to call functions from Windows DLLs.
- **Public Class Form1**: This line declares the main form of the application, where all the controls and logic will be defined.

---

## Enumerations

```vb
Enum Tool
    Add
    Subtract
    Move
End Enum
```
- **Enum Tool**: This enumeration defines the types of tools available in the application: 
  - **Add**: To add points to the shape.
  - **Subtract**: To remove points from the shape.
  - **Move**: To move points or the entire shape.

```vb
Public Enum DwmWindowAttribute
    DWMWA_USE_IMMERSIVE_DARK_MODE = 21
    DWMWA_MICA_EFFECT = 1029
End Enum
```
- **Enum DwmWindowAttribute**: This enumeration is used to define attributes for the window, specifically for enabling dark mode and the Mica effect in Windows.

---

## DLL Imports

```vb
<DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
Public Shared Function DwmSetWindowAttribute(hWnd As IntPtr, dwAttribute As DwmWindowAttribute, ByRef pvAttribute As Integer, cbAttribute As Integer) As Integer
End Function

<DllImport("uxtheme.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
Public Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
End Function
```
- **DllImport**: These attributes indicate that the methods are imported from unmanaged DLLs.
  - **DwmSetWindowAttribute**: Used to set attributes for the window, such as enabling dark mode.
  - **SetWindowTheme**: Used to change the theme of the window.

---

## Class Variables

```vb
Private CurrentTool As Tool = Tool.Add
```
- **CurrentTool**: A variable to keep track of which tool is currently selected (default is `Add`).

```vb
Private Points As New List(Of Point)()
Private LeftMouseButtonDown As Boolean = False
Private SelectedPointIndex As Integer = -1
Private HoveredPointIndex As Integer = -1
Private Const ControlHandleSize As Integer = 15
Private ScaleFactor As Double = 1.0
```
- **Points**: A list to store the points that define the shape.
- **LeftMouseButtonDown**: A boolean to check if the left mouse button is currently pressed.
- **SelectedPointIndex**: An integer to track the index of the currently selected point.
- **HoveredPointIndex**: An integer to track which point is currently hovered over.
- **ControlHandleSize**: A constant that defines the size of the control handles for the points.
- **ScaleFactor**: A double to manage the scaling of the drawing area.

The rest of the variables are used for colors, brushes, and other visual aspects of the UI.

---

## Form Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeApplication()
End Sub
```
- **Form1_Load**: This event is triggered when the form is loaded. It calls the `InitializeApplication` method to set up the application.

---

## Paint Event

```vb
Protected Overrides Sub OnPaint(e As PaintEventArgs)
    MyBase.OnPaint(e)
    e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)
    e.Graphics.Clear(BackgroundColor)
    DrawBoundingRectangle(e)
    DrawGrid(e)
    DrawCoordinateAxes(e)
    DrawCenterMark(e)
    DrawShape(e)
    DrawPointHandles(e)
End Sub
```
- **OnPaint**: This method is overridden to customize the painting of the form. It:
  - Translates the graphics context to the center of the drawing area.
  - Clears the background with the specified color.
  - Calls various drawing methods to render the bounding rectangle, grid, coordinate axes, center mark, shape, and point handles.

---

## Mouse Events

### Mouse Down

```vb
Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
    If e.Button = MouseButtons.Left Then
        AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))
        SelectedPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)

        If CurrentTool = Tool.Add Then
            If SelectedPointIndex = -1 Then
                AddPoint(AdjustedMouseLocation)
            Else
                InsertNewPoint(SelectedPointIndex)
            End If
        ElseIf CurrentTool = Tool.Move Then
            Dim BoundingRect As Rectangle = GetBoundingRectangle()
            If SelectedPointIndex <> -1 Then
                MovePoint(AdjustedMouseLocation)
            ElseIf BoundingRect.Contains(AdjustedMouseLocation) Then
                MoveStartLocation = AdjustedMouseLocation
                MovingShape = True
            End If
        ElseIf CurrentTool = Tool.Subtract Then
            If SelectedPointIndex <> -1 Then
                RemovePoint(SelectedPointIndex)
            End If
        End If

        GeneratePointArrayText()
        Invalidate(DrawingArea)
        InvalidateToolButtons()
        LeftMouseButtonDown = True
    End If
End Sub
```
- **Form1_MouseDown**: This event is triggered when the mouse button is pressed. It checks if the left button is pressed and then:
  - Adjusts the mouse location based on the drawing center and scale factor.
  - Determines if a point is selected at the mouse location.
  - Depending on the current tool, it either adds a point, moves a point, or removes a point.
  - Calls `GeneratePointArrayText()` to update the text representation of the points.
  - Calls `Invalidate()` to refresh the drawing area.

### Mouse Move

```vb
Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
    AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

    If LeftMouseButtonDown AndAlso SelectedPointIndex <> -1 Then
        MovePoint(AdjustedMouseLocation)
        Invalidate(DrawingArea)
        InvalidateToolButtons()
        GeneratePointArrayText()
    End If

    Dim newHoveredPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)
    If newHoveredPointIndex <> HoveredPointIndex Then
        HoveredPointIndex = newHoveredPointIndex
        Invalidate(DrawingArea)
        InvalidateToolButtons()
    End If

    If MovingShape And LeftMouseButtonDown Then
        Dim offsetX As Integer = (AdjustedMouseLocation.X - MoveStartLocation.X)
        Dim offsetY As Integer = (AdjustedMouseLocation.Y - MoveStartLocation.Y)
        For i = 0 To Points.Count - 1
            Points(i) = New Point(Points(i).X + offsetX, Points(i).Y + offsetY)
        Next
        MoveStartLocation = AdjustedMouseLocation
        Invalidate(DrawingArea)
        InvalidateToolButtons()
        GeneratePointArrayText()
    End If
End Sub
```
- **Form1_MouseMove**: This event is triggered when the mouse is moved. It:
  - Adjusts the mouse location.
  - If the left mouse button is down and a point is selected, it moves the point.
  - Updates the hovered point index and refreshes the drawing area.
  - If the shape is being moved, it calculates the offset and shifts all points accordingly.

### Mouse Up

```vb
Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
    If e.Button = MouseButtons.Left Then
        LeftMouseButtonDown = False
        SelectedPointIndex = -1
        GeneratePointArrayText()
        MovingShape = False
    End If
End Sub
```
- **Form1_MouseUp**: This event is triggered when the mouse button is released. It resets the relevant flags and updates the point array text.

---

## Key Events

### Key Down

```vb
Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
    If e.Control AndAlso e.KeyCode = Keys.C Then
        If Not String.IsNullOrEmpty(TextBox1.Text) Then
            Clipboard.SetText(TextBox1.Text)
        End If
        e.Handled = True
    ElseIf e.Control AndAlso e.KeyCode = Keys.D Then
        If Not ControlDDown Then
            ControlDDown = True
            DarkMode = Not DarkMode
            DarkModeToolStripMenuItem.Text = If(DarkMode, "Light Mode", "Dark Mode")
            ApplyUITheme()
            Refresh()
        End If
        e.Handled = True
    End If
    ' Additional key handling...
End Sub
```
- **Form1_KeyDown**: This event is triggered when a key is pressed. It checks for specific key combinations (like Ctrl+C for copying text) and toggles dark mode with Ctrl+D.

### Key Up

```vb
Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
    If e.Control AndAlso e.KeyCode = Keys.D Then
        ControlDDown = False
    End If
End Sub
```
- **Form1_KeyUp**: This event resets the control flags when the keys are released.

---

## Drawing Methods

### DrawGrid

```vb
Private Sub DrawGrid(e As PaintEventArgs)
    Dim stepSize As Integer = CInt(20 * ScaleFactor)
    Dim gridPen As Pen = If(DarkMode, GridPenDark, GridPenLight)

    For i As Integer = -(((DrawingArea.Width \ 2) * ScaleFactor) \ stepSize) To ((DrawingArea.Width \ 2) * ScaleFactor) \ stepSize
        Dim x As Integer = i * stepSize
        e.Graphics.DrawLine(gridPen, x, -CInt((DrawingArea.Height \ 2) * ScaleFactor), x, CInt((DrawingArea.Height \ 2) * ScaleFactor))
    Next

    For i As Integer = -(((DrawingArea.Height \ 2) * ScaleFactor) \ stepSize) To ((DrawingArea.Height \ 2) * ScaleFactor) \ stepSize
        Dim y As Integer = i * stepSize
        e.Graphics.DrawLine(gridPen, -CInt((DrawingArea.Width \ 2) * ScaleFactor), y, CInt((DrawingArea.Width \ 2) * ScaleFactor), y)
    Next
End Sub
```
- **DrawGrid**: This method draws a grid on the drawing area. It calculates the step size based on the scale factor and draws vertical and horizontal lines at regular intervals.

### DrawCoordinateAxes

```vb
Private Sub DrawCoordinateAxes(e As PaintEventArgs)
    e.Graphics.DrawLine(CoordinateSystemPen, -CInt((DrawingArea.Width \ 2) * ScaleFactor), 0, CInt((DrawingArea.Width \ 2) * ScaleFactor), 0)
    e.Graphics.DrawLine(CoordinateSystemPen, 0, -CInt((DrawingArea.Height \ 2) * ScaleFactor), 0, CInt((DrawingArea.Height \ 2) * ScaleFactor))
End Sub
```
- **DrawCoordinateAxes**: This method draws the X and Y axes at the center of the drawing area.

### DrawShape

```vb
Private Sub DrawShape(e As PaintEventArgs)
    If Points.Count > 1 Then
        Dim orderedPoints = GetOrderedPoints()
        Dim scaledPoints = orderedPoints.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()

        If FillShape Then
            e.Graphics.FillPolygon(ShapeFillBrush, scaledPoints)
        End If

        e.Graphics.DrawPolygon(ShapePen, scaledPoints)
    End If
End Sub
```
- **DrawShape**: This method draws the shape defined by the points. It checks if there are enough points to form a shape, scales the points, fills the shape if required, and then draws the polygon outline.

### DrawBoundingRectangle

```vb
Private Sub DrawBoundingRectangle(e As PaintEventArgs)
    If IsInsideBoundingRectangle AndAlso CurrentTool = Tool.Move Then
        Dim BoundingRect As Rectangle = GetBoundingRectangle()
        Dim ScaledBoundingRectangle As Rectangle = BoundingRect
        ScaledBoundingRectangle.X = CInt(BoundingRect.X * ScaleFactor)
        ScaledBoundingRectangle.Y = CInt(BoundingRect.Y * ScaleFactor)
        ScaledBoundingRectangle.Width = CInt(BoundingRect.Width * ScaleFactor)
        ScaledBoundingRectangle.Height = CInt(BoundingRect.Height * ScaleFactor)

        e.Graphics.FillRectangle(BoundingBrush, ScaledBoundingRectangle)
    End If
End Sub
```
- **DrawBoundingRectangle**: This method draws a bounding rectangle around the shape if the mouse is inside it and the current tool is set to move.

---

## Shape Manipulation Methods

### AddPoint

```vb
Private Sub AddPoint(location As Point)
    Points.Add(location)
    Points.Add(GetMirroredPoint(location))
    SelectedPointIndex = Points.Count - 2
End Sub
```
- **AddPoint**: This method adds a new point to the shape and its mirrored counterpart. It also updates the selected point index.

### MovePoint

```vb
Private Sub MovePoint(location As Point)
    Points(SelectedPointIndex) = location
    Points(SelectedPointIndex + 1) = GetMirroredPoint(location)
End Sub
```
- **MovePoint**: This method updates the position of the selected point and its mirrored counterpart.

### RemovePoint

```vb
Private Sub RemovePoint(index As Integer)
    If index >= 0 AndAlso index < Points.Count - 1 Then
        Points.RemoveAt(index + 1) ' Remove mirrored point
        Points.RemoveAt(index)     ' Remove actual point
    End If
    SelectedPointIndex = -1
    GeneratePointArrayText()
    Invalidate(DrawingArea)
End Sub
```
- **RemovePoint**: This method removes a point and its mirrored counterpart from the shape.

### InsertNewPoint

```vb
Private Sub InsertNewPoint(index As Integer)
    Dim newPoint As New Point(Points(index).X, Points(index).Y)
    newPoint.Offset(-1, -1)
    Points.Insert(index + 2, newPoint)
    Points.Insert(index + 3, GetMirroredPoint(newPoint))
    SelectedPointIndex += 2
    GeneratePointArrayText()
    Invalidate(DrawingArea)
End Sub
```
- **InsertNewPoint**: This method inserts a new point at the specified index and adds its mirrored counterpart.

---

## File Operations

### SaveShapeToFile

```vb
Private Sub SaveShapeToFile()
    Using saveFileDialog As New SaveFileDialog()
        saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
        saveFileDialog.Title = "Save Shape"
        saveFileDialog.InitialDirectory = Application.StartupPath

        If saveFileDialog.ShowDialog(Me) = DialogResult.OK Then
            Using writer As New StreamWriter(saveFileDialog.FileName)
                writer.WriteLine("X,Y")
                For Each point As Point In Points
                    writer.WriteLine($"{point.X},{point.Y}")
                Next
            End Using
            Text = $"{Path.GetFileName(saveFileDialog.FileName)} - Shape Editor - Code with Joe"
        End If
    End Using
End Sub
```
- **SaveShapeToFile**: This method opens a file dialog to save the current shape as a CSV file. It writes the points to the file in a structured format.

### OpenShapeFile

```vb
Private Sub OpenShapeFile()
    Using openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        openFileDialog.Title = "Open Shape"
        openFileDialog.InitialDirectory = Application.StartupPath

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Points.Clear()
            Try
                Using reader As New StreamReader(openFileDialog.FileName)
                    While Not reader.EndOfStream
                        Dim line As String = reader.ReadLine()
                        Dim parts As String() = line.Split(","c)
                        If parts.Length = 2 Then
                            Dim x As Integer
                            Dim y As Integer
                            If Integer.TryParse(parts(0), x) AndAlso Integer.TryParse(parts(1), y) Then
                                Points.Add(New Point(x, y))
                            End If
                        End If
                    End While
                    Text = $"{Path.GetFileName(openFileDialog.FileName)} - Shape Editor - Code with Joe"
                End Using
            Catch ex As Exception
                ' Handle exceptions...
            End Try
        End If
    End Using
End Sub
```
- **OpenShapeFile**: This method opens a file dialog to select a CSV file and reads the points from the file, adding them to the shape.

---

## Utility Methods

### GeneratePointArrayText

```vb
Private Sub GeneratePointArrayText()


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































## Index



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
 - Accessing the Icon in Code

**[License](#license)**
- License Information







