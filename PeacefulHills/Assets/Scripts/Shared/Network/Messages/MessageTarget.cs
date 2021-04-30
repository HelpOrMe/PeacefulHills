﻿using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public struct MessageTarget : IComponentData
    {
        public NetworkConnection Connection;
    }
}