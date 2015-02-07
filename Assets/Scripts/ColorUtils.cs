using UnityEngine;
using System.Collections;

public class ColorUtils {

    public static Vector3 RGB2HSL(Color color) {
        float[] colorHSL = {0, 0, 0};
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float[] rgbs = {r, g, b};
        float min = Mathf.Min(rgbs);
        float max = Mathf.Max(rgbs);
        
        colorHSL[2] = (max + min)/2;
        
        if (max != min) {
            if (colorHSL[2] > 0.5) {
                colorHSL[1] = (max-min)/(2-max-min);
            }
            else {
                colorHSL[1] = (max-min)/(max+min);
            }
        }
        else {
            colorHSL[1] = 0;
        }

        if (colorHSL[1] > 0) {
            if (max == r) {
                colorHSL[0] = (g-b)/(max-min);
            } else if (max == g) {
                colorHSL[0] = 2 + (b-r)/(max-min);
            } else {
                colorHSL[0] = 4 + (r-g)/(max-min);
            }
        }

        colorHSL[0] *= 60;
        colorHSL[0] = (colorHSL[0] + 360) % 360;

        return new Vector3(colorHSL[0], colorHSL[1], colorHSL[2]);
    }

    public static Vector3 HSL2RGB(Vector3 hsl) {
        float h = hsl.x;
        float s = hsl.y;
        float l = hsl.z;

        if (s == 0) {
            return new Vector3(l, l, l);
        }

        float t1;

        if (l < 0.5f) {
            t1 = l*(1+s);
        } else {
            t1 = l+s-l*s;
        }

        float t2 = 2*l - t1;

        h /= 360;
        float tr = (h + 0.33333f) % 1;
        float tg = h;
        float tb = (h + 0.66666f) % 1;

        float r;
        float g;
        float b;

        if (6 * tr < 1) {
            r = t2 + (t1-t2)*6*tr;
        } else if (2 * tr < 1) {
            r = t1;
        } else if (3 * tr < 2) {
            r = t2 + (t1-t2)*(0.66666f - tr)*6;
        } else {
            r = t2;
        }

        if (6 * tg < 1) {
            g = t2 + (t1-t2)*6*tg;
        } else if (2 * tg < 1) {
            g = t1;
        } else if (3 * tg < 2) {
            g = t2 + (t1-t2)*(0.66666f - tg)*6;
        } else {
            g = t2;
        }

        if (6 * tb < 1) {
            b = t2 + (t1-t2)*6*tb;
        } else if (2 * tb < 1) {
            b = t1;
        } else if (3 * tb < 2) {
            b = t2 + (t1-t2)*(0.66666f - tb)*6;
        } else {
            b = t2;
        }

        return new Vector3(r, g, b);
    }

    public static Color lightenColor(Color c, float amount) {
        Vector3 hsl = RGB2HSL(c);

        Vector3 newHsl = new Vector3(hsl[0], hsl[1], Mathf.Clamp01(hsl[2] + amount));

        Vector3 newRgb = HSL2RGB(newHsl);

        return new Color(newRgb.x, newRgb.y, newRgb.z, 1);
    }
}
