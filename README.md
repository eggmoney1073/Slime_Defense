# 슬라임 디펜스 (Slime Defense)

> Unity ECS(DOTS) 기반 모바일 성장형 디펜스 게임  
> 1인 개발 | Android 빌드 완료 | 2026.05.07 완성

<br/>

## 1. 프로젝트 소개

**슬라임 디펜스**는 중앙에 고정된 플레이어가 몰려오는 슬라임을 막으며 성장하는 모바일 디펜스 게임입니다.  
전투 중 경험치를 획득하고, 레벨업 시 3개의 보상 중 하나를 선택해 무기와 능력을 강화하는 구조입니다.

이 프로젝트는 단순한 기능 구현보다, **Unity ECS/DOTS를 실제 게임 구조에 적용하고 Android 실기기 환경에서 검증하는 것**을 목표로 제작했습니다.

<br/>

## 2. 링크

| 항목 | 링크 |
|------|------|
| 플레이 영상 | https://www.youtube.com/watch?v=FuFSvaY_o_8&t=170s |
| 플레이 파일 다운로드 | https://github.com/eggmoney1073/Slime_Defense/releases/tag/v1.0.0 |

<br/>

## 3. 게임 정보

| 항목 | 내용 |
|------|------|
| 플랫폼 | Android |
| 장르 | 디펜스 + 서바이벌 성장형 |
| 개발 인원 | 1인 개발 |
| 개발 기간 | 2026.03.22 ~ 2026.05.07 |
| 플레이 방식 | 중앙 고정형 자동 전투 |
| 클리어 조건 | 제한 시간 동안 생존 |
| 실패 조건 | 플레이어 체력 소진 |

<br/>

## 4. 게임 루프

```text
전투 시작
→ 적 웨이브 스폰
→ 자동 전투 진행
→ 경험치 획득
→ 레벨업 보상 선택
→ 무기 및 능력 강화
→ 제한 시간 생존 시 클리어 / 체력 소진 시 실패
```

<br/>

## 5. 사용 기술

| 기술 | 사용 목적 |
|------|----------|
| Unity 6000.3.11f1 | 게임 엔진 |
| C# | 게임 로직 구현 |
| Unity ECS / DOTS | 적, 투사체, 충돌, 생존 상태 처리 |
| Unity Entities | Entity / Component / System 구조 구현 |
| Burst Compiler | ECS Job 최적화 |
| Job System | 대량 Entity 병렬 처리 |
| Addressables | 씬 및 에셋 비동기 로딩 |
| URP | 2D 스프라이트 렌더링 |
| ADB (Android Debug Bridge) | APK 설치, 실기기 실행 확인, Android 빌드 검증 |

<br/>

## 6. 프로젝트 구조

```text
Assets/
├── 1.Scenes/                  # Bootstrap, Loading, Game, SubScene
├── 2.Scripts/
│   ├── ECS/
│   │   ├── Authorings/        # MonoBehaviour → Entity 변환용 Authoring
│   │   ├── Bakers/            # Baker 구현
│   │   ├── Bridge/            # ECS ↔ MonoBehaviour 연결
│   │   ├── Entity Structs/    # IComponentData / IBufferElementData 정의
│   │   └── Entity Systems/    # ECS System 구현
│   │       ├── 1.Spawn/       # 적, 무기, 투사체 생성
│   │       ├── 2.Move/        # 적/투사체 이동
│   │       ├── 3.Judgement/   # Grid 기반 충돌 판정
│   │       ├── 4.Calculate/   # 데미지, 관통, 생존 계산
│   │       └── 5.Dead/        # Entity 제거 처리
│   ├── Loading/               # Addressables 씬 로딩, SubScene 로딩
│   ├── DownLoad/              # Addressables 다운로드 관리
│   ├── Input/                 # 입력 처리
│   ├── UI/                    # 게임 UI
│   └── Utility/               # Singleton, ObjectPool 등 공통 유틸
├── 3.InputSystem/             # Input Action Asset
├── 4.Prefabs/                 # 게임 프리팹
└── AddressableAssetsData/     # Addressables 설정
```

<br/>

## 7. ECS 시스템 흐름

```text
SlimeDefenseSystemGroup
│
├── SpawnSystemGroup
│   └── 적 스폰, 무기 발사, 투사체 생성
│
├── MoveSystemGroup
│   └── 적 이동, 투사체 이동
│
├── JudgementSystemGroup
│   └── Grid 기반 충돌 판정
│
├── CalculateSystemGroup
│   └── 데미지 계산, 투사체 관통 체크, 생존 상태 처리
│
└── DestroySystemGroup
    └── 비활성 Entity 제거
```

<br/>

## 8. 주요 구현 내용

### 8-1. ECS 기반 적/투사체 처리

- 적과 투사체를 Entity로 관리했습니다.
- `EnemyTag`, `ProjectileTag`, `LiveTag` 등 태그 컴포넌트로 처리 대상을 구분했습니다.
- 수명, 관통 수, 데미지, 이동 방향 등을 Component로 분리했습니다.
- 다수의 적과 투사체 이동을 ECS System과 Job으로 처리했습니다.

<br/>

