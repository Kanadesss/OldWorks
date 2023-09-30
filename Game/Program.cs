using System;

namespace Game {

  public class Queue<T> {
    private int size = 10;
    private int last;
    private T[] array;

    public Queue(int size) {
      this.size = size;
      array = new T[size];
      last = 0;
    }

    public bool Enqueue(T value) {
      if (last < size) {
        array[last] = value;
        last++;
        return true;
      } else {
        return false;
      }
    }

    public T Dequeue() {
      if (last >= 0) {
        T valToReturn = array[0];
        --last;
        for (int i = 0; i < last; i++) {
          array[i] = array[i + 1];
        }
        return valToReturn;
      } else {
        throw new InvalidOperationException();
      }
    }

    public bool IsEmpty() {
      if (last == 0) {
        return true;
      } else {
        return false;
      }
    }

    public T ShowCurPosition(int posToShow) {
      if (posToShow >= last) {
        throw new InvalidOperationException();
      } else {
        return array[posToShow];
      }
    }

    public int GetLastPosNumber() {
      return last;
    }
  }

  public class Map {
    protected void ShowRoof(ConsoleColor color) {
      Console.ForegroundColor = color;
      Console.Write(" __ ");
      Console.ResetColor();
    }

    protected void ShowWalls(ConsoleColor color) {
      Console.ForegroundColor = color;
      Console.Write("|  |");
      Console.ResetColor();
    }

    protected void ShowDistance(int distance) {
      for (int i = 0; i < distance; i++) {
        Console.Write(" ");
      }
    }
  }

  public class BattleMap : Map {
    private Queue<Warrior> myWarriors;
    private Queue<Warrior> pcWarriors;
    private string field;

    public void MakeArmies(string inputData) {
      myWarriors = MakeMyWarriorsQueue(inputData);
      pcWarriors = MakePcWarriorsQueue(inputData);
    }

    public void Fight() {
      field = "!!!!!!!!!!!!!!!!!!!!";                              //при обработке размер field увеличивается до 40 и удаляет первые 20
      ShowBattleMap();                                             
      while (!myWarriors.IsEmpty() && !pcWarriors.IsEmpty()) {
        for (int i = 0; i < myWarriors.GetLastPosNumber(); i++) {  //сколько бойцов в армии
          AtackProcess(myWarriors.ShowCurPosition(i), pcWarriors.ShowCurPosition(0));
          if (IsDestroyed(pcWarriors.ShowCurPosition(0))) {
            DeleteWarrior(pcWarriors);
            if (pcWarriors.IsEmpty()) {
              break;
            }
          }
        }
        if (!pcWarriors.IsEmpty()) {
          for (int i = 0; i < pcWarriors.GetLastPosNumber(); i++) {
            AtackProcess(pcWarriors.ShowCurPosition(i), myWarriors.ShowCurPosition(0));
            if (IsDestroyed(myWarriors.ShowCurPosition(0))) {
              DeleteWarrior(myWarriors);
              if (myWarriors.IsEmpty()) {
                break;
              }
            }
          }
        }
        if (!myWarriors.IsEmpty() && !pcWarriors.IsEmpty()) {
          myWarriors = Move(myWarriors, pcWarriors);
          ShowBattleMap();
        }
      }
      ShowWinner();
    }

    private void AtackProcess(Warrior warriorAtack, Warrior warriorDef) {
      if (Warrior.AtackDistanceProvide(warriorAtack, warriorDef)) {
        warriorDef.SetHealth(Warrior.AtackProvide(warriorAtack, warriorDef));
      }
    }

    private Queue<Warrior> MakeQueueFromString(string inputData, bool isPcQueue) {
      int length = inputData.Length;
      Queue<Warrior> queue = new Queue<Warrior>(length);
      if (isPcQueue) {
        length = 20 - length;
      }
      foreach (var letter in inputData) {
        switch (letter) {
          case 'k':
            queue.Enqueue(new Knight(length));
            break;
          case 'w':
            queue.Enqueue(new Wizard(length));
            break;
          case 's':
            queue.Enqueue(new Sniper(length));
            break;
        }
        length = isPcQueue ? ++length : --length;
      }
      return queue;
    }

    private Queue<Warrior> MakeMyWarriorsQueue(string inputData) {
      bool isPcQueue = false;
      return MakeQueueFromString(inputData, isPcQueue);
    }

