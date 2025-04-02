# Shape Editor


The Shape Editor is an innovative tool designed for the interactive creation of shapes. It seamlessly generates a corresponding array of points in code that represent the drawn shape, making it an essential resource for developers looking to integrate shape data into their projects.


![002](https://github.com/user-attachments/assets/9cbcd506-613c-4f06-8f22-a444ad4b5ed3)




## Features

- **Code Generation**: Automatically generates an array of points for the drawn shape, simplifying integration into your codebase.
- **Copy and Paste Functionality**: Easily copy the generated code from the code view and paste it into your preferred code editor for further development.
- **Immediate Feedback**: Quickly test and adjust shapes as you create them, enhancing your workflow.
- **User-Friendly Interface**: Simplifies the process of integrating shape data, making it accessible for users of all skill levels.
- **Interactive Development**: Encourages hands-on shape creation and coding, fostering creativity and experimentation.

## Point Mirroring



![004](https://github.com/user-attachments/assets/e328602a-22af-4f5b-aace-ca03877acdd6)




### How It Works
- **Symmetrical Design**: Draw a shape on one side of the horizontal axis, and the Shape Editor will automatically generate the mirrored counterpart on the opposite side.
- **Real-Time Updates**: Adjust points or modify the shape on one side, and the mirrored shape updates instantly, providing immediate visual feedback.

### Benefits
- **Efficiency**: Saves time in designing symmetrical shapes, making your workflow faster.
- **Creativity**: Facilitates experimentation with shapes, allowing for complex designs without manual adjustments.
- **Precision**: Ensures both sides of the shape are aligned and proportional, enhancing design quality.
- **User-Friendly**: Accessible for beginners while still being valuable for experienced developers.

## Dark Mode



![001](https://github.com/user-attachments/assets/268cc0d3-bc66-414b-8008-7aaed2167266)




- **Visual Comfort**: Reduces eye strain in low-light environments with a darker interface.
- **Aesthetic Appeal**: Offers a sleek, modern design that many users prefer.
- **Energy Efficiency**: Saves battery life on OLED screens by using less power for darker pixels.
- **User Preference**: Easily toggle between Light and Dark Modes to suit your environment or personal taste.

## Comma-Separated Values (CSV) Files

### File Format
CSV files are plain text files where data is organized in a tabular format, with each value separated by a comma. For the Shape Editor, each line represents a point in the shape, formatted as:
```
x,y
```

![028](https://github.com/user-attachments/assets/a048f2c6-1e18-4532-976f-f8447372d141)



### Usage
- **Saving Shapes**: Users can save drawn shapes as CSV files for easy storage and retrieval.
- **Loading Shapes**: The application can read CSV files to reconstruct shapes, facilitating project continuity.
- **Data Integration**: CSV format allows for easy integration with other applications or programming environments.

### Benefits
- **Simplicity**: Easy to read and write, making it user-friendly.
- **Compatibility**: Works with various applications, including spreadsheet software and programming languages.
- **Portability**: Easily shared and transferred between different systems.










## Getting Started
To begin using the Shape Editor, launch the application and start creating shapes. The generated code will be available for you to copy and use in your projects.

## License
This project is licensed under the MIT License.










# Code Walkthrough



## Imports

```vb
Imports System.IO
```
This line imports the `System.IO` namespace, which contains classes for working with input and output, including reading from and writing to files. This is essential for saving and opening shape data.

## Class Declaration

```vb
Public Class Form1
```
Here, we declare a public class named `Form1`. This class represents the main form of our application, where all the controls and functionalities are defined.

## Variable Declarations

```vb
Private points As New List(Of Point)()
Private isDrawing As Boolean = False
Private selectedPointIndex As Integer = -1
Private hoveredPointIndex As Integer = -1
Private Const handleSize As Integer = 15
Private ScaleFactor As Double = 1.0
```
- `points`: A list to store the points that define the shape.
- `isDrawing`: A boolean flag to indicate whether the user is currently drawing a shape.
- `selectedPointIndex`: Stores the index of the currently selected point.
- `hoveredPointIndex`: Stores the index of the point currently hovered over by the mouse.
- `handleSize`: A constant that defines the size of the control handles that appear on the points.
- `ScaleFactor`: A double that represents the scaling factor for the drawing area.

## Graphics Settings

```vb
Private ShapePen As New Pen(Color.Black, 2)
Private ShapeBrush As New SolidBrush(Color.FromArgb(128, Color.Blue))
Private DrawingCenter As Point
Private AdjustedMouseLocation As Point
Private HandleBrush As New SolidBrush(Color.FromArgb(255, Color.DarkGray))
Private HoverBrush As New SolidBrush(Color.FromArgb(255, Color.Gray))
```
- `ShapePen`: A pen used to outline the shapes, initialized with a black color and a width of 2.
- `ShapeBrush`: A brush used to fill shapes, initialized with a semi-transparent blue color.
- `DrawingCenter`: A point that will be used to define the center of the drawing area.
- `AdjustedMouseLocation`: A point that represents the mouse location adjusted for scaling.
- `HandleBrush`: A brush for drawing the control handles in dark gray.
- `HoverBrush`: A brush for highlighting handles when hovered over.

## Color Settings

```vb
Private GridColorDark As Color = Color.FromArgb(255, 64, 64, 64)
Private DarkModeControlColor As Color = Color.FromArgb(255, 32, 32, 32)
Private GridPenDark As New Pen(GridColorDark, 1)
```
- `GridColorDark`: Defines the color of the grid when dark mode is activated.
- `DarkModeControlColor`: A color for controls in dark mode.
- `GridPenDark`: A pen used for drawing the grid lines in dark mode.

## Form Load Event

```vb
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
```
This subroutine handles the `Load` event of the form, which occurs when the form is first displayed.

### Inside Load Event

```vb
Me.DoubleBuffered = True
Me.KeyPreview = True
```
- `DoubleBuffered`: Enables double buffering to reduce flickering during redraws.
- `KeyPreview`: Allows the form to receive key events before they are passed to the controls.

```vb
Text = "Shape Editor - Code with Joe"
```
Sets the title of the form.

```vb
ScaleFactor = TrackBar1.Value / 100.0
Label1.Text = $"Scale Factor: {ScaleFactor:N2}"
```
Initializes the `ScaleFactor` based on the value of a `TrackBar` control and updates a label to display it.

```vb
AddHandler HideControlHandlesCheckBox.CheckedChanged, AddressOf HideControlHandlesCheckBox_CheckedChanged
AddHandler FillShapeCheckBox.CheckedChanged, AddressOf FillShapeCheckBox_CheckedChanged
AddHandler DarkModeCheckBox.CheckedChanged, AddressOf DarkModeCheckBox_CheckedChanged
```
These lines add event handlers for the checkboxes that control various features of the application.

```vb
CenterToScreen()
WindowState = FormWindowState.Maximized
```
Centers the form on the screen and maximizes it upon loading.

## Paint Event

```vb
Protected Overrides Sub OnPaint(e As PaintEventArgs)
```
This subroutine overrides the `OnPaint` method to handle custom drawing on the form.

### Inside OnPaint

```vb
MyBase.OnPaint(e)
```
Calls the base class's `OnPaint` method to ensure any default painting behavior occurs.

```vb
e.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver
e.Graphics.Clear(If(DarkModeCheckBox.Checked, Color.Black, Color.White))
```
Sets the compositing mode and clears the drawing area to either black or white based on whether dark mode is enabled.

```vb
e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)
```
Translates the origin of the graphics to the center of the drawing area.

```vb
DrawGrid(e.Graphics)
```
Calls a method to draw a grid on the form.

### Drawing Axes

```vb
e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.Gray, Pens.Silver), -ClientSize.Width * 8, 0, ClientSize.Width * 8, 0) ' X-axis
e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.Gray, Pens.Silver), 0, -ClientSize.Height * 8, 0, ClientSize.Height * 8) ' Y-axis
```
Draws the X and Y axes in different colors depending on dark mode.

### Drawing Intersecting Lines

```vb
e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), -5, 0, 5, 0) ' Horizontal line
e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), 0, -5, 0, 5) ' Vertical line
```
Draws small intersecting lines at the origin.

### Quality Settings

```vb
e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
```
Sets various graphics quality settings for smoother rendering.

### Drawing Points and Shapes

```vb
If points.Count > 1 Then
    Dim orderedPoints = GetOrderedPoints()
    Dim scaledPoints = orderedPoints.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()

    If FillShapeCheckBox.Checked Then
        e.Graphics.FillPolygon(ShapeBrush, scaledPoints)
    End If

    e.Graphics.DrawPolygon(ShapePen, scaledPoints)
End If
```
If there are more than one point, it retrieves the ordered points, scales them according to the `ScaleFactor`, and draws the shape. If the fill checkbox is checked, it fills the shape as well.

### Drawing Control Handles

```vb
If Not HideControlHandlesCheckBox.Checked Then
    For i As Integer = 0 To points.Count - 1 Step 2
        Dim point = points(i)
        Dim scaledPoint = New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
        If i = selectedPointIndex OrElse i = hoveredPointIndex Then
            e.Graphics.FillRectangle(HoverBrush, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
        Else
            e.Graphics.FillRectangle(HandleBrush, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
        End If
    Next
End If
```
If the checkbox to hide control handles is not checked, it draws rectangles around each point to represent control handles. The color of the handle changes if it's selected or hovered over.

## Mouse Events

### Mouse Down Event

```vb
Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
```
Handles mouse button presses on the form.

### Inside Mouse Down

```vb
If e.Button = MouseButtons.Left Then
    AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor),
                                      CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))
```
Checks if the left mouse button is pressed, then calculates the adjusted mouse location based on the current scale and drawing center.

```vb
selectedPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)
```
Gets the index of the point at the mouse location.

```vb
If selectedPointIndex = -1 Then
    points.Add(AdjustedMouseLocation)
    points.Add(New Point(AdjustedMouseLocation.X, -AdjustedMouseLocation.Y))
    selectedPointIndex = points.Count - 2
End If
```
If no point was selected, it adds a new point and its mirror point, then updates the selected index.

```vb
isDrawing = True
GeneratePointArrayText()
Invalidate()
```
Sets the drawing flag to true, generates the point array text, and invalidates the form to trigger a repaint.

### Mouse Move Event

```vb
Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
```
Handles mouse movement over the form.

### Inside Mouse Move

```vb
AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))
```
Calculates the adjusted mouse location again.

```vb
If isDrawing AndAlso selectedPointIndex <> -1 Then
    points(selectedPointIndex) = AdjustedMouseLocation
    points(selectedPointIndex + 1) = New Point(AdjustedMouseLocation.X, -AdjustedMouseLocation.Y)
    GeneratePointArrayText()
    Invalidate()
End If
```
If the user is drawing and has selected a point, it updates the position of the selected point and its mirror point.

```vb
Dim newHoveredPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)
If newHoveredPointIndex <> hoveredPointIndex Then
    hoveredPointIndex = newHoveredPointIndex
    Invalidate()
End If
```
Checks if the hovered point has changed and updates the `hoveredPointIndex` accordingly.

### Mouse Up Event

```vb
Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
```
Handles when the mouse button is released.

### Inside Mouse Up

```vb
If e.Button = MouseButtons.Left Then
    isDrawing = False
    selectedPointIndex = -1
    GeneratePointArrayText()
End If
```
Resets the drawing flag and selected point index when the left mouse button is released.

## Key Down Event

```vb
Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
```
Handles key presses when the form is active.

### Inside Key Down

```vb
If e.KeyCode = Keys.Enter AndAlso points.Count > 2 Then
    points.Add(points(0)) ' Close the shape
    points.Add(New Point(points(1).X, -points(1).Y)) ' Close the mirror shape
    Invalidate()
    GeneratePointArrayText()
ElseIf e.KeyCode = Keys.Delete AndAlso selectedPointIndex <> -1 Then
    points.RemoveAt(selectedPointIndex)
    points.RemoveAt(selectedPointIndex)
    selectedPointIndex = -1
    GeneratePointArrayText()
    Invalidate()
ElseIf e.KeyCode = Keys.N AndAlso selectedPointIndex <> -1 Then
    Dim newPoint As New Point(points(selectedPointIndex).X + 10, points(selectedPointIndex).Y + 10)
    points.Insert(selectedPointIndex + 2, newPoint)
    points.Insert(selectedPointIndex + 3, New Point(newPoint.X, -newPoint.Y))
    selectedPointIndex += 2
    GeneratePointArrayText()
    Invalidate()
End If
```
- **Enter Key**: Closes the shape by adding the first point again.
- **Delete Key**: Removes the selected point and its mirror.
- **N Key**: Adds a new point offset from the selected point.

## Resize Event

```vb
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
```
Handles resizing of the form.

### Inside Resize

```vb
Dim clientWidth As Integer = ClientSize.Width
Dim clientHeight As Integer = ClientSize.Height
```
Gets the current width and height of the client area.

```vb
CenterDrawingArea()
```
Calls a method to recenter the drawing area.

```vb
' Update various controls' positions and sizes based on the new form size
```
Adjusts the positions and sizes of various controls like `TextBox`, `TrackBar`, and `ScrollBars` based on the new form dimensions.

```vb
Invalidate()
```
Invalidates the form to trigger a repaint.

## TrackBar Scroll Event

```vb
Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
```
Handles scrolling of the `TrackBar` control.

### Inside TrackBar Scroll

```vb
ResetScrollBars()
CenterDrawingArea()
ScaleFactor = TrackBar1.Value / 100.0
Label1.Text = $"Scale Factor: {ScaleFactor:N2}"
```
Resets the scroll bars, recenters the drawing area, updates the scale factor, and displays it.

```vb
If ScaleFactor >= 3 Then
    ' Adjust scroll bar limits based on scale factor
Else
    ' Disable scroll bars if scale factor is less than 3
End If
```
Adjusts the scroll bar limits based on the scale factor.

```vb
GeneratePointArrayText()
Invalidate()
```
Generates the point array text and invalidates the form.

## Scroll Bar Events

### Horizontal Scroll Event

```vb
Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll
```
Handles horizontal scrolling.

```vb
DrawingCenter.X = (ClientSize.Width \ 4) - (VScrollBar1.Width \ 2) - HScrollBar1.Value
Invalidate()
```
Updates the drawing center based on the horizontal scroll value.

### Vertical Scroll Event

```vb
Private Sub VScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles VScrollBar1.Scroll
```
Handles vertical scrolling.

```vb
DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2 - VScrollBar1.Value
Invalidate()
```
Updates the drawing center based on the vertical scroll value.

## Button Click Event

```vb
Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
```
Handles clicks on a button.

```vb
ResetScrollBars()
CenterDrawingArea()
Invalidate()
```
Resets the scroll bars, recenters the drawing area, and invalidates the form.

## Checkbox Events

### Hide Control Handles

```vb
Private Sub HideControlHandlesCheckBox_CheckedChanged(sender As Object, e As EventArgs)
Invalidate()
```
Invalidates the form when the checkbox state changes.

### Fill Shape

```vb
Private Sub FillShapeCheckBox_CheckedChanged(sender As Object, e As EventArgs)
Invalidate()
```
Invalidates the form when the fill shape checkbox state changes.

### Dark Mode Checkbox

```vb
Private Sub DarkModeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DarkModeCheckBox.CheckedChanged
UpdateUIForDarkMode()
Invalidate()
```
Updates the UI colors for dark mode and invalidates the form.

## File Operations

### New File

```vb
Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
points.Clear()
TextBox1.Clear()
Invalidate()
```
Clears the points and the text box for a new shape.

### Save File

```vb
Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
Using saveFileDialog As New SaveFileDialog()
    ' Set up the save file dialog and write points to a file
End Using
```
Opens a save file dialog and writes the points to a file.

### Open File

```vb
Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
Using openFileDialog As New OpenFileDialog()
    ' Set up the open file dialog and read points from a file
End Using
```
Opens a file dialog to read points from a file and load them into the application.

### Exit Application

```vb
Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
Close()
```
Closes the application.

### About Box

```vb
Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
MessageBox.Show("Shape Editor" _
              & vbCrLf _
              & "A simple shape editor that allows you to draw shapes and generate the corresponding point array in VB.NET." _
              & vbCrLf _
              & vbCrLf _
              & "MIT License" _
              & vbCrLf _
              & "Copyright(c) 2025 Joseph W. Lumbley" _
              & vbCrLf _
              & vbCrLf _
              & "https://github.com/JoeLumbley/Shape-Editor",
                "About Shape Editor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
```
Displays an about message box with information about the application.

### Form Closing Event

```vb
Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
If MessageBox.Show("Are you sure you want to exit?", "Exit Shape Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
    e.Cancel = True
End If
```
Prompts the user for confirmation before closing the application.

## Mouse Wheel Event

```vb
Private Sub Form1_MouseWheel(sender As Object, e As MouseEventArgs) Handles MyBase.MouseWheel

CenterDrawingArea()

If e.Delta > 0 Then
    If TrackBar1.Value + 100 <= TrackBar1.Maximum Then
        TrackBar1.Value += 100
    Else
        TrackBar1.Value = TrackBar1.Maximum
    End If
Else
    If TrackBar1.Value - 100 >= TrackBar1.Minimum Then
        TrackBar1.Value -= 100
    Else
        TrackBar1.Value = TrackBar1.Minimum
    End If
End If

ScaleFactor = TrackBar1.Value / 100.0
Label1.Text = $"Scale Factor: {ScaleFactor:N2}"

If ScaleFactor >= 3 Then
    HScrollBar1.Minimum = -ClientSize.Width * (ScaleFactor / 16)
    HScrollBar1.Maximum = ClientSize.Width * (ScaleFactor / 16)
    VScrollBar1.Minimum = -ClientSize.Height * (ScaleFactor / 16)
    VScrollBar1.Maximum = ClientSize.Height * (ScaleFactor / 16)

    If Not HScrollBar1.Enabled Then HScrollBar1.Enabled = True
    If Not VScrollBar1.Enabled Then VScrollBar1.Enabled = True
Else
    If HScrollBar1.Enabled Then HScrollBar1.Enabled = False
    If VScrollBar1.Enabled Then VScrollBar1.Enabled = False
End If

GeneratePointArrayText()
Invalidate()
```
### Explanation of Mouse Wheel Event

This event handles the mouse wheel scrolling action:

- **CenterDrawingArea()**: Repositions the drawing area to the center of the form.
  
- **Scrolling Up (e.Delta > 0)**: If the user scrolls up, it increases the `TrackBar1` value by 100, provided it does not exceed the maximum limit. If it exceeds, it sets it to the maximum value.

- **Scrolling Down (else)**: If the user scrolls down, it decreases the `TrackBar1` value by 100, ensuring it does not fall below the minimum limit.

- **Scale Factor Update**: The `ScaleFactor` is recalculated based on the new `TrackBar1` value, and the label displaying the scale factor is updated.

- **Scroll Bar Adjustments**: If the `ScaleFactor` is 3 or greater, the minimum and maximum values for both horizontal and vertical scroll bars are adjusted. If the scale factor is less than 3, the scroll bars are disabled.

- **Final Steps**: Calls `GeneratePointArrayText()` to update the text box with the current points, and `Invalidate()` to refresh the drawing.

## Center Drawing Area Method

```vb
Private Sub CenterDrawingArea()
    DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2
    DrawingCenter.X = ClientSize.Width \ 4 - VScrollBar1.Width \ 2
End Sub
```
### Explanation of CenterDrawingArea

This method calculates and sets the `DrawingCenter` coordinates:

- **Y Coordinate**: Centers the drawing area vertically by taking the client height, subtracting the heights of the `TrackBar`, `HScrollBar`, and adding the height of the `MenuStrip`. The result is divided by 2 to find the vertical center.

- **X Coordinate**: Centers the drawing area horizontally by taking a quarter of the client width and adjusting it by half the width of the vertical scroll bar.

## Reset Scroll Bars Method

```vb
Private Sub ResetScrollBars()
    HScrollBar1.Value = 0
    VScrollBar1.Value = 0
End Sub
```
### Explanation of ResetScrollBars

This method resets both horizontal and vertical scroll bars to their starting positions (0). This is useful when the drawing area is re-centered or when the scale changes significantly.

## Update UI for Dark Mode Method

```vb
Private Sub UpdateUIForDarkMode()
    ShapeBrush = New SolidBrush(Color.FromArgb(128, If(DarkModeCheckBox.Checked, Color.Silver, Color.DodgerBlue)))
    ShapePen = New Pen(If(DarkModeCheckBox.Checked, Color.White, Color.Black), 2)
    HandleBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.DodgerBlue, Color.DarkGray)))
    HoverBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.Orchid, Color.Gray)))

    TextBox1.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    TextBox1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

    TrackBar1.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    Label1.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    Label1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

    HideControlHandlesCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    HideControlHandlesCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

    FillShapeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
    FillShapeCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

    DarkModeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
    DarkModeCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

    MenuStrip1.BackColor = If(DarkModeCheckBox.Checked, Color.Gray, SystemColors.Control)
    MenuStrip1.ForeColor = If(DarkModeCheckBox.Checked, Color.Black, Color.Black)

    FileToolStripMenuItem.BackColor = If(DarkModeCheckBox.Checked, Color.Gray, SystemColors.Control)
    FileToolStripMenuItem.ForeColor = If(DarkModeCheckBox.Checked, Color.Black, Color.Black)

    OpenToolStripMenuItem.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    OpenToolStripMenuItem.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
    SaveToolStripMenuItem.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    SaveToolStripMenuItem.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
    NewToolStripMenuItem.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    NewToolStripMenuItem.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
    ExitToolStripMenuItem.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    ExitToolStripMenuItem.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
    AboutToolStripMenuItem.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)
    AboutToolStripMenuItem.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
End Sub
```
### Explanation of UpdateUIForDarkMode

This method updates the UI elements when dark mode is toggled:

- **Brush and Pen Colors**: Adjusts the colors of the shape brush, shape pen, handle brush, and hover brush based on whether dark mode is active.

- **Control Background and Foreground Colors**: Updates the background and foreground colors of various controls, including `TextBox`, `Label`, `CheckBoxes`, and the `MenuStrip`, to ensure good visibility in dark mode.

## Draw Grid Method

```vb
Private Sub DrawGrid(g As Graphics)
```
This method draws a grid on the form using the provided graphics object.

### Inside DrawGrid

```vb
Dim stepSize As Integer = CInt(20 * ScaleFactor)
```
Calculates the step size for the grid lines based on the `ScaleFactor`.

```vb
Dim gridPen As Pen = If(DarkModeCheckBox.Checked, GridPenDark, Pens.Gainsboro)
```
Selects the pen color based on whether dark mode is enabled.

### Drawing Vertical and Horizontal Lines

```vb
For i As Integer = -((ClientSize.Width * 8) \ stepSize) To (ClientSize.Width * 8) \ stepSize
    Dim x As Integer = i * stepSize
    g.DrawLine(gridPen, x, -ClientSize.Height * 8, x, ClientSize.Height * 8)
Next

For i As Integer = -((ClientSize.Height * 8) \ stepSize) To (ClientSize.Height * 8) \ stepSize
    Dim y As Integer = i * stepSize
    g.DrawLine(gridPen, -ClientSize.Width * 8, y, ClientSize.Width * 8, y)
Next
```
- **Vertical Lines**: Loops through calculated intervals to draw vertical grid lines across the form.
- **Horizontal Lines**: Similarly, loops to draw horizontal grid lines.

## Get Point Index at Location Method

```vb
Private Function GetPointIndexAtLocation(location As Point) As Integer
```
This function checks if a point exists at a specified location and returns its index.

### Inside GetPointIndexAtLocation

```vb
For i As Integer = 0 To points.Count - 1 Step 2
    Dim point As Point = points(i)
    Dim scaledPoint As New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
    Dim rect As New Rectangle(scaledPoint.X - handleSize / 2, scaledPoint.Y - handleSize / 2, handleSize, handleSize)

    If rect.Contains(New Point(CInt(location.X * ScaleFactor), CInt(location.Y * ScaleFactor))) Then
        Return i
    End If
Next
```
- **Loop Through Points**: Iterates through the points list to find the point at the given location.
- **Rectangle Check**: Creates a rectangle around each point to check if the mouse location falls within it.

```vb
Return -1
```
If no point is found, it returns -1.

## Generate Point Array Text Method

```vb
Private Sub GeneratePointArrayText()
```
This method generates a string representation of the points in an array format and updates the text box.

### Inside GeneratePointArrayText

```vb
Dim sb As New System.Text.StringBuilder()
sb.AppendLine("Dim ScaleFactor As Double = 1.0 ' Adjust the scale factor as needed")
sb.AppendLine("")
sb.AppendLine("Dim Shape As Point() = {")
```
Initializes a `StringBuilder` and starts building the string with a comment and the beginning of a point array declaration.

```vb
Dim orderedPoints = GetOrderedPoints()

For i As Integer = 0 To orderedPoints.Count - 1
    If i < orderedPoints.Count - 1 Then
        sb.AppendLine($"    New Point(CInt({orderedPoints(i).X} * ScaleFactor), CInt({orderedPoints(i).Y} * ScaleFactor)),")
    Else
        sb.AppendLine($"    New Point(CInt({orderedPoints(i).X} * ScaleFactor), CInt({orderedPoints(i).Y} * ScaleFactor))")
    End If
Next
```
- **Get Ordered Points**: Retrieves the ordered points for consistent output.
- **Build Point Array**: Loops through ordered points to add each one to the string, formatting it correctly for VB.NET.

```vb
sb.AppendLine("}")
TextBox1.Text = sb.ToString()
```
Completes the point array declaration and updates the text box with the generated string.

## Get Ordered Points Method

```vb
Private Function GetOrderedPoints() As List(Of Point)
```
This function retrieves the points in a specific order for drawing.

### Inside GetOrderedPoints

```vb
Dim orderedPoints As New List(Of Point)()

For i As Integer = 0 To points.Count - 1 Step 2
    orderedPoints.Add(points(i))
Next

For i As Integer = points.Count - 1 To 1 Step -2
    orderedPoints.Add(points(i))
Next
```
- **First Loop**: Collects the original points.
- **Second Loop**: Collects the mirror points in reverse order.

```vb
If points.Count > 0 Then
    orderedPoints.Add(points(0)) ' Close the shape
End If

Return orderedPoints
```
If there are points, it adds the first point again to close the shape and returns the ordered list.






























