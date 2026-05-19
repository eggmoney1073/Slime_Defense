# Slime Defense

Unity ECS/DOTS를 사용해 제작한 모바일 디펜스 게임입니다.

플레이어는 중앙에 고정된 상태로 몰려오는 슬라임을 막고, 전투 중 얻은 경험치로 무기를 강화하며 제한 시간 동안 생존해야 합니다.

이 프로젝트에서는 게임 기능을 구현하는 것뿐 아니라, 적과 투사체가 많이 늘어나는 상황에서도 전투 흐름이 안정적으로 처리되도록 구조를 나누는 데 집중했습니다.

## 링크

| 항목 | 링크 |
| --- | --- |
| 플레이 영상 | https://www.youtube.com/watch?v=FuFSvaY_o_8&t=170s |
| 플레이 파일 다운로드 | https://github.com/eggmoney1073/Slime_Defense/releases/tag/v1.0.0 |

## 프로젝트 정보

| 항목 | 내용 |
| --- | --- |
| 프로젝트명 | Slime Defense |
| 장르 | 모바일 디펜스, 서바이벌 성장형 |
| 플랫폼 | Android |
| 개발 인원 | 1인 개발 |
| 개발 기간 | 2026.03.22 ~ 2026.05.07 |
| 개발 도구 | Unity 6000.3.11f1 |
| 주요 목표 | ECS 기반 전투 구조 구현, 충돌 판정 최적화, Android 실기기 검증 |

## 게임 진행 방식

```text
전투 시작
-> 적 웨이브 스폰
-> 플레이어 공격 및 투사체 처리
-> 적 처치 후 경험치 획득
-> 레벨업 보상 선택
-> 무기 성능 강화
-> 제한 시간 생존 시 클리어
```

## 개발하면서 신경 쓴 부분

- 적과 투사체를 Entity로 관리하는 ECS 기반 전투 구조
- 전투 처리 순서를 명확하게 나누기 위한 SystemGroup 구성
- 적과 투사체 수가 늘어났을 때를 고려한 Grid 기반 충돌 판정
- 투사체 관통 수, 수명, 데미지 계산 흐름 분리
- Addressables 기반 씬 로딩
- ECS SubScene의 Android 빌드 포함 문제 확인 및 해결
- APK 빌드 후 실제 Android 기기에서 실행 검증

## 사용 기술

| 기술 | 사용 목적 |
| --- | --- |
| Unity 6000.3.11f1 | 게임 엔진 |
| C# | 게임 로직 구현 |
| Unity ECS / DOTS | Entity, Component, System 기반 전투 로직 구현 |
| Unity Entities | 적, 투사체, 전투 데이터 처리 |
| Burst Compiler | ECS Job 최적화 |
| Job System | 반복 연산 병렬 처리 |
| Addressables | 씬 및 리소스 비동기 로딩 |
| Input System | 입력 처리 |
| URP | 2D 렌더링 환경 구성 |
| Android / ADB | APK 빌드 및 실기기 실행 확인 |

## 프로젝트 구조

```text
Assets/
├─ 1.Scenes/
│  ├─ Scene_Title
│  ├─ Scene_Loading
│  ├─ Scene_Lobby
│  └─ Scene_Game
│
├─ 2.Scripts/
│  ├─ ECS/
│  │  ├─ Authorings/
│  │  ├─ Bakers/
│  │  ├─ Bridge/
│  │  ├─ Entity Structs/
│  │  ├─ Entity Systems/
│  │  │  ├─ 1.Spawn/
│  │  │  ├─ 2.Move/
│  │  │  ├─ 3.Judgement/
│  │  │  ├─ 4.Calculate/
│  │  │  ├─ 5.Dead/
│  │  │  └─ Manage/
│  │  └─ SystemFlow.cs
│  │
│  ├─ Input/
│  ├─ LevelUp/
│  ├─ Loading/
│  ├─ UI/
│  └─ Utility/
│
├─ 3.InputSystem/
├─ 4.Prefabs/
└─ AddressableAssetsData/
```

## ECS 시스템 흐름

전투 로직은 `SimulationSystemGroup` 안에서 아래 순서로 실행되도록 나눴습니다.

```text
ManageSystemGroup
-> SpawnSystemGroup
-> MoveSystemGroup
-> JudgementSystemGroup
-> CalculateSystemGroup
-> DestroySystemGroup
```

| 그룹 | 역할 |
| --- | --- |
| ManageSystemGroup | 시간, 사운드, 강화 요청 등 전투 관리 |
| SpawnSystemGroup | 적, 무기, 투사체 생성 |
| MoveSystemGroup | 적과 투사체 이동 |
| JudgementSystemGroup | 충돌 판정 |
| CalculateSystemGroup | 데미지, 관통 수, 생존 상태 계산 |
| DestroySystemGroup | 비활성 Entity 제거 |

전투 단계를 이렇게 나눈 이유는 생성, 이동, 판정, 계산, 삭제가 한 프레임 안에서 섞이지 않게 하기 위해서입니다.

## 주요 구현 내용

### ECS 기반 적 스폰

