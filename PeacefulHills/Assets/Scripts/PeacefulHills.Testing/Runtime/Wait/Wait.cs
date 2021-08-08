using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace PeacefulHills.Testing
{
    public static class Wait
    {
        private static readonly List<IWaitRequest> WaitRequests = new List<IWaitRequest>();

        public static async Task Ms(int ms)
        {
            await Task.Delay(ms);
        }

        public static async Task Sec(int secs)
        {
            await Task.Delay(secs * 1000);
        }

        public static async Task Frames(int frames)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            WaitRequests.Add(new WaitFramesRequest(frames, taskCompletionSource));
            await taskCompletionSource.Task;
        }

        public static async Task For(Func<bool> condition, int timeoutMs = 10)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            WaitRequests.Add(new WaitForRequest(condition, taskCompletionSource, timeoutMs));
            await taskCompletionSource.Task;
        }

        public static async Task<EntityQueryMutable<TComponent0>> For<TComponent0>(
            int timeoutMs = 10)
            where TComponent0 : unmanaged, IComponentData
        {
            var description = new EntityQueryDesc {All = new ComponentType[] {typeof(TComponent0)}};
            EntityQueryMutable queryMutable = await Wait.For(description, timeoutMs);
            return new EntityQueryMutable<TComponent0>(queryMutable);
        }
        
        public static async Task<EntityQueryMutable<TComponent0, TComponent1>> For<TComponent0, TComponent1>(
            int timeoutMs = 10)
            where TComponent0 : unmanaged, IComponentData
            where TComponent1 : unmanaged, IComponentData
        {
            var description = new EntityQueryDesc {All = new ComponentType[] {typeof(TComponent0), typeof(TComponent1)}};
            EntityQueryMutable queryMutable = await Wait.For(description, timeoutMs);
            return new EntityQueryMutable<TComponent0, TComponent1>(queryMutable);
        }
        
        public static async Task<EntityQueryMutable> For(EntityQueryDesc queryDesc, int timeoutMs = 10)
        {
            EntityQuery entityQuery = Worlds.Now.EntityManager.CreateEntityQuery(queryDesc);
            
            while (entityQuery.IsEmpty && timeoutMs > 0)
            {
                if (!Worlds.Now.EntityManager.IsQueryValid(entityQuery))
                {
                    entityQuery = Worlds.Now.EntityManager.CreateEntityQuery(queryDesc);
                }

                await Task.Delay(1);
                timeoutMs--;
            }

            return new EntityQueryMutable(entityQuery);
        }
    }
}