    private Queue<Warrior> MakePcWarriorsQueue(string inputData) {
      bool isPcQueue = true;
      return MakeQueueFromString(MakeRandomString(inputData.Length + 2), isPcQueue);
    }

    private string MakeRandomString(int length) {
      string returnString = "";
      Random random = new Random();
      for (int i = 0; i < length; i++) {
        switch (random.Next(0, 2)) {
          case 0:
            returnString += "k";
            break;
          case 1:
            returnString += "w";
            break;
          case 2:
            returnString += "s";
            break;
        }
      }
      Console.WriteLine(returnString);
      return returnString;
    }

    private byte ShowBattleMap() {          // отрисовка всех объектов на карте                  
      ShowBases();
      ShowArmies();
      return 0;
    }

    private void ShowBases() {
      Console.Write("\n");
      ShowRoof(ConsoleColor.Blue);
      ShowDistance(20);
      ShowRoof(ConsoleColor.Red);
      Console.Write("\n");
      ShowWalls(ConsoleColor.Blue);
      ShowDistance(20);
      ShowWalls(ConsoleColor.Red);
      Console.Write("\n");
    }

    private byte FillField(Queue<Warrior> queue) {          // конвертация отряда из очереди в строку
      int size = queue.GetLastPosNumber();
      string newField = "";
      for (int i = 0; i < size; i++) {
        var curWarrior = queue.ShowCurPosition(i);
        switch (curWarrior.GetDamageDistance()) {
          case 0:
            newField += "k";
            break;
          case 2:
            newField += "w";
            break;
          default:
            newField += "s";
            break;
        }
      }
      Console.Write(newField);
      field += newField;
      return (byte)size;
    }         

    private void ShowArmies() {         // отрисовка отряда на поле
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.Write("|  |");
      int emptyFieldLength = myWarriors.ShowCurPosition(myWarriors.GetLastPosNumber() - 1).GetPosition() - 1;  // расстояние от базы  
      PrintDistance(emptyFieldLength);                                                                         // до ближайшего воина
      int firstDistance = emptyFieldLength;
      if (!myWarriors.IsEmpty()) {          // моя армия жива
        Console.ForegroundColor = ConsoleColor.Blue;
        emptyFieldLength = FillField(myWarriors) + firstDistance;
        Console.ForegroundColor = ConsoleColor.White;
        int pcDistance = pcWarriors.ShowCurPosition(0).GetPosition();         // расстояние до ближайшего воина компьютера
        if (pcDistance - emptyFieldLength > 0) {          // проверки расстояния между армиями
          PrintDistance(pcDistance - emptyFieldLength);
        }
        if (!pcWarriors.IsEmpty()) {          //жива ли армия противника
          Console.ForegroundColor = ConsoleColor.Red;
          emptyFieldLength = FillField(pcWarriors);
          pcDistance = pcWarriors.ShowCurPosition(0).GetPosition();
          PrintDistance(20 - emptyFieldLength - pcDistance); 
        }
      } else {          //моя армия мертва
        int pcDistance = pcWarriors.ShowCurPosition(0).GetPosition();
        PrintDistance(pcDistance);
        Console.ForegroundColor = ConsoleColor.Red;
        FillField(pcWarriors);
      }
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write("|  |");
      Console.ForegroundColor = ConsoleColor.White;
      field = field.Substring(20, 20);
    }

    private void PrintDistance(int distance) {          // заполнение пустоты на поле
      Console.ForegroundColor = ConsoleColor.White;
      for (int i = 0; i < distance; i++) {
        Console.Write(".");
        field += ".";
      }
    }

    private bool IsDestroyed(Warrior warrior) {         
      if (warrior.GetHealth() <= 0) {
        return true;
      } else {
        return false;
      }
    }

    private void DeleteWarrior(Queue<Warrior> queue) {
      queue.Dequeue();
    }

    private Queue<Warrior> Move(Queue<Warrior> queueMove, Queue<Warrior> queueStand) {          // перемещение отряда
      int lastWarriorPos = queueMove.GetLastPosNumber();
      int pos1 = queueMove.ShowCurPosition(lastWarriorPos - 1).GetPosition();
      int pos2 = queueStand.ShowCurPosition(0).GetPosition();
      if (Math.Abs(pos1 - pos2) > 0) {          // расстояние между отрядами
        for (int i = 0; i < lastWarriorPos; i++) {          // обновление позиций бойцов
          Warrior curWarrior = queueMove.Dequeue();
          curWarrior.SetPosition(curWarrior.GetPosition() + 1);
          queueMove.Enqueue(curWarrior);
        }
      }
      return queueMove;
    }

