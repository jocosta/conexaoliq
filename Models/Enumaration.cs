using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public abstract class Enumeration<T> : IComparable where T : Enumeration<T>, new()
{
    protected int _value;
    protected string _displayName;

    protected Enumeration()
    {
    }

    protected Enumeration(int value)
    {
        _value = value;
        SetDisplayNameFromValue(_value);
    }
    protected Enumeration(int value, string displayName)
    {
        _value = value;
        _displayName = displayName;
    }

    public int Value
    {
        get { return _value; }
        set
        {
            _value = value;

            SetDisplayNameFromValue(_value);

        }
    }

    private void SetDisplayNameFromValue(int value)
    {
        var item = Enumeration<T>.FromValue(value);
        _displayName = item.DisplayName;
    }

    public string DisplayName
    {
        get { return _displayName; }
    }

    public override string ToString()
    {
        return DisplayName;
    }

    public static IEnumerable<T> GetAll()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            var instance = new T();
            var locatedValue = info.GetValue(instance) as T;

            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    public static IEnumerable GetAll(Type type)
    {
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            object instance = Activator.CreateInstance(type);
            yield return info.GetValue(instance);
        }
    }

    public override bool Equals(object obj)
    {
        var otherValue = obj as Enumeration<T>;

        if (otherValue == null)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = _value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static int AbsoluteDifference(Enumeration<T> firstValue, Enumeration<T> secondValue)
    {
        var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
        return absoluteDifference;
    }

    public static T FromValue(int value)
    {
        var matchingItem = parse<int>(value, "value", item => item.Value == value);
        return matchingItem;
    }

    public static T FromDisplayName(string displayName)
    {
        var matchingItem = parse<string>(displayName, "display name", item => item.DisplayName == displayName);
        return matchingItem;
    }

    private static T parse<K>(K value, string description, Func<T, bool> predicate)
    {
        var matchingItem = GetAll().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
            throw new ApplicationException(message);
        }

        return matchingItem;
    }

    public virtual int CompareTo(object other)
    {
        return Value.CompareTo(((Enumeration<T>)other).Value);
    }

    public static bool operator ==(Enumeration<T> current, Enumeration<T> other)
    {

        return current.Value == other?.Value;
    }

    public static bool operator !=(Enumeration<T> current, Enumeration<T> other)
    {

        return current.Value != other?.Value;
    }
}