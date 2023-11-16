using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Device
{
    public class A2
    {
        /// <summary>RS485串口1</summary>
        public static Modbus COM1 { get; } = new ModbusRtu { PortName = "/dev/ttyAMA0" };

        /// <summary>RS485串口2</summary>
        public static Modbus COM2 { get; } = new ModbusRtu { PortName = "/dev/ttyAMA1" };

        /// <summary>RS485串口3</summary>
        public static Modbus COM3 { get; } = new ModbusRtu { PortName = "/dev/ttyAMA2" };

        /// <summary>RS485串口4</summary>
        public static Modbus COM4 { get; } = new ModbusRtu { PortName = "/dev/ttyAMA3" };
    }
}
