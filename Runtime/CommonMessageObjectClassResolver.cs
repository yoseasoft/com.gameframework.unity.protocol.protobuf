/// <summary>
/// Game Framework
/// 
/// 创建者：Hurley
/// 创建时间：2025-11-10
/// 功能描述：
/// </summary>

using System.Reflection;
using GameEngine;
using GameEngine.Loader.Symboling;

using SystemType = System.Type;

namespace Game.Module.Protocol.Protobuf
{
    /// <summary>
    /// 消息对象类解析器通用对象类
    /// </summary>
    public class CommonMessageObjectClassResolver : ISymbolResolverOfInstantiationClass
    {
        public bool Matches(SystemType targetType)
        {
            if (typeof(ProtoBuf.Extension.IMessage).IsAssignableFrom(targetType))
            {
                return true;
            }

            return false;
        }

        public void Resolve(SymClass symbol)
        {
            if (symbol.HasAttribute(typeof(ProtoBuf.Extension.MessageAttribute)))
            {
                int opcode = 0, responseCode = 0;
                ProtoBuf.Extension.MessageAttribute messageAttr = symbol.ClassType.GetCustomAttribute<ProtoBuf.Extension.MessageAttribute>();
                if (null != messageAttr)
                    opcode = messageAttr.Opcode;

                ProtoBuf.Extension.MessageResponseTypeAttribute messageResponseTypeAttr = symbol.ClassType.GetCustomAttribute<ProtoBuf.Extension.MessageResponseTypeAttribute>();
                if (null != messageResponseTypeAttr)
                    responseCode = messageResponseTypeAttr.Opcode;

                Debugger.Log("消息对象类‘{%s}’自动绑定‘MessageObjectAttribute’特性标签操作完成，操作码：{%d}，响应码：{%d}。", symbol.FullName, opcode, responseCode);

                // symbol.AddFeatureType(typeof(MessageObjectAttribute));
                MessageObjectAttribute attribute = new MessageObjectAttribute(opcode, responseCode);
                symbol.AddFeatureObject(attribute);
            }
        }
    }
}
