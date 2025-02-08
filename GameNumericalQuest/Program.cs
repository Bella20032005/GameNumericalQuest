using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameNumericalQuest
{
    internal class Program
    { 

        static Random random = new Random();
        //созданий комнат и присвоение им номеров 
        const int EMPTY_ROOM = 0; 
        const int MONSTER = 1; 
        const int TRAP = 2; 
        const int CHEST = 3;         
        const int MERCHANT = 4; 
        const int BOSS = 5;
        

        class Player // класс для игрока 
        { 
          // имя хп голда и стрелы 
          public string Name { get; set; } 
          public int Health { get; set; } = 100; 
          public int Gold { get; set; } = 0; 
          public int Arrows { get; set; } = 5;


            public void LoseHealth(int count) // метод для потери здоровья
            {
                Health -= count;
                Console.WriteLine($"{Name} теряет {count} здоровья! Текущее здоровье: {Health}");
                Console.ReadLine();
            }
            public void GiveHealth(int count) // метод для потери здоровья
            {
                Health += count;
                Console.WriteLine($"{Name} восполняет {count} здоровья! Текущее здоровье: {Health}");
                Console.ReadLine();
            }
            public void GiveGold(int count) // метод для получения золота 
            {
                Gold += count;
                Console.WriteLine($"{Name} получает {count} золота! Текущее золото: {Gold}");
                Console.ReadLine();
            }
            public void GiveArrows(int count) // метод для получения золота 
            {
                Gold += count;
                Console.WriteLine($"{Name} получает {count} стрел! Стрел: {Arrows}");
                Console.ReadLine();
            }

        }
        class Monster // класс для монстра 
        {
            public int Health { get; set; } 
            public Monster()
            {
                Health = random.Next(20, 50); // Случайное здоровье от 20 до 50
            }

            public int Attack()
            {
                return random.Next(5, 15); // Ответный урон от 5 до 15
            }
        }
        class Dungeon
        {
            public int[] rooms;

            public Dungeon()
            {
                rooms = new int[10];
                for (int i = 0; i < 9; i++)
                {
                    rooms[i] = random.Next(0, 5); // Случайные события от 1 до 4
                }
                rooms[9] = BOSS; // Последняя комната - босс
            }

            public void EnterRoom(Player player, int roomNumber) //выбор комнаты
            {

                int eventType = rooms[roomNumber];
                switch (eventType)
                {
                    case MONSTER:
                        FightMonster(player);
                        break;
                    case TRAP:
                        TriggerTrap(player);
                        break;
                    case CHEST:
                        OpenChest(player);
                        break;
                    case MERCHANT:
                        VisitMerchant(player);
                        break;
                    case BOSS:
                        FightBoss(player);
                        break;
                    default:
                        Console.WriteLine("Комната пуста.");
                        Console.ReadLine();
                        break;
                }
            }

            private void FightMonster(Player player) //борьба с монстром
            {
                Monster monster = new Monster();
                Console.WriteLine($"Вы встретили монстра! с {monster.Health} HP!") ;
                while (monster.Health > 0 && player.Health > 0)
                {
                    Console.WriteLine("\nВыберите оружие:");
                    Console.WriteLine("1. Меч");
                    Console.WriteLine("2. Лук");

                    string choice = Console.ReadLine();
                    int damage = 0;

                    switch (choice)
                    {
                        case "1":
                            damage = random.Next(10, 21); // Урон мечом от 10 до 20
                            Console.WriteLine($"{player.Name} атакует монстра мечом и наносит {damage} урона!");
                            break;
                        case "2":
                            if (player.Arrows > 0)
                            {
                                damage = random.Next(5, 16); // Урон луком от 5 до 15
                                player.Arrows--;
                                Console.WriteLine($"{player.Name} атакует монстра луком и наносит {damage} урона! Осталось стрел: {player.Arrows}");
                            }
                            else
                            {
                                Console.WriteLine("У вас закончились стрелы! Вы не можете использовать лук.");
                                continue; // Пропускаем итерацию цикла, если стрел нет
                            }
                            break;
                        default:
                            Console.WriteLine("Неверный выбор! Попробуйте снова.");
                            continue; 
                    }

                    monster.Health -= damage; //    монстр теряет хп
                    Console.WriteLine($"Монстр теперь имеет {monster.Health} HP.");

                    if (monster.Health > 0) // Если монстр еще жив, он атакует игрока
                    {
                        int monsterDamage = monster.Attack();
                        player.LoseHealth(monsterDamage);
                    }
                }

                if (player.Health > 0) 
                {
                    Console.WriteLine($"Вы победили монстра!");
                    player.GiveGold(30); // Награда за победу
                }
                else
                {
                    Console.WriteLine($"{player.Name} погиб в бою...");
                }
            }

            private void TriggerTrap(Player player) // метод ловушка
            {
                Console.WriteLine("Вы попали в ловушку!");
                player.LoseHealth(15);
            }

            private void OpenChest(Player player)// метод сундук
            {
                Console.WriteLine("Вы нашли сундук! Решите математическую загадку.");

                // Генерируем простую математическую задачу
                int num1 = random.Next(1, 11); // Случайное число от 1 до 10
                int num2 = random.Next(1, 11); // Случайное число от 1 до 10
                int correctAnswer = num1 + num2; // Загадка: сложение
                while (true)
                {
                    Console.WriteLine($"Сколько будет {num1} + {num2}?");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int playerAnswer) && playerAnswer == correctAnswer)
                    {
                        Console.WriteLine("Правильно! Вы открыли сундук.");
                        GiveReward(player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неправильный ответ. Попробуйте еще раз.");
                    }
                }
            }
            private void GiveReward(Player player)
            {
                int rewardType = random.Next(1, 4); // Случайный тип награды (1 - золото, 2 - зелье, 3 - стрелы)

                switch (rewardType)
                {
                    case 1:
                        int goldAmount = random.Next(20, 101); // Случайное количество золота от 20 до 100
                        player.GiveGold(goldAmount);
                        break;
                    case 2:
                        player.GiveGold(10);
                        break;
                    case 3:
                        int arrowsAmount = random.Next(5, 20); // Случайное количество стрел от 5 до 20
                        player.GiveArrows(arrowsAmount);
                        break;
                }
            }


            private void VisitMerchant(Player player) // метод для зелья при покупке зелья игроку накидывается 20 хп 
            {
                Console.WriteLine("Вы встретили торговца! У вас есть золото: " + player.Gold);
                Console.Write("Хотите купить зелье за 20 золота? (да/нет): ");
                string choice = Console.ReadLine();
                if (choice.ToLower() == "да" && player.Gold >= 20)
                {
                    player.GiveGold(-20);
                    Console.WriteLine("Вы купили зелье!");
                    player.GiveHealth(20);
                }
                else
                {
                    Console.WriteLine("Недостаточно золота или отмена покупки.");
                }
            }

            private void FightBoss(Player player)
            {
                Console.WriteLine("Вы встретили БОССА!");
                player.LoseHealth(50);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в подземелье!");
            Console.Write("Введите имя вашего персонажа: ");
            string playerName = Console.ReadLine();
            Player player = new Player { Name = playerName };
            Dungeon dungeon = new Dungeon();

            for (int roomNumber = 0; roomNumber < dungeon.rooms.Length; roomNumber++)
            {
                Console.WriteLine($"\nВы входите в комнату {roomNumber + 1}.");
                dungeon.EnterRoom(player, roomNumber);

                if (player.Health <= 0)
                {
                    Console.WriteLine($"{player.Name} погиб в подземелье...");
                    break;
                }


            }
        }
    }
}