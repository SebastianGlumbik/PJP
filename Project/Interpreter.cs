namespace Project;

public class Interpreter(string code)
{
    private List<string> _code = code.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
    private Stack<object> _stack = new();
    private Dictionary<string, object> _memory = new();
    private Dictionary<int, int> _labels = new();
    
    public void Run()
    {
        for (var i = 0; i < _code.Count; i++)
        {
            var line = _code[i];
            var start = line.Split(' ', 2).First();
            switch (start)
            {
                case "add": Add(); break;
                case "sub": Sub(); break;
                case "mul": Mul(); break;
                case "div": Div(); break;
                case "mod": Mod(); break;
                case "uminus": Uminus(); break;
                case "concat": Concat(); break;
                case "and": And(); break;
                case "or": Or(); break;
                case "gt": Gt(); break;
                case "lt": Lt(); break;
                case "eq": Eq(); break;
                case "not": Not(); break;
                case "itof": Itof(); break;
                case "push": Push(line); break;
                case "pop": Pop(); break;
                case "load": Load(line); break;
                case "save": Save(line); break;
                case "label": Label(i, line); break;
                case "jmp":
                {
                    i = FindLabel(i, line);
                    break;
                }
                case "fjmp":
                {
                    var value = (bool)_stack.Pop();
                    if (value)
                        continue;
                    
                    i = FindLabel(i, line);
                    break;
                }
                case "print": Print(line); break;
                case "read": Read(line); break;
            }
        }
    }

    private void Add()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case int l when right is int r:
                _stack.Push(l + r);
                break;
            case float l when right is float r:
                _stack.Push(l + r);
                break;
        }
    }
    
    private void Sub()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case int l when right is int r:
                _stack.Push(l - r);
                break;
            case float l when right is float r:
                _stack.Push(l - r);
                break;
        }
    }
    
    private void Mul()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case int l when right is int r:
                _stack.Push(l * r);
                break;
            case float l when right is float r:
                _stack.Push(l * r);
                break;
        }
    }
    
    private void Div()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case int l when right is int r:
                _stack.Push(l / r);
                break;
            case float l when right is float r:
                _stack.Push(l / r);
                break;
        }
    }
    
    private void Mod()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        if (left is int l && right is int r)
        {
            _stack.Push(l % r);
        }
    }
    
    private void Uminus()
    {
        var value = _stack.Pop();
        switch (value)
        {
            case int v:
                _stack.Push(-v);
                break;
            case float v:
                _stack.Push(-v);
                break;
        }
    }
    
    private void Concat()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case string l when right is string r:
                _stack.Push(l + r);
                break;
        }
    }
    
    private void And()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case bool l when right is bool r:
                _stack.Push(l && r);
                break;
        }
    }
    
    private void Or()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case bool l when right is bool r:
                _stack.Push(l || r);
                break;
        }
    }
    
    private void Gt()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case int l when right is int r:
                _stack.Push(l > r);
                break;
            case float l when right is float r:
                _stack.Push(l > r);
                break;
        }
    }
    
    private void Lt()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case int l when right is int r:
                _stack.Push(l < r);
                break;
            case float l when right is float r:
                _stack.Push(l < r);
                break;
        }
    }
    
    private void Eq()
    {
        var right = _stack.Pop();
        var left = _stack.Pop();
        
        switch (left)
        {
            case int l when right is int r:
                _stack.Push(l == r);
                break;
            case float l when right is float r:
                _stack.Push(Math.Abs(l - r) < 0.1);
                break;
            case string l when right is string r:
                _stack.Push(l == r);
                break;
            case bool l when right is bool r:
                _stack.Push(l == r);
                break;
        }
    }
    
    private void Not()
    {
        var value = _stack.Pop();
        switch (value)
        {
            case bool v:
                _stack.Push(!v);
                break;
        }
    }
    
    private void Itof()
    {
        var value = _stack.Pop();
        switch (value)
        {
            case int v:
                _stack.Push((float)v);
                break;
        }
    }

    private void Push(string line)
    {
        var type = line.Split(" ").ElementAt(1);
        var value = type == "S" ? line.Split("\"").ElementAt(1) : line.Split(" ").Last();
        _stack.Push(type switch
        {
            "I" => int.Parse(value),
            "F" => float.Parse(value),
            "S" => value,
            "B" => bool.Parse(value),
            _ => throw new Exception("push - unknown type")
        });
    }
    
    private void Pop()
    {
        _stack.Pop();
    }
    
    private void Load(string line)
    {
        var key = line.Split(" ").Last();
        _stack.Push(_memory[key]);
    }
    
    private void Save(string line)
    {
        var value = _stack.Pop();
        var key = line.Split(" ").Last();
        _memory[key] = value;
    }

    private void Label(int number, string line)
    {
        var label = line.Split(" ").Last();
        _labels[int.Parse(label)] = number;
    }
    
    private int FindLabel(int number, string line)
    {
        var destination = int.Parse(line.Split(" ").Last());
        if (_labels.TryGetValue(destination, out var label))
        {
            return --label;
        }
        for (var j = number; j < _code.Count; j++)
        {
            if (!_code[j].StartsWith("label") || !_code[j].EndsWith(destination.ToString())) 
                continue;

            return --j;
        }
        
        return number;
    }
    
    private void Print(string line)
    {
        var count = int.Parse(line.Split(" ").Last());
        var tmp = new Stack<object>();
        for (var i = 0; i < count; i++)
        {
            tmp.Push(_stack.Pop());
        }
        for (var i = 0; i < count; i++)
        {
            var value = tmp.Pop();
            switch (value)
            {
                case bool b:
                    Console.Write(b ? "true" : "false");
                    break;
                case float f:
                    Console.Write(f.ToString(".0###########"));
                    break;
                default:
                    Console.Write(value);
                    break;
            }
        }
        Console.WriteLine();
    }
    
    private void Read(string line)
    {
        var type = line.Split(" ").Last();
        var value = Console.ReadLine();
        if (value == null)
            throw new Exception("read - null value");
        
        _stack.Push(type switch
        {
            "I" => int.Parse(value),
            "F" => float.Parse(value),
            "S" => value,
            "B" => bool.Parse(value),
            _ => throw new Exception("read - unknown type")
        });
    }
}