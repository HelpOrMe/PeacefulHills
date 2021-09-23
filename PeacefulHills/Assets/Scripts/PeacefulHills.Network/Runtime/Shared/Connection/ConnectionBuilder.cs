using PeacefulHills.Extensions;
using PeacefulHills.Network;
using PeacefulHills.Network.Packet;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Networking.Transport;
using UnityEngine;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<ConnectionBuilder>))]

namespace PeacefulHills.Network
{
    [BurstCompatible]
    public struct ConnectionBuilder : IWorldExtension, INativeDisposable
    {
        private NativeList<PacketType> _packetTypes;
        private int _maxId;

        public ConnectionBuilder(Allocator allocator)
        {
            _packetTypes = new NativeList<PacketType>(allocator);
            _maxId = 0;
        }

        /// <summary>
        /// Add packet whose agent will be added to the agents pool of a new connection.
        /// Does not affect already created connections.
        /// </summary>
        public void AddPacket(PacketType packet)
        {
            _packetTypes.Add(packet);
            _maxId = math.max(_maxId, packet.Id);
        }

        /// <remarks>
        /// Does not affect already created connections.
        /// </remarks>
        public void RemovePacket(PacketType packet)
        {
            if (_packetTypes[_maxId].Equals(packet))
            {
                _packetTypes.RemoveAt(_maxId);
                _maxId--;
                return;
            }
            
            _maxId = 0;
                
            for (int id = 0; id < _packetTypes.Length; id++)
            {
                PacketType otherPacket = _packetTypes[packet.Id];
                if (otherPacket.Equals(packet))
                {
                    _packetTypes.RemoveAt(id);
                }
                else
                {
                    _maxId = math.max(_maxId, otherPacket.Id);
                }
            }
        }

        /// <summary>
        /// Creates connection entity with <see cref="DriverConnection"/>, <see cref="PacketAgentsPool"/> and
        /// packet agents as described in the PacketAgentPool.
        /// </summary>
        public Entity CreateConnection(EntityCommandBuffer commandBuffer, NetworkConnection connection)
        {
            Entity connectionEntity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent(connectionEntity, new DriverConnection { Value = connection });

            DynamicBuffer<PacketAgentsPool> agentsPool = commandBuffer.AddBuffer<PacketAgentsPool>(connectionEntity);
            agentsPool.ResizeUninitialized(_maxId + 1);
    
            for (int i = 0; i < _packetTypes.Length; i++)
            {
                PacketType packetType = _packetTypes[i];
                agentsPool[packetType.Id] = new PacketAgentsPool
                {
                    Entity = CreatePacketAgent(commandBuffer, connectionEntity, packetType)
                };
            }

            return connectionEntity;
        }

        public Entity CreatePacketAgent(EntityCommandBuffer commandBuffer, Entity connection, PacketType packet)
        {
            Entity agentEntity = commandBuffer.CreateEntity();
            commandBuffer.AddBuffer<PacketSendBuffer>(agentEntity);
            commandBuffer.AddBuffer<PacketReceiveBuffer>(agentEntity);
            commandBuffer.AddComponent(agentEntity, new ConnectionLink { Entity = connection });
            commandBuffer.AddComponent(agentEntity, packet.AgentComponents);
            return agentEntity;
        }

        #if !UNITY_SERVER || UNITY_EDITOR

        /// <summary>
        /// Client only.
        /// The same as <see cref="CreateConnection"/> but does not create send buffers in packet agents and mark
        /// connection entity with <see cref="HostConnection"/> tag.
        /// </summary>
        public Entity CreateHostConnection(EntityCommandBuffer commandBuffer, NetworkConnection connection)
        {
            Entity connectionEntity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent(connectionEntity, new DriverConnection { Value = connection });
            commandBuffer.AddComponent<HostConnection>(connectionEntity);

            DynamicBuffer<PacketAgentsPool> agentsPool = commandBuffer.AddBuffer<PacketAgentsPool>(connectionEntity);
            agentsPool.ResizeUninitialized(_maxId + 1);

            for (int i = 0; i < _packetTypes.Length; i++)
            {
                PacketType packetType = _packetTypes[i];
                agentsPool[packetType.Id] = new PacketAgentsPool
                {
                    Entity = CreateHostPacketAgent(commandBuffer, connectionEntity, packetType)
                };
            }

            return connectionEntity;
        }
        
        /// <remark>
        /// Client only.
        /// </remark>
        public Entity CreateHostPacketAgent(EntityCommandBuffer commandBuffer, Entity connection, PacketType packet)
        {
            Entity agentEntity = commandBuffer.CreateEntity();
            commandBuffer.AddBuffer<PacketReceiveBuffer>(agentEntity);
            commandBuffer.AddComponent(agentEntity, new ConnectionLink { Entity = connection });
            commandBuffer.AddComponent(agentEntity, packet.AgentComponents);
            return agentEntity;
        }
        
        #endif
        
        public void Dispose()
        {
            _packetTypes.Dispose();
        }

        public JobHandle Dispose(JobHandle inputDeps)
        {
            return _packetTypes.Dispose(inputDeps);
        }
    }
}