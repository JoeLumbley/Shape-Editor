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
        MenuStrip1 = New MenuStrip()
        FileToolStripMenuItem = New ToolStripMenuItem()
        NewToolStripMenuItem = New ToolStripMenuItem()
        OpenToolStripMenuItem = New ToolStripMenuItem()
        SaveToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        OptionsToolStripMenuItem = New ToolStripMenuItem()
        HideHandlesToolStripMenuItem = New ToolStripMenuItem()
        FillShapeToolStripMenuItem = New ToolStripMenuItem()
        DarkModeToolStripMenuItem = New ToolStripMenuItem()
        GroupBox1 = New GroupBox()
        AddPointToolButton = New Button()
        MovePointToolButton = New Button()
        SubtractPointToolButton = New Button()
        Panel1 = New Panel()
        LanguageLabel = New Label()
        CopyLabel = New LinkLabel()
        CType(TrackBar1, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.BorderStyle = BorderStyle.FixedSingle
        TextBox1.Location = New Point(478, 171)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.PlaceholderText = "< Draw shape in the area to the left."
        TextBox1.ReadOnly = True
        TextBox1.ScrollBars = ScrollBars.Both
        TextBox1.Size = New Size(382, 55)
        TextBox1.TabIndex = 0
        TextBox1.TabStop = False
        TextBox1.WordWrap = False
        ' 
        ' TrackBar1
        ' 
        TrackBar1.LargeChange = 100
        TrackBar1.Location = New Point(13, 340)
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
        Label1.Location = New Point(67, 385)
        Label1.Name = "Label1"
        Label1.Size = New Size(63, 25)
        Label1.TabIndex = 2
        Label1.Text = "Label1"
        ' 
        ' HScrollBar1
        ' 
        HScrollBar1.Location = New Point(373, 384)
        HScrollBar1.Minimum = -100
        HScrollBar1.Name = "HScrollBar1"
        HScrollBar1.Size = New Size(120, 39)
        HScrollBar1.TabIndex = 3
        ' 
        ' VScrollBar1
        ' 
        VScrollBar1.Location = New Point(968, 171)
        VScrollBar1.Name = "VScrollBar1"
        VScrollBar1.Size = New Size(39, 180)
        VScrollBar1.TabIndex = 4
        ' 
        ' CenterDrawingButton
        ' 
        CenterDrawingButton.Font = New Font("Segoe UI", 10.2089548F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        CenterDrawingButton.Location = New Point(1, -1)
        CenterDrawingButton.Name = "CenterDrawingButton"
        CenterDrawingButton.Size = New Size(47, 43)
        CenterDrawingButton.TabIndex = 5
        CenterDrawingButton.TabStop = False
        CenterDrawingButton.UseVisualStyleBackColor = True
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.ImageScalingSize = New Size(24, 24)
        MenuStrip1.Items.AddRange(New ToolStripItem() {FileToolStripMenuItem, OptionsToolStripMenuItem})
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
        NewToolStripMenuItem.Size = New Size(237, 34)
        NewToolStripMenuItem.Text = "New"
        ' 
        ' OpenToolStripMenuItem
        ' 
        OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        OpenToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.O
        OpenToolStripMenuItem.Size = New Size(237, 34)
        OpenToolStripMenuItem.Text = "Open"
        ' 
        ' SaveToolStripMenuItem
        ' 
        SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        SaveToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.S
        SaveToolStripMenuItem.Size = New Size(237, 34)
        SaveToolStripMenuItem.Text = "Save As"
        ' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(237, 34)
        AboutToolStripMenuItem.Text = "About"
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.Q
        ExitToolStripMenuItem.Size = New Size(237, 34)
        ExitToolStripMenuItem.Text = "Exit"
        ' 
        ' OptionsToolStripMenuItem
        ' 
        OptionsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {HideHandlesToolStripMenuItem, FillShapeToolStripMenuItem, DarkModeToolStripMenuItem})
        OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        OptionsToolStripMenuItem.Size = New Size(92, 29)
        OptionsToolStripMenuItem.Text = "Options"
        ' 
        ' HideHandlesToolStripMenuItem
        ' 
        HideHandlesToolStripMenuItem.AutoSize = False
        HideHandlesToolStripMenuItem.Name = "HideHandlesToolStripMenuItem"
        HideHandlesToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+H"
        HideHandlesToolStripMenuItem.Size = New Size(284, 34)
        HideHandlesToolStripMenuItem.Text = "Hide Handles"
        ' 
        ' FillShapeToolStripMenuItem
        ' 
        FillShapeToolStripMenuItem.Name = "FillShapeToolStripMenuItem"
        FillShapeToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+F"
        FillShapeToolStripMenuItem.Size = New Size(284, 34)
        FillShapeToolStripMenuItem.Text = "Fill Shape"
        ' 
        ' DarkModeToolStripMenuItem
        ' 
        DarkModeToolStripMenuItem.Name = "DarkModeToolStripMenuItem"
        DarkModeToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+D"
        DarkModeToolStripMenuItem.Size = New Size(284, 34)
        DarkModeToolStripMenuItem.Text = "Dark Mode"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(CenterDrawingButton)
        GroupBox1.FlatStyle = FlatStyle.Flat
        GroupBox1.Location = New Point(524, 385)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(310, 152)
        GroupBox1.TabIndex = 11
        GroupBox1.TabStop = False
        ' 
        ' AddPointToolButton
        ' 
        AddPointToolButton.Font = New Font("Segoe UI", 10.2089548F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        AddPointToolButton.Location = New Point(691, 213)
        AddPointToolButton.Name = "AddPointToolButton"
        AddPointToolButton.Size = New Size(47, 43)
        AddPointToolButton.TabIndex = 12
        AddPointToolButton.TabStop = False
        AddPointToolButton.UseVisualStyleBackColor = True
        ' 
        ' MovePointToolButton
        ' 
        MovePointToolButton.Location = New Point(447, 171)
        MovePointToolButton.Name = "MovePointToolButton"
        MovePointToolButton.Size = New Size(47, 43)
        MovePointToolButton.TabIndex = 13
        MovePointToolButton.TabStop = False
        MovePointToolButton.UseVisualStyleBackColor = True
        ' 
        ' SubtractPointToolButton
        ' 
        SubtractPointToolButton.Location = New Point(447, 74)
        SubtractPointToolButton.Name = "SubtractPointToolButton"
        SubtractPointToolButton.Size = New Size(112, 34)
        SubtractPointToolButton.TabIndex = 14
        SubtractPointToolButton.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = SystemColors.ControlLight
        Panel1.Location = New Point(67, 139)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(310, 152)
        Panel1.TabIndex = 15
        ' 
        ' LanguageLabel
        ' 
        LanguageLabel.AutoSize = True
        LanguageLabel.BackColor = SystemColors.ControlLight
        LanguageLabel.Location = New Point(59, 60)
        LanguageLabel.Name = "LanguageLabel"
        LanguageLabel.Size = New Size(68, 25)
        LanguageLabel.TabIndex = 0
        LanguageLabel.Text = "VB.NET"
        ' 
        ' CopyLabel
        ' 
        CopyLabel.AutoSize = True
        CopyLabel.BackColor = SystemColors.ControlLight
        CopyLabel.LinkBehavior = LinkBehavior.HoverUnderline
        CopyLabel.LinkColor = Color.Black
        CopyLabel.Location = New Point(176, 67)
        CopyLabel.Name = "CopyLabel"
        CopyLabel.Size = New Size(54, 25)
        CopyLabel.TabIndex = 16
        CopyLabel.TabStop = True
        CopyLabel.Text = "Copy"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1258, 664)
        Controls.Add(CopyLabel)
        Controls.Add(LanguageLabel)
        Controls.Add(Panel1)
        Controls.Add(MenuStrip1)
        Controls.Add(TextBox1)
        Controls.Add(HScrollBar1)
        Controls.Add(Label1)
        Controls.Add(TrackBar1)
        Controls.Add(SubtractPointToolButton)
        Controls.Add(VScrollBar1)
        Controls.Add(MovePointToolButton)
        Controls.Add(GroupBox1)
        Controls.Add(AddPointToolButton)
        KeyPreview = True
        MainMenuStrip = MenuStrip1
        Name = "Form1"
        SizeGripStyle = SizeGripStyle.Show
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
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents AddPointToolButton As Button
    Friend WithEvents MovePointToolButton As Button
    Friend WithEvents SubtractPointToolButton As Button
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HideHandlesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FillShapeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DarkModeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Panel1 As Panel
    Friend WithEvents LanguageLabel As Label
    Friend WithEvents CopyLabel As LinkLabel

End Class
