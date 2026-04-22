# 🟢 슬라임 디펜스 (Slime Defense)

> Unity ECS(DOTS) 기반 모바일 타워 디펜스 게임  
> 1인 개발 | 뱀파이어 서바이버 스타일 성장형 디펜스

<br/>

## 📌 프로젝트 소개

왕국을 침공하는 슬라임 무리를 막는 초급 마법사 이야기.  
플레이어는 중앙에 고정된 채 무기를 획득하고 성장하며, **5분 동안 생존**하면 승리합니다.

Unity **ECS / DOTS** 구조를 실제 게임에 적용하고 검증하는 것을 핵심 목표로 개발하였습니다.

<br/>

## 🎮 게임 플레이

| 항목 | 내용 |
|------|------|
| 플랫폼 | 모바일 (Android / iOS) |
| 장르 | 타워 디펜스 + 서바이벌 성장형 |
| 플레이 인원 | 1인 오프라인 |
| 1판 플레이 타임 | 약 5분 |
| 시점 | 탑뷰 고정 시점 |

**게임 루프**
```
전투 → 경험치 획득 → 레벨업(3지선다) → 강화 → 클리어 / 패배
```

<br/>

## 🛠️ 사용 기술

| 기술 | 용도 |
|------|------|
| Unity 6000.3.11f1 | 게임 엔진 |
| Unity ECS / DOTS | 투사체·적 Entity 처리 (고성능 병렬 처리) |
| Addressables | 씬·에셋 비동기 로딩, 원격 빌드 |
| Unity Input System | 조이스틱·터치 입력 처리 |
| URP (Universal Render Pipeline) | 2D 스프라이트 렌더링 |

<br/>

## 📁 프로젝트 구조

```
Assets/
├── 1.Scenes/               # 씬 파일 (Title, MainMenu, Game, Loading)
├── 2.Scripts/
│   ├── ECS/
│   │   ├── Authorings/     # MonoBehaviour → Entity 변환 (Baker)
│   │   ├── Bakers/         # Baker 구현체
│   │   ├── Bridge/         # ECS ↔ Mono 데이터 브릿지
│   │   ├── Entity Structs/ # IComponentData 구조체 정의
│   │   └── Entity Systems/ # ISystem 구현체
│   │       ├── 1.Spawn/    # 적·무기 스폰
│   │       ├── 2.Move/     # 이동 Job
│   │       ├── 3.Judgement/# 충돌 판정 (Grid 기반)
│   │       ├── 4.Calculate/# 데미지 계산·생존 체크
│   │       └── 5.Dead/     # Entity 제거
│   ├── Loading/            # Addressables 씬 로딩 시스템
│   ├── DownLoad/           # 원격 에셋 다운로드 관리
│   ├── Input/              # 입력 핸들러 (InGame / UI)
│   ├── UI/                 # UI MonoBehaviour (카운터 표시 등)
│   └── Utility/            # ObjectPool, Singleton 유틸
├── 3.InputSystem/          # Input Action Asset
├── 4.Prefabs/              # 프리팹 (Enemy, Weapon, UI 등)
└── AddressableAssetsData/  # Addressables 설정
```

<br/>

## ⚙️ ECS 시스템 플로우

```
SlimeDefenseSystemGroup (SimulationSystemGroup 하위)
│
├── SpawnSystemGroup        ← EnemySpawn, WeaponSpawn, ManualFire
├── MoveSystemGroup         ← EnemyMove, ProjectileMove
├── JudgementSystemGroup    ← Grid 기반 Collision 판정
├── CalculateSystemGroup    ← EnemyDamaged, ProjectileLiveCheck, EnemyDead
└── DestroySystemGroup      ← EntityDestroy (LiveTag 비활성 Entity 제거)
```

**충돌 처리 방식**: 공간을 Grid로 분할하여 `NativeParallelMultiHashMap`으로 적 위치를 관리, 투사체와의 충돌을 병렬 Job으로 처리합니다.

<br/>

## 🗓️ MVP 개발 일정

