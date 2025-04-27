<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
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
        GitHubLink = New LinkLabel()
        Label1 = New Label()
        OKButton = New Button()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        SuspendLayout()
        ' 
        ' GitHubLink
        ' 
        GitHubLink.AutoSize = True
        GitHubLink.Font = New Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        GitHubLink.LinkBehavior = LinkBehavior.HoverUnderline
        GitHubLink.Location = New Point(211, 301)
        GitHubLink.Name = "GitHubLink"
        GitHubLink.Size = New Size(376, 28)
        GitHubLink.TabIndex = 0
        GitHubLink.TabStop = True
        GitHubLink.Text = "GitHub.com/JoeLumbley/Shape-Editor"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(348, 189)
        Label1.Name = "Label1"
        Label1.Size = New Size(103, 25)
        Label1.TabIndex = 1
        Label1.Text = "MIT License"
        ' 
        ' OKButton
        ' 
        OKButton.Location = New Point(343, 379)
        OKButton.Name = "OKButton"
        OKButton.Size = New Size(112, 34)
        OKButton.TabIndex = 2
        OKButton.Text = "OK"
        OKButton.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(243, 230)
        Label2.Name = "Label2"
        Label2.Size = New Size(312, 25)
        Label2.TabIndex = 3
        Label2.Text = "Copyright(c) 2025 Joseph W. Lumbley"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(204, 93)
        Label3.Name = "Label3"
        Label3.Size = New Size(390, 25)
        Label3.TabIndex = 4
        Label3.Text = "A simple editor that allows you to draw a shape"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(152, 133)
        Label4.Name = "Label4"
        Label4.Size = New Size(494, 25)
        Label4.TabIndex = 5
        Label4.Text = "and generates the corresponding point array in VB.NET code."
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(285, 30)
        Label5.Name = "Label5"
        Label5.Size = New Size(229, 48)
        Label5.TabIndex = 6
        Label5.Text = "Shape Editor"
        ' 
        ' AboutForm
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(OKButton)
        Controls.Add(Label1)
        Controls.Add(GitHubLink)
        Name = "AboutForm"
        Text = "AboutForm"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents GitHubLink As LinkLabel
    Friend WithEvents Label1 As Label
    Friend WithEvents OKButton As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
End Class
