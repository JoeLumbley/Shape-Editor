' Shape Editor

' Create shapes interactively and generate code automatically.

' MIT License
' Copyright(c) 2025 Joseph W. Lumbley

' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:

' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO AND THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE A NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.

Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1

    Enum Tool
        Add
        Subtract
        Move
    End Enum

    Private CurrentTool As Tool = Tool.Add

    Public Enum DwmWindowAttribute
        DWMWA_USE_IMMERSIVE_DARK_MODE = 21
        DWMWA_MICA_EFFECT = 1029
    End Enum

    <DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Shared Function DwmSetWindowAttribute(hWnd As IntPtr, dwAttribute As DwmWindowAttribute, ByRef pvAttribute As Integer, cbAttribute As Integer) As Integer
    End Function

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
    End Function

    Private Points As New List(Of Point)()
    Private LeftMouseButtonDown As Boolean = False
    Private SelectedPointIndex As Integer = -1
    Private HoveredPointIndex As Integer = -1
    Private Const ControlHandleSize As Integer = 15
    Private ScaleFactor As Double = 1.0

    Private ShapePen As New Pen(Color.Black, 2)

    Private DrawingCenter As Point
    Private AdjustedMouseLocation As Point
    Private HandleBrush As New SolidBrush(Color.FromArgb(255, Color.DarkGray))
    Private HoverBrush As New SolidBrush(Color.FromArgb(255, Color.Gray))

    Private GridColorDark As Color = Color.FromArgb(255, 35, 35, 35)
    Private GridColorLight As Color = Color.FromArgb(255, 240, 240, 240)

    Public ControlColorDark As Color = Color.FromArgb(255, 23, 23, 23)
    Public ControlColorLight As Color = Color.FromArgb(255, 240, 240, 240)

    Private GridPenDark As New Pen(GridColorDark, 1)
    Private GridPenLight As New Pen(GridColorLight, 1)

    Private CoordinateSystemPenDarkMode As New Pen(Color.FromArgb(255, 64, 64, 64), 1)
    Private CoordinateSystemPenLightMode As New Pen(Color.FromArgb(255, 200, 200, 200), 1)

    ' Set the fill color for the shape in light and dark modes
    Private ShapeFillColorLightMode As Color = Color.FromArgb(98, 30, 144, 255)
    Private ShapeFillColorDarkMode As Color = Color.FromArgb(24, Color.DodgerBlue)
    Private ShapeFillBrushLightMode As New SolidBrush(ShapeFillColorLightMode)
    Private ShapeFillBrushDarkMode As New SolidBrush(ShapeFillColorDarkMode)
    Private ShapeFillBrush As New SolidBrush(ShapeFillColorLightMode)

    ' Light mode colors
    Private MenuItemBackgroundColor_LightMode As Color = Color.FromArgb(255, 240, 240, 240)
    Private MenuItemBackgroundSelected_LightMode As Color = Color.FromArgb(16, Color.DodgerBlue)
    Private ToolStripBackground_LightMode As Color = Color.FromArgb(255, 240, 240, 240)
    Private MenuItemBorderColor_LightMode As Color = Color.FromArgb(255, 240, 240, 240)
    Private MenuItemSelectedColor_LightMode As Color = Color.FromArgb(64, Color.Gray)
    Private MenuItemTextColor_LightMode As Color = Color.FromArgb(255, Color.Black)
    Private SelectedBorderColor_LightMode As Color = Color.FromArgb(255, Color.DodgerBlue)

    ' Dark mode colors
    Private MenuItemBackgroundColor_DarkMode As Color = Color.FromArgb(255, 23, 23, 23)
    Private MenuItemBackgroundSelectedColor_DarkMode As Color = Color.FromArgb(255, 50, 50, 50)
    Private ToolStripBackground_DarkMode As Color = Color.FromArgb(255, 23, 23, 23)
    Private MenuItemBorderColor_DarkMode As Color = Color.FromArgb(255, 23, 23, 23)
    Private MenuItemSelectedColor_DarkMode As Color = Color.FromArgb(255, 64, 64, 64)
    Private MenuItemTextColor_DarkMode As Color = Color.FromArgb(255, 255, 255, 255)
    Private MenuItemSelectedBorderColor_DarkMode As Color = Color.FromArgb(255, Color.Gray)

    ' Set menu to Light mode colors.
    Private CustomMenuRenderer As New CustomColorMenuStripRenderer(MenuItemBackgroundColor_LightMode,
                                                                    MenuItemBackgroundSelected_LightMode,
                                                                    ToolStripBackground_LightMode,
                                                                    MenuItemSelectedColor_LightMode,
                                                                    MenuItemTextColor_LightMode,
                                                                    SelectedBorderColor_LightMode)

    Private OsVersion As Version = Environment.OSVersion.Version

    Public DarkMode As Boolean = False

    Private FillShape As Boolean = False

    Private HideControlHandles As Boolean = False

    Private Const ThresholdDistance As Double = 10.0 ' Distance threshold for point selection

    Private DrawingArea As Rectangle

    Private MoveStartLocation As Point
    Private MovingShape As Boolean = False

    Private IsInsideBoundingRectangle As Boolean = False

    Private BoundingColorLightMode As Color = Color.FromArgb(16, 30, 144, 255)
    Private BoundingColorDarkMode As Color = Color.FromArgb(75, Color.DodgerBlue)
    Private BoundingBrushLightMode As New SolidBrush(BoundingColorLightMode)
    Private BoundingBrushDarkMode As New SolidBrush(BoundingColorDarkMode)

    Private BoundingBrush As New SolidBrush(BoundingColorLightMode)

    Private LinkColorDark As Color = Color.FromArgb(255, 28, 138, 224)
    Private LinkHoverColorDark As Color = Color.FromArgb(255, 255, 255, 255)
    Private ActiveLinkColorDark As Color = Color.Purple

    Private LinkColorLight As Color = Color.FromArgb(255, 28, 138, 224)
    Private LinkHoverColorLight As Color = Color.FromArgb(255, 0, 0, 0)
    Private ActiveLinkColorLight As Color = Color.Purple

    Private ControlDDown As Boolean = False

    Private ControlFDown As Boolean = False

    Private ControlHDown As Boolean = False

    Private BackgroundColorDark As Color = Color.Black

    Private BackgroundColorLight As Color = Color.White

    Private BackgroundColor As Color = BackgroundColorLight


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeApplication()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Translate the origin to the center of the drawing area
        e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)

        e.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.Bilinear
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.None
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None

        e.Graphics.Clear(BackgroundColor)

        DrawGrid(e)

        DrawCoordinateAxes(e)

        DrawCenterMark(e)

        DrawBoundingRectangle(e)

        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

        DrawShape(e)

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None

        DrawPointHandles(e)

    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown

        ' Check if the left mouse button is pressed
        If e.Button = MouseButtons.Left Then

            ' Calculate the adjusted mouse location based on the scale factor and the drawing center
            AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor),
                                              CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

            SelectedPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)

            If CurrentTool = Tool.Add Then

                ' If no point was selected, add a new point
                If SelectedPointIndex = -1 Then

                    AddPoint(AdjustedMouseLocation)

                Else

                    InsertNewPoint(SelectedPointIndex)

                End If

            ElseIf CurrentTool = Tool.Move Then

                ' Define the shapes bounding rectangle.
                Dim BoundingRect As Rectangle = GetBoundingRectangle()

                If SelectedPointIndex <> -1 Then

                    ' Move a specific point
                    MovePoint(AdjustedMouseLocation)

                    ' If the inside of the shape was selected, start moving it
                ElseIf BoundingRect.Contains(AdjustedMouseLocation) Then

                    ' Store initial position for moving the entire shape
                    MoveStartLocation = AdjustedMouseLocation

                    MovingShape = True

                End If

            ElseIf CurrentTool = Tool.Subtract Then

                ' If a point was selected, remove it
                If SelectedPointIndex <> -1 Then

                    RemovePoint(SelectedPointIndex)

                End If

            End If

            LeftMouseButtonDown = True

            GeneratePointArrayText()

            Invalidate(DrawingArea)

            InvalidateToolButtons()

        End If

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove

        If ActiveControl IsNot Nothing Then

            ActiveControl = Nothing

        End If

        ' Calculate the adjusted mouse location based on the scale factor
        AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor),
                                          CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

        ' Check if the left mouse button is pressed and a point is selected
        If LeftMouseButtonDown AndAlso SelectedPointIndex <> -1 Then
            'Yes, we are moving a point

            ' Adjust the selected point's location based on the mouse movement
            MovePoint(AdjustedMouseLocation)

            Invalidate(DrawingArea)

            InvalidateToolButtons()

            GeneratePointArrayText()

        End If

        ' Update hovered point index
        ' Get the new hovered point index based on the adjusted mouse location
        Dim newHoveredPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)

        ' If the new hovered point index is different from the current hovered point index then
        If newHoveredPointIndex <> HoveredPointIndex Then

            ' Set the new hovered point index
            HoveredPointIndex = newHoveredPointIndex

            ' Invalidate the form to trigger a repaint
            Invalidate(DrawingArea)

            InvalidateToolButtons()

        End If

        If MovingShape And LeftMouseButtonDown Then

            Dim offsetX As Integer = (AdjustedMouseLocation.X - MoveStartLocation.X)
            Dim offsetY As Integer = (AdjustedMouseLocation.Y - MoveStartLocation.Y)

            ' Shift all points by the movement offset
            For i = 0 To Points.Count - 1
                Points(i) = New Point(Points(i).X + offsetX, Points(i).Y + offsetY)
            Next

            MoveStartLocation = AdjustedMouseLocation ' Update tracking position

            Invalidate(DrawingArea)

            InvalidateToolButtons()

            GeneratePointArrayText()

        End If

        ' Define the shapes bounding rectangle.
        Dim BoundingRect As Rectangle = GetBoundingRectangle()

        If BoundingRect.Contains(AdjustedMouseLocation) Then

            If Not IsInsideBoundingRectangle Then

                IsInsideBoundingRectangle = True

                Invalidate(DrawingArea)

                InvalidateToolButtons()

            End If

        ElseIf IsInsideBoundingRectangle Then

            IsInsideBoundingRectangle = False

            Invalidate(DrawingArea)

            InvalidateToolButtons()

        End If

    End Sub

    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp

        If e.Button = MouseButtons.Left Then

            LeftMouseButtonDown = False

            SelectedPointIndex = -1

            GeneratePointArrayText()

            MovingShape = False

        End If

    End Sub

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

        UpdateUIScaleFactor()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        ' Check if Control + C is pressed
        If e.Control AndAlso e.KeyCode = Keys.C Then

            ' Check if TextBox1.Text is not empty
            If Not String.IsNullOrEmpty(TextBox1.Text) Then

                ' Copy the text from TextBox1 to the clipboard
                Clipboard.SetText(TextBox1.Text)

            End If

            e.Handled = True

        ElseIf e.Control AndAlso e.KeyCode = Keys.D Then

            If Not ControlDDown Then

                ControlDDown = True

                If DarkMode Then

                    DarkMode = False

                    DarkModeToolStripMenuItem.Text = "Dark Mode"

                Else

                    DarkMode = True

                    DarkModeToolStripMenuItem.Text = "Light Mode"

                End If

                ApplyUITheme()

                UpdateTitleBarTheme()

                Refresh()

            End If

            e.Handled = True

        ElseIf e.Control AndAlso e.KeyCode = Keys.F Then

            If Not ControlFDown Then

                ControlFDown = True

                ToggleShapeFill()

                Invalidate(DrawingArea)

                InvalidateToolButtons()

            End If

            e.Handled = True

        ElseIf e.Control AndAlso e.KeyCode = Keys.H Then

            If Not ControlHDown Then

                ControlHDown = True

                ToggleHandlesVisibility()

                Invalidate(DrawingArea)

                InvalidateToolButtons()

            End If

            e.Handled = True

        ElseIf e.KeyCode = Keys.Delete AndAlso SelectedPointIndex <> -1 Then

            RemovePoint(SelectedPointIndex)

            Invalidate(DrawingArea)

            InvalidateToolButtons()

            e.Handled = True

        ElseIf e.KeyCode = Keys.OemMinus AndAlso SelectedPointIndex <> -1 Then

            RemovePoint(SelectedPointIndex)

            Invalidate(DrawingArea)

            InvalidateToolButtons()

            e.Handled = True

        ElseIf e.KeyCode = Keys.Subtract AndAlso SelectedPointIndex <> -1 Then

            RemovePoint(SelectedPointIndex)

            Invalidate(DrawingArea)

            InvalidateToolButtons()

            e.Handled = True

        ElseIf e.KeyCode = Keys.N AndAlso SelectedPointIndex <> -1 Then

            InsertNewPoint(SelectedPointIndex)

            Invalidate(DrawingArea)

            e.Handled = True

        ElseIf e.KeyCode = Keys.Oemplus AndAlso SelectedPointIndex <> -1 Then

            InsertNewPoint(SelectedPointIndex)

            Invalidate(DrawingArea)

            e.Handled = True

        ElseIf e.KeyCode = Keys.Add AndAlso SelectedPointIndex <> -1 Then

            InsertNewPoint(SelectedPointIndex)

            Invalidate(DrawingArea)

            e.Handled = True

        End If

    End Sub

    Private Sub ToggleShapeFill()

        If FillShape Then

            FillShape = False

            If DarkMode Then

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOnDark)

            Else

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOnLight)

            End If

            FillShapeToolStripMenuItem.Text = "Fill Shape"

        Else

            FillShape = True

            If DarkMode Then

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOffDark)

            Else

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOffLight)

            End If

            FillShapeToolStripMenuItem.Text = "No Fill"

        End If

    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp

        If e.Control AndAlso e.KeyCode = Keys.D Then

            ControlDDown = False

        ElseIf e.Control AndAlso e.KeyCode = Keys.F Then

            ControlFDown = False

        ElseIf e.Control AndAlso e.KeyCode = Keys.H Then

            ControlHDown = False

        End If

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        ' Intercept the Arrow keys

        ' Check if the active control is not a TextBox or other input control
        If ActiveControl Is Nothing Then

            Select Case keyData
                Case Keys.Up
                    ' Handle Up Arrow Key

                    ' Check if the scroll value exceeds the minimum limit
                    If VScrollBar1.Value - 10 < VScrollBar1.Minimum Then
                        VScrollBar1.Value = VScrollBar1.Minimum
                    Else
                        VScrollBar1.Value -= 10
                    End If

                    ' Update the drawing center based on the scroll value
                    UpdateDrawingCenterY()

                    Invalidate(DrawingArea)

                    InvalidateToolButtons()

                    Return True

                Case Keys.Down
                    ' Handle Down Arrow Key
                    If VScrollBar1.Value + 10 > VScrollBar1.Maximum Then
                        VScrollBar1.Value = VScrollBar1.Maximum
                    Else
                        VScrollBar1.Value += 10
                    End If

                    ' Update the drawing center based on the scroll value
                    UpdateDrawingCenterY()

                    Invalidate(DrawingArea)

                    InvalidateToolButtons()

                    Return True

                Case Keys.Left
                    ' Handle Left Arrow Key

                    ' Check if the scroll value exceeds the minimum limit
                    If HScrollBar1.Value - 10 < HScrollBar1.Minimum Then
                        HScrollBar1.Value = HScrollBar1.Minimum
                    Else
                        HScrollBar1.Value -= 10
                    End If

                    ' Update the drawing center based on the scroll value
                    UpdateDrawingCenterX()

                    Invalidate(DrawingArea)

                    InvalidateToolButtons()

                    Return True

                Case Keys.Right
                    ' Handle Right Arrow Key

                    ' Check if the scroll value exceeds the maximum limit
                    If HScrollBar1.Value + 10 > HScrollBar1.Maximum Then
                        HScrollBar1.Value = HScrollBar1.Maximum
                    Else
                        HScrollBar1.Value += 10
                    End If

                    ' Update the drawing center based on the scroll value
                    UpdateDrawingCenterX()

                    Invalidate(DrawingArea)

                    InvalidateToolButtons()

                    Return True

            End Select

        End If





        ' Call the base class implementation for other keys
        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll

        ScaleFactor = TrackBar1.Value / 100.0

        UpdateUIScaleFactor()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll

        UpdateDrawingCenterX()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub VScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles VScrollBar1.Scroll

        UpdateDrawingCenterY()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click

        NewShape()

    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click

        SaveShapeToFile()

    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click

        OpenShapeFile()

    End Sub

    Private Sub MovePointToolButton_Click(sender As Object, e As EventArgs) Handles MovePointToolButton.Click

        CurrentTool = Tool.Move

        UpdateToolIcons()

    End Sub

    Private Sub AddPointToolButton_Click(sender As Object, e As EventArgs) Handles AddPointToolButton.Click

        CurrentTool = Tool.Add

        UpdateToolIcons()

    End Sub

    Private Sub SubtractPointToolButton_Click(sender As Object, e As EventArgs) Handles SubtractPointToolButton.Click

        CurrentTool = Tool.Subtract

        UpdateToolIcons()

    End Sub

    Private Sub DarkModeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DarkModeToolStripMenuItem.Click

        If DarkMode Then

            DarkMode = False

            DarkModeToolStripMenuItem.Text = "Dark Mode"

        Else

            DarkMode = True

            DarkModeToolStripMenuItem.Text = "Light Mode"

        End If

        ApplyUITheme()

        UpdateTitleBarTheme()

        Refresh()

    End Sub

    Private Sub UpdateTitleBarTheme()
        ' Title bar theme update workaround

        ' This is a workaround for the issue where the title bar does not update
        ' correctly when switching between light and dark mode in Windows 10.
        ' This is a known issue in Windows 10, and this workaround forces the
        ' title bar to update. The workaround is only needed for Windows 10
        ' Windows 11 handles the theme change correctly.

        ' Check if the OS is Windows 10
        If OsVersion.Major = 10 And OsVersion.Minor = 0 And OsVersion.Build < 22000 Then
            ' The first public build of Windows 11 had the build number 10.0.22000
            ' So, we can assume that any build number less than 22000 is Windows 10.

            ' Create a new instance of the ApplyingThemeForm
            Dim ThemeForm As New ApplyingThemeForm()

            ' Force a redraw of form1 by showing applying theme form.
            ThemeForm.ShowDialog()

            ' Tested on 10.0.19045.5737 - Windows 10 Home Version 22H2

        End If

        ' For Windows 11, the title bar should update automatically, so no action is needed.

    End Sub

    Private Sub FillShapeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FillShapeToolStripMenuItem.Click

        ToggleShapeFill()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub HideHandlesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideHandlesToolStripMenuItem.Click

        ToggleHandlesVisibility()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub CenterDrawingButton_Click(sender As Object, e As EventArgs) Handles CenterDrawingButton.Click

        CenterDrawingArea()

        ResetScrollBars()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub CopyLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles CopyLabel.LinkClicked

        ' Check if TextBox1.Text is not null or empty
        If Not String.IsNullOrEmpty(TextBox1.Text) Then

            ' Copy the text in TextBox1 to the clipboard
            Clipboard.SetText(TextBox1.Text)

        End If

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Close()

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click

        AboutForm.ShowDialog()

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        LayoutForm()

        Invalidate(DrawingArea)

        InvalidateToolButtons()

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        Dim result As DialogResult = MessageForm.Show("Are you sure you want to exit?", "Exit - Shape Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Cancel closing if user selects "No" or X's out of the form
        If result = DialogResult.No OrElse result = DialogResult.Cancel Then

            e.Cancel = True

        End If

    End Sub

    Private Sub DrawGrid(e As PaintEventArgs)

        ' Start at the origin (0, 0) and draw the grid lines in both directions at intervals of 20 units multiplied by the scale factor.
        Dim stepSize As Integer = CInt(20 * ScaleFactor)

        Dim gridPen As Pen = If(DarkMode, GridPenDark, GridPenLight)

        ' Draw vertical grid lines
        'For i As Integer = -((ClientSize.Width * 8) \ stepSize) To (ClientSize.Width * 8) \ stepSize
        For i As Integer = -(((DrawingArea.Width \ 2) * ScaleFactor) \ stepSize) To ((DrawingArea.Width \ 2) * ScaleFactor) \ stepSize

            Dim x As Integer = i * stepSize

            '                  (x, GridTop,x, GridBottom)
            'e.Graphics.DrawLine(gridPen, x, -ClientSize.Height * 8, x, ClientSize.Height * 8)
            'e.Graphics.DrawLine(gridPen, x, CInt((-ClientSize.Height * ScaleFactor) + 10), x, CInt(ClientSize.Height * ScaleFactor))
            e.Graphics.DrawLine(gridPen, x, -CInt((DrawingArea.Height \ 2) * ScaleFactor), x, CInt((DrawingArea.Height \ 2) * ScaleFactor))


        Next


        ' Draw horizontal grid lines
        'For i As Integer = -((ClientSize.Height * 8) \ stepSize) To (ClientSize.Height * 8) \ stepSize
        For i As Integer = -(((DrawingArea.Height \ 2) * ScaleFactor) \ stepSize) To ((DrawingArea.Height \ 2) * ScaleFactor) \ stepSize

            Dim y As Integer = i * stepSize

            'e.Graphics.DrawLine(gridPen, -ClientSize.Width * 8, y, ClientSize.Width * 8, y)
            e.Graphics.DrawLine(gridPen, -CInt((DrawingArea.Width \ 2) * ScaleFactor), y, CInt((DrawingArea.Width \ 2) * ScaleFactor), y)


        Next

    End Sub


    Private Sub DrawCoordinateAxes(e As PaintEventArgs)
        ' Draw two lines intersecting at the center of the drawing area to represent the coordinate axes.

        ' Draw the X-axis line.
        e.Graphics.DrawLine(If(DarkMode, CoordinateSystemPenDarkMode, CoordinateSystemPenLightMode), -ClientSize.Width * 8, 0, ClientSize.Width * 8, 0)

        ' Draw the Y-axis line.
        e.Graphics.DrawLine(If(DarkMode, CoordinateSystemPenDarkMode, CoordinateSystemPenLightMode), 0, -ClientSize.Height * 8, 0, ClientSize.Height * 8)

    End Sub

    Private Sub DrawCenterMark(e As PaintEventArgs)
        ' Draw a small cross at the center of the drawing area.

        ' Draw the horizontal line
        e.Graphics.DrawLine(If(DarkMode, Pens.White, Pens.Black), -5, 0, 5, 0)

        ' Draw the vertical line
        e.Graphics.DrawLine(If(DarkMode, Pens.White, Pens.Black), 0, -5, 0, 5)

    End Sub

    Private Sub DrawShape(e As PaintEventArgs)
        ' DrawShape

        ' Draw the shape if there are points
        If Points.Count > 1 Then

            Dim orderedPoints = GetOrderedPoints()

            Dim scaledPoints = orderedPoints.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()

            ' Fill the shape if the checkbox is checked
            If FillShape Then

                e.Graphics.FillPolygon(ShapeFillBrush, scaledPoints)

            End If

            e.Graphics.DrawPolygon(ShapePen, scaledPoints)

        End If

    End Sub

    Private Sub DrawPointHandles(e As PaintEventArgs)

        ' Draw the point handles if the handles are not hidden
        If Not HideControlHandles Then

            For i As Integer = 0 To Points.Count - 1 Step 2

                Dim point = Points(i)

                Dim scaledPoint = New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))

                ' Check if the point is selected or hovered
                If i = SelectedPointIndex OrElse i = HoveredPointIndex Then

                    ' Draw the selected or hovered point handle
                    e.Graphics.FillRectangle(HoverBrush, CInt(scaledPoint.X - ControlHandleSize / 2), CInt(scaledPoint.Y - ControlHandleSize / 2), ControlHandleSize, ControlHandleSize)

                Else

                    ' Draw the normal point handle
                    e.Graphics.FillRectangle(HandleBrush, CInt(scaledPoint.X - ControlHandleSize / 2), CInt(scaledPoint.Y - ControlHandleSize / 2), ControlHandleSize, ControlHandleSize)

                End If

            Next

        End If

    End Sub

    Private Sub DrawBoundingRectangle(e As PaintEventArgs)

        ' Draw the bounding rectangle if the mouse is inside it and the current tool is Move
        If IsInsideBoundingRectangle AndAlso CurrentTool = Tool.Move Then

            ' Define the shapes bounding rectangle.
            Dim BoundingRect As Rectangle = GetBoundingRectangle()

            ' Scale the bounding rectangle based on the scale factor
            Dim ScaledBoundingRectangle As Rectangle = BoundingRect

            ScaledBoundingRectangle.X = CInt(BoundingRect.X * ScaleFactor)
            ScaledBoundingRectangle.Y = CInt(BoundingRect.Y * ScaleFactor)
            ScaledBoundingRectangle.Width = CInt(BoundingRect.Width * ScaleFactor)
            ScaledBoundingRectangle.Height = CInt(BoundingRect.Height * ScaleFactor)

            e.Graphics.FillRectangle(BoundingBrush, ScaledBoundingRectangle)

        End If

    End Sub

    Private Sub NewShape()

        Points.Clear()

        TextBox1.Clear()

        CopyLabel.Enabled = False

        CenterDrawingArea()

        ResetScrollBars()

        UpdateToolIcons()

        ScaleFactor = 8

        TrackBar1.Value = CInt(ScaleFactor * 100)

        HideControlHandles = False

        UpdateUIScaleFactor()

        Invalidate(DrawingArea)

        CurrentTool = Tool.Add

        UpdateToolIcons()

        InvalidateToolButtons()

        Text = "Shape Editor - Code with Joe"

    End Sub

    Private Sub SaveShapeToFile()

        Using saveFileDialog As New SaveFileDialog()

            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            saveFileDialog.Title = "Save Shape"
            saveFileDialog.InitialDirectory = Application.StartupPath

            If saveFileDialog.ShowDialog(Me) = DialogResult.OK Then

                Using writer As New StreamWriter(saveFileDialog.FileName)

                    ' Write the CSV headers (optional).
                    writer.WriteLine("X,Y")

                    For Each point As Point In Points
                        writer.WriteLine($"{point.X},{point.Y}")
                    Next

                End Using

                ' Add file name to "Shape Editor - Code with Joe" and display in titlebar.
                Text = $"{Path.GetFileName(saveFileDialog.FileName)} - Shape Editor - Code with Joe"

            End If

        End Using

    End Sub

    Private Sub OpenShapeFile()

        ' Open a file dialog to select a CSV file
        Using openFileDialog As New OpenFileDialog()

            openFileDialog.AutoUpgradeEnabled = True
            openFileDialog.ShowReadOnly = False
            openFileDialog.ShowHelp = False
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            openFileDialog.Title = "Open Shape"
            openFileDialog.InitialDirectory = Application.StartupPath

            If openFileDialog.ShowDialog() = DialogResult.OK Then

                Points.Clear()

                Dim fileIsValid As Boolean = False

                Try

                    ' Read the file and parse the points
                    Using reader As New StreamReader(openFileDialog.FileName)

                        While Not reader.EndOfStream

                            Dim line As String = reader.ReadLine()
                            Dim parts As String() = line.Split(","c)

                            If parts.Length = 2 Then

                                Dim x As Integer
                                Dim y As Integer

                                If Integer.TryParse(parts(0), x) AndAlso Integer.TryParse(parts(1), y) Then
                                    Points.Add(New Point(x, y))

                                    ' Validate the point
                                    fileIsValid = Integer.TryParse(parts(0), x)
                                    fileIsValid = Integer.TryParse(parts(1), y)

                                End If

                            End If

                        End While

                        ' Add file name to "Shape Editor - Code with Joe" and display in titlebar.
                        Text = $"{Path.GetFileName(openFileDialog.FileName)} - Shape Editor - Code with Joe"

                    End Using

                Catch ex As Exception

                    Select Case True
                        Case TypeOf ex Is IOException
                            MessageForm.Show("This file is in use by another app. Close the file and try again.", "File In Use - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case TypeOf ex Is FileNotFoundException
                            MessageForm.Show("The file was not found. Please check the file path.", "File Not Found - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case TypeOf ex Is FormatException
                            MessageForm.Show("The file format is invalid. Please check the file contents.", "Bad Format - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case TypeOf ex Is ArgumentException
                            MessageForm.Show("The file path is invalid. Please check the file path.", "Bad Path - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case TypeOf ex Is PathTooLongException
                            MessageForm.Show("The file path is too long. Please shorten the file path.", "Path Too Long - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case TypeOf ex Is UnauthorizedAccessException
                            MessageForm.Show("You do not have permission to access this file.", "Unauthorized - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case Else
                            ' Handle other exceptions
                            MessageForm.Show("An unexpected error occurred: " & ex.Message, "Error - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Select

                    fileIsValid = False

                End Try

                If Not fileIsValid Then

                    Text = $"Bad File - Shape Editor - Code with Joe"

                End If

                CurrentTool = Tool.Move

                UpdateToolIcons()

                ScaleFactor = 8

                TrackBar1.Value = CInt(ScaleFactor * 100)

                UpdateUIScaleFactor()

                GeneratePointArrayText()

                Invalidate(DrawingArea)

                InvalidateToolButtons()

            End If

        End Using

    End Sub

    Private Sub AddPoint(location As Point)
        ' Method for adding points and their mirrored counterparts

        Points.Add(location)

        Points.Add(GetMirroredPoint(location))

        SelectedPointIndex = Points.Count - 2

    End Sub

    Private Sub MovePoint(location As Point)
        ' Method for moving points and updating their mirrored counterparts

        Points(SelectedPointIndex) = location

        Points(SelectedPointIndex + 1) = GetMirroredPoint(location)

    End Sub

    Private Function GetMirroredPoint(p As Point) As Point
        ' Method to calculate the mirrored point

        Return New Point(p.X, -p.Y)

    End Function

    Private Sub RemovePoint(index As Integer)

        If index >= 0 AndAlso index < Points.Count - 1 Then

            Points.RemoveAt(index + 1) ' Remove mirrored point
            Points.RemoveAt(index)     ' Remove actual point

        End If

        SelectedPointIndex = -1

        GeneratePointArrayText()

        Invalidate(DrawingArea)

    End Sub

    Private Sub InsertNewPoint(index As Integer)

        Dim newPoint As New Point(Points(index).X, Points(index).Y)

        newPoint.Offset(-1, -1)

        Points.Insert(index + 2, newPoint)

        Points.Insert(index + 3, GetMirroredPoint(newPoint))

        SelectedPointIndex += 2

        GeneratePointArrayText()

        Invalidate(DrawingArea)

    End Sub

    Private Function GetPointIndexAtLocation(location As Point) As Integer

        For i As Integer = 0 To Points.Count - 1 Step 2

            Dim point As Point = Points(i)

            Dim scaledPoint As New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))

            Dim rect As New Rectangle(scaledPoint.X - ControlHandleSize / 2, scaledPoint.Y - ControlHandleSize / 2, ControlHandleSize, ControlHandleSize)

            If rect.Contains(New Point(CInt(location.X * ScaleFactor), CInt(location.Y * ScaleFactor))) Then

                Return i

            End If

        Next

        Return -1

    End Function

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

    Function GetBoundingRectangle() As Rectangle
        ' This function calculates the bounding rectangle of the shape defined by the points in the Points list.

        ' If there are no points, return an empty rectangle.
        If Points.Count = 0 Then Return Rectangle.Empty

        ' Calculate the minimum and maximum X and Y coordinates of the points in the Points list.
        Dim minX As Integer = Points.Min(Function(p) p.X)
        Dim minY As Integer = Points.Min(Function(p) p.Y)
        Dim maxX As Integer = Points.Max(Function(p) p.X)
        Dim maxY As Integer = Points.Max(Function(p) p.Y)

        ' Return a Rectangle object that represents the smallest rectangle that can contain all the points.
        Return New Rectangle(minX, minY, maxX - minX, maxY - minY)

    End Function

    Private Sub CreateShapesFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "Airplane.csv")

        CreateFileFromResource(FilePath, My.Resources.Resource1.Airplane)

        FilePath = Path.Combine(Application.StartupPath, "Alien Ship.csv")

        CreateFileFromResource(FilePath, My.Resources.Resource1.Alien_Ship)

    End Sub

    Private Sub CreateFileFromResource(filepath As String, resource As Byte())

        Try

            If Not IO.File.Exists(filepath) Then

                IO.File.WriteAllBytes(filepath, resource)

            End If

        Catch ex As Exception

            Debug.Print($"Error creating file: {ex.Message}")

        End Try

    End Sub

    Private Function ResourceToImage(resource As Byte()) As Image
        ' Convert the byte array to an Image using a MemoryStream and the
        ' Image.FromStream method to create an Image object from the byte array.
        ' This allows you to use the byte array as an image.

        Using ms As New MemoryStream(resource)

            Return Image.FromStream(ms)

        End Using

    End Function

    Private Sub ToggleHandlesVisibility()
        ' This method toggles the visibility of control handles in the drawing area.

        ' If the control handles are hidden then
        If HideControlHandles Then

            ' Show them
            HideControlHandles = False

            If DarkMode Then

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOnDark)

            Else

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOnLight)

            End If

            HideHandlesToolStripMenuItem.Text = "Hide Handles"

        Else
            ' If the control handles are visible

            ' Hide them
            HideControlHandles = True

            If DarkMode Then

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOffDark)

            Else

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOffLight)

            End If

            HideHandlesToolStripMenuItem.Text = "Show Handles"

        End If

    End Sub

    Private Sub InvalidateToolButtons()

        MovePointToolButton.Invalidate()

        AddPointToolButton.Invalidate()

        SubtractPointToolButton.Invalidate()

        CenterDrawingButton.Invalidate()

    End Sub

    Private Sub UpdateDrawingCenterY()

        ' Update the drawing center Y coordinate based on the vertical scroll bar value
        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2 - VScrollBar1.Value

    End Sub

    Private Sub UpdateDrawingCenterX()

        ' Update the drawing center X coordinate based on the horizontal scroll bar value
        DrawingCenter.X = (ClientSize.Width \ 4) - (VScrollBar1.Width \ 2) - HScrollBar1.Value

    End Sub

    Private Sub CenterDrawingArea()

        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2

        DrawingCenter.X = ClientSize.Width \ 4 - VScrollBar1.Width \ 2

    End Sub

    Private Sub InitializeApplication()

        KeyPreview = True

        DoubleBuffered = True

        Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled

        Application.EnableVisualStyles()

        ApplyUITheme()

        NewShape()

        CreateShapesFiles()

        MenuStrip1.RenderMode = ToolStripRenderMode.Professional

        ' Inject our custom rendering into the MenuStrip
        MenuStrip1.Renderer = CustomMenuRenderer

        LayoutForm()

        MenuStrip1.Refresh()

    End Sub

    Private Sub ResetScrollBars()

        HScrollBar1.Value = 0
        VScrollBar1.Value = 0

    End Sub

    Private Sub UpdateToolIcons()

        If CurrentTool = Tool.Move Then

            If DarkMode Then
                ' Dark mode

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonDarkMode)

                ' Selected
                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkModeSelected)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolDark)

            Else
                ' Light mode

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonLightMode)

                ' Selected
                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonSelectedLightMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolLight)

            End If

        End If

        If CurrentTool = Tool.Add Then

            If DarkMode Then
                ' Dark mode
                ' Selected
                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelectedDarkMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolDark)

            Else
                ' Light mode
                ' Selected
                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelectedLightMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonLightMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolLight)

            End If

        End If

        If CurrentTool = Tool.Subtract Then

            If DarkMode Then
                ' Dark mode
                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonDarkMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkMode)
                ' Selected
                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolSelectedDark)

            Else
                ' Light mode
                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonLightMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonLightMode)
                ' Selected
                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolSelectedLight)

            End If

        End If

    End Sub

    Private Sub ApplyUITheme()

        If DarkMode Then

            ' set title color - dark mode
            DwmSetWindowAttribute(Handle, 20, 1, Marshal.SizeOf(GetType(Boolean)))

            ' Set the theme to dark mode
            SetWindowTheme(Me.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(Me.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Me.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(HScrollBar1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(VScrollBar1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(CenterDrawingButton.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(CenterDrawingButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(CenterDrawingButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(AddPointToolButton.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(AddPointToolButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(AddPointToolButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(MovePointToolButton.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(MovePointToolButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(MovePointToolButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(SubtractPointToolButton.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(SubtractPointToolButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(SubtractPointToolButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(GroupBox1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(TextBox1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            ' Set the menu colors for dark mode
            CustomMenuRenderer.MenuItemBackground = MenuItemBackgroundColor_DarkMode
            CustomMenuRenderer.MenuItemBackgroundSelected = MenuItemBackgroundSelectedColor_DarkMode
            CustomMenuRenderer.ToolStripBackground = ToolStripBackground_DarkMode ' *****************
            CustomMenuRenderer.MenuItemSelectedColor = MenuItemSelectedColor_DarkMode
            CustomMenuRenderer.TextColor = MenuItemTextColor_DarkMode
            CustomMenuRenderer.SelectedBorderColor = MenuItemSelectedBorderColor_DarkMode

            SaveToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.SaveFileAsDark)
            OpenToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.OpenFileDark)
            NewToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.NewFileDark)
            AboutToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.AboutDark)
            ExitToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.ExitDark)

            If CurrentTool = Tool.Add Then

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelectedDarkMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolDark)

            ElseIf CurrentTool = Tool.Move Then

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonDarkMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkModeSelected)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolDark)

            ElseIf CurrentTool = Tool.Subtract Then

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonDarkMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolSelectedDark)

            End If

            CenterDrawingButton.Image = ResourceToImage(My.Resources.Resource1.CenterDrawingToolButtonDarkMode)

            If FillShape Then

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOffDark)

            Else

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOnDark)

            End If

            If HideControlHandles Then

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOffDark)

            Else

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOnDark)

            End If

            DarkModeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.DarkModeOff)

            CopyLabel.LinkColor = LinkColorDark
            CopyLabel.ActiveLinkColor = ActiveLinkColorDark

            BackgroundColor = BackgroundColorDark

        Else

            'set title color - light mode
            DwmSetWindowAttribute(Handle, 20, 0, Marshal.SizeOf(GetType(Boolean)))

            ' Set the theme to light mode
            SetWindowTheme(Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(HScrollBar1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(VScrollBar1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(CenterDrawingButton.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(CenterDrawingButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(CenterDrawingButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(AddPointToolButton.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(AddPointToolButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(AddPointToolButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(MovePointToolButton.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(MovePointToolButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(MovePointToolButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(SubtractPointToolButton.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(SubtractPointToolButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(SubtractPointToolButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(GroupBox1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(TextBox1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            ' Set the menu colors for light mode
            CustomMenuRenderer.MenuItemBackground = MenuItemBackgroundColor_LightMode
            CustomMenuRenderer.MenuItemBackgroundSelected = MenuItemBackgroundSelected_LightMode
            CustomMenuRenderer.ToolStripBackground = ToolStripBackground_LightMode
            CustomMenuRenderer.MenuItemSelectedColor = MenuItemSelectedColor_LightMode
            CustomMenuRenderer.TextColor = MenuItemTextColor_LightMode
            CustomMenuRenderer.SelectedBorderColor = SelectedBorderColor_LightMode

            SaveToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None
            SaveToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.SaveFileAsLight)
            OpenToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.OpenFileLight)
            OpenToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None
            NewToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.NewFileLight)
            NewToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None
            AboutToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.AboutLight)
            AboutToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None
            ExitToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.ExitLight)

            If CurrentTool = Tool.Add Then

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelectedLightMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonLightMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolLight)

            ElseIf CurrentTool = Tool.Move Then

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonLightMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonSelectedLightMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolLight)

            ElseIf CurrentTool = Tool.Subtract Then

                AddPointToolButton.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonLightMode)

                MovePointToolButton.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonLightMode)

                SubtractPointToolButton.Image = ResourceToImage(My.Resources.Resource1.SubtractPointToolSelectedLight)

            End If

            CenterDrawingButton.Image = ResourceToImage(My.Resources.Resource1.CenterDrawingToolButtonLightMode)

            If FillShape Then

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOffLight)

            Else

                FillShapeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.FillShapeOnLight)

            End If

            If HideControlHandles Then

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOffLight)

            Else

                HideHandlesToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.HideHandlesOnLight)

            End If

            DarkModeToolStripMenuItem.Image = ResourceToImage(My.Resources.Resource1.DarkModeOn)

            CopyLabel.LinkColor = LinkColorDark
            CopyLabel.ActiveLinkColor = ActiveLinkColorDark

            BackgroundColor = BackgroundColorLight

        End If

        MenuStrip1.Renderer = CustomMenuRenderer

        BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)

        TrackBar1.BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)

        TextBox1.BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)

        ShapeFillBrush = If(DarkMode, ShapeFillBrushDarkMode, ShapeFillBrushLightMode)

        BoundingBrush = If(DarkMode, BoundingBrushDarkMode, BoundingBrushLightMode)

        ShapePen = New Pen(If(DarkMode, Color.White, Color.Black), 2)

        HandleBrush = New SolidBrush(Color.FromArgb(255, If(DarkMode, Color.DodgerBlue, Color.DarkGray)))

        HoverBrush = New SolidBrush(Color.FromArgb(255, If(DarkMode, Color.Orchid, Color.DodgerBlue)))

        Label1.BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)

        TextBox1.ForeColor = If(DarkMode, Color.FromArgb(255, 230, 230, 230), Color.FromArgb(255, 32, 32, 32))

        Label1.ForeColor = If(DarkMode, Color.White, Color.Black)

        CenterDrawingButton.ForeColor = If(DarkMode, Color.White, Color.Black)

        GroupBox1.BackColor = If(DarkMode, Color.FromArgb(255, 23, 23, 23), Color.White)

        Panel1.BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)
        LanguageLabel.BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)
        CopyLabel.BackColor = If(DarkMode, ControlColorDark, SystemColors.Control)

        LanguageLabel.ForeColor = If(DarkMode, Color.White, Color.Black)
        CopyLabel.LinkColor = If(DarkMode, Color.White, Color.Black)

    End Sub

    Private Sub LayoutForm()

        ' Calculate common values
        Dim clientWidth As Integer = ClientSize.Width
        Dim clientHeight As Integer = ClientSize.Height
        Dim halfClientWidth As Integer = clientWidth \ 2
        Dim quarterClientWidth As Integer = clientWidth \ 4
        Dim menuStripHeight As Integer = MenuStrip1.Height
        Dim trackBarHeight As Integer = TrackBar1.Height
        Dim hScrollBarHeight As Integer = HScrollBar1.Height
        Dim vScrollBarWidth As Integer = VScrollBar1.Width

        CenterDrawingArea()

        CopyLabel.Top = ClientRectangle.Top + menuStripHeight + 3
        CopyLabel.Left = ClientRectangle.Right - CopyLabel.Width - VScrollBar1.Width

        LanguageLabel.Top = ClientRectangle.Top + menuStripHeight + 3
        LanguageLabel.Left = halfClientWidth + 5
        LanguageLabel.Height = 20

        Panel1.Top = ClientRectangle.Top + menuStripHeight
        Panel1.Left = halfClientWidth
        Panel1.Width = halfClientWidth
        Panel1.Height = menuStripHeight

        ' Update TextBox1
        TextBox1.Top = ClientRectangle.Top + menuStripHeight * 2
        TextBox1.Left = halfClientWidth
        TextBox1.Width = halfClientWidth
        TextBox1.Height = clientHeight - menuStripHeight * 2

        ' Update TrackBar1
        TrackBar1.Top = ClientRectangle.Bottom - trackBarHeight
        TrackBar1.Left = ClientRectangle.Left
        TrackBar1.Width = halfClientWidth

        ' Update Label1
        Label1.Top = TrackBar1.Bottom - Label1.Height - 5
        Label1.Left = ClientRectangle.Left + 5
        Label1.Width = 200
        Label1.Height = 20

        ' Update HScrollBar1
        HScrollBar1.Top = ClientRectangle.Bottom - trackBarHeight - hScrollBarHeight
        HScrollBar1.Left = ClientRectangle.Left
        HScrollBar1.Width = halfClientWidth - vScrollBarWidth
        HScrollBar1.Minimum = -clientWidth * 2
        HScrollBar1.Maximum = clientWidth * 2
        HScrollBar1.Value = 0

        ' Update VScrollBar1
        VScrollBar1.Top = ClientRectangle.Top + menuStripHeight
        VScrollBar1.Left = TextBox1.Left - vScrollBarWidth
        VScrollBar1.Height = clientHeight - trackBarHeight - hScrollBarHeight - menuStripHeight
        VScrollBar1.Minimum = -clientHeight * 4
        VScrollBar1.Maximum = clientHeight * 4
        VScrollBar1.Value = 0

        CenterDrawingButton.Width = vScrollBarWidth + 2
        CenterDrawingButton.Height = hScrollBarHeight + 2

        AddPointToolButton.Width = CenterDrawingButton.Width
        AddPointToolButton.Height = CenterDrawingButton.Height

        MovePointToolButton.Width = CenterDrawingButton.Width
        MovePointToolButton.Height = CenterDrawingButton.Height

        SubtractPointToolButton.Width = CenterDrawingButton.Width
        SubtractPointToolButton.Height = CenterDrawingButton.Height

        GroupBox1.Top = HScrollBar1.Top
        GroupBox1.Left = VScrollBar1.Left - 2
        GroupBox1.Width = vScrollBarWidth + 10
        GroupBox1.Height = hScrollBarHeight + 10

        AddPointToolButton.Top = HScrollBar1.Top - AddPointToolButton.Height
        AddPointToolButton.Left = VScrollBar1.Left - AddPointToolButton.Width

        MovePointToolButton.Top = HScrollBar1.Top - AddPointToolButton.Height - MovePointToolButton.Height
        MovePointToolButton.Left = VScrollBar1.Left - AddPointToolButton.Width

        SubtractPointToolButton.Top = HScrollBar1.Top - AddPointToolButton.Height - MovePointToolButton.Height - SubtractPointToolButton.Height
        SubtractPointToolButton.Left = VScrollBar1.Left - AddPointToolButton.Width

        DrawingArea.X = 0
        DrawingArea.Y = menuStripHeight
        DrawingArea.Width = ClientRectangle.Width \ 2 - VScrollBar1.Width
        DrawingArea.Height = ClientRectangle.Height - menuStripHeight - TrackBar1.Height - HScrollBar1.Height

    End Sub

    Private Sub UpdateUIScaleFactor()

        ResetScrollBars()

        CenterDrawingArea()

        AddPointToolButton.Top = HScrollBar1.Top - AddPointToolButton.Height

        AddPointToolButton.Left = VScrollBar1.Left - AddPointToolButton.Width

        MovePointToolButton.Top = HScrollBar1.Top - AddPointToolButton.Height - MovePointToolButton.Height

        MovePointToolButton.Left = VScrollBar1.Left - AddPointToolButton.Width

        Dim ScaleFactorDiv16 = ScaleFactor / 16

        'HScrollBar1.Minimum = -ClientSize.Width * ScaleFactorDiv16
        HScrollBar1.Minimum = -CInt(((DrawingArea.Width \ 2) * ScaleFactor) - (DrawingArea.Width \ 2))



        'HScrollBar1.Maximum = ClientSize.Width * ScaleFactorDiv16
        HScrollBar1.Maximum = CInt(((DrawingArea.Width \ 2) * ScaleFactor) - (DrawingArea.Width \ 2))



        'VScrollBar1.Minimum = -ClientSize.Height * ScaleFactorDiv16

        'VScrollBar1.Maximum = ClientSize.Height * ScaleFactorDiv16

        'VScrollBar1.Minimum = CInt(((-DrawingArea.Height \ 2) - DrawingArea.Height) * ScaleFactor)
        VScrollBar1.Minimum = -CInt(((DrawingArea.Height \ 2) * ScaleFactor) - (DrawingArea.Height \ 2))


        'VScrollBar1.Maximum = CInt((DrawingArea.Height \ 2) * ScaleFactor)
        VScrollBar1.Maximum = CInt(((DrawingArea.Height \ 2) * ScaleFactor) - (DrawingArea.Height \ 2))


        Label1.Text = $"Scale: {ScaleFactor:N2}"

    End Sub

End Class

Public Class CustomColorMenuStripRenderer
    Inherits ToolStripProfessionalRenderer

    Public MenuItemBackground As Color
    Public MenuItemBackgroundSelected As Color
    Public ToolStripBackground As Color
    Public SelectedBorderColor As Color
    Public MenuItemSelectedColor As Color
    Public MenuItemSelectedGradientBegin As Color
    Public MenuItemSelectedGradientEnd As Color
    Public SeparatorColor As Color
    Public CheckmarkColor As Color
    Public CheckmarkBackColor As Color

    Public TextColor As Color

    Public Sub New(menuItemBackground As Color, menuItemBackgroundSelected As Color, toolStripBackground As Color, menuItemSelectedColor As Color, textColor As Color, selectedBorderColor As Color)
        Me.MenuItemBackground = menuItemBackground
        Me.MenuItemBackgroundSelected = menuItemBackgroundSelected
        Me.ToolStripBackground = toolStripBackground
        Me.MenuItemSelectedColor = menuItemSelectedColor
        Me.TextColor = textColor
        Me.SelectedBorderColor = selectedBorderColor

        MenuItemSelectedGradientBegin = Color.FromArgb(255, 64, 64, 64)
        MenuItemSelectedGradientEnd = Color.FromArgb(255, 64, 64, 64)
        SeparatorColor = Color.FromArgb(255, 50, 50, 50)
        CheckmarkColor = Color.Black
        CheckmarkBackColor = Color.White

    End Sub

    Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
        e.Item.ForeColor = TextColor
        MyBase.OnRenderItemText(e)
    End Sub

    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)

        ' Define the rectangle that represents the size of the menu item
        Dim rect As New Rectangle(Point.Empty, e.Item.Size)

        ' Check if the menu item is selected
        If e.Item.Selected Then

            ' Check if the item is a ToolStripMenuItem
            If TypeOf e.Item Is ToolStripMenuItem Then

                ' Check if the dropdown menu is visible
                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then
                    ' Use selected background color when the dropdown is open
                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        ' e.Graphics.DrawRectangle(New Pen(BorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)
                    End Using
                Else
                    ' Use selected background color and draw a border when dropdown is closed
                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        e.Graphics.DrawRectangle(New Pen(SelectedBorderColor), rect.Left, rect.Top, rect.Right - 1, rect.Bottom - 1)
                    End Using
                End If

            Else ' The item is not a ToolStripMenuItem
                ' Check if dropdown is visible
                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then
                    ' Use selected background color and draw a border when dropdown is open
                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        'e.Graphics.DrawRectangle(New Pen(SelectedBorderColor), rect.Left + 1, rect.Top, rect.Right - 2, rect.Bottom - 1)
                    End Using
                Else
                    ' Use default background color when not selected
                    Using Brush As New SolidBrush(MenuItemBackground)
                        e.Graphics.FillRectangle(Brush, rect)
                    End Using
                End If
            End If

        Else ' The item is not selected
            ' Check if the item is a ToolStripMenuItem
            If TypeOf e.Item Is ToolStripMenuItem Then

                ' Check if dropdown is visible
                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then
                    ' Use selected background color when dropdown is open
                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        ' e.Graphics.DrawRectangle(New Pen(BorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)
                    End Using
                Else
                    ' Use default background color when not selected
                    Using Brush As New SolidBrush(MenuItemBackground)
                        e.Graphics.FillRectangle(Brush, rect)
                    End Using
                End If

            Else ' The item is not a ToolStripMenuItem
                ' Check if dropdown is visible
                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then
                    ' Use selected background color when dropdown is open
                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        ' e.Graphics.DrawRectangle(New Pen(SelectedBorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)
                    End Using
                Else
                    ' Use default background color when not selected
                    Using Brush As New SolidBrush(MenuItemBackground)
                        e.Graphics.FillRectangle(Brush, rect)
                    End Using
                End If
            End If
        End If
    End Sub

    ' Render the overall background
    Protected Overrides Sub OnRenderToolStripBackground(e As ToolStripRenderEventArgs)
        Using Brush As New SolidBrush(ToolStripBackground)
            e.Graphics.FillRectangle(Brush, e.AffectedBounds)
        End Using
    End Sub

    ' Render the border
    Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
        ' No border rendering needed.
    End Sub

    Protected Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
        Dim rect As Rectangle = e.Item.ContentRectangle
        Dim pen As New Pen(SeparatorColor)
        e.Graphics.DrawLine(pen, rect.Left, rect.Height \ 2, rect.Right, rect.Height \ 2)
        pen.Dispose()
    End Sub

End Class


