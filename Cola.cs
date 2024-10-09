namespace Hilo;


class Cola<T>
{
    public Nodo<T> primero;
    public Nodo<T> ultimo;

    public Cola()
    {
        primero = null;
        ultimo = null;
    }

    public void Enqueue(T valor)
    {
        Nodo<T> nuevoNodo = new Nodo<T>
        {
            info = valor,
            siguiente = null
        };

        if (primero == null)
        {
            primero = nuevoNodo;
            ultimo = nuevoNodo;
        }
        else
        {
            ultimo.siguiente = nuevoNodo;
            ultimo = nuevoNodo;
        }
    }

    public T Desencolar()
    {
        if (primero == null)
        {
            Console.WriteLine("Cola vacía");
            return default(T);
        }
        else
        {
            T valor = primero.info;
            primero = primero.siguiente;

            if (primero == null)
            {
                ultimo = null;
            }
            return valor;
        }
    }

    public void Mostrar()
    {
        if (primero == null)
        {
            Console.WriteLine("Cola vacía");
        }
        else
        {
            Nodo<T> puntero = primero;
            while (puntero != null)
            {
                Console.WriteLine($"{puntero.info}");
                puntero = puntero.siguiente;
            }
        }
    }

    public bool EstaVacia()
    {
        return primero == null;
    }
}
