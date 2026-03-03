using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    internal class Program
    {
            static int hp;
            static int maxHp;
            static int gold;
            static int potions;
            static int arrows;
            static int swordMin;
            static int swordMax;
            static int currentRoom;
            static int monsterHP;
            static int monsterAttack;
            static bool gameOver;

            
            static Random rnd = new Random();

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
                currentRoom = 0;
                gameOver = false;
                Console.WriteLine("Числовой квест DnD");
                Console.WriteLine("");
                Console.WriteLine("Таверна осталась позади. Факел трещит в руке. Вы спускаетесь в подземелье...Пусть фортуна улыбнется вам в странствиях!!");
            }

            // 2. Запуск игры
            static void StartGame()
            {
                InitializeGame();

                while (currentRoom < 15 && gameOver == false)
                {
                    currentRoom = currentRoom + 1;

                    if (currentRoom == 15)
                    {
                        Console.WriteLine("Комната 15: Тронный зал подземелья");


                        FightBoss();
                    }
                    else
                    {
                        ProcessRoom();
                    }
                }
            }

            // 3. Обработка комнаты
            static void ProcessRoom()
            {
                Console.WriteLine($"Комната {currentRoom}");
                ShowStats();

                int eventType = rnd.Next(1, 9);

                if (eventType == 1 || eventType == 2)
                {
                    monsterHP = rnd.Next(20, 51);
                    monsterAttack = rnd.Next(5, 16);

                    string[] names = { "Гоблин-разведчик", "Скелет-воин", "Болотная крыса", "Тень из стены", "Одичалый кобольд" };
                    string name = names[rnd.Next(names.Length)];

                    Console.WriteLine($"Из темноты выходит {name}! (HP: {monsterHP}, Атака: {monsterAttack})");
                    FightMonster();
                }
                else if (eventType == 3)
                {
                    string[] traps = {
                    "Пол уходит из-под ног - ядовитые шипы!",
                    "С потолка падает камень!",
                    "Вы задели растяжку - дротик летит в вашу сторону!"
                };
                    Console.WriteLine(traps[rnd.Next(traps.Length)]);
                    int damage = rnd.Next(5, 21);
                    hp = hp - damage;
                    Console.WriteLine($"Вы получили {damage} урона.");
                }
                else if (eventType == 4)
                {
                    Console.WriteLine("В углу комнаты стоит старый сундук, покрытый пылью...");
                    OpenChest();
                }
                else if (eventType == 5)
                {
                    Console.WriteLine("У стены сидит бродячий торговец. Он кивает вам и раскладывает товар.");
                    VisitMerchant();
                }
                else if (eventType == 6)
                {
                    Console.WriteLine("Посреди комнаты стоит каменный алтарь. От него исходит тёплый свет.");
                    VisitAltar();
                }
                else if (eventType == 7)
                {
                    Console.WriteLine("Воздух холодеет. Из тени выступает фигура в чёрном балахоне...");
                    MeetDarkMage();
                }
                else if (eventType == 8)
                {
                    Console.WriteLine("На стене высечены древние руны. Кажется, это загадка...");
                    SolveRiddle();
                }

                if (hp <= 0)
                {
                    gameOver = true;
                    EndGameLose();
                }
            }

            // 4. Бой с монстром
            static void FightMonster()
            {
                while (monsterHP > 0 && hp > 0)
                {
                    Console.WriteLine($"Враг HP: {monsterHP} | Ваше HP: {hp}");
                    Console.WriteLine("1 - Взмах мечом");
                    Console.WriteLine("2 - Выстрел из лука (стрел: " + arrows + ")");
                    Console.WriteLine("3 - Глотнуть зелье (зелий: " + potions + ")");
                    Console.Write("Ваш ход: ");

                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        int dmg = rnd.Next(swordMin, swordMax + 1);
                        monsterHP = monsterHP - dmg;
                        Console.WriteLine($"Сталь свистнула в воздухе! Вы нанесли {dmg} урона.");
                    }
                    else if (input == "2")
                    {
                        if (arrows > 0)
                        {
                            int dmg = rnd.Next(5, 16);
                            monsterHP = monsterHP - dmg;
                            arrows = arrows - 1;
                            Console.WriteLine($"Стрела нашла цель! {dmg} урона.");
                        }
                        else
                        {
                            Console.WriteLine("Колчан пуст! Вы теряете инициативу.");
                        }
                    }
                    else if (input == "3")
                    {
                        UsePotion();
                    }
                    else
                    {
                        Console.WriteLine("Вы замешкались и теряете инициативу.");
                    }

                    if (monsterHP > 0)
                    {
                        int mDmg = rnd.Next(1, monsterAttack + 1);
                        hp = hp - mDmg;
                        Console.WriteLine($"Враг наносит ответный удар! {mDmg} урона.");
                    }
                }

                if (monsterHP <= 0)
                {
                    int reward = rnd.Next(5, 16);
                    gold = gold + reward;
                    Console.WriteLine($"Враг повержен! Вы обыскали тело и нашли {reward} золотых.");
                }
            }

            // 5. Открытие сундука
            static void OpenChest()
            {
                int chance = rnd.Next(0, 3);

                if (chance == 0)
                {
                    Console.WriteLine("Замок щёлкнул... Сундук окутало тёмное пламя! Проклятие!");
                    int g = rnd.Next(5, 16);
                    gold = gold + g;
                    maxHp = maxHp - 10;
                    if (hp > maxHp) hp = maxHp;
                    Console.WriteLine($"Внутри {g} золотых, но проклятие забрало 10 макс. HP.");
                }
                else
                {
                    int loot = rnd.Next(1, 4);
                    if (loot == 1)
                    {
                        int g = rnd.Next(5, 21);
                        gold = gold + g;
                        Console.WriteLine($"Крышка со скрипом открылась. Внутри блестят {g} золотых!");
                    }
                    else if (loot == 2)
                    {
                        potions = potions + 1;
                        Console.WriteLine("Внутри аккуратно лежит склянка с красным зельем. Зелье здоровья!");
                    }
                    else
                    {
                        int a = rnd.Next(2, 6);
                        arrows = arrows + a;
                        Console.WriteLine($"Под тряпками обнаружилась связка из {a} стрел.");
                    }
                }
            }

            // 6. Торговец
            static void VisitMerchant()
            {
                Console.WriteLine($"Ваш кошель: {gold} золотых");
                Console.WriteLine("1 - Зелье здоровья (10 золотых)");
                Console.WriteLine("2 - Связка из 3 стрел (5 золотых)");
                Console.WriteLine("3 - Пройти мимо");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    if (gold >= 10)
                    {
                        gold = gold - 10;
                        potions = potions + 1;
                        Console.WriteLine("Торговец протягивает вам склянку. Сделка заключена.");
                    }
                    else
                        Console.WriteLine("Торговец качает головой - монет не хватает.");
                }
                else if (input == "2")
                {
                    if (gold >= 5)
                    {
                        gold = gold - 5;
                        arrows = arrows + 3;
                        Console.WriteLine("Торговец отсчитывает три стрелы. Наконечники острые.");
                    }
                    else
                        Console.WriteLine("Торговец качает головой - монет не хватает.");
                }
                else
                {
                    Console.WriteLine("Вы киваете торговцу и идёте дальше.");
                }
            }

            // 7. Алтарь усиления
            static void VisitAltar()
            {
                if (gold < 10)
                {
                    Console.WriteLine("Алтарь молчит. У вас нет 10 золотых для подношения.");
                    return;
                }

                Console.WriteLine("Положить 10 золотых на алтарь?");
                Console.WriteLine("1 - Просить силу (урон меча +5)");
                Console.WriteLine("2 - Просить исцеление (+20 HP)");
                Console.WriteLine("3 - Отойти");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    gold = gold - 10;
                    swordMin = swordMin + 5;
                    swordMax = swordMax + 5;
                    Console.WriteLine("Алтарь засиял. Ваш клинок покрылся лёгким свечением. Урон +5.");
                }
                else if (input == "2")
                {
                    gold = gold - 10;
                    hp = hp + 20;
                    if (hp > maxHp) hp = maxHp;
                    Console.WriteLine("Тёплая волна прошла по телу. Раны затянулись. +20 HP.");
                }
                else
                {
                    Console.WriteLine("Вы убираете руку и уходите. Свет гаснет.");
                }
            }

            // 8. Тёмный маг
            static void MeetDarkMage()
            {
                if (hp <= 10)
                {
                    Console.WriteLine("Маг окидывает вас взглядом и исчезает. Вы слишком слабы для сделки.");
                    return;
                }

                Console.WriteLine("Маг шепчет: Отдай каплю жизни - 10 HP. Взамен получишь 2 зелья и 5 стрел.");
                Console.WriteLine("1 - Протянуть руку");
                Console.WriteLine("2 - Отступить");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    hp = hp - 10;
                    potions = potions + 2;
                    arrows = arrows + 5;
                    Console.WriteLine("Холод пробежал по руке. Сделка заключена. 2 зелья и 5 стрел ваши.");
                }
                else
                {
                    Console.WriteLine("Маг усмехнулся и растворился в темноте.");
                }
            }

            // 9. Использование зелья
            static void UsePotion()
            {
                if (potions > 0)
                {
                    potions = potions - 1;
                    hp = hp + 30;
                    if (hp > maxHp) hp = maxHp;
                    Console.WriteLine("Вы опрокинули склянку. Тепло разлилось по телу. +30 HP.");
                }
                else
                {
                    Console.WriteLine("Зелий не осталось!");
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

                Console.WriteLine("Земля дрожит. Перед вами встаёт Тёмный Страж - хранитель подземелья.");
                Console.WriteLine("HP: 100 | Атака: 15-25");

                while (bossHp > 0 && hp > 0)
                {
                    turn = turn + 1;
                    Console.WriteLine($"Раунд {turn} | Страж HP: {bossHp} | Ваше HP: {hp}");
                    Console.WriteLine("1 - Взмах мечом");
                    Console.WriteLine("2 - Выстрел из лука (стрел: " + arrows + ")");
                    Console.WriteLine("3 - Глотнуть зелье (зелий: " + potions + ")");
                    Console.Write("Ваш ход: ");

                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        int dmg = rnd.Next(swordMin, swordMax + 1);
                        bossHp = bossHp - dmg;
                        Console.WriteLine($"Клинок рассекает воздух! {dmg} урона.");
                    }
                    else if (input == "2")
                    {
                        if (arrows > 0)
                        {
                            int dmg = rnd.Next(5, 16);
                            bossHp = bossHp - dmg;
                            arrows = arrows - 1;
                            Console.WriteLine($"Стрела вонзается в броню! {dmg} урона.");
                        }
                        else
                        {
                            Console.WriteLine("Колчан пуст! Вы теряете инициативу.");
                        }
                    }
                    else if (input == "3")
                    {
                        UsePotion();
                    }
                    else
                    {
                        Console.WriteLine("Вы замешкались и теряете инициативу.");
                    }

                    if (bossHp <= 0) break;

                    if (turn % 3 == 0)
                    {
                        bossHp = bossHp + 10;
                        Console.WriteLine("Тёмная энергия окутала Стража. Он восстановил 10 HP!");
                    }

                    if (rnd.Next(0, 3) == 0)
                    {
                        int dmg1 = rnd.Next(15, 26);
                        int dmg2 = rnd.Next(15, 26);
                        hp = hp - (dmg1 + dmg2);
                        Console.WriteLine($"Страж обрушивает двойной удар! {dmg1} + {dmg2} = {dmg1 + dmg2} урона!");
                    }
                    else
                    {
                        int dmg = rnd.Next(15, 26);
                        hp = hp - dmg;
                        Console.WriteLine($"Тяжёлый кулак Стража обрушивается на вас. {dmg} урона.");
                    }
                }

                if (bossHp <= 0 && hp > 0)
                    EndGameWin();
                else
                    EndGameLose();

                gameOver = true;
            }

            // 12. Загадка
            static void SolveRiddle()
            {
                int a = rnd.Next(1, 20);
                int b = rnd.Next(1, 20);
                int answer = a + b;

                Console.WriteLine($"Руны складываются в вопрос: сколько будет {a} + {b}?");
                Console.Write("Ваш ответ: ");

                string input = Console.ReadLine();
                int playerAnswer = Convert.ToInt32(input);

                if (playerAnswer == answer)
                {
                    int reward = rnd.Next(5, 16);
                    gold = gold + reward;
                    Console.WriteLine($"Руны засветились зелёным. Стена раздвинулась - за ней {reward} золотых!");
                }
                else
                {
                    int dmg = rnd.Next(5, 11);
                    hp = hp - dmg;
                    Console.WriteLine($"Руны вспыхнули красным! Ловушка сработала. {dmg} урона.");
                }
            }

            // 13. Конец игры - победа
            static void EndGameWin()
            {
                Console.WriteLine();
                Console.WriteLine("Тёмный Страж рушится на колени и рассыпается в прах.");
                Console.WriteLine("Свет пробивается сквозь трещины в потолке. Вы прошли подземелье!");
                Console.WriteLine("Финальные характеристики:");
                ShowStats();
            }

            // 14. Конец игры - поражение
            static void EndGameLose()
            {
                Console.WriteLine();
                Console.WriteLine("Тьма поглотила вас. Ваше приключение окончено...");
                Console.WriteLine("Характеристики на момент гибели:");
            
                ShowStats();
            }

            static void Main(string[] args)
            {
                StartGame();
            }
        }
    }
