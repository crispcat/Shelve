# Shelve
Research implementation of reactive calculations with declatative defined functional models.

This is my graduate work focused on learning how to implement hypothetical translator of mathematic expressions with some useful capabilities to work with functional mathematical models.

# Capabilities

- One or more expressions can be assigned to a variable. 
- Variables can be combined into models. 
- Models can be merged into each orher to make variables depend on corresponded expressions in both.
- All expressions are reactive.
- All expressions are lazy, cached and support change propagation.
- All values are 64bit floating point numbers.
- All values are stateless.
- All, but iterators are not.

# Example

Define models like that:

```json
{
  "Name" : "Knight"
  "Expressions" :
  [
    "Power = 5",
    "Damage = 0",
    "CriticalDamage = Power * Damage"
  ]
},

{
  "Name" : "Sword",
  "Uses" : ["Knight"],
  "Expressions" : 
  {
    "Power += 5",
    "Damage += 1 + 1.25 * Power"
  }
}

{
  "Name" : "Shield",
  "Uses" : "Knight",
  "Expressions" :
  [
    "Power -= 5",
  ]
}
```
Use from code:

```csharp
var knight = VariableSet.GetInstance("Knight");
var sword = VariableSet.GetInstance("Sword");
var shield = VariableSet.GetInstance("Shield");

knight.Merge(sword).Merge(shield);
var criticalDamage = knight["CriticalDamage"]; // 36.25
```

# Iterators 

Iterators are statefull values.
```json
"it = [0, it + 1]"
```
Where: 
- 0 - start value
- it + 1 - expression for calculating next value.

Iterator store last calculated value and recalculates on MoveNextValue() call.

# More interesting example

```json
{
  "Name" : "Fibonacci"
  "Expressions":
  {
    "f1 = [0, f2]",
    "f2 = [1, f3]",
    "f3 = [1, t1 + t2]"
  }
}
```

```csharp
var fibonacci = VariableSet.Get("Fibonacci");
Debug.Log(fibonacci["f1"]); // 0
Debug.Log(fibonacci["f2"]); // 1
Debug.Log(fibonacci["f3"]); // 1

var f1 = fibonacci.Get<Iterator>("f1");
var f2 = fibonacci.Get<Iterator>("f2");
var f3 = fibonacci.Get<Iterator>("f3");

f1.MoveNextValue();
f2.MoveNextValue();
f3.MoveNextValue();

Debug.Log(fibonacci["f1"]); // 1
Debug.Log(fibonacci["f2"]); // 1
Debug.Log(fibonacci["f3"]); // 2

f1.MoveNextValue();
f2.MoveNextValue();
f3.MoveNextValue();

Debug.Log(fibonacci["t1"]); // 1
Debug.Log(fibonacci["t2"]); // 2
Debug.Log(fibonacci["t"]);  // 3

f1.MoveNextValue();
f2.MoveNextValue();
f3.MoveNextValue();

Debug.Log(fibonacci["t1"]); // 2
Debug.Log(fibonacci["t2"]); // 3
Debug.Log(fibonacci["t"]);  // 5

// ...
```
