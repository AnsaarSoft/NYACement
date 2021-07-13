namespace mfmFFS
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.npMain = new DevComponents.DotNetBar.NavigationPane();
            this.navigationPanePanel1 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.tvMain = new DevComponents.AdvTree.AdvTree();
            this.imglstTreeView = new System.Windows.Forms.ImageList(this.components);
            this.InvisibleLines = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle3 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.tbMain = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.pnlHead = new Telerik.WinControls.UI.RadPanel();
            this.txtFullText2 = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.txtCWeight2 = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.txtFullText = new Telerik.WinControls.UI.RadTextBox();
            this.txtCWeight = new Telerik.WinControls.UI.RadTextBox();
            this.btnLogout = new Telerik.WinControls.UI.RadButton();
            this.txtLoginDisplay = new Telerik.WinControls.UI.RadTextBox();
            this.txtNameDisplay = new Telerik.WinControls.UI.RadTextBox();
            this.txtUserCodeDisplay = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ssMain = new Telerik.WinControls.UI.RadStatusStrip();
            this.lblAppStatus = new Telerik.WinControls.UI.RadLabelElement();
            this.pnlBase = new Telerik.WinControls.UI.RadPanel();
            this.tmrIntegration = new System.Windows.Forms.Timer(this.components);
            this.tmrSync = new System.Windows.Forms.Timer(this.components);
            this.tmrAlreadyReading = new System.Windows.Forms.Timer(this.components);
            this.tmrCamFront = new System.Windows.Forms.Timer(this.components);
            this.tmrAlreadyReading2 = new System.Windows.Forms.Timer(this.components);
            this.tmrCamFront2 = new System.Windows.Forms.Timer(this.components);
            this.npMain.SuspendLayout();
            this.navigationPanePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMain)).BeginInit();
            this.tbMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlHead)).BeginInit();
            this.pnlHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullText2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCWeight2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNameDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserCodeDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBase)).BeginInit();
            this.pnlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // npMain
            // 
            this.npMain.CanCollapse = true;
            this.npMain.ConfigureAddRemoveVisible = false;
            this.npMain.ConfigureItemVisible = false;
            this.npMain.ConfigureNavOptionsVisible = false;
            this.npMain.ConfigureShowHideVisible = false;
            this.npMain.Controls.Add(this.navigationPanePanel1);
            this.npMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.npMain.ItemPaddingBottom = 2;
            this.npMain.ItemPaddingTop = 2;
            this.npMain.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.npMain.Location = new System.Drawing.Point(0, 0);
            this.npMain.Name = "npMain";
            this.npMain.Padding = new System.Windows.Forms.Padding(1);
            this.npMain.Size = new System.Drawing.Size(246, 670);
            this.npMain.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.npMain.TabIndex = 1;
            // 
            // 
            // 
            this.npMain.TitlePanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.npMain.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.npMain.TitlePanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.npMain.TitlePanel.Location = new System.Drawing.Point(1, 1);
            this.npMain.TitlePanel.Name = "panelTitle";
            this.npMain.TitlePanel.Size = new System.Drawing.Size(244, 24);
            this.npMain.TitlePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.npMain.TitlePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.npMain.TitlePanel.Style.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.npMain.TitlePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.npMain.TitlePanel.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.npMain.TitlePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.npMain.TitlePanel.Style.GradientAngle = 90;
            this.npMain.TitlePanel.Style.MarginLeft = 4;
            this.npMain.TitlePanel.TabIndex = 0;
            this.npMain.TitlePanel.Text = "Main Menu";
            // 
            // navigationPanePanel1
            // 
            this.navigationPanePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.navigationPanePanel1.Controls.Add(this.tvMain);
            this.navigationPanePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel1.Location = new System.Drawing.Point(1, 25);
            this.navigationPanePanel1.Name = "navigationPanePanel1";
            this.navigationPanePanel1.ParentItem = this.buttonItem1;
            this.navigationPanePanel1.Size = new System.Drawing.Size(244, 612);
            this.navigationPanePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.navigationPanePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.navigationPanePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.navigationPanePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.navigationPanePanel1.Style.GradientAngle = 90;
            this.navigationPanePanel1.TabIndex = 2;
            // 
            // tvMain
            // 
            this.tvMain.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.tvMain.AllowDrop = true;
            this.tvMain.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.tvMain.BackgroundStyle.Class = "TreeBorderKey";
            this.tvMain.ColumnsVisible = false;
            this.tvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMain.ExpandBorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemDisabledBackground;
            this.tvMain.ExpandButtonType = DevComponents.AdvTree.eExpandButtonType.Image;
            this.tvMain.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.MenuBackground2;
            this.tvMain.GridColumnLines = false;
            this.tvMain.HotTracking = true;
            this.tvMain.ImageList = this.imglstTreeView;
            this.tvMain.Location = new System.Drawing.Point(0, 0);
            this.tvMain.Name = "tvMain";
            this.tvMain.NodesConnector = this.InvisibleLines;
            this.tvMain.NodeSpacing = 0;
            this.tvMain.NodeStyle = this.elementStyle1;
            this.tvMain.NodeStyleMouseOver = this.elementStyle3;
            this.tvMain.NodeStyleSelected = this.elementStyle2;
            this.tvMain.PathSeparator = ";";
            this.tvMain.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.FullRowSelect;
            this.tvMain.Size = new System.Drawing.Size(244, 612);
            this.tvMain.Styles.Add(this.elementStyle1);
            this.tvMain.Styles.Add(this.elementStyle2);
            this.tvMain.Styles.Add(this.elementStyle3);
            this.tvMain.TabIndex = 0;
            this.tvMain.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.tvMain_NodeClick);
            // 
            // imglstTreeView
            // 
            this.imglstTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstTreeView.ImageStream")));
            this.imglstTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstTreeView.Images.SetKeyName(0, "richtext.png");
            this.imglstTreeView.Images.SetKeyName(1, "kdeprint_testprinter.png");
            // 
            // InvisibleLines
            // 
            this.InvisibleLines.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.InvisibleLines.LineColor = System.Drawing.Color.Transparent;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle3
            // 
            this.elementStyle3.BackColor = System.Drawing.Color.White;
            this.elementStyle3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(240)))));
            this.elementStyle3.BackColorGradientAngle = 90;
            this.elementStyle3.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderBottomWidth = 1;
            this.elementStyle3.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle3.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderLeftWidth = 1;
            this.elementStyle3.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderRightWidth = 1;
            this.elementStyle3.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderTopWidth = 1;
            this.elementStyle3.CornerDiameter = 4;
            this.elementStyle3.Description = "Gray";
            this.elementStyle3.Name = "elementStyle3";
            this.elementStyle3.PaddingBottom = 1;
            this.elementStyle3.PaddingLeft = 1;
            this.elementStyle3.PaddingRight = 1;
            this.elementStyle3.PaddingTop = 1;
            this.elementStyle3.TextColor = System.Drawing.Color.Black;
            // 
            // elementStyle2
            // 
            this.elementStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(236)))), ((int)(((byte)(243)))));
            this.elementStyle2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(187)))), ((int)(((byte)(210)))));
            this.elementStyle2.BackColorGradientAngle = 90;
            this.elementStyle2.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderBottomWidth = 1;
            this.elementStyle2.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle2.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderLeftWidth = 1;
            this.elementStyle2.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderRightWidth = 1;
            this.elementStyle2.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderTopWidth = 1;
            this.elementStyle2.CornerDiameter = 4;
            this.elementStyle2.Description = "BlueMist";
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.PaddingBottom = 1;
            this.elementStyle2.PaddingLeft = 1;
            this.elementStyle2.PaddingRight = 1;
            this.elementStyle2.PaddingTop = 1;
            this.elementStyle2.TextColor = System.Drawing.Color.Black;
            // 
            // buttonItem1
            // 
            this.buttonItem1.Checked = true;
            this.buttonItem1.Image = ((System.Drawing.Image)(resources.GetObject("buttonItem1.Image")));
            this.buttonItem1.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.OptionGroup = "navBar";
            this.buttonItem1.Text = "Main Menu";
            // 
            // tbMain
            // 
            this.tbMain.AutoCloseTabs = true;
            this.tbMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.tbMain.CanReorderTabs = true;
            this.tbMain.CloseButtonOnTabsVisible = true;
            this.tbMain.CloseButtonPosition = DevComponents.DotNetBar.eTabCloseButtonPosition.Right;
            this.tbMain.CloseButtonVisible = true;
            this.tbMain.Controls.Add(this.tabControlPanel2);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedTabFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.tbMain.SelectedTabIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(746, 544);
            this.tbMain.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Dock;
            this.tbMain.TabIndex = 2;
            this.tbMain.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tbMain.Tabs.Add(this.tabItem2);
            this.tbMain.Text = "tabControl1";
            this.tbMain.SelectedTabChanged += new DevComponents.DotNetBar.TabStrip.SelectedTabChangedEventHandler(this.tbMain_SelectedTabChanged);
            this.tbMain.TabItemClose += new DevComponents.DotNetBar.TabStrip.UserActionEventHandler(this.tbMain_TabItemClose);
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 24);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(746, 520);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right) 
            | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItem2;
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel2;
            this.tabItem2.CloseButtonVisible = false;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "Home";
            // 
            // pnlHead
            // 
            this.pnlHead.Controls.Add(this.txtFullText2);
            this.pnlHead.Controls.Add(this.radLabel5);
            this.pnlHead.Controls.Add(this.txtCWeight2);
            this.pnlHead.Controls.Add(this.radLabel4);
            this.pnlHead.Controls.Add(this.txtFullText);
            this.pnlHead.Controls.Add(this.txtCWeight);
            this.pnlHead.Controls.Add(this.btnLogout);
            this.pnlHead.Controls.Add(this.txtLoginDisplay);
            this.pnlHead.Controls.Add(this.txtNameDisplay);
            this.pnlHead.Controls.Add(this.txtUserCodeDisplay);
            this.pnlHead.Controls.Add(this.radLabel3);
            this.pnlHead.Controls.Add(this.radLabel2);
            this.pnlHead.Controls.Add(this.radLabel1);
            this.pnlHead.Controls.Add(this.pictureBox1);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Location = new System.Drawing.Point(246, 0);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(746, 100);
            this.pnlHead.TabIndex = 3;
            // 
            // txtFullText2
            // 
            this.txtFullText2.Location = new System.Drawing.Point(711, 32);
            this.txtFullText2.Name = "txtFullText2";
            this.txtFullText2.Size = new System.Drawing.Size(23, 20);
            this.txtFullText2.TabIndex = 146;
            this.txtFullText2.Visible = false;
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(418, 49);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(54, 18);
            this.radLabel5.TabIndex = 144;
            this.radLabel5.Text = "Bridge 02";
            // 
            // txtCWeight2
            // 
            this.txtCWeight2.Location = new System.Drawing.Point(478, 47);
            this.txtCWeight2.Name = "txtCWeight2";
            this.txtCWeight2.Size = new System.Drawing.Size(129, 20);
            this.txtCWeight2.TabIndex = 143;
            this.txtCWeight2.TextChanged += new System.EventHandler(this.txtCWeight2_TextChanged);
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(418, 25);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(54, 18);
            this.radLabel4.TabIndex = 142;
            this.radLabel4.Text = "Bridge 01";
            // 
            // txtFullText
            // 
            this.txtFullText.Location = new System.Drawing.Point(711, 7);
            this.txtFullText.Name = "txtFullText";
            this.txtFullText.Size = new System.Drawing.Size(23, 20);
            this.txtFullText.TabIndex = 141;
            this.txtFullText.Visible = false;
            // 
            // txtCWeight
            // 
            this.txtCWeight.Location = new System.Drawing.Point(478, 23);
            this.txtCWeight.Name = "txtCWeight";
            this.txtCWeight.Size = new System.Drawing.Size(129, 20);
            this.txtCWeight.TabIndex = 140;
            this.txtCWeight.TextChanged += new System.EventHandler(this.txtCWeight_TextChanged);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(633, 67);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(110, 24);
            this.btnLogout.TabIndex = 40;
            this.btnLogout.Text = "&LogOut";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // txtLoginDisplay
            // 
            this.txtLoginDisplay.Location = new System.Drawing.Point(177, 23);
            this.txtLoginDisplay.Name = "txtLoginDisplay";
            this.txtLoginDisplay.ReadOnly = true;
            this.txtLoginDisplay.Size = new System.Drawing.Size(134, 20);
            this.txtLoginDisplay.TabIndex = 6;
            // 
            // txtNameDisplay
            // 
            this.txtNameDisplay.Location = new System.Drawing.Point(177, 71);
            this.txtNameDisplay.Name = "txtNameDisplay";
            this.txtNameDisplay.ReadOnly = true;
            this.txtNameDisplay.Size = new System.Drawing.Size(229, 20);
            this.txtNameDisplay.TabIndex = 5;
            // 
            // txtUserCodeDisplay
            // 
            this.txtUserCodeDisplay.Location = new System.Drawing.Point(177, 47);
            this.txtUserCodeDisplay.Name = "txtUserCodeDisplay";
            this.txtUserCodeDisplay.ReadOnly = true;
            this.txtUserCodeDisplay.Size = new System.Drawing.Size(134, 20);
            this.txtUserCodeDisplay.TabIndex = 4;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(111, 49);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(58, 18);
            this.radLabel3.TabIndex = 3;
            this.radLabel3.Text = "User Code";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(121, 25);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(48, 18);
            this.radLabel2.TabIndex = 2;
            this.radLabel2.Text = "Login @";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(133, 73);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(36, 18);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Name";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::mfmWeighment.Properties.Resources.abacuslogopic;
            this.pictureBox1.Location = new System.Drawing.Point(7, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(86, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.lblAppStatus});
            this.ssMain.Location = new System.Drawing.Point(246, 644);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(746, 26);
            this.ssMain.TabIndex = 4;
            this.ssMain.Text = "radStatusStrip1";
            // 
            // lblAppStatus
            // 
            this.lblAppStatus.Name = "lblAppStatus";
            this.ssMain.SetSpring(this.lblAppStatus, false);
            this.lblAppStatus.Text = "Status : ...";
            this.lblAppStatus.TextWrap = true;
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.tbMain);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBase.Location = new System.Drawing.Point(246, 100);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(746, 544);
            this.pnlBase.TabIndex = 5;
            this.pnlBase.Text = "temp holder";
            // 
            // tmrIntegration
            // 
            this.tmrIntegration.Enabled = true;
            this.tmrIntegration.Interval = 180000;
            this.tmrIntegration.Tick += new System.EventHandler(this.tmrIntegration_Tick);
            // 
            // tmrSync
            // 
            this.tmrSync.Interval = 360000;
            this.tmrSync.Tick += new System.EventHandler(this.tmrSync_Tick);
            // 
            // tmrAlreadyReading
            // 
            this.tmrAlreadyReading.Enabled = true;
            this.tmrAlreadyReading.Interval = 1000;
            // 
            // tmrAlreadyReading2
            // 
            this.tmrAlreadyReading2.Enabled = true;
            this.tmrAlreadyReading2.Interval = 1000;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 670);
            this.Controls.Add(this.pnlBase);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.pnlHead);
            this.Controls.Add(this.npMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Weighment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.npMain.ResumeLayout(false);
            this.navigationPanePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbMain)).EndInit();
            this.tbMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlHead)).EndInit();
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullText2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCWeight2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNameDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserCodeDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBase)).EndInit();
            this.pnlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.NavigationPane npMain;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.AdvTree.AdvTree tvMain;
        private DevComponents.AdvTree.NodeConnector InvisibleLines;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private Telerik.WinControls.UI.RadPanel pnlHead;
        private Telerik.WinControls.UI.RadStatusStrip ssMain;
        private Telerik.WinControls.UI.RadPanel pnlBase;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private Telerik.WinControls.UI.RadTextBox txtLoginDisplay;
        private Telerik.WinControls.UI.RadTextBox txtNameDisplay;
        private Telerik.WinControls.UI.RadTextBox txtUserCodeDisplay;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        public DevComponents.DotNetBar.TabControl tbMain;
        private Telerik.WinControls.UI.RadLabelElement lblAppStatus;
        private System.Windows.Forms.ImageList imglstTreeView;
        private DevComponents.DotNetBar.ElementStyle elementStyle3;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private System.Windows.Forms.Timer tmrIntegration;
        private System.Windows.Forms.Timer tmrSync;
        private Telerik.WinControls.UI.RadButton btnLogout;
        private Telerik.WinControls.UI.RadTextBox txtFullText;
        private System.Windows.Forms.Timer tmrAlreadyReading;
        private System.Windows.Forms.Timer tmrCamFront;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadTextBox txtFullText2;
        private System.Windows.Forms.Timer tmrAlreadyReading2;
        private System.Windows.Forms.Timer tmrCamFront2;
        public Telerik.WinControls.UI.RadTextBox txtCWeight;
        public Telerik.WinControls.UI.RadTextBox txtCWeight2;
    }
}

