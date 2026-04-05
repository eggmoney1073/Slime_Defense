# 🧪 슬라임 디펜스 (Slime Defense)

> Unity ECS 기반 모바일 타워 디펜스 게임  
> 1인 개발 포트폴리오 프로젝트

<br>

## 📖 프로젝트 소개

왕국을 침공하는 슬라임 군단을 막아내는 고정형 타워 디펜스 게임입니다.  
플레이어는 화면 중앙에 고정된 마법사로, **터치 조작으로 방향을 조준**하며 밀려오는 슬라임을 처치합니다.  
뱀파이어 서바이버와 유사하게, 전투 중 무기를 획득하고 레벨업하며 성장하는 구조를 가집니다.

> 이 프로젝트의 핵심 목표는 **Unity DOTS(ECS)를 실제 게임에 적용**하고,  
> 다량의 투사체와 적 엔티티를 효율적으로 처리하는 구조를 직접 설계해보는 것입니다.

<br>

## 🎮 게임플레이

| 항목 | 내용 |
|------|------|
| 플랫폼 | 모바일 (Android / iOS) |
| 장르 | 타워 디펜스 + 서바이벌 성장형 |
| 플레이 시간 | 1판 약 5분 |
| 시점 | 탑뷰 고정 |
| 조작 | 터치 조이스틱으로 조준 방향 제어 |

**승리 조건:** 5분 동안 생존하며 슬라임을 모두 처치  
**루프 구조:** 전투 → 경험치 획득 → 레벨업 (3지선다 무기 선택) → 반복

<br>

## 🛠 기술 스택

```
Engine   : Unity 6000.3.11f1
언어     : C#
렌더링   : 2D 스프라이트 (URP)
아키텍처 : Unity DOTS / ECS (Entities 패키지)
리소스   : Addressables
입력     : Unity Input System
```

<br>

## 🏗 아키텍처 개요

이 프로젝트는 **ECS(Entity Component System)** 를 중심으로 설계되었습니다.  
투사체·적 등 대량으로 생성되는 오브젝트를 Entity로 처리하여 성능을 확보합니다.

```
Assets/
└── 2.Scripts/
    ├── ECS/
    │   ├── Authorings/       # MonoBehaviour → Entity 변환 데이터 정의
    │   ├── Bakers/           # Authoring을 Entity로 굽는 Baker 클래스
    │   ├── Entity Structs/   # IComponentData 구조체 정의
    │   ├── Entity Systems/   # ISystem 업데이트 로직
    │   │   ├── Job/          # IJobEntity (병렬 처리)
    │   │   └── WeaponFire/   # 무기 발사 시스템
    │   └── Bridge/           # ECS ↔ MonoBehaviour 데이터 공유
    ├── OOP/                  # 프로토타입용 MonoBehaviour 구현체
    ├── Loading/              # Addressables 씬/리소스 로딩
    ├── PopUp/                # 팝업 UI 시스템
    ├── Input/                # Input System 핸들러
    └── Utility/              # 오브젝트 풀, 싱글톤 등 공용 유틸
```

<br>

## ⚙️ 주요 시스템

### 🔫 무기 & 발사 시스템
- `ECS_WeaponSpawnSystem` — 게임 시작 시 무기 프리팹을 Entity로 일괄 생성
- `ECS_ManualFireSystem` — 터치 조이스틱 방향(`AimDirectionBridge`)을 읽어 투사체 발사
- `WeaponEnabledTag (IEnableableComponent)` — 무기 활성/비활성을 컴포넌트 활성화로 제어

### 👾 적 스폰 & 이동 시스템
- `ECS_EnemySpawnSystem` — 인터벌마다 적 Entity를 스폰, 웨이포인트 경로 연결
- `ECS_EnemyMoveSystem` + `EnemyMoveJob` — 웨이포인트 기반 이동을 `IJobEntity`로 병렬 처리
- `ECS_PathReference / ECS_WayPoint` — 경로 데이터를 DynamicBuffer로 공유

### 🚀 투사체 시스템
- `Projectile_Linear_MoveSystem` — `[BurstCompile]` 적용, 수천 개의 투사체를 병렬 이동 처리
- `LifetimeData` — 수명이 다한 투사체를 `EntityCommandBuffer`로 일괄 제거

### 💥 충돌 시스템
- `GridBuildJob` — 공간 분할 그리드(`NativeParallelMultiHashMap`)로 충돌 탐색 범위 최적화
- `ECS_CollisionSystem` — 그리드 기반 투사체 ↔ 적 충돌 감지 (구현 진행 중)

### 📦 리소스 & 씬 관리
- `LoadingSystem` — Addressables 초기화, 씬 로드/언로드 흐름 관리
- `DownLoadManager` — 최초 실행 시 패치 파일 다운로드 및 진행률 표시

<br>

## 🔗 ECS ↔ Mono 브릿지

ECS 시스템 데이터를 UI 등 MonoBehaviour에서 읽기 위해 정적 브릿지 클래스를 사용합니다.

```csharp
// ECS System에서 쓰기
AimDirectionBridge.SetAimDirection(x, z);
EnemyCount.SetCount(query.CalculateEntityCount());

// MonoBehaviour(UI)에서 읽기
_countText.text = EnemyCount.Count.ToString();
```

<br>

## 📱 조작 방법

| 조작 | 설명 |
|------|------|
| 화면 터치 & 드래그 | 조이스틱 생성 및 조준 방향 제어 |
| (자동) | 쿨다운마다 조준 방향으로 자동 발사 |
| 레벨업 시 | 3가지 무기/스킬 중 하나 선택 |

<br>

## 🧩 개발 현황

- [x] ECS 적 스폰 & 웨이포인트 이동
- [x] ECS 투사체 발사 & 이동 & 수명 제거
- [x] 터치 조이스틱 조준 연동
- [x] Addressables 리소스 / 씬 로딩 파이프라인
- [x] 그리드 기반 충돌 시스템 (GridBuildJob)
- [ ] 충돌 판정 완성 (CollisionJob 연결)
- [ ] 레벨업 3지선다 UI
- [ ] 적 처치 & 경험치 시스템
- [ ] 스테이지 / 웨이브 데이터 설계
- [ ] 게임 오버 / 클리어 흐름

<br>

## 👤 개발 정보

| 항목 | 내용 |
|------|------|
| 개발 인원 | 1인 |
| 그래픽 | AI 이미지 생성 활용 |
| 사운드 | 무료 에셋 |
| 수익 모델 | 광고 기반 부활 시스템 |

<br>

---

> 포트폴리오 목적의 개인 프로젝트입니다.
