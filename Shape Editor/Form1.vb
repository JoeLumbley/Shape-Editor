Public Class Form1
    Private points As New List(Of Point)()
    Private isDrawing As Boolean = False
    Private selectedPointIndex As Integer = -1
    Private Const handleSize As Integer = 15
    Private ScaleFactor As Double = 1.0 ' Adjust the scale factor as needed

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        Me.KeyPreview = True ' Enable KeyPreview to capture key events at the form level
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim centerX As Integer = Me.ClientSize.Width \ 4
        Dim centerY As Integer = Me.ClientSize.Height \ 2

        e.Graphics.TranslateTransform(centerX, centerY)

        ' Draw intersecting lines at the origin
        e.Graphics.DrawLine(Pens.Black, -5, 0, 5, 0) ' Horizontal line
        e.Graphics.DrawLine(Pens.Black, 0, -5, 0, 5) ' Vertical line

        If points.Count > 1 Then
            Dim scaledPoints = points.Select(Function(p) New Point(CInt(p.X * ScaleFactor), CInt(p.Y * ScaleFactor))).ToArray()
            e.Graphics.DrawPolygon(Pens.Black, scaledPoints)
        End If

        ' Draw point handles
        For Each point As Point In points
            Dim scaledPoint = New Point(CInt(point.X * ScaleFactor), CInt(point.Y * ScaleFactor))
            e.Graphics.FillRectangle(Brushes.Red, CInt(scaledPoint.X - handleSize / 2), CInt(scaledPoint.Y - handleSize / 2), handleSize, handleSize)
        Next
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        Dim centerX As Integer = Me.ClientSize.Width \ 4
        Dim centerY As Integer = Me.ClientSize.Height \ 2
        Dim adjustedLocation As New Point(CInt((e.Location.X - centerX) / ScaleFactor), CInt((e.Location.Y - centerY) / ScaleFactor))

        If e.Button = MouseButtons.Left Then
            selectedPointIndex = GetPointIndexAtLocation(adjustedLocation)
            If selectedPointIndex = -1 Then
                points.Add(adjustedLocation)
                selectedPointIndex = points.Count - 1
            End If
            isDrawing = True
            GeneratePointArrayText()

            Invalidate()
        End If
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        Dim centerX As Integer = Me.ClientSize.Width \ 4
        Dim centerY As Integer = Me.ClientSize.Height \ 2
        Dim adjustedLocation As New Point(CInt((e.Location.X - centerX) / ScaleFactor), CInt((e.Location.Y - centerY) / ScaleFactor))

        If isDrawing AndAlso selectedPointIndex <> -1 Then
            points(selectedPointIndex) = adjustedLocation
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
            Invalidate()
            GeneratePointArrayText()
        ElseIf e.KeyCode = Keys.Up Then
            'ScaleFactor += 0.1
            'GeneratePointArrayText()
            'Invalidate()
        ElseIf e.KeyCode = Keys.Down Then
            'ScaleFactor -= 0.1
            'If ScaleFactor < 0.1 Then ScaleFactor = 0.1 ' Prevent scale factor from going below 0.1
            'GeneratePointArrayText()
            'Invalidate()
        End If
    End Sub

    Private Function GetPointIndexAtLocation(location As Point) As Integer
        For i As Integer = 0 To points.Count - 1
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
        sb.AppendLine("Body = {")
        For i As Integer = 0 To points.Count - 1
            Dim point As Point = points(i)
            sb.AppendLine($"    New Point(CInt({point.X} * ScaleFactor), CInt({point.Y} * ScaleFactor)),")
        Next
        sb.AppendLine("}")
        Dim result As String = sb.ToString()
        ' Display or use the result as needed
        'MessageBox.Show(result)

        TextBox1.Text = result
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        TextBox1.Top = ClientRectangle.Top
        TextBox1.Left = ClientSize.Width / 2
        TextBox1.Width = ClientSize.Width / 2
        TextBox1.Height = ClientSize.Height
        Invalidate()
    End Sub
End Class


