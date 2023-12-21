## 💡 [프로젝트 소개]
너무 재미잇는 깨임입니다 <br/>
참 재미잇어서 꺄르륵 입니다

## ⏰ [개발 기간]
2023.12.15 - 2023.12.21

## 👥 [멤버 구성]
+ <b>김세진</b> - 역할 조율, 풀링, 오브젝트 관리, 데이터 및 스테이터스 연결, 던전 구조, 코드 결합 및 리팩토링 <br/>
+ <b>우진영</b> - 몬스터 패턴, 몬스터 이동 및 공격, Nav 통한 길찾기 적용 <br/>
+ <b>장성규</b> - 던전 구조 생성 알고리즘 <br/>
+ <b>정채운</b> - 데이터 관리, 리워드 UI, 팀 과제 발표 <br/>
+ <b>최성재</b> - 무기 상세 로직 구현 <br/>

## 👥 [팀 노션]
https://www.notion.so/ec07636ab369459cb373ac3ee8ae2952

## ⚙ [주요 기능]
### 0. 데이터, 리소스 분리
데이터와 리소스를 분리하여, 오브젝트 생성 시 설정된 값과 리소스를 할당합니다.

### 1. 체계적인 씬 로드
Scene이 로드되면, 게임을 시작하기 전에 해당 씬에 필요한 리소스와 데이터들을 로드합니다. <br/>
리소스와 데이터가 로드되면, 던전을 생성하고 패스파인딩을 위한 Navmesh를 빌드합니다. <br/>
던전의 생성이 끝나면, 플레이어와 몬스터, 아이템 등을 스폰하고 게임을 시작합니다. <br/>

### 2. 던전 구조 생성 알고리즘
<img src = https://github.com/Lovescream/TeamNotionOrigin/assets/148977728/552eab38-f514-466b-a823-3f012796359d width="20%" height="20%">
<img src = https://github.com/Lovescream/TeamNotionOrigin/assets/148977728/6e1cc0ae-84a9-4270-ade1-86889a0dda96 width="20%" height="20%"><br/>
매번 다른 구조의 던전을 생성합니다. <br/>
Navmesh를 빌드하기 위해 벽, 장애물과 같은 갈 수 없는 곳과 방, 복도와 같은 갈 수 있는 곳을 분리하여 생성합니다.

### 3. 런타임 Navmesh 빌드
<img src = https://github.com/Lovescream/TeamNotionOrigin/assets/148977728/bc0d0224-bb77-4f5b-97e9-853238529f4f width="40%" height="40%"><br/>
던전의 구조가 매번 바뀌기 떄문에, 패스파인딩을 위한 Navmesh도 던전이 생성된 이후에 진행합니다.

### 4. 오브젝트 풀링
대부분의 오브젝트는 풀링으로 관리하고 있습니다.<br/>
![image](https://github.com/Lovescream/TeamNotionOrigin/assets/148977728/aa474de8-a658-40b7-8abc-cc67d718e18e)

### 5. 플레이어 캐릭터
![image](https://github.com/Lovescream/TeamNotionOrigin/assets/148977728/ecbc31c0-b898-4655-bb58-ec8df21ef2fd)<br/>
WASD로 이동하고, 마우스 버튼 클릭으로 무기를 발사합니다. <br/>
최대 3개의 무기 슬롯이 존재하고, 마우스 휠 스크롤로 무기 슬롯을 교체할 수 있습니다. <br/>
발사 시, 현재 무기의 장탄이 줄어듭니다. 모든 장탄을 소모하면 재장전을 합니다. <br/>
재장전 시, 잔탄을 소모하여 총알을 충전합니다. <br/>

### 6. 몬스터
[Navmesh plus](https://github.com/h8man/NavMeshPlus)를 이용한 패스파인딩 <br/>
유한 상태 머신을 이용한 패턴 구현



