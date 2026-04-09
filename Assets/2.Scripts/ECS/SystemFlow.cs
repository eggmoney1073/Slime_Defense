using Unity.Entities;

// =================================================================

// 시스템 그룹 정의
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class SlimeDefenseSystemGroup : ComponentSystemGroup { }

// =================================================================

// 0. 관리 그룹
[UpdateInGroup(typeof(SlimeDefenseSystemGroup))]
public partial class ManageSystemGroup : ComponentSystemGroup { }

// 1. 생성 그룹
[UpdateInGroup(typeof(SlimeDefenseSystemGroup))]
[UpdateAfter(typeof(ManageSystemGroup))]
public partial class SpawnSystemGroup : ComponentSystemGroup { }

// 2. 이동 그룹
[UpdateInGroup(typeof(SlimeDefenseSystemGroup))]
[UpdateAfter(typeof(SpawnSystemGroup))]
public partial class MoveSystemGroup : ComponentSystemGroup { }

// 3. 판정 그룹
[UpdateInGroup(typeof(SlimeDefenseSystemGroup))]
[UpdateAfter(typeof(MoveSystemGroup))]
public partial class JudgementSystemGroup : ComponentSystemGroup { }

// 4. 대미지 계산 그룹
[UpdateInGroup(typeof(SlimeDefenseSystemGroup))]
[UpdateAfter(typeof(JudgementSystemGroup))]
public partial class CalculateSystemGroup : ComponentSystemGroup { }

// 5. 삭제 그룹 
[UpdateInGroup(typeof(SlimeDefenseSystemGroup))]
[UpdateAfter(typeof(CalculateSystemGroup))]
public partial class DestroySystemGroup : ComponentSystemGroup { }

// =================================================================
