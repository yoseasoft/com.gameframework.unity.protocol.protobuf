/// <summary>
/// Game Framework
/// 
/// 创建者：Hurley
/// 创建时间：2025-11-11
/// 功能描述：
/// </summary>

namespace Game.Module.Protocol.Protobuf
{
    /// <summary>
    /// 程序集的管理模块对象类
    /// </summary>
    public class GameModule : GameEngine.IHotModule
    {
        /// <summary>
        /// 初始化回调函数
        /// </summary>
        public void Startup()
        {
            GameEngine.GameApi.SetMessageProtocolType(typeof(ProtoBuf.Extension.IMessage));
            GameEngine.GameApi.RegisterMessageTranslator<TcpMessageTranslator>((int) NovaEngine.NetworkServiceType.Tcp);

            GameEngine.Loader.CodeLoader.RegisterSymbolResolverOfInstantiationClass<CommonMessageObjectClassResolver>();
        }

        /// <summary>
        /// 清理回调函数
        /// </summary>
        public void Shutdown()
        {
            GameEngine.GameApi.UnregisterMessageTranslator((int) NovaEngine.NetworkServiceType.Tcp);

            GameEngine.Loader.CodeLoader.UnregisterSymbolResolverOfInstantiationClass<CommonMessageObjectClassResolver>();
        }
    }
}
