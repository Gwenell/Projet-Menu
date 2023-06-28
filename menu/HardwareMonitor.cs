using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;

public class HardwareMonitor : IDisposable
{
    private Computer computer;

    public HardwareMonitor()
    {
        computer = new Computer();
        computer.Open();
    }

    public void Dispose()
    {
        computer.Close();
    }

    public float GetGPUTemperature()
    {
        var gpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.GpuNvidia || h.HardwareType == HardwareType.GpuAmd);
        if (gpu != null)
        {
            gpu.Update();
            var sensor = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature);
            if (sensor != null && sensor.Value.HasValue)
            {
                return sensor.Value.Value;
            }
        }
        return 0f;
    }

    public float GetCPUUsage()
    {
        var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
        if (cpu != null)
        {
            cpu.Update();
            var sensor = cpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "CPU Total");
            if (sensor != null && sensor.Value.HasValue)
            {
                return sensor.Value.Value;
            }
        }
        return 0f;
    }

    public float GetGPUUsage()
    {
        var gpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.GpuNvidia || h.HardwareType == HardwareType.GpuAmd);
        if (gpu != null)
        {
            gpu.Update();
            var sensor = gpu.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load && s.Name == "GPU Core");
            if (sensor != null && sensor.Value.HasValue)
            {
                return sensor.Value.Value;
            }
        }
        return 0f;
    }

    public float GetRAMUsage()
    {
        var ram = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Memory);
        if (ram != null)
        {
            ram.Update();
            var usedSensor = ram.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Used");
            var availableSensor = ram.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Memory Available");
            if (usedSensor != null && usedSensor.Value.HasValue && availableSensor != null && availableSensor.Value.HasValue)
            {
                var used = usedSensor.Value.Value;
                var available = availableSensor.Value.Value;
                if (available > 0)
                {
                    return (used / available) * 100f;
                }
            }
        }
        return 0f;
    }

    public float GetUploadUsage()
    {
        var network = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Network);
        if (network != null)
        {
            network.Update();
            var sensor = network.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Data Uploaded");
            if (sensor != null && sensor.Value.HasValue)
            {
                return sensor.Value.Value;
            }
        }
        return 0f;
    }

    public float GetDownloadUsage()
    {
        var network = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Network);
        if (network != null)
        {
            network.Update();
            var sensor = network.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Data && s.Name == "Data Downloaded");
            if (sensor != null && sensor.Value.HasValue)
            {
                return sensor.Value.Value;
            }
        }
        return 0f;
    }
}