using System.Drawing;
using System.Windows.Forms;

public class TemperaturePanel : Panel
{
    private ProgressBar temperatureProgressBar;
    private Label valueLabel;

    public TemperaturePanel()
    {
        this.BackColor = ColorTranslator.FromHtml("#2C2C2C");
        this.Padding = new Padding(5);
        this.BorderStyle = BorderStyle.FixedSingle;
        this.Height = 50;

        temperatureProgressBar = new ProgressBar()
        {
            Location = new Point(10, 20),
            Size = new Size(this.Width - 20, 20),
            Minimum = 0,
            Maximum = 100,
            Style = ProgressBarStyle.Continuous
        };
        this.Controls.Add(temperatureProgressBar);

        valueLabel = new Label()
        {
            Location = new Point(10, temperatureProgressBar.Bottom),
            AutoSize = true,
            ForeColor = Color.White
        };
        this.Controls.Add(valueLabel);
    }

    public void UpdateTemperature(float value)
    {
        temperatureProgressBar.Value = (int)value;
        valueLabel.Text = $"{value:0}°C";
        UpdateColorGradient();
    }

    private void UpdateColorGradient()
    {
        int hue = (int)(((temperatureProgressBar.Value - temperatureProgressBar.Minimum) /
            (float)(temperatureProgressBar.Maximum - temperatureProgressBar.Minimum)) * 240);
        temperatureProgressBar.ForeColor = ColorFromHSL(hue, 1f, 0.5f);
    }

    private Color ColorFromHSL(int hue, float saturation, float lightness)
    {
        float chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
        float huePrime = hue / 60f;
        float x = chroma * (1 - Math.Abs(huePrime % 2 - 1));
        float red = 0;
        float green = 0;
        float blue = 0;

        if (huePrime >= 0 && huePrime < 1)
        {
            red = chroma;
            green = x;
        }
        else if (huePrime >= 1 && huePrime < 2)
        {
            red = x;
            green = chroma;
        }
        else if (huePrime >= 2 && huePrime < 3)
        {
            green = chroma;
            blue = x;
        }
        else if (huePrime >= 3 && huePrime < 4)
        {
            green = x;
            blue = chroma;
        }
        else if (huePrime >= 4 && huePrime < 5)
        {
            red = x;
            blue = chroma;
        }
        else if (huePrime >= 5 && huePrime < 6)
        {
            red = chroma;
            blue = x;
        }

        float lightnessAdjustment = lightness - chroma / 2;
        red += lightnessAdjustment;
        green += lightnessAdjustment;
        blue += lightnessAdjustment;

        return Color.FromArgb((int)(red * 255), (int)(green * 255), (int)(blue * 255));
    }
}