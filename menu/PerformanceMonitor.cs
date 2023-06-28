using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class PerformanceMonitor : Form
{
    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int HOTKEY_ID = 9000;
    private const uint MOD_ALT = 0x0001;
    private const uint VK_W = 0x57;

    private HardwareMonitor hardwareMonitor;
    private PerformanceInfoPanel performanceInfoPanel;
    private TemperaturePanel temperaturePanel;
    private UsagePanel cpuUsagePanel;
    private UsagePanel gpuUsagePanel;
    private UsagePanel ramUsagePanel;
    private UsagePanel uploadUsagePanel;
    private UsagePanel downloadUsagePanel;

    private System.Windows.Forms.Timer refreshTimer;

    public PerformanceMonitor()
    {
        RegisterHotKey(this.Handle, HOTKEY_ID, MOD_ALT, VK_W);
        this.MouseClick += (s, e) => this.Hide();

        // Set the window size and position
        this.StartPosition = FormStartPosition.Manual;
        this.Location = new System.Drawing.Point(0, 0);
        this.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width / 4, Screen.PrimaryScreen.Bounds.Height);

        // Remove the window border and set the background color to black
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = Color.Black;

        // Initialize hardware monitor
        hardwareMonitor = new HardwareMonitor();

        // Initialize performance info panel
        performanceInfoPanel = new PerformanceInfoPanel()
        {
            Location = new System.Drawing.Point(10, 10)
        };
        this.Controls.Add(performanceInfoPanel);
        // Initialize temperature panel
        temperaturePanel = new TemperaturePanel()
        {
            Location = new System.Drawing.Point(10, performanceInfoPanel.Bottom + 10),
            Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035))
        };
        this.Controls.Add(temperaturePanel);

        // Initialize CPU usage panel
        cpuUsagePanel = new UsagePanel("CPU", "CPU Total", Color.FromKnownColor(KnownColor.Blue))
        {
            Location = new System.Drawing.Point(10, temperaturePanel.Bottom + 10),
            Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035))
        };
        this.Controls.Add(cpuUsagePanel);

        // Initialize GPU usage panel
        gpuUsagePanel = new UsagePanel("GPU", "GPU Core", Color.FromKnownColor(KnownColor.Green))
        {
            Location = new System.Drawing.Point(10, cpuUsagePanel.Bottom + 10),
            Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035))
        };
        this.Controls.Add(gpuUsagePanel);

        // Initialize RAM usage panel
        ramUsagePanel = new UsagePanel("RAM", "Memory Used", "Memory Available", Color.FromKnownColor(KnownColor.Red))
        {
            Location = new System.Drawing.Point(10, gpuUsagePanel.Bottom + 10),
            Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035))
        };
        this.Controls.Add(ramUsagePanel);

        // Initialize upload usage panel
        uploadUsagePanel = new UsagePanel("Upload", "Data Uploaded", Color.FromKnownColor(KnownColor.Orange))
        {
            Location = new System.Drawing.Point(10, ramUsagePanel.Bottom + 10),
            Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035))
        };
        this.Controls.Add(uploadUsagePanel);

        // Initialize download usage panel
        downloadUsagePanel = new UsagePanel("Download", "Data Downloaded", Color.FromKnownColor(KnownColor.Purple))
        {
            Location = new System.Drawing.Point(10, uploadUsagePanel.Bottom + 10),
            Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035))
        };
        this.Controls.Add(downloadUsagePanel);

        refreshTimer = new System.Windows.Forms.Timer();
        refreshTimer.Interval = 1000; // 1 second
        refreshTimer.Tick += RefreshTimer_Tick;
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        // Adjust the position and size of the panels based on the window size
        temperaturePanel.Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035));
        cpuUsagePanel.Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035));
        gpuUsagePanel.Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035));
        ramUsagePanel.Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035));
        uploadUsagePanel.Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035));
        downloadUsagePanel.Size = new System.Drawing.Size(this.Width - 20, (int)(this.Height * 0.035));
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref mVoici la dernière partie du code:

```csharp
        if (m.Msg == 0x0312 && m.WParam.ToInt32() == HOTKEY_ID)
        {
            // Alt+W was pressed
            if (this.Visible)
            {
                this.Hide();
                StopRefreshing();
            }
            else
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                StartRefreshing();
                performanceInfoPanel.UpdatePerformanceInformation(hardwareMonitor);
            }
        }
    }

    private void StartRefreshing()
    {
        refreshTimer.Start();
        RefreshTimer_Tick(null, EventArgs.Empty); // Trigger immediate refresh
    }

    private void StopRefreshing()
    {
        refreshTimer.Stop();
    }

    private void RefreshTimer_Tick(object sender, EventArgs e)
    {
        UpdateUsagePanels();
        UpdateTemperaturePanel();
    }

    private void UpdateUsagePanels()
    {
        cpuUsagePanel.UpdateUsage(hardwareMonitor.GetCPUUsage());
        gpuUsagePanel.UpdateUsage(hardwareMonitor.GetGPUUsage());
        ramUsagePanel.UpdateUsage(hardwareMonitor.GetRAMUsage());
        uploadUsagePanel.UpdateUsage(hardwareMonitor.GetUploadUsage());
        downloadUsagePanel.UpdateUsage(hardwareMonitor.GetDownloadUsage());
    }

    private void UpdateTemperaturePanel()
    {
        temperaturePanel.UpdateTemperature(hardwareMonitor.GetGPUTemperature());
    }

    protected override void Dispose(bool disposing)
    {
        UnregisterHotKey(this.Handle, HOTKEY_ID);
        StopRefreshing();
        if (disposing)
        {
            hardwareMonitor.Dispose();
        }
        base.Dispose(disposing);
    }
}

class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new PerformanceMonitor());
    }
}
