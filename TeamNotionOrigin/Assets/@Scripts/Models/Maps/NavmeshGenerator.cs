using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


namespace Dungeon
{
    public class NavmeshGenerator
    {
        public NavMeshSurface Surface2D { get; private set; }
        public CollectSources2d Source2D { get; private set; }
        public CollectSourcesCache2d Cache2D { get; private set; }

        private Action<AsyncOperation> _callback;

        public void GenerateAsync(Action<AsyncOperation> callback = null)
        {
            var modifier = Main.Dungeon.DungeonGenerator.Background.Map.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = 1;

            modifier = Main.Dungeon.DungeonGenerator.Room.Map.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = 0;

            modifier = Main.Dungeon.DungeonGenerator.Road.Map.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = 0;

            modifier = Main.Dungeon.DungeonGenerator.Wall.Map.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = 1;

            modifier = Main.Dungeon.DungeonGenerator.Obstacle.Map.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = 1;

            _callback = callback;

            Surface2D = new GameObject(typeof(NavMeshSurface).Name).AddComponent<NavMeshSurface>();
            Surface2D.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            Source2D = Surface2D.AddComponent<CollectSources2d>();
            Cache2D = Surface2D.AddComponent<CollectSourcesCache2d>();
            Surface2D.transform.Rotate(-90, 0, 0);
            Surface2D.StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            // 던전 생성이 끝날 때까지 기다림
            yield return new WaitWhile(() => !Main.Dungeon.DungeonGenerator.IsDone);

            if (Surface2D.useGeometry == NavMeshCollectGeometry.PhysicsColliders)
                yield return new WaitForFixedUpdate();
            var op = Surface2D.BuildNavMeshAsync();
            op.allowSceneActivation = false;
            op.completed += op =>
            {
                op.allowSceneActivation = true;
                _callback?.Invoke(op);
            };
            yield return null;
        }
    }
}