적은 `EnemySpawner` 데이터를 기준으로 생성됩니다.

스폰 시 `EntityCommandBuffer`를 사용해 Entity를 생성하고, 위치, 체력, 경로 참조, 데미지 버퍼를 함께 설정했습니다.

```text
EnemySpawner
-> EntityCommandBuffer.Instantiate
-> LocalTransform 설정
-> ECS_PathReference 설정
-> EnemyHealth 설정
-> Damaged Buffer 추가
```

적 이동과 데미지 계산에 필요한 값은 Component로 나누고, 같은 종류의 Entity를 System에서 일괄 처리할 수 있도록 구성했습니다.

### Grid 기반 충돌 판정

모든 투사체가 모든 적을 검사하면 적과 투사체 수가 늘어날수록 검사량이 빠르게 증가합니다.

이를 줄이기 위해 적의 위치를 Grid Cell에 등록하고, 투사체는 자신이 있는 Cell 주변만 검사하도록 구현했습니다.

```text
Enemy 위치 등록
-> Grid Cell Index 계산
-> NativeParallelMultiHashMap에 Enemy Entity 저장
-> Projectile 주변 3x3 Cell 조회
-> 거리 계산 후 충돌 여부 판단
```

충돌 거리는 제곱 거리로 비교해 불필요한 제곱근 계산을 피했습니다. 또한 이미 맞은 적은 `HitEnemyBufferElement`에 기록해 같은 투사체가 같은 적을 중복 타격하지 않도록 처리했습니다.

### 데미지 계산 분리

충돌 판정 단계에서는 적의 체력을 직접 변경하지 않고, `Damaged` Buffer에 데미지를 기록합니다.

이후 `EnemyDamaged_System`에서 Buffer에 누적된 데미지를 읽어 체력에 반영하고, 계산이 끝나면 Buffer를 비웁니다.

```text
Collision_Job
-> Damaged Buffer에 데미지 기록

EnemyDamaged_System
-> Damaged Buffer 순회
-> EnemyHealth 감소
-> Buffer Clear
```

충돌 판정과 데미지 계산을 분리해 각 시스템이 맡는 역할을 단순하게 유지했습니다.

### 투사체 관통 및 생존 처리

투사체는 충돌한 적을 `HitEnemyBufferElement`에 기록합니다.

기록된 적 수가 최대 관통 수에 도달하면 `LiveTag`를 비활성화하고, 이후 Destroy 단계에서 제거되도록 처리했습니다.

```text
HitEnemyBufferElement 개수 확인
-> maxPierceCount와 비교
-> 조건 충족 시 LiveTag 비활성화
-> DestroySystemGroup에서 Entity 제거
```

Entity를 바로 삭제하지 않고 생존 상태를 먼저 바꾸는 방식으로 처리해 삭제 흐름을 한 단계로 모았습니다.

### 레벨업 보상 적용

레벨업 UI는 MonoBehaviour에서 처리하고, 실제 무기 능력치 변경은 ECS 쪽으로 요청을 보내는 방식으로 구성했습니다.

```text
레벨업 UI 선택
-> UpgradeRequest Entity 생성
-> Upgrade_System에서 요청 처리
-> 무기 능력치 변경
```

강화 항목은 공격 속도, 데미지, 관통 수, 투사체 수로 구성했습니다.

### Addressables 기반 씬 로딩

씬 로딩은 Addressables를 사용했습니다.

Loading Scene을 유지한 상태에서 Lobby Scene과 Game Scene을 Additive 방식으로 교체하도록 구성했습니다.

```text
Addressables 초기화
-> Loading Scene 로드
-> 기존 Content Scene 언로드
-> 필요 시 ECS World Reset
-> 새 Content Scene Additive 로드
```

Game Scene에서 빠져나올 때는 ECS World를 리셋해 이전 전투의 Entity 데이터가 다음 전투에 남지 않도록 처리했습니다.

### ECS SubScene 로딩

ECS SubScene은 일반 GameObject 씬과 로딩 방식이 달라서 `EntitySceneReference`와 `SceneSystem.LoadSceneAsync`를 사용해 따로 로드했습니다.

```text
SubSceneController
-> World.DefaultGameObjectInjectionWorld 확인
-> EntitySceneReference 참조
-> SceneSystem.LoadSceneAsync
-> SceneLoadFlags.LoadAdditive
```

Editor에서는 정상 동작했지만 Android 빌드에서 SubScene 데이터가 빠지는 문제가 있었습니다. 빌드에 포함되는 씬에서 SubScene 참조를 유지하도록 수정해 APK 실행 환경에서도 ECS 데이터가 로드되도록 해결했습니다.

## 문제 해결 경험

### 전투 시스템 실행 순서 정리

전투 로직은 생성, 이동, 충돌, 데미지 계산, 삭제가 순서대로 이어져야 합니다.

처음부터 이 흐름이 섞이지 않도록 SystemGroup을 나누고 실행 순서를 명시했습니다.

```text
관리 -> 생성 -> 이동 -> 판정 -> 계산 -> 삭제
```

