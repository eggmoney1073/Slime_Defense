using System.Collections;
using Unity.Entities;
using UnityEngine;

public static class EcsWorldResetter
{
    public static IEnumerator ResetDefaultWorldRoutine()
    {
        World oldWorld = World.DefaultGameObjectInjectionWorld;

        if (oldWorld != null && oldWorld.IsCreated)
        {
            Debug.Log($"Dispose ECS World: {oldWorld.Name}");

            // PlayerLoop에서 기존 World 시스템 제거
            ScriptBehaviourUpdateOrder.RemoveWorldFromCurrentPlayerLoop(oldWorld);

            // 기본 World 참조 제거
            World.DefaultGameObjectInjectionWorld = null;

            // World 제거
            oldWorld.Dispose();

            // PlayerLoop 변경은 다음 루프부터 적용되므로 한 프레임 대기
            yield return null;
        }

        // 새 Default World 생성
        World newWorld = DefaultWorldInitialization.Initialize("Default World", false);

        Debug.Log($"New ECS World Created: {newWorld.Name}");

        yield return null;
    }
}