# Mahjong1

ToDo List 및 메모
1. 역 관련
- 대사희하면 백발중 1판 들어가는거 없애기
- 

2. 멀티 플레이 만들기
- 4명일때 누군가가 start누르면 scene넘기면서 시작하기
- 서버 script 관련
  - update로 레디 확인
  - 플레이어 행동시 모든 플레이어에 동기화하기

3. 치퐁캉등 우는거
- 깡하면 드로우 가능 패 수 -1

4. 승리 확인
- 결국 몸통 4개 머리 1개 만드는 게임
- 몸통 3개, 머리 1개 or 몸통 4개, 머리 0개 or 머리 6개면 승리 가능 확인하기(역 뭐 있나 확인하기)
- 츠모기리 인지 확인해서 맞으면 다시 확인 안해도 될
- 국사는 또 따로 확인해야할듯 위에서 치또이는 확인한듯?
   
6. 치퐁캉등 울었을때 플레이어의 핸드 위치 변경
7. 쵼보 확인

슌츠 관련
1234면 123만 나올텐데 234는 어떻게?? 12345는?
1있으면 2 3 4 5 6 계속 있는지 없는지 확인하고 있으면 다음 없으면 끝 이런식으로 알고리즘 변경해야할듯 그래서 슌츠 가능한 모든 경우의 수를 찾아서 반환 해줘야할듯?


게임 전체적인 알고리즘
입장 -> 시작 -> 도라표시패 만들기 -> 개인 패 배부 -> (후로 가능 패 찾기?)
-> 오야부터 츠모 -> 일정시간 대기(코루틴으로 구현) or 키 입력으로 타패 확인 -> 츠모기리가 아닐시 타패 한 사람의 후로 가능 패 다시 서치 & 이전에 찾은 다른 사람들의 후로 가능 패들을 기준으로 다른 플레이어들의 후로 선택권 부여 -> 다른 플레이어가 타패된 패로 론이나 후로하는 경우가 아닐시 다음 사람 츠모
-> 츠모 & 타패 70개까지 반복 
루트1 유국
유국만관 확인, 텐파이 확인, 텐파이들 점수 계산, 텐파이인 사람이 오야면 연장
루트2 론 or 츠모
점수 계산 -> 마지막 판인지 연장인지등등 확인하고 다음 판 시작하거나 게임 완전 종료
