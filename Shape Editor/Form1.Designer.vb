<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        TextBox1 = New TextBox()
        TrackBar1 = New TrackBar()
        Label1 = New Label()
        HScrollBar1 = New HScrollBar()
        CType(TrackBar1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(473, 137)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.PlaceholderText = "Draw shape in the drawing area to the left."
        TextBox1.ReadOnly = True
        TextBox1.Size = New Size(155, 45)
        TextBox1.TabIndex = 0
        TextBox1.TabStop = False
        ' 
        ' TrackBar1
        ' 
        TrackBar1.Location = New Point(12, 388)
        TrackBar1.Maximum = 1000
        TrackBar1.Minimum = 100
        TrackBar1.Name = "TrackBar1"
        TrackBar1.Size = New Size(156, 69)
        TrackBar1.TabIndex = 1
        TrackBar1.Value = 100
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(56, 426)
        Label1.Name = "Label1"
        Label1.Size = New Size(63, 25)
        Label1.TabIndex = 2
        Label1.Text = "Label1"
        ' 
        ' HScrollBar1
        ' 
        HScrollBar1.Location = New Point(249, 340)
        HScrollBar1.Minimum = -100
        HScrollBar1.Name = "HScrollBar1"
        HScrollBar1.Size = New Size(120, 39)
        HScrollBar1.TabIndex = 3
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(889, 489)
        Controls.Add(HScrollBar1)
        Controls.Add(Label1)
        Controls.Add(TrackBar1)
        Controls.Add(TextBox1)
        KeyPreview = True
        Name = "Form1"
        Text = "Form1"
        CType(TrackBar1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents HScrollBar1 As HScrollBar

End Class
