Public Class Form1
    Private points As New List(Of Point)()
    Private isDrawing As Boolean = False
    Private selectedPointIndex As Integer = -1
    Private hoveredPointIndex As Integer = -1
    Private Const handleSize As Integer = 15
    Private ScaleFactor As Double = 1.0 ' Adjust the scale factor as needed

    Private ShapePen As New Pen(Color.Black, 2)
    Private DrawingCenter As Point
    Private AdjustedMouseLocation As Point

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        Me.KeyPreview = True ' Enable KeyPreview to capture key events at the form level

        ' Set focus to the form itself
        Me.Focus()
        Me.ActiveControl = Nothing

        ScaleFactor = TrackBar1.Value / 100.0
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver
        e.Graphics.Clear(SystemColors.Control)
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

        ' Translate the origin to the center of the drawing area
        e.Graphics.TranslateTransform(DrawingCenter.X, DrawingCenter.Y)

        ' Draw intersecting lines at the origin
        e.Graphics.DrawLine(Pens.Black, -5, 0, 5, 0) ' Horizontal line
        e.Graphics.DrawLine(Pens.Black, 0, -5, 0, 5) ' Vertical line

        If points.Count > 1 Then
            Dim orderedPoints = GetOrderedPoints()
            Dim scaledPoints = orderedPoints.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()
            e.Graphics.DrawPolygon(ShapePen, scaledPoints)
        End If

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None

        ' Draw point handles
        For i As Integer = 0 To points.Count - 1 Step 2
            Dim point = points(i)
            Dim scaledPoint = New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
            If i = selectedPointIndex OrElse i = hoveredPointIndex Then
                e.Graphics.FillRectangle(Brushes.Purple, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
            Else
                e.Graphics.FillRectangle(Brushes.Red, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
            End If
        Next

        ' Draw the mouse location
        'e.Graphics.FillEllipse(Brushes.Green, AdjustedMouseLocation.X - 2, AdjustedMouseLocation.Y - 2, 4, 4)
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        If e.Button = MouseButtons.Left Then
            AdjustedMouseLocation = New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))
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

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down Then
            e.SuppressKeyPress = True
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
        Dim result As String = sb.ToString()
        TextBox1.Text = result
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
        VScrollBar1.Maximum = (ClientSize.Height \ 4) * 3
        VScrollBar1.Value = 0

        Button1.Top = HScrollBar1.Top
        Button1.Left = VScrollBar1.Left
        Button1.Width = VScrollBar1.Width
        Button1.Height = HScrollBar1.Height

        Invalidate()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        ScaleFactor = TrackBar1.Value / 100.0
        Label1.Text = $"Scale Factor: {ScaleFactor:N2}"
        GeneratePointArrayText()
        Invalidate()
    End Sub

    Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll
        DrawingCenter.X = ClientSize.Width \ 4 - HScrollBar1.Value
        Invalidate()
    End Sub

    Private Sub VScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles VScrollBar1.Scroll
        DrawingCenter.Y = ClientSize.Height \ 2 - VScrollBar1.Value
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

End Class
