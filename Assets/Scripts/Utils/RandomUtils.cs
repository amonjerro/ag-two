using System;

public class RandomUtils {
    
    public readonly static Random _rnd = new Random();

    public static T RandomEnumValue<T>(){
        var v = Enum.GetValues(typeof(T));
        return (T) v.GetValue(_rnd.Next(v.Length));
    }
}