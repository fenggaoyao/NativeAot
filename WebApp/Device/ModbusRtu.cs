using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Device
{
    public class ModbusRtu : Modbus
    {
        /// <summary>端口</summary>
        public String PortName { get; set; }

        /// <summary>波特率</summary>
        public Int32 Baudrate { get; set; } = 9600;

        private SerialPort _port;

        protected override void Init()
        {
            if (_port == null)
            {
                var p = new SerialPort(PortName, Baudrate);
                p.ReadTimeout = 3_000;
                p.WriteTimeout = 3_000;
                p.Open();
                _port = p;

            }
        }

        /// <summary>发送两字节命令，并接收返回</summary>
        /// <param name="host"></param>
        /// <param name="code"></param>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Byte[] SendCommand(Byte host, Byte code, UInt16 address, UInt16 value)
        {
            Init();

            var cmd = new Byte[8];
            cmd[0] = host;
            cmd[1] = code;
            cmd[2] = (Byte)(address >> 8); //取高位
            cmd[3] = (Byte)(address & 0xFF); //取低位
            cmd[4] = (Byte)(value >> 8);
            cmd[5] = (Byte)(value & 0xFF);

            var crc = Crc(cmd, 0, cmd.Length - 2);
            cmd[6] = (Byte)(crc & 0xFF);
            cmd[7] = (Byte)(crc >> 8);

            {
                _port.Write(cmd, 0, cmd.Length);
                Thread.Sleep(90);
            }

            try
            {

                var rs = new Byte[32];
                var c = _port.Read(rs, 0, rs.Length);
                rs = rs.ReadBytes(0, c);
               

                if (rs.Length < 2 + 2) return null;

                // 校验Crc
                crc = Crc(rs, 0, rs.Length - 2);
                var crc2 = rs.ToUInt16(rs.Length - 2);

                return rs.ReadBytes(2, rs.Length - 2 - 2);
            }
            catch (TimeoutException) { return null; }
        }

        public static String[] GetPortNames() => SerialPort.GetPortNames();

        #region CRC
        private static readonly UInt16[] crc_ta = new UInt16[16] { 0x0000, 0xCC01, 0xD801, 0x1400, 0xF001, 0x3C00, 0x2800, 0xE401, 0xA001, 0x6C00, 0x7800, 0xB401, 0x5000, 0x9C01, 0x8801, 0x4400, };

        /// <summary>Crc校验</summary>
        /// <param name="data"></param>
        /// <param name="offset">偏移</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static UInt16 Crc(Byte[] data, Int32 offset, Int32 count = -1)
        {
            if (data == null || data.Length < 1) return 0;

            UInt16 u = 0xFFFF;
            Byte b;

            if (count == 0) count = data.Length - offset;

            for (var i = offset; i < count; i++)
            {
                b = data[i];
                u = (UInt16)(crc_ta[(b ^ u) & 15] ^ (u >> 4));
                u = (UInt16)(crc_ta[((b >> 4) ^ u) & 15] ^ (u >> 4));
            }

            return u;
        }
        #endregion
    }
}