> 시작일: 2026.03.22 | 예정 기간: 8주 → **실제 핵심 시스템 2주 완성**

| 상태 | 작업 | 기간 | 완료일 |
|------|------|------|--------|
| ✅ | Addressables 구조 설계 및 씬 로딩 시스템 | 4일 (03.22 ~ 03.25) | 2026.03.25 |
| ✅ | 플레이어 조준 · 투사체 발사 | 2일 (03.26 ~ 03.27) | 2026.03.27 |
| ✅ | 적 스폰 · 웨이포인트 이동 | 1일 (03.28) | 2026.03.28 |
| ✅ | 충돌 · 데미지 · 사망 처리 | 7일 (03.29 ~ 04.04) | 2026.04.04 |
| ✅ | 경험치 · 레벨업 · 3지선다 UI | 13일 (04.10 ~ 04.22) | 2026.04.22 |
| 🔲 | 타이머 · 승리/패배 처리 | - | - |
| 🔲 | 강화 시스템 · 상점 UI | - | - |
| 🔲 | UI 폴리싱 · 사운드 · 최적화 · 광고 부활 | - | - |

<br/>

## 🧩 주요 구현 내용

### ECS 투사체 처리
- `ProjectileTag`, `LifetimeData`, `PierceData` 컴포넌트로 투사체 수명·관통 관리
- `IJobEntity`를 활용한 Burst 컴파일 병렬 이동 처리
- `IEnableableComponent` (LiveTag) 기반 소프트 삭제 → 프레임 말 일괄 제거

### Addressables 씬 관리
- 씬 전환은 모두 Addressables를 통해 비동기 로드
- 원격 서버 빌드 지원 (Remote BuildPath / LoadPath 설정)
- 초기 실행 시 다운로드 크기 체크 후 사용자에게 안내

### Grid 기반 충돌 시스템
- 매 프레임 적 위치를 `NativeParallelMultiHashMap<int, Entity>`에 기록
- 투사체 주변 셀만 조회하여 O(n) 충돌 탐색 최소화
- 명중 시 `Damaged` 동적 버퍼에 데미지 누적 → 다음 시스템에서 일괄 계산

<br/>

## 📦 빌드 환경

```
Unity       : 6000.3.11f1
Render Pipeline : URP
Target Platform : Android / iOS
ECS Package : com.unity.entities
Addressables : com.unity.addressables
Input System : com.unity.inputsystem
```

<br/>

## 👤 개발자

1인 개발 프로젝트  
그래픽: AI 이미지 생성 활용  
사운드: 무료 에셋 사용

<br/>

## 📝 개발 회고

> 개발하면서 겪은 문제와 해결 과정을 기록합니다.

- [✅] ECS와 Addressables 동시 사용 시 SubScene 로드 타이밍 이슈

        - SubScene Loader를 사용

- [✅] Burst Compile 조건에서 static 접근 제한

        - Burst Compile 을 제거한 코드에서 static 사용

- [✅] Entity의 Render Sort 가 무작위로 정렬되는 이슈

        - Entity의 Position을 카메라 기준으로 정렬

- [✅] 삭제 예정 Entity를 DeadTag의 활성화로 구현하려고 했지만 Spawner Job에서 Tag 비활성화가 어려움

        - IEnableComponent는 생성과 동시에 활성화 되기 때문에 LiveTag로 전환하여 해결

- [✅] Enemy의 Damage 처리를 투사체에서 Damage를 버퍼에 Add 하는데, 여러 투사체에서 Add를 병렬 처리하다가 동시에 참조하는 문제

        - 투사체에선 Damage Add Request를 하고 Damage System에서 병렬로 Add하는 것으로 변경
        
<br/>

## 📸 스크린샷

> 인게임 스크린샷

![스크린샷1](ScreenShot/Caputre1.png) ![스크린샷2](ScreenShot/Caputre2.png)

<br/>

---

> 본 프로젝트는 Unity ECS/DOTS를 실전 게임에 적용하여 구조와 성능을 검증하는 것을 목적으로 합니다.
