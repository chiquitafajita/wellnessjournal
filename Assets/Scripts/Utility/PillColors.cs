using UnityEngine;

public static class PillColors{
    public static Color GetColor(int code){

        Color color = new Color(0, 0, 0);

        switch(code){
            // white
            case 0:
                color = CreateColor(0xFF, 0xFF, 0xFF);
                break;
            // gray
            case 1:
                color = CreateColor(0x80, 0x80, 0x80);
                break;
            // beige
            case 2:
                color = CreateColor(0xDD, 0x79, 0x52);
                break;
            // brown
            case 3:
                color = CreateColor(0xA9, 0x40, 0x18);
                break;
            // black
            case 4:
                color = CreateColor(0x20, 0x20, 0x20);
                break;
            // pink
            case 5:
                color = CreateColor(0xFF, 0x80, 0x80);
                break;
            // red
            case 6:
                color = CreateColor(0xFF, 0x00, 0x00);
                break;
            // orange
            case 7:
                color = CreateColor(0xFF, 0x80, 0x00);
                break;
            // yellow
            case 8:
                color = CreateColor(0xFF, 0xFF, 0x00);
                break;
            // green
            case 9:
                color = CreateColor(0x00, 0xB9, 0x00);
                break;
            // blue
            case 10:
                color = CreateColor(0x40, 0x40, 0xFF);
                break;
            // purple
            case 11:
                color = CreateColor(0x80, 0x00, 0x80);
                break;
        }

        return color;

    }

    public static Color CreateColor(int r, int g, int b){

        return new Color((float)r / 0xFF, (float)g / 0xFF, (float)b / 0xFF, 1);

    }
}