Imports Shape_Editor.Form1
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class ApplyingThemeForm
    Private Sub ApplyingThemeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CenterToParent()

        Me.FormBorderStyle = FormBorderStyle.FixedDialog

        Text = "Shape Editor"

        If Form1.DarkMode Then

            DoDarkMode()

        Else

            DoLightMode()

        End If

        Close()

    End Sub


    Private Sub DoDarkMode()

        'set title color - dark mode
        DwmSetWindowAttribute(Handle, 20, 1, Marshal.SizeOf(GetType(Boolean)))

        ' Set the theme to dark mode
        SetWindowTheme(Handle, "DarkMode_Explorer", Nothing)
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, 1, Marshal.SizeOf(GetType(Integer)))
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, 1, Marshal.SizeOf(GetType(Integer)))

        Me.BackColor = Form1.ControlColorDark

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

        ' Set all labels themes to light mode.
        For Each control In Controls

            If control.GetType Is GetType(Label) Then

                control.ForeColor = Color.Black
                control.BackColor = Form1.ControlColorLight

            End If

        Next

    End Sub

End Class