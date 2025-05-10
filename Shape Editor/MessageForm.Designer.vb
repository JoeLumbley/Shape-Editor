<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MessageForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        IconPictureBox = New PictureBox()
        OKButton = New Button()
        CancelBut = New Button()
        Label1 = New Label()
        CType(IconPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' IconPictureBox
        ' 
        IconPictureBox.Location = New Point(32, 22)
        IconPictureBox.Name = "IconPictureBox"
        IconPictureBox.Size = New Size(53, 53)
        IconPictureBox.TabIndex = 0
        IconPictureBox.TabStop = False
        ' 
        ' OKButton
        ' 
        OKButton.Location = New Point(148, 144)
        OKButton.Name = "OKButton"
        OKButton.Size = New Size(101, 31)
        OKButton.TabIndex = 1
        OKButton.Text = "OK"
        OKButton.UseVisualStyleBackColor = True
        ' 
        ' CancelBut
        ' 
        CancelBut.Location = New Point(269, 144)
        CancelBut.Name = "CancelBut"
        CancelBut.Size = New Size(101, 31)
        CancelBut.TabIndex = 2
        CancelBut.Text = "Cancel"
        CancelBut.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.Location = New Point(104, 22)
        Label1.Name = "Label1"
        Label1.Size = New Size(277, 83)
        Label1.TabIndex = 5
        Label1.Text = "Label1"
        ' 
        ' MessageForm
        ' 
        AutoScaleDimensions = New SizeF(9.0F, 23.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(393, 200)
        Controls.Add(Label1)
        Controls.Add(CancelBut)
        Controls.Add(OKButton)
        Controls.Add(IconPictureBox)
        Name = "MessageForm"
        Text = "MessageForm"
        CType(IconPictureBox, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents IconPictureBox As PictureBox
    Friend WithEvents OKButton As Button
    Friend WithEvents CancelBut As Button
    Friend WithEvents Label1 As Label
End Class
