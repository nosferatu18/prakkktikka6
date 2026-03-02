using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal class Program

    {
            static Random rnd = new Random();

            static int hp = 100;
            static int maxHp = 100;
            static int gold = 10;
            static int potions = 2;
            static int arrows = 5;
            static int swordMin = 10;
            static int swordMax = 20;

            // 1. Инициализация игры
            static void InitializeGame()
            {
                hp = 100;
                maxHp = 100;
                gold = 10;
                potions = 2;
                arrows = 5;
                swordMin = 10;
                swordMax = 20;
                Console.WriteLine("Числовой квест ULTIMATE");
                Console.WriteLine("Вы входите в подземелье...\n");
            }

            // 2. Запуск игры
            static void StartGame()
            {
                InitializeGame();

                for (int room = 1; room <= 15; room++)
                {
                    if (hp <= 0)
                    {
                        EndGame(false);
                        return;
                    }

                    if (room == 15)
                    {
                        Console.WriteLine("-- Комната 15: очень страшный орк --");
                        FightBoss();
                        return;
                    }
                    else
                    {
                        ProcessRoom(room);
                    }
                }

                if (hp > 0)
                    EndGame(true);
            }

            // 3. Обработка комнаты
            static void ProcessRoom(int roomNumber)
            {
                Console.WriteLine($"-- Комната {roomNumber} --");
                ShowStats();

                int eventType = rnd.Next(1, 9);

                switch (eventType)
                {
                    case 1:
                    case 2:
                        int mHp = rnd.Next(20, 51);
                        int mAtk = rnd.Next(5, 16);
                        Console.WriteLine($"Вы встретили злодея! (HP: {mHp}, Атака: {mAtk})");
                        FightMonster(mHp, mAtk);
                        break;
                    case 3:
                        Console.WriteLine("Вы наступили на ловушку!");
                        int damage = rnd.Next(5, 21);
                        hp -= damage;
                        Console.WriteLine($"Вы потеряли {damage} HP.");
                        break;
                    case 4:
                        Console.WriteLine("Вы нашли сокровище!");
                        OpenChest();
                        break;
                    case 5:
                        Console.WriteLine("Вы встретили таинственного чудика.");
                        VisitMerchant();
                        break;
                    case 6:
                        Console.WriteLine("Вы нашли древний алтарь.");
                        VisitAltar();
                        break;
                    case 7:
                        Console.WriteLine("Из тени появляется тёмный...загадочный...маг...");
                        MeetDarkMage();
                        break;
                    case 8:
                        Console.WriteLine("Вы нашли головоломку на стене...подумайте!");
                        SolveRiddle();
                        break;
                }

                if (hp <= 0)
                {
                    EndGame(false);
                    Environment.Exit(0);
                }
            }

            // 4. Бой с монстром
            static void FightMonster(int monsterHP, int monsterAttack)
            {
                while (monsterHP > 0 && hp > 0)
                {
                    Console.WriteLine($"Монстр HP: {monsterHP} | Ваше HP: {hp}");
                    Console.WriteLine("1 - Атака мечом");
                    Console.WriteLine("2 - Выстрел из лука (стрел: " + arrows + ")");
                    Console.WriteLine("3 - Выпить зелье (зелий: " + potions + ")");
                    Console.Write("Ваш выбор: ");

                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        int dmg = rnd.Next(swordMin, swordMax + 1);
                        monsterHP -= dmg;
                        Console.WriteLine($"Вы ударили мечом на {dmg} урона.");
                    }
                    else if (input == "2")
                    {
                        if (arrows > 0)
                        {
                            int dmg = rnd.Next(5, 16);
                            monsterHP -= dmg;
                            arrows--;
                            Console.WriteLine($"Вы выстрелили из лука на {dmg} урона.");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет стрел! Вы пропускаете ход.");
                        }
                    }
                    else if (input == "3")
                    {
                        UsePotion();
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор, вы пропускаете ход.");
                    }

                    if (monsterHP > 0)
                    {
                        int mDmg = rnd.Next(1, monsterAttack + 1);
                        hp -= mDmg;
                        Console.WriteLine($"Монстр атакует вас на {mDmg} урона, берегись");
                    }
                }

                if (monsterHP <= 0)
                {
                    int reward = rnd.Next(5, 16);
                    gold += reward;
                    Console.WriteLine($"Монстр повержен, торжествуйте! Вы получили {reward} золота.");
                }
            }

            // 5. Открытие сундука
            static void OpenChest()
            {
                bool cursed = rnd.Next(0, 3) == 0;

                if (cursed)
                {
                    Console.WriteLine("Сундук оказался проклятым, возможно в следующий раз тебе повезет больше!");
                    int g = rnd.Next(5, 16);
                    gold += g;
                    maxHp -= 10;
                    if (hp > maxHp) hp = maxHp;
                    Console.WriteLine($"Вы получили {g} золота, но макс. HP уменьшилось на 10.");
                }
                else
                {
                    int loot = rnd.Next(1, 4);
                    if (loot == 1)
                    {
                        int g = rnd.Next(5, 21);
                        gold += g;
                        Console.WriteLine($"В сундуке {g} золота!");
                    }
                    else if (loot == 2)
                    {
                        potions++;
                        Console.WriteLine("В сундуке зелье здоровья! :)");
                    }
                    else
                    {
                        int a = rnd.Next(2, 6);
                        arrows += a;
                        Console.WriteLine($"В сундуке {a} стрелы!");
                    }
                }
            }

            // 6. Торговец
            static void VisitMerchant()
            {
                Console.WriteLine($"Ваше золото: {gold}");
                Console.WriteLine("1 - Купить зелье (10 золота)");
                Console.WriteLine("2 - Купить 3 стрелы (5 золота)");
                Console.WriteLine("3 - Уйти");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    if (gold >= 10)
                    {
                        gold -= 10;
                        potions++;
                        Console.WriteLine("Вы купили зелье.");
                    }
                    else
                        Console.WriteLine("Не хватает золота.");
                }
                else if (input == "2")
                {
                    if (gold >= 5)
                    {
                        gold -= 5;
                        arrows += 3;
                        Console.WriteLine("Вы купили 3 стрелы.");
                    }
                    else
                        Console.WriteLine("Не хватает золота.");
                }
                else
                {
                    Console.WriteLine("Вы прошли мимо торговца.");
                }
            }

            // 7. Алтарь усиления
            static void VisitAltar()
            {
                if (gold < 10)
                {
                    Console.WriteLine("У вас нет 10 золота для пожертвования. Ничего не происходит.");
                    return;
                }

                Console.WriteLine("Пожертвовать 10 золота?");
                Console.WriteLine("1 - Увеличить урон меча на 5");
                Console.WriteLine("2 - Восстановить 20 HP");
                Console.WriteLine("3 - Отказаться");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    gold -= 10;
                    swordMin += 5;
                    swordMax += 5;
                    Console.WriteLine("Ваш меч стал сильнее! Урон увеличен на 5.");
                }
                else if (input == "2")
                {
                    gold -= 10;
                    hp += 20;
                    if (hp > maxHp) hp = maxHp;
                    Console.WriteLine("Алтарь восстановил вам 20 HP.");
                }
                else
                {
                    Console.WriteLine("Вы отошли от алтаря.");
                }
            }

            // 8. Тёмный маг
            static void MeetDarkMage()
            {
                if (hp <= 10)
                {
                    Console.WriteLine("Маг смотрит на вас и исчезает... Вы слишком слабы.");
                    return;
                }

                Console.WriteLine("Маг предлагает: отдай 10 HP и получи 2 зелья и 5 стрел.");
                Console.WriteLine("1 - Согласиться");
                Console.WriteLine("2 - Отказаться");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    hp -= 10;
                    potions += 2;
                    arrows += 5;
                    Console.WriteLine("Сделка заключена! Вы получили 2 зелья и 5 стрел.");
                }
                else
                {
                    Console.WriteLine("Маг растворяется в тенях.");
                }
            }

            // 9. Использование зелья
            static void UsePotion()
            {
                if (potions > 0)
                {
                    potions--;
                    hp += 30;
                    if (hp > maxHp) hp = maxHp;
                    Console.WriteLine("Вы выпили зелье. +30 HP.");
                }
                else
                {
                    Console.WriteLine("У вас нет зелий!");
                }
            }

            // 10. Показать характеристики
            static void ShowStats()
            {
                Console.WriteLine($"HP: {hp}/{maxHp} | Золото: {gold} | Зелья: {potions} | Стрелы: {arrows} | Меч: {swordMin}-{swordMax}");
            }

            // 11. Битва с боссом
            static void FightBoss()
            {
                int bossHp = 100;
                int turn = 0;

                Console.WriteLine("Перед вами огромный Тёмный Страж! (HP: 100, Атака: 15-25)");

                while (bossHp > 0 && hp > 0)
                {
                    turn++;
                    Console.WriteLine($"\nХод {turn} | Босс HP: {bossHp} | Ваше HP: {hp}");
                    Console.WriteLine("1 - Атака мечом");
                    Console.WriteLine("2 - Выстрел из лука (стрел: " + arrows + ")");
                    Console.WriteLine("3 - Выпить зелье (зелий: " + potions + ")");
                    Console.Write("Ваш выбор: ");

                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        int dmg = rnd.Next(swordMin, swordMax + 1);
                        bossHp -= dmg;
                        Console.WriteLine($"Вы ударили мечом на {dmg} урона.");
                    }
                    else if (input == "2")
                    {
                        if (arrows > 0)
                        {
                            int dmg = rnd.Next(5, 16);
                            bossHp -= dmg;
                            arrows--;
                            Console.WriteLine($"Вы выстрелили из лука на {dmg} урона.");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет стрел! Вы пропускаете ход.");
                        }
                    }
                    else if (input == "3")
                    {
                        UsePotion();
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор, вы пропускаете ход.");
                    }

                    if (bossHp <= 0) break;

                    if (turn % 3 == 0)
                    {
                        bossHp += 10;
                        Console.WriteLine("Босс восстановил 10 HP!");
                    }

                    if (rnd.Next(0, 3) == 0)
                    {
                        int dmg1 = rnd.Next(15, 26);
                        int dmg2 = rnd.Next(15, 26);
                        hp -= dmg1 + dmg2;
                        Console.WriteLine($"Босс наносит ДВОЙНОЙ УДАР: {dmg1} + {dmg2} = {dmg1 + dmg2} урона!");
                    }
                    else
                    {
                        int dmg = rnd.Next(15, 26);
                        hp -= dmg;
                        Console.WriteLine($"Босс атакует вас на {dmg} урона.");
                    }
                }

                if (bossHp <= 0 && hp > 0)
                    EndGame(true);
                else
                    EndGame(false);
            }

            // 12. Загадка
            static void SolveRiddle()
            {
                int a = rnd.Next(1, 20);
                int b = rnd.Next(1, 20);
                int answer = a + b;

                Console.WriteLine($"Решите загадку: сколько будет {a} + {b}?");
                Console.Write("Ваш ответ: ");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int playerAnswer) && playerAnswer == answer)
                {
                    int reward = rnd.Next(5, 16);
                    gold += reward;
                    Console.WriteLine($"Правильно! Вы получили {reward} золота.");
                }
                else
                {
                    int dmg = rnd.Next(5, 11);
                    hp -= dmg;
                    Console.WriteLine($"Неправильно! Ловушка сработала, вы потеряли {dmg} HP.");
                }
            }

            // 13. Завершение игры
            static void EndGame(bool isWin)
            {
                Console.WriteLine();
                if (isWin)
                {
                    Console.WriteLine("ПОБЕДА! Вы прошли подземелье!");
                    Console.WriteLine("Финальные характеристики:");
                }
                else
                {
                    Console.WriteLine("ВЫ ПОГИБЛИ...");
                    Console.WriteLine("Ваши характеристики на момент смерти:");
                }
                ShowStats();
            }

            static void Main(string[] args)
            {
                StartGame();
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

    }