    private void ShowWinner() {
      if (myWarriors.IsEmpty()) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n\tYOU LOOSE");
      } else {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n\tYOU WIN!!!");
      }
    }
  }

  public class Entity {
    protected int health;
    protected int position;
    protected int damage;
    protected int damageDistance;

    public int GetHealth() {
      return health;
    }

    public void SetHealth(int health) {
      this.health = health;
    }

    public int GetDamage() {
      return damage;
    }

    public void SetDamage(int damage) {
      this.damage = damage;
    }

    public int GetDamageDistance() {
      return damageDistance;
    }

    public int GetPosition() {
      return position;
    }

    public void SetPosition(int position) {
      this.position = position;
    }
  }

  public class Warrior : Entity{
    protected enum AdditionalAbilities : byte {
      IronSkin = 1,
      RevengeOfDead = 2,
      CoolDamage = 3,
      Nothing = 3,
    }

    protected AdditionalAbilities additionalAbility;

    public static int AtackProvide(Warrior warriorAtack, Warrior warriorDef) {
      if (AtackDistanceProvide(warriorAtack, warriorDef)) {
        return warriorDef.GetHealth() - warriorAtack.GetDamage();
      } else {
        return warriorDef.GetHealth();
      }
    }

    public static bool AtackDistanceProvide(Warrior warriorAtack, Warrior warriorDef) {
      int distance = Math.Abs(warriorAtack.GetPosition() - warriorDef.GetPosition());
      if (0 <= distance && distance <= warriorAtack.GetDamageDistance()) {
        return true;
      } else {
        return false;
      }
    }

    protected void SetAddAbility(byte value) {
      if (0 < value && value < 4) {
        additionalAbility = (AdditionalAbilities)value;
      } else {
        additionalAbility = AdditionalAbilities.Nothing;
      }
      if (additionalAbility == AdditionalAbilities.IronSkin) {
        SetHealth(GetHealth() * 2);
      } else if (additionalAbility == AdditionalAbilities.CoolDamage) {
        SetDamage(GetDamage() % 2);
      }
    }

    protected void MakeAddAbility() {
      Random random = new Random();
      SetAddAbility((byte)random.Next(1, 11));
    }
  }

  public class Knight : Warrior {
    public Knight(int position) {
      health = 5;
      this.position = position;
      damage = 1;
      damageDistance = 1;
    }
  }

  public class Wizard : Warrior {
    public Wizard(int position) {
      health = 2;
      this.position = position;
      damage = 2;
      damageDistance = 2;
    }
  }

  public class Sniper : Warrior {
    public Sniper(int position) {
      health = 1;
      this.position = position;
      damage = 3;
      damageDistance = 3;
    }
  }

  class Program {

    static void ShowRules() {
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.WriteLine("ПРАВИЛА:");
      Console.WriteLine("игрок и ИИ делают ход поочередно");
      Console.WriteLine("составьте свой отряд для штурма базы противника");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine("k, w, s - обозначения солдат\nk - рыцарь, w - маг, s - снайпер");
      Console.ForegroundColor = ConsoleColor.Gray;
      Console.WriteLine("размер игрового поля 20");
      Console.ResetColor();
    }

    static string CheckInput() {          // корректность входных данных
      string inData;
      do {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Сформируйте отряд из не более 6 бойцов\nНапример, kwsw");
        inData = Console.ReadLine();
        if (0 < inData.Length && inData.Length <= 6 && CheckInputLetters(inData)) {
          break;
        }
      } while (true);
      return inData;
    }

    static bool CheckInputLetters(string input) {
      foreach (var letter in input) {
        if(letter != 'k' && letter != 'w' && letter != 's') {
          Console.WriteLine("Используйте только символы k, w, s");
          return false;
        }
      }
      return true;
    }

    static void Main() {
      ShowRules();
      string inData = CheckInput();
      BattleMap battleMap = new BattleMap();
      battleMap.MakeArmies(inData);
      battleMap.Fight();
    }
  }
}
