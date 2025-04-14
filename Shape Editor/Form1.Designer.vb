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
        CenterDrawingButton = New Button()
        HideControlHandlesCheckBox = New CheckBox()
        FillShapeCheckBox = New CheckBox()
        DarkModeCheckBox = New CheckBox()
        MenuStrip1 = New MenuStrip()
        FileToolStripMenuItem = New ToolStripMenuItem()
        NewToolStripMenuItem = New ToolStripMenuItem()
        OpenToolStripMenuItem = New ToolStripMenuItem()
        SaveToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        GroupBox1 = New GroupBox()
        Button2 = New Button()
        Button3 = New Button()
        CType(TrackBar1, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(473, 137)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.PlaceholderText = "< Draw shape in the area to the left."
        TextBox1.ReadOnly = True
        TextBox1.ScrollBars = ScrollBars.Vertical
        TextBox1.Size = New Size(382, 55)
        TextBox1.TabIndex = 0
        TextBox1.TabStop = False
        TextBox1.WordWrap = False
        ' 
        ' TrackBar1
        ' 
        TrackBar1.LargeChange = 100
        TrackBar1.Location = New Point(12, 388)
        TrackBar1.Maximum = 6400
        TrackBar1.Minimum = 100
        TrackBar1.Name = "TrackBar1"
        TrackBar1.Size = New Size(156, 69)
        TrackBar1.SmallChange = 10
        TrackBar1.TabIndex = 1
        TrackBar1.TickFrequency = 5
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
        ' CenterDrawingButton
        ' 
        CenterDrawingButton.Font = New Font("Segoe UI", 10.2089548F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        CenterDrawingButton.Location = New Point(-1, -1)
        CenterDrawingButton.Name = "CenterDrawingButton"
        CenterDrawingButton.Size = New Size(47, 43)
        CenterDrawingButton.TabIndex = 5
        CenterDrawingButton.TabStop = False
        CenterDrawingButton.UseVisualStyleBackColor = True
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
        FileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {NewToolStripMenuItem, OpenToolStripMenuItem, SaveToolStripMenuItem, AboutToolStripMenuItem, ExitToolStripMenuItem})
        FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        FileToolStripMenuItem.Size = New Size(54, 29)
        FileToolStripMenuItem.Text = "File"
        ' 
        ' NewToolStripMenuItem
        ' 
        NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        NewToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.N
        NewToolStripMenuItem.Size = New Size(270, 34)
        NewToolStripMenuItem.Text = "New"
        ' 
        ' OpenToolStripMenuItem
        ' 
        OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        OpenToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.O
        OpenToolStripMenuItem.Size = New Size(270, 34)
        OpenToolStripMenuItem.Text = "Open"
        ' 
        ' SaveToolStripMenuItem
        ' 
        SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        SaveToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.S
        SaveToolStripMenuItem.Size = New Size(270, 34)
        SaveToolStripMenuItem.Text = "Save As"
        ' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(270, 34)
        AboutToolStripMenuItem.Text = "About"
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.Size = New Size(270, 34)
        ExitToolStripMenuItem.Text = "Exit"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(CenterDrawingButton)
        GroupBox1.FlatStyle = FlatStyle.Flat
        GroupBox1.Location = New Point(559, 262)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(310, 152)
        GroupBox1.TabIndex = 11
        GroupBox1.TabStop = False
        ' 
        ' Button2
        ' 
        Button2.Font = New Font("Segoe UI", 10.2089548F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button2.Location = New Point(691, 213)
        Button2.Name = "Button2"
        Button2.Size = New Size(47, 43)
        Button2.TabIndex = 12
        Button2.TabStop = False
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(946, 208)
        Button3.Name = "Button3"
        Button3.Size = New Size(47, 43)
        Button3.TabIndex = 13
        Button3.TabStop = False
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1258, 664)
        Controls.Add(MenuStrip1)
        Controls.Add(Button3)
        Controls.Add(GroupBox1)
        Controls.Add(HScrollBar1)
        Controls.Add(VScrollBar1)
        Controls.Add(DarkModeCheckBox)
        Controls.Add(FillShapeCheckBox)
        Controls.Add(HideControlHandlesCheckBox)
        Controls.Add(Label1)
        Controls.Add(TrackBar1)
        Controls.Add(TextBox1)
        Controls.Add(Button2)
        KeyPreview = True
        MainMenuStrip = MenuStrip1
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form1"
        CType(TrackBar1, ComponentModel.ISupportInitialize).EndInit()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        GroupBox1.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents HScrollBar1 As HScrollBar
    Friend WithEvents VScrollBar1 As VScrollBar
    Friend WithEvents CenterDrawingButton As Button
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
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button

End Class
