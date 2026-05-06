using Unity.Entities;

// =================================================================

// 0. 관리 그룹
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class ManageSystemGroup : ComponentSystemGroup { }

// 1. 생성 그룹
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(ManageSystemGroup))]
public partial class SpawnSystemGroup : ComponentSystemGroup { }

// 2. 이동 그룹
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(SpawnSystemGroup))]
public partial class MoveSystemGroup : ComponentSystemGroup { }

// 3. 판정 그룹
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(MoveSystemGroup))]
public partial class JudgementSystemGroup : ComponentSystemGroup { }

// 4. 대미지 계산 그룹
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(JudgementSystemGroup))]
public partial class CalculateSystemGroup : ComponentSystemGroup { }

// 5. 삭제 그룹 
[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(CalculateSystemGroup))]
public partial class DestroySystemGroup : ComponentSystemGroup { }

// =================================================================
