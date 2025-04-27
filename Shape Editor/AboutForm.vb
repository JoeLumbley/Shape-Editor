Imports Shape_Editor.Form1
Imports System.Runtime.InteropServices

Public Class AboutForm

    Private LinkColorDark As Color = Color.FromArgb(255, 28, 138, 224)
    Private LinkHoverColorDark As Color = Color.FromArgb(255, 255, 255, 255)
    Private ActiveLinkColorDark As Color = Color.Purple

    Private LinkColorLight As Color = Color.FromArgb(255, 28, 138, 224)
    Private LinkHoverColorLight As Color = Color.FromArgb(255, 0, 0, 0)
    Private ActiveLinkColorLight As Color = Color.Purple

    Private Sub AboutForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CenterToParent()

        Me.FormBorderStyle = FormBorderStyle.FixedDialog

        Text = "About - Shape Editor"

        If Form1.DarkMode Then

            DoDarkMode()

        Else

            DoLightMode()

        End If

    End Sub

    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click

        Me.Close()

    End Sub

    Private Sub GitHubLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles GitHubLink.LinkClicked

        OpenGitHubLink()

    End Sub

    Private Sub LinkLabel1_MouseHover(sender As Object, e As EventArgs) Handles GitHubLink.MouseHover

        ' Change the link color when hovered
        If Form1.DarkMode Then

            GitHubLink.LinkColor = LinkHoverColorDark

        Else

            GitHubLink.LinkColor = LinkHoverColorLight

        End If

    End Sub

    Private Sub LinkLabel1_MouseLeave(sender As Object, e As EventArgs) Handles GitHubLink.MouseLeave

        ' Reset the link color when not hovered
        If Form1.DarkMode Then

            GitHubLink.LinkColor = LinkColorDark

        Else

            GitHubLink.LinkColor = LinkColorLight

        End If

    End Sub

    Private Sub DoDarkMode()

        'set title color - dark mode
        DwmSetWindowAttribute(Handle, 20, 1, Marshal.SizeOf(GetType(Boolean)))

        ' Set the theme to dark mode
        SetWindowTheme(Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

        Me.BackColor = Form1.ControlColorDark

        GitHubLink.LinkColor = LinkColorDark
        GitHubLink.ActiveLinkColor = ActiveLinkColorDark

        ' Set the theme to dark mode
        SetWindowTheme(OKButton.Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

        OKButton.ForeColor = Color.White

        ' Set all labels themes to dark mode.
        For Each control In Controls

            If control.GetType Is GetType(Label) Then

                control.ForeColor = Color.White
                control.BackColor = Form1.ControlColorDark

            End If

        Next

    End Sub

    Private Sub DoLightMode()

        'set title color - light mode
        DwmSetWindowAttribute(Handle, 20, 0, Marshal.SizeOf(GetType(Boolean)))

        ' Set the theme to light mode
        SetWindowTheme(Handle, "Explorer", Nothing)
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

        BackColor = Form1.ControlColorLight

        GitHubLink.LinkColor = LinkColorLight
        GitHubLink.ActiveLinkColor = ActiveLinkColorLight

        ' Set the theme to light mode
        SetWindowTheme(OKButton.Handle, "Explorer", Nothing)
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 0, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(OKButton.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 0, Marshal.SizeOf(GetType(Integer)))

        OKButton.ForeColor = Color.Black

        ' Set all labels themes to light mode.
        For Each control In Controls

            If control.GetType Is GetType(Label) Then

                control.ForeColor = Color.Black
                control.BackColor = Form1.ControlColorLight

            End If

        Next

    End Sub

    Private Shared Sub OpenGitHubLink()
        ' This method opens the GitHub link in the default web browser.
        ' You can replace the URL with the desired GitHub link.

        ' The web adress of the repo to open.
        Dim url As String = "https://github.com/JoeLumbley/Shape-Editor"

        ' Check if the URL is valid
        If Not String.IsNullOrEmpty(url) Then

            Try

                Dim psi As New ProcessStartInfo() With {
                    .FileName = url,
                    .UseShellExecute = True
                }

                ' ShellExecute the URL
                Process.Start(psi)

            Catch ex As Exception

                'MessageBox.Show("Unable to open the link. Please check your default browser settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                MessageForm.Show("Can't open the repo. Please check your internet connection and your browser settings.", "Can't Open - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)

            End Try

        Else

            'MessageBox.Show("The URL is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            MessageForm.Show("Can't open the repo. The URL is invalid.", "Can't Open - Shape Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        End If

    End Sub

End Class