### 8-2. Grid 기반 충돌 처리

- 매 프레임 적 위치를 Grid Cell에 등록했습니다.
- `NativeParallelMultiHashMap<int, Entity>`를 사용해 Cell별 적 Entity를 관리했습니다.
- 투사체는 전체 적을 검사하지 않고 주변 Cell만 검사하도록 구성했습니다.
- 충돌 시 즉시 체력을 변경하지 않고, `Damaged` Buffer에 데미지를 누적한 뒤 별도 계산 시스템에서 처리했습니다.

<br/>

### 8-3. Addressables 기반 씬 로딩

- 게임 씬을 Addressables로 비동기 로드했습니다.
- Loading Scene을 유지한 상태에서 Game Scene을 교체하는 구조를 사용했습니다.
- ECS SubScene은 `EntitySceneReference`와 `SceneSystem.LoadSceneAsync`를 사용해 별도로 로드했습니다.
- Android 빌드 환경에서 Addressables Scene과 ECS Entity Scene 포함 관계를 검증했습니다.

<br/>

### 8-4. 레벨업 선택 시스템

- 전투 중 경험치를 획득하면 레벨업 UI를 표시했습니다.
- 레벨업 시 3개의 보상 후보 중 하나를 선택하도록 구성했습니다.
- 선택 결과에 따라 무기 또는 능력치가 강화되도록 처리했습니다.
- 전투 로직과 UI 로직이 직접 강하게 묶이지 않도록 역할을 분리했습니다.

<br/>

## 9. 문제 해결

### 9-1. SubScene / Entity Scene이 APK에 포함되지 않는 문제

**문제 상황**  
Editor에서는 ECS SubScene이 정상적으로 동작했지만, Android APK 빌드 후 실행하면 ECS 데이터가 로드되지 않았습니다.

**원인 분석**  
메인 게임 씬을 Addressables로 로드하고 있었기 때문에, 씬 안에서 참조하는 ECS SubScene의 Entity Scene 데이터가 APK 빌드에 포함되지 않았습니다.  
Addressables 씬 로딩과 ECS Entity Scene 빌드 포함 처리가 서로 별도로 동작하는 구조였기 때문에 발생한 문제였습니다.

**해결 방법**  
빌드에 항상 포함되는 Bootstrap Scene에서 SubScene을 직접 참조하도록 구조를 변경했습니다.  
그 결과 Android 빌드 시 Entity Scene 데이터가 함께 포함되었고, 실제 APK 실행 환경에서도 ECS 데이터가 정상적으로 로드되었습니다.

**결과**  
Addressables 기반 씬 로딩 구조를 유지하면서도, ECS SubScene 데이터가 Android 빌드에 안정적으로 포함되도록 해결했습니다.

<br/>

### 9-2. 대량 적/투사체 충돌 누락 문제

**문제 상황**  
적과 투사체가 많아질수록 일부 충돌이 누락되거나, 투사체가 설정된 관통 수보다 많은 적을 통과하는 문제가 발생했습니다.

**원인 분석**  
처음에는 Grid Buffer 용량 부족을 의심했지만, 실제 적 수보다 Buffer 용량이 충분했기 때문에 원인이 아니었습니다.  
이후 충돌 거리 계산을 확인한 결과, 렌더링 정렬에 사용하는 좌표 기준과 충돌 판정에 사용하는 좌표 기준이 섞여 있었습니다.

**해결 방법**  
렌더링 정렬용 좌표와 충돌 판정용 좌표를 분리했습니다.  
또한 모든 적을 직접 검사하는 방식 대신 Grid 기반 공간 분할 구조를 적용해, 투사체 주변 Cell에 있는 적만 충돌 후보로 검사하도록 변경했습니다.

**결과**  
충돌 판정 안정성을 개선했고, 대량의 적과 투사체가 존재하는 상황에서도 불필요한 전체 탐색을 줄였습니다.

<br/>

### 9-3. ECS와 Addressables를 함께 사용할 때 SubScene 로드 타이밍이 어긋나는 문제

**문제 상황**  
Addressables로 Game Scene을 로드하더라도 ECS SubScene 데이터는 일반 GameObject 씬처럼 자동으로 준비되지 않았습니다.

**원인 분석**  
Addressables Scene 로딩과 ECS Entity Scene 로딩은 서로 다른 흐름으로 동작합니다.  
따라서 Game Scene 로드 완료만으로 ECS Entity 데이터가 준비되었다고 판단하면 안 되는 구조였습니다.

**해결 방법**  
SubScene 로드를 별도로 담당하는 로더를 두고, `EntitySceneReference`를 통해 ECS World에 Entity Scene을 명시적으로 로드하도록 구성했습니다.

**결과**  
Addressables 씬 로딩과 ECS SubScene 로딩의 책임을 분리해, 씬 전환 구조를 더 명확하게 관리할 수 있게 되었습니다.

<br/>

### 9-4. Burst Compile 환경에서 static 접근이 제한되는 문제

**문제 상황**  
ECS Job을 Burst Compile 대상으로 작성하는 과정에서 static 데이터 또는 managed 객체 접근이 필요한 코드와 충돌이 발생했습니다.

