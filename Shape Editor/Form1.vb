Public Class Form1
    Private points As New List(Of Point)()
    Private isDrawing As Boolean = False
    Private selectedPointIndex As Integer = -1
    Private hoveredPointIndex As Integer = -1
    Private Const handleSize As Integer = 15
    Private ScaleFactor As Double = 1.0 ' Adjust the scale factor as needed

    Private ShapePen As New Pen(Color.Black, 2)
    Private ShapeBrush As New SolidBrush(Color.FromArgb(128, Color.Blue)) ' Semi-transparent blue brush for filling the shape
    Private DrawingCenter As Point
    Private AdjustedMouseLocation As Point
    Private HandleBrush As New SolidBrush(Color.FromArgb(255, Color.DarkGray)) ' Semi-transparent blue brush for the control handles
    Private HoverBrush As New SolidBrush(Color.FromArgb(255, Color.Gray)) ' Semi-transparent blue brush for the control handles


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        Me.KeyPreview = True ' Enable KeyPreview to capture key events at the form level

        ' Set focus to the form itself
        Me.Focus()
        Me.ActiveControl = Nothing

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
        e.Graphics.Clear(If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control))
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None

        ' Translate the origin to the center of the drawing area
        e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)

        DrawGrid(e.Graphics)

        ' Draw the coordinate system
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.Gray, Pens.Gray), -ClientSize.Width * 3, 0, ClientSize.Width * 3, 0) ' X-axis
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.Gray, Pens.Gray), 0, -ClientSize.Height * 3, 0, ClientSize.Height * 3) ' Y-axis

        ' Draw intersecting lines at the origin
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), -5, 0, 5, 0) ' Horizontal line
        e.Graphics.DrawLine(If(DarkModeCheckBox.Checked, Pens.White, Pens.Black), 0, -5, 0, 5) ' Vertical line

        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        e.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality

        '' Update brushes and pens based on dark mode state
        'ShapeBrush = New SolidBrush(Color.FromArgb(128, If(DarkModeCheckBox.Checked, Color.DarkSlateGray, Color.Blue)))
        'ShapePen = New Pen(If(DarkModeCheckBox.Checked, Color.White, Color.Black), 2)
        'HandleBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.DeepSkyBlue, Color.Red)))

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

    Private Sub DrawGrid(g As Graphics)
        ' Start at the origin (0, 0) and draw the grid lines in both directions at intervals of 20 units multiplied by the scale factor.
        Dim stepSize As Integer = CInt(20 * ScaleFactor)
        Dim gridPen As Pen = If(DarkModeCheckBox.Checked, Pens.DarkSlateGray, Pens.LightGray)

        ' Draw vertical grid lines
        For i As Integer = -((ClientSize.Width * 3) \ stepSize) To (ClientSize.Width * 3) \ stepSize
            Dim x As Integer = i * stepSize
            g.DrawLine(gridPen, x, -ClientSize.Height * 3, x, ClientSize.Height * 3)
        Next

        ' Draw horizontal grid lines
        For i As Integer = -((ClientSize.Height * 3) \ stepSize) To (ClientSize.Height * 3) \ stepSize
            Dim y As Integer = i * stepSize
            g.DrawLine(gridPen, -ClientSize.Width * 3, y, ClientSize.Width * 3, y)
        Next
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

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        DrawingCenter = New Point(ClientSize.Width \ 4 - VScrollBar1.Width \ 2, (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height) \ 2)

        TextBox1.Top = ClientRectangle.Top
        TextBox1.Left = ClientSize.Width / 2
        TextBox1.Width = ClientSize.Width / 2
        TextBox1.Height = ClientSize.Height

        TrackBar1.Top = ClientRectangle.Bottom - TrackBar1.Height
        TrackBar1.Left = ClientRectangle.Left
        TrackBar1.Width = ClientSize.Width / 2

        Label1.Top = TrackBar1.Bottom - Label1.Height - 5
        Label1.Left = ClientRectangle.Left + 5
        Label1.Width = 200
        Label1.Height = 20

        HScrollBar1.Top = ClientRectangle.Bottom - TrackBar1.Height - HScrollBar1.Height
        HScrollBar1.Left = ClientRectangle.Left
        HScrollBar1.Width = ClientSize.Width / 2 - VScrollBar1.Width
        HScrollBar1.Minimum = (-ClientSize.Width \ 4) * 3
        HScrollBar1.Maximum = (ClientSize.Width \ 4) * 3
        HScrollBar1.Value = 0

        VScrollBar1.Top = ClientRectangle.Top
        VScrollBar1.Left = TextBox1.Left - VScrollBar1.Width
        VScrollBar1.Height = ClientSize.Height - TrackBar1.Height - HScrollBar1.Height
        VScrollBar1.Minimum = (-ClientSize.Height \ 4) * 3
        'VScrollBar1.Maximum = (ClientSize.Height \ 4) * 3
        VScrollBar1.Maximum = (ClientSize.Height \ 4) * 3


        VScrollBar1.Value = 0

        Button1.Top = HScrollBar1.Top
        Button1.Left = VScrollBar1.Left
        Button1.Width = VScrollBar1.Width
        Button1.Height = HScrollBar1.Height

        HideControlHandlesCheckBox.Top = TrackBar1.Bottom - Label1.Height - 5
        HideControlHandlesCheckBox.Left = Label1.Right + 25

        FillShapeCheckBox.Top = HideControlHandlesCheckBox.Top
        FillShapeCheckBox.Left = HideControlHandlesCheckBox.Right + 25

        DarkModeCheckBox.Top = HideControlHandlesCheckBox.Top
        DarkModeCheckBox.Left = FillShapeCheckBox.Right + 25

        Invalidate()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        ScaleFactor = TrackBar1.Value / 100.0
        Label1.Text = $"Scale Factor: {ScaleFactor:N2}"
        GeneratePointArrayText()
        Invalidate()
    End Sub

    Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll

        ' Update the drawing center based on the scroll value


        DrawingCenter.X = (ClientSize.Width \ 4) - (VScrollBar1.Width \ 2) - HScrollBar1.Value



        Invalidate()
    End Sub




    Private Sub VScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles VScrollBar1.Scroll

        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height) \ 2 - VScrollBar1.Value



        Invalidate()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Center the drawing area
        VScrollBar1.Value = 0
        DrawingCenter.Y = (ClientSize.Height - TrackBar1.Height - HScrollBar1.Height) \ 2

        HScrollBar1.Value = 0
        DrawingCenter.X = ClientSize.Width \ 4 - VScrollBar1.Width \ 2

        Invalidate()
    End Sub

    Private Sub HideControlHandlesCheckBox_CheckedChanged(sender As Object, e As EventArgs)
        Invalidate()
    End Sub

    Private Sub FillShapeCheckBox_CheckedChanged(sender As Object, e As EventArgs)
        Invalidate()
    End Sub

    Private Sub DarkModeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DarkModeCheckBox.CheckedChanged

        ' Update brushes and pens based on dark mode state
        ShapeBrush = New SolidBrush(Color.FromArgb(128, If(DarkModeCheckBox.Checked, Color.Silver, Color.DodgerBlue)))
        ShapePen = New Pen(If(DarkModeCheckBox.Checked, Color.White, Color.Black), 2)


        HandleBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.DodgerBlue, Color.DarkGray)))

        HoverBrush = New SolidBrush(Color.FromArgb(255, If(DarkModeCheckBox.Checked, Color.Orchid, Color.Gray)))



        TrackBar1.BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)


        Label1.BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)
        Label1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
        TextBox1.BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)
        TextBox1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)


        HideControlHandlesCheckBox.BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)
        HideControlHandlesCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        FillShapeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
        FillShapeCheckBox.BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)


        DarkModeCheckBox.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)
        DarkModeCheckBox.BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)



        'Button1.BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)

        'Button1.ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)

        'BackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)

        'ForeColor = If(DarkModeCheckBox.Checked, Color.White, Color.Black)




        'Control.DefaultBackColor = If(DarkModeCheckBox.Checked, Color.Black, SystemColors.Control)



        Invalidate()

    End Sub

End Class
