
namespace WebApp.Device
{
    /// <summary>串口</summary>
    public abstract class Modbus
    {
        /// <summary>性能追踪器</summary>

        protected virtual void Init() { }

        /// <summary>写入单线圈，0x05</summary>
        /// <param name="host">主机。一般是1</param>
        /// <param name="address">地址。例如0x0002</param>
        /// <param name="value">值。一般是 0xFF00/0x0000</param>
        /// <returns></returns>
        public Byte[] WriteSingleCoil(Byte host, UInt16 address, UInt16 value)
        {

            var rs = SendCommand(host, 0x05, address, value);
            if (rs == null || rs.Length <= 0) return null;

            return rs.ReadBytes(1, rs[0]);
        }

        /// <summary>写入单寄存器，0x06</summary>
        /// <param name="host">主机。一般是1</param>
        /// <param name="address">地址。例如0x0002</param>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public Byte[] WriteRegister(Byte host, UInt16 address, UInt16 value)
        {

            var rs = SendCommand(host, 0x06, address, value);
            if (rs == null || rs.Length <= 0) return null;

            return rs;
        }

        /// <summary>读取线圈，0x01</summary>
        /// <param name="host">主机。一般是1</param>
        /// <param name="address">地址。例如0x0002</param>
        /// <param name="count">线圈个数</param>
        /// <returns></returns>
        public Byte[] ReadCoil(Byte host, UInt16 address, UInt16 count)
        {

            var rs = SendCommand(host, 0x01, address, count);
            if (rs == null || rs.Length <= 0) return null;

            return rs.ReadBytes(1, rs[0]);
        }

        /// <summary>读取保持寄存器，0x03</summary>
        /// <param name="host">主机。一般是1</param>
        /// <param name="address">地址。例如0x0002</param>
        /// <param name="count">寄存器个数。每个寄存器2个字节</param>
        /// <returns></returns>
        public Byte[] ReadHoldingRegister(Byte host, UInt16 address, UInt16 count)
        {

            var rs = SendCommand(host, 0x03, address, count);
            if (rs == null || rs.Length <= 0) return null;

            return rs.ReadBytes(1, rs[0]);
        }

        /// <summary>发送两字节命令，并接收返回</summary>
        /// <param name="host">主机。一般是1</param>
        /// <param name="code">功能码</param>
        /// <param name="address">地址。例如0x0002</param>
        /// <param name="value">数据值</param>
        /// <returns></returns>
        public abstract Byte[] SendCommand(Byte host, Byte code, UInt16 address, UInt16 value);

       
    }
}