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
        public static async Task Ms(int ms)
        {
            await Task.Delay(ms);
        }

        public static async Task Sec(int secs)
        {
            await Task.Delay(secs * 1000);
        }

        public static async Task For(Func<bool> condition, int timeoutMs = 1000, Action timeoutAction = null)
        {
            while (!condition())
            {
                await Task.Delay(1);

                if (timeoutMs-- == 0)
                {
                    timeoutAction?.Invoke();
                    throw new TimeoutException();
                }
            }
        }

        public static async Task<EntityQueryMutable<TComponent0>> For<TComponent0>(
            int timeoutMs = 100, Action timeoutAction = null)
            where TComponent0 : unmanaged, IComponentData
        {
            var description = new EntityQueryDesc {All = new ComponentType[] {typeof(TComponent0)}};
            EntityQueryMutable entityQueryMutable = await Wait.For(description, timeoutMs, timeoutAction);
            return new EntityQueryMutable<TComponent0>(entityQueryMutable);
        }
        
        public static async Task<EntityQueryMutable<TComponent0, TComponent1>> For<TComponent0, TComponent1>(
            int timeoutMs = 10, Action timeoutAction = null)
            where TComponent0 : unmanaged, IComponentData
            where TComponent1 : unmanaged, IComponentData
        {
            var description = new EntityQueryDesc {All = new ComponentType[] {typeof(TComponent0), typeof(TComponent1)}};
            EntityQueryMutable entityQueryMutable = await Wait.For(description, timeoutMs, timeoutAction);
            return new EntityQueryMutable<TComponent0, TComponent1>(entityQueryMutable);
        }
        
        public static async Task<EntityQueryMutable> For(
            EntityQueryDesc queryDesc, int timeoutMs = 100, Action timeoutAction = null)
        {
            EntityQuery entityQuery = Worlds.Current.EntityManager.CreateEntityQuery(queryDesc);
            
            while (entityQuery.IsEmpty)
            {
                if (!Worlds.Current.EntityManager.IsQueryValid(entityQuery))
                {
                    entityQuery = Worlds.Current.EntityManager.CreateEntityQuery(queryDesc);
                }

                await Task.Delay(1);
                
                if (timeoutMs-- == 0)
                {
                    timeoutAction?.Invoke();
                    throw new TimeoutException();
                }
            }

            return new EntityQueryMutable(entityQuery);
        }
    }
}