using GamePlay;
using GameLogic;
public static class GameStart
{
    private static void Main()
    {
        var player = new Warrior();
        var game = new Game();
        var input = 0;

        while (true)
        {
            input = game.MainInput();

            switch (input)
            {
                case 1:
                    game.InfoInput(player);
                    break;

                case 2:
                    game.Inventory(player);
                    break;

                case 3:
                    game.Shop(player);
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }
}

namespace GameLogic
{
    public class Game
    {
        private Item[] gameItem =
        {
            //방어구
            new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, "방어력", 5),
            new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 1500, "방어력", 9),
            new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, "방어력", 15),

            //무기
            new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 600, "공격력", 2),
            new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1500, "공격력", 5),
            new Item("스파르타의 창", "쉽게 볼 수 있는 낡은 검 입니다.", 3500, "공격력", 7),
        };

        public bool CheckInput(out int _value)
        {
            string input = Console.ReadLine();

            if (int.TryParse(input, out _value)) return true;

            Console.WriteLine("숫자를 입력하세요.");
            Thread.Sleep(1000);
            return false;
        }

        public int MainInput()
        {
            int input = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (CheckInput(out input))
                {
                    //메뉴얼 선택시
                    if (input > 0 || input < 4)
                    {
                        break;
                    }

                    //메뉴얼 이탈시
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }

            return input;
        }

        public void InfoInput(Job _player)
        {
            int input = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                Console.WriteLine($"Lv. {_player.level:00}");
                Console.WriteLine($"Chad ({_player.chad})");
                Console.WriteLine($"공격력 : {_player.atk}");
                Console.WriteLine($"방어력 : {_player.def}");
                Console.WriteLine($"체력 : {_player.health}");
                Console.WriteLine($"Gold : {_player.gold} G");

                Console.WriteLine("\n0. 나가기");

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public void Inventory(Job _player)
        {
            int input = 0;
            int playerItem = _player.item.Count;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

                Console.WriteLine("\n[아이템 목록]\n");

                //소지 중인 아이템 만큼만 출력
                for (int i = 0; i < _player.item.Count; i++)
                {
                    var item = _player.item[i];
                    Console.WriteLine($"{(item.equipped ? "[E] " : "")}{item.name} | {item.Ability()} | {item.itemInfo}");
                }

                if (playerItem > 0) Console.WriteLine("\n1. 아이템 사용");
                else Console.Write("\n");

                Console.WriteLine("0. 나가기");

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }

                    //플레이어의 아이템이 존재할 경우에만 선택 가능
                    else if (input == 1 && playerItem > 0)
                    {
                        SelectItem(_player, input);
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public void SelectItem(Job _player, int _itemIndex)
        {
            int input = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

                Console.WriteLine("\n[아이템 선택]\n");

                //소지 중인 아이템 만큼만 출력
                for (int i = 0; i < _player.item.Count; i++)
                {
                    var item = _player.item[i];
                    Console.WriteLine($"{i + 1}번 아이템. {(item.equipped ? "[E]" : "")}{item.name} | {item.Ability()} | {item.itemInfo}\n");
                }

                Console.WriteLine("\n원하시는 아이템을 입력해주세요.");
                Console.Write(">>");

                if (CheckInput(out input))
                {
                    input--;

                    if (input > -1 && input < _player.item.Count)
                    {
                        //스택 오버플로우 방지
                        EquippedItem(_player, input);
                        break;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public void EquippedItem(Job _player, int _itemIndex)
        {
            int input = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

                Console.WriteLine("\n[선택된 아이템]\n");
                var item = _player.item[_itemIndex];

                if (item.equipped)
                {
                    Console.WriteLine($"[E] {item.name} | {item.Ability()} | {item.itemInfo}");
                    Console.WriteLine("\n1. 장착 해제");
                }

                else
                {
                    Console.WriteLine($"{item.name} | {item.Ability()} | {item.itemInfo}");
                    Console.WriteLine("\n1. 장착");
                }

                Console.WriteLine("0. 취소");

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }

                    else if (input == 1)
                    {
                        item.EquippedItem(!item.equipped);
                        break;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public void Shop(Job _player)
        {
            int input = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{_player.gold} G");

                Console.WriteLine("\n[아이템 목록]\n");

                for (int i = 0; i < gameItem.Length; i++)
                {
                    var item = gameItem[i];
                    var isBuy = _player.item.Contains(item) ? "구매 완료\n" : $"{item.gold} G\n";
                    Console.WriteLine($"- {item.name} | {item.Ability()} | {item.itemInfo} | {isBuy}");
                }

                Console.WriteLine("\n1. 아이템 선택");
                Console.WriteLine("0. 나가기");

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                if (CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }

                    else if (input == 1)
                    {
                        BuyItem(_player);
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public void BuyItem(Job _player)
        {
            int input = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{_player.gold} G");

                Console.WriteLine("\n[아이템 목록]\n");


                for (int i = 0; i < gameItem.Length; i++)
                {
                    var item = gameItem[i];
                    var isBuy = _player.item.Contains(item) ? "구매 완료\n" : $"{item.gold} G\n";
                    Console.WriteLine($"- {i + 1}번 {item.name} | {item.Ability()} | {item.itemInfo} | {isBuy}");
                }

                Console.WriteLine("0. 취소");

                Console.WriteLine("\n원하시는 아이템을 입력해주세요.");
                Console.Write(">>");

                if (CheckInput(out input))
                {
                    if (input == 0)
                    {
                        break;
                    }

                    else if (input <= gameItem.Length)
                    {
                        input--;

                        //이미 소지 중인가?
                        if (_player.item.Contains(gameItem[input]))
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                        }

                        //골드가 충분한가?
                        else if (_player.gold >= gameItem[input].gold)
                        {
                            _player.gold -= gameItem[input].gold;
                            _player.item.Add(gameItem[input]);

                            Console.WriteLine("구매를 완료했습니다.");
                        }

                        else
                        {
                            Console.WriteLine("Gold 가 부족합니다.");
                        }

                        Thread.Sleep(1000);
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}

namespace GamePlay
{
    public class Item
    {
        public string name { get; private init; }
        public string itemInfo { get; private init; }
        public int atk { get; private init; }
        public int def { get; private init; }
        public int health { get; private init; }
        public int gold { get; private init; }
        public bool equipped { get; private set; }

        public Item(string _name, string _itemInfo, int _gold = 0, string _ability = "", int _value = 0)
        {
            name = _name;
            itemInfo = _itemInfo;
            gold = _gold;

            atk = 0;
            def = 0;
            health = 0;

            if (_ability != "")
            {
                switch (_ability)
                {
                    case "공격력":
                        atk = _value;
                        break;

                    case "방어력":
                        def = _value;
                        break;

                    case "체력":
                        health = _value;
                        break;

                    default:
                        Console.Write($"({_ability})라는 Ability는 존재하지 않음");
                        break;
                }
            }
        }

        public string Ability()
        {
            if (atk != 0) return $"공격력 {(atk < 0 ? "-" : "+")} {atk}";
            else if (def != 0) return $"방어력 {(def < 0 ? "-" : "+")} {def}";
            else if (health != 0) return $"체력 {(health < 0 ? "-" : "+")} {health}";

            return "";
        }

        public void EquippedItem(bool _equipped) => equipped = _equipped;
    }

    public abstract class Job
    {
        private int fieldLevel;
        public int level
        {
            get => fieldLevel;
            set => fieldLevel = value > 99 ? 99 : value;
        }

        public string name { get; protected set; } = "";
        public string chad { get; protected set; } = "";

        public List<Item> item = new List<Item>();

        public int atk;
        public int def;
        public int health;
        public int gold;

        public void SetName(string _name) => name = _name;
    }

    public class Warrior : Job
    {
        public Warrior()
        {
            level = 1;
            chad = "전사";
            atk = 10;
            def = 5;
            health = 100;
            gold = 1500;
        }
    }
}