이 구조 덕분에 충돌이 정상적으로 발생했는지, 데미지가 적용되었는지, 삭제가 처리되었는지를 단계별로 확인할 수 있었습니다.

### 대량 충돌 판정 비용 감소

적과 투사체가 많아질수록 모든 조합을 비교하는 방식은 부담이 커집니다.

이를 줄이기 위해 Grid 기반 공간 분할을 적용했습니다. 적을 Cell 단위로 등록하고, 투사체는 주변 Cell만 검사하도록 구성해 충돌 후보를 줄였습니다.

하나의 Cell에 여러 적이 들어갈 수 있기 때문에 `NativeParallelMultiHashMap<int, Entity>`를 사용했습니다.

### 투사체 관통 처리 안정화

투사체가 같은 적을 여러 번 타격하거나, 의도한 관통 수보다 더 많이 관통하면 전투 밸런스가 흔들릴 수 있습니다.

이를 막기 위해 투사체별로 이미 맞은 적을 Buffer에 기록하고, 기록된 적 수가 최대 관통 수에 도달하면 `LiveTag`를 비활성화하도록 처리했습니다.

### Addressables와 ECS SubScene 로딩 분리

Addressables로 Game Scene을 로드해도 ECS SubScene 데이터가 항상 함께 준비되는 것은 아니었습니다.

GameObject 씬은 Addressables가 담당하고, ECS SubScene은 `SubSceneController`에서 명시적으로 로드하도록 나누었습니다.

이 방식으로 Editor와 Android 빌드 환경의 차이를 확인하고, 실제 APK 실행 환경에서도 SubScene 데이터가 로드되도록 수정했습니다.

### Entity 제거 방식 단순화

처음에는 제거 대상 Entity를 `DeadTag`로 관리하는 방식도 고려했습니다.

하지만 대부분의 Entity는 생성 직후 살아 있는 상태이기 때문에, 살아 있는 동안 `LiveTag`를 유지하고 제거 대상이 되면 비활성화하는 방식이 더 직관적이라고 판단했습니다.

현재는 `LiveTag`가 비활성화된 Entity를 Destroy 단계에서 일괄 제거합니다.

## 개선할 수 있는 부분

| 항목 | 개선 방향 |
| --- | --- |
| Grid Map Capacity | 현재 고정 용량을 사용하므로 적 수에 따라 조정되는 구조로 개선 가능 |
| 충돌 결과 기록 | 충돌 결과를 별도 이벤트 데이터로 모아 데미지 계산 단계에서 처리하는 구조로 개선 가능 |
| Manager 구조 | 일부 Singleton과 static 흐름을 더 작게 나누어 테스트하기 쉬운 구조로 개선 가능 |
| 밸런스 데이터 | 무기와 적 데이터를 외부 데이터 기반으로 더 명확하게 관리 가능 |

## 빌드 및 실행

### 개발 환경

```text
Unity: 6000.3.11f1
Target Platform: Android
Render Pipeline: URP
Addressables: 2.9.1
Input System: 1.19.0
```

### 실행 방법

```text
1. Repository Clone
2. Unity Hub에서 Unity 6000.3.11f1로 프로젝트 열기
3. Addressables 설정 확인
4. Android Build Settings 확인
5. Build 또는 Build And Run 실행
```

APK 파일은 Release 페이지에서 받을 수 있습니다.

```text
https://github.com/eggmoney1073/Slime_Defense/releases/tag/v1.0.0
```

## 스크린샷

![게임 플레이 화면 1](ScreenShot/Caputre1.png)

![게임 플레이 화면 2](ScreenShot/Caputre2.png)

## 담당 범위

| 구분 | 내용 |
| --- | --- |
| 기획 | 게임 규칙, 성장 구조, 전투 흐름 설계 |
| 클라이언트 구현 | 플레이어, 적, 투사체, 레벨업, UI 구현 |
| ECS 구조 | Entity Component 설계, SystemGroup 흐름 구성 |
| 충돌 처리 | Grid 기반 충돌 후보 축소 및 관통 처리 |
| 로딩 구조 | Addressables Scene 로딩, ECS SubScene 로딩 |
| 빌드 검증 | Android APK 빌드 및 실기기 실행 확인 |

## 프로젝트를 통해 경험한 내용

- Unity ECS/DOTS 구조를 실제 게임 전투 로직에 적용
- 다수의 적과 투사체를 처리하기 위한 데이터 중심 구조 설계
- Grid 기반 충돌 후보 필터링 적용
- Addressables와 ECS SubScene을 함께 사용할 때의 로딩 흐름 확인
- Editor 환경과 Android 실기기 환경의 차이 확인
- 문제 원인을 좁히고 구조를 수정해 해결하는 과정 경험

## 마무리

Slime Defense는 Unity ECS/DOTS를 실제 플레이 가능한 모바일 게임 구조에 적용해 본 프로젝트입니다.

전투 기능 구현뿐 아니라 시스템 실행 순서, 충돌 판정, Entity 생존 처리, Addressables 씬 로딩, Android 실기기 검증까지 진행하며 구조와 성능을 함께 고려하는 개발 과정을 경험했습니다.
