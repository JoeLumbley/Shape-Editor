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
        CancelButt = New Button()
        MessageTextBox = New TextBox()
        CType(IconPictureBox, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' IconPictureBox
        ' 
        IconPictureBox.Location = New Point(35, 24)
        IconPictureBox.Name = "IconPictureBox"
        IconPictureBox.Size = New Size(59, 58)
        IconPictureBox.TabIndex = 0
        IconPictureBox.TabStop = False
        ' 
        ' OKButton
        ' 
        OKButton.Location = New Point(165, 157)
        OKButton.Name = "OKButton"
        OKButton.Size = New Size(112, 34)
        OKButton.TabIndex = 1
        OKButton.Text = "OK"
        OKButton.UseVisualStyleBackColor = True
        ' 
        ' CancelButt
        ' 
        CancelButt.Location = New Point(299, 157)
        CancelButt.Name = "CancelButt"
        CancelButt.Size = New Size(112, 34)
        CancelButt.TabIndex = 2
        CancelButt.Text = "Cancel"
        CancelButt.UseVisualStyleBackColor = True
        ' 
        ' MessageTextBox
        ' 
        MessageTextBox.BackColor = SystemColors.Control
        MessageTextBox.BorderStyle = BorderStyle.None
        MessageTextBox.Location = New Point(103, 33)
        MessageTextBox.Multiline = True
        MessageTextBox.Name = "MessageTextBox"
        MessageTextBox.Size = New Size(308, 90)
        MessageTextBox.TabIndex = 4
        MessageTextBox.Text = "MessageTextBox"
        ' 
        ' MessageForm
        ' 
        AutoScaleDimensions = New SizeF(10.0F, 25.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(437, 214)
        Controls.Add(MessageTextBox)
        Controls.Add(CancelButt)
        Controls.Add(OKButton)
        Controls.Add(IconPictureBox)
        Name = "MessageForm"
        Text = "MessageForm"
        CType(IconPictureBox, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents IconPictureBox As PictureBox
    Friend WithEvents OKButton As Button
    Friend WithEvents CancelButt As Button
    Friend WithEvents MessageTextBox As TextBox
End Class
