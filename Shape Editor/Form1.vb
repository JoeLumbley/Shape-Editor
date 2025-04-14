' Shape Editor

' A simple shape editor that allows you to draw shapes and generate the corresponding point array in VB.NET code.

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
        dwmwa_invalid = -1
        DWMWA_NCRENDERING_ENABLED = 1
        DWMWA_CAPTION_COLOR = 3
        DWMWA_FLIP3D_POLICY = 8
        DWMWA_EXTENDED_FRAME_BOUNDS = 9
        DWMWA_HAS_ICONIC_BITMAP = 10
        DWMWA_DISALLOW_PEEK = 11
        DWMWA_EXCLUDED_FROM_PEEK = 12
        DWMWA_LAST = 13
        dwmwa_use_dark_theme = 19
        dwmwa_use_light_theme = 20
        DWMWA_USE_IMMERSIVE_DARK_MODE = 21
        DWMWA_MICA_EFFECT = 1029
    End Enum

    <DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Shared Function DwmSetWindowAttribute(hWnd As IntPtr, dwAttribute As DwmWindowAttribute, ByRef pvAttribute As Integer, cbAttribute As Integer) As Integer
    End Function

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Public Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
    End Function

    Private points As New List(Of Point)()
    Private isDrawing As Boolean = False
    Private selectedPointIndex As Integer = -1
    Private hoveredPointIndex As Integer = -1
    Private Const handleSize As Integer = 15
    Private ScaleFactor As Double = 1.0

    Private ShapePen As New Pen(Color.Black, 2)

    Private DrawingCenter As Point
    Private AdjustedMouseLocation As Point
    Private HandleBrush As New SolidBrush(Color.FromArgb(255, Color.DarkGray))
    Private HoverBrush As New SolidBrush(Color.FromArgb(255, Color.Gray))

    Private GridColorDark As Color = Color.FromArgb(255, 42, 42, 42)
    Private GridColorLight As Color = Color.FromArgb(255, 240, 240, 240)

    Private DarkModeControlColor As Color = Color.FromArgb(255, 32, 32, 32)
    '
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
    Private MenuItemBorderColor_LightMode As Color = Color.FromArgb(255, 200, 200, 200)
    Private MenuItemSelectedColor_LightMode As Color = Color.FromArgb(64, Color.Gray)
    Private MenuItemTextColor_LightMode As Color = Color.FromArgb(255, Color.Black)
    Private SelectedBorderColor_LightMode As Color = Color.FromArgb(255, Color.DodgerBlue)

    ' Dark mode colors
    Private MenuItemBackgroundColor_DarkMode As Color = Color.FromArgb(255, 32, 32, 32)
    Private MenuItemBackgroundSelectedColor_DarkMode As Color = Color.FromArgb(255, 64, 64, 64)
    Private ToolStripBackground_DarkMode As Color = Color.FromArgb(255, 32, 32, 32)
    Private MenuItemBorderColor_DarkMode As Color = Color.FromArgb(255, 50, 50, 50)
    Private MenuItemSelectedColor_DarkMode As Color = Color.FromArgb(255, 64, 64, 64)
    Private MenuItemTextColor_DarkMode As Color = Color.FromArgb(255, 255, 255, 255)
    Private MenuItemSelectedBorderColor_DarkMode As Color = Color.FromArgb(255, Color.DodgerBlue)

    ' Set menu to Light mode colors.
    Dim CustomMenuRenderer As New CustomColorMenuStripRenderer(MenuItemBackgroundColor_LightMode,
                                                                    MenuItemBackgroundSelected_LightMode,
                                                                    ToolStripBackground_LightMode,
                                                                    MenuItemBorderColor_LightMode,
                                                                    MenuItemSelectedColor_LightMode,
                                                                    MenuItemTextColor_LightMode,
                                                                    SelectedBorderColor_LightMode)

    <DllImport("dwmapi.dll")>
    Private Shared Function DwmSetWindowAttribute(hWnd As IntPtr,
                                                  Attribute As Integer,
                                                  ByRef Value As Integer,
                                                  SizeOfValue As Integer) As Integer
    End Function

    Private OsVersion As Version = Environment.OSVersion.Version


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        KeyPreview = True

        DoubleBuffered = True

        Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled
        Application.EnableVisualStyles()

        ApplyUITheme()

        Text = "Shape Editor - Code with Joe"

        ScaleFactor = TrackBar1.Value / 100.0
        Label1.Text = $"Scale Factor: {ScaleFactor:N2}"

        CreateShapesFiles()

        ' Set the TextBox1 to read-only to prevent user edits.
        TextBox1.ReadOnly = True

        ' Set the TextBox1 to multiline to allow for multiple lines of text.
        TextBox1.Multiline = True

        ' Set the TextBox1 to auto-scroll to allow scrolling through the text.
        TextBox1.ScrollBars = ScrollBars.Both

        ' Set the TextBox1 to accept only read-only text.
        TextBox1.AcceptsTab = False

        'CenterToScreen()

        '' Maximize the form
        WindowState = FormWindowState.Maximized

        MenuStrip1.RenderMode = ToolStripRenderMode.Professional

        ' Inject our custom rendering into the MenuStrip
        MenuStrip1.Renderer = CustomMenuRenderer

        LayoutForm()

        MenuStrip1.Refresh()

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

        e.Graphics.Clear(If(DarkModeCheckBox.Checked, Color.Black, Color.White))

        DrawGrid(e.Graphics)

        ' Draw the coordinate system
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, CoordinateSystemPenDarkMode, CoordinateSystemPenLightMode), -ClientSize.Width * 8, 0, ClientSize.Width * 8, 0) ' X-axis
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, CoordinateSystemPenDarkMode, CoordinateSystemPenLightMode), 0, -ClientSize.Height * 8, 0, ClientSize.Height * 8) ' Y-axis

        ' Draw intersecting lines at the origin, the center of the drawing area
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), -5, 0, 5, 0) ' Horizontal line
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), 0, -5, 0, 5) ' Vertical line

        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

        ' Draw the shape if there are points
        If points.Count > 1 Then
            Dim orderedPoints = GetOrderedPoints()
            Dim scaledPoints = orderedPoints.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()

            ' Fill the shape if the checkbox is checked
            If FillShapeCheckBox.Checked Then

                e.Graphics.FillPolygon(ShapeFillBrush, scaledPoints)

            End If

            e.Graphics.DrawPolygon(ShapePen, scaledPoints)

        End If

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None

        ' Draw point handles if the checkbox is not checked
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

    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown

        ' Check if the left mouse button is pressed
        If e.Button = MouseButtons.Left Then
            ' Calculate the adjusted mouse location based on the scale factor
            AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor),
                                          CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

            selectedPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)

            If CurrentTool = Tool.Add Then
                ' If no point was selected, add a new point
                If selectedPointIndex = -1 Then
                    AddPoint(AdjustedMouseLocation)
                End If
            ElseIf CurrentTool = Tool.Move Then
                ' If a point was selected, start moving it
                If selectedPointIndex <> -1 Then
                    MovePoint(AdjustedMouseLocation)
                End If
            ElseIf CurrentTool = Tool.Subtract Then
                ' If a point was selected, remove it
                If selectedPointIndex <> -1 Then
                    RemovePoint(selectedPointIndex)
                End If
            End If

            ' Set the drawing flag to true
            isDrawing = True

            GeneratePointArrayText()

            Invalidate()

        End If

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove

        ' Calculate the adjusted mouse location based on the scale factor
        AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor),
                                          CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

        If ActiveControl IsNot Nothing Then

            ' Clear the active control
            ActiveControl = Nothing

            Focus()

        End If

        ' Check if the left mouse button is pressed and and a point is selected
        If isDrawing AndAlso selectedPointIndex <> -1 Then
            'Yes, we are moving a point

            ' Adjust the selected point's location based on the mouse movement
            MovePoint(AdjustedMouseLocation)

            GeneratePointArrayText()

            Invalidate()

        End If

        ' Update hovered point index
        ' Get the new hovered point index based on the adjusted mouse location
        Dim newHoveredPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)

        ' If the new hovered point index is different from the current hovered point index then
        If newHoveredPointIndex <> hoveredPointIndex Then

            ' Set the new hovered point index
            hoveredPointIndex = newHoveredPointIndex

            ' Invalidate the form to trigger a repaint
            Invalidate()

        End If

    End Sub

    Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        If e.Button = MouseButtons.Left Then
            isDrawing = False
            selectedPointIndex = -1
            GeneratePointArrayText()
        End If
    End Sub

    Private Sub AddPoint(location As Point)
        ' Helper method for adding points and their mirrored counterparts

        points.Add(location)
        points.Add(GetMirroredPoint(location))
        selectedPointIndex = points.Count - 2
    End Sub

    Private Sub MovePoint(location As Point)
        ' Helper method for moving points and updating their mirrored counterparts

        points(selectedPointIndex) = location
        points(selectedPointIndex + 1) = GetMirroredPoint(location)
    End Sub

    Private Function GetMirroredPoint(p As Point) As Point
        ' Helper method to calculate the mirrored point

        Return New Point(p.X, -p.Y)
    End Function

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter AndAlso points.Count > 2 Then
            ' Close the shape
            CloseShape()
        ElseIf e.KeyCode = Keys.Delete AndAlso selectedPointIndex <> -1 Then
            ' Remove the selected point
            RemovePoint(selectedPointIndex)
        ElseIf e.KeyCode = Keys.OemMinus AndAlso selectedPointIndex <> -1 Then
            ' Remove the selected point
            RemovePoint(selectedPointIndex)

        ElseIf e.KeyCode = Keys.N AndAlso selectedPointIndex <> -1 Then
            ' Insert a new point near the selected point
            InsertNewPoint(selectedPointIndex)

        ElseIf e.KeyCode = Keys.Oemplus AndAlso selectedPointIndex <> -1 Then
            ' Insert a new point near the selected point
            InsertNewPoint(selectedPointIndex)



        End If

        'e.Handled = True

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        ' Intercept the Arrow keys

        If keyData = Keys.Up Then

            If VScrollBar1.Value - 10 < VScrollBar1.Minimum Then

                VScrollBar1.Value = VScrollBar1.Minimum

            Else

                VScrollBar1.Value -= 10

            End If

            ' Update the drawing center based on the scroll value
            DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2 - VScrollBar1.Value

            Invalidate()

            ' Return True to indicate the key event has been handled
            Return True

        ElseIf keyData = Keys.Down Then

            If VScrollBar1.Value + 10 > VScrollBar1.Maximum Then
                VScrollBar1.Value = VScrollBar1.Maximum
            Else
                VScrollBar1.Value += 10
            End If
            ' Update the drawing center based on the scroll value
            DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2 - VScrollBar1.Value
            Invalidate()
            ' Return True to indicate the key event has been handled
            Return True

        ElseIf keyData = Keys.Left Then

            If HScrollBar1.Value - 10 < HScrollBar1.Minimum Then

                HScrollBar1.Value = HScrollBar1.Minimum

            Else

                HScrollBar1.Value -= 10

            End If

            ' Update the drawing center based on the scroll value
            DrawingCenter.X = (ClientSize.Width \ 4) - (VScrollBar1.Width \ 2) - HScrollBar1.Value

            Invalidate()

            Return True

        ElseIf keyData = Keys.Right Then

            ' Check if the scroll value exceeds the maximum limit
            If HScrollBar1.Value + 10 > HScrollBar1.Maximum Then

                ' Set the scroll value to the maximum limit
                HScrollBar1.Value = HScrollBar1.Maximum

            Else

                ' Move the drawing area to the right
                HScrollBar1.Value += 10

            End If

            ' Update the drawing center based on the scroll value
            DrawingCenter.X = (ClientSize.Width \ 4) - (VScrollBar1.Width \ 2) - HScrollBar1.Value

            Invalidate()

            Return True

        End If

        ' Call the base class implementation for other keys
        Return MyBase.ProcessCmdKey(msg, keyData)

    End Function


    Private Sub CloseShape()
        points.Add(points(0)) ' Close the shape
        points.Add(GetMirroredPoint(points(1))) ' Close the mirror shape
        GeneratePointArrayText()
        Invalidate()
    End Sub


    Private Sub RemovePoint(index As Integer)
        If index >= 0 AndAlso index < points.Count - 1 Then
            points.RemoveAt(index + 1) ' Remove mirrored point
            points.RemoveAt(index)     ' Remove actual point
        End If
        selectedPointIndex = -1
        GeneratePointArrayText()
        Invalidate()
    End Sub


    Private Sub InsertNewPoint(index As Integer)
        Dim newPoint As New Point(points(index).X + 10, points(index).Y + 10)
        points.Insert(index + 2, newPoint)
        points.Insert(index + 3, GetMirroredPoint(newPoint))
        selectedPointIndex += 2
        GeneratePointArrayText()
        Invalidate()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        LayoutForm()

        Invalidate()

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

        ' Update TextBox1
        TextBox1.Top = ClientRectangle.Top + menuStripHeight
        TextBox1.Left = halfClientWidth
        TextBox1.Width = halfClientWidth
        TextBox1.Height = clientHeight - menuStripHeight

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

        ' Update Button1
        'Button1.Top = HScrollBar1.Top
        'Button1.Left = VScrollBar1.Left
        CenterDrawingButton.Width = vScrollBarWidth + 2
        CenterDrawingButton.Height = hScrollBarHeight + 2

        'Button2.Top = HScrollBar1.Top - Button2.Height
        'Button2.Left = VScrollBar1.Left - Button2.Width
        Button2.ImageAlign = ContentAlignment.TopLeft
        Button2.Width = CenterDrawingButton.Width
        Button2.Height = CenterDrawingButton.Height

        Button3.ImageAlign = ContentAlignment.TopLeft
        Button3.Width = CenterDrawingButton.Width
        Button3.Height = CenterDrawingButton.Height

        GroupBox1.Top = HScrollBar1.Top
        GroupBox1.Left = VScrollBar1.Left

        GroupBox1.Width = vScrollBarWidth
        GroupBox1.Height = hScrollBarHeight

        If ScaleFactor >= 8 Then

            'HScrollBar1.Minimum = -ClientSize.Width * (ScaleFactor / 16)

            'HScrollBar1.Maximum = ClientSize.Width * (ScaleFactor / 16)

            'VScrollBar1.Minimum = -ClientSize.Height * (ScaleFactor / 16)

            'VScrollBar1.Maximum = ClientSize.Height * (ScaleFactor / 16)

            'If Not HScrollBar1.Visible Then HScrollBar1.Visible = True

            'If Not VScrollBar1.Visible Then VScrollBar1.Visible = True

            Button2.Top = HScrollBar1.Top - Button2.Height
            Button2.Left = VScrollBar1.Left - Button2.Width

            Button3.Top = HScrollBar1.Top - Button2.Height - Button3.Height
            Button3.Left = VScrollBar1.Left - Button2.Width



        Else

            'If HScrollBar1.Visible Then HScrollBar1.Visible = False

            'If VScrollBar1.Visible Then VScrollBar1.Visible = False


            Button2.Top = HScrollBar1.Top - Button2.Height
            Button2.Left = GroupBox1.Left - 1

        End If

        ' Update CheckBoxes
        HideControlHandlesCheckBox.Top = TrackBar1.Bottom - Label1.Height - 5
        HideControlHandlesCheckBox.Left = Label1.Right + 25

        FillShapeCheckBox.Top = HideControlHandlesCheckBox.Top
        FillShapeCheckBox.Left = HideControlHandlesCheckBox.Right + 25

        DarkModeCheckBox.Top = HideControlHandlesCheckBox.Top
        DarkModeCheckBox.Left = FillShapeCheckBox.Right + 25

    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll

        ScaleFactor = TrackBar1.Value / 100.0

        UpdateUIScaleFactor()

        Invalidate()

    End Sub

    Private Sub UpdateUIScaleFactor()

        ResetScrollBars()

        CenterDrawingArea()

        If ScaleFactor >= 8 Then

            Button2.Top = HScrollBar1.Top - Button2.Height

            Button2.Left = VScrollBar1.Left - Button2.Width

            Button3.Top = HScrollBar1.Top - Button2.Height - Button3.Height

            Button3.Left = VScrollBar1.Left - Button2.Width


            Dim ScaleFactorDiv16 = ScaleFactor / 16

            HScrollBar1.Minimum = -ClientSize.Width * ScaleFactorDiv16

            HScrollBar1.Maximum = ClientSize.Width * ScaleFactorDiv16

            VScrollBar1.Minimum = -ClientSize.Height * ScaleFactorDiv16

            VScrollBar1.Maximum = ClientSize.Height * ScaleFactorDiv16

            If Not HScrollBar1.Visible Then HScrollBar1.Visible = True

            If Not VScrollBar1.Visible Then VScrollBar1.Visible = True

            'Else

            '    Button2.Top = HScrollBar1.Top - Button2.Height

            '    Button2.Left = GroupBox1.Left - 1

            '    Button3.Top = HScrollBar1.Top - Button2.Height - Button3.Height

            '    Button3.Left = GroupBox1.Left - 1


            '    If HScrollBar1.Visible Then HScrollBar1.Visible = False

            '    If VScrollBar1.Visible Then VScrollBar1.Visible = False

        End If

        Label1.Text = $"Scale Factor: {ScaleFactor:N2}"

    End Sub

    Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll

        ' Update the drawing center based on the scroll value
        DrawingCenter.X = (ClientSize.Width \ 4) - (VScrollBar1.Width \ 2) - HScrollBar1.Value

        Invalidate()

    End Sub

    Private Sub VScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles VScrollBar1.Scroll

        ' Update the drawing center based on the scroll value
        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2 - VScrollBar1.Value

        Invalidate()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles CenterDrawingButton.Click

        ResetScrollBars()

        CenterDrawingArea()

        Invalidate()

    End Sub

    Private Sub HideControlHandlesCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles HideControlHandlesCheckBox.CheckedChanged
        Invalidate()
    End Sub

    Private Sub FillShapeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles FillShapeCheckBox.CheckedChanged
        Invalidate()
    End Sub



    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        points.Clear()
        TextBox1.Clear()
        CenterDrawingArea()
        ResetScrollBars()
        Invalidate()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click

        Using saveFileDialog As New SaveFileDialog()

            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            saveFileDialog.Title = "Save Shape"
            saveFileDialog.InitialDirectory = Application.StartupPath

            If saveFileDialog.ShowDialog(Me) = DialogResult.OK Then

                Using writer As New StreamWriter(saveFileDialog.FileName)

                    ' Write the CSV headers (optional).
                    writer.WriteLine("X,Y")

                    For Each point As Point In points
                        writer.WriteLine($"{point.X},{point.Y}")
                    Next

                End Using

                ' Add file name to "Shape Editor - Code with Joe" and display in titlebar.
                Text = $"{Path.GetFileName(saveFileDialog.FileName)} - Shape Editor - Code with Joe"

            End If

        End Using

    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click

        Using openFileDialog As New OpenFileDialog()

            openFileDialog.AutoUpgradeEnabled = True
            openFileDialog.ShowReadOnly = False
            openFileDialog.ShowHelp = False

            'openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"

            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

            openFileDialog.Title = "Open Shape"
            openFileDialog.InitialDirectory = Application.StartupPath

            If openFileDialog.ShowDialog() = DialogResult.OK Then

                points.Clear()

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
                                    points.Add(New Point(x, y))

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
                            ' Handle IOException (e.g., file being used by another process)
                            MessageBox.Show("The file in use by another app. Close the file and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Case TypeOf ex Is FileNotFoundException
                            ' Handle FileNotFoundException
                            MessageBox.Show("The file was not found. Please check the file path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Case TypeOf ex Is FormatException
                            ' Handle FormatException
                            MessageBox.Show("The file format is invalid. Please check the file contents.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Case TypeOf ex Is ArgumentException
                            ' Handle ArgumentException
                            MessageBox.Show("The file path is invalid. Please check the file path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Case TypeOf ex Is PathTooLongException
                            ' Handle PathTooLongException
                            MessageBox.Show("The file path is too long. Please shorten the file path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Case TypeOf ex Is NotSupportedException

                        Case TypeOf ex Is UnauthorizedAccessException
                            ' Handle UnauthorizedAccessException
                            MessageBox.Show("You do not have permission to access this file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Case Else
                            ' Handle other exceptions
                            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                    End Select

                    fileIsValid = False

                End Try

                If Not fileIsValid Then

                    Text = $"Bad File - Shape Editor - Code with Joe"

                End If

                ScaleFactor = 8

                TrackBar1.Value = CInt(ScaleFactor * 100)

                UpdateUIScaleFactor()

                GeneratePointArrayText()

                Invalidate()

            End If

        End Using

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

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
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If MessageBox.Show("Are you sure you want to exit?", "Exit Shape Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            e.Cancel = True
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

        Invalidate()

    End Sub

    Private Sub CenterDrawingArea()

        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2

        DrawingCenter.X = ClientSize.Width \ 4 - VScrollBar1.Width \ 2

    End Sub

    Private Sub ResetScrollBars()

        HScrollBar1.Value = 0
        VScrollBar1.Value = 0

    End Sub

    Private Sub ApplyUITheme()

        If DarkModeCheckBox.Checked Then

            'set title color - dark mode
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

            SetWindowTheme(Button2.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(Button2.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Button2.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(Button3.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(Button3.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Button3.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

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
            CustomMenuRenderer.BorderColor = MenuItemBorderColor_DarkMode
            CustomMenuRenderer.MenuItemSelectedColor = MenuItemSelectedColor_DarkMode
            CustomMenuRenderer.TextColor = MenuItemTextColor_DarkMode
            CustomMenuRenderer.SelectedBorderColor = MenuItemSelectedBorderColor_DarkMode

            If CurrentTool = Tool.Add Then

                Button2.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelectedDarkMode)
                Button3.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkMode)

            ElseIf CurrentTool = Tool.Move Then

                Button2.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonDarkMode)
                Button3.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkModeSelected)

            ElseIf CurrentTool = Tool.Subtract Then

                Button2.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonDarkMode)
                Button3.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkMode)

            End If

            CenterDrawingButton.Image = ResourceToImage(My.Resources.Resource1.CenterDrawingToolButtonDarkMode)

        Else

            'set title color - light mode
            DwmSetWindowAttribute(Handle, 20, 0, Marshal.SizeOf(GetType(Boolean)))

            ' Set the theme to light mode
            SetWindowTheme(HScrollBar1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(VScrollBar1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(CenterDrawingButton.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(CenterDrawingButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(CenterDrawingButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(Button2.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(Button2.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Button2.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(Button3.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(Button3.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Button3.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

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
            CustomMenuRenderer.BorderColor = MenuItemBorderColor_LightMode
            CustomMenuRenderer.MenuItemSelectedColor = MenuItemSelectedColor_LightMode
            CustomMenuRenderer.TextColor = MenuItemTextColor_LightMode
            CustomMenuRenderer.SelectedBorderColor = SelectedBorderColor_LightMode

            ' Set the button images for light mode
            Button2.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelectedLightMode)
            CenterDrawingButton.Image = ResourceToImage(My.Resources.Resource1.CenterDrawingToolButtonLightMode)

        End If

        MenuStrip1.Renderer = CustomMenuRenderer

        TrackBar1.BackColor = If(DarkModeCheckBox.Checked, Color.FromArgb(255, 32, 32, 32), SystemColors.Control)

        DarkModeCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        FillShapeCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        TextBox1.BackColor = If(DarkModeCheckBox.Checked, Color.FromArgb(255, 32, 32, 32), SystemColors.Control)

        ShapeFillBrush = If(DarkModeCheckBox.Checked, ShapeFillBrushDarkMode, ShapeFillBrushLightMode)

        ShapePen = New Pen(If(DarkModeCheckBox.Checked, Color.White, Color.Black), 2)

        HandleBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.DodgerBlue, Color.DarkGray)))

        HoverBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.Orchid, Color.DodgerBlue)))

        Label1.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        HideControlHandlesCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        TextBox1.ForeColor = If(DarkModeCheckBox.Checked, Color.FromArgb(255, 230, 230, 230), Color.FromArgb(255, 32, 32, 32))

        Label1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        HideControlHandlesCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        FillShapeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        DarkModeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        CenterDrawingButton.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        GroupBox1.BackColor = If(DarkModeCheckBox.Checked, Color.FromArgb(255, 32, 32, 32), Color.White)

    End Sub

    Private Sub DrawGrid(g As Graphics)

        ' Start at the origin (0, 0) and draw the grid lines in both directions at intervals of 20 units multiplied by the scale factor.
        Dim stepSize As Integer = CInt(20 * ScaleFactor)

        Dim gridPen As Pen = If(DarkModeCheckBox.Checked, GridPenDark, GridPenLight)

        ' Draw vertical grid lines
        For i As Integer = -((ClientSize.Width * 8) \ stepSize) To (ClientSize.Width * 8) \ stepSize
            Dim x As Integer = i * stepSize
            g.DrawLine(gridPen, x, -ClientSize.Height * 8, x, ClientSize.Height * 8)
        Next

        ' Draw horizontal grid lines
        For i As Integer = -((ClientSize.Height * 8) \ stepSize) To (ClientSize.Height * 8) \ stepSize
            Dim y As Integer = i * stepSize
            g.DrawLine(gridPen, -ClientSize.Width * 8, y, ClientSize.Width * 8, y)
        Next

    End Sub

    Private Function GetPointIndexAtLocation(location As Point) As Integer

        For i As Integer = 0 To points.Count - 1 Step 2
            Dim point As Point = points(i)
            Dim scaledPoint As New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
            Dim rect As New Rectangle(scaledPoint.X - handleSize / 2, scaledPoint.Y - handleSize / 2, handleSize, handleSize)

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

        ' Retrieve the ordered list of points (assumed to be a method returning a collection of points).
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

    End Sub

    Private Function GetOrderedPoints() As List(Of Point)

        Dim orderedPoints As New List(Of Point)()

        For i As Integer = 0 To points.Count - 1 Step 2
            orderedPoints.Add(points(i))
        Next

        For i As Integer = points.Count - 1 To 1 Step -2
            orderedPoints.Add(points(i))
        Next

        If points.Count > 0 Then
            orderedPoints.Add(points(0)) ' Close the shape
        End If

        Return orderedPoints

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

        ' The MemoryStream is disposed of automatically when it goes out of scope,
        ' so you don't need to worry about memory management.

    End Function

    Private Sub DarkModeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DarkModeCheckBox.CheckedChanged

        ApplyUITheme()

        Refresh()

        Invalidate()

        ' Fixes title bar theme update issue in Windows 10
        ' Check if the OS is Windows 10
        If OsVersion.Major = 10 And OsVersion.Minor = 0 And OsVersion.Build < 22000 Then
            ' The first public build of Windows 11 had the build number 10.0.22000

            ' Force a redraw of the form by showing a message box.
            MsgBox("Applying UI Theme", MsgBoxStyle.OkOnly, "Shape Editor - Code with Joe")

            ' 10.0.19045.5737 - Windows 10 Home Version	22H2
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        CurrentTool = Tool.Move

        Button2.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonDarkMode)
        Button3.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkModeSelected)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        CurrentTool = Tool.Add

        Button2.Image = ResourceToImage(My.Resources.Resource1.AddPointToolButtonSelectedDarkMode)
        Button3.Image = ResourceToImage(My.Resources.Resource1.MovePointToolButtonDarkMode)

    End Sub

End Class

Public Class CustomColorMenuStripRenderer
    Inherits ToolStripProfessionalRenderer

    Public MenuItemBackground As Color
    Public MenuItemBackgroundSelected As Color
    Public ToolStripBackground As Color
    Public BorderColor As Color
    Public SelectedBorderColor As Color
    Public MenuItemSelectedColor As Color
    Public MenuItemSelectedGradientBegin As Color
    Public MenuItemSelectedGradientEnd As Color
    Public SeparatorColor As Color
    Public CheckmarkColor As Color
    Public TextColor As Color

    'constructor
    Public Sub New(menuItemBackground As Color, menuItemBackgroundSelected As Color, toolStripBackground As Color, borderColor As Color, menuItemSelectedColor As Color, textColor As Color, selectedBorderColor As Color)
        Me.MenuItemBackground = menuItemBackground
        Me.MenuItemBackgroundSelected = menuItemBackgroundSelected
        Me.ToolStripBackground = toolStripBackground
        Me.BorderColor = borderColor
        Me.MenuItemSelectedColor = menuItemSelectedColor
        Me.TextColor = textColor
        Me.SelectedBorderColor = selectedBorderColor

        MenuItemSelectedGradientBegin = Color.FromArgb(255, 64, 64, 64)
        MenuItemSelectedGradientEnd = Color.FromArgb(255, 64, 64, 64)
        SeparatorColor = Color.FromArgb(255, 50, 50, 50)
        CheckmarkColor = Color.FromArgb(255, 255, 255, 255)

    End Sub

    ' Render the menu item text
    Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
        e.Item.ForeColor = TextColor
        MyBase.OnRenderItemText(e)
    End Sub

    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)

        Dim rect As New Rectangle(Point.Empty, e.Item.Size)

        If e.Item.Selected Then

            If TypeOf e.Item Is ToolStripMenuItem Then


                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then

                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        'e.Graphics.DrawRectangle(New Pen(BorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)
                    End Using

                Else 'SelectedBorderColor

                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        e.Graphics.DrawRectangle(New Pen(SelectedBorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)

                    End Using

                End If

            Else

                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then

                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        e.Graphics.DrawRectangle(New Pen(SelectedBorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)
                    End Using

                Else

                    Using Brush As New SolidBrush(MenuItemBackground)
                        e.Graphics.FillRectangle(Brush, rect)
                    End Using

                End If

            End If

        Else
            'Not selected

            ' Check if the item is a ToolStripMenuItem
            If TypeOf e.Item Is ToolStripMenuItem Then

                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then

                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        'e.Graphics.DrawRectangle(New Pen(BorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)
                    End Using

                Else

                    Using Brush As New SolidBrush(MenuItemBackground)
                        e.Graphics.FillRectangle(Brush, rect)
                    End Using

                End If

            Else

                If CType(e.Item, ToolStripMenuItem).DropDown.Visible Then

                    Using Brush As New SolidBrush(MenuItemBackgroundSelected)
                        e.Graphics.FillRectangle(Brush, rect)
                        e.Graphics.DrawRectangle(New Pen(SelectedBorderColor), rect.Left + 2, rect.Top, rect.Right - 4, rect.Bottom - 1)
                    End Using

                Else

                    Using Brush As New SolidBrush(MenuItemBackground)
                        e.Graphics.FillRectangle(Brush, rect)
                    End Using

                End If

            End If

        End If

    End Sub

    ' Render the overall background
    Protected Overrides Sub OnRenderToolStripBackground(e As ToolStripRenderEventArgs)
        'Dim gradientBrush As New LinearGradientBrush(e.AffectedBounds, Color.Black, Color.Gray, LinearGradientMode.Vertical)
        Using Brush As New SolidBrush(ToolStripBackground)
            e.Graphics.FillRectangle(Brush, e.AffectedBounds)
        End Using
        'e.Graphics.FillRectangle(gradientBrush, e.AffectedBounds)
        'gradientBrush.Dispose()
    End Sub

    ' Render the border
    Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
        e.Graphics.DrawRectangle(New Pen(BorderColor), New Rectangle(Point.Empty, e.ToolStrip.Size - New Size(1, 1)))
    End Sub

    ' Render the separator
    Protected Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
        Dim rect As Rectangle = e.Item.ContentRectangle
        Dim pen As New Pen(SeparatorColor)
        e.Graphics.DrawLine(pen, rect.Left, rect.Height \ 2, rect.Right, rect.Height \ 2)
        pen.Dispose()
    End Sub

End Class


