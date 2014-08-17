Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Reflection
Imports System.Xml
Imports System.Data.OleDb
Imports System.Threading

Public Class Preferences
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents conf_main_tab As System.Windows.Forms.TabControl
    Friend WithEvents runtime_tab As System.Windows.Forms.TabPage
    Friend WithEvents scan_opts_tab As System.Windows.Forms.TabPage
    Friend WithEvents regex_tab As System.Windows.Forms.TabPage
    Friend WithEvents log_opts_tab As System.Windows.Forms.TabPage
    Friend WithEvents misc_opts_tab As System.Windows.Forms.TabPage
    Friend WithEvents save_opts_button As System.Windows.Forms.Button
    Friend WithEvents reset_defs_button As System.Windows.Forms.Button
    Friend WithEvents cancel_opts_button As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents runtime_mode_tab As System.Windows.Forms.TabPage
    Friend WithEvents runtime_proc_opts_tab As System.Windows.Forms.TabPage
    Friend WithEvents scan_method_opts_tab As System.Windows.Forms.TabPage
    Friend WithEvents search_obj_box As System.Windows.Forms.GroupBox
    Friend WithEvents disk_obj_radio As System.Windows.Forms.RadioButton
    Friend WithEvents web_obj_radio As System.Windows.Forms.RadioButton
    Friend WithEvents unalloc_obj_radio As System.Windows.Forms.RadioButton
    Friend WithEvents proc_opts_box As System.Windows.Forms.GroupBox
    Friend WithEvents completion_opts_box As System.Windows.Forms.GroupBox
    Friend WithEvents low_prio_button As System.Windows.Forms.RadioButton
    Friend WithEvents normal_prio_button As System.Windows.Forms.RadioButton
    Friend WithEvents high_prio_button As System.Windows.Forms.RadioButton
    Friend WithEvents highest_prio_button As System.Windows.Forms.RadioButton
    Friend WithEvents when_running_box As System.Windows.Forms.GroupBox
    Friend WithEvents when_finished_box As System.Windows.Forms.GroupBox
    Friend WithEvents minimize_when_running As System.Windows.Forms.CheckBox
    Friend WithEvents restore_window_radio As System.Windows.Forms.RadioButton
    Friend WithEvents exit_radio As System.Windows.Forms.RadioButton
    Friend WithEvents view_log_radio As System.Windows.Forms.RadioButton
    Friend WithEvents match_behavior_box As System.Windows.Forms.GroupBox
    Friend WithEvents fastmatch_box As System.Windows.Forms.CheckBox
    Friend WithEvents scanning_behavior_box As System.Windows.Forms.GroupBox
    Friend WithEvents incremental_box As System.Windows.Forms.CheckBox
    Friend WithEvents checkpoint_box As System.Windows.Forms.CheckBox
    Friend WithEvents clear_incremental As System.Windows.Forms.Button
    Friend WithEvents clear_checkpoints As System.Windows.Forms.Button
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents disk_scan_opts_tab As System.Windows.Forms.TabPage
    Friend WithEvents web_scan_opt_tab As System.Windows.Forms.TabPage
    Friend WithEvents hidden_scan_opt_tab As System.Windows.Forms.TabPage
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents scan_depth_box As System.Windows.Forms.NumericUpDown
    Friend WithEvents scan_depth_label As System.Windows.Forms.Label
    Friend WithEvents regex_tab_control As System.Windows.Forms.TabControl
    Friend WithEvents system_regex_tab As System.Windows.Forms.TabPage
    Friend WithEvents custom_regex_tab As System.Windows.Forms.TabPage
    Friend WithEvents log_options_tab As System.Windows.Forms.TabControl
    Friend WithEvents log2file_options_tab As System.Windows.Forms.TabPage
    Friend WithEvents log2syslog_options_tab As System.Windows.Forms.TabPage
    Friend WithEvents log2evt_options_tab As System.Windows.Forms.TabPage
    Friend WithEvents startdir_box As System.Windows.Forms.TextBox
    Friend WithEvents startdir_select_button As System.Windows.Forms.Button
    Friend WithEvents file_select As System.Windows.Forms.GroupBox
    Friend WithEvents skip_exts_button As System.Windows.Forms.Button
    Friend WithEvents keep_exts_button As System.Windows.Forms.Button
    Friend WithEvents skip_path_button As System.Windows.Forms.Button
    Friend WithEvents recurse_box As System.Windows.Forms.CheckBox
    Friend WithEvents local_drives_box As System.Windows.Forms.CheckBox
    Friend WithEvents reset_atimes_box As System.Windows.Forms.CheckBox
    Friend WithEvents date_picker_label As System.Windows.Forms.Label
    Friend WithEvents time_select_lo As System.Windows.Forms.DateTimePicker
    Friend WithEvents time_select_hi As System.Windows.Forms.DateTimePicker
    Friend WithEvents mactime_box As System.Windows.Forms.ComboBox
    Friend WithEvents web_start_url_box As System.Windows.Forms.TextBox
    Friend WithEvents start_url_label As System.Windows.Forms.Label
    Friend WithEvents web_Firefly_opts As System.Windows.Forms.GroupBox
    Friend WithEvents web_auth_opts As System.Windows.Forms.GroupBox
    Friend WithEvents web_other_opts As System.Windows.Forms.GroupBox
    Friend WithEvents web_recurse_box As System.Windows.Forms.CheckBox
    Friend WithEvents max_link_depth_box As System.Windows.Forms.NumericUpDown
    Friend WithEvents link_depth_label As System.Windows.Forms.Label
    Friend WithEvents respect_robots As System.Windows.Forms.CheckBox
    Friend WithEvents jump_domain_box As System.Windows.Forms.CheckBox
    Friend WithEvents domain_components_box As System.Windows.Forms.NumericUpDown
    Friend WithEvents domain_comp_label As System.Windows.Forms.Label
    Friend WithEvents domain_example_label As System.Windows.Forms.Label
    Friend WithEvents supply_basic_auth_box As System.Windows.Forms.CheckBox
    Friend WithEvents basic_auth_user As System.Windows.Forms.TextBox
    Friend WithEvents basic_auth_pass As System.Windows.Forms.TextBox
    Friend WithEvents user_label As System.Windows.Forms.Label
    Friend WithEvents pass_label As System.Windows.Forms.Label
    Friend WithEvents max_content_size_box As System.Windows.Forms.NumericUpDown
    Friend WithEvents content_length_label As System.Windows.Forms.Label
    Friend WithEvents forge_user_agent As System.Windows.Forms.CheckBox
    Friend WithEvents useragent_box As System.Windows.Forms.TextBox
    Friend WithEvents unalloc_search_opts As System.Windows.Forms.GroupBox
    Friend WithEvents unalloc_opts As System.Windows.Forms.GroupBox
    Friend WithEvents scan_ads As System.Windows.Forms.CheckBox
    Friend WithEvents scan_slack As System.Windows.Forms.CheckBox
    Friend WithEvents scan_unalloc As System.Windows.Forms.CheckBox
    Friend WithEvents scan_unalloc_drive As System.Windows.Forms.ComboBox
    Friend WithEvents unalloc_drive_label As System.Windows.Forms.Label
    Friend WithEvents unalloc_when_found_box As System.Windows.Forms.GroupBox
    Friend WithEvents wipe_unalloc_match_blocks As System.Windows.Forms.RadioButton
    Friend WithEvents copy_unalloc_to_file As System.Windows.Forms.RadioButton
    Friend WithEvents unalloc_match_file As System.Windows.Forms.TextBox
    Friend WithEvents unalloc_file_select_button As System.Windows.Forms.Button
    Friend WithEvents system_regex_select As System.Windows.Forms.CheckedListBox
    Friend WithEvents regex_example_box As System.Windows.Forms.GroupBox
    Friend WithEvents regex_text As System.Windows.Forms.Label
    Friend WithEvents regex_description_box As System.Windows.Forms.RichTextBox
    Friend WithEvents write_local_log As System.Windows.Forms.CheckBox
    Friend WithEvents log_path As System.Windows.Forms.TextBox
    Friend WithEvents select_log_path As System.Windows.Forms.Button
    Friend WithEvents append_log_box As System.Windows.Forms.CheckBox
    Friend WithEvents wipe_log As System.Windows.Forms.CheckBox
    Friend WithEvents csv_options_box As System.Windows.Forms.GroupBox
    Friend WithEvents csv_log_box As System.Windows.Forms.CheckBox
    Friend WithEvents annotate_log_button As System.Windows.Forms.Button
    Friend WithEvents regex_box As System.Windows.Forms.TextBox
    Friend WithEvents regex_name_box As System.Windows.Forms.TextBox
    Friend WithEvents regex_test_label As System.Windows.Forms.Label
    Friend WithEvents test_regex_button As System.Windows.Forms.Button
    Friend WithEvents regex_test_data As System.Windows.Forms.TextBox
    Friend WithEvents add_regex_button As System.Windows.Forms.Button
    Friend WithEvents edit_regex_button As System.Windows.Forms.Button
    Friend WithEvents delete_regex_button As System.Windows.Forms.Button
    Friend WithEvents send_syslog_box As System.Windows.Forms.CheckBox
    Friend WithEvents syslog_host As System.Windows.Forms.TextBox
    Friend WithEvents log_host_label As System.Windows.Forms.Label
    Friend WithEvents log_facility_box As System.Windows.Forms.ComboBox
    Friend WithEvents log_fac_label As System.Windows.Forms.Label
    Friend WithEvents send_to_evt_box As System.Windows.Forms.CheckBox
    Friend WithEvents progress2evt_box As System.Windows.Forms.CheckBox
    Friend WithEvents validator_box As System.Windows.Forms.GroupBox
    Friend WithEvents validator_ssn_button As System.Windows.Forms.RadioButton
    Friend WithEvents luhn_validator_button As System.Windows.Forms.RadioButton
    Friend WithEvents no_validator_button As System.Windows.Forms.RadioButton
    Friend WithEvents log_attributes_box As System.Windows.Forms.CheckedListBox
    Friend WithEvents sin_validator_radio As System.Windows.Forms.RadioButton
    Friend WithEvents custom_regex_tree As System.Windows.Forms.ListBox
    Friend WithEvents regex_val_label As System.Windows.Forms.Label
    Friend WithEvents regex_name_label As System.Windows.Forms.Label
    Friend WithEvents resource_limits_box As System.Windows.Forms.GroupBox
    Friend WithEvents max_archive_unpack As System.Windows.Forms.NumericUpDown
    Friend WithEvents max_archive_label As System.Windows.Forms.Label
    Friend WithEvents min_free_space As System.Windows.Forms.NumericUpDown
    Friend WithEvents min_free_label As System.Windows.Forms.Label
    Friend WithEvents skipcontent_button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.conf_main_tab = New System.Windows.Forms.TabControl
        Me.runtime_tab = New System.Windows.Forms.TabPage
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.runtime_mode_tab = New System.Windows.Forms.TabPage
        Me.search_obj_box = New System.Windows.Forms.GroupBox
        Me.unalloc_obj_radio = New System.Windows.Forms.RadioButton
        Me.web_obj_radio = New System.Windows.Forms.RadioButton
        Me.disk_obj_radio = New System.Windows.Forms.RadioButton
        Me.runtime_proc_opts_tab = New System.Windows.Forms.TabPage
        Me.completion_opts_box = New System.Windows.Forms.GroupBox
        Me.when_finished_box = New System.Windows.Forms.GroupBox
        Me.view_log_radio = New System.Windows.Forms.RadioButton
        Me.exit_radio = New System.Windows.Forms.RadioButton
        Me.restore_window_radio = New System.Windows.Forms.RadioButton
        Me.when_running_box = New System.Windows.Forms.GroupBox
        Me.minimize_when_running = New System.Windows.Forms.CheckBox
        Me.proc_opts_box = New System.Windows.Forms.GroupBox
        Me.highest_prio_button = New System.Windows.Forms.RadioButton
        Me.high_prio_button = New System.Windows.Forms.RadioButton
        Me.normal_prio_button = New System.Windows.Forms.RadioButton
        Me.low_prio_button = New System.Windows.Forms.RadioButton
        Me.scan_method_opts_tab = New System.Windows.Forms.TabPage
        Me.scanning_behavior_box = New System.Windows.Forms.GroupBox
        Me.clear_checkpoints = New System.Windows.Forms.Button
        Me.clear_incremental = New System.Windows.Forms.Button
        Me.checkpoint_box = New System.Windows.Forms.CheckBox
        Me.incremental_box = New System.Windows.Forms.CheckBox
        Me.match_behavior_box = New System.Windows.Forms.GroupBox
        Me.fastmatch_box = New System.Windows.Forms.CheckBox
        Me.scan_depth_box = New System.Windows.Forms.NumericUpDown
        Me.scan_depth_label = New System.Windows.Forms.Label
        Me.log_opts_tab = New System.Windows.Forms.TabPage
        Me.log_options_tab = New System.Windows.Forms.TabControl
        Me.log2file_options_tab = New System.Windows.Forms.TabPage
        Me.Label2 = New System.Windows.Forms.Label
        Me.csv_options_box = New System.Windows.Forms.GroupBox
        Me.log_attributes_box = New System.Windows.Forms.CheckedListBox
        Me.annotate_log_button = New System.Windows.Forms.Button
        Me.csv_log_box = New System.Windows.Forms.CheckBox
        Me.wipe_log = New System.Windows.Forms.CheckBox
        Me.append_log_box = New System.Windows.Forms.CheckBox
        Me.select_log_path = New System.Windows.Forms.Button
        Me.log_path = New System.Windows.Forms.TextBox
        Me.write_local_log = New System.Windows.Forms.CheckBox
        Me.log2syslog_options_tab = New System.Windows.Forms.TabPage
        Me.log_fac_label = New System.Windows.Forms.Label
        Me.log_facility_box = New System.Windows.Forms.ComboBox
        Me.log_host_label = New System.Windows.Forms.Label
        Me.syslog_host = New System.Windows.Forms.TextBox
        Me.send_syslog_box = New System.Windows.Forms.CheckBox
        Me.log2evt_options_tab = New System.Windows.Forms.TabPage
        Me.progress2evt_box = New System.Windows.Forms.CheckBox
        Me.send_to_evt_box = New System.Windows.Forms.CheckBox
        Me.regex_tab = New System.Windows.Forms.TabPage
        Me.regex_tab_control = New System.Windows.Forms.TabControl
        Me.system_regex_tab = New System.Windows.Forms.TabPage
        Me.regex_example_box = New System.Windows.Forms.GroupBox
        Me.regex_description_box = New System.Windows.Forms.RichTextBox
        Me.regex_text = New System.Windows.Forms.Label
        Me.system_regex_select = New System.Windows.Forms.CheckedListBox
        Me.custom_regex_tab = New System.Windows.Forms.TabPage
        Me.regex_name_label = New System.Windows.Forms.Label
        Me.regex_val_label = New System.Windows.Forms.Label
        Me.custom_regex_tree = New System.Windows.Forms.ListBox
        Me.validator_box = New System.Windows.Forms.GroupBox
        Me.sin_validator_radio = New System.Windows.Forms.RadioButton
        Me.no_validator_button = New System.Windows.Forms.RadioButton
        Me.luhn_validator_button = New System.Windows.Forms.RadioButton
        Me.validator_ssn_button = New System.Windows.Forms.RadioButton
        Me.delete_regex_button = New System.Windows.Forms.Button
        Me.edit_regex_button = New System.Windows.Forms.Button
        Me.add_regex_button = New System.Windows.Forms.Button
        Me.regex_test_data = New System.Windows.Forms.TextBox
        Me.test_regex_button = New System.Windows.Forms.Button
        Me.regex_test_label = New System.Windows.Forms.Label
        Me.regex_name_box = New System.Windows.Forms.TextBox
        Me.regex_box = New System.Windows.Forms.TextBox
        Me.scan_opts_tab = New System.Windows.Forms.TabPage
        Me.TabControl2 = New System.Windows.Forms.TabControl
        Me.disk_scan_opts_tab = New System.Windows.Forms.TabPage
        Me.Label1 = New System.Windows.Forms.Label
        Me.reset_atimes_box = New System.Windows.Forms.CheckBox
        Me.local_drives_box = New System.Windows.Forms.CheckBox
        Me.recurse_box = New System.Windows.Forms.CheckBox
        Me.file_select = New System.Windows.Forms.GroupBox
        Me.mactime_box = New System.Windows.Forms.ComboBox
        Me.time_select_hi = New System.Windows.Forms.DateTimePicker
        Me.time_select_lo = New System.Windows.Forms.DateTimePicker
        Me.date_picker_label = New System.Windows.Forms.Label
        Me.skip_path_button = New System.Windows.Forms.Button
        Me.keep_exts_button = New System.Windows.Forms.Button
        Me.skip_exts_button = New System.Windows.Forms.Button
        Me.startdir_select_button = New System.Windows.Forms.Button
        Me.startdir_box = New System.Windows.Forms.TextBox
        Me.web_scan_opt_tab = New System.Windows.Forms.TabPage
        Me.web_other_opts = New System.Windows.Forms.GroupBox
        Me.skipcontent_button = New System.Windows.Forms.Button
        Me.useragent_box = New System.Windows.Forms.TextBox
        Me.forge_user_agent = New System.Windows.Forms.CheckBox
        Me.content_length_label = New System.Windows.Forms.Label
        Me.max_content_size_box = New System.Windows.Forms.NumericUpDown
        Me.web_auth_opts = New System.Windows.Forms.GroupBox
        Me.pass_label = New System.Windows.Forms.Label
        Me.user_label = New System.Windows.Forms.Label
        Me.basic_auth_pass = New System.Windows.Forms.TextBox
        Me.basic_auth_user = New System.Windows.Forms.TextBox
        Me.supply_basic_auth_box = New System.Windows.Forms.CheckBox
        Me.web_Firefly_opts = New System.Windows.Forms.GroupBox
        Me.domain_example_label = New System.Windows.Forms.Label
        Me.domain_comp_label = New System.Windows.Forms.Label
        Me.domain_components_box = New System.Windows.Forms.NumericUpDown
        Me.jump_domain_box = New System.Windows.Forms.CheckBox
        Me.respect_robots = New System.Windows.Forms.CheckBox
        Me.link_depth_label = New System.Windows.Forms.Label
        Me.max_link_depth_box = New System.Windows.Forms.NumericUpDown
        Me.web_recurse_box = New System.Windows.Forms.CheckBox
        Me.start_url_label = New System.Windows.Forms.Label
        Me.web_start_url_box = New System.Windows.Forms.TextBox
        Me.hidden_scan_opt_tab = New System.Windows.Forms.TabPage
        Me.unalloc_opts = New System.Windows.Forms.GroupBox
        Me.unalloc_when_found_box = New System.Windows.Forms.GroupBox
        Me.unalloc_file_select_button = New System.Windows.Forms.Button
        Me.unalloc_match_file = New System.Windows.Forms.TextBox
        Me.copy_unalloc_to_file = New System.Windows.Forms.RadioButton
        Me.wipe_unalloc_match_blocks = New System.Windows.Forms.RadioButton
        Me.unalloc_search_opts = New System.Windows.Forms.GroupBox
        Me.unalloc_drive_label = New System.Windows.Forms.Label
        Me.scan_unalloc_drive = New System.Windows.Forms.ComboBox
        Me.scan_unalloc = New System.Windows.Forms.CheckBox
        Me.scan_slack = New System.Windows.Forms.CheckBox
        Me.scan_ads = New System.Windows.Forms.CheckBox
        Me.misc_opts_tab = New System.Windows.Forms.TabPage
        Me.resource_limits_box = New System.Windows.Forms.GroupBox
        Me.min_free_label = New System.Windows.Forms.Label
        Me.min_free_space = New System.Windows.Forms.NumericUpDown
        Me.max_archive_label = New System.Windows.Forms.Label
        Me.max_archive_unpack = New System.Windows.Forms.NumericUpDown
        Me.save_opts_button = New System.Windows.Forms.Button
        Me.reset_defs_button = New System.Windows.Forms.Button
        Me.cancel_opts_button = New System.Windows.Forms.Button
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.conf_main_tab.SuspendLayout()
        Me.runtime_tab.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.runtime_mode_tab.SuspendLayout()
        Me.search_obj_box.SuspendLayout()
        Me.runtime_proc_opts_tab.SuspendLayout()
        Me.completion_opts_box.SuspendLayout()
        Me.when_finished_box.SuspendLayout()
        Me.when_running_box.SuspendLayout()
        Me.proc_opts_box.SuspendLayout()
        Me.scan_method_opts_tab.SuspendLayout()
        Me.scanning_behavior_box.SuspendLayout()
        Me.match_behavior_box.SuspendLayout()
        CType(Me.scan_depth_box, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.log_opts_tab.SuspendLayout()
        Me.log_options_tab.SuspendLayout()
        Me.log2file_options_tab.SuspendLayout()
        Me.csv_options_box.SuspendLayout()
        Me.log2syslog_options_tab.SuspendLayout()
        Me.log2evt_options_tab.SuspendLayout()
        Me.regex_tab.SuspendLayout()
        Me.regex_tab_control.SuspendLayout()
        Me.system_regex_tab.SuspendLayout()
        Me.regex_example_box.SuspendLayout()
        Me.custom_regex_tab.SuspendLayout()
        Me.validator_box.SuspendLayout()
        Me.scan_opts_tab.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.disk_scan_opts_tab.SuspendLayout()
        Me.file_select.SuspendLayout()
        Me.web_scan_opt_tab.SuspendLayout()
        Me.web_other_opts.SuspendLayout()
        CType(Me.max_content_size_box, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.web_auth_opts.SuspendLayout()
        Me.web_Firefly_opts.SuspendLayout()
        CType(Me.domain_components_box, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.max_link_depth_box, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.hidden_scan_opt_tab.SuspendLayout()
        Me.unalloc_opts.SuspendLayout()
        Me.unalloc_when_found_box.SuspendLayout()
        Me.unalloc_search_opts.SuspendLayout()
        Me.misc_opts_tab.SuspendLayout()
        Me.resource_limits_box.SuspendLayout()
        CType(Me.min_free_space, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.max_archive_unpack, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'conf_main_tab
        '
        Me.conf_main_tab.Controls.Add(Me.runtime_tab)
        Me.conf_main_tab.Controls.Add(Me.log_opts_tab)
        Me.conf_main_tab.Controls.Add(Me.regex_tab)
        Me.conf_main_tab.Controls.Add(Me.scan_opts_tab)
        Me.conf_main_tab.Controls.Add(Me.misc_opts_tab)
        Me.conf_main_tab.Location = New System.Drawing.Point(8, 8)
        Me.conf_main_tab.Name = "conf_main_tab"
        Me.conf_main_tab.SelectedIndex = 0
        Me.conf_main_tab.Size = New System.Drawing.Size(736, 526)
        Me.conf_main_tab.TabIndex = 0
        '
        'runtime_tab
        '
        Me.runtime_tab.Controls.Add(Me.TabControl1)
        Me.runtime_tab.Location = New System.Drawing.Point(4, 25)
        Me.runtime_tab.Name = "runtime_tab"
        Me.runtime_tab.Size = New System.Drawing.Size(728, 497)
        Me.runtime_tab.TabIndex = 0
        Me.runtime_tab.Text = "Runtime"
        Me.runtime_tab.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.runtime_mode_tab)
        Me.TabControl1.Controls.Add(Me.runtime_proc_opts_tab)
        Me.TabControl1.Controls.Add(Me.scan_method_opts_tab)
        Me.TabControl1.Location = New System.Drawing.Point(8, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(640, 397)
        Me.TabControl1.TabIndex = 0
        '
        'runtime_mode_tab
        '
        Me.runtime_mode_tab.Controls.Add(Me.search_obj_box)
        Me.runtime_mode_tab.Location = New System.Drawing.Point(4, 25)
        Me.runtime_mode_tab.Name = "runtime_mode_tab"
        Me.runtime_mode_tab.Size = New System.Drawing.Size(632, 368)
        Me.runtime_mode_tab.TabIndex = 0
        Me.runtime_mode_tab.Text = "Scan Mode"
        '
        'search_obj_box
        '
        Me.search_obj_box.Controls.Add(Me.unalloc_obj_radio)
        Me.search_obj_box.Controls.Add(Me.web_obj_radio)
        Me.search_obj_box.Controls.Add(Me.disk_obj_radio)
        Me.search_obj_box.Location = New System.Drawing.Point(19, 18)
        Me.search_obj_box.Name = "search_obj_box"
        Me.search_obj_box.Size = New System.Drawing.Size(279, 333)
        Me.search_obj_box.TabIndex = 0
        Me.search_obj_box.TabStop = False
        Me.search_obj_box.Text = "Objects to Search"
        '
        'unalloc_obj_radio
        '
        Me.unalloc_obj_radio.Location = New System.Drawing.Point(38, 157)
        Me.unalloc_obj_radio.Name = "unalloc_obj_radio"
        Me.unalloc_obj_radio.Size = New System.Drawing.Size(192, 28)
        Me.unalloc_obj_radio.TabIndex = 2
        Me.unalloc_obj_radio.Text = "Hidden Spaces on Disk"
        '
        'web_obj_radio
        '
        Me.web_obj_radio.Location = New System.Drawing.Point(38, 102)
        Me.web_obj_radio.Name = "web_obj_radio"
        Me.web_obj_radio.Size = New System.Drawing.Size(192, 27)
        Me.web_obj_radio.TabIndex = 1
        Me.web_obj_radio.Text = "Web Pages"
        '
        'disk_obj_radio
        '
        Me.disk_obj_radio.Location = New System.Drawing.Point(38, 46)
        Me.disk_obj_radio.Name = "disk_obj_radio"
        Me.disk_obj_radio.Size = New System.Drawing.Size(192, 28)
        Me.disk_obj_radio.TabIndex = 0
        Me.disk_obj_radio.Text = "Disk or Network Share"
        '
        'runtime_proc_opts_tab
        '
        Me.runtime_proc_opts_tab.Controls.Add(Me.completion_opts_box)
        Me.runtime_proc_opts_tab.Controls.Add(Me.proc_opts_box)
        Me.runtime_proc_opts_tab.Location = New System.Drawing.Point(4, 25)
        Me.runtime_proc_opts_tab.Name = "runtime_proc_opts_tab"
        Me.runtime_proc_opts_tab.Size = New System.Drawing.Size(632, 368)
        Me.runtime_proc_opts_tab.TabIndex = 1
        Me.runtime_proc_opts_tab.Text = "Process Options"
        '
        'completion_opts_box
        '
        Me.completion_opts_box.Controls.Add(Me.when_finished_box)
        Me.completion_opts_box.Controls.Add(Me.when_running_box)
        Me.completion_opts_box.Location = New System.Drawing.Point(336, 18)
        Me.completion_opts_box.Name = "completion_opts_box"
        Me.completion_opts_box.Size = New System.Drawing.Size(259, 333)
        Me.completion_opts_box.TabIndex = 1
        Me.completion_opts_box.TabStop = False
        Me.completion_opts_box.Text = "Scanning Behavior"
        '
        'when_finished_box
        '
        Me.when_finished_box.Controls.Add(Me.view_log_radio)
        Me.when_finished_box.Controls.Add(Me.exit_radio)
        Me.when_finished_box.Controls.Add(Me.restore_window_radio)
        Me.when_finished_box.Location = New System.Drawing.Point(10, 157)
        Me.when_finished_box.Name = "when_finished_box"
        Me.when_finished_box.Size = New System.Drawing.Size(240, 157)
        Me.when_finished_box.TabIndex = 1
        Me.when_finished_box.TabStop = False
        Me.when_finished_box.Text = "When Finished ..."
        '
        'view_log_radio
        '
        Me.view_log_radio.Location = New System.Drawing.Point(29, 102)
        Me.view_log_radio.Name = "view_log_radio"
        Me.view_log_radio.Size = New System.Drawing.Size(163, 27)
        Me.view_log_radio.TabIndex = 2
        Me.view_log_radio.Text = "Launch log viewer"
        '
        'exit_radio
        '
        Me.exit_radio.Location = New System.Drawing.Point(29, 65)
        Me.exit_radio.Name = "exit_radio"
        Me.exit_radio.Size = New System.Drawing.Size(125, 27)
        Me.exit_radio.TabIndex = 1
        Me.exit_radio.Text = "Exit Firefly"
        '
        'restore_window_radio
        '
        Me.restore_window_radio.Location = New System.Drawing.Point(29, 28)
        Me.restore_window_radio.Name = "restore_window_radio"
        Me.restore_window_radio.Size = New System.Drawing.Size(125, 27)
        Me.restore_window_radio.TabIndex = 0
        Me.restore_window_radio.Text = "Restore window"
        '
        'when_running_box
        '
        Me.when_running_box.Controls.Add(Me.minimize_when_running)
        Me.when_running_box.Location = New System.Drawing.Point(10, 28)
        Me.when_running_box.Name = "when_running_box"
        Me.when_running_box.Size = New System.Drawing.Size(240, 115)
        Me.when_running_box.TabIndex = 0
        Me.when_running_box.TabStop = False
        Me.when_running_box.Text = "When Fireflying ..."
        '
        'minimize_when_running
        '
        Me.minimize_when_running.Location = New System.Drawing.Point(19, 28)
        Me.minimize_when_running.Name = "minimize_when_running"
        Me.minimize_when_running.Size = New System.Drawing.Size(125, 27)
        Me.minimize_when_running.TabIndex = 0
        Me.minimize_when_running.Text = "Minimize"
        '
        'proc_opts_box
        '
        Me.proc_opts_box.Controls.Add(Me.highest_prio_button)
        Me.proc_opts_box.Controls.Add(Me.high_prio_button)
        Me.proc_opts_box.Controls.Add(Me.normal_prio_button)
        Me.proc_opts_box.Controls.Add(Me.low_prio_button)
        Me.proc_opts_box.Location = New System.Drawing.Point(19, 18)
        Me.proc_opts_box.Name = "proc_opts_box"
        Me.proc_opts_box.Size = New System.Drawing.Size(259, 333)
        Me.proc_opts_box.TabIndex = 0
        Me.proc_opts_box.TabStop = False
        Me.proc_opts_box.Text = "Process Options"
        '
        'highest_prio_button
        '
        Me.highest_prio_button.Location = New System.Drawing.Point(10, 175)
        Me.highest_prio_button.Name = "highest_prio_button"
        Me.highest_prio_button.Size = New System.Drawing.Size(240, 28)
        Me.highest_prio_button.TabIndex = 3
        Me.highest_prio_button.Text = "Cripple the system while scanning"
        '
        'high_prio_button
        '
        Me.high_prio_button.Location = New System.Drawing.Point(10, 129)
        Me.high_prio_button.Name = "high_prio_button"
        Me.high_prio_button.Size = New System.Drawing.Size(192, 28)
        Me.high_prio_button.TabIndex = 2
        Me.high_prio_button.Text = "Scan at high priority"
        '
        'normal_prio_button
        '
        Me.normal_prio_button.Location = New System.Drawing.Point(10, 83)
        Me.normal_prio_button.Name = "normal_prio_button"
        Me.normal_prio_button.Size = New System.Drawing.Size(230, 28)
        Me.normal_prio_button.TabIndex = 1
        Me.normal_prio_button.Text = "Scan at system-assigned priority"
        '
        'low_prio_button
        '
        Me.low_prio_button.Location = New System.Drawing.Point(10, 37)
        Me.low_prio_button.Name = "low_prio_button"
        Me.low_prio_button.Size = New System.Drawing.Size(192, 28)
        Me.low_prio_button.TabIndex = 0
        Me.low_prio_button.Text = "Scan at low priority"
        '
        'scan_method_opts_tab
        '
        Me.scan_method_opts_tab.Controls.Add(Me.scanning_behavior_box)
        Me.scan_method_opts_tab.Controls.Add(Me.match_behavior_box)
        Me.scan_method_opts_tab.Location = New System.Drawing.Point(4, 25)
        Me.scan_method_opts_tab.Name = "scan_method_opts_tab"
        Me.scan_method_opts_tab.Size = New System.Drawing.Size(632, 368)
        Me.scan_method_opts_tab.TabIndex = 2
        Me.scan_method_opts_tab.Text = "Scan Method"
        '
        'scanning_behavior_box
        '
        Me.scanning_behavior_box.Controls.Add(Me.clear_checkpoints)
        Me.scanning_behavior_box.Controls.Add(Me.clear_incremental)
        Me.scanning_behavior_box.Controls.Add(Me.checkpoint_box)
        Me.scanning_behavior_box.Controls.Add(Me.incremental_box)
        Me.scanning_behavior_box.Location = New System.Drawing.Point(317, 18)
        Me.scanning_behavior_box.Name = "scanning_behavior_box"
        Me.scanning_behavior_box.Size = New System.Drawing.Size(307, 333)
        Me.scanning_behavior_box.TabIndex = 1
        Me.scanning_behavior_box.TabStop = False
        Me.scanning_behavior_box.Text = "Scanning behavior"
        '
        'clear_checkpoints
        '
        Me.clear_checkpoints.Location = New System.Drawing.Point(202, 83)
        Me.clear_checkpoints.Name = "clear_checkpoints"
        Me.clear_checkpoints.Size = New System.Drawing.Size(90, 27)
        Me.clear_checkpoints.TabIndex = 3
        Me.clear_checkpoints.Text = "Clear"
        '
        'clear_incremental
        '
        Me.clear_incremental.Location = New System.Drawing.Point(202, 37)
        Me.clear_incremental.Name = "clear_incremental"
        Me.clear_incremental.Size = New System.Drawing.Size(90, 26)
        Me.clear_incremental.TabIndex = 2
        Me.clear_incremental.Text = "Clear"
        '
        'checkpoint_box
        '
        Me.checkpoint_box.Location = New System.Drawing.Point(29, 83)
        Me.checkpoint_box.Name = "checkpoint_box"
        Me.checkpoint_box.Size = New System.Drawing.Size(173, 28)
        Me.checkpoint_box.TabIndex = 1
        Me.checkpoint_box.Text = "Stop button checkpoints"
        '
        'incremental_box
        '
        Me.incremental_box.Location = New System.Drawing.Point(29, 37)
        Me.incremental_box.Name = "incremental_box"
        Me.incremental_box.Size = New System.Drawing.Size(163, 28)
        Me.incremental_box.TabIndex = 0
        Me.incremental_box.Text = "Incremental Scanning"
        '
        'match_behavior_box
        '
        Me.match_behavior_box.Controls.Add(Me.fastmatch_box)
        Me.match_behavior_box.Controls.Add(Me.scan_depth_box)
        Me.match_behavior_box.Controls.Add(Me.scan_depth_label)
        Me.match_behavior_box.Location = New System.Drawing.Point(19, 18)
        Me.match_behavior_box.Name = "match_behavior_box"
        Me.match_behavior_box.Size = New System.Drawing.Size(279, 333)
        Me.match_behavior_box.TabIndex = 0
        Me.match_behavior_box.TabStop = False
        Me.match_behavior_box.Text = "Matching behavior"
        '
        'fastmatch_box
        '
        Me.fastmatch_box.Location = New System.Drawing.Point(19, 37)
        Me.fastmatch_box.Name = "fastmatch_box"
        Me.fastmatch_box.Size = New System.Drawing.Size(125, 28)
        Me.fastmatch_box.TabIndex = 0
        Me.fastmatch_box.Text = "Fast Matching"
        '
        'scan_depth_box
        '
        Me.scan_depth_box.Location = New System.Drawing.Point(19, 74)
        Me.scan_depth_box.Name = "scan_depth_box"
        Me.scan_depth_box.Size = New System.Drawing.Size(77, 22)
        Me.scan_depth_box.TabIndex = 4
        '
        'scan_depth_label
        '
        Me.scan_depth_label.Location = New System.Drawing.Point(106, 74)
        Me.scan_depth_label.Name = "scan_depth_label"
        Me.scan_depth_label.Size = New System.Drawing.Size(153, 28)
        Me.scan_depth_label.TabIndex = 5
        Me.scan_depth_label.Text = "Stop scanning after xx KB"
        '
        'log_opts_tab
        '
        Me.log_opts_tab.Controls.Add(Me.log_options_tab)
        Me.log_opts_tab.Location = New System.Drawing.Point(4, 25)
        Me.log_opts_tab.Name = "log_opts_tab"
        Me.log_opts_tab.Size = New System.Drawing.Size(728, 497)
        Me.log_opts_tab.TabIndex = 3
        Me.log_opts_tab.Text = "Report Options"
        Me.log_opts_tab.UseVisualStyleBackColor = True
        '
        'log_options_tab
        '
        Me.log_options_tab.Controls.Add(Me.log2file_options_tab)
        Me.log_options_tab.Controls.Add(Me.log2syslog_options_tab)
        Me.log_options_tab.Controls.Add(Me.log2evt_options_tab)
        Me.log_options_tab.Location = New System.Drawing.Point(8, 16)
        Me.log_options_tab.Name = "log_options_tab"
        Me.log_options_tab.SelectedIndex = 0
        Me.log_options_tab.Size = New System.Drawing.Size(360, 464)
        Me.log_options_tab.TabIndex = 0
        '
        'log2file_options_tab
        '
        Me.log2file_options_tab.Controls.Add(Me.Label2)
        Me.log2file_options_tab.Controls.Add(Me.csv_options_box)
        Me.log2file_options_tab.Controls.Add(Me.wipe_log)
        Me.log2file_options_tab.Controls.Add(Me.append_log_box)
        Me.log2file_options_tab.Controls.Add(Me.select_log_path)
        Me.log2file_options_tab.Controls.Add(Me.log_path)
        Me.log2file_options_tab.Controls.Add(Me.write_local_log)
        Me.log2file_options_tab.Location = New System.Drawing.Point(4, 25)
        Me.log2file_options_tab.Name = "log2file_options_tab"
        Me.log2file_options_tab.Size = New System.Drawing.Size(352, 435)
        Me.log2file_options_tab.TabIndex = 0
        Me.log2file_options_tab.Text = "File"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 16)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Report File"
        '
        'csv_options_box
        '
        Me.csv_options_box.Controls.Add(Me.log_attributes_box)
        Me.csv_options_box.Controls.Add(Me.annotate_log_button)
        Me.csv_options_box.Controls.Add(Me.csv_log_box)
        Me.csv_options_box.Location = New System.Drawing.Point(8, 138)
        Me.csv_options_box.Name = "csv_options_box"
        Me.csv_options_box.Size = New System.Drawing.Size(336, 286)
        Me.csv_options_box.TabIndex = 5
        Me.csv_options_box.TabStop = False
        Me.csv_options_box.Text = "Attributes to log"
        '
        'log_attributes_box
        '
        Me.log_attributes_box.CheckOnClick = True
        Me.log_attributes_box.Items.AddRange(New Object() {"Access Time", "Application", "Create Time", "File MD5", "File Score", "File Size", "File Type", "Hit Location Pointer", "Match Text Fragment", "Match Text Strings", "Modify Time", "Path", "Regular Expression", "Total Matches in File"})
        Me.log_attributes_box.Location = New System.Drawing.Point(16, 88)
        Me.log_attributes_box.Name = "log_attributes_box"
        Me.log_attributes_box.Size = New System.Drawing.Size(221, 191)
        Me.log_attributes_box.Sorted = True
        Me.log_attributes_box.TabIndex = 2
        Me.log_attributes_box.ThreeDCheckBoxes = True
        '
        'annotate_log_button
        '
        Me.annotate_log_button.Location = New System.Drawing.Point(16, 56)
        Me.annotate_log_button.Name = "annotate_log_button"
        Me.annotate_log_button.Size = New System.Drawing.Size(125, 26)
        Me.annotate_log_button.TabIndex = 1
        Me.annotate_log_button.Text = "Annotate report"
        '
        'csv_log_box
        '
        Me.csv_log_box.Location = New System.Drawing.Point(16, 28)
        Me.csv_log_box.Name = "csv_log_box"
        Me.csv_log_box.Size = New System.Drawing.Size(201, 27)
        Me.csv_log_box.TabIndex = 0
        Me.csv_log_box.Text = "Generate a CSV report file"
        '
        'wipe_log
        '
        Me.wipe_log.Location = New System.Drawing.Point(152, 104)
        Me.wipe_log.Name = "wipe_log"
        Me.wipe_log.Size = New System.Drawing.Size(182, 27)
        Me.wipe_log.TabIndex = 4
        Me.wipe_log.Text = "Wipe report before writing"
        '
        'append_log_box
        '
        Me.append_log_box.Location = New System.Drawing.Point(152, 72)
        Me.append_log_box.Name = "append_log_box"
        Me.append_log_box.Size = New System.Drawing.Size(182, 27)
        Me.append_log_box.TabIndex = 3
        Me.append_log_box.Text = "Append to report file"
        '
        'select_log_path
        '
        Me.select_log_path.Location = New System.Drawing.Point(320, 40)
        Me.select_log_path.Name = "select_log_path"
        Me.select_log_path.Size = New System.Drawing.Size(24, 26)
        Me.select_log_path.TabIndex = 2
        Me.select_log_path.Text = "..."
        '
        'log_path
        '
        Me.log_path.Location = New System.Drawing.Point(8, 40)
        Me.log_path.Name = "log_path"
        Me.log_path.Size = New System.Drawing.Size(308, 22)
        Me.log_path.TabIndex = 1
        '
        'write_local_log
        '
        Me.write_local_log.Location = New System.Drawing.Point(16, 72)
        Me.write_local_log.Name = "write_local_log"
        Me.write_local_log.Size = New System.Drawing.Size(125, 27)
        Me.write_local_log.TabIndex = 0
        Me.write_local_log.Text = "Write a report file"
        '
        'log2syslog_options_tab
        '
        Me.log2syslog_options_tab.Controls.Add(Me.log_fac_label)
        Me.log2syslog_options_tab.Controls.Add(Me.log_facility_box)
        Me.log2syslog_options_tab.Controls.Add(Me.log_host_label)
        Me.log2syslog_options_tab.Controls.Add(Me.syslog_host)
        Me.log2syslog_options_tab.Controls.Add(Me.send_syslog_box)
        Me.log2syslog_options_tab.Location = New System.Drawing.Point(4, 25)
        Me.log2syslog_options_tab.Name = "log2syslog_options_tab"
        Me.log2syslog_options_tab.Size = New System.Drawing.Size(352, 435)
        Me.log2syslog_options_tab.TabIndex = 1
        Me.log2syslog_options_tab.Text = "Syslog"
        '
        'log_fac_label
        '
        Me.log_fac_label.Location = New System.Drawing.Point(160, 120)
        Me.log_fac_label.Name = "log_fac_label"
        Me.log_fac_label.Size = New System.Drawing.Size(120, 27)
        Me.log_fac_label.TabIndex = 4
        Me.log_fac_label.Text = "Log Facility"
        '
        'log_facility_box
        '
        Me.log_facility_box.Items.AddRange(New Object() {"local0", "local1", "local2", "local3", "local4", "local5", "local6", "local7"})
        Me.log_facility_box.Location = New System.Drawing.Point(8, 120)
        Me.log_facility_box.Name = "log_facility_box"
        Me.log_facility_box.Size = New System.Drawing.Size(146, 24)
        Me.log_facility_box.Sorted = True
        Me.log_facility_box.TabIndex = 3
        '
        'log_host_label
        '
        Me.log_host_label.Location = New System.Drawing.Point(8, 64)
        Me.log_host_label.Name = "log_host_label"
        Me.log_host_label.Size = New System.Drawing.Size(120, 16)
        Me.log_host_label.TabIndex = 2
        Me.log_host_label.Text = "Log host"
        '
        'syslog_host
        '
        Me.syslog_host.Location = New System.Drawing.Point(8, 88)
        Me.syslog_host.Name = "syslog_host"
        Me.syslog_host.Size = New System.Drawing.Size(279, 22)
        Me.syslog_host.TabIndex = 1
        '
        'send_syslog_box
        '
        Me.send_syslog_box.Location = New System.Drawing.Point(8, 16)
        Me.send_syslog_box.Name = "send_syslog_box"
        Me.send_syslog_box.Size = New System.Drawing.Size(183, 28)
        Me.send_syslog_box.TabIndex = 0
        Me.send_syslog_box.Text = "Send to UNIX syslog"
        '
        'log2evt_options_tab
        '
        Me.log2evt_options_tab.Controls.Add(Me.progress2evt_box)
        Me.log2evt_options_tab.Controls.Add(Me.send_to_evt_box)
        Me.log2evt_options_tab.Location = New System.Drawing.Point(4, 25)
        Me.log2evt_options_tab.Name = "log2evt_options_tab"
        Me.log2evt_options_tab.Size = New System.Drawing.Size(352, 435)
        Me.log2evt_options_tab.TabIndex = 2
        Me.log2evt_options_tab.Text = "Event Log"
        '
        'progress2evt_box
        '
        Me.progress2evt_box.Location = New System.Drawing.Point(8, 64)
        Me.progress2evt_box.Name = "progress2evt_box"
        Me.progress2evt_box.Size = New System.Drawing.Size(278, 28)
        Me.progress2evt_box.TabIndex = 1
        Me.progress2evt_box.Text = "Send progress updates to event log"
        '
        'send_to_evt_box
        '
        Me.send_to_evt_box.Location = New System.Drawing.Point(8, 16)
        Me.send_to_evt_box.Name = "send_to_evt_box"
        Me.send_to_evt_box.Size = New System.Drawing.Size(240, 28)
        Me.send_to_evt_box.TabIndex = 0
        Me.send_to_evt_box.Text = "Send to Windows Event Log"
        '
        'regex_tab
        '
        Me.regex_tab.Controls.Add(Me.regex_tab_control)
        Me.regex_tab.Location = New System.Drawing.Point(4, 25)
        Me.regex_tab.Name = "regex_tab"
        Me.regex_tab.Size = New System.Drawing.Size(728, 497)
        Me.regex_tab.TabIndex = 2
        Me.regex_tab.Text = "Regular Expressions"
        Me.regex_tab.UseVisualStyleBackColor = True
        '
        'regex_tab_control
        '
        Me.regex_tab_control.Controls.Add(Me.system_regex_tab)
        Me.regex_tab_control.Controls.Add(Me.custom_regex_tab)
        Me.regex_tab_control.Location = New System.Drawing.Point(8, 16)
        Me.regex_tab_control.Name = "regex_tab_control"
        Me.regex_tab_control.SelectedIndex = 0
        Me.regex_tab_control.Size = New System.Drawing.Size(712, 425)
        Me.regex_tab_control.TabIndex = 0
        '
        'system_regex_tab
        '
        Me.system_regex_tab.Controls.Add(Me.regex_example_box)
        Me.system_regex_tab.Controls.Add(Me.system_regex_select)
        Me.system_regex_tab.Location = New System.Drawing.Point(4, 25)
        Me.system_regex_tab.Name = "system_regex_tab"
        Me.system_regex_tab.Size = New System.Drawing.Size(704, 396)
        Me.system_regex_tab.TabIndex = 0
        Me.system_regex_tab.Text = "System Regexes"
        '
        'regex_example_box
        '
        Me.regex_example_box.Controls.Add(Me.regex_description_box)
        Me.regex_example_box.Controls.Add(Me.regex_text)
        Me.regex_example_box.Location = New System.Drawing.Point(248, 8)
        Me.regex_example_box.Name = "regex_example_box"
        Me.regex_example_box.Size = New System.Drawing.Size(448, 312)
        Me.regex_example_box.TabIndex = 1
        Me.regex_example_box.TabStop = False
        Me.regex_example_box.Text = "Regular expression and description"
        '
        'regex_description_box
        '
        Me.regex_description_box.Location = New System.Drawing.Point(8, 72)
        Me.regex_description_box.Name = "regex_description_box"
        Me.regex_description_box.ReadOnly = True
        Me.regex_description_box.Size = New System.Drawing.Size(429, 222)
        Me.regex_description_box.TabIndex = 1
        Me.regex_description_box.Text = ""
        '
        'regex_text
        '
        Me.regex_text.Location = New System.Drawing.Point(16, 37)
        Me.regex_text.Name = "regex_text"
        Me.regex_text.Size = New System.Drawing.Size(419, 26)
        Me.regex_text.TabIndex = 0
        '
        'system_regex_select
        '
        Me.system_regex_select.Items.AddRange(New Object() {"AMEX", "AMEX_b", "NINO", "SIN333", "SIN333_b", "SIN9", "SIN9_b", "SSN324", "SSN324_b", "SSN9", "SSN9_b", "VMCD", "VMCD_b"})
        Me.system_regex_select.Location = New System.Drawing.Point(8, 8)
        Me.system_regex_select.Name = "system_regex_select"
        Me.system_regex_select.Size = New System.Drawing.Size(231, 310)
        Me.system_regex_select.Sorted = True
        Me.system_regex_select.TabIndex = 0
        Me.system_regex_select.ThreeDCheckBoxes = True
        '
        'custom_regex_tab
        '
        Me.custom_regex_tab.Controls.Add(Me.regex_name_label)
        Me.custom_regex_tab.Controls.Add(Me.regex_val_label)
        Me.custom_regex_tab.Controls.Add(Me.custom_regex_tree)
        Me.custom_regex_tab.Controls.Add(Me.validator_box)
        Me.custom_regex_tab.Controls.Add(Me.delete_regex_button)
        Me.custom_regex_tab.Controls.Add(Me.edit_regex_button)
        Me.custom_regex_tab.Controls.Add(Me.add_regex_button)
        Me.custom_regex_tab.Controls.Add(Me.regex_test_data)
        Me.custom_regex_tab.Controls.Add(Me.test_regex_button)
        Me.custom_regex_tab.Controls.Add(Me.regex_test_label)
        Me.custom_regex_tab.Controls.Add(Me.regex_name_box)
        Me.custom_regex_tab.Controls.Add(Me.regex_box)
        Me.custom_regex_tab.Location = New System.Drawing.Point(4, 25)
        Me.custom_regex_tab.Name = "custom_regex_tab"
        Me.custom_regex_tab.Size = New System.Drawing.Size(704, 396)
        Me.custom_regex_tab.TabIndex = 1
        Me.custom_regex_tab.Text = "Custom Regexes"
        '
        'regex_name_label
        '
        Me.regex_name_label.Location = New System.Drawing.Point(400, 80)
        Me.regex_name_label.Name = "regex_name_label"
        Me.regex_name_label.Size = New System.Drawing.Size(57, 16)
        Me.regex_name_label.TabIndex = 12
        Me.regex_name_label.Text = "Name"
        '
        'regex_val_label
        '
        Me.regex_val_label.Location = New System.Drawing.Point(400, 16)
        Me.regex_val_label.Name = "regex_val_label"
        Me.regex_val_label.Size = New System.Drawing.Size(57, 16)
        Me.regex_val_label.TabIndex = 11
        Me.regex_val_label.Text = "Regex"
        '
        'custom_regex_tree
        '
        Me.custom_regex_tree.ItemHeight = 16
        Me.custom_regex_tree.Location = New System.Drawing.Point(8, 16)
        Me.custom_regex_tree.Name = "custom_regex_tree"
        Me.custom_regex_tree.Size = New System.Drawing.Size(221, 308)
        Me.custom_regex_tree.TabIndex = 10
        '
        'validator_box
        '
        Me.validator_box.Controls.Add(Me.sin_validator_radio)
        Me.validator_box.Controls.Add(Me.no_validator_button)
        Me.validator_box.Controls.Add(Me.luhn_validator_button)
        Me.validator_box.Controls.Add(Me.validator_ssn_button)
        Me.validator_box.Location = New System.Drawing.Point(400, 176)
        Me.validator_box.Name = "validator_box"
        Me.validator_box.Size = New System.Drawing.Size(288, 176)
        Me.validator_box.TabIndex = 9
        Me.validator_box.TabStop = False
        Me.validator_box.Text = "Match validator algorithms"
        '
        'sin_validator_radio
        '
        Me.sin_validator_radio.Location = New System.Drawing.Point(29, 92)
        Me.sin_validator_radio.Name = "sin_validator_radio"
        Me.sin_validator_radio.Size = New System.Drawing.Size(221, 28)
        Me.sin_validator_radio.TabIndex = 3
        Me.sin_validator_radio.Text = "SIN (Luhn, plus reserved digits)"
        '
        'no_validator_button
        '
        Me.no_validator_button.Location = New System.Drawing.Point(29, 120)
        Me.no_validator_button.Name = "no_validator_button"
        Me.no_validator_button.Size = New System.Drawing.Size(201, 28)
        Me.no_validator_button.TabIndex = 2
        Me.no_validator_button.Text = "None"
        '
        'luhn_validator_button
        '
        Me.luhn_validator_button.Location = New System.Drawing.Point(29, 65)
        Me.luhn_validator_button.Name = "luhn_validator_button"
        Me.luhn_validator_button.Size = New System.Drawing.Size(201, 27)
        Me.luhn_validator_button.TabIndex = 1
        Me.luhn_validator_button.Text = "Luhn algorithm (CCNs)"
        '
        'validator_ssn_button
        '
        Me.validator_ssn_button.Location = New System.Drawing.Point(29, 37)
        Me.validator_ssn_button.Name = "validator_ssn_button"
        Me.validator_ssn_button.Size = New System.Drawing.Size(201, 28)
        Me.validator_ssn_button.TabIndex = 0
        Me.validator_ssn_button.Text = "SSN area/group"
        '
        'delete_regex_button
        '
        Me.delete_regex_button.Location = New System.Drawing.Point(240, 128)
        Me.delete_regex_button.Name = "delete_regex_button"
        Me.delete_regex_button.Size = New System.Drawing.Size(154, 28)
        Me.delete_regex_button.TabIndex = 8
        Me.delete_regex_button.Text = "Delete Regex"
        '
        'edit_regex_button
        '
        Me.edit_regex_button.Location = New System.Drawing.Point(240, 72)
        Me.edit_regex_button.Name = "edit_regex_button"
        Me.edit_regex_button.Size = New System.Drawing.Size(154, 28)
        Me.edit_regex_button.TabIndex = 7
        Me.edit_regex_button.Text = "Edit Regex ->"
        '
        'add_regex_button
        '
        Me.add_regex_button.Location = New System.Drawing.Point(240, 16)
        Me.add_regex_button.Name = "add_regex_button"
        Me.add_regex_button.Size = New System.Drawing.Size(154, 28)
        Me.add_regex_button.TabIndex = 6
        Me.add_regex_button.Text = "<- Add Regex"
        '
        'regex_test_data
        '
        Me.regex_test_data.Location = New System.Drawing.Point(400, 144)
        Me.regex_test_data.Name = "regex_test_data"
        Me.regex_test_data.Size = New System.Drawing.Size(288, 22)
        Me.regex_test_data.TabIndex = 5
        Me.regex_test_data.Text = "regex test data"
        '
        'test_regex_button
        '
        Me.test_regex_button.Location = New System.Drawing.Point(632, 104)
        Me.test_regex_button.Name = "test_regex_button"
        Me.test_regex_button.Size = New System.Drawing.Size(58, 27)
        Me.test_regex_button.TabIndex = 4
        Me.test_regex_button.Text = "TEST"
        '
        'regex_test_label
        '
        Me.regex_test_label.Location = New System.Drawing.Point(536, 104)
        Me.regex_test_label.Name = "regex_test_label"
        Me.regex_test_label.Size = New System.Drawing.Size(77, 27)
        Me.regex_test_label.TabIndex = 3
        '
        'regex_name_box
        '
        Me.regex_name_box.Location = New System.Drawing.Point(400, 104)
        Me.regex_name_box.Name = "regex_name_box"
        Me.regex_name_box.Size = New System.Drawing.Size(120, 22)
        Me.regex_name_box.TabIndex = 2
        '
        'regex_box
        '
        Me.regex_box.Location = New System.Drawing.Point(400, 40)
        Me.regex_box.Name = "regex_box"
        Me.regex_box.Size = New System.Drawing.Size(288, 22)
        Me.regex_box.TabIndex = 1
        '
        'scan_opts_tab
        '
        Me.scan_opts_tab.Controls.Add(Me.TabControl2)
        Me.scan_opts_tab.Location = New System.Drawing.Point(4, 25)
        Me.scan_opts_tab.Name = "scan_opts_tab"
        Me.scan_opts_tab.Size = New System.Drawing.Size(728, 497)
        Me.scan_opts_tab.TabIndex = 1
        Me.scan_opts_tab.Text = "Scan Options"
        Me.scan_opts_tab.UseVisualStyleBackColor = True
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.disk_scan_opts_tab)
        Me.TabControl2.Controls.Add(Me.web_scan_opt_tab)
        Me.TabControl2.Controls.Add(Me.hidden_scan_opt_tab)
        Me.TabControl2.Location = New System.Drawing.Point(8, 16)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(712, 440)
        Me.TabControl2.TabIndex = 0
        '
        'disk_scan_opts_tab
        '
        Me.disk_scan_opts_tab.Controls.Add(Me.Label1)
        Me.disk_scan_opts_tab.Controls.Add(Me.reset_atimes_box)
        Me.disk_scan_opts_tab.Controls.Add(Me.local_drives_box)
        Me.disk_scan_opts_tab.Controls.Add(Me.recurse_box)
        Me.disk_scan_opts_tab.Controls.Add(Me.file_select)
        Me.disk_scan_opts_tab.Controls.Add(Me.startdir_select_button)
        Me.disk_scan_opts_tab.Controls.Add(Me.startdir_box)
        Me.disk_scan_opts_tab.Location = New System.Drawing.Point(4, 25)
        Me.disk_scan_opts_tab.Name = "disk_scan_opts_tab"
        Me.disk_scan_opts_tab.Size = New System.Drawing.Size(704, 411)
        Me.disk_scan_opts_tab.TabIndex = 0
        Me.disk_scan_opts_tab.Text = "Disk"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 23)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Start Directory"
        '
        'reset_atimes_box
        '
        Me.reset_atimes_box.Location = New System.Drawing.Point(16, 144)
        Me.reset_atimes_box.Name = "reset_atimes_box"
        Me.reset_atimes_box.Size = New System.Drawing.Size(256, 28)
        Me.reset_atimes_box.TabIndex = 5
        Me.reset_atimes_box.Text = "Reset file access times"
        '
        'local_drives_box
        '
        Me.local_drives_box.Location = New System.Drawing.Point(16, 112)
        Me.local_drives_box.Name = "local_drives_box"
        Me.local_drives_box.Size = New System.Drawing.Size(250, 28)
        Me.local_drives_box.TabIndex = 4
        Me.local_drives_box.Text = "Scan all local drives"
        '
        'recurse_box
        '
        Me.recurse_box.Location = New System.Drawing.Point(16, 80)
        Me.recurse_box.Name = "recurse_box"
        Me.recurse_box.Size = New System.Drawing.Size(250, 28)
        Me.recurse_box.TabIndex = 3
        Me.recurse_box.Text = "Process subdirectories"
        '
        'file_select
        '
        Me.file_select.Controls.Add(Me.mactime_box)
        Me.file_select.Controls.Add(Me.time_select_hi)
        Me.file_select.Controls.Add(Me.time_select_lo)
        Me.file_select.Controls.Add(Me.date_picker_label)
        Me.file_select.Controls.Add(Me.skip_path_button)
        Me.file_select.Controls.Add(Me.keep_exts_button)
        Me.file_select.Controls.Add(Me.skip_exts_button)
        Me.file_select.Location = New System.Drawing.Point(392, 24)
        Me.file_select.Name = "file_select"
        Me.file_select.Size = New System.Drawing.Size(296, 360)
        Me.file_select.TabIndex = 2
        Me.file_select.TabStop = False
        Me.file_select.Text = "File Selections"
        '
        'mactime_box
        '
        Me.mactime_box.Items.AddRange(New Object() {"Create Time", "Modify Time", "Access Time"})
        Me.mactime_box.Location = New System.Drawing.Point(38, 305)
        Me.mactime_box.Name = "mactime_box"
        Me.mactime_box.Size = New System.Drawing.Size(240, 24)
        Me.mactime_box.TabIndex = 6
        Me.mactime_box.Text = "timestamp"
        '
        'time_select_hi
        '
        Me.time_select_hi.CustomFormat = "dd-MM-yyyy HH:mm:ss"
        Me.time_select_hi.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.time_select_hi.Location = New System.Drawing.Point(38, 268)
        Me.time_select_hi.Name = "time_select_hi"
        Me.time_select_hi.ShowUpDown = True
        Me.time_select_hi.Size = New System.Drawing.Size(240, 22)
        Me.time_select_hi.TabIndex = 5
        '
        'time_select_lo
        '
        Me.time_select_lo.CustomFormat = "dd-MM-yyyy HH:mm:ss"
        Me.time_select_lo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.time_select_lo.Location = New System.Drawing.Point(38, 231)
        Me.time_select_lo.Name = "time_select_lo"
        Me.time_select_lo.ShowUpDown = True
        Me.time_select_lo.Size = New System.Drawing.Size(240, 22)
        Me.time_select_lo.TabIndex = 4
        '
        'date_picker_label
        '
        Me.date_picker_label.Location = New System.Drawing.Point(38, 203)
        Me.date_picker_label.Name = "date_picker_label"
        Me.date_picker_label.Size = New System.Drawing.Size(154, 27)
        Me.date_picker_label.TabIndex = 3
        Me.date_picker_label.Text = "Between times"
        '
        'skip_path_button
        '
        Me.skip_path_button.Location = New System.Drawing.Point(67, 148)
        Me.skip_path_button.Name = "skip_path_button"
        Me.skip_path_button.Size = New System.Drawing.Size(173, 26)
        Me.skip_path_button.TabIndex = 2
        Me.skip_path_button.Text = "Paths to Skip"
        '
        'keep_exts_button
        '
        Me.keep_exts_button.Location = New System.Drawing.Point(67, 92)
        Me.keep_exts_button.Name = "keep_exts_button"
        Me.keep_exts_button.Size = New System.Drawing.Size(173, 27)
        Me.keep_exts_button.TabIndex = 1
        Me.keep_exts_button.Text = "File Extensions to Keep"
        '
        'skip_exts_button
        '
        Me.skip_exts_button.Location = New System.Drawing.Point(67, 37)
        Me.skip_exts_button.Name = "skip_exts_button"
        Me.skip_exts_button.Size = New System.Drawing.Size(173, 26)
        Me.skip_exts_button.TabIndex = 0
        Me.skip_exts_button.Text = "File Extensions to Skip"
        '
        'startdir_select_button
        '
        Me.startdir_select_button.Location = New System.Drawing.Point(344, 40)
        Me.startdir_select_button.Name = "startdir_select_button"
        Me.startdir_select_button.Size = New System.Drawing.Size(24, 24)
        Me.startdir_select_button.TabIndex = 1
        Me.startdir_select_button.Text = "..."
        '
        'startdir_box
        '
        Me.startdir_box.Location = New System.Drawing.Point(8, 40)
        Me.startdir_box.Name = "startdir_box"
        Me.startdir_box.Size = New System.Drawing.Size(328, 22)
        Me.startdir_box.TabIndex = 0
        '
        'web_scan_opt_tab
        '
        Me.web_scan_opt_tab.Controls.Add(Me.web_other_opts)
        Me.web_scan_opt_tab.Controls.Add(Me.web_auth_opts)
        Me.web_scan_opt_tab.Controls.Add(Me.web_Firefly_opts)
        Me.web_scan_opt_tab.Controls.Add(Me.start_url_label)
        Me.web_scan_opt_tab.Controls.Add(Me.web_start_url_box)
        Me.web_scan_opt_tab.Location = New System.Drawing.Point(4, 25)
        Me.web_scan_opt_tab.Name = "web_scan_opt_tab"
        Me.web_scan_opt_tab.Size = New System.Drawing.Size(704, 411)
        Me.web_scan_opt_tab.TabIndex = 1
        Me.web_scan_opt_tab.Text = "Web"
        '
        'web_other_opts
        '
        Me.web_other_opts.Controls.Add(Me.skipcontent_button)
        Me.web_other_opts.Controls.Add(Me.useragent_box)
        Me.web_other_opts.Controls.Add(Me.forge_user_agent)
        Me.web_other_opts.Controls.Add(Me.content_length_label)
        Me.web_other_opts.Controls.Add(Me.max_content_size_box)
        Me.web_other_opts.Location = New System.Drawing.Point(288, 232)
        Me.web_other_opts.Name = "web_other_opts"
        Me.web_other_opts.Size = New System.Drawing.Size(259, 168)
        Me.web_other_opts.TabIndex = 5
        Me.web_other_opts.TabStop = False
        Me.web_other_opts.Text = "Other Options"
        '
        'skipcontent_button
        '
        Me.skipcontent_button.Location = New System.Drawing.Point(48, 128)
        Me.skipcontent_button.Name = "skipcontent_button"
        Me.skipcontent_button.Size = New System.Drawing.Size(163, 26)
        Me.skipcontent_button.TabIndex = 4
        Me.skipcontent_button.Text = "Content-types to Skip"
        '
        'useragent_box
        '
        Me.useragent_box.Location = New System.Drawing.Point(29, 96)
        Me.useragent_box.Name = "useragent_box"
        Me.useragent_box.Size = New System.Drawing.Size(211, 22)
        Me.useragent_box.TabIndex = 3
        '
        'forge_user_agent
        '
        Me.forge_user_agent.Location = New System.Drawing.Point(29, 64)
        Me.forge_user_agent.Name = "forge_user_agent"
        Me.forge_user_agent.Size = New System.Drawing.Size(182, 28)
        Me.forge_user_agent.TabIndex = 2
        Me.forge_user_agent.Text = "Change UserAgent"
        '
        'content_length_label
        '
        Me.content_length_label.Location = New System.Drawing.Point(96, 24)
        Me.content_length_label.Name = "content_length_label"
        Me.content_length_label.Size = New System.Drawing.Size(134, 26)
        Me.content_length_label.TabIndex = 1
        Me.content_length_label.Text = "Max content size, MB"
        '
        'max_content_size_box
        '
        Me.max_content_size_box.Location = New System.Drawing.Point(29, 24)
        Me.max_content_size_box.Name = "max_content_size_box"
        Me.max_content_size_box.Size = New System.Drawing.Size(57, 22)
        Me.max_content_size_box.TabIndex = 0
        Me.max_content_size_box.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'web_auth_opts
        '
        Me.web_auth_opts.Controls.Add(Me.pass_label)
        Me.web_auth_opts.Controls.Add(Me.user_label)
        Me.web_auth_opts.Controls.Add(Me.basic_auth_pass)
        Me.web_auth_opts.Controls.Add(Me.basic_auth_user)
        Me.web_auth_opts.Controls.Add(Me.supply_basic_auth_box)
        Me.web_auth_opts.Location = New System.Drawing.Point(288, 92)
        Me.web_auth_opts.Name = "web_auth_opts"
        Me.web_auth_opts.Size = New System.Drawing.Size(259, 132)
        Me.web_auth_opts.TabIndex = 4
        Me.web_auth_opts.TabStop = False
        Me.web_auth_opts.Text = "Authentication Options"
        '
        'pass_label
        '
        Me.pass_label.Location = New System.Drawing.Point(152, 96)
        Me.pass_label.Name = "pass_label"
        Me.pass_label.Size = New System.Drawing.Size(86, 27)
        Me.pass_label.TabIndex = 4
        Me.pass_label.Text = "Password"
        '
        'user_label
        '
        Me.user_label.Location = New System.Drawing.Point(154, 64)
        Me.user_label.Name = "user_label"
        Me.user_label.Size = New System.Drawing.Size(86, 27)
        Me.user_label.TabIndex = 3
        Me.user_label.Text = "Username"
        '
        'basic_auth_pass
        '
        Me.basic_auth_pass.Location = New System.Drawing.Point(32, 96)
        Me.basic_auth_pass.Name = "basic_auth_pass"
        Me.basic_auth_pass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.basic_auth_pass.Size = New System.Drawing.Size(115, 22)
        Me.basic_auth_pass.TabIndex = 2
        '
        'basic_auth_user
        '
        Me.basic_auth_user.Location = New System.Drawing.Point(32, 64)
        Me.basic_auth_user.Name = "basic_auth_user"
        Me.basic_auth_user.Size = New System.Drawing.Size(115, 22)
        Me.basic_auth_user.TabIndex = 1
        '
        'supply_basic_auth_box
        '
        Me.supply_basic_auth_box.Location = New System.Drawing.Point(29, 24)
        Me.supply_basic_auth_box.Name = "supply_basic_auth_box"
        Me.supply_basic_auth_box.Size = New System.Drawing.Size(182, 28)
        Me.supply_basic_auth_box.TabIndex = 0
        Me.supply_basic_auth_box.Text = "Supply basic auth"
        '
        'web_Firefly_opts
        '
        Me.web_Firefly_opts.Controls.Add(Me.domain_example_label)
        Me.web_Firefly_opts.Controls.Add(Me.domain_comp_label)
        Me.web_Firefly_opts.Controls.Add(Me.domain_components_box)
        Me.web_Firefly_opts.Controls.Add(Me.jump_domain_box)
        Me.web_Firefly_opts.Controls.Add(Me.respect_robots)
        Me.web_Firefly_opts.Controls.Add(Me.link_depth_label)
        Me.web_Firefly_opts.Controls.Add(Me.max_link_depth_box)
        Me.web_Firefly_opts.Controls.Add(Me.web_recurse_box)
        Me.web_Firefly_opts.Location = New System.Drawing.Point(16, 92)
        Me.web_Firefly_opts.Name = "web_Firefly_opts"
        Me.web_Firefly_opts.Size = New System.Drawing.Size(260, 308)
        Me.web_Firefly_opts.TabIndex = 3
        Me.web_Firefly_opts.TabStop = False
        Me.web_Firefly_opts.Text = "Web Firefly Options"
        '
        'domain_example_label
        '
        Me.domain_example_label.Location = New System.Drawing.Point(29, 231)
        Me.domain_example_label.Name = "domain_example_label"
        Me.domain_example_label.Size = New System.Drawing.Size(211, 26)
        Me.domain_example_label.TabIndex = 7
        '
        'domain_comp_label
        '
        Me.domain_comp_label.Location = New System.Drawing.Point(96, 185)
        Me.domain_comp_label.Name = "domain_comp_label"
        Me.domain_comp_label.Size = New System.Drawing.Size(144, 26)
        Me.domain_comp_label.TabIndex = 6
        Me.domain_comp_label.Text = "Domain Components"
        '
        'domain_components_box
        '
        Me.domain_components_box.Location = New System.Drawing.Point(29, 185)
        Me.domain_components_box.Name = "domain_components_box"
        Me.domain_components_box.Size = New System.Drawing.Size(57, 22)
        Me.domain_components_box.TabIndex = 5
        Me.domain_components_box.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'jump_domain_box
        '
        Me.jump_domain_box.Location = New System.Drawing.Point(29, 148)
        Me.jump_domain_box.Name = "jump_domain_box"
        Me.jump_domain_box.Size = New System.Drawing.Size(211, 27)
        Me.jump_domain_box.TabIndex = 4
        Me.jump_domain_box.Text = "Follow links to other domains"
        '
        'respect_robots
        '
        Me.respect_robots.Location = New System.Drawing.Point(29, 111)
        Me.respect_robots.Name = "respect_robots"
        Me.respect_robots.Size = New System.Drawing.Size(153, 27)
        Me.respect_robots.TabIndex = 3
        Me.respect_robots.Text = "Respect robots.txt"
        '
        'link_depth_label
        '
        Me.link_depth_label.Location = New System.Drawing.Point(106, 74)
        Me.link_depth_label.Name = "link_depth_label"
        Me.link_depth_label.Size = New System.Drawing.Size(120, 26)
        Me.link_depth_label.TabIndex = 2
        Me.link_depth_label.Text = "Max depth"
        '
        'max_link_depth_box
        '
        Me.max_link_depth_box.Location = New System.Drawing.Point(29, 74)
        Me.max_link_depth_box.Name = "max_link_depth_box"
        Me.max_link_depth_box.Size = New System.Drawing.Size(67, 22)
        Me.max_link_depth_box.TabIndex = 1
        Me.max_link_depth_box.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'web_recurse_box
        '
        Me.web_recurse_box.Location = New System.Drawing.Point(29, 37)
        Me.web_recurse_box.Name = "web_recurse_box"
        Me.web_recurse_box.Size = New System.Drawing.Size(125, 28)
        Me.web_recurse_box.TabIndex = 0
        Me.web_recurse_box.Text = "Descend links"
        '
        'start_url_label
        '
        Me.start_url_label.Location = New System.Drawing.Point(24, 16)
        Me.start_url_label.Name = "start_url_label"
        Me.start_url_label.Size = New System.Drawing.Size(67, 26)
        Me.start_url_label.TabIndex = 2
        Me.start_url_label.Text = "Start URL"
        '
        'web_start_url_box
        '
        Me.web_start_url_box.Location = New System.Drawing.Point(24, 48)
        Me.web_start_url_box.Name = "web_start_url_box"
        Me.web_start_url_box.Size = New System.Drawing.Size(326, 22)
        Me.web_start_url_box.TabIndex = 1
        '
        'hidden_scan_opt_tab
        '
        Me.hidden_scan_opt_tab.Controls.Add(Me.unalloc_opts)
        Me.hidden_scan_opt_tab.Controls.Add(Me.unalloc_search_opts)
        Me.hidden_scan_opt_tab.Location = New System.Drawing.Point(4, 25)
        Me.hidden_scan_opt_tab.Name = "hidden_scan_opt_tab"
        Me.hidden_scan_opt_tab.Size = New System.Drawing.Size(704, 411)
        Me.hidden_scan_opt_tab.TabIndex = 2
        Me.hidden_scan_opt_tab.Text = "Hidden"
        '
        'unalloc_opts
        '
        Me.unalloc_opts.Controls.Add(Me.unalloc_when_found_box)
        Me.unalloc_opts.Location = New System.Drawing.Point(336, 16)
        Me.unalloc_opts.Name = "unalloc_opts"
        Me.unalloc_opts.Size = New System.Drawing.Size(317, 323)
        Me.unalloc_opts.TabIndex = 1
        Me.unalloc_opts.TabStop = False
        Me.unalloc_opts.Text = "When found ..."
        '
        'unalloc_when_found_box
        '
        Me.unalloc_when_found_box.Controls.Add(Me.unalloc_file_select_button)
        Me.unalloc_when_found_box.Controls.Add(Me.unalloc_match_file)
        Me.unalloc_when_found_box.Controls.Add(Me.copy_unalloc_to_file)
        Me.unalloc_when_found_box.Controls.Add(Me.wipe_unalloc_match_blocks)
        Me.unalloc_when_found_box.Location = New System.Drawing.Point(19, 37)
        Me.unalloc_when_found_box.Name = "unalloc_when_found_box"
        Me.unalloc_when_found_box.Size = New System.Drawing.Size(279, 175)
        Me.unalloc_when_found_box.TabIndex = 0
        Me.unalloc_when_found_box.TabStop = False
        Me.unalloc_when_found_box.Text = "Unallocated Space"
        '
        'unalloc_file_select_button
        '
        Me.unalloc_file_select_button.Location = New System.Drawing.Point(96, 138)
        Me.unalloc_file_select_button.Name = "unalloc_file_select_button"
        Me.unalloc_file_select_button.Size = New System.Drawing.Size(90, 27)
        Me.unalloc_file_select_button.TabIndex = 3
        Me.unalloc_file_select_button.Text = "Select file"
        '
        'unalloc_match_file
        '
        Me.unalloc_match_file.Location = New System.Drawing.Point(38, 102)
        Me.unalloc_match_file.Name = "unalloc_match_file"
        Me.unalloc_match_file.Size = New System.Drawing.Size(192, 22)
        Me.unalloc_match_file.TabIndex = 2
        '
        'copy_unalloc_to_file
        '
        Me.copy_unalloc_to_file.Location = New System.Drawing.Point(38, 65)
        Me.copy_unalloc_to_file.Name = "copy_unalloc_to_file"
        Me.copy_unalloc_to_file.Size = New System.Drawing.Size(192, 27)
        Me.copy_unalloc_to_file.TabIndex = 1
        Me.copy_unalloc_to_file.Text = "Copy blocks to file"
        '
        'wipe_unalloc_match_blocks
        '
        Me.wipe_unalloc_match_blocks.Location = New System.Drawing.Point(38, 28)
        Me.wipe_unalloc_match_blocks.Name = "wipe_unalloc_match_blocks"
        Me.wipe_unalloc_match_blocks.Size = New System.Drawing.Size(192, 27)
        Me.wipe_unalloc_match_blocks.TabIndex = 0
        Me.wipe_unalloc_match_blocks.Text = "Wipe matching blocks"
        '
        'unalloc_search_opts
        '
        Me.unalloc_search_opts.Controls.Add(Me.unalloc_drive_label)
        Me.unalloc_search_opts.Controls.Add(Me.scan_unalloc_drive)
        Me.unalloc_search_opts.Controls.Add(Me.scan_unalloc)
        Me.unalloc_search_opts.Controls.Add(Me.scan_slack)
        Me.unalloc_search_opts.Controls.Add(Me.scan_ads)
        Me.unalloc_search_opts.Location = New System.Drawing.Point(8, 16)
        Me.unalloc_search_opts.Name = "unalloc_search_opts"
        Me.unalloc_search_opts.Size = New System.Drawing.Size(317, 323)
        Me.unalloc_search_opts.TabIndex = 0
        Me.unalloc_search_opts.TabStop = False
        Me.unalloc_search_opts.Text = "Where to search"
        '
        'unalloc_drive_label
        '
        Me.unalloc_drive_label.Location = New System.Drawing.Point(144, 185)
        Me.unalloc_drive_label.Name = "unalloc_drive_label"
        Me.unalloc_drive_label.Size = New System.Drawing.Size(120, 26)
        Me.unalloc_drive_label.TabIndex = 4
        Me.unalloc_drive_label.Text = "On Drive"
        '
        'scan_unalloc_drive
        '
        Me.scan_unalloc_drive.Location = New System.Drawing.Point(38, 185)
        Me.scan_unalloc_drive.Name = "scan_unalloc_drive"
        Me.scan_unalloc_drive.Size = New System.Drawing.Size(87, 24)
        Me.scan_unalloc_drive.TabIndex = 3
        '
        'scan_unalloc
        '
        Me.scan_unalloc.Location = New System.Drawing.Point(38, 138)
        Me.scan_unalloc.Name = "scan_unalloc"
        Me.scan_unalloc.Size = New System.Drawing.Size(202, 28)
        Me.scan_unalloc.TabIndex = 2
        Me.scan_unalloc.Text = "Unallocated space"
        '
        'scan_slack
        '
        Me.scan_slack.Location = New System.Drawing.Point(38, 92)
        Me.scan_slack.Name = "scan_slack"
        Me.scan_slack.Size = New System.Drawing.Size(202, 28)
        Me.scan_slack.TabIndex = 1
        Me.scan_slack.Text = "File slack space"
        '
        'scan_ads
        '
        Me.scan_ads.Location = New System.Drawing.Point(38, 46)
        Me.scan_ads.Name = "scan_ads"
        Me.scan_ads.Size = New System.Drawing.Size(202, 28)
        Me.scan_ads.TabIndex = 0
        Me.scan_ads.Text = "Alternate Data Streams"
        '
        'misc_opts_tab
        '
        Me.misc_opts_tab.Controls.Add(Me.resource_limits_box)
        Me.misc_opts_tab.Location = New System.Drawing.Point(4, 25)
        Me.misc_opts_tab.Name = "misc_opts_tab"
        Me.misc_opts_tab.Size = New System.Drawing.Size(728, 497)
        Me.misc_opts_tab.TabIndex = 5
        Me.misc_opts_tab.Text = "Misc Options"
        Me.misc_opts_tab.UseVisualStyleBackColor = True
        '
        'resource_limits_box
        '
        Me.resource_limits_box.Controls.Add(Me.min_free_label)
        Me.resource_limits_box.Controls.Add(Me.min_free_space)
        Me.resource_limits_box.Controls.Add(Me.max_archive_label)
        Me.resource_limits_box.Controls.Add(Me.max_archive_unpack)
        Me.resource_limits_box.Location = New System.Drawing.Point(8, 16)
        Me.resource_limits_box.Name = "resource_limits_box"
        Me.resource_limits_box.Size = New System.Drawing.Size(240, 168)
        Me.resource_limits_box.TabIndex = 0
        Me.resource_limits_box.TabStop = False
        Me.resource_limits_box.Text = "Resource Limits"
        '
        'min_free_label
        '
        Me.min_free_label.Location = New System.Drawing.Point(96, 102)
        Me.min_free_label.Name = "min_free_label"
        Me.min_free_label.Size = New System.Drawing.Size(134, 46)
        Me.min_free_label.TabIndex = 3
        Me.min_free_label.Text = "Minimum free space while unpacking, GB"
        '
        'min_free_space
        '
        Me.min_free_space.Location = New System.Drawing.Point(19, 102)
        Me.min_free_space.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.min_free_space.Name = "min_free_space"
        Me.min_free_space.Size = New System.Drawing.Size(67, 22)
        Me.min_free_space.TabIndex = 2
        Me.min_free_space.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'max_archive_label
        '
        Me.max_archive_label.Location = New System.Drawing.Point(96, 46)
        Me.max_archive_label.Name = "max_archive_label"
        Me.max_archive_label.Size = New System.Drawing.Size(134, 46)
        Me.max_archive_label.TabIndex = 1
        Me.max_archive_label.Text = "Max file to unpack from archives, MB"
        '
        'max_archive_unpack
        '
        Me.max_archive_unpack.Location = New System.Drawing.Point(19, 46)
        Me.max_archive_unpack.Maximum = New Decimal(New Integer() {1024, 0, 0, 0})
        Me.max_archive_unpack.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.max_archive_unpack.Name = "max_archive_unpack"
        Me.max_archive_unpack.Size = New System.Drawing.Size(67, 22)
        Me.max_archive_unpack.TabIndex = 0
        Me.max_archive_unpack.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'save_opts_button
        '
        Me.save_opts_button.Location = New System.Drawing.Point(320, 544)
        Me.save_opts_button.Name = "save_opts_button"
        Me.save_opts_button.Size = New System.Drawing.Size(134, 26)
        Me.save_opts_button.TabIndex = 1
        Me.save_opts_button.Text = "Save"
        '
        'reset_defs_button
        '
        Me.reset_defs_button.Location = New System.Drawing.Point(464, 544)
        Me.reset_defs_button.Name = "reset_defs_button"
        Me.reset_defs_button.Size = New System.Drawing.Size(134, 26)
        Me.reset_defs_button.TabIndex = 2
        Me.reset_defs_button.Text = "Reset Defaults"
        '
        'cancel_opts_button
        '
        Me.cancel_opts_button.Location = New System.Drawing.Point(608, 544)
        Me.cancel_opts_button.Name = "cancel_opts_button"
        Me.cancel_opts_button.Size = New System.Drawing.Size(134, 26)
        Me.cancel_opts_button.TabIndex = 3
        Me.cancel_opts_button.Text = "Cancel"
        '
        'Preferences
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(752, 584)
        Me.Controls.Add(Me.cancel_opts_button)
        Me.Controls.Add(Me.reset_defs_button)
        Me.Controls.Add(Me.save_opts_button)
        Me.Controls.Add(Me.conf_main_tab)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.Name = "Preferences"
        Me.Text = "Firefly Configuration"
        Me.conf_main_tab.ResumeLayout(False)
        Me.runtime_tab.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.runtime_mode_tab.ResumeLayout(False)
        Me.search_obj_box.ResumeLayout(False)
        Me.runtime_proc_opts_tab.ResumeLayout(False)
        Me.completion_opts_box.ResumeLayout(False)
        Me.when_finished_box.ResumeLayout(False)
        Me.when_running_box.ResumeLayout(False)
        Me.proc_opts_box.ResumeLayout(False)
        Me.scan_method_opts_tab.ResumeLayout(False)
        Me.scanning_behavior_box.ResumeLayout(False)
        Me.match_behavior_box.ResumeLayout(False)
        CType(Me.scan_depth_box, System.ComponentModel.ISupportInitialize).EndInit()
        Me.log_opts_tab.ResumeLayout(False)
        Me.log_options_tab.ResumeLayout(False)
        Me.log2file_options_tab.ResumeLayout(False)
        Me.log2file_options_tab.PerformLayout()
        Me.csv_options_box.ResumeLayout(False)
        Me.log2syslog_options_tab.ResumeLayout(False)
        Me.log2syslog_options_tab.PerformLayout()
        Me.log2evt_options_tab.ResumeLayout(False)
        Me.regex_tab.ResumeLayout(False)
        Me.regex_tab_control.ResumeLayout(False)
        Me.system_regex_tab.ResumeLayout(False)
        Me.regex_example_box.ResumeLayout(False)
        Me.custom_regex_tab.ResumeLayout(False)
        Me.custom_regex_tab.PerformLayout()
        Me.validator_box.ResumeLayout(False)
        Me.scan_opts_tab.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.disk_scan_opts_tab.ResumeLayout(False)
        Me.disk_scan_opts_tab.PerformLayout()
        Me.file_select.ResumeLayout(False)
        Me.web_scan_opt_tab.ResumeLayout(False)
        Me.web_scan_opt_tab.PerformLayout()
        Me.web_other_opts.ResumeLayout(False)
        Me.web_other_opts.PerformLayout()
        CType(Me.max_content_size_box, System.ComponentModel.ISupportInitialize).EndInit()
        Me.web_auth_opts.ResumeLayout(False)
        Me.web_auth_opts.PerformLayout()
        Me.web_Firefly_opts.ResumeLayout(False)
        CType(Me.domain_components_box, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.max_link_depth_box, System.ComponentModel.ISupportInitialize).EndInit()
        Me.hidden_scan_opt_tab.ResumeLayout(False)
        Me.unalloc_opts.ResumeLayout(False)
        Me.unalloc_when_found_box.ResumeLayout(False)
        Me.unalloc_when_found_box.PerformLayout()
        Me.unalloc_search_opts.ResumeLayout(False)
        Me.misc_opts_tab.ResumeLayout(False)
        Me.resource_limits_box.ResumeLayout(False)
        CType(Me.min_free_space, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.max_archive_unpack, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Preferences_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' take the existing set of preferences and set our GUI accordingly.
        reset_interface()
    End Sub

    Private Sub cancel_opts_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancel_opts_button.Click
        Me.Close()
    End Sub

    Private Sub reset_interface()
        ' called last after reset defaults is clicked; basically sets the whole GUI dispo according 
        ' to the currently active prefs.

        ' page by page

        ' run mode page
        ' do we want to disable parts of the interface for different modes?
        ' don't know yet ...
        If (FireflyRunMode And Configuration.RunMode.Disk) Then
            disk_obj_radio.Checked = True
            disk_scan_opts_tab.Enabled = True
        ElseIf (FireflyRunMode And Configuration.RunMode.Hidden) Then
            unalloc_obj_radio.Checked = True
            hidden_scan_opt_tab.Enabled = True
        ElseIf (FireflyRunMode And Configuration.RunMode.Web) Then
            web_obj_radio.Checked = True
            web_scan_opt_tab.Enabled = True
        End If

        ' process options
        If (My.Settings.FireflyRunPriority And Configuration.RunPriority.Low) Then
            low_prio_button.Checked = True
        ElseIf (My.Settings.FireflyRunPriority And Configuration.RunPriority.Normal) Then
            normal_prio_button.Checked = True
        ElseIf (My.Settings.FireflyRunPriority And Configuration.RunPriority.High) Then
            high_prio_button.Checked = True
        ElseIf (My.Settings.FireflyRunPriority And Configuration.RunPriority.Highest) Then
            highest_prio_button.Checked = True
        End If

        ' scan behavior

        If (FireflyRunOptions And Configuration.RunOptions.Minimize) Then
            minimize_when_running.Checked = True
        Else
            minimize_when_running.Checked = False
        End If

        ' when finished

        If (FireflyRunOptions And Configuration.RunOptions.ExitWhenDone) Then
            exit_radio.Checked = True
        ElseIf (FireflyRunOptions And Configuration.RunOptions.Restore) Then
            restore_window_radio.Checked = True
        ElseIf (FireflyRunOptions And Configuration.RunOptions.LaunchLogViewer) Then
            view_log_radio.Checked = True
        End If

        ' scan method, matching behavior
        ' certain log attributes can disable this;
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrScore) Or (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrTotalMatches) Then
            fastmatch_box.Checked = False
            fastmatch_box.Enabled = False
        Else
            fastmatch_box.Enabled = True
            If (FireflyRunOptions And Configuration.RunOptions.FastMatch) Then
                fastmatch_box.Checked = True
            Else
                fastmatch_box.Checked = False
            End If
        End If

        ' scan depth
        scan_depth_box.Value = ScanDepth

        ' incremental scanning and checkpointing only valid under disk mode
        If (FireflyRunMode And Configuration.RunMode.Disk) Then
            clear_incremental.Enabled = True
            clear_checkpoints.Enabled = True
            incremental_box.Enabled = True
            checkpoint_box.Enabled = True
            If (FireflyScanDiskOptions And Configuration.DiskModeOptions.Incremental) Then
                incremental_box.Checked = True
            Else
                incremental_box.Checked = False
            End If
            If (FireflyScanDiskOptions And Configuration.DiskModeOptions.CheckPoint) Then
                checkpoint_box.Checked = True
            Else
                checkpoint_box.Checked = False
            End If
        Else
            incremental_box.Checked = False
            incremental_box.Enabled = False
            checkpoint_box.Checked = False
            checkpoint_box.Enabled = False
            clear_incremental.Enabled = False
            clear_checkpoints.Enabled = False
        End If

        ' DISK MODE OPTIONS

        ' scan options tab, start directory
        startdir_box.Text = My.Settings.ScanDir

        ' scan options
        If (FireflyScanDiskOptions And Configuration.DiskModeOptions.AllLocalDrives) Then
            local_drives_box.Checked = True
        Else
            local_drives_box.Checked = False
        End If

        If (FireflyScanDiskOptions And Configuration.DiskModeOptions.Recurse) Then
            recurse_box.Checked = True
        Else
            recurse_box.Checked = False
        End If

        If (FireflyScanDiskOptions And Configuration.DiskModeOptions.NoAtime) Then
            reset_atimes_box.Checked = True
        Else
            reset_atimes_box.Checked = False
        End If

        ' if the match_time value is cleared, set the counters both to epoch
        ' otherwise, set them and the MAC time selector
        If Match_Time.GetMatchType = DateMatch.cleared Then
            time_select_lo.Value = DateTimePicker.MinDateTime
            time_select_hi.Value = DateTimePicker.MinDateTime
            mactime_box.ValueMember = "timestamp"
        Else
            time_select_lo.Value = Match_Time.GetDateLo
            time_select_hi.Value = Match_Time.GetDateHi
            Select Case Match_Time.GetMatchType
                Case DateMatch.accesstime
                    mactime_box.ValueMember = "Access Time"
                Case DateMatch.createtime
                    mactime_box.ValueMember = "Create Time"
                Case DateMatch.modifytime
                    mactime_box.ValueMember = "Modify Time"
            End Select
        End If

        ' WEB MODE OPTIONS

        web_start_url_box.Text = WebModeStartURL

        ' follow links
        If (FireflyWebModeOptions And Configuration.WebModeOptions.Recurse) Then
            web_recurse_box.Checked = True
        Else
            web_recurse_box.Checked = False
        End If

        ' max link depth box
        max_link_depth_box.Value = MaxURLDepth

        If (FireflyWebModeOptions And Configuration.WebModeOptions.RespectRobots) Then
            respect_robots.Checked = True
        Else
            respect_robots.Checked = False
        End If

        ' 
        If (FireflyWebModeOptions And Configuration.WebModeOptions.JumpDomain) Then
            jump_domain_box.Checked = True
        Else
            jump_domain_box.Checked = False
        End If

        domain_components_box.Value = DomainDepth

        ' we will want to populate the text field with what this means based on the URL
        ' they've provided. 
        ' we'll also want to set the max value for the scroller to be the number of
        ' domain components, i.e.:
        ' www.foo.bar.com = 4
        ' .foo.bar.com = 3
        ' .bar.com = 2
        ' .com = 1

        If (FireflyWebModeOptions And Configuration.WebModeOptions.SupplyBasicAuth) Then
            supply_basic_auth_box.Checked = True
            ' username and password, if we have them
            basic_auth_user.Enabled = True
            basic_auth_pass.Enabled = True
            basic_auth_user.Text = BasicAuthUser
            basic_auth_pass.Text = BasicAuthPassword
        Else
            supply_basic_auth_box.Checked = False
            basic_auth_user.Enabled = False
            basic_auth_pass.Enabled = False
        End If

        max_content_size_box.Value = MaxContentLength

        ' UA
        If (FireflyWebModeOptions And Configuration.WebModeOptions.ChangeUA) Then
            forge_user_agent.Checked = True
            useragent_box.Enabled = True
            useragent_box.Text = UserAgent
        Else
            forge_user_agent.Checked = False
            useragent_box.Enabled = False
            useragent_box.Text = UserAgent
        End If

        ' HIDDEN MODE OPTIONS

        If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.ScanADS) Then
            scan_ads.Checked = True
        Else
            scan_ads.Checked = False
        End If

        If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.ScanSlack) Then
            scan_slack.Checked = True
        Else
            scan_slack.Checked = False
        End If

        ' need noatime

        If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.ScanUnalloc) Then
            scan_unalloc.Checked = True
        Else
            scan_unalloc.Checked = False
        End If

        ' populate the box
        Dim drivelist() As String
        Dim i As Integer
        drivelist = Directory.GetLogicalDrives()
        For i = 0 To drivelist.Length - 1
            Me.scan_unalloc_drive.Items.Add(drivelist(i).TrimEnd("\"))
        Next

        If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.WipeUnallocMatchingBlocks) Then
            wipe_unalloc_match_blocks.Checked = True
        Else
            wipe_unalloc_match_blocks.Checked = False
        End If

        If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.PullUnalloctoFile) Then
            copy_unalloc_to_file.Checked = True
            unalloc_match_file.Enabled = True
            unalloc_file_select_button.Enabled = True
        Else
            copy_unalloc_to_file.Checked = False
            unalloc_match_file.Enabled = False
            unalloc_file_select_button.Enabled = False
        End If

        unalloc_match_file.Text = UnallocFile

        ' REGULAR EXPRESSIONS

        regex_text.Text = ""
        regex_description_box.Text = ""

        ' system canned regexes

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexAMEX) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("AMEX"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("AMEX"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexAMEX_b) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("AMEX_b"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("AMEX_b"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexNINO) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("NINO"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("NINO"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSIN333) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN333"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN333"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSIN333_b) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN333_b"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN333_b"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSIN9) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN9"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN9"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSIN9_b) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN9_b"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SIN9_b"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSSN324) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN324"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN324"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSSN324_b) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN324_b"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN324_b"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSSN9) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN9"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN9"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexSSN9_b) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN9_b"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("SSN9_b"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexVMCD) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("VMCD"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("VMCD"), False)
        End If

        If (FireflyRegexOptions And Configuration.LRegexOptions.RegexVMCD_b) Then
            system_regex_select.SetItemChecked(system_regex_select.FindString("VMCD_b"), True)
        Else
            system_regex_select.SetItemChecked(system_regex_select.FindString("VMCD_b"), False)
        End If

        '   custom regexes
        If (FireflyRegexOptions And Configuration.LRegexOptions.CustomRegexes) Then
            ' we have some, simply populate the box with them, iterating over the
            ' linked list

            Dim curRE As RegexEntry = RE_Custom_Start.Head
            While Not (curRE Is Nothing)
                ' all we really need to do is stick in the regex itself

                Me.custom_regex_tree.Items.Add(curRE.RegName)
                curRE = curRE.NextItem
            End While
        End If

        ''LOG OPTIONS, FILE TAB
        'If (FireflyReportOptions And Configuration.ReportOptions.WriteToFile) Then
        '    ' pretty much enables the rest of the things on this form.
        '    write_local_log.Checked = True
        '    ' enable everything else
        '    log_path.Enabled = True
        '    append_log_box.Enabled = True
        '    wipe_log.Enabled = True
        '    csv_log_box.Enabled = True
        '    select_log_path.Enabled = True
        '    annotate_log_button.Enabled = True
        '    log_attributes_box.Enabled = True
        'Else
        '    write_local_log.Checked = False
        '    ' disable everything else
        '    log_path.Enabled = False
        '    append_log_box.Enabled = False
        '    wipe_log.Enabled = False
        '    csv_log_box.Enabled = False
        '    select_log_path.Enabled = False
        '    annotate_log_button.Enabled = False
        '    log_attributes_box.Enabled = False
        'End If

        If (FireflyReportOptions And Configuration.ReportOptions.CsvReport) Then
            Me.csv_log_box.Checked = True
        Else
            Me.csv_log_box.Checked = False
        End If

        ' set the log attributes
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrAllHits) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Match Text Strings"), True)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrApp) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Application"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Application"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrAtime) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Access Time"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Access Time"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrCtime) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Create Time"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Create Time"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrHash) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File MD5"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File MD5"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrLocation) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Hit Location Pointer"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Hit Location Pointer"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrMatch) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Match Text Fragment"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Match Text Fragment"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrMtime) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Modify Time"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Modify Time"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrPath) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Path"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Path"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrRegex) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Regular Expression"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Regular Expression"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrScore) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File Score"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File Score"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrSize) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File Size"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File Size"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrTotalMatches) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Total Matches in File"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("Total Matches in File"), False)
        End If
        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrType) Then
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File Type"), True)
        Else
            log_attributes_box.SetItemChecked(log_attributes_box.FindString("File Type"), False)
        End If

        ' log path

        log_path.Text = ReportPath

        ' LOG OPTIONS, SYSLOG

        If (FireflyReportOptions And Configuration.ReportOptions.WriteToSysLog) Then
            send_syslog_box.Checked = True
            syslog_host.Enabled = True
            log_facility_box.Enabled = True
        Else
            send_syslog_box.Checked = False
            syslog_host.Enabled = False
            log_facility_box.Enabled = False
        End If

        syslog_host.Text = Loghost

        log_facility_box.SelectedText = Facility

        ' LOG OPTIONS, EVENT LOG
        If (FireflyReportOptions And Configuration.ReportOptions.WriteToEventLog) Then
            send_to_evt_box.Checked = True
            progress2evt_box.Enabled = True
        Else
            send_to_evt_box.Checked = False
            progress2evt_box.Enabled = False
        End If

        If (FireflyLogOptions And Configuration.LogOptions.LogProgress) Then
            progress2evt_box.Enabled = True
        Else
            progress2evt_box.Enabled = False
        End If

        ' resource limits
        min_free_space.Value = MinFreeGB
        max_archive_unpack.Value = MaxArchiveSize

        ' that ought to be it.
        Return

    End Sub
    Private Function grab_interface() As Boolean
        ' called to get all the button dispositions and set up for pushing to the registry
        ' here's where it gets dirty.

        ' we'll start by clearing all our options.  It's easier to set than set/unset
        FireflyScanDiskOptions = DiskModeOptions.cleared
        FireflyWebModeOptions = WebModeOptions.cleared
        FireflyHiddenModeOptions = HiddenModeOptions.cleared
        FireflyRegexOptions = LRegexOptions.cleared
        FireflyReportOptions = ReportOptions.cleared
        FireflyRunOptions = RunOptions.cleared
        FireflyRunMode = RunMode.cleared
        My.Settings.FireflyRunPriority = RunPriority.cleared
        My.Settings.FireflyLogAttributes = LogMask.cleared

        ' clear our hash tables, if we need to.

        ' start setting options, working our way across the interface
        Try
            ' run mode
            If Me.disk_obj_radio.Checked Then
                FireflyRunMode = Configuration.RunMode.Disk
            ElseIf Me.web_obj_radio.Checked Then
                FireflyRunMode = Configuration.RunMode.Web
            ElseIf Me.unalloc_obj_radio.Checked Then
                FireflyRunMode = Configuration.RunMode.Hidden
            End If

            ' process options, priority
            If Me.low_prio_button.Checked Then
                My.Settings.FireflyRunPriority = Configuration.RunPriority.Low
            ElseIf Me.normal_prio_button.Checked Then
                My.Settings.FireflyRunPriority = Configuration.RunPriority.Normal
            ElseIf Me.high_prio_button.Checked Then
                My.Settings.FireflyRunPriority = Configuration.RunPriority.High
            ElseIf Me.highest_prio_button.Checked Then
                My.Settings.FireflyRunPriority = Configuration.RunPriority.Highest
            End If

            ' minimize 
            If Me.minimize_when_running.Checked Then
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.Minimize
            End If

            ' restore/exit/view log
            If Me.restore_window_radio.Checked Then
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.Restore
            ElseIf Me.exit_radio.Checked Then
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.ExitWhenDone
            ElseIf Me.view_log_radio.Checked Then
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.LaunchLogViewer
            End If

            ' scan method, fast match

            If Me.fastmatch_box.Checked And Me.fastmatch_box.Enabled Then
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.FastMatch
            End If

            ScanDepth = Me.scan_depth_box.Value

            ' incremental/checkpoint

            If Me.incremental_box.Checked Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or Configuration.DiskModeOptions.Incremental
            End If

            If Me.checkpoint_box.Checked Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or Configuration.DiskModeOptions.CheckPoint
            End If

            ' scan options, disk tab

            My.Settings.ScanDir = Me.startdir_box.Text

            ' recurse
            If Me.recurse_box.Checked Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or Configuration.DiskModeOptions.Recurse
            End If

            ' local drives
            If Me.local_drives_box.Checked Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or Configuration.DiskModeOptions.AllLocalDrives
            End If

            ' noatime
            If Me.reset_atimes_box.Checked Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or Configuration.DiskModeOptions.NoAtime
            End If

            ' check the mactime box; if something other than "timestamp" is displayed, we need to 
            ' compare the two times, lo and hi.  They must be different and hi must be greater than lo; 
            ' otherwise, error
            Dim mactype As String = Me.mactime_box.SelectedItem

            If (mactype <> "timestamp" And mactype <> String.Empty) Then
                Dim tlo As Date
                Dim thi As Date
                tlo = Me.time_select_lo.Value
                thi = Me.time_select_hi.Value
                If tlo = thi Then
                    MsgBox("Selecting files by timestamp requires two different times.", MsgBoxStyle.Critical, My.Application.Info.ProductName)
                    Return False
                End If
                If thi < tlo Then
                    MsgBox("End date must be newer than start date.", MsgBoxStyle.Critical)
                    Return False
                End If
                ' otherwise
                Select Case mactype
                    Case "Create Time"
                        Match_Time.Add(tlo, thi, Scanner.DateMatch.createtime)
                    Case "Access Time"
                        Match_Time.Add(tlo, thi, Scanner.DateMatch.accesstime)
                    Case "Modify Time"
                        Match_Time.Add(tlo, thi, Scanner.DateMatch.modifytime)
                End Select
            End If

            ' Scan opts, Web tab
            WebModeStartURL = Me.web_start_url_box.Text

            ' descend links, etc.
            If Me.web_recurse_box.Checked Then
                FireflyWebModeOptions = FireflyWebModeOptions Or Configuration.WebModeOptions.Recurse
            End If

            ' robots

            If Me.respect_robots.Checked Then
                FireflyWebModeOptions = FireflyWebModeOptions Or Configuration.WebModeOptions.RespectRobots
            End If

            ' jump domain
            If Me.jump_domain_box.Checked Then
                FireflyWebModeOptions = FireflyWebModeOptions Or Configuration.WebModeOptions.JumpDomain
            End If

            ' max depths
            DomainDepth = Me.domain_components_box.Value
            MaxURLDepth = Me.max_link_depth_box.Value

            ' basic auth/user/pass
            If Me.supply_basic_auth_box.Checked Then
                FireflyWebModeOptions = FireflyWebModeOptions Or Configuration.WebModeOptions.SupplyBasicAuth
            End If

            ' basic auth user/pass
            BasicAuthUser = Me.basic_auth_user.Text
            BasicAuthPassword = Me.basic_auth_pass.Text

            ' content length
            MaxContentLength = Me.max_content_size_box.Value

            ' user agent

            If Me.forge_user_agent.Checked Then
                FireflyWebModeOptions = FireflyWebModeOptions Or Configuration.WebModeOptions.ChangeUA
            End If

            UserAgent = Me.useragent_box.Text

            ' scan options, hidden spaces

            If Me.scan_ads.Checked Then
                FireflyHiddenModeOptions = FireflyHiddenModeOptions Or Configuration.HiddenModeOptions.ScanADS
            End If

            ' scan slack/scan unalloc
            If Me.scan_slack.Checked Then
                FireflyHiddenModeOptions = FireflyHiddenModeOptions Or Configuration.HiddenModeOptions.ScanSlack
            End If

            If Me.scan_unalloc.Checked Then
                FireflyHiddenModeOptions = FireflyHiddenModeOptions Or Configuration.HiddenModeOptions.ScanUnalloc
            End If

            ' unalloc search drive
            If Me.scan_unalloc_drive.SelectedText <> String.Empty Then
                UnallocSearchDrive = Me.scan_unalloc_drive.SelectedText
            End If

            ' wipe unalloc match blocks
            If Me.wipe_unalloc_match_blocks.Checked Then
                FireflyHiddenModeOptions = FireflyHiddenModeOptions Or Configuration.HiddenModeOptions.WipeUnallocMatchingBlocks
            End If

            ' copy to file
            If Me.copy_unalloc_to_file.Checked Then
                FireflyHiddenModeOptions = FireflyHiddenModeOptions Or Configuration.HiddenModeOptions.PullUnalloctoFile
            End If

            UnallocFile = Me.unalloc_match_file.Text

            ' regexes, custom is saved directly through the form, so we only need to concern ourselves 
            ' with the system regexes.
            RE_Start.Clear()

            ' walk on through, adding regexes in order of appearance
            Dim regexitem As System.Windows.Forms.CheckedListBox.CheckedItemCollection = Me.system_regex_select.CheckedItems

            For Each rei As Object In regexitem
                Debug.WriteLine("SYSTEM REGEX: " + Me.system_regex_select.GetItemText(rei).ToString)
                Select Case Me.system_regex_select.GetItemText(rei).ToString
                    'Case "AMEX"
                    '    RE_Start.Add(reAMEX, "AMEX", "AMEX", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexAMEX
                    'Case "AMEX_b"
                    '    RE_Start.Add(reAMEX_break, "AMEX_b", "AMEX_b", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexAMEX_b
                    'Case "NINO"
                    '    RE_Start.Add(reNINO, "NINO", "NINO", Validator.None, ValidatorTypes.cleared)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexNINO
                    'Case "SIN333"
                    '    RE_Start.Add(reSIN333, "SIN333", "SIN333", Validator.CCN, ValidatorTypes.CanadianSIN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSIN333
                    'Case "SIN333_b"
                    '    RE_Start.Add(reSIN333_break, "SIN333_b", "SIN333_b", Validator.CCN, ValidatorTypes.CanadianSIN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSIN333_b
                    'Case "SIN9"
                    '    RE_Start.Add(reSSN9, "SIN9", "SIN9", Validator.CCN, ValidatorTypes.CanadianSIN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSIN9
                    'Case "SIN9_b"
                    '    RE_Start.Add(reSSN9_break, "SIN9_b", "SIN9_b", Validator.CCN, ValidatorTypes.CanadianSIN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSIN9_b
                    'Case "SSN324"
                    '    RE_Start.Add(reSSN324, "SSN324", "SSN324", Validator.SSN, ValidatorTypes.US_SSN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSSN324
                    'Case "SSN324_b"
                    '    RE_Start.Add(reSSN324_break, "SSN324_b", "SSN324_b", Validator.SSN, ValidatorTypes.US_SSN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSSN324_b
                    'Case "SSN9"
                    '    RE_Start.Add(reSSN9, "SSN9", "SSN9", Validator.SSN, ValidatorTypes.US_SSN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSSN9
                    'Case ("SSN9_b")
                    '    RE_Start.Add(reSSN9_break, "SSN9_b", "SSN9_b", Validator.SSN, ValidatorTypes.US_SSN)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexSSN9_b
                    'Case "VMCD"
                    '    RE_Start.Add(reVMCD, "VMCD", "VMCD", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexVMCD
                    'Case "VMCD_b"
                    '    RE_Start.Add(reVMCD_break, "VMCD_b", "VMCD_b", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
                    '    FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.RegexVMCD_b
                End Select
            Next


            If Me.custom_regex_tree.Items.Count > 0 Then
                FireflyRegexOptions = FireflyRegexOptions Or Configuration.LRegexOptions.CustomRegexes
            End If

            ' need to update FireflyRegexOptions while we're at it.

            '' log options, another mess
            'If Me.write_local_log.Checked Then
            '    FireflyReportOptions = FireflyReportOptions Or Configuration.ReportOptions.WriteToFile
            'End If

            ' LogFile location
            ReportPath = Me.log_path.Text

            ' append log

            If Me.append_log_box.Checked Then
                FireflyReportOptions = FireflyReportOptions Or Configuration.ReportOptions.AppendLog
            End If

            ' wipe before writing
            If Me.wipe_log.Checked Then
                FireflyReportOptions = FireflyReportOptions Or Configuration.ReportOptions.WipeLogBeforeUse
            End If

            ' CSV logging and options

            If Me.csv_log_box.Checked Then
                FireflyReportOptions = FireflyReportOptions Or Configuration.ReportOptions.CsvReport
            End If

            ' and now, the options
            Dim logitem As System.Windows.Forms.CheckedListBox.CheckedItemCollection = Me.log_attributes_box.CheckedItems

            For Each li As Object In logitem
                Select Case Me.log_attributes_box.GetItemText(li).ToString
                    Case "Access Time"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrAtime
                    Case "Application"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrApp
                    Case "Create Time"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrCtime
                    Case "File MD5"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrHash
                    Case "File Score"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrScore
                    Case "File Size"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrSize
                    Case "File Type"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrType
                    Case "Hit Location Pointer"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrLocation
                    Case "Match Text Fragment"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrMatch
                    Case "Modify Time"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrMtime
                    Case "Path"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrPath
                    Case "Regular Expression"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrRegex
                    Case "Total Matches in File"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrTotalMatches
                    Case "Match Text Strings"
                        My.Settings.FireflyLogAttributes = My.Settings.FireflyLogAttributes Or Configuration.LogMask.attrAllHits
                End Select
            Next

            ' syslog
            If Me.send_syslog_box.Checked Then
                FireflyReportOptions = FireflyReportOptions Or Configuration.ReportOptions.WriteToSysLog
            End If

            Loghost = Me.syslog_host.Text

            ' facility
            Facility = Me.log_facility_box.SelectedItem

            If Facility = String.Empty Then
                Facility = "facility"
            End If

            ' event log stuff

            If Me.send_to_evt_box.Checked Then
                FireflyReportOptions = FireflyReportOptions Or Configuration.ReportOptions.WriteToEventLog
            End If

            If Me.progress2evt_box.Checked Then
                FireflyLogOptions = FireflyLogOptions Or Configuration.LogOptions.LogProgress
            End If

            ' misc crap
            MinFreeGB = Me.min_free_space.Value
            MaxArchiveSize = Me.max_archive_unpack.Value
        Catch ex As Exception
            LogError(New Exception("An error occurred while saving settings.", ex))
            Return False
        End Try

        Return True

    End Function
    Private Sub reset_defs_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reset_defs_button.Click
        default_config()
        reset_interface()
        Return
    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scan_depth_box.ValueChanged

    End Sub

    Private Sub regex_tab_control_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles regex_tab_control.SelectedIndexChanged

    End Sub

    Private Sub save_opts_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles save_opts_button.Click
        If Not grab_interface() Then
            MsgBox("There was an error.  Configuration not saved.", MsgBoxStyle.Critical, My.Application.Info.ProductName)
            Return
        End If
        ' other things; write to registry, etc.
        If save_config() Then
            MsgBox("Configuration Saved", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
            Me.Close()
        Else
            MsgBox("Error saving Firefly configuration.", MsgBoxStyle.Critical, My.Application.Info.ProductName)
        End If

        Return
    End Sub

    Private Sub fastmatch_box_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fastmatch_box.CheckedChanged
        ' if fastmatching is checked, we automatically unselect log_score and log_totalhits

        If Me.fastmatch_box.CheckState = CheckState.Checked Then
            Me.log_attributes_box.SetItemChecked(log_attributes_box.FindString("Total Matches in File"), False)
            Me.log_attributes_box.SetItemChecked(log_attributes_box.FindString("File Score"), False)
        End If

    End Sub

    Private Sub web_start_url_box_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles web_start_url_box.TextChanged
        ' break the host part up into pieces and set the counter, only if follow_links is set
        If Me.jump_domain_box.CheckState = CheckState.Unchecked Then
            Return
        End If
        ' otherwise...
        ' awful way to do this ...
        If web_start_url_box.Text.IndexOf("//") < 0 Then
            Return
        End If

        Dim parts() As String = web_start_url_box.Text.Split("/")

        If parts.Length < 3 Or web_start_url_box.Text.EndsWith("//") Then
            Return
        End If

        Dim fooURI As New Uri(web_start_url_box.Text)
        Dim hostname As String = fooURI.Host
        Dim hostparts() As String = hostname.Split(".")



        Me.domain_components_box.Maximum = hostparts.Length
        Me.domain_components_box.Minimum = 1
        Me.domain_components_box.Value = hostparts.Length

        Me.domain_example_label.Text = hostname

    End Sub

    Private Sub domain_components_box_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles domain_components_box.ValueChanged
        ' only applies if the jump_domain box is checked
        If Me.jump_domain_box.CheckState = CheckState.Unchecked Then
            Return
        End If

        ' awful way to do this ...
        If web_start_url_box.Text.IndexOf("//") < 0 Then
            Return
        End If

        Dim parts() As String = web_start_url_box.Text.Split("/")

        If parts.Length < 3 Or web_start_url_box.Text.EndsWith("//") Then
            Return
        End If
        Dim fooURI As New Uri(web_start_url_box.Text)
        Dim hostname As String = fooURI.Host
        Dim hostparts() As String = hostname.Split(".")

        Dim j As Integer

        Debug.WriteLine("hostparts length: " + hostparts.Length.ToString)

        hostname = ""

        If hostparts.Length < domain_components_box.Value Then
            hostname = fooURI.Host
        Else
            ' take the last pieces
            For j = (hostparts.Length - domain_components_box.Value) To hostparts.Length - 1
                '          Debug.WriteLine("PIECE: " + tmpParts(j))
                hostname += "." + hostparts(j)
            Next
        End If

        If (domain_components_box.Value = domain_components_box.Maximum) Then
            hostname = hostname.TrimStart(".")
        End If

        Me.domain_example_label.Text = hostname

    End Sub

    Private Sub clear_incremental_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear_incremental.Click
        Throw New Exception("Incremental scan is not implemented in " + APP_NAME)
        '' we'll reset the incremental date back to the epoch
        'Dim regKey As RegistryKey

        'Try
        '    regKey = Registry.CurrentUser.CreateSubKey(REGISTRY_HOME)
        'Catch ex As Exception
        '    MsgBox("Firefly registry keys not found.", MsgBoxStyle.Critical, My.Application.Info.ProductName)
        '    Return
        'End Try

        'regKey.SetValue("LastScanDate", OUR_EPOCH.ToString)
        'regKey.Close()

        'MsgBox("Last scan date reset.", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
        'Return

    End Sub

    Private Sub clear_checkpoints_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clear_checkpoints.Click
        Dim regKey As RegistryKey

        Try
            regKey = Registry.CurrentUser.CreateSubKey(REGISTRY_HOME)
        Catch ex As Exception
            MsgBox("Firefly registry keys not found.", MsgBoxStyle.Critical, My.Application.Info.ProductName)
            Return
        End Try

        regKey.SetValue("CheckPointPath", "")
        regKey.Close()

        MsgBox("Last file scanned reset.", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
        Return
    End Sub

    Private Sub skip_exts_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles skip_exts_button.Click
        Dim frm6 As New FileTypesForm
        frm6.Show()
    End Sub

    Private Sub keep_exts_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keep_exts_button.Click
        Dim frm6 As New FileTypesForm
        frm6.Show()
    End Sub

    Private Sub skip_path_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles skip_path_button.Click
        Dim frm8 As New PathsToSkipForm
        Form8_State = Scanner.Form8_Dispo.SkipPaths
        frm8.Show()
    End Sub

    Private Sub supply_basic_auth_box_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles supply_basic_auth_box.CheckedChanged
        If Me.supply_basic_auth_box.CheckState = CheckState.Checked Then
            Me.basic_auth_pass.Enabled = True
            Me.basic_auth_user.Enabled = True
        End If
        If Me.supply_basic_auth_box.CheckState = CheckState.Unchecked Then
            Me.basic_auth_user.Enabled = False
            Me.basic_auth_pass.Enabled = False
        End If
    End Sub

    Private Sub forge_user_agent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles forge_user_agent.CheckedChanged
        If Me.forge_user_agent.CheckState = CheckState.Checked Then
            Me.useragent_box.Enabled = True
        End If
        If Me.forge_user_agent.CheckState = CheckState.Unchecked Then
            Me.useragent_box.Enabled = False
        End If
    End Sub

    Private Sub system_regex_select_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles system_regex_select.SelectedIndexChanged
        ' we'll grab the selected item, then update the text box with the regex and the description
        Dim msg As String = ""
        Dim selected_int As Object = Me.system_regex_select.SelectedItem
        Dim selected As String = Me.system_regex_select.GetItemText(selected_int)

        Select Case selected
            Case "AMEX"
                Me.regex_description_box.Text = AMEX_desc
                Me.regex_text.Text = AMEX_regex
            Case "AMEX_b"
                msg += AMEX_desc + vbCrLf
                msg += Break_desc
                Me.regex_description_box.Text = msg
                msg = "\b" + AMEX_regex + "\b"
                Me.regex_text.Text = msg
            Case "VMCD"
                Me.regex_description_box.Text = VMCD_desc
                Me.regex_text.Text = VMCD_regex
            Case "VMCD_b"
                msg += VMCD_desc + vbCrLf
                msg += Break_desc
                Me.regex_description_box.Text = msg
                msg = "\b" + VMCD_regex + "\b"
                Me.regex_text.Text = msg
            Case "SIN333"
                Me.regex_description_box.Text = SIN333_desc
                Me.regex_text.Text = SIN333_regex
            Case "SIN333_b"
                msg += SIN333_desc + vbCrLf
                msg += Break_desc
                Me.regex_description_box.Text = msg
                msg = "\b" + SIN333_regex + "\b"
                Me.regex_text.Text = msg
            Case "SIN9"
                Me.regex_description_box.Text = SIN9_desc
                Me.regex_text.Text = SIN9_regex
            Case "SIN9_b"
                msg += SIN9_desc + vbCrLf
                msg += Break_desc
                Me.regex_description_box.Text = msg
                msg = "\b" + SIN9_regex + "\b"
                Me.regex_text.Text = msg
            Case "SSN324"
                Me.regex_description_box.Text = SSN324_desc
                Me.regex_text.Text = SSN324_regex
            Case "SSN324_b"
                msg += SSN324_desc + vbCrLf
                msg += Break_desc
                Me.regex_description_box.Text = msg
                msg = "\b" + SSN324_regex + "\b"
                Me.regex_text.Text = msg
            Case "SSN9"
                Me.regex_description_box.Text = SSN9_desc
                Me.regex_text.Text = SSN9_regex
            Case "SSN9_b"
                msg += SSN9_desc + vbCrLf
                msg += Break_desc
                Me.regex_description_box.Text = msg
                msg = "\b" + SSN9_regex + "\b"
                Me.regex_text.Text = msg
            Case "NINO"
                Me.regex_description_box.Text = NINO_desc
                Me.regex_text.Text = NINO_regex
            Case Else
                Me.regex_text.Text = ""
                Me.regex_description_box.Text = ""
        End Select
    End Sub

    Private Sub write_local_log_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles write_local_log.CheckedChanged
        ' enable/disable
        Select Case Me.write_local_log.CheckState
            Case CheckState.Checked
                ' enable every else on the page
                Me.log_path.Enabled = True
                Me.select_log_path.Enabled = True
                Me.append_log_box.Enabled = True
                Me.wipe_log.Enabled = True
                Me.csv_log_box.Enabled = True
                Me.annotate_log_button.Enabled = True
                Me.log_attributes_box.Enabled = True
            Case CheckState.Unchecked
                ' disable everything else on the page
                Me.log_path.Enabled = False
                Me.select_log_path.Enabled = False
                Me.append_log_box.Enabled = False
                Me.wipe_log.Enabled = False
                Me.csv_log_box.Enabled = False
                Me.annotate_log_button.Enabled = False
                Me.log_attributes_box.Enabled = False
        End Select
    End Sub

    Private Sub log_attributes_box_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles log_attributes_box.SelectedIndexChanged
        ' Dim msg As String = ""
        Dim selected_int As Object = Me.log_attributes_box.SelectedItem
        Dim selected As String = Me.log_attributes_box.GetItemText(selected_int)
        Dim selected_state As Boolean = Me.log_attributes_box.GetItemChecked(Me.log_attributes_box.SelectedIndex)

        Select Case selected
            Case "Total Matches in File"
                If selected_state Then
                    Me.fastmatch_box.CheckState = CheckState.Unchecked
                Else
                    ' unchecked
                    Me.log_attributes_box.SetItemChecked(Me.log_attributes_box.FindString("File Score"), False)
                    Me.log_attributes_box.SetItemChecked(Me.log_attributes_box.FindString("Match Text Strings"), False)
                End If
            Case "File Score"
                ' if it's checked, check Total Matches and uncheck fastmatch
                If selected_state Then
                    ' checked
                    Me.log_attributes_box.SetItemChecked(Me.log_attributes_box.FindString("Total Matches in File"), True)
                    Me.fastmatch_box.CheckState = CheckState.Unchecked
                End If
            Case "Match Text Strings"
                ' if it's checked, check total matches and uncheck fastmatch
                If selected_state Then
                    Me.log_attributes_box.SetItemChecked(Me.log_attributes_box.FindString("Total Matches in File"), True)
                    Me.fastmatch_box.CheckState = CheckState.Unchecked
                End If
        End Select

    End Sub

    Private Sub send_syslog_box_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles send_syslog_box.CheckedChanged
        ' enable/disable the rest of the crap on the page
        Select Case Me.send_syslog_box.CheckState
            Case CheckState.Checked
                Me.log_facility_box.Enabled = True
                Me.syslog_host.Enabled = True
            Case CheckState.Unchecked
                Me.log_facility_box.Enabled = False
                Me.syslog_host.Enabled = False
        End Select
    End Sub

    Private Sub send_to_evt_box_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles send_to_evt_box.CheckedChanged
        Select Case Me.send_to_evt_box.CheckState
            Case CheckState.Checked
                Me.progress2evt_box.Enabled = True
            Case CheckState.Unchecked
                Me.progress2evt_box.Enabled = False
        End Select
    End Sub

    Private Sub startdir_select_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startdir_select_button.Click
        Dim path As String
        Dim FBD As New System.Windows.Forms.FolderBrowserDialog

        FBD.SelectedPath = My.Settings.ScanDir
        FBD.ShowDialog()

        path = FBD.SelectedPath

        If path = String.Empty Then
            Return
        End If

        My.Settings.ScanDir = path
        Me.startdir_box.Text = My.Settings.ScanDir.ToString
    End Sub

    Private Sub add_regex_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles add_regex_button.Click
        Dim revalid As Validator
        Dim re_extras As ValidatorTypes
        ' if the regex name already exists in the list, we'll throw an error

        If Me.custom_regex_tree.FindStringExact(Me.regex_name_box.Text) >= 0 Then
            MsgBox("Regular expressions must have unique names", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
            Return
        End If

        ' otherwise, push the name over and add everything to the list
        If Me.validator_ssn_button.Checked Then
            revalid = Validator.SSN
            re_extras = ValidatorTypes.US_SSN
        ElseIf Me.luhn_validator_button.Checked Then
            revalid = Validator.CCN
            re_extras = ValidatorTypes.CreditCardPrefixes
        ElseIf Me.sin_validator_radio.Checked Then
            revalid = Validator.CCN
            re_extras = ValidatorTypes.CanadianSIN
        Else
            revalid = Validator.None
            re_extras = ValidatorTypes.cleared
        End If

        RE_Custom_Start.Add(Nothing, Me.regex_box.Text, Me.regex_name_box.Text, revalid, re_extras)

        Me.custom_regex_tree.Items.Add(Me.regex_name_box.Text)
    End Sub

    Private Sub edit_regex_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles edit_regex_button.Click
        ' walk the linked list until we find what we need, then set the various items accordingly.
        Dim selected As String = Me.custom_regex_tree.GetItemText(Me.custom_regex_tree.SelectedItem)
        Dim curRE As RegexEntry = RE_Custom_Start.Head



        While Not (curRE Is Nothing)
            If curRE.RegName = selected Then
                ' that's us; populate the form
                Me.regex_name_box.Text = curRE.RegName
                Me.regex_box.Text = curRE.RegExText
                ' set the validator
                Select Case curRE.RegValidator
                    Case Validator.SSN
                        Me.validator_ssn_button.Checked = True
                    Case Validator.CCN
                        If curRE.RegValidatorExtra = ValidatorTypes.CanadianSIN Then
                            Me.sin_validator_radio.Checked = True
                        Else
                            Me.luhn_validator_button.Checked = True
                        End If
                End Select
                Exit While
            End If
            curRE = curRE.NextItem
        End While

    End Sub

    Private Sub delete_regex_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles delete_regex_button.Click
        ' we walk the tree until we get a match, then delete it
        Dim selected As String = Me.custom_regex_tree.GetItemText(Me.custom_regex_tree.SelectedItem)

        RE_Custom_Start.Remove(selected)

        Me.custom_regex_tree.Items.Remove(Me.custom_regex_tree.SelectedItem)

    End Sub

    Private Sub annotate_log_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles annotate_log_button.Click
        Dim LHI As String
        Dim frm9 As New LogOptionsForm
        Dim i As Integer

        i = frm9.ShowDialog
        Debug.WriteLine("got back: " + i.ToString)

        If i = Windows.Forms.DialogResult.OK Then
            LHI = frm9.Firefly_loghostinfo.Text
            If LHI.ToString <> String.Empty Then
                LogHostInfo = LHI
                Debug.WriteLine("set log host info")
                My.Settings.FireflyLogAttributes = (LogMask.attrHostinfo Or My.Settings.FireflyLogAttributes)
            Else
                My.Settings.FireflyLogAttributes = (LogMask.attrHostinfo Xor My.Settings.FireflyLogAttributes)
                LogHostInfo = ""
            End If
        End If

        Return
    End Sub

    Private Sub select_log_path_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles select_log_path.Click
        Dim path As String
        Dim FBD As New System.Windows.Forms.FolderBrowserDialog

        FBD.SelectedPath = ReportPath
        FBD.ShowDialog()

        path = FBD.SelectedPath

        If path = String.Empty Then
            Return
        End If

        ReportPath = path
        Me.log_path.Text = ReportPath.ToString

    End Sub

    Private Sub test_regex_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles test_regex_button.Click

        If Regex.IsMatch(Me.regex_test_data.Text, Me.regex_box.Text) Then
            Me.regex_test_label.Text = "MATCH"
        Else
            Me.regex_test_label.Text = "NO MATCH"
        End If

    End Sub

    Private Sub TabControl2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl2.SelectedIndexChanged

    End Sub

    Private Sub skipcontent_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles skipcontent_button.Click
        Dim frm8 As New PathsToSkipForm
        Form8_State = Scanner.Form8_Dispo.SkipContent
        frm8.Show()
    End Sub

    Private Sub log2file_options_tab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles log2file_options_tab.Click

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub web_other_opts_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles web_other_opts.Enter

    End Sub

    Private Sub local_drives_box_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles local_drives_box.CheckedChanged

    End Sub
End Class
