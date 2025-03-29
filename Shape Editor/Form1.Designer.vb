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
        VScrollBar1 = New VScrollBar()
        Button1 = New Button()
        HideControlHandlesCheckBox = New CheckBox()
        FillShapeCheckBox = New CheckBox()
        DarkModeCheckBox = New CheckBox()
        MenuStrip1 = New MenuStrip()
        FileToolStripMenuItem = New ToolStripMenuItem()
        OpenToolStripMenuItem = New ToolStripMenuItem()
        NewToolStripMenuItem = New ToolStripMenuItem()
        SaveToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        Panel1 = New Panel()
        CType(TrackBar1, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(473, 137)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.PlaceholderText = "Draw shape in the drawing area to the left."
        TextBox1.ReadOnly = True
        TextBox1.ScrollBars = ScrollBars.Vertical
        TextBox1.Size = New Size(155, 45)
        TextBox1.TabIndex = 0
        TextBox1.TabStop = False
        ' 
        ' TrackBar1
        ' 
        TrackBar1.Location = New Point(12, 388)
        TrackBar1.Maximum = 6400
        TrackBar1.Minimum = 100
        TrackBar1.Name = "TrackBar1"
        TrackBar1.Size = New Size(156, 69)
        TrackBar1.TabIndex = 1
        TrackBar1.Value = 800
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
        ' VScrollBar1
        ' 
        VScrollBar1.Location = New Point(350, 174)
        VScrollBar1.Name = "VScrollBar1"
        VScrollBar1.Size = New Size(39, 180)
        VScrollBar1.TabIndex = 4
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(340, 340)
        Button1.Name = "Button1"
        Button1.Size = New Size(112, 34)
        Button1.TabIndex = 5
        Button1.TabStop = False
        Button1.Text = "C"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' HideControlHandlesCheckBox
        ' 
        HideControlHandlesCheckBox.AutoSize = True
        HideControlHandlesCheckBox.Location = New Point(350, 477)
        HideControlHandlesCheckBox.Name = "HideControlHandlesCheckBox"
        HideControlHandlesCheckBox.Size = New Size(144, 29)
        HideControlHandlesCheckBox.TabIndex = 6
        HideControlHandlesCheckBox.Text = "Hide Handles"
        HideControlHandlesCheckBox.UseVisualStyleBackColor = True
        ' 
        ' FillShapeCheckBox
        ' 
        FillShapeCheckBox.AutoSize = True
        FillShapeCheckBox.Location = New Point(30, 512)
        FillShapeCheckBox.Name = "FillShapeCheckBox"
        FillShapeCheckBox.Size = New Size(113, 29)
        FillShapeCheckBox.TabIndex = 7
        FillShapeCheckBox.Text = "Fill Shape"
        FillShapeCheckBox.UseVisualStyleBackColor = True
        ' 
        ' DarkModeCheckBox
        ' 
        DarkModeCheckBox.AutoSize = True
        DarkModeCheckBox.Location = New Point(173, 532)
        DarkModeCheckBox.Name = "DarkModeCheckBox"
        DarkModeCheckBox.Size = New Size(127, 29)
        DarkModeCheckBox.TabIndex = 8
        DarkModeCheckBox.Text = "Dark Mode"
        DarkModeCheckBox.UseVisualStyleBackColor = True
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.ImageScalingSize = New Size(24, 24)
        MenuStrip1.Items.AddRange(New ToolStripItem() {FileToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1258, 33)
        MenuStrip1.TabIndex = 9
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' FileToolStripMenuItem
        ' 
        FileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {OpenToolStripMenuItem, NewToolStripMenuItem, SaveToolStripMenuItem, AboutToolStripMenuItem, ExitToolStripMenuItem})
        FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        FileToolStripMenuItem.Size = New Size(54, 29)
        FileToolStripMenuItem.Text = "File"
        ' 
        ' OpenToolStripMenuItem
        ' 
        OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        OpenToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.O
        OpenToolStripMenuItem.Size = New Size(223, 34)
        OpenToolStripMenuItem.Text = "Open"
        ' 
        ' NewToolStripMenuItem
        ' 
        NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        NewToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.N
        NewToolStripMenuItem.Size = New Size(223, 34)
        NewToolStripMenuItem.Text = "New"
        ' 
        ' SaveToolStripMenuItem
        ' 
        SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        SaveToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.S
        SaveToolStripMenuItem.Size = New Size(223, 34)
        SaveToolStripMenuItem.Text = "Save"
        ' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(223, 34)
        AboutToolStripMenuItem.Text = "About"
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.Size = New Size(223, 34)
        ExitToolStripMenuItem.Text = "Exit"
        ' 
        ' Panel1
        ' 
        Panel1.Location = New Point(409, 288)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(300, 150)
        Panel1.TabIndex = 10
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1258, 664)
        Controls.Add(HScrollBar1)
        Controls.Add(VScrollBar1)
        Controls.Add(Button1)
        Controls.Add(Panel1)
        Controls.Add(DarkModeCheckBox)
        Controls.Add(FillShapeCheckBox)
        Controls.Add(HideControlHandlesCheckBox)
        Controls.Add(Label1)
        Controls.Add(TrackBar1)
        Controls.Add(TextBox1)
        Controls.Add(MenuStrip1)
        KeyPreview = True
        MainMenuStrip = MenuStrip1
        Name = "Form1"
        Text = "Form1"
        CType(TrackBar1, ComponentModel.ISupportInitialize).EndInit()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents HScrollBar1 As HScrollBar
    Friend WithEvents VScrollBar1 As VScrollBar
    Friend WithEvents Button1 As Button
    Friend WithEvents HideControlHandlesCheckBox As CheckBox
    Friend WithEvents FillShapeCheckBox As CheckBox
    Friend WithEvents DarkModeCheckBox As CheckBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Panel1 As Panel

End Class
