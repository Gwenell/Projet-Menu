using System.Drawing;
using System.Windows.Forms;

public class UsagePanel : Panel
{
    private ProgressBar usageProgressBar;
    private Label nameLabel;
    private Label valueLabel;

    public UsagePanel(string name, string sensorName, Color color)
    {
        this.BackColor = ColorTranslator.FromHtml("#2C2C2C");
        this.Padding = new Padding(5);
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Height = 50;

        usageProgressBar = new ProgressBar()
        {
            Location = new Point(10, 20),
            Size = new Size(this.Width - 20, 20),
            Minimum = 0,
            Maximum = 100,
            Style = ProgressBarStyle.Continuous,
            ForeColor = color
        };
        this.Controls.Add(usageProgressBar);

        nameLabel = new Label()
        {
            Location = new Point(10, 0),
            AutoSize = true,
            Text = name,
            ForeColor = Color.White
        };
        this.Controls.Add(nameLabel);

        valueLabel = new Label()
        {
            Location = new Point(10, usageProgressBar.Bottom),
            AutoSize = true,
            ForeColor = Color.White
        };
        this.Controls.Add(valueLabel);

        SetSensorName(sensorName);
    }

    public void SetSensorName(string sensorName)
    {
        nameLabel.Text = sensorName;
    }

    public void UpdateUsage(float value)
    {
        usageProgressBar.Value = (int)value;
        valueLabel.Text = $"{value:0}%";
        UpdateTextColor();
    }

    private void UpdateTextColor()
    {
        if (usageProgressBar.Value < 50)
        {
            valueLabel.ForeColor = Color.White;
        }
        else
        {
            valueLabel.ForeColor = Color.Black;
        }
    }
}