**원인 분석**  
Burst Compile은 고성능 네이티브 코드 생성을 목표로 하기 때문에, managed 객체 접근이나 일부 static 접근 패턴에 제한이 있습니다.

**해결 방법**  
Burst Compile이 필요한 순수 연산 로직과, managed/static 접근이 필요한 로직을 분리했습니다.  
Burst가 필요한 Job에는 순수 데이터만 전달하고, static 접근이 필요한 코드는 Burst Compile 대상에서 제외했습니다.

**결과**  
Burst Compile의 장점을 유지하면서도 Unity 관리 객체가 필요한 로직을 안정적으로 처리할 수 있게 되었습니다.

<br/>

### 9-5. Entity 렌더링 정렬이 무작위처럼 보이는 문제

**문제 상황**  
다수의 Entity가 화면에 표시될 때, 렌더링 정렬이 의도와 다르게 보이는 문제가 있었습니다.

**원인 분석**  
Entity의 위치와 카메라 기준 정렬 값이 명확히 분리되어 있지 않아, 화면상의 앞뒤 관계가 일관되지 않게 보였습니다.

**해결 방법**  
Entity의 위치 값을 카메라 기준 정렬 방식에 맞게 계산하고, 렌더링에 사용하는 기준을 별도로 정리했습니다.

**결과**  
다수의 Entity가 동시에 등장해도 화면 표시 순서를 더 안정적으로 제어할 수 있게 되었습니다.

<br/>

### 9-6. DeadTag 방식의 Entity 제거 구조가 직관적이지 않은 문제

**문제 상황**  
삭제 예정 Entity를 `DeadTag` 활성화로 처리하려 했지만, 스폰 직후 Tag 상태를 관리하는 과정이 복잡해졌습니다.

**원인 분석**  
생성된 Entity는 기본적으로 활성 상태로 다뤄지기 때문에, 죽은 상태를 나타내는 `DeadTag`를 비활성으로 시작해 필요할 때 활성화하는 방식은 흐름이 직관적이지 않았습니다.

**해결 방법**  
`DeadTag` 대신 `LiveTag` 방식으로 전환했습니다.  
Entity가 살아 있는 동안 `LiveTag`를 활성 상태로 두고, 제거 대상이 되면 `LiveTag`를 비활성화한 뒤 Destroy System에서 일괄 제거하도록 구성했습니다.

**결과**  
Entity 생존 상태를 더 직관적으로 표현할 수 있었고, 제거 대상 처리도 시스템 흐름에 맞게 단순화했습니다.

<br/>

### 9-7. Enemy Damage Buffer 병렬 처리 문제

**문제 상황**  
여러 투사체가 동시에 같은 Enemy의 Damage Buffer에 접근하면서 병렬 처리 충돌 가능성이 있었습니다.

**원인 분석**  
충돌 판정 시스템에서 적 체력을 바로 수정하거나 여러 투사체가 직접 Damage Buffer에 접근하면, 병렬 처리 과정에서 데이터 접근 순서가 복잡해질 수 있었습니다.

**해결 방법**  
충돌 판정과 데미지 계산 단계를 분리했습니다.  
충돌 단계에서는 데미지를 `Damaged` Buffer에 누적하고, 이후 Damage System에서 누적 데미지를 순회하며 체력에 반영한 뒤 Buffer를 비우도록 구성했습니다.

**결과**  
충돌 판정, 데미지 계산, 사망 처리가 단계별로 분리되어 ECS 시스템 흐름이 명확해졌습니다.

<br/>

## 10. 빌드 환경

```text
Unity              : 6000.3.11f1
Target Platform    : Android
Scripting Backend  : IL2CPP
Architecture       : ARM64
Render Pipeline    : URP
ECS Package        : com.unity.entities
Addressables       : com.unity.addressables
Input System       : com.unity.inputsystem
```

<br/>

## 11. 스크린샷

> 인게임 스크린샷

![스크린샷1](ScreenShot/Caputre1.png)
![스크린샷2](ScreenShot/Caputre2.png)

<br/>

## 12. 개발 정보

| 항목 | 내용 |
|------|------|
| 개발 형태 | 1인 개발 |
| 담당 범위 | 기획, 프로그래밍, ECS 구조 설계, Addressables 로딩, Android 빌드, ADB 테스트 |
| 그래픽 | AI 이미지 생성 및 무료 리소스 활용 |
| 사운드 | 무료 에셋 활용 |

<br/>

## 13. 프로젝트를 통해 검증한 내용

- ECS 기반 대량 Entity 처리 구조
- Grid 기반 충돌 최적화 구조
- Addressables와 ECS SubScene을 함께 사용하는 씬 로딩 구조
- Android APK 빌드 및 ADB 테스트
- MonoBehaviour UI와 ECS 로직의 역할 분리
- 문제 원인 분석 후 구조를 변경해 해결하는 개발 과정

<br/>

---

> 본 프로젝트는 Unity ECS/DOTS를 실제 모바일 게임 구조에 적용하고, Addressables 기반 씬 로딩과 대량 Entity 처리 구조를 검증하기 위해 제작되었습니다.
