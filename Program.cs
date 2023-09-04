using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Engine
    {
        private int I; //Момент инерции двигателя
        private List<double> M; //Крутящий момент
        private List<double> V; //Скорость вращения коленвала
        private int Tover; //Температура перегрева
        private double Hm; //Коэффициент зависимости скорости нагрева от крутящего момента
        private double Hv; //Коэффициент зависимости скорости нагрева от скорости вращения коленвала
        private double C; //Коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды
        private double T; //Температура двигателя
        private int Tenv; //Температура окружающей среды

        public Engine(int I, List<double> M, List<double> V, int Tover, double Hm, double Hv, double C)
        {
            this.I = I;
            this.M = M;
            this.V = V;
            this.Tover = Tover;
            this.Hm = Hm;
            this.Hv = Hv;
            this.C = C;
        }
        public void temperatureTest()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nВведите температуру окружающей среды:");
                    Tenv = Int32.Parse(Console.ReadLine());
                    T = Tenv;
                    break;
                }
                catch (Exception e)
                {
                }
            }
            double timer = 0;//Счетчик времени теста
            double time_unit = 0.1; //Единица времени теста
            int round_time_unit = timeRound(time_unit);//Количество знаков после запятой у единицы времени для округления
            int counter = 0; //Счетчик для смены коэффициента зависимости скорости вращения коленвала от крутящего момента
            double Mtest = M[0]; //Крутящий момент двигателя в момент тестирования
            double Vtest = V[0]; //Скорость вращения коленвала в момент тестирования
            while (T < Tover)
            {
                double coeff = (V[counter + 1] - V[counter]) / (M[counter + 1] - M[counter]); //Коэффициент зависимости скорости вращения коленвала от крутящего момента
                double a = Mtest / I * time_unit;//Ускорение вращения
                Vtest += a;
                Mtest += a / coeff;
                double Vh = Mtest * Hm + Vtest * Vtest * Hv; //Скорость нагрева двигателя
                double Vc = C * (Tenv - T); //Скорость охлаждения двигателя
                T += Vh + Vc;
                timer += time_unit;
                timer = Math.Round(timer, round_time_unit);
                Console.WriteLine("Время: " + timer + " сек.");
                Console.WriteLine("Коэффициент V/M: " + coeff);
                Console.WriteLine("Скорость вращения: " + Vtest + " радиан/сек." + " (стремится к" + V[counter + 1] + ")");
                Console.WriteLine("Момент вращения: " + Mtest + " Н*м" + " (стремится к " + M[counter + 1] + ")");
                Console.WriteLine("Температура двигателя: " + T + "°C");
                Console.WriteLine("\n");
                if (Vtest >= V[counter + 1] && Mtest >= M[counter + 1])
                {
                    if (counter < M.Count - 2 && counter < V.Count - 2)
                    {
                        counter++;

                    }
                }
                if (timer > 600) {
                    Console.WriteLine("\nТестирование длится более 600 секунд. Тест остановлен во избежание зацикливания.\n");
                    break;
                }
            }
            Console.WriteLine("\nВремя работы двигателя: "+timer+" сек.\n");
            Console.WriteLine("Нажмите Enter, чтобы продолжить...");
            Console.ReadLine();
        }
        public void powerTest()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\nВведите температуру окружающей среды:");
                    Tenv = Int32.Parse(Console.ReadLine());
                    T = Tenv;
                    break;
                }
                catch (Exception e)
                {
                }
            }
            double timer = 0;//Счетчик времени теста
            double time_unit = 0.1; //Единица времени теста
            double Nmax = 0;//Максимальная мощность двигателя
            int round_time_unit = timeRound(time_unit);//Количество знаков после запятой у единицы времени для округления
            int counter = 0; //Счетчик для смены коэффициента зависимости скорости вращения коленвала от крутящего момента
            double Mtest = M[0]; //Крутящий момент двигателя в момент тестирования
            double Vtest = V[0]; //Скорость вращения коленвала в момент тестирования
            while (T < Tover) //На случай, если двигатель перегреется раньше, чем сможет достичь максимального крутящего момента
            {
                if (M[counter] < M[counter + 1])
                {
                    double coeff = (V[counter + 1] - V[counter]) / (M[counter + 1] - M[counter]); //Коэффициент зависимости скорости вращения коленвала от крутящего момента
                    double a = Mtest / I * time_unit;//Ускорение вращения
                    Vtest += a;
                    Mtest += a / coeff;
                    double Vh = Mtest * Hm + Vtest * Vtest * Hv; //Скорость нагрева двигателя
                    double Vc = C * (Tenv - T); //Скорость охлаждения двигателя
                    T += Vh + Vc;
                    double N = Mtest * Vtest / 1000; //Мощность двигателя
                    if (N > Nmax) { 
                        Nmax = N;
                    }
                    timer += time_unit;
                    timer = Math.Round(timer, round_time_unit);
                    if (Vtest >= V[counter + 1] && Mtest >= M[counter + 1])
                    {
                        if (counter < M.Count - 2 && counter < V.Count - 2)
                        {
                            counter++;
                        }
                    }
                    Console.WriteLine("Время: " + timer + " сек.");
                    Console.WriteLine("Мощность двигателя: "+N+" кВт");
                    Console.WriteLine("Момент вращения: " + Mtest + " Н*м" + " (стремится к" + M[counter + 1] + ")");
                    Console.WriteLine("Температура двигателя: " + T + "°C");
                    Console.WriteLine("\n");
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Максимально достигнутая мощность двигателя: "+Nmax + " кВт\n");
            Console.WriteLine("Нажмите Enter, чтобы продолжить...");
            Console.ReadLine();
        }
        public int timeRound(double time_unit) 
        {
            int round_time_unit = time_unit.ToString().Length - (time_unit.ToString().IndexOf(",") + 1);
            return round_time_unit;
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                int I = 10;
                List<double> M = new List<double> { 20, 75, 100, 105, 75, 0 };
                List<double> V = new List<double> { 0, 75, 150, 200, 250, 300 };
                int Tover = 110;
                double Hm = 0.01;
                double Hv = 0.0001;
                double C = 0.1;
                Engine engine = new Engine(I, M, V, Tover, Hm, Hv, C);

                while (true) {
                    Console.WriteLine("Выберите тестовый стенд:");
                    Console.WriteLine("1. Тестовый стенд нагрева двигателя");
                    Console.WriteLine("2. Тестовый стенд мощности двигателя");
                    Console.WriteLine("3. Выход");
                    string input = Console.ReadLine();
                    if (input == "1")
                    {
                        engine.temperatureTest();
                    }
                    else if (input == "2")
                    {
                        engine.powerTest();
                    }
                    else if (input == "3")
                    {
                        Environment.Exit(0);
                    }
                    else 
                    {
                    };
                }

            }
        }
    }
}
