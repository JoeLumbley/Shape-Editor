Imports Shape_Editor.Form1
Imports System.Runtime.InteropServices

Public Class MessageForm

    Public Overloads Shared Function Show(message As String, title As String, buttons As MessageBoxButtons, icon As MessageBoxIcon) As DialogResult

        ' Create a new instance of the MessageForm
        Dim form As New MessageForm()

        ' Set window title and message content
        form.Text = title
        form.MessageTextBox.Text = message

        ' Configure button visibility and labels
        Select Case buttons
            Case MessageBoxButtons.OK
                form.OKButton.Visible = True
                form.CancelButton.Visible = False

            Case MessageBoxButtons.OKCancel, MessageBoxButtons.YesNo
                form.OKButton.Visible = True
                form.CancelButton.Visible = True

                ' Adapt button text based on button type
                form.OKButton.Text = If(buttons = MessageBoxButtons.YesNo, "Yes", "OK")
                form.CancelButton.Text = If(buttons = MessageBoxButtons.YesNo, "No", "Cancel")
        End Select

        ' Assign the appropriate system icon
        Select Case icon
            Case MessageBoxIcon.Error
                form.IconPictureBox.Image = SystemIcons.Error.ToBitmap()
            Case MessageBoxIcon.Information
                form.IconPictureBox.Image = SystemIcons.Information.ToBitmap()
            Case MessageBoxIcon.Question
                form.IconPictureBox.Image = SystemIcons.Question.ToBitmap()
            Case MessageBoxIcon.Warning
                form.IconPictureBox.Image = SystemIcons.Warning.ToBitmap()
        End Select

        ' Display the message dialog and return user selection
        Return form.ShowDialog()

    End Function

    Private Sub MessageForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CenterToParent()

        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        MinimizeBox = False
        MaximizeBox = False

        If Form1.DarkMode Then

            DoDarkMode()

        Else

            DoLightMode()

        End If

    End Sub

    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Me.DialogResult = DialogResult.OK
        If OKButton.Text = "Yes" Then
            Me.DialogResult = DialogResult.Yes
        End If
        Me.Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
        If CancelButton.Text = "No" Then
            Me.DialogResult = DialogResult.No
        End If
        Me.Close()
    End Sub

    Private Sub DoDarkMode()

        'set title color - dark mode
        DwmSetWindowAttribute(Handle, 20, 1, Marshal.SizeOf(GetType(Boolean)))

        ' Set the theme to dark mode
        SetWindowTheme(Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

        Me.BackColor = Form1.ControlColorDark

        ' Set the theme to dark mode
        SetWindowTheme(OKButton.Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

        OKButton.ForeColor = Color.White

        ' Set the theme to dark mode
        SetWindowTheme(CancelButton.Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(CancelButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(CancelButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

        CancelButton.ForeColor = Color.White


        ' Set the theme to dark mode
        SetWindowTheme(MessageTextBox.Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(MessageTextBox.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(MessageTextBox.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

        MessageTextBox.ForeColor = Color.White
        MessageTextBox.BackColor = Form1.ControlColorDark
        MessageTextBox.BorderStyle = BorderStyle.None

    End Sub

    Private Sub DoLightMode()

        'set title color - light mode
        DwmSetWindowAttribute(Handle, 20, 0, Marshal.SizeOf(GetType(Boolean)))

        ' Set the theme to light mode
        SetWindowTheme(Handle, "Explorer", Nothing)
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

        BackColor = Form1.ControlColorLight

        ' Set the theme to light mode
        SetWindowTheme(OKButton.Handle, "Explorer", Nothing)
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

        OKButton.ForeColor = Color.Black

        SetWindowTheme(CancelButton.Handle, "Explorer", Nothing)
        DwmSetWindowAttribute(CancelButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(CancelButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

        CancelButton.ForeColor = Color.Black

        ' Set the theme to light mode
        SetWindowTheme(MessageTextBox.Handle, "Explorer", Nothing)
        DwmSetWindowAttribute(MessageTextBox.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(MessageTextBox.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

        MessageTextBox.ForeColor = Color.Black
        MessageTextBox.BackColor = Form1.ControlColorLight
        MessageTextBox.BorderStyle = BorderStyle.None

    End Sub

End Class

