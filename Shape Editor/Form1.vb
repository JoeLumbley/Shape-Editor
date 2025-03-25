Public Class Form1
    Private points As New List(Of Point)()
    Private isDrawing As Boolean = False
    Private selectedPointIndex As Integer = -1
    Private Const handleSize As Integer = 15
    Private ScaleFactor As Double = 1.0 ' Adjust the scale factor as needed

    Private ShapePen As New Pen(Color.Black, 2)



    Private DrawingCenter As Point

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        Me.KeyPreview = True ' Enable KeyPreview to capture key events at the form level



        ' Set focus to the form itself
        Me.Focus()
        Me.ActiveControl = Nothing
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)



        'Dim centerX As Integer = Me.ClientSize.Width \ 4
        'Dim centerY As Integer = Me.ClientSize.Height \ 2

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

        e.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None

        ' Draw point handles
        For i As Integer = 0 To points.Count - 1 Step 2
            Dim point = points(i)
            Dim scaledPoint = New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
            If i = selectedPointIndex Then
                e.Graphics.FillRectangle(Brushes.Purple, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
            Else
                e.Graphics.FillRectangle(Brushes.Red, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
            End If
        Next

        '' Draw mirror point handles
        'For i As Integer = 1 To points.Count - 1 Step 2
        '    Dim point = points(i)
        '    Dim scaledPoint = New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
        '    If i = selectedPointIndex Then
        '        e.Graphics.FillRectangle(Brushes.Purple, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
        '    Else
        '        e.Graphics.FillRectangle(Brushes.Blue, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
        '    End If
        'Next
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        'Dim centerX As Integer = Me.ClientSize.Width \ 4
        'Dim centerY As Integer = Me.ClientSize.Height \ 2

        Dim adjustedLocation As New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

        If e.Button = MouseButtons.Left Then
            selectedPointIndex = GetPointIndexAtLocation(adjustedLocation)

            ' If no point was selected, add a new point
            If selectedPointIndex = -1 Then
                ' Add the point
                points.Add(adjustedLocation)

                ' Add the mirror point
                points.Add(New Point(adjustedLocation.X, -adjustedLocation.Y))

                selectedPointIndex = points.Count - 2
            End If

            isDrawing = True

            GeneratePointArrayText()

            Invalidate()
        End If
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        'Dim centerX As Integer = Me.ClientSize.Width \ 4
        'Dim centerY As Integer = Me.ClientSize.Height \ 2
        Dim adjustedLocation As New Point(CInt((e.Location.X - DrawingCenter.X) / ScaleFactor), CInt((e.Location.Y - DrawingCenter.Y) / ScaleFactor))

        If isDrawing AndAlso selectedPointIndex <> -1 Then
            points(selectedPointIndex) = adjustedLocation
            points(selectedPointIndex + 1) = New Point(adjustedLocation.X, -adjustedLocation.Y)
            GeneratePointArrayText()

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
        ElseIf e.KeyCode = Keys.Up Then
            'ScaleFactor += 0.01
            'GeneratePointArrayText()
            'Label1.Text = $"Scale Factor: {ScaleFactor:N2}"
            'TrackBar1.Value = CInt(ScaleFactor * 100)

            'Invalidate()
        ElseIf e.KeyCode = Keys.Down Then
            'ScaleFactor -= 0.01
            'If ScaleFactor < 1 Then ScaleFactor = 1 ' Prevent scale factor from going below 1
            'GeneratePointArrayText()
            'Label1.Text = $"Scale Factor: {ScaleFactor:N2}"

            'TrackBar1.Value = CInt(ScaleFactor * 100)
            'Invalidate()
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
        'e.SuppressKeyPress = True

        'e.Handled = True
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Function GetPointIndexAtLocation(location As Point) As Integer
        For i As Integer = 0 To points.Count - 1 Step 2
            Dim point As Point = points(i)
            Dim rect As New Rectangle(point.X - handleSize / 2, point.Y - handleSize / 2, handleSize, handleSize)
            If rect.Contains(location) Then
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

        '' Set focus to the form itself
        'Me.Focus()
        'Me.ActiveControl = Nothing

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


        DrawingCenter = New Point(ClientSize.Width \ 4, ClientSize.Height \ 2)



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
        HScrollBar1.Width = ClientSize.Width / 2

        Invalidate()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll







        ScaleFactor = TrackBar1.Value / 100.0

        Label1.Text = $"Scale Factor: {ScaleFactor:N2}"

        GeneratePointArrayText()
        Invalidate()
    End Sub

    Private Sub HScrollBar1_Scroll(sender As Object, e As ScrollEventArgs) Handles HScrollBar1.Scroll
        HScrollBar1.Minimum = (-ClientSize.Width \ 4) * 2
        HScrollBar1.Maximum = (ClientSize.Width \ 4) * 2


        'ClientSize.Width \ 4
        DrawingCenter = New Point(ClientSize.Width \ 4 - HScrollBar1.Value, ClientSize.Height \ 2)

        Invalidate()

    End Sub

End Class
