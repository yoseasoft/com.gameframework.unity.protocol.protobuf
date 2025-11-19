/// <summary>
/// Game Framework
/// 
/// 创建者：Hurley
/// 创建时间：2025-11-10
/// 功能描述：
/// </summary>

using System.Customize.Extension;

namespace Game.Module.Protocol.Protobuf
{
    /// <summary>
    /// 套接字类型通道的消息解析器对象类，用于对套接字通道的网络消息数据进行加工
    /// </summary>
    public abstract class SocketMessageTranslator : GameEngine.IMessageTranslator
    {
        /// <summary>
        /// 消息操作码的长度，以字节为单位
        /// </summary>
        private const int OpcodeSize = 2;

        /// <summary>
        /// 将指定的消息内容编码为可发送的消息字节流
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>若编码有效的数据则返回其对应的字节流，否则返回null</returns>
        public byte[] Encode(object message)
        {
            ProtoBuf.Extension.IMessage msg = message as ProtoBuf.Extension.IMessage;
            GameEngine.Debugger.Assert(null != msg, NovaEngine.ErrorText.InvalidArguments);

            int opcode = GameEngine.NetworkHandler.Instance.GetOpcodeByMessageType(message.GetType());
            byte[] msgBytes = ProtoBuf.Extension.ProtobufHelper.ToBytes(message);

            byte[] buffer = new byte[msgBytes.Length + OpcodeSize];
            buffer.WriteToBig(0, (ushort) opcode);
            for (int n = 0; n < msgBytes.Length; ++n)
            {
                buffer[n + OpcodeSize] = msgBytes[n];
            }

            return buffer;
        }

        /// <summary>
        /// 将指定的消息字节流解码成消息内容
        /// </summary>
        /// <param name="buffer">消息字节流</param>
        /// <returns>返回解码后的消息内容，若解码失败则返回null</returns>
        public object Decode(byte[] buffer)
        {
            // short opcode = System.BitConverter.ToInt16(buffer, 0);
            // opcode = System.Net.IPAddress.NetworkToHostOrder(opcode);
            int opcode = buffer.ReadBigUInt16(0);
            System.Type messageType = GameEngine.NetworkHandler.Instance.GetMessageClassByType(opcode);

            object message = ProtoBuf.Extension.ProtobufHelper.FromBytes(messageType, buffer, OpcodeSize, buffer.Length - OpcodeSize);

            return message;
        }
    }
}
