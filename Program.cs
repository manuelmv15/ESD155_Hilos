using System;
using System.Collections.Generic;
using System.Threading;

internal class Program
{
    static void Main(string[] args)
    {
        Console.Write("Ingrese la cantidad de clientes: ");
        int cantidadClientes = int.Parse(Console.ReadLine());

        Barberia barberia = new Barberia(4); // Crear barbería con 4 barberos


        for (int i = 0; i < 4; i++)
        {
            Thread barbero = new Thread(barberia.AtenderClientes);
            barbero.Start();
        }


        for (int i = 1; i <= cantidadClientes; i++)
        {
            Thread.Sleep(500); // Tiempo entre llegadas de clientes
            Cliente cliente = new Cliente($"Cliente {i}");
            barberia.AgregarCliente(cliente);
        }


        barberia.EsperarATerminar();


        Console.WriteLine("Todos los clientes han sido atendidos. La barbería está cerrando.");
        Console.ReadLine();
    }
}


class Cliente
{
    public string Nombre { get; set; }

    public Cliente(string nombre)
    {
        Nombre = nombre;
    }
}

class Barberia
{
    private Cola<Cliente> colaClientes = new Cola<Cliente>();
    private List<Thread> barberos = new List<Thread>();
    private object lockObject = new object();
    private bool running = true;
    private int clientesAtendidos = 0;
    private int totalClientes = 0;

    public Barberia(int cantidadBarberos)
    {
        for (int i = 0; i < cantidadBarberos; i++)
        {
            barberos.Add(new Thread(AtenderClientes));
        }
    }

    public void AgregarCliente(Cliente cliente)
    {
        lock (lockObject)
        {
            colaClientes.Enqueue(cliente);
            totalClientes++; // Aumentar el total de clientes
            Console.WriteLine($"{cliente.Nombre} ha llegado y se ha puesto en la cola.");
        }
    }

    public void AtenderClientes()
    {
        while (running)
        {
            Cliente cliente = null;

            lock (lockObject)
            {
                if (!colaClientes.EstaVacia())
                {
                    cliente = colaClientes.Desencolar();
                    Console.WriteLine($"El barbero {Thread.CurrentThread.ManagedThreadId} está atendiendo a {cliente.Nombre}.");
                    clientesAtendidos++;
                }
            }

            if (cliente != null)
            {
                // Simular tiempo de atención
                Thread.Sleep(2000); // 2 segundos
                Console.WriteLine($"{cliente.Nombre} ha terminado su corte de cabello.");
            }
            else
            {
                // Si no hay clientes, el barbero espera
                Console.WriteLine($"Barbero {Thread.CurrentThread.ManagedThreadId}: No hay clientes en la cola. Estoy esperando.");
                Thread.Sleep(1000); // 1 segundo
            }

            // Terminar si se han atendido todos los clientes
            lock (lockObject)
            {
                if (clientesAtendidos == totalClientes && colaClientes.EstaVacia())
                {
                    running = false; // Detener todos los barberos
                }
            }
        }
    }

    public void EsperarATerminar()
    {
        foreach (Thread barbero in barberos)
        {
            barbero.Join(); // Esperar a que cada barbero termine
        }
    }
}
