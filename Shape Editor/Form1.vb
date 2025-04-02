' Shape Editor

' A simple shape editor that allows you to draw shapes and generate the corresponding point array in VB.NET.

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

    Private GridColorDark As Color = Color.FromArgb(255, 64, 64, 64)
    Private GridColorLight As Color = Color.FromArgb(255, 240, 240, 240)

    Private DarkModeControlColor As Color = Color.FromArgb(255, 32, 32, 32)

    Private GridPenDark As New Pen(GridColorDark, 1)
    Private GridPenLight As New Pen(GridColorLight, 1)

    Private ShapeColorLight As Color = Color.FromArgb(128, Color.Blue)
    Private ShapeColorDark As Color = Color.FromArgb(128, 128, 128, 128)
    Private ShapeBrush As New SolidBrush(ShapeColorLight)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        Me.KeyPreview = True

        Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled

        Text = "Shape Editor - Code with Joe"

        ScaleFactor = TrackBar1.Value / 100.0
        Label1.Text = $"Scale Factor: {ScaleFactor:N2}"

        ' Add event handlers for checkboxes
        AddHandler HideControlHandlesCheckBox.CheckedChanged, AddressOf HideControlHandlesCheckBox_CheckedChanged
        AddHandler FillShapeCheckBox.CheckedChanged, AddressOf FillShapeCheckBox_CheckedChanged
        AddHandler DarkModeCheckBox.CheckedChanged, AddressOf DarkModeCheckBox_CheckedChanged

        CenterToScreen()

        ' Maximize the form
        WindowState = FormWindowState.Maximized

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver
        e.Graphics.Clear(If(DarkModeCheckBox.Checked, Color.Black, Color.White))
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None

        ' Translate the origin to the center of the drawing area
        e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)

        DrawGrid(e.Graphics)

        ' Draw the coordinate system
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.Gray, Pens.Silver), -ClientSize.Width * 8, 0, ClientSize.Width * 8, 0) ' X-axis
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.Gray, Pens.Silver), 0, -ClientSize.Height * 8, 0, ClientSize.Height * 8) ' Y-axis

        ' Draw intersecting lines at the origin
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), -5, 0, 5, 0) ' Horizontal line
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), 0, -5, 0, 5) ' Vertical line

        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

        If points.Count > 1 Then
            Dim orderedPoints = GetOrderedPoints()
            Dim scaledPoints = orderedPoints.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()

            ' Fill the shape if the checkbox is checked
            If FillShapeCheckBox.Checked Then
                e.Graphics.FillPolygon(ShapeBrush, scaledPoints)
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

        If e.Button = MouseButtons.Left Then
            AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor),
                                              CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

            selectedPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)

            ' If no point was selected, add a new point
            If selectedPointIndex = -1 Then
                ' Add the point
                points.Add(AdjustedMouseLocation)

                ' Add the mirror point
                points.Add(New Point(AdjustedMouseLocation.X, -AdjustedMouseLocation.Y))

                selectedPointIndex = points.Count - 2
            End If

            isDrawing = True

            GeneratePointArrayText()

            Invalidate()

        End If

    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove

        AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

        If isDrawing AndAlso selectedPointIndex <> -1 Then
            points(selectedPointIndex) = AdjustedMouseLocation
            points(selectedPointIndex + 1) = New Point(AdjustedMouseLocation.X, -AdjustedMouseLocation.Y)

            GeneratePointArrayText()

            Invalidate()
        End If

        ' Update hovered point index
        Dim newHoveredPointIndex = GetPointIndexAtLocation(AdjustedMouseLocation)

        If newHoveredPointIndex <> hoveredPointIndex Then
            hoveredPointIndex = newHoveredPointIndex
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

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

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

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

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
        Button1.Width = vScrollBarWidth + 2
        Button1.Height = hScrollBarHeight + 2

        GroupBox1.Top = HScrollBar1.Top
        GroupBox1.Left = VScrollBar1.Left

        GroupBox1.Width = vScrollBarWidth
        GroupBox1.Height = hScrollBarHeight

        ' Update CheckBoxes
        HideControlHandlesCheckBox.Top = TrackBar1.Bottom - Label1.Height - 5
        HideControlHandlesCheckBox.Left = Label1.Right + 25

        FillShapeCheckBox.Top = HideControlHandlesCheckBox.Top
        FillShapeCheckBox.Left = HideControlHandlesCheckBox.Right + 25

        DarkModeCheckBox.Top = HideControlHandlesCheckBox.Top
        DarkModeCheckBox.Left = FillShapeCheckBox.Right + 25

        Invalidate()

    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll

        ' UpdateScaleFactor()

        ScaleFactor = TrackBar1.Value / 100.0

        UpdateUIScaleFactor()

        GeneratePointArrayText()

        Invalidate()

    End Sub

    Private Sub UpdateUIScaleFactor()

        ResetScrollBars()

        CenterDrawingArea()

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

    End Sub

    Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll

        ' Update the drawing center based on the scroll value
        DrawingCenter.X = (ClientSize.Width \ 4) - (VScrollBar1.Width \ 2) - HScrollBar1.Value

        Invalidate()

    End Sub

    Private Sub VScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles VScrollBar1.Scroll

        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2 - VScrollBar1.Value

        Invalidate()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ResetScrollBars()

        CenterDrawingArea()

        Invalidate()

    End Sub

    Private Sub HideControlHandlesCheckBox_CheckedChanged(sender As Object, e As EventArgs)
        Invalidate()
    End Sub

    Private Sub FillShapeCheckBox_CheckedChanged(sender As Object, e As EventArgs)
        Invalidate()
    End Sub

    Private Sub DarkModeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DarkModeCheckBox.CheckedChanged

        UpdateUIForDarkMode()

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

            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            saveFileDialog.Title = "Save Points"
            saveFileDialog.InitialDirectory = Application.StartupPath

            If saveFileDialog.ShowDialog(Me) = DialogResult.OK Then

                Using writer As New StreamWriter(saveFileDialog.FileName)

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

            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            openFileDialog.Title = "Open Points"
            openFileDialog.InitialDirectory = Application.StartupPath

            If openFileDialog.ShowDialog() = DialogResult.OK Then

                points.Clear()

                Using reader As New StreamReader(openFileDialog.FileName)

                    While Not reader.EndOfStream

                        Dim line As String = reader.ReadLine()
                        Dim parts As String() = line.Split(","c)

                        If parts.Length = 2 Then

                            Dim x As Integer
                            Dim y As Integer

                            If Integer.TryParse(parts(0), x) AndAlso Integer.TryParse(parts(1), y) Then
                                points.Add(New Point(x, y))
                            End If

                        End If

                    End While

                    ' Add file name to "Shape Editor - Code with Joe" and display in titlebar.
                    Text = $"{Path.GetFileName(openFileDialog.FileName)} - Shape Editor - Code with Joe"

                End Using

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

    End Sub

    Private Sub CenterDrawingArea()

        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height + MenuStrip1.Height) \ 2

        DrawingCenter.X = ClientSize.Width \ 4 - VScrollBar1.Width \ 2

    End Sub

    Private Sub ResetScrollBars()

        HScrollBar1.Value = 0
        VScrollBar1.Value = 0

    End Sub

    Private Sub UpdateUIForDarkMode()

        Dim renderer As New CustomColorMenuStripRenderer(Color.FromArgb(255, 8, 8, 8), Color.FromArgb(255, 50, 50, 50), Color.FromArgb(255, 8, 8, 8), Color.FromArgb(255, 50, 50, 50), Color.FromArgb(255, 64, 64, 64), Color.FromArgb(255, 255, 255, 255), Color.FromArgb(255, 255, 255, 255))

        If DarkModeCheckBox.Checked Then

            SetWindowTheme(HScrollBar1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(VScrollBar1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(Button1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(Button1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Button1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(GroupBox1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(TextBox1.Handle, "DarkMode_Explorer", Nothing)
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

            renderer.MenuItemBackground = Color.FromArgb(255, 32, 32, 32)
            renderer.MenuItemBackgroundSelected = Color.FromArgb(255, 50, 50, 50)
            renderer.ToolStripBackground = Color.FromArgb(255, 32, 32, 32)
            renderer.BorderColor = Color.FromArgb(255, 50, 50, 50)
            renderer.MenuItemSelectedColor = Color.FromArgb(255, 64, 64, 64)
            renderer.TextColor = Color.FromArgb(255, 255, 255, 255)
            renderer.SelectedBorderColor = Color.FromArgb(255, Color.DodgerBlue)

        Else
            renderer.MenuItemBackground = Color.FromArgb(255, 240, 240, 240)
            renderer.MenuItemBackgroundSelected = Color.FromArgb(255, 255, 255, 255)
            renderer.ToolStripBackground = SystemColors.Control
            renderer.BorderColor = Color.FromArgb(255, 255, 255, 255)
            renderer.MenuItemSelectedColor = Color.FromArgb(64, Color.Gray)
            renderer.TextColor = Color.FromArgb(255, 0, 0, 0)
            renderer.SelectedBorderColor = Color.FromArgb(255, Color.DodgerBlue)

            SetWindowTheme(HScrollBar1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(HScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(VScrollBar1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(VScrollBar1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(Button1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(Button1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(Button1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(GroupBox1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(GroupBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

            SetWindowTheme(TextBox1.Handle, "Explorer", Nothing)
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
            DwmSetWindowAttribute(TextBox1.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

        End If

        MainMenuStrip.Renderer = renderer

        TrackBar1.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        DarkModeCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        FillShapeCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        TextBox1.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        ShapeBrush = New SolidBrush(If(DarkModeCheckBox.Checked, ShapeColorDark, ShapeColorLight))

        ShapePen = New Pen(If(DarkModeCheckBox.Checked, Color.White, Color.Black), 2)

        HandleBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.DodgerBlue, Color.DarkGray)))

        HoverBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.Orchid, Color.Gray)))

        Label1.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        HideControlHandlesCheckBox.BackColor = If(DarkModeCheckBox.Checked, DarkModeControlColor, SystemColors.Control)

        TextBox1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        Label1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        HideControlHandlesCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        FillShapeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        DarkModeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

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

        Dim sb As New System.Text.StringBuilder()

        sb.AppendLine("Dim ScaleFactor As Double = 1.0 ' Adjust the scale factor as needed")
        sb.AppendLine("")
        sb.AppendLine("Dim Shape As Point() = {")

        Dim orderedPoints = GetOrderedPoints()

        For i As Integer = 0 To orderedPoints.Count - 1
            If i < orderedPoints.Count - 1 Then
                sb.AppendLine($"    New Point(CInt({orderedPoints(i).X} * ScaleFactor), CInt({orderedPoints(i).Y} * ScaleFactor)),")
            Else
                sb.AppendLine($"    New Point(CInt({orderedPoints(i).X} * ScaleFactor), CInt({orderedPoints(i).Y} * ScaleFactor))")
            End If
        Next

        sb.AppendLine("}")

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


