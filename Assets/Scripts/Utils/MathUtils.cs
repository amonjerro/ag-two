using System;

class MathUtils{
    public static float ExponentialGradient(float input, int offset, float gradient, int y_intercept){
        return Convert.ToSingle(gradient*Math.Exp(Convert.ToDouble(input)+offset))+y_intercept;
    }
}