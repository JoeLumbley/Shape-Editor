# Shape Editor


Shape Editor allows you to draw shapes on the left side of the screen while the code is automatically generated on the right. This means you can focus on creativity. No more tedious typing or guesswork—just draw and watch the code come to life!


![126](https://github.com/user-attachments/assets/5388b962-699a-4777-8f03-1b168688d48a)




## Features

- **Code Generation**: Automatically generates an array of points for the drawn shape, simplifying integration into your codebase.
- **Copy and Paste Functionality**: Easily copy the generated code from the code view and paste it into your preferred code editor for further development.
- **Immediate Feedback**: Quickly test and adjust shapes as you create them, enhancing your workflow.
- **User-Friendly Interface**: Simplifies the process of integrating shape data, making it accessible for users of all skill levels.
- **Interactive Development**: Encourages hands-on shape creation and coding, fostering creativity and experimentation.
  
 **[Code Walkthrough](#code-walkthrough)**


[Index](#index)












## Point Mirroring

### How It Works
- **Symmetrical Design**: Draw a shape on one side of the horizontal axis, and the Shape Editor will automatically generate the mirrored counterpart on the opposite side.
- **Real-Time Updates**: Adjust points or modify the shape on one side, and the mirrored shape updates instantly, providing immediate visual feedback.



![127](https://github.com/user-attachments/assets/850385f6-3338-4d97-b2d7-0d8772f22b3c)



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


![128](https://github.com/user-attachments/assets/03721dd7-ca5a-44e0-a826-9692966c3ee5)



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




![129](https://github.com/user-attachments/assets/83693928-eada-4f10-aacb-af3a07956143)



- **Visual Comfort**: Reduces eye strain in low-light environments with a darker interface.
- **Aesthetic Appeal**: Offers a sleek, modern design that many users prefer.
- **Energy Efficiency**: Saves battery life on OLED screens by using less power for darker pixels.
- **User Preference**: Easily toggle between Light and Dark Modes to suit your environment or personal taste.

[Index](#index)







## Getting Started
To begin using the Shape Editor, launch the application and start creating shapes. The generated code will be available for you to copy and use in your projects.




### Contribute Your Unique Touch!

We believe that creativity knows no bounds, and that's why we encourage you to **fork this repository** and make it your own! The **Shape Editor** is just the beginning, and we’re excited to see how you can take this project in new directions. Here are some ideas to inspire your modifications:

- **Change the Language**: Instead of generating Visual Basic .NET code, how about creating a version that outputs **Java** or **C#**? Imagine the possibilities of integrating this tool into different ecosystems and making it accessible to a wider audience!

- **Explore Freeform Design**: While our current version focuses on symmetrical designs, you could implement a **freeform drawing feature**. This would allow users to create unique shapes without the constraints of symmetry, fostering even more creativity in their designs.

- **Add New Features**: Consider introducing additional functionalities, such as:
  - **Shape Animation**: Allow users to animate the shapes they create, bringing their designs to life.
  - **Export Options**: Implement options to export shapes in various formats (SVG, PNG, etc.) for easier integration into other projects.
  - **User-Defined Styles**: Enable users to customize the appearance of their shapes with different styles, colors, and patterns.

- **Integrate with Other Tools**: Think about how you could connect the Shape Editor with other design or development tools. Perhaps a plugin for popular IDEs or design software could enhance its usability.

### How to Get Started

1. **Fork the Repository**: Click the “Fork” button at the top right of this page to create your own copy of the Shape Editor.
2. **Make Your Changes**: Dive into the code and start implementing your ideas. Don’t hesitate to experiment and try new things!
3. **Share Your Work**: Once you’ve made your modifications, share your version with the community. We’d love to see what you’ve created!

### Join the Community

By forking this repository, you’re not just modifying code; you’re becoming part of a vibrant community of developers who are passionate about creativity and innovation. We can’t wait to see how you’ll contribute to the **Shape Editor** and inspire others along the way!

Happy coding! 🎉


---





# Code Walkthrough




## Imports and Class Declaration

```vb
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1
```
- **Imports System.IO**: This namespace is included for handling input and output operations, such as reading from and writing to files.
- **Imports System.Runtime.InteropServices**: This namespace is used for interoperation with unmanaged code, allowing the application to call functions from Windows DLLs.
- **Public Class Form1**: This line declares the main form of the application, where all the controls and logic will be defined.


[Index](#index)

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

[Index](#index)

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

[Index](#index)

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

[Index](#index)

---

## Form Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeApplication()
End Sub
```
- **Form1_Load**: This event is triggered when the form is loaded. It calls the `InitializeApplication` method to set up the application.

[Index](#index)

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

[Index](#index)

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

[Index](#index)

---

## Key Events

### Key Down Event Handling

The `Form1_KeyDown` method manages various keyboard inputs to enhance user interaction within the Shape Editor. Below is a breakdown of the functionality provided by each key combination:

### Code Implementation

```vb
Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

    ' Check if Control + C is pressed
    If e.Control AndAlso e.KeyCode = Keys.C Then
        ' Check if TextBox1.Text is not empty
        If Not String.IsNullOrEmpty(TextBox1.Text) Then
            ' Copy the text from TextBox1 to the clipboard
            Clipboard.SetText(TextBox1.Text)
        End If
        e.Handled = True

    ' Check if Control + D is pressed for Dark Mode toggle
    ElseIf e.Control AndAlso e.KeyCode = Keys.D Then
        If Not ControlDDown Then
            ControlDDown = True
            ' Toggle Dark Mode
            DarkMode = Not DarkMode
            DarkModeToolStripMenuItem.Text = If(DarkMode, "Light Mode", "Dark Mode")
            ApplyUITheme()
            UpdateTitleBarTheme()
            Refresh()
        End If
        e.Handled = True

    ' Check if Control + F is pressed for toggling shape fill
    ElseIf e.Control AndAlso e.KeyCode = Keys.F Then
        If Not ControlFDown Then
            ControlFDown = True
            ToggleShapeFill()
            Invalidate(DrawingArea)
            InvalidateToolButtons()
        End If
        e.Handled = True

    ' Check if Control + H is pressed for toggling handles visibility
    ElseIf e.Control AndAlso e.KeyCode = Keys.H Then
        If Not ControlHDown Then
            ControlHDown = True
            ToggleHandlesVisibility()
            Invalidate(DrawingArea)
            InvalidateToolButtons()
        End If
        e.Handled = True

    ' Check if Delete key is pressed to remove selected point
    ElseIf e.KeyCode = Keys.Delete AndAlso SelectedPointIndex <> -1 Then
        RemovePoint(SelectedPointIndex)
        Invalidate(DrawingArea)
        InvalidateToolButtons()
        e.Handled = True

    ' Check if OemMinus key is pressed to remove selected point
    ElseIf e.KeyCode = Keys.OemMinus AndAlso SelectedPointIndex <> -1 Then
        RemovePoint(SelectedPointIndex)
        Invalidate(DrawingArea)
        InvalidateToolButtons()
        e.Handled = True

    ' Check if Subtract key is pressed to remove selected point
    ElseIf e.KeyCode = Keys.Subtract AndAlso SelectedPointIndex <> -1 Then
        RemovePoint(SelectedPointIndex)
        Invalidate(DrawingArea)
        InvalidateToolButtons()
        e.Handled = True

    ' Check if N key is pressed to insert a new point at the selected index
    ElseIf e.KeyCode = Keys.N AndAlso SelectedPointIndex <> -1 Then
        InsertNewPoint(SelectedPointIndex)
        Invalidate(DrawingArea)
        e.Handled = True

    ' Check if Oemplus key is pressed to insert a new point
    ElseIf e.KeyCode = Keys.Oemplus AndAlso SelectedPointIndex <> -1 Then
        InsertNewPoint(SelectedPointIndex)
        Invalidate(DrawingArea)
        e.Handled = True

    ' Check if Add key is pressed to insert a new point
    ElseIf e.KeyCode = Keys.Add AndAlso SelectedPointIndex <> -1 Then
        InsertNewPoint(SelectedPointIndex)
        Invalidate(DrawingArea)
        e.Handled = True

    End If
End Sub
```

### Key Functionality Breakdown

1. **Copy to Clipboard (Ctrl + C)**
   - **Purpose**: Allows users to copy the current text from `TextBox1` to the clipboard for easy pasting elsewhere.
   - **Implementation**: Checks if the Control key is pressed along with the 'C' key. If `TextBox1` is not empty, it copies its content to the clipboard.

2. **Toggle Dark Mode (Ctrl + D)**
   - **Purpose**: Enables users to switch between dark and light themes for better visibility based on their preference.
   - **Implementation**: Toggles the `DarkMode` variable and updates the menu item text accordingly. Calls `ApplyUITheme()` to refresh the UI with the new theme.

3. **Toggle Shape Fill (Ctrl + F)**
   - **Purpose**: Allows users to toggle the fill of shapes being drawn.
   - **Implementation**: Calls `ToggleShapeFill()` to change the fill state and refreshes the drawing area.

4. **Toggle Handles Visibility (Ctrl + H)**
   - **Purpose**: Lets users show or hide the control handles of shapes for a cleaner workspace.
   - **Implementation**: Calls `ToggleHandlesVisibility()` to update the visibility state and refreshes the drawing area.

5. **Remove Selected Point (Delete, OemMinus, Subtract)**
   - **Purpose**: Enables users to delete the currently selected point from the shape.
   - **Implementation**: Checks if a point is selected and removes it using `RemovePoint()`, followed by refreshing the drawing area.

6. **Insert New Point (N, Oemplus, Add)**
   - **Purpose**: Allows users to insert a new point at the position of the currently selected point.
   - **Implementation**: Calls `InsertNewPoint()` to add a new point and refreshes the drawing area.

Each key combination enhances user interaction by providing intuitive shortcuts for common tasks, thereby improving the overall usability of the application.



### Key Up
[](#key-up)

The `Form1_KeyUp` event is triggered when a key is released. This event is used to reset certain control flags that were set during key down events. Below is the implementation of the `Form1_KeyUp` method:

```vb
Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
    If e.Control AndAlso e.KeyCode = Keys.D Then
        ControlDDown = False
    ElseIf e.Control AndAlso e.KeyCode = Keys.F Then
        ControlFDown = False
    ElseIf e.Control AndAlso e.KeyCode = Keys.H Then
        ControlHDown = False
    End If
End Sub
```

### Explanation of the Code
- **Purpose**: This method checks if specific control key combinations are released and updates the corresponding flags accordingly.
- **Control Flags**:
  - `ControlDDown`: This flag is set to `False` when the user releases the Control + D key combination, which is typically used for toggling dark mode.
  - `ControlFDown`: This flag is set to `False` when the user releases the Control + F key combination, which is used for toggling the shape fill.
  - `ControlHDown`: This flag is set to `False` when the user releases the Control + H key combination, which is used for toggling the visibility of shape handles.

### Usage
This event is essential for managing the state of keyboard shortcuts in the application, ensuring that the application responds correctly to user input and maintains an intuitive user experience.

[Index](#index)

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

[Index](#index)

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

[Index](#index)

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

[Index](#index)

---

## Utility Methods

### GeneratePointArrayText

```vb

    Private Sub GeneratePointArrayText()

        ' Create a new StringBuilder to construct the output text.
        Dim sb As New System.Text.StringBuilder()

        ' Add a line defining the scale factor variable with a comment.
        sb.AppendLine("Dim ScaleFactor As Double = 1.0 ' Adjust the scale factor as needed")

        ' Add a blank line for better readability.
        sb.AppendLine("")

        ' Start defining the array of Points.
        sb.AppendLine("Dim Shape As Point() = {")

        ' Retrieve the ordered list of points.
        Dim orderedPoints = GetOrderedPoints()

        ' Iterate through all the ordered points to format them as scaled Point objects.
        For i As Integer = 0 To orderedPoints.Count - 1
            If i < orderedPoints.Count - 1 Then

                ' Append each point with a trailing comma if it's not the last point in the list.
                sb.AppendLine($"    New Point(CInt({orderedPoints(i).X} * ScaleFactor), CInt({orderedPoints(i).Y} * ScaleFactor)),")

            Else

                ' Append the last point without a trailing comma.
                sb.AppendLine($"    New Point(CInt({orderedPoints(i).X} * ScaleFactor), CInt({orderedPoints(i).Y} * ScaleFactor))")

            End If
        Next

        ' Close the array definition.
        sb.AppendLine("}")

        ' Set the constructed string as the content of TextBox1.
        TextBox1.Text = sb.ToString()

        ' Check if TextBox1.Text is not null or empty
        If Not String.IsNullOrEmpty(TextBox1.Text) Then

            CopyLabel.Enabled = True

        End If

    End Sub

```
- **GeneratePointArrayText**: This method constructs a string representation of the points in the shape, formatted for easy copying. It generates a VB.NET array of `Point` objects, scaling the points by the `ScaleFactor`. The generated text is displayed in a `TextBox`, and if there's text, it enables a copy label.

### GetOrderedPoints

```vb
Private Function GetOrderedPoints() As List(Of Point)
    Dim orderedPoints As New List(Of Point)()
    For i As Integer = 0 To Points.Count - 1 Step 2
        orderedPoints.Add(Points(i))
    Next
    For i As Integer = Points.Count - 1 To 1 Step -2
        orderedPoints.Add(Points(i))
    Next
    If Points.Count > 0 Then
        orderedPoints.Add(Points(0)) ' Close the shape
    End If
    Return orderedPoints
End Function
```
- **GetOrderedPoints**: This function retrieves the points in the shape, ensuring they are ordered correctly for drawing. It adds points from the list and closes the shape by adding the first point again at the end.

### GetBoundingRectangle

```vb
Function GetBoundingRectangle() As Rectangle
    If Points.Count = 0 Then Return Rectangle.Empty
    Dim minX As Integer = Points.Min(Function(p) p.X)
    Dim minY As Integer = Points.Min(Function(p) p.Y)
    Dim maxX As Integer = Points.Max(Function(p) p.X)
    Dim maxY As Integer = Points.Max(Function(p) p.Y)
    Return New Rectangle(minX, minY, maxX - minX, maxY - minY)
End Function
```
- **GetBoundingRectangle**: This function calculates the smallest rectangle that can contain all the points in the shape by finding the minimum and maximum X and Y coordinates.




![125](https://github.com/user-attachments/assets/f2587956-53ee-43e1-8db8-03c9f194e25b)








### GetScaledPoint

```vb
Private Function GetScaledPoint(originalPoint As Point) As Point
    Return New Point(CInt(originalPoint.X * ScaleFactor), CInt(originalPoint.Y * ScaleFactor))
End Function
```
- **GetScaledPoint**: This function scales a given point by the `ScaleFactor`, allowing for dynamic resizing of the shape based on the current zoom level.

### GetHandleRectangle

```vb
Private Function GetHandleRectangle(scaledPoint As Point) As Rectangle
    Return New Rectangle(scaledPoint.X - ControlHandleSize \ 2,
                         scaledPoint.Y - ControlHandleSize \ 2,
                         ControlHandleSize,
                         ControlHandleSize)
End Function
```
- **GetHandleRectangle**: This function returns a rectangle that defines the area of a control handle around a point, making it easier to detect mouse interactions.

[Index](#index)

---

## UI Theme Management

### ApplyUITheme

```vb
Private Sub ApplyUITheme()
    If DarkMode Then
        ' Set dark mode attributes
        DwmSetWindowAttribute(Handle, 20, 1, Marshal.SizeOf(GetType(Boolean)))
        SetWindowTheme(Me.Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(Me.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        ' Additional theme settings...

        BackgroundColor = BackgroundColorDark
    Else
        ' Set light mode attributes
        DwmSetWindowAttribute(Handle, 20, 0, Marshal.SizeOf(GetType(Boolean)))
        SetWindowTheme(Handle, "Explorer", Nothing)
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
        ' Additional theme settings...

        BackgroundColor = BackgroundColorLight
    End If

    ' Refresh UI components
    MenuStrip1.Renderer = CustomMenuRenderer
    BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)
    ' Other UI updates...
End Sub
```
- **ApplyUITheme**: This method applies the current UI theme (dark or light) to the application. It sets various attributes for controls and updates colors based on the selected theme.

[Index](#index)

---

## Layout Management

### LayoutForm

```vb
Private Sub LayoutForm()
    Dim clientWidth As Integer = ClientSize.Width
    Dim clientHeight As Integer = ClientSize.Height
    ' Calculate positions and sizes for controls...
    CenterDrawingArea()
    ' Update control positions...
End Sub
```
- **LayoutForm**: This method arranges the UI components in the form based on the current size of the form. It ensures that everything is properly positioned and sized, making the application responsive.

### CenterDrawingArea

```vb
Private Sub CenterDrawingArea()
    DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2
    DrawingCenter.X = ClientSize.Width \ 4 - VScrollBar1.Width \ 2
End Sub
```
- **CenterDrawingArea**: This method centers the drawing area within the form, calculating its position based on the sizes of other controls.

[Index](#index)

---



## Further Learning Resources
- [VB.NET Documentation](https://docs.microsoft.com/en-us/dotnet/visual-basic/)

---


I created this app because I was tired of manually typing out shape data for my projects. It felt tedious and inefficient, especially when I was developing my airplane simulator app. I often found myself typing in shape data, running the app, and then going back to adjust the code based on what I saw. It was like drawing in the dark and only turning on the light to see what I had created. This back-and-forth process was frustrating and needed to change.


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



---






















## 🌟 Support the Shape Editor!

We invite you to consider giving the **Shape Editor** repository a star! By doing so, you contribute to its recognition and increase its chances of being included in the prestigious **GitHub Archive**.

### Why Star the Shape Editor?

The **Shape Editor** is not just another repository; it serves as a foundational and robust application primitive for illustrating event-driven interactive graphics in computer-aided design (CAD). Here’s why your support matters:

- **Foundation for Innovation**: The Shape Editor provides a solid base for developers to create new applications, extending its functionality to meet diverse needs in graphics and design.
- **Community Contribution**: By starring this project, you help elevate its visibility within the open-source community, encouraging more developers to explore and enhance it.
- **Preservation in the GitHub Archive**: Your star can help ensure that this valuable resource is preserved in the GitHub Archive, recognizing the collaborative efforts of developers worldwide.

### How You Can Help

 **Star the Repository**: Click the ⭐️ button at the top right of this page.
 **Spread the Word**: Share this project with your network, encouraging others to explore and star it.

Thank you for your support in making the **Shape Editor** a part of the GitHub Archive!

---









## 🎞️ The Arctic Code Vault

The **Arctic Code Vault** is a groundbreaking initiative by GitHub to preserve open-source software for future generations. On February 2, 2020, GitHub captured a snapshot of every active public repository and stored it in the Arctic World Archive (AWA), located in Svalbard, Norway. This unique archive is designed for long-term preservation, ensuring that the collective knowledge of developers around the world remains accessible for centuries to come.


![002](https://github.com/user-attachments/assets/12f7a8bd-7d8d-4c97-b4ea-ab12cb11c2e0)

### Why the Arctic Code Vault Matters

- **Cultural Heritage**: The Arctic Code Vault serves as a time capsule for the open-source community, safeguarding the contributions of millions of developers and the evolution of software.
- **Global Collaboration**: By archiving repositories, GitHub highlights the collaborative spirit of open source, showcasing the collective efforts that drive innovation and creativity in software development.
- **Future Accessibility**: The AWA is engineered to withstand the test of time, ensuring that the code and projects stored within can be accessed by future generations, even in the face of technological changes.

### How You Can Contribute

By starring the **Shape Editor** repository, you help enhance its visibility and increase its chances of being included in the Arctic Code Vault in future snapshots. Your support can make a significant difference in preserving this project as part of the global software heritage.

Learn more about the Arctic Code Vault and its mission by visiting:

- [GitHub Archive Program](https://archiveprogram.github.com/)
- [GitHub Arctic Code Vault](https://archiveprogram.github.com/arctic-vault/)
- [YouTube - GitHub Arctic Code Vault](https://www.youtube.com/watch?v=fzI9FNjXQ0o)





















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



 [Imports and Class Declaration](#imports-and-class-declaration)
 
 [Enumerations](#enumerations)
 
 [DLL Imports](#dll-imports)
 
 [Class Variables](#class-variables)
 
 [Form Load Event](#form-load-event)
 
 [Paint Event](#paint-event)
 
 [Mouse Events](#mouse-events)
 
 [Key Events](#key-events)
 
 [Drawing Methods](#drawing-methods)
 
 [Shape Manipulation Methods](#shape-manipulation-methods)
 
 [File Operations](#file-operations)
 
 [Utility Methods](#utility-methods)
 
 [UI Theme Management](#ui-theme-management)
 
 [Layout Management](#layout-management)
   



 **[Creating Transparent Icons in GIMP](#creating-transparent-icons-in-gimp)**
 - Step-by-Step Guide

 **[Adding Icons to a Resource File](#adding-icons-to-a-resource-file)**
 - Open Your Project
 - Accessing the Icon in Code

**[License](#license)**
- License Information




---

## Clones


![124](https://github.com/user-attachments/assets/afcd862b-6a8a-4a54-b226-61a6f80cf785)


![113](https://github.com/user-attachments/assets/fb776e50-686e-470e-ba5c-cf0cb376a0f4)

![090](https://github.com/user-attachments/assets/88254d62-f74a-4d53-951d-b1337cad01b3)

![073](https://github.com/user-attachments/assets/0c224dc3-dd5a-40b8-b161-6829be5c9caf)

![044](https://github.com/user-attachments/assets/4f6a7ce3-7102-407a-9a16-f12672c225d8)








## License
This project is licensed under the MIT License.

![120](https://github.com/user-attachments/assets/63c864ac-ed23-4199-9a86-76a1235d2238)



![038](https://github.com/user-attachments/assets/9ccae402-4d60-4032-9696-6770b2